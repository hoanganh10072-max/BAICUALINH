using System.ComponentModel.DataAnnotations;

namespace WEBLINH.Models
{
    public class Menu
    {
        public int MaMenu { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên menu")]
        [StringLength(100)]
        [Display(Name = "Tên menu")]
        public string TenMenu { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đường dẫn")]
        [StringLength(300)]
        [RegularExpression(@"^/(?!/|\\)[^\s\\]*$", ErrorMessage = "Đường dẫn trong website phải bắt đầu bằng / và không chứa khoảng trắng.")]
        [Display(Name = "Đường dẫn (URL)")]
        public string Url { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int ThuTu { get; set; }

        [Display(Name = "Hiển thị")]
        public bool HienThi { get; set; } = true;
    }
}
