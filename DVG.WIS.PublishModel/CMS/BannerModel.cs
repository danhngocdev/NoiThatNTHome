using System;
using System.Collections.Generic;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;

namespace DVG.WIS.PublicModel.CMS
{
    public class BannerModel
    {
        public BannerModel() { }
        public BannerModel(Entities.Banner banner)
        {
            this.Id = banner.Id;
            this.Name = banner.Name;
            this.Title = banner.Title;
            this.Embed = banner.Embed.IndexOf("http") == -1 ? CoreUtils.BuildCropAvatar(banner.Embed) : banner.Embed;
            this.Status = banner.Status;
            this.Position = banner.Position;
            this.PageId = banner.PageId;
            this.TargetLink = banner.TargetLink;
            this.FromDate = banner.FromDate;
            this.UntilDate = banner.UntilDate;
            this.CreatedDate = banner.CreatedDate;
            this.CreatedBy = banner.CreatedBy;
            this.ModifiedDate = banner.ModifiedDate;
            this.ModifiedBy = banner.ModifiedBy;
            this.Platform = banner.Platform;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Embed { get; set; }
        public int Status { get; set; }
        public int? Position { get; set; }
        public int? PageId { get; set; }
        public string TargetLink { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? UntilDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedDateStr
        {
            get { return CreatedDate != null ? CreatedDate.Value.ToString(Const.NormalDateFormat) : string.Empty; }
            set { }
        }
        public string ModifiedDateStr
        {
            get { return ModifiedDate != null ? ModifiedDate.Value.ToString(Const.NormalDateFormat) : string.Empty; }
            set { }
        }
        public string FromDateStr
        {
            get
            {
                return FromDate != null ? FromDate.Value.ToString(Const.NormalDateFormat) : string.Empty;
            }
            set
            {
                FromDate = !string.IsNullOrEmpty(value) ? Utilities.Utils.ConvertStringToDateTime(value, Const.NormalDateFormat) : DateTime.Now;
            }
        }
        public string UntilDateStr
        {
            get { return UntilDate != null ? UntilDate.Value.ToString(Const.NormalDateFormat) : string.Empty; }
            set
            {
                UntilDate = !string.IsNullOrEmpty(value) ? Utilities.Utils.ConvertStringToDateTime(value, Const.NormalDateFormat) : DateTime.Now;
            }
        }
        public string PositionName { get; set; }
        public int Platform { get; set; }
        public string StatusName { get; set; }
        public string PageName { get; set; }
    }
    public class BannerSearchModel
    {
        public BannerSearchModel()
        {
            this.EditItem = new BannerModel();
        }

        public int BannerId { get; set; }
        public string Keyword { get; set; }
        public int Position { get; set; }
        public int Platform { get; set; }
        public int PageId { get; set; }
        public int Status { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListPosition { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListPage { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListStatus { get; set; }
        public IEnumerable<DVG.WIS.Utilities.EnumHelper.Enums> ListPlatform { get; set; }
        public List<BannerModel> ListData { get; set; }
        public BannerModel EditItem { get; set; }
    }
}