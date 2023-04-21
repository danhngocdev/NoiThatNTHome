using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DVG.WIS.Business
{
    public class BuildLinkHelper
    {
        public static Dictionary<string, int> DictionaryNewsType { get; set; }

        private static object objectLock = new object();
        
        private static void InitDictionnary()
        {
            if (DictionaryNewsType == null)
            {
                lock (objectLock)
                {
                    if (null == DictionaryNewsType)
                    {
                        DictionaryNewsType = new Dictionary<string, int>();

                        DictionaryNewsType.Add(StringUtils.GetEnumDescription(NewsTypeStringEnum.News), (int)NewsTypeStringEnum.News);
                    }
                }
            }
        }

        static BuildLinkHelper()
        {
            InitDictionnary();
        }

        public BuildLinkHelper()
        {

        }
       
        
        public static string BuildURLForNews(string cateUrl, string newsUrl, int newsType, long newsId)
        {
            switch (newsType)
            {
                case (int)NewsTypeEnum.News:
                default:
                    return CoreUtils.BuildURL(ConstUrl.NewsDetailUrl, new object[] { cateUrl, newsUrl, newsId });
            }
        }

        public static string BuildURLForCategory(string cateUrl, int newsType, int id)
        {
            switch (newsType)
            {
                
                default:
                    return (!string.IsNullOrEmpty(cateUrl) && !cateUrl.StartsWith("/")) ? cateUrl : cateUrl;
            }
        }

    }
}
