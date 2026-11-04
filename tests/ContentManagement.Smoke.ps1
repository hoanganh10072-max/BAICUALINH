param([string]$BaseUrl = 'http://localhost:57982')
$ErrorActionPreference = 'Stop'
$taskSession = New-Object Microsoft.PowerShell.Commands.WebRequestSession
$taskAnonymous = New-Object Microsoft.PowerShell.Commands.WebRequestSession
[xml]$taskConfig = Get-Content -Raw (Join-Path $PSScriptRoot '..\WEBLINH\Web.config')
$taskConnection = New-Object System.Data.SqlClient.SqlConnection($taskConfig.configuration.connectionStrings.add.connectionString)
$taskConnection.Open()
$taskCommand = $taskConnection.CreateCommand()
$taskCommand.CommandText = 'SELECT ImageName FROM HomeBanner WHERE Id=1'
$taskOriginalImage = [string]$taskCommand.ExecuteScalar()
$taskUploadNames = New-Object 'System.Collections.Generic.List[string]'
function Assert-True($condition, [string]$message) {
    if (-not $condition) { throw $message }
    Write-Output "PASS: $message"
}
function Get-Page([string]$path, $session = $taskSession) {
    Invoke-WebRequest -Uri ($BaseUrl + $path) -WebSession $session -UseBasicParsing -TimeoutSec 30
}
function Input-Value([string]$html, [string]$name) {
    $tag = [regex]::Match($html, '<input\b[^>]*\bname="' + [regex]::Escape($name) + '"[^>]*>').Value
    [System.Net.WebUtility]::HtmlDecode([regex]::Match($tag, '\bvalue="([^"]*)"').Groups[1].Value)
}
function Banner-Fields([string]$html) {
    $fields = @{}
    foreach ($field in @('Eyebrow','Title','Highlight','ImageAlt','ButtonText','ButtonUrl')) { $fields[$field] = Input-Value $html $field }
    $fields.Description = [System.Net.WebUtility]::HtmlDecode([regex]::Match($html, '<textarea\b[^>]*name="Description"[^>]*>([\s\S]*?)</textarea>').Groups[1].Value).TrimStart("`r","`n")
    $fields.Visible = [regex]::IsMatch($html, '<input\b(?=[^>]*name="Visible")(?=[^>]*checked)[^>]*>')
    return $fields
}
function Submit-Fields([string]$path, $fields) {
    $formPage = Get-Page $(if ($path -eq '/Admin/BaiViet/Save') { '/Admin/BaiViet/Create' } else { $path })
    $fields.__RequestVerificationToken = Input-Value $formPage.Content '__RequestVerificationToken'
    Invoke-WebRequest -Uri ($BaseUrl + $path) -Method Post -Body $fields -WebSession $taskSession -UseBasicParsing -TimeoutSec 30
}

foreach ($path in @('/Admin/TrangChu','/Admin/BaiViet','/Admin/Menu')) {
    $p = Get-Page $path $taskAnonymous
    Assert-True ($p.Content -match 'name="matKhau"') "Anonymous blocked from $path"
}
$loginPage = Get-Page '/Admin/Account/DangNhap'
$login = Invoke-WebRequest -Uri ($BaseUrl + '/Admin/Account/DangNhap') -Method Post -WebSession $taskSession -Body @{tenDN='admin';matKhau='123456';__RequestVerificationToken=(Input-Value $loginPage.Content '__RequestVerificationToken')} -UseBasicParsing -TimeoutSec 30
Assert-True ($login.Content -match 'Banner trang chủ') 'Admin login and new navigation links'
foreach ($path in @('/Admin/TrangChu','/Admin/BaiViet','/Admin/BaiViet/Create','/Admin/Menu','/User/BaiViet')) {
    $p = Get-Page $path
    Assert-True ($p.StatusCode -eq 200) "Page renders: $path"
}

