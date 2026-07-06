using System.Web.Mvc;
using WEBLINH.Models;

namespace WEBLINH.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            ViewBag.NoiBat = SanPhamDAO.GetNoiBat(8);
            ViewBag.MoiNhat = SanPhamDAO.GetMoiNhat(8);
            return View();
        }

        public ActionResult GioiThieu()
        {
            return View();
        }

        public ActionResult LienHe()
        {
            return View();
        }
    }
}
