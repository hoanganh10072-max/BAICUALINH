using System.Web.Mvc;
using WEBLINH.Models;

namespace WEBLINH.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string tenDN, string matKhau)
        {
            var nd = NguoiDungDAO.Login(tenDN, matKhau);
            if (nd == null || nd.VaiTro != "Admin")
            {
                ViewBag.Loi = "Sai tài khoản hoặc bạn không có quyền admin!";
                return View();
            }
            Session["AdminUser"] = nd;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DangXuat()
        {
            Session.Remove("AdminUser");
            return RedirectToAction("DangNhap");
        }
    }
}
