using DVG.WIS.Core;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel.CMS
{
    public class RecruitmentModel
    {
        public RecruitmentModel() { }
        public RecruitmentModel(Recruitment model)
        {

            this.Id = model.Id;
            this.CateName = model.CateName;
            this.Position = model.Position;
            this.Address = model.Address;
            this.EndDate = model.EndDate;
            this.Description = model.Description;
            this.CreatedDate = model.CreatedDate;
            this.CreatedBy = model.CreatedBy;
            this.Status = model.Status;
            this.EndDateStr = EndDate != null ? EndDate.Value.ToString(Const.NormalDateFormat) : string.Empty;
        }
        public int Id { get; set; }
        public string CateName { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int Status { get; set; }
        public string EndDateStr
        {
            get; set;
        }
        public string StatusStr
        {
            get
            {
                return Status == 1 ? "Hoạt động" : "Không hoạt động";
            }
        }
    }

    public class RecruitmentSearchModel
    {
        public RecruitmentSearchModel()
        {
            this.EditItem = new RecruitmentModel();
        }
        public int RecruitmentId { get; set; }
        public string Position { get; set; }
        public string CateName { get; set; }
        public int Status { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<RecruitmentModel> ListData { get; set; }
        public RecruitmentModel EditItem { get; set; }
    }
}
