using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WEBLINH.Filters;
using WEBLINH.Models;

namespace WEBLINH.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class SanPhamController : Controller
    {
        public ActionResult Index()
        {
            return View(SanPhamDAO.GetAll());
        }

        public ActionResult Create()
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            return View(new SanPham());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sp)
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            if (!ModelState.IsValid) return View(sp);

            if (sp.FileAnh != null && sp.FileAnh.ContentLength > 0)
            {
                sp.Anh = SaveImage(sp.FileAnh);
            }
            SanPhamDAO.Insert(sp);
            TempData["Msg"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var sp = SanPhamDAO.GetById(id);
            if (sp == null) return HttpNotFound();
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sp)
        {
            ViewBag.DanhMuc = DanhMucDAO.GetAll();
            if (!ModelState.IsValid) return View(sp);

            if (sp.FileAnh != null && sp.FileAnh.ContentLength > 0)
            {
                sp.Anh = SaveImage(sp.FileAnh);
            }
            else
            {
                // Giu anh cu
                var old = SanPhamDAO.GetById(sp.MaSP);
                sp.Anh = old != null ? old.Anh : sp.Anh;
            }
            SanPhamDAO.Update(sp);
            TempData["Msg"] = "Cập nhật sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                SanPhamDAO.Delete(id);
                TempData["Msg"] = "Đã xóa sản phẩm.";
            }
            catch
            {
                TempData["Msg"] = "Không thể xóa: sản phẩm đã có trong đơn hàng.";
            }
            return RedirectToAction("Index");
        }

        private string SaveImage(HttpPostedFileBase file)
        {
            var uploadDir = Server.MapPath("~/Content/uploads");
            if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

            var ext = Path.GetExtension(file.FileName);
            var name = Guid.NewGuid().ToString("N") + ext;
            var path = Path.Combine(uploadDir, name);
            file.SaveAs(path);
            return name;
        }
    }
}
