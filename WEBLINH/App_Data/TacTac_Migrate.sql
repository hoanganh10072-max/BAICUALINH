USE WEBLINH;
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

UPDATE DanhMuc SET TenDM = N'Chân gà sả tắc', MoTa = N'Thơm mùi sả, tắc tươi và lá chanh' WHERE MaDM = 1;
UPDATE DanhMuc SET TenDM = N'Chân gà sốt Thái', MoTa = N'Chua cay đậm đà với sốt Thái nhà làm' WHERE MaDM = 2;
UPDATE DanhMuc SET TenDM = N'Chân gà rút xương', MoTa = N'Dễ ăn, giòn ngon và thấm vị' WHERE MaDM = 3;
UPDATE DanhMuc SET TenDM = N'Combo ăn vui', MoTa = N'Combo nhiều vị cho nhóm bạn và gia đình' WHERE MaDM = 4;
UPDATE DanhMuc SET TenDM = N'Món ăn kèm', MoTa = N'Các món ăn vặt dùng kèm' WHERE MaDM = 5;
UPDATE DanhMuc SET TenDM = N'Đồ uống', MoTa = N'Nước uống thanh mát cân bằng vị cay' WHERE MaDM = 6;

UPDATE SanPham SET TenSP=N'Chân gà sả tắc truyền thống', MaDM=1, Gia=79000, SoLuong=80, MoTa=N'Chân gà giòn trộn cùng sả non, tắc tươi, lá chanh và nước mắm chua ngọt cân bằng. Phần 500g.', Anh=N'changa1.png', NoiBat=1 WHERE MaSP=1;
UPDATE SanPham SET TenSP=N'Chân gà sả tắc siêu cay', MaDM=1, Gia=85000, SoLuong=70, MoTa=N'Phiên bản đậm vị với ớt tươi và sa tế nhà làm, thơm nồng nhưng vẫn giữ vị tắc thanh mát. Phần 500g.', Anh=N'changa2.png', NoiBat=1 WHERE MaSP=2;
UPDATE SanPham SET TenSP=N'Chân gà sốt Thái chua cay', MaDM=2, Gia=89000, SoLuong=90, MoTa=N'Sốt Thái sánh quyện, chua cay ngọt hài hòa cùng tắc, ớt và rau thơm. Phần 500g.', Anh=N'changa3.png', NoiBat=1 WHERE MaSP=3;
UPDATE SanPham SET TenSP=N'Chân gà sốt Thái đặc biệt', MaDM=2, Gia=119000, SoLuong=60, MoTa=N'Phần đặc biệt nhiều topping, sốt Thái đậm đà và vị cay bùng nổ dành cho hội mê ăn vặt. Phần 700g.', Anh=N'changa4.png', NoiBat=1 WHERE MaSP=4;
UPDATE SanPham SET TenSP=N'Chân gà rút xương sả tắc', MaDM=3, Gia=99000, SoLuong=65, MoTa=N'Chân gà rút xương giòn sần sật, dễ ăn, trộn sả tắc thơm tươi. Phần 500g.', Anh=N'changa5.png', NoiBat=0 WHERE MaSP=5;
UPDATE SanPham SET TenSP=N'Chân gà rút xương sốt Thái', MaDM=3, Gia=105000, SoLuong=65, MoTa=N'Chân gà rút xương thấm sốt Thái chua cay, thêm tắc và rau thơm cho vị ngon tròn đầy. Phần 500g.', Anh=N'changa6.png', NoiBat=1 WHERE MaSP=6;
UPDATE SanPham SET TenSP=N'Combo Song Vị', MaDM=4, Gia=159000, SoLuong=50, MoTa=N'Hai phần chân gà sả tắc và sốt Thái để đổi vị, phù hợp 2-3 người.', Anh=N'changa7.png', NoiBat=1 WHERE MaSP=7;
UPDATE SanPham SET TenSP=N'Combo Tiệc Vui 4 vị', MaDM=4, Gia=299000, SoLuong=30, MoTa=N'Đại tiệc gồm sả tắc, sốt Thái, sốt me và bơ tỏi dành cho nhóm 4-6 người.', Anh=N'changa8.png', NoiBat=1 WHERE MaSP=8;
UPDATE SanPham SET TenSP=N'Chân gà sốt me rang hành', MaDM=2, Gia=89000, SoLuong=55, MoTa=N'Sốt me chua ngọt sánh mịn, thơm hành phi và mè rang, cuốn vị từ miếng đầu tiên. Phần 500g.', Anh=N'changa9.png', NoiBat=0 WHERE MaSP=9;
UPDATE SanPham SET TenSP=N'Chân gà bơ tỏi thơm giòn', MaDM=2, Gia=95000, SoLuong=55, MoTa=N'Bơ tỏi thơm béo phủ đều chân gà giòn, thêm chút ớt cho vị đậm mà không ngấy. Phần 500g.', Anh=N'changa10.png', NoiBat=0 WHERE MaSP=10;
UPDATE SanPham SET TenSP=N'Xoài non lắc mắm', MaDM=5, Gia=45000, SoLuong=100, MoTa=N'Xoài non giòn chua lắc mắm đường, ớt và muối tôm, món ăn kèm đúng điệu.', Anh=N'changa11.png', NoiBat=0 WHERE MaSP=11;
UPDATE SanPham SET TenSP=N'Trà tắc sả nhà làm', MaDM=6, Gia=25000, SoLuong=150, MoTa=N'Trà tắc thanh mát, thơm sả, vị chua ngọt nhẹ giúp cân bằng món cay.', Anh=N'changa12.png', NoiBat=0 WHERE MaSP=12;

UPDATE NguoiDung SET Email=N'admin@tactac.vn', SDT=N'0909123456', DiaChi=N'TP.HCM' WHERE TenDN=N'admin';

COMMIT TRANSACTION;
GO

SELECT MaDM, TenDM FROM DanhMuc ORDER BY MaDM;
SELECT MaSP, TenSP, Gia, Anh FROM SanPham ORDER BY MaSP;
GO
