using System.Web.Mvc;

namespace WEBLINH.Controllers
{
    public class RootRedirectController : Controller
    {
        // Chuyen huong tat ca truy cap root ve area User
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "User" });
        }
    }
}
