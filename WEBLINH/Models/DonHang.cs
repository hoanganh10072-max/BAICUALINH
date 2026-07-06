using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEBLINH.Models
{
    public class DonHang
    {
        public int MaDH { get; set; }
        public int MaND { get; set; }
        public string TenNguoiDat { get; set; }

        public DateTime NgayDat { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ tên người nhận")]
        public string HoTenNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string SDTNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DiaChiGiao { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal TongTien { get; set; }

        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; }

        [Display(Name = "Ảnh bill chuyển khoản")]
        public string AnhBill { get; set; }

        public List<ChiTietDonHang> ChiTiet { get; set; }
    }
}
