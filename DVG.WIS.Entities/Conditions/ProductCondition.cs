using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities.Conditions
{
    [Serializable]
    public class ProductSearch
    {
        public int Status { get; set; }
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
