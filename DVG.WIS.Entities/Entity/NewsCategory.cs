using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    public partial class NewsCategory
    {
        public long NewsId { get; set; }
        public int CateId { get; set; }
        public Nullable<bool> IsPrimary { get; set; }
        public Nullable<int> Ordinal { get; set; }
    }
}
