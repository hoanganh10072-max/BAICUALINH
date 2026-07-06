using System.Web.Mvc;
using System.Web.Routing;

namespace WEBLINH
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Root "/" redirect ve /User/Home/Index
            routes.MapRoute(
                name: "Root",
                url: "",
                defaults: new { controller = "RootRedirect", action = "Index" },
                namespaces: new[] { "WEBLINH.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "RootRedirect", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WEBLINH.Controllers" }
            );
        }
    }
}
