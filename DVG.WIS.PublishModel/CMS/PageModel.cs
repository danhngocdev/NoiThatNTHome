using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel.CMS
{
    public class PageModel
    {
        public PageModel() { }
        public PageModel(Entities.Page model)
        {
            this.Id = model.Id;
            this.Title = model.Title;
            this.Description = model.Description;
            this.SEOTitle = model.SEOTitle;
            this.SEODescription = model.SEODescription;
            this.SEOKeyword = model.SEOKeyword;
        }
        public int Id { set; get; }
        public string Title { set; get; }

        public string Description { set; get; }
        public string SEOTitle { set; get; }
        public string SEODescription { set; get; }
        public string SEOKeyword { set; get; }
        public string EncryptPageId
        {
            get { return EncryptUtility.EncryptId(this.Id); }
        }
        public string IdStr
        {
            get { return Id.ToString(); }
            set
            {
                Id = !string.IsNullOrEmpty(value) ? Convert.ToInt32(value) : 0;
            }
        }

    }

}
