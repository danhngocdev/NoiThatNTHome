using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DVG.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            bool debugMode = AppSettings.Instance.GetBool("DebugMode", false);
            if (debugMode) return;

            Exception ex = Server.GetLastError().GetBaseException();
            string errorMessage = string.Concat(Environment.NewLine, "IsMobileDevice: ", Request.Browser.IsMobileDevice, Environment.NewLine, Request.RawUrl.ToString(), Environment.NewLine, "Refer: ", Request.UrlReferrer, Environment.NewLine, "From: ", GetIP(), Environment.NewLine, ex.ToString(), Environment.NewLine, ex.StackTrace, Environment.NewLine, "====================", Environment.NewLine);

            if (ex.Message.IndexOf("was not found") != -1)
            {
                Logger.WriteLog(Logger.LogType.Trace, errorMessage);
                Response.StatusCode = 404;
            }
            else
            {
                Logger.WriteLog(Logger.LogType.Error, errorMessage);
                if (HttpContext.Current.Request.Path.ToLower() != "/")
                    Response.Redirect("/");
                else
                    Response.Redirect("/upgrade.html");
            }
        }

        private string GetIP()
        {
            return HttpContext.Current.Request.Headers["X-Forwarded-For"];
        }
    }
}
