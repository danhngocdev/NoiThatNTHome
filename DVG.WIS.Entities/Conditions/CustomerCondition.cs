using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities.Conditions
{
    [Serializable]
    public class CustomerCondition
    {
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
