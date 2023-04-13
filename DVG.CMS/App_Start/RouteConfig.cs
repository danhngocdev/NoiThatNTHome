using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DVG.CMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login", returnUrl = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "PermissionDenied",
                url: "permission-denied",
                defaults: new { controller = "Account", action = "PermissionDenied", returnUrl = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "ChangePassword",
                url: "doi-mat-khau",
                defaults: new { controller = "Account", action = "Manager" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "dang-xuat",
                defaults: new { controller = "Account", action = "Logout", returnUrl = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "DVG.CMS.Controllers" }
            );
        }
    }
}
