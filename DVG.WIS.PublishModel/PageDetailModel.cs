using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class PageDetailModel
    {
        public PageDetailModel() { }

        public int Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string SEOTitle { set; get; }
        public string SEODescription { set; get; }
        public string SEOKeyword { set; get; }
    }
}
