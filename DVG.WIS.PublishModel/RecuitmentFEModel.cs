using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class RecuitmentFEModel
    {
        public RecuitmentFEModel(Entities.Recruitment model)
        {
            this.Id = model.Id;
            this.CateName = model.CateName;
            this.Position = model.Position;
            this.Address = model.Address;
            this.EndDate = model.EndDate;
            this.Description = model.Description;
            this.CreatedDate = model.CreatedDate;
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
    }
}
