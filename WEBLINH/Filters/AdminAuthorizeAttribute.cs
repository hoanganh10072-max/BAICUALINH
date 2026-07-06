using System.Web.Mvc;
using System.Web.Routing;
using WEBLINH.Models;

namespace WEBLINH.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var user = session != null ? session["AdminUser"] as NguoiDung : null;

            if (user == null || user.VaiTro != "Admin")
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        area = "Admin",
                        controller = "Account",
                        action = "DangNhap"
                    }));
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class UserAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var user = session != null ? session["User"] as NguoiDung : null;

            if (user == null)
            {
                var request = filterContext.HttpContext.Request;
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        area = "User",
                        controller = "TaiKhoan",
                        action = "DangNhap",
                        returnUrl = request.RawUrl
                    }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
