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
    public class CustomerModel
    {
        public CustomerModel() { }
        public CustomerModel(Customer model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Email = model.Email;
            this.Phone = model.Phone;
            this.Address = model.Address;
            this.Title = model.Title;
            this.Description = model.Description;
        }
        public int Id { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string Title { set; get; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CustomerSearchModel
    {
        public CustomerSearchModel()
        {
            this.EditItem = new CustomerModel();
        }
        public int CustomerId { set; get; }
        public DateTime? StartDate { get; set; }
        public string StartDateStr { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndDateStr { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<CustomerModel> ListData { get; set; }
        public CustomerModel EditItem { get; set; }
    }

}
