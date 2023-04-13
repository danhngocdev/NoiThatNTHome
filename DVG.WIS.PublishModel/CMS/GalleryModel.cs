using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
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
    public class GalleryModel
    {
        public GalleryModel()
        {
        }

        public GalleryModel(Gallery model)
        {
            Id = model.Id;
            Url = model.Url;
            Priority = model.Priority;
            Status = model.Status;
            CreatedDate = model.CreatedDate;
        }
        public int Id { set; get; }
        public string Url { set; get; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Priority { get; set; }
        public string CreatedDateStr
        {
            get
            {
                return CreatedDate.ToString(Const.DateTimeFormatAdmin);
            }
        }
        public string StatusStr
        {
            get
            {
                return Status == 1 ? "Hoạt động" : "Không hoạt động";
            }
        }

        public string AvatarStr
        {
            get
            {
                return !string.IsNullOrEmpty(Url) ? CoreUtils.BuildCropAvatar(Url.TrimStart('/'), StaticVariable.NoImage, AppSettings.Instance.GetString("CropSizeCMS")) : StaticVariable.NoImage;
            }
        }
    }


    public class GallerySearchModel
    {
        public GallerySearchModel()
        {
            this.EditItem = new GalleryModel();
        }
        public int Id { get; set; }
        public int Status { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<GalleryModel> ListData { get; set; }
        public GalleryModel EditItem { get; set; }
    }
}
