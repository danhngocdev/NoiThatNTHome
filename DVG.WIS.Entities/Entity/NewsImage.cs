using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    public partial class NewsImage
    {
        public int Id { get; set; }
        public long NewsId { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Alternate { get; set; }
        public string With { get; set; }
        public string Height { get; set; }
        public Nullable<double> Opacity { get; set; }
        public Nullable<int> Ordinal { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedDateSpan { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedDateSpan { get; set; }
        public string ImageUrlCrop { get; set; }
    }
}
