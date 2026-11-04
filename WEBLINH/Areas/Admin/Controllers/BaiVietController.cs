using System;
using System.Web;
using System.Web.Mvc;
using WEBLINH.Filters;
using WEBLINH.Models;

namespace WEBLINH.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class BaiVietController : Controller
    {
        public ActionResult Index() { return View(ContentDAO.GetPosts()); }
        public ActionResult Create() { return View("Edit", new BaiViet()); }
        public ActionResult Edit(int id)
        {
            var b = ContentDAO.GetPost(id);
            return b == null ? (ActionResult)HttpNotFound() : View(b);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Save(BaiViet model, HttpPostedFileBase coverImage)
        {
            var old = model.Id == 0 ? null : ContentDAO.GetPost(model.Id);
            if (model.Id != 0 && old == null) return HttpNotFound();
            model.Anh = old == null ? null : old.Anh;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Anh = ContentImageUpload.Save(coverImage, Server.MapPath("~/Content/uploads")) ?? model.Anh;
                    ContentDAO.SavePost(model);
                    TempData["Msg"] = model.DaDang ? "Đã xuất bản bài viết lên website." : "Đã lưu bản nháp. Bài chưa hiển thị với khách hàng.";
                    return RedirectToAction("Index");
                }
                catch (ArgumentException ex) { ModelState.AddModelError("", ex.Message); }
            }
            return View("Edit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            ContentDAO.DeletePost(id);
            TempData["Msg"] = "Đã xóa bài viết.";
            return RedirectToAction("Index");
        }
    }
}
