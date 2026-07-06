using System.Web.Mvc;
using WEBLINH.Filters;
using WEBLINH.Models;

namespace WEBLINH.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class DanhMucController : Controller
    {
        public ActionResult Index()
        {
            return View(DanhMucDAO.GetAll());
        }

        public ActionResult Create()
        {
            return View(new DanhMuc());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DanhMuc dm)
        {
            if (!ModelState.IsValid) return View(dm);
            DanhMucDAO.Insert(dm);
            TempData["Msg"] = "Thêm danh mục thành công!";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var dm = DanhMucDAO.GetById(id);
            if (dm == null) return HttpNotFound();
            return View(dm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DanhMuc dm)
        {
            if (!ModelState.IsValid) return View(dm);
            DanhMucDAO.Update(dm);
            TempData["Msg"] = "Cập nhật danh mục thành công!";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                DanhMucDAO.Delete(id);
                TempData["Msg"] = "Đã xóa danh mục.";
            }
            catch
            {
                TempData["Msg"] = "Không thể xóa: danh mục còn sản phẩm.";
            }
            return RedirectToAction("Index");
        }
    }
}
