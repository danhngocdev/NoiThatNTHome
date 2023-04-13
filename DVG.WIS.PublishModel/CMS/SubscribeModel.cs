using System;
using System.Collections.Generic;
using System.Linq;
using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities;

namespace DVG.WIS.PublicModel.CMS
{
    public class SubscribeModel
    {
        public SubscribeModel() { }
        public SubscribeModel(Entities.Subscribe sub)
        {
            this.Id = sub.Id;
            this.Email = sub.Email;
            this.CreatedDate = sub.CreatedDate;
            this.CreatedDateSpan = sub.CreatedDateSpan;
            this.Status = sub.Status;
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedDateSpan { get; set; }
        public Nullable<int> Status { get; set; }
        public string CreatedDateStr
        {
            get { return CreatedDate != null ? CreatedDate.Value.ToString(Const.CustomeDateFormat) : string.Empty; }
            set { }
        }
        public string StatusName { get; set; }
    }
    public class SubscribeSearchModel
    {
        public string Email { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<SubscribeModel> ListData { get; set; }
    }
}