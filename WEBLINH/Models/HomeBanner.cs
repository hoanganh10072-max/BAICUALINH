using System.ComponentModel.DataAnnotations;

namespace WEBLINH.Models
{
    public class HomeBanner
    {
        [Required, StringLength(150), Display(Name = "Dòng giới thiệu")]
        public string Eyebrow { get; set; }
        [Required, StringLength(150), Display(Name = "Tiêu đề chính")]
        public string Title { get; set; }
        [StringLength(150), Display(Name = "Phần chữ nhấn màu cam")]
        public string Highlight { get; set; }
        [Required, StringLength(600), Display(Name = "Mô tả")]
        public string Description { get; set; }
        public string ImageName { get; set; }
        [Required, StringLength(200), Display(Name = "Mô tả ảnh")]
        public string ImageAlt { get; set; }
        [Required, StringLength(80), Display(Name = "Chữ trên nút")]
        public string ButtonText { get; set; }
        [Required, StringLength(300), RegularExpression(@"^/(?!/|\\)[^\s\\]*$", ErrorMessage = "Nhập đường dẫn trong website, bắt đầu bằng /"), Display(Name = "Đường dẫn nút")]
        public string ButtonUrl { get; set; }
        [Display(Name = "Hiển thị banner trên trang chủ")]
        public bool Visible { get; set; }
    }
}
