using System.Web.Mvc;
using WEBLINH.Filters;
using WEBLINH.Models;

namespace WEBLINH.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class MenuController : Controller
    {
        public ActionResult Index()
        {
            return View(MenuDAO.GetAll());
        }

        public ActionResult Create()
        {
            return View(new Menu { HienThi = true, ThuTu = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menu m)
        {
            if (!ModelState.IsValid) return View(m);
            MenuDAO.Insert(m);
            TempData["Msg"] = "Thêm menu thành công!";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var m = MenuDAO.GetById(id);
            if (m == null) return HttpNotFound();
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu m)
        {
            if (!ModelState.IsValid) return View(m);
            MenuDAO.Update(m);
            TempData["Msg"] = "Cập nhật menu thành công!";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            MenuDAO.Delete(id);
            TempData["Msg"] = "Đã xóa menu.";
            return RedirectToAction("Index");
        }
    }
}
