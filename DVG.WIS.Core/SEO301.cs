using DVG.WIS.Core.Constants;
using DVG.WIS.Utilities;
using System.Collections.Generic;
using System.Web;

namespace DVG.WIS.Core
{
    public class SEO301
    {
        private static SEO301 _instance;
        private static object syncLock = new object();

        public static SEO301 Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (syncLock)
                    {
                        if (null == _instance) _instance = new SEO301();
                    }
                }
                return _instance;
            }
        }


        public string BindingMeta(string standardUrl, string title, string description, string keyword, bool isAMP = false)
        {
            if (string.IsNullOrEmpty(title))
            {
                title = AppSettings.Instance.GetString(Const.MainTitle);
            }
            if (string.IsNullOrEmpty(description))
            {
                description = AppSettings.Instance.GetString(Const.MainDescription);
            }
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = AppSettings.Instance.GetString(Const.MainKeyword);
            }

            title = Utilities.StringUtils.RemoveStrHtmlTags(title);
            description = Utilities.StringUtils.RemoveStrHtmlTags(description);
            keyword = Utilities.StringUtils.RemoveStrHtmlTags(keyword);

            title = StringUtils.ReplaceSpecialCharater(title);
            description = StringUtils.ReplaceSpecialCharater(description);
            keyword = HttpUtility.HtmlDecode(keyword);

            string metaOgTitle = string.Format("<meta property=\"og:title\" content=\"{0}\" /> \r\n", title);
            string metaOgDesc = string.Format("<meta property=\"og:description\" content=\"{0}\" /> \r\n", description);
            string metaDesc = string.Format("<meta name=\"description\" content=\"{0}\" /> \r\n", description);
            string metaKeyword = string.Format("<meta name=\"keywords\" content=\"{0}\" />", keyword);

            string metaCanonical = string.Concat("\r\n", BindingLinkTags("canonical", StaticVariable.BaseUrlNoSlash + standardUrl));
            string metaAlternate = string.Empty;
            if (isAMP)
            {
                // If AMP then using domain Mobile to set meta canonical
                metaCanonical = string.Concat("\r\n", BindingLinkTags("canonical", StaticVariable.BaseMobileUrlNoSlash + standardUrl));
            }
            else
            {
                metaAlternate = string.Concat(metaAlternate, "\r\n", BindingLinkTags("alternate", StaticVariable.BaseMobileUrlNoSlash + standardUrl, "only screen and (max-width: 640px)"));
                metaAlternate = string.Concat("\r\n", BindingLinkTags("alternate", StaticVariable.BaseMobileUrlNoSlash + standardUrl, "handheld"));
            }

            string meta = $"{metaOgTitle}\n{metaOgDesc}\n{metaDesc}\n{metaKeyword}\n{metaCanonical}\n{metaAlternate}";

            return meta;
        }

        public string BindingLinkTags(string rel, string href, string media = "")
        {
            if (string.IsNullOrEmpty(rel) || string.IsNullOrEmpty(href)) return string.Empty;
            Dictionary<string, string> dicts = new Dictionary<string, string>();
            dicts.Add("rel", rel);
            dicts.Add("href", href);
            if (!string.IsNullOrEmpty(media))
            {
                dicts.Add("media", media);
            }
            return CoreUtils.AddMeta("link", dicts);
        }

        public bool ValidateMobile(out string destinationUrl)
        {
            destinationUrl = string.Empty;
            string url = HttpContext.Current.Request.Url.ToString().ToLower();

            string hostRequest = HttpContext.Current.Request.Url.Host.ToLower();

            hostRequest = hostRequest.Replace(string.Format(":{0}", HttpContext.Current.Request.Url.Port), string.Empty);

            string hostConfig = StaticVariable.Domain;
            string mobileHostConfig = StaticVariable.BaseMobileUrl;
            if (!hostRequest.Equals(hostConfig) && !hostRequest.Equals(mobileHostConfig))
            {
                destinationUrl = string.Empty;
                //HttpContext.Current.Response.StatusCode = 403;
                HttpContext.Current.Response.Redirect(string.Format("http://{0}", hostRequest));
                HttpContext.Current.Response.Close();
                return false;
            }

            return false;
        }

        public void Redirect301(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string hostRequest = HttpContext.Current.Request.Url.Host.ToLower();

                hostRequest = hostRequest.Replace(string.Format(":{0}", HttpContext.Current.Request.Url.Port), string.Empty);

                string mobileHostConfig = StaticVariable.BaseMobileUrl;
                var baseUrl = hostRequest.Equals(mobileHostConfig) ? StaticVariable.BaseMobileUrlNoSlash : StaticVariable.BaseUrlNoSlash;

                url = baseUrl + "/" + url.TrimStart('/');
                HttpContext.Current.Response.RedirectPermanent(url);
                /*HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", url);*/
            }
        }
    }
}