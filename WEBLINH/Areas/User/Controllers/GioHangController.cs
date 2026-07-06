using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBLINH.Filters;
using WEBLINH.Models;

namespace WEBLINH.Areas.User.Controllers
{
    public class GioHangController : Controller
    {
        private const string CART_KEY = "Cart";

        private List<CartItem> GetCart()
        {
            var cart = Session[CART_KEY] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session[CART_KEY] = cart;
            }
            return cart;
        }

        public ActionResult Index()
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            return View(GetCart());
        }

        public ActionResult Them(int id, int soLuong = 1)
        {
            var sp = SanPhamDAO.GetById(id);
            if (sp == null) return HttpNotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSP == id);
            if (item != null)
            {
                item.SoLuong += soLuong;
            }
            else
            {
                cart.Add(new CartItem
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    Anh = sp.Anh,
                    Gia = sp.Gia,
                    SoLuong = soLuong
                });
            }
            Session[CART_KEY] = cart;
            TempData["Msg"] = "Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CapNhat(int id, int soLuong)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSP == id);
            if (item != null)
            {
                if (soLuong <= 0) cart.Remove(item);
                else item.SoLuong = soLuong;
            }
            Session[CART_KEY] = cart;
            return RedirectToAction("Index");
        }

        public ActionResult Xoa(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.MaSP == id);
            if (item != null) cart.Remove(item);
            Session[CART_KEY] = cart;
            return RedirectToAction("Index");
        }

        public ActionResult XoaTatCa()
        {
            Session[CART_KEY] = new List<CartItem>();
            return RedirectToAction("Index");
        }

        [UserAuthorize]
        public ActionResult ThanhToan()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Msg"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }
            var user = Session["User"] as NguoiDung;
            var dh = new DonHang
            {
                HoTenNhan = user.HoTen,
                SDTNhan = user.SDT,
                DiaChiGiao = user.DiaChi
            };
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            ViewBag.Cart = cart;
            ViewBag.TongTien = cart.Sum(c => c.ThanhTien);
            return View(dh);
        }

        [HttpPost]
        [UserAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult ThanhToan(DonHang dh)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Msg"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.DanhMuc = DanhMucDAO.GetAll();
                ViewBag.Cart = cart;
                ViewBag.TongTien = cart.Sum(c => c.ThanhTien);
                return View(dh);
            }

            var user = Session["User"] as NguoiDung;
            dh.MaND = user.MaND;
            dh.TongTien = cart.Sum(c => c.ThanhTien);

            int maDH = DonHangDAO.CreateOrder(dh, cart);
            Session[CART_KEY] = new List<CartItem>();
            return RedirectToAction("QRThanhToan", new { id = maDH });
        }

        [UserAuthorize]
        public ActionResult QRThanhToan(int id)
        {
            var user = Session["User"] as NguoiDung;
            var dh = DonHangDAO.GetById(id);
            if (dh == null || dh.MaND != user.MaND) return HttpNotFound();

            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            ViewBag.BankCode = ConfigurationManager.AppSettings["BankCode"];
            ViewBag.BankAccount = ConfigurationManager.AppSettings["BankAccount"];
            ViewBag.BankAccountName = ConfigurationManager.AppSettings["BankAccountName"];

            // Noi dung chuyen khoan
            string noiDung = "DH" + dh.MaDH + " " + user.TenDN;
            ViewBag.NoiDungCK = noiDung;

            // Tao URL VietQR (khong can API key)
            // Format: https://img.vietqr.io/image/{bank}-{account}-compact2.png?amount=X&addInfo=Y&accountName=Z
            string vietQRUrl = string.Format(
                "https://img.vietqr.io/image/{0}-{1}-compact2.png?amount={2}&addInfo={3}&accountName={4}",
                ViewBag.BankCode,
                ViewBag.BankAccount,
                (long)dh.TongTien,
                Uri.EscapeDataString(noiDung),
                Uri.EscapeDataString((string)ViewBag.BankAccountName));
            ViewBag.QRUrl = vietQRUrl;

            return View(dh);
        }

        [HttpPost]
        [UserAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult UploadBill(int id, HttpPostedFileBase billFile)
        {
            var user = Session["User"] as NguoiDung;
            var dh = DonHangDAO.GetById(id);
            if (dh == null || dh.MaND != user.MaND) return HttpNotFound();

            if (billFile == null || billFile.ContentLength == 0)
            {
                TempData["Msg"] = "Vui lòng chọn ảnh bill chuyển khoản!";
                return RedirectToAction("QRThanhToan", new { id = id });
            }

            var uploadDir = Server.MapPath("~/Content/bills");
            if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

            var ext = Path.GetExtension(billFile.FileName);
            var name = "bill_" + id + "_" + Guid.NewGuid().ToString("N") + ext;
            var path = Path.Combine(uploadDir, name);
            billFile.SaveAs(path);

            DonHangDAO.UpdateBill(id, name);
            return RedirectToAction("DatHangThanhCong", new { id = id });
        }

        public ActionResult DatHangThanhCong(int id)
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            ViewBag.MaDH = id;
            return View();
        }
    }
}
