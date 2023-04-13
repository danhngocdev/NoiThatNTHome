using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Serializable]
    public class Pagings
    {
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }
        public String LinkSite { get; set; }
        public Int32 Count { get; set; }
    }

    [Serializable]
    public class PagingEntity
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int NumPage { get; set; }
    }
}
