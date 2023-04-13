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
    public class ProductModel
    {
        public ProductModel() { }
        public ProductModel(Entities.Product model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Sapo = model.Sapo;
            this.Description = model.Description;
            this.Avatar = model.Avatar;
            this.SEOTitle = model.SEOTitle;
            this.SEODescription = model.SEODescription;
            this.SEOKeyword = model.SEOKeyword;
            this.TextSearch = model.TextSearch;
            this.Status = model.Status;
            this.CreatedBy = model.CreatedBy;
            this.CreatedDate = model.CreatedDate;
            this.ModifiedBy = model.ModifiedBy;
            this.ModifiedDate = model.ModifiedDate;
            this.Price = model.Price;
            this.PricePromotion = model.PricePromotion;
            this.Code = model.Code;
            this.Capacity = model.Capacity;
            this.MadeIn = model.MadeIn;
            this.IsOutStock = model.IsOutStock;
            this.IsHighLight = model.IsHighLight;
            this.CategoryId = model.CategoryId;
        }
        public int Id { set; get; }
        public int CategoryId { set; get; }
        public int IsHighLight { set; get; }
        public string Name { set; get; }
        public string Sapo { set; get; }
    
        public string Description { set; get; }
        public string Avatar { set; get; }
        public string SEOTitle { set; get; }
        public string SEODescription { set; get; }
        public string SEOKeyword { set; get; }
        public string TextSearch { set; get; }
        public int Status { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public DateTime ModifiedDate { set; get; }
        public string ModifiedBy { set; get; }
        public DateTime PublishedDate { set; get; }
        public double Price { get; set; }
        public double? PricePromotion { get; set; }
        public string Code { get; set; }
        public string Capacity { get; set; }
        public string MadeIn { get; set; }
        public int IsOutStock { get; set; }
        public string StatusName
        {
            get
            {
                return EnumHelper.Instance.ConvertEnumToList<ProductStatusEnum>().Where(x => x.Id == Status).FirstOrDefault().Name;
            }
        }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToString(Const.DateTimeFormatAdmin);
            }
        }
        public string ModifiedDateStr
        {
            get
            {
                return ModifiedDate.ToString(Const.DateTimeFormatAdmin);
            }
        }

        public bool IsPublished
        {
            get
            {
                return Status == (int)ProductStatusEnum.Published;
            }
        }

        public string IdStr
        {
            get { return Id.ToString(); }
            set
            {
                Id = !string.IsNullOrEmpty(value) ? Convert.ToInt32(value) : 0;
            }
        }

        public string EncryptProductId
        {
            get { return EncryptUtility.EncryptId(this.Id); }
        }

        public string AvatarStr
        {
            get { return !string.IsNullOrEmpty(Avatar) ? CoreUtils.BuildCropAvatar(Avatar, StaticVariable.NoImage, AppSettings.Instance.GetString("CropSizeCMS")) : StaticVariable.NoImage; }
        }

        public StatusOfProduct StatusOfProduct
        {
            get
            {
                switch (Status)
                {
                    case (int)ProductStatusEnum.PendingApproved:
                        return new StatusOfProduct() { IsPendingApproved = true };
                    case (int)ProductStatusEnum.Deleted:
                        return new StatusOfProduct() { IsDeleted = true };
                    case (int)ProductStatusEnum.Published:
                        return new StatusOfProduct() { IsPublished = true };
                    case (int)ProductStatusEnum.UnPublished:
                        return new StatusOfProduct() { IsUnPublished = true };
                }
                return new StatusOfProduct();
            }
        }
        public List<Entities.NewsImage> ListImage { get; set; }
        public List<Entities.Category> ListCategory { get; set; }
    }

    public class ProductSearchModel
    {
        public ProductSearchModel()
        {
            this.EditItem = new ProductModel();
        }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int Status { get; set; }
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public StatusOfProductPermission StatusOfProductPermission { get; set; }
        public List<ProductModel> ListData { get; set; }
        public ProductModel EditItem { get; set; }
        public IEnumerable<EnumHelper.Enums> ListStatus { get; set; }
        public IEnumerable<EnumHelper.Enums> ListUnit { get; set; }
    }

    [Serializable]
    public class StatusOfProduct
    {
        public bool IsPendingApproved { get; set; }
        public bool IsPublished { get; set; }
        public bool IsUnPublished { get; set; }
        public bool IsDeleted { get; set; }
    }

    [Serializable]
    public class StatusOfProductPermission
    {
        public bool IsPendingApprove { get; set; }
        public bool IsPublish { get; set; }
        public bool IsUnPublish { get; set; }
        public bool IsDelete { get; set; }
    }
}
