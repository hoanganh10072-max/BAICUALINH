-- =========================================================
-- WEBLINH - Web ban quan ao tre so sinh
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
VALUES (N'admin', N'123456', N'Quản Trị Viên', N'admin@weblinh.com', N'0900000000', N'Hà Nội', N'Admin');

-- Tai khoan user mau: user1 / 123456
INSERT INTO NguoiDung (TenDN, MatKhau, HoTen, Email, SDT, DiaChi, VaiTro)
VALUES (N'user1', N'123456', N'Nguyễn Văn A', N'user1@gmail.com', N'0911111111', N'Hà Nội', N'User');

-- Danh muc san pham
INSERT INTO DanhMuc (TenDM, MoTa) VALUES
(N'Bodysuit', N'Bodysuit liền thân cho bé sơ sinh'),
(N'Áo sơ sinh', N'Áo cotton mềm mại cho bé'),
(N'Quần sơ sinh', N'Quần thoáng mát dễ mặc'),
(N'Set quần áo', N'Bộ quần áo trọn gói'),
(N'Mũ - Bao tay - Bao chân', N'Phụ kiện giữ ấm cho bé'),
(N'Khăn - Yếm', N'Khăn quấn, yếm ăn dặm');

-- San pham mau
INSERT INTO SanPham (TenSP, MaDM, Gia, SoLuong, MoTa, Anh, NoiBat) VALUES
(N'Bodysuit cotton hình gấu 0-6 tháng', 1, 129000, 50, N'Chất liệu cotton 100%, mềm mại, thoáng mát, an toàn cho da bé.', N'sp1.jpg', 1),
(N'Bodysuit dài tay in họa tiết', 1, 149000, 40, N'Bodysuit dài tay giữ ấm cho bé mùa lạnh.', N'sp2.jpg', 1),
(N'Áo cotton cột dây bo tay', 2, 89000, 100, N'Áo cotton cột dây tiện lợi, không gây khó chịu cho bé.', N'sp3.jpg', 0),
(N'Áo tay dài cài vai', 2, 99000, 80, N'Áo tay dài cài vai dễ mặc, giữ ấm hiệu quả.', N'sp4.jpg', 1),
(N'Quần dài đáy rộng cotton', 3, 79000, 90, N'Quần dài mềm mại, đáy rộng, thoải mái vận động.', N'sp5.jpg', 0),
(N'Quần sơ sinh có bo cổ chân', 3, 69000, 70, N'Quần cotton có bo cổ chân giữ ấm cho bé.', N'sp6.jpg', 0),
(N'Set bộ 5 chi tiết đầy đủ', 4, 350000, 30, N'Bộ trọn gói gồm áo, quần, mũ, bao tay, bao chân.', N'sp7.jpg', 1),
(N'Set 3 bộ bodysuit hình thú', 4, 299000, 25, N'Set 3 bodysuit với nhiều màu sắc dễ thương.', N'sp8.jpg', 1),
(N'Mũ cotton thóp co giãn', 5, 45000, 150, N'Mũ mềm mại bảo vệ thóp bé.', N'sp9.jpg', 0),
(N'Bao tay bao chân sơ sinh combo', 5, 55000, 120, N'Combo bao tay + bao chân giữ ấm.', N'sp10.jpg', 0),
(N'Khăn quấn cotton đa năng', 6, 120000, 60, N'Khăn quấn mềm mại nhiều công dụng.', N'sp11.jpg', 0),
(N'Yếm ăn dặm chống thấm', 6, 39000, 200, N'Yếm chống thấm, dễ vệ sinh.', N'sp12.jpg', 0);
GO

-- Menu dieu huong (navbar)
INSERT INTO Menu (TenMenu, Url, ThuTu, HienThi) VALUES
(N'Trang chủ',        N'/',                        1, 1),
(N'Tất cả sản phẩm',  N'/User/SanPham/DanhSach',   2, 1),
(N'Giới thiệu',       N'/User/Home/GioiThieu',     3, 1),
(N'Liên hệ',          N'/User/Home/LienHe',        4, 1);
GO

PRINT N'Da tao database WEBLINH thanh cong!';
GO
