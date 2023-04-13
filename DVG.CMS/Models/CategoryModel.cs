using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;

namespace DVG.CMS.Models
{
    public class CategoryModelCMS
    {
        public CategoryModelCMS() { }
        public CategoryModelCMS(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.Description = category.Description;
            this.ModifiedDate = category.ModifiedDate;
            this.CreatedDate = category.CreatedDate;
            this.ShortURL = category.ShortURL;
            this.SortOrder = category.SortOrder;
            this.ParentId = category.ParentId;
            this.Invisibled = category.Invisibled;
            this.Status = category.Status;
            this.AllowComment = category.AllowComment;
            this.Type = category.Type;
            this.MetaTitle = category.MetaTitle;
            this.MetaDescription = category.MetaDescription;
        }
        public CategoryModelCMS(CategoryModel category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.Description = category.Description;
            this.ModifiedDate = category.ModifiedDate;
            this.CreatedDate = category.CreatedDate;
            this.ShortURL = category.ShortURL;
            this.SortOrder = category.SortOrder;
            this.ParentId = category.ParentId;
            this.Invisibled = category.Invisibled;
            this.Status = category.Status;
            this.AllowComment = category.AllowComment;
            this.Type = category.Type;
            this.Level = category.Level;
            this.CountNews = category.CountNews;
            this.MetaTitle = category.MetaTitle;
            this.MetaDescription = category.MetaDescription;
        }
        public int Id { get; set; }
        public string EncryptCateId { get { return EncryptUtility.EncryptId(Id); } }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedDateStr
        {
            get { return CreatedDate != null ? CreatedDate.Value.ToString("dd/MM/yyyy hh:mm") : string.Empty; }
        }
        public string ShortURL { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public int ParentId { get; set; }
        public bool Invisibled { get; set; }
        public int Status { get; set; }
        public Nullable<bool> AllowComment { get; set; }
        public int Type { get; set; }
        public int Level { get; set; }

        public IEnumerable<EnumHelper.Enums> ListNewsType
        {
            get { return EnumHelper.Instance.ConvertEnumToList<NewsTypeEnum>(); }
        }
        public int CountNews { get; set; }
        public string NewsTypeName { get; set; }
        public string StatusName { get; set; }

    }

    public class CategorySearchModel
    {
        public string Keyword { get; set; }
        public int ParentId { get; set; }
        public int NewsType { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class CategoryParamModel
    {
        public List<EnumHelper.Enums> ListNewsType { get; set; }
        public List<Category> ListCateParrent { get; set; }
    }
}