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
    public class NewsModel
    {
        public NewsModel() { }
        public NewsModel(Entities.News model)
        {
            this.Id = model.Id;
            this.Type = model.Type;
            this.Title = model.Title;
            this.Sapo = model.Sapo;
            this.Description = model.Description;
            this.Avatar = model.Avatar;
            this.CategoryId = model.CategoryId;
            this.Source = model.Source;
            this.SEOTitle = model.SEOTitle;
            this.SEODescription = model.SEODescription;
            this.SEOKeyword = model.SEOKeyword;
            this.TextSearch = model.TextSearch;
            this.Status = model.Status;
            this.CreatedBy = model.CreatedBy;
            this.CreatedDate = model.CreatedDate;
            this.ModifiedBy = model.ModifiedBy;
            this.ModifiedDate = model.ModifiedDate;
            this.PublishedDate = model.PublishedDate;
            this.LanguageId = model.LanguageId;
            this.IsHighLight = model.IsHighLight;
        }
        public int Id { set; get; }
        public int Type { set; get; }
        public string Title { set; get; }
        public string Sapo { set; get; }
        public string Description { set; get; }
        public string Avatar { set; get; }
        public string Source { set; get; }
        public int CategoryId { set; get; }
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
        public string StatusName
        {
            get
            {
                return EnumHelper.Instance.ConvertEnumToList<NewsStatusEnum>().Where(x => x.Id == Status).FirstOrDefault().Name;
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
        public string PublishedDateStr
        {
            get
            {
                return PublishedDate.ToString(Const.DateTimeFormatAdmin);
            }
            set
            {
                PublishedDate = !string.IsNullOrEmpty(value) ? Utils.ConvertStringToDateTime(value, Const.CustomeDateFormat) : DateTime.Now;
            }
        }

        public bool IsPublished
        {
            get
            {
                return Status == (int)NewsStatusEnum.Published;
            }
        }
        public bool IsBomb
        {
            get { return DateTime.Compare(DateTime.Now, PublishedDate) < 0; }
        }

        public string IdStr
        {
            get { return Id.ToString(); }
            set
            {
                Id = !string.IsNullOrEmpty(value) ? Convert.ToInt32(value) : 0;
            }
        }

        public string EncryptNewsId
        {
            get { return EncryptUtility.EncryptId(this.Id); }
        }

        public string AvatarStr
        {
            get { return !string.IsNullOrEmpty(Avatar) ? CoreUtils.BuildCropAvatar(Avatar, StaticVariable.NoImage, AppSettings.Instance.GetString("CropSizeCMS")) : StaticVariable.NoImage; }
        }

        public StatusOfNews StatusOfNews
        {
            get
            {
                switch (Status)
                {
                    case (int)NewsStatusEnum.PendingApproved:
                        return new StatusOfNews() { IsPendingApproved = true };
                    case (int)NewsStatusEnum.Deleted:
                        return new StatusOfNews() { IsDeleted = true };
                    case (int)NewsStatusEnum.Published:
                        return new StatusOfNews() { IsPublished = true };
                    case (int)NewsStatusEnum.UnPublished:
                        return new StatusOfNews() { IsUnPublished = true };
                }
                return new StatusOfNews();
            }
        }

        public IEnumerable<Entities.Category> ListCategory { get; set; }
        public List<Entities.NewsImage> ListImage { get; set; }
        public IEnumerable<EnumHelper.Enums> ListLanguage { get; set; }
        public int LanguageId { get; set; }
        public int IsHighLight { get; set; }
    }

    public class NewsSearchModel
    {
        public NewsSearchModel()
        {
            this.EditItem = new NewsModel();
        }
        public int NewsId { get; set; }
        public int CategoryId { get; set; }
        public int Status { get; set; }
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public StatusOfNewsPermission StatusOfNewsPermission { get; set; }
        public List<NewsModel> ListData { get; set; }
        public NewsModel EditItem { get; set; }
        public IEnumerable<Entities.Category> ListCategory { get; set; }
        public IEnumerable<EnumHelper.Enums> ListStatus { get; set; }
    }

    [Serializable]
    public class StatusOfNews
    {
        public bool IsTemp { get; set; }
        public bool IsPendingApproved { get; set; }
        public bool IsPublished { get; set; }
        public bool IsUnPublished { get; set; }
        public bool IsDeleted { get; set; }
    }
}
