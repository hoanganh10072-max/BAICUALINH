# Quản trị nội dung TẮC TẮC

Đăng nhập tại http://localhost:57982/Admin/Account/DangNhap.

- **Menu điều hướng**: thêm hoặc sửa tên mục, đường dẫn, thứ tự; bỏ chọn Hiển thị để ẩn. Các thay đổi xuất hiện trên thanh điều hướng trang khách hàng.
- **Banner trang chủ**: tải ảnh JPG/PNG (tối đa 8 MB), sửa tiêu đề, mô tả, chữ trên nút và đường dẫn. Ảnh ngang 16:9, chủ thể bên phải phù hợp bố cục hiện tại. Xem ảnh trước khi lưu; để trống ô tải ảnh để giữ ảnh cũ. Có thể ẩn/hiện banner.
- **Bài viết**: chọn Viết bài mới, nhập tiêu đề, tóm tắt, nội dung và ảnh bìa. Đánh dấu Xuất bản để đăng ngay; bỏ đánh dấu để lưu nháp hoặc gỡ bài đã đăng. Nội dung là văn bản, giữ nguyên các lần xuống dòng.
- Bài đã xuất bản có trang chi tiết tại `/User/BaiViet/ChiTiet/{mã bài}`, xuất hiện tại `/User/BaiViet` và trong 3 bài mới nhất trên trang chủ. Bản nháp không truy cập được bằng URL công khai.
- **Sản phẩm / Danh mục** tiếp tục quản lý thực đơn món ăn. Sản phẩm đánh dấu Nổi bật xuất hiện ở phần món bán chạy.

## Cập nhật cơ sở dữ liệu trên máy khác

Với cơ sở dữ liệu WEBLINH đã có, chạy `WEBLINH/App_Data/ContentManagement.sql` (UTF-8) trong SQL Server. Script chỉ thêm bảng banner, bài viết và mục menu Bài viết còn thiếu; có thể chạy lại. Không chạy lại script tạo mới `WEBLINH_Database.sql` trên cơ sở dữ liệu đang dùng vì script đó xóa dữ liệu cũ.

Với cơ sở dữ liệu tạo mới, `WEBLINH_Database.sql` đã bao gồm các bảng nội dung.

## Kiểm tra chức năng trên local

`pwsh -NoProfile -File tests/ContentManagement.Smoke.ps1`

Kiểm tra quyền admin, menu, banner, tải ảnh, bản nháp, xuất bản, gỡ bài và chống gửi form thiếu token. Script tạo dữ liệu tạm, khôi phục menu/banner và dọn bài/ảnh thử nghiệm sau khi chạy. Dùng tài khoản mẫu admin hiện có.
