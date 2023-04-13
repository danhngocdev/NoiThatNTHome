using DVG.WIS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Local
{
    public class Helper
    {
        public static void BuildLinkByResourceKey(string resourceKey, ref string linkVn, ref string linkEn)
        {
            var resourceManager = new System.Resources.ResourceManager(typeof(Resource));
            linkVn = resourceManager.GetString(resourceKey, CultureInfo.GetCultureInfo(LanguageEnum.Vi.ToString()));
            linkEn = resourceManager.GetString(resourceKey, CultureInfo.GetCultureInfo(LanguageEnum.En.ToString()));
        }
    }
}
