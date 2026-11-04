-- =========================================================
-- TẮC TẮC - Website bán chân gà sả tắc và sốt Thái
-- Chay script nay trong SQL Server Management Studio (SSMS)
-- =========================================================

IF DB_ID('WEBLINH') IS NOT NULL
BEGIN
    ALTER DATABASE WEBLINH SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE WEBLINH;
END
GO

CREATE DATABASE WEBLINH;
GO

USE WEBLINH;
GO

-- =========================================================
-- Bang DanhMuc
-- =========================================================
CREATE TABLE DanhMuc (
    MaDM        INT IDENTITY(1,1) PRIMARY KEY,
    TenDM       NVARCHAR(100) NOT NULL,
    MoTa        NVARCHAR(500) NULL
);
GO

-- =========================================================
-- Bang NguoiDung (user va admin)
-- =========================================================
CREATE TABLE NguoiDung (
    MaND        INT IDENTITY(1,1) PRIMARY KEY,
    TenDN       NVARCHAR(50)  NOT NULL UNIQUE,
    MatKhau     NVARCHAR(200) NOT NULL,
    HoTen       NVARCHAR(100) NOT NULL,
    Email       NVARCHAR(100) NULL,
    SDT         NVARCHAR(20)  NULL,
    DiaChi      NVARCHAR(300) NULL,
    VaiTro      NVARCHAR(20)  NOT NULL DEFAULT 'User',  -- 'Admin' hoac 'User'
    NgayTao     DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

-- =========================================================
-- Bang SanPham
-- =========================================================
CREATE TABLE SanPham (
    MaSP        INT IDENTITY(1,1) PRIMARY KEY,
    TenSP       NVARCHAR(200) NOT NULL,
    MaDM        INT NOT NULL,
    Gia         DECIMAL(18,0) NOT NULL DEFAULT 0,
    SoLuong     INT NOT NULL DEFAULT 0,
    MoTa        NVARCHAR(MAX) NULL,
    Anh         NVARCHAR(300) NULL,
    NgayTao     DATETIME NOT NULL DEFAULT GETDATE(),
    NoiBat      BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_SanPham_DanhMuc FOREIGN KEY (MaDM) REFERENCES DanhMuc(MaDM)
);
GO

-- =========================================================
-- Bang DonHang
-- =========================================================
CREATE TABLE DonHang (
    MaDH        INT IDENTITY(1,1) PRIMARY KEY,
    MaND        INT NOT NULL,
    NgayDat     DATETIME NOT NULL DEFAULT GETDATE(),
    HoTenNhan   NVARCHAR(100) NOT NULL,
    SDTNhan     NVARCHAR(20)  NOT NULL,
    DiaChiGiao  NVARCHAR(300) NOT NULL,
    GhiChu      NVARCHAR(500) NULL,
    TongTien    DECIMAL(18,0) NOT NULL DEFAULT 0,
    TrangThai   NVARCHAR(30)  NOT NULL DEFAULT N'Chờ thanh toán',
    -- 'Chờ thanh toán' | 'Đã chuyển khoản' | 'Đã xác nhận' | 'Đang giao' | 'Hoàn thành' | 'Đã hủy'
    AnhBill     NVARCHAR(300) NULL,
    CONSTRAINT FK_DonHang_NguoiDung FOREIGN KEY (MaND) REFERENCES NguoiDung(MaND)
);
GO

-- =========================================================
-- Bang ChiTietDonHang
-- =========================================================
CREATE TABLE ChiTietDonHang (
    MaDH        INT NOT NULL,
    MaSP        INT NOT NULL,
    SoLuong     INT NOT NULL,
    DonGia      DECIMAL(18,0) NOT NULL,
    CONSTRAINT PK_CTDH PRIMARY KEY (MaDH, MaSP),
    CONSTRAINT FK_CTDH_DonHang FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH) ON DELETE CASCADE,
    CONSTRAINT FK_CTDH_SanPham FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO

-- =========================================================
-- Bang Menu (menu dieu huong navbar)
-- =========================================================
CREATE TABLE Menu (
    MaMenu   INT IDENTITY(1,1) PRIMARY KEY,
    TenMenu  NVARCHAR(100) NOT NULL,
    Url      NVARCHAR(300) NOT NULL,
    ThuTu    INT NOT NULL DEFAULT 0,
    HienThi  BIT NOT NULL DEFAULT 1
);
GO

-- =========================================================
-- Du lieu mau
-- =========================================================

-- Tai khoan admin mac dinh: admin / 123456
INSERT INTO NguoiDung (TenDN, MatKhau, HoTen, Email, SDT, DiaChi, VaiTro)
VALUES (N'admin', N'123456', N'Quản Trị Viên', N'admin@tactac.vn', N'0909123456', N'TP.HCM', N'Admin');

-- Tai khoan user mau: user1 / 123456
INSERT INTO NguoiDung (TenDN, MatKhau, HoTen, Email, SDT, DiaChi, VaiTro)
VALUES (N'user1', N'123456', N'Nguyễn Văn A', N'user1@gmail.com', N'0911111111', N'TP.HCM', N'User');

-- Danh muc san pham
INSERT INTO DanhMuc (TenDM, MoTa) VALUES
(N'Chân gà sả tắc', N'Thơm mùi sả, tắc tươi và lá chanh'),
(N'Chân gà sốt Thái', N'Chua cay đậm đà với sốt Thái nhà làm'),
(N'Chân gà rút xương', N'Dễ ăn, giòn ngon và thấm vị'),
(N'Combo ăn vui', N'Combo nhiều vị cho nhóm bạn và gia đình'),
(N'Món ăn kèm', N'Các món ăn vặt dùng kèm'),
(N'Đồ uống', N'Nước uống thanh mát cân bằng vị cay');

-- San pham mau
INSERT INTO SanPham (TenSP, MaDM, Gia, SoLuong, MoTa, Anh, NoiBat) VALUES
(N'Chân gà sả tắc truyền thống', 1, 79000, 80, N'Chân gà giòn trộn cùng sả non, tắc tươi, lá chanh và nước mắm chua ngọt cân bằng. Phần 500g.', N'changa1.png', 1),
(N'Chân gà sả tắc siêu cay', 1, 85000, 70, N'Phiên bản đậm vị với ớt tươi và sa tế nhà làm, thơm nồng nhưng vẫn giữ vị tắc thanh mát. Phần 500g.', N'changa2.png', 1),
(N'Chân gà sốt Thái chua cay', 2, 89000, 90, N'Sốt Thái sánh quyện, chua cay ngọt hài hòa cùng tắc, ớt và rau thơm. Phần 500g.', N'changa3.png', 1),
(N'Chân gà sốt Thái đặc biệt', 2, 119000, 60, N'Phần đặc biệt nhiều topping, sốt Thái đậm đà và vị cay bùng nổ dành cho hội mê ăn vặt. Phần 700g.', N'changa4.png', 1),
(N'Chân gà rút xương sả tắc', 3, 99000, 65, N'Chân gà rút xương giòn sần sật, dễ ăn, trộn sả tắc thơm tươi. Phần 500g.', N'changa5.png', 0),
(N'Chân gà rút xương sốt Thái', 3, 105000, 65, N'Chân gà rút xương thấm sốt Thái chua cay, thêm tắc và rau thơm cho vị ngon tròn đầy. Phần 500g.', N'changa6.png', 1),
(N'Combo Song Vị', 4, 159000, 50, N'Hai phần chân gà sả tắc và sốt Thái để đổi vị, phù hợp 2-3 người.', N'changa7.png', 1),
(N'Combo Tiệc Vui 4 vị', 4, 299000, 30, N'Đại tiệc gồm sả tắc, sốt Thái, sốt me và bơ tỏi dành cho nhóm 4-6 người.', N'changa8.png', 1),
(N'Chân gà sốt me rang hành', 2, 89000, 55, N'Sốt me chua ngọt sánh mịn, thơm hành phi và mè rang, cuốn vị từ miếng đầu tiên. Phần 500g.', N'changa9.png', 0),
(N'Chân gà bơ tỏi thơm giòn', 2, 95000, 55, N'Bơ tỏi thơm béo phủ đều chân gà giòn, thêm chút ớt cho vị đậm mà không ngấy. Phần 500g.', N'changa10.png', 0),
(N'Xoài non lắc mắm', 5, 45000, 100, N'Xoài non giòn chua lắc mắm đường, ớt và muối tôm, món ăn kèm đúng điệu.', N'changa11.png', 0),
(N'Trà tắc sả nhà làm', 6, 25000, 150, N'Trà tắc thanh mát, thơm sả, vị chua ngọt nhẹ giúp cân bằng món cay.', N'changa12.png', 0);
GO

-- Menu dieu huong (navbar)
INSERT INTO Menu (TenMenu, Url, ThuTu, HienThi) VALUES
(N'Trang chủ',        N'/',                        1, 1),
(N'Tất cả sản phẩm',  N'/User/SanPham/DanhSach',   2, 1),
(N'Giới thiệu',       N'/User/Home/GioiThieu',     3, 1),
(N'Liên hệ',          N'/User/Home/LienHe',        4, 1);
GO

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
INSERT dbo.HomeBanner VALUES (1, N'Chân gà nhà làm · Trộn mới mỗi ngày', N'Giòn sần sật,', N'chua cay đã miệng', N'Sả thơm, tắc tươi, sốt Thái đậm đà quyện cùng chân gà giòn ngon. Một hộp là vui, hai hộp mới đã!', N'hero-changa.png', N'Chân gà sả tắc và sốt Thái TẮC TẮC', N'Đặt món ngay', N'/User/SanPham/DanhSach', 1);
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
INSERT dbo.Menu(TenMenu, Url, ThuTu, HienThi) VALUES (N'Bài viết', N'/User/BaiViet', 5, 1);
GO
PRINT N'Da tao database TẮC TẮC thanh cong!';
GO
