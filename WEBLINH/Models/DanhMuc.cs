using System.ComponentModel.DataAnnotations;

namespace WEBLINH.Models
{
    public class DanhMuc
    {
        public int MaDM { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [Display(Name = "Tên danh mục")]
        public string TenDM { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
    }
}
