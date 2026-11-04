using System;
using System.Web;
using System.Web.Mvc;
using WEBLINH.Filters;
using WEBLINH.Models;

namespace WEBLINH.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class TrangChuController : Controller
    {
        public ActionResult Index() { return View(ContentDAO.GetBanner()); }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(HomeBanner model, HttpPostedFileBase bannerImage)
        {
            model.ImageName = ContentDAO.GetBanner().ImageName;
            if (ModelState.IsValid)
            {
                try
                {
                    model.ImageName = ContentImageUpload.Save(bannerImage, Server.MapPath("~/Content/uploads")) ?? model.ImageName;
                    ContentDAO.SaveBanner(model);
                    TempData["Msg"] = "Đã cập nhật banner trang chủ.";
                    return RedirectToAction("Index");
                }
                catch (ArgumentException ex) { ModelState.AddModelError("", ex.Message); }
            }
            return View(model);
        }
    }
}
