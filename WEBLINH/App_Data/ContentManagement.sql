USE WEBLINH;
GO
IF OBJECT_ID(N'dbo.HomeBanner', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HomeBanner (
        Id INT NOT NULL PRIMARY KEY CHECK (Id=1),
        Eyebrow NVARCHAR(150) NOT NULL,
        Title NVARCHAR(150) NOT NULL,
        Highlight NVARCHAR(150) NULL,
        Description NVARCHAR(600) NOT NULL,
        ImageName NVARCHAR(300) NOT NULL,
        ImageAlt NVARCHAR(200) NOT NULL,
        ButtonText NVARCHAR(80) NOT NULL,
        ButtonUrl NVARCHAR(300) NOT NULL,
        Visible BIT NOT NULL DEFAULT 1
    );
END;
IF NOT EXISTS (SELECT 1 FROM dbo.HomeBanner WHERE Id=1)
INSERT dbo.HomeBanner VALUES (1, N'Chân gà nhà làm · Trộn mới mỗi ngày', N'Giòn sần sật,', N'chua cay đã miệng', N'Sả thơm, tắc tươi, sốt Thái đậm đà quyện cùng chân gà giòn ngon. Một hộp là vui, hai hộp mới đã!', N'hero-changa.png', N'Chân gà sả tắc và sốt Thái TẮC TẮC', N'Đặt món ngay', N'/User/SanPham/DanhSach', 1);

IF OBJECT_ID(N'dbo.BaiViet', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BaiViet (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TieuDe NVARCHAR(200) NOT NULL,
        TomTat NVARCHAR(600) NULL,
        NoiDung NVARCHAR(MAX) NOT NULL,
        Anh NVARCHAR(300) NULL,
        DaDang BIT NOT NULL DEFAULT 0,
        NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
        NgayDang DATETIME NULL
    );
END;
IF NOT EXISTS (SELECT 1 FROM dbo.Menu WHERE Url=N'/User/BaiViet')
INSERT dbo.Menu(TenMenu, Url, ThuTu, HienThi) VALUES (N'Bài viết', N'/User/BaiViet', 5, 1);
GO
