using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Local;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class BaseController : Controller
    {
        //protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        //{
        //    //if (!string.IsNullOrEmpty(Request.QueryString[Const.QueryString.Language]))
        //    //    return base.BeginExecuteCore(callback, state);
        //    string cultureName = RouteData.Values["culture"] as string;

        //    // Attempt to read the culture cookie from Request
        //    if (cultureName == null)
        //    {
        //        //cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages
        //        cultureName = StaticVariable.DefaultLanguage;
        //        RouteData.Values["culture"] = StaticVariable.DefaultLanguage;
        //    }
        //    // Validate culture name
        //    cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

        //    if (RouteData.Values["culture"] as string != cultureName)
        //    {

        //        // Force a valid culture in the URL
        //        RouteData.Values["culture"] = cultureName.ToLowerInvariant(); // lower case too

        //        // Redirect user
        //        Response.RedirectToRoute(RouteData.Values);
        //    }

        //    // Modify current thread's cultures            
        //    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
        //    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        //    return base.BeginExecuteCore(callback, state);
        //}

        protected string Lang
        {
            get
            {
                return RouteData.Values["culture"].ToString();
            }
        }

        protected int LanguageId
        {
            get
            {
                //if (Lang.ToLower().Equals(LanguageEnum.En.ToString().ToLower()))
                //    return 1;
                //else
                return 0;
            }
        }

        protected ActionResult Redirect301(string standardUrl)
        {
            string destinationUrl = standardUrl;
            destinationUrl = destinationUrl.Replace(StaticVariable.BaseUrlNoSlash, string.Empty);
            if (!destinationUrl.StartsWith("/")) destinationUrl = string.Concat("/", destinationUrl);
            destinationUrl = string.Concat(StaticVariable.BaseUrlNoSlash, destinationUrl);
            return RedirectPermanent(destinationUrl);
        }
    }
}