$bannerOriginalPage = Get-Page '/Admin/TrangChu'
$bannerOriginal = Banner-Fields $bannerOriginalPage.Content
$originalImage = [regex]::Match($bannerOriginalPage.Content, 'id="banner-preview" src="([^"]+)"').Groups[1].Value
$postId = 0
$menuOriginal = $null
try {
    $bannerChange = $bannerOriginal.Clone()
    $bannerChange.Title = 'CMS smoke banner'
    $bannerChange.Visible = 'true'
    $saved = Submit-Fields '/Admin/TrangChu' $bannerChange
    Assert-True ($saved.Content -match 'CMS smoke banner') 'Banner text saved in admin'
    $p = Get-Page '/User/Home' $taskAnonymous
    Assert-True ($p.Content -match 'CMS smoke banner') 'Banner change visible on homepage'

    $bad = $bannerOriginal.Clone(); $bad.ButtonUrl = 'javascript:alert(1)'
    $rejected = Submit-Fields '/Admin/TrangChu' $bad
    Assert-True ($rejected.Content -match 'field-validation-error|validation-summary-errors') 'Unsafe banner link rejected'

    $upload = $bannerChange.Clone()
    $upload.__RequestVerificationToken = Input-Value (Get-Page '/Admin/TrangChu').Content '__RequestVerificationToken'
    $upload.bannerImage = Get-Item (Join-Path $PSScriptRoot '..\WEBLINH\Content\uploads\hero-changa.png')
    $uploaded = Invoke-WebRequest -Uri ($BaseUrl + '/Admin/TrangChu') -Method Post -Form $upload -WebSession $taskSession -UseBasicParsing -TimeoutSec 30
    $newImage = [regex]::Match($uploaded.Content, 'id="banner-preview" src="([^"]+)"').Groups[1].Value
    Assert-True ($newImage -match '/content_[a-f0-9]+\.png$') 'Banner image upload saved with generated filename'
    $taskUploadNames.Add([IO.Path]::GetFileName($newImage))
    $p = Get-Page $newImage $taskAnonymous
    Assert-True ($p.StatusCode -eq 200) 'Uploaded banner image serves'
    $invalidUpload = $bannerChange.Clone()
    $invalidUpload.__RequestVerificationToken = Input-Value (Get-Page '/Admin/TrangChu').Content '__RequestVerificationToken'
    $invalidUpload.bannerImage = Get-Item (Join-Path $PSScriptRoot '..\WEBLINH\App_Data\ContentManagement.sql')
    $invalidImageResult = Invoke-WebRequest -Uri ($BaseUrl + '/Admin/TrangChu') -Method Post -Form $invalidUpload -WebSession $taskSession -UseBasicParsing -TimeoutSec 30
    Assert-True ($invalidImageResult.Content -match 'validation-summary-errors') 'Non-image upload rejected'

    $hidden = $bannerOriginal.Clone(); $hidden.Visible = 'false'
    $null = Submit-Fields '/Admin/TrangChu' $hidden
    Assert-True ((Get-Page '/User/Home' $taskAnonymous).Content -notmatch '<section class="hero food-hero">') 'Banner hide toggle works'

    $stamp = [Guid]::NewGuid().ToString('N')
    $post = @{Id='0';TieuDe=('CMS smoke ' + $stamp);TomTat='Tóm tắt thử nghiệm';NoiDung="Nội dung tiếng Việt.`nĐoạn thứ hai.";DaDang='false'}
    $created = Submit-Fields '/Admin/BaiViet/Save' $post
    $tag = [regex]::Match($created.Content, '<tr>[^\r\n]*' + $stamp + '[\s\S]*?</tr>').Value
    $postId = [int][regex]::Match($tag, '/Admin/BaiViet/Edit/(\d+)').Groups[1].Value
    Assert-True ($postId -gt 0) 'Draft post created'
    $publicList = Get-Page '/User/BaiViet' $taskAnonymous
    Assert-True ($publicList.Content -notmatch $stamp) 'Draft hidden from public list'
    $draftStatus = Invoke-WebRequest -Uri ($BaseUrl + '/User/BaiViet/ChiTiet/' + $postId) -WebSession $taskAnonymous -SkipHttpErrorCheck -UseBasicParsing -TimeoutSec 30
    Assert-True ($draftStatus.StatusCode -eq 404) 'Draft direct URL returns 404'

    $post.Id = $postId.ToString(); $post.DaDang = 'true'
    $post.__RequestVerificationToken = Input-Value (Get-Page '/Admin/BaiViet/Create').Content '__RequestVerificationToken'
    $post.coverImage = Get-Item (Join-Path $PSScriptRoot '..\WEBLINH\Content\uploads\changa1.png')
    $null = Invoke-WebRequest -Uri ($BaseUrl + '/Admin/BaiViet/Save') -Method Post -Form $post -WebSession $taskSession -UseBasicParsing -TimeoutSec 30
    $published = Get-Page ('/User/BaiViet/ChiTiet/' + $postId) $taskAnonymous
    Assert-True ($published.Content -match $stamp -and $published.Content -match 'content_[a-f0-9]+\.png') 'Article publishes with cover image'
    $taskUploadNames.Add([regex]::Match($published.Content, 'content_[a-f0-9]+\.png').Value)
    Assert-True ((Get-Page '/User/Home' $taskAnonymous).Content -match $stamp) 'Published article appears on homepage'
    Assert-True ((Get-Page '/User/BaiViet' $taskAnonymous).Content -match $stamp) 'Published article appears on public list'

    $post.Remove('coverImage'); $post.DaDang='false'
    $null = Submit-Fields '/Admin/BaiViet/Save' $post
    Assert-True ((Get-Page '/User/BaiViet' $taskAnonymous).Content -notmatch $stamp) 'Unpublished article hidden again'

    $menuPage = Get-Page '/Admin/Menu/Edit/1'
    $menuOriginal = @{MaMenu='1';TenMenu=(Input-Value $menuPage.Content 'TenMenu');Url=(Input-Value $menuPage.Content 'Url');ThuTu=(Input-Value $menuPage.Content 'ThuTu');HienThi=([regex]::IsMatch($menuPage.Content, '<input\b(?=[^>]*name="HienThi")(?=[^>]*checked)[^>]*>')).ToString()}
    $menu = $menuOriginal.Clone(); $menu.TenMenu='CMS smoke menu'
    $null = Submit-Fields '/Admin/Menu/Edit/1' $menu
    Assert-True ((Get-Page '/User/Home' $taskAnonymous).Content -match 'CMS smoke menu') 'Edited menu visible on homepage'
    $menu.HienThi='false'; $null = Submit-Fields '/Admin/Menu/Edit/1' $menu
    Assert-True ((Get-Page '/User/Home' $taskAnonymous).Content -notmatch 'CMS smoke menu') 'Menu visibility toggle works'

    $csrf = Invoke-WebRequest -Uri ($BaseUrl + '/Admin/BaiViet/Delete/' + $postId) -Method Post -WebSession $taskSession -SkipHttpErrorCheck -UseBasicParsing -TimeoutSec 30
    Assert-True ($csrf.StatusCode -ge 400) 'Mutation without anti-forgery token rejected'
}
finally {
    if ($menuOriginal) { $null = Submit-Fields '/Admin/Menu/Edit/1' $menuOriginal }
    $bannerOriginal.Visible = $bannerOriginal.Visible.ToString()
    $null = Submit-Fields '/Admin/TrangChu' $bannerOriginal
    $taskCommand.CommandText = 'UPDATE HomeBanner SET ImageName=@original WHERE Id=1'
    $null = $taskCommand.Parameters.AddWithValue('@original', $taskOriginalImage)
    $null = $taskCommand.ExecuteNonQuery()
    $taskCommand.Parameters.Clear()
    if ($postId -gt 0) {
        $token = Input-Value (Get-Page '/Admin/BaiViet/Create').Content '__RequestVerificationToken'
        $null = Invoke-WebRequest -Uri ($BaseUrl + '/Admin/BaiViet/Delete/' + $postId) -Method Post -WebSession $taskSession -Body @{__RequestVerificationToken=$token} -UseBasicParsing -TimeoutSec 30
        Assert-True ((Get-Page '/Admin/BaiViet').Content -notmatch ('CMS smoke ' + $stamp)) 'Test article removed'
    }
    foreach ($uploadName in $taskUploadNames) {
        if ($uploadName -notmatch '^content_[a-f0-9]+\.png$') { continue }
        $taskCommand.CommandText = 'SELECT (SELECT COUNT(*) FROM HomeBanner WHERE ImageName=@image) + (SELECT COUNT(*) FROM BaiViet WHERE Anh=@image)'
        $null = $taskCommand.Parameters.AddWithValue('@image', $uploadName)
        $referenceCount = [int]$taskCommand.ExecuteScalar()
        $taskCommand.Parameters.Clear()
        if ($referenceCount -eq 0) {
            $taskUploadRoot = (Resolve-Path (Join-Path $PSScriptRoot '..\WEBLINH\Content\uploads')).Path
            $taskUploadPath = [IO.Path]::GetFullPath((Join-Path $taskUploadRoot $uploadName))
            if ([IO.Path]::GetDirectoryName($taskUploadPath) -eq $taskUploadRoot) { Remove-Item -LiteralPath $taskUploadPath -ErrorAction SilentlyContinue }
        }
    }
    $taskCommand.Dispose()
    $taskConnection.Dispose()
}
Write-Output 'CMS smoke checks completed.'
