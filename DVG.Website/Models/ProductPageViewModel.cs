using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVG.Website.Models
{
    public class ProductPageViewModel
    {
        public ProductPageViewModel()
        {
            ListProductForFace = new List<ProductFEListModel>();
            ListProductForBody = new List<ProductFEListModel>();
        }
        public List<ProductFEListModel> ListProductForFace { get; set; }
        public List<ProductFEListModel> ListProductForBody { get; set; }
    }
}