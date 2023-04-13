using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel.CMS
{
    public class ExportExcelResponseModel
    {
        public ExportExcelResponseModel(Customer model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Address = model.Address;
            this.Phone = model.Phone;
            this.Email = model.Email;
            this.Title = model.Title;
            this.Description = model.Description;
            this.CreatedDate = model.CreatedDate;
        }
        public int Id { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
