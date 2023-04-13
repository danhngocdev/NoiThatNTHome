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
    public class PersonModel
    {
        public PersonModel()
        {
        }

        public PersonModel(Person model)
        {
            Id = model.Id;
            Name = model.Name;
            Avatar = model.Avatar;
            Description = model.Description;
            Position = model.Position;
            Age = model.Age;
            Score = model.Score;
            Status = model.Status;
            CreatedDate = model.CreatedDate;
            CreatedBy = model.CreatedBy;
            ModifiedDate = model.ModifiedDate;
            ModifiedBy = model.ModifiedBy;
            PageId = model.PageId;
            Priority = model.Priority;
            AvatarStr = !string.IsNullOrEmpty(Avatar) ? CoreUtils.BuildCropAvatar(Avatar, StaticVariable.NoImage, AppSettings.Instance.GetString("CropSizeCMS")) : StaticVariable.NoImage;
        }

        public int Id { set; get; }
        public string Name { set; get; }
        public string Avatar { set; get; }
        public string Description { set; get; }
        public string Position { get; set; }
        public int Age { get; set; }
        public string Score { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int PageId { get; set; }
        public int Priority { get; set; }
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
        public string StatusStr
        {
            get
            {
                return Status == 1 ? "Hoạt động" : "Không hoạt động";
            }
        }

        public string AvatarStr { get; set; }

        public string PageName
        {
            get
            {
                return EnumHelper.Instance.ConvertEnumToList<CustomerSourceEnum>().Where(x => x.Id == PageId).FirstOrDefault().Name;
            }
        }

    }


    public class PersonSearchModel
    {
        public PersonSearchModel()
        {
            this.EditItem = new PersonModel();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public int? PageId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<PersonModel> ListData { get; set; }
        public IEnumerable<EnumHelper.Enums> ListPage { get; set; }
        public PersonModel EditItem { get; set; }
    }
}
