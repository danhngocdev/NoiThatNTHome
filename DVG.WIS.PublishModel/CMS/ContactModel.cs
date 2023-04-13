using DVG.WIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{

    public class InfoContactModel
    {
        public InfoContactModel()
        {

        }

        public InfoContactModel(Entities.InfoContact model)
        {
            this.Id = model.Id;
            this.Name = model.Name;      
            this.Status = model.Status;
            //this.Link = model.Link;
            this.Content = model.Content;
            this.Phone = model.Phone;
            this.DateCreated = model.CreatedDate;

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public DateTime? DateCreated { get; set; }
        public int Status { get; set; }

        public string CreatedDateStr
        {
            get { return DateCreated != null ? DateCreated.Value.ToString(Const.NormalDateFormat) : string.Empty; }
            set { }
        }
    }

    public class InfoContactSearchModel
    {
        public InfoContactSearchModel()
        {
            //this.EditItem = new BannerModel();
        }

        public DateTime? CreateDate { get; set; }
        public int BannerId { get; set; }
        public string Keyword { get; set; }
        public int Status { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListStatus { get; set; }
        public List<InfoContactModel> ListData { get; set; }
        //public BannerModel EditItem { get; set; }
    }
}
