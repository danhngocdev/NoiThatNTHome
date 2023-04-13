using DVG.WIS.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core
{
    public class SEO
    {
        public static string AddTitle(string title)
        {
            //return string.Format("{0} | {1}", title, Const.SiteName);
            return title;
        }
        public static string AddTitleHasDomain(string title)
        {
            return string.Format("{0} | {1}", title, StaticVariable.Domain);
        }
        public static string AddMeta(string metaName, string content)
        {
            const string metaFormat = "<meta name=\"{0}\" content=\"{1}\" />";
            return string.IsNullOrEmpty(content) ? string.Empty : string.Format(metaFormat, metaName, content);
        }

        public static string AddMetaFacebook(string title, string type, string description, string url, string image, string domainAndCropSize = "", int imgWidth = 620, int imgHeight = 324)
        {
            description = description.Trim() == string.Empty ? " " : PlainText(description);
            title = title.Trim() == string.Empty ? " " : PlainText(title);
            type = type.Trim() == string.Empty ? " " : type;
            url = url.Trim() == string.Empty ? System.Web.HttpContext.Current.Request.RawUrl : url;
            StringBuilder metaFormat = new StringBuilder();
            metaFormat.AppendFormat("<meta property=\"og:site_name\" content=\"{0}\" />", StaticVariable.Domain);
            metaFormat.AppendFormat("<meta property=\"og:title\" content=\"{0}\" />", title);
            metaFormat.AppendFormat("<meta property=\"og:type\" content=\"{0}\" />", type);
            metaFormat.AppendFormat("<meta property=\"og:description\" content=\"{0}\" />", description);
            metaFormat.AppendFormat("<meta property=\"og:url\" content=\"{0}\" />", url);
            if (!string.IsNullOrEmpty(image))
            {
                metaFormat.AppendFormat("<meta property=\"og:image\" content=\"{0}\" />", domainAndCropSize + image);
                metaFormat.Append("<meta property=\"og:image:type\" content=\"image/jpg\" />");
                metaFormat.AppendFormat("<meta property=\"og:image:width\" content=\"{0}\" />", imgWidth);
                metaFormat.AppendFormat("<meta property=\"og:image:height\" content=\"{0}\" />", imgHeight);
            }
            return metaFormat.ToString();
        }

        public static string AddMetaImages(string image, string domainAndCropSize = "", int imgWidth = 620, int imgHeight = 324)
        {
            StringBuilder metaFormat = new StringBuilder();
            domainAndCropSize = string.Concat(StaticVariable.DomainImage, "/crop/620x324/");
            metaFormat.AppendFormat("<meta property=\"og:image\" content=\"{0}\" />", domainAndCropSize + image);
            metaFormat.Append("<meta property=\"og:image:type\" content=\"image/jpg\" />");
            metaFormat.AppendFormat("<meta property=\"og:image:width\" content=\"{0}\" />", imgWidth);
            metaFormat.AppendFormat("<meta property=\"og:image:height\" content=\"{0}\" />", imgHeight);
            return metaFormat.ToString();
        }

        public static string MetaCanonical(string link)
        {
            if (String.IsNullOrEmpty(link)) return string.Empty;
            return string.Format("<link rel=\"canonical\" href=\"{0}\" />", link);
        }
        public static string MetaAlternate(string link)
        {
            if (String.IsNullOrEmpty(link)) return string.Empty;
            return string.Format("<link rel=\"alternate\" href=\"{0}\" media=\"only screen and (max-width: 640px)\" /><link rel=\"alternate\" href=\"{0}\" media=\"handheld\" /><link rel=\"alternate\" hreflang=\"en-vi\" href=\"{0}\"/>", link);
        }

        public static string RemoveHTMLTag(string htmlString)
        {
            string pattern = @"(<[^>]+>)";
            string text = System.Text.RegularExpressions.Regex.Replace(htmlString, pattern, string.Empty);
            return text;
        }
        public static string PreProcessSearchString(string searchString)
        {
            string output = searchString.Replace("'", " ").Replace("\"\"", " ").Replace(">", " ").Replace("<", " ").Replace(",", " ").Replace("(", " ").Replace(")", " ").Replace("\"", " ");
            output = System.Text.RegularExpressions.Regex.Replace(output, "[ ]+", "+");
            return output.Trim();
        }

        public static string PlainText(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return RemoveHTMLTag(input).Replace("\"", string.Empty).Replace("'", string.Empty).Trim();
            return string.Empty;
        }
    }
}
