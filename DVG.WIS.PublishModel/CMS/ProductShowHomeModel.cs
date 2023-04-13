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
    public class ProductShowHomeModel
    {
        public ProductShowHomeModel()
        {

        }

        public ProductShowHomeModel(ProductShowHome productShowHome)
        {
            this.Id = productShowHome.Id;
            this.Title = productShowHome.Title;
            this.Status = productShowHome.Status;
            this.CategoryId = productShowHome.CategoryId;
            this.Limit = productShowHome.Limit;

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Limit { get; set; }
        public int CategoryId { get; set; }
        public int Status { get; set; }

        public string CateName { get; set; }
        public string StatusName
        {
            get
            {
                return EnumHelper.Instance.ConvertEnumToList<ListStatusEnum>().Where(x => x.Id == Status).FirstOrDefault().Name;
            }
        }
    }

    public class ProductShowHomeSearchModel
    {
        public ProductShowHomeSearchModel()
        {
            this.EditItem = new ProductShowHomeModel();
        }

        public int Status { get; set; }
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<ProductShowHomeModel> ListData { get; set; }
        public ProductShowHomeModel EditItem { get; set; }
        public IEnumerable<EnumHelper.Enums> ListStatus { get; set; }
    }
}
