using DVG.WIS.Core;
using DVG.WIS.Local;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class ProductFEListModel
    {
        public ProductFEListModel(Entities.Product model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Sapo = model.Sapo;
            this.Avatar = model.Avatar;
            this.Link = CoreUtils.BuildURL("{0}/{1}-pid{2}", ConstUrl.Product, StringUtils.UnicodeToUnsignCharAndDash(Name), Id);
            this.Price = model.Price;
            this.PricePromotion = model.PricePromotion;
        }
        public int Id { set; get; }
        public string Name { set; get; }
        public string Sapo { set; get; }
        public string Avatar { set; get; }
        public string Link { get; set; }
        public double Price { get; set; }
        public double? PricePromotion { get; set; }
        public string GetAvatar(string crop)
        {
            return CoreUtils.BuildCropAvatar(this.Avatar, string.Empty, crop);
        }
        public string PriceString
        {
            get
            {
                if (this.Price > 0)
                    return string.Format("{0:#,##0} ₫", this.Price);
                else
                    return "Liên Hệ";
            }
        }
        public string PricePromotionString
        {
            get
            {
                if (this.PricePromotion > 0)
                    return string.Format("{0:#,##0} ₫", this.PricePromotion);
                else
                    return string.Empty;
            }
        }
    }

    public class ProductFEDetailModel
    {
        public ProductFEDetailModel(Entities.Product model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Sapo = model.Sapo;
            this.Avatar = model.Avatar;
            this.Description = model.Description;
            this.SEOTitle = model.SEOTitle;
            this.SEODescription = model.SEODescription;
            this.SEOKeyword = model.SEOKeyword;
            this.Price = model.Price;
            this.PricePromotion = model.PricePromotion;
        }
        public int Id { set; get; }
        public string Name { set; get; }
        public string Sapo { set; get; }
        public string Information { set; get; }
        public string Standard { set; get; }
        public string Certificate { set; get; }
        public string Description { get; set; }
        public string Avatar { set; get; }
        public string SEOTitle { set; get; }
        public string SEODescription { set; get; }
        public string SEOKeyword { set; get; }
        public double Price { set; get; }
        public double? PricePromotion { set; get; }
    }
}
