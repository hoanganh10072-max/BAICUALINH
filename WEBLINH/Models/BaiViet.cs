using System;
using System.ComponentModel.DataAnnotations;

namespace WEBLINH.Models
{
    public class BaiViet
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập tiêu đề bài viết"), StringLength(200), Display(Name = "Tiêu đề")]
        public string TieuDe { get; set; }
        [StringLength(600), Display(Name = "Tóm tắt")]
        public string TomTat { get; set; }
        [Required(ErrorMessage = "Nhập nội dung bài viết"), StringLength(100000), Display(Name = "Nội dung")]
        public string NoiDung { get; set; }
        public string Anh { get; set; }
        [Display(Name = "Xuất bản (bỏ chọn để lưu bản nháp)")]
        public bool DaDang { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayDang { get; set; }
    }
}
