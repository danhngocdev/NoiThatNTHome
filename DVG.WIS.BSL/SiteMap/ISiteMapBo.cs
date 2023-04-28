using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.SiteMap
{
    public interface ISiteMapBo
    {
        string GenSiteMapIndex();

        string GenSiteMapCategory();
        string GenSiteMapProduct();
        string GenSiteMapArticle();

    }
}
