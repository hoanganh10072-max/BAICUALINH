using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WEBLINH.Models
{
    public class SanPham
    {
        public int MaSP { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string TenSP { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int MaDM { get; set; }

        public string TenDM { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Range(0, 999999999, ErrorMessage = "Giá phải >= 0")]
        [Display(Name = "Giá (VND)")]
        public decimal Gia { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(0, 100000, ErrorMessage = "Số lượng không hợp lệ")]
        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Display(Name = "Ảnh")]
        public string Anh { get; set; }

        public DateTime NgayTao { get; set; }

        [Display(Name = "Nổi bật")]
        public bool NoiBat { get; set; }

        [Display(Name = "Chọn ảnh")]
        public HttpPostedFileBase FileAnh { get; set; }
    }
}
