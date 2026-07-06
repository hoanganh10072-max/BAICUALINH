using System.Web.Mvc;
using WEBLINH.Filters;
using WEBLINH.Models;

namespace WEBLINH.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.SoDonHang = DonHangDAO.CountDonHang();
            ViewBag.SoSanPham = DonHangDAO.CountSanPham();
            ViewBag.SoNguoiDung = DonHangDAO.CountNguoiDung();
            ViewBag.DoanhThu = DonHangDAO.TongDoanhThu();
            return View();
        }
    }
}
