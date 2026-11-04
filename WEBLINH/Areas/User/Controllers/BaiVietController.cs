using System.Web.Mvc;
using WEBLINH.Models;

namespace WEBLINH.Areas.User.Controllers
{
    public class BaiVietController : Controller
    {
        public ActionResult Index() { return View(ContentDAO.GetPosts(true)); }
        public ActionResult ChiTiet(int id)
        {
            var b = ContentDAO.GetPost(id, true);
            return b == null ? (ActionResult)HttpNotFound() : View(b);
        }
    }
}
