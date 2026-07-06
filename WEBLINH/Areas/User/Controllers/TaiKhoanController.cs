using System.Web.Mvc;
using WEBLINH.Filters;
using WEBLINH.Models;

namespace WEBLINH.Areas.User.Controllers
{
    public class TaiKhoanController : Controller
    {
        public ActionResult DangNhap(string returnUrl)
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string tenDN, string matKhau, string returnUrl)
        {
            var nd = NguoiDungDAO.Login(tenDN, matKhau);
            if (nd == null)
            {
                ViewBag.DanhMuc = DanhMucDAO.GetAll();
                ViewBag.Loi = "Sai tên đăng nhập hoặc mật khẩu!";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            if (nd.VaiTro == "Admin")
            {
                ViewBag.DanhMuc = DanhMucDAO.GetAll();
                ViewBag.Loi = "Tài khoản admin không đăng nhập ở trang này!";
                return View();
            }
            Session["User"] = nd;
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DangKy()
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(NguoiDung nd, string xacNhanMK)
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            if (!ModelState.IsValid) return View(nd);

            if (nd.MatKhau != xacNhanMK)
            {
                ViewBag.Loi = "Mật khẩu xác nhận không trùng khớp!";
                return View(nd);
            }
            if (NguoiDungDAO.ExistsUsername(nd.TenDN))
            {
                ViewBag.Loi = "Tên đăng nhập đã tồn tại!";
                return View(nd);
            }

            NguoiDungDAO.Register(nd);
            TempData["Msg"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap");
        }

        public ActionResult DangXuat()
        {
            Session.Remove("User");
            return RedirectToAction("Index", "Home");
        }

        [UserAuthorize]
        public ActionResult DonHangCuaToi()
        {
            var user = Session["User"] as NguoiDung;
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            var list = DonHangDAO.GetByUser(user.MaND);
            return View(list);
        }

        [UserAuthorize]
        public ActionResult ChiTietDonHang(int id)
        {
            var user = Session["User"] as NguoiDung;
            var dh = DonHangDAO.GetById(id);
            if (dh == null || dh.MaND != user.MaND) return HttpNotFound();
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            return View(dh);
        }

        // Hoa don: chi hien thi khi don da duoc admin xac nhan tro len
        [UserAuthorize]
        public ActionResult HoaDon(int id)
        {
            var user = Session["User"] as NguoiDung;
            var dh = DonHangDAO.GetById(id);
            if (dh == null || dh.MaND != user.MaND) return HttpNotFound();

            var choPhep = new[] { "Đã xác nhận", "Đang giao", "Hoàn thành" };
            if (System.Array.IndexOf(choPhep, dh.TrangThai) < 0)
            {
                TempData["Msg"] = "Đơn hàng chưa được xác nhận, chưa thể xem hóa đơn.";
                return RedirectToAction("DonHangCuaToi");
            }

            ViewBag.KhachHang = NguoiDungDAO.GetById(user.MaND);
            return View(dh);
        }

        [UserAuthorize]
        public ActionResult ThongTin()
        {
            var user = Session["User"] as NguoiDung;
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            return View(NguoiDungDAO.GetById(user.MaND));
        }

        [HttpPost]
        [UserAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult ThongTin(NguoiDung nd)
        {
            var user = Session["User"] as NguoiDung;
            nd.MaND = user.MaND;
            NguoiDungDAO.UpdateThongTin(nd);
            Session["User"] = NguoiDungDAO.GetById(user.MaND);
            TempData["Msg"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("ThongTin");
        }
    }
}
