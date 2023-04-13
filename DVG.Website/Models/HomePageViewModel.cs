using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVG.Website.Models
{
    public class HomePageViewModel
    {
        public HomePageViewModel()
        {
            ListProduct = new List<ProductFEListModel>();
        }
        public string Title { get; set; }
        public string Link { get; set; }
        public List<ProductFEListModel> ListProduct { get; set; }
    }
}