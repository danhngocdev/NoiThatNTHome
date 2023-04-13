using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.Core.Enums;
using DVG.WIS.Core;

namespace DVG.WIS.PublicModel.CMS
{
    public class UsersModel
    {
        private string _birthDay;

        public UsersModel() { }


        public UsersModel(User users)
        {
            this.UserId = users.UserId;
            this.UserName = users.UserName;
            this.Password = users.Password;
            this.Email = users.Email;
            this.Mobile = users.Mobile;
            this.FullName = users.FullName;
            this.Avatar = users.Avatar;
            this.Status = users.Status;
            this.CreatedDate = users.CreatedDate;
            this.CreatedDateSpan = users.CreatedDateSpan;
            this.Gender = users.Gender;
            this.Address = users.Address;
            this.CityId = users.CityId;
            this.Birthday = users.Birthday;
            this.PasswordQuestion = users.PasswordQuestion;
            this.PasswordAnswer = users.PasswordAnswer;
            this.Desciption = users.Desciption;
            this.Signature = users.Signature;
            this.LastLogin = users.LastLogin;
            this.LastPasswordChange = users.LastPasswordChange;
            this.LastUpdate = users.LastUpdate;
            this.Skype = users.Skype;
            this.Facebook = users.Facebook;
            this.Google = users.Google;
            this.GoogleId = users.GoogleId;
            this.FacebookId = users.FacebookId;
            this.Transporter = users.Transporter;
            this.UserType = users.UserType;

        }
        public UsersModel(UserOnList users)
        {
            this.UserId = users.UserId;
            this.UserName = users.UserName;
            this.Password = users.Password;
            this.Email = users.Email;
            this.Mobile = users.Mobile;
            this.FullName = users.FullName;
            this.Avatar = users.Avatar;
            this.Status = users.Status;
            this.CreatedDate = users.CreatedDate;
            this.CreatedDateSpan = users.CreatedDateSpan;
            this.Gender = users.Gender;
            this.Address = users.Address;
            this.CityId = users.CityId;
            this.Birthday = users.Birthday;
            this.PasswordQuestion = users.PasswordQuestion;
            this.PasswordAnswer = users.PasswordAnswer;
            this.Desciption = users.Desciption;
            this.Signature = users.Signature;
            this.LastLogin = users.LastLogin;
            this.LastPasswordChange = users.LastPasswordChange;
            this.LastUpdate = users.LastUpdate;
            this.Skype = users.Skype;
            this.Facebook = users.Facebook;
            this.Google = users.Google;
            this.GoogleId = users.GoogleId;
            this.FacebookId = users.FacebookId;
            this.Transporter = users.Transporter;
            this.UserType = users.UserType;
            this.BankId = users.BankId;
            this.BankBranchId = users.BankBranchId;
            this.ProvinceId = users.ProvinceId;
            this.DistrictId = users.DistrictId;
            this.TotalRows = users.TotalRows;
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public int Status { get; set; }

        public string StatusBack
        {
            get
            {
                switch (Status)
                {
                    case (int)UserStatusAdmin.Deactived:
                        return StringUtils.GetEnumDescription(UserStatusAdmin.Deactived);
                    case (int)UserStatusAdmin.Actived:
                        return StringUtils.GetEnumDescription(UserStatusAdmin.Actived);
                    case (int)UserStatusAdmin.Deleted:
                        return StringUtils.GetEnumDescription(UserStatusAdmin.Deleted);;
                    default:
                        return string.Empty;
                }
            }
        }

        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedDateSpan { get; set; }

        public string CreatedDateStr
        {
            get
            {
                if (CreatedDate.HasValue)
                {
                    return CreatedDate.Value.ToString("dd/MM/yyyy");
                }
                return string.Empty;
            }
        }

        public Nullable<bool> Gender { get; set; }

        public string GenderStr
        {
            get
            {
                if (Gender.HasValue && Gender.Value)
                {
                    return Const.Male;
                }
                return Const.Female;
            }
        }

        public string Address { get; set; }
        public Nullable<byte> CityId { get; set; }
        public Nullable<long> Birthday { get; set; }

        public string BirthdayStr
        {
            get
            {
                if (Birthday.HasValue && Birthday.Value > 0)
                {
                    return Utilities.Utils.UnixTimeStampToDateTime(Birthday.Value).ToString("dd/MM/yyyy");
                }
                return string.Empty;
            }
            set { Birthday = Utilities.Utils.DateTimeToUnixTime(Utilities.Utils.ConvertStringToDateTime(value, "dd/MM/yyyy")); }
        }

        public int TotalRows { get; set; }


        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public string Desciption { get; set; }
        public string Signature { get; set; }
        public Nullable<long> LastLogin { get; set; }
        public Nullable<long> LastPasswordChange { get; set; }
        public Nullable<long> LastUpdate { get; set; }
        public string Skype { get; set; }
        public string Facebook { get; set; }
        public string Google { get; set; }
        public string GoogleId { get; set; }
        public string FacebookId { get; set; }
        public string Transporter { get; set; }
        public Nullable<long> UserType { get; set; }
        public Nullable<int> AuthGroupId { get; set; }

        public List<SelectListItem> UsersTypeList { get; set; }

        public int BankId { get; set; }
        public int BankBranchId { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
    }

    public class UsersSearchModel
    {
        public string Keyword { get; set; }
        public int UserType { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class UserOnControll
    {
        public UserOnControll() { }
        public UserOnControll(Entities.User user)
        {
            this.UserId = user.UserId;
            this.UserName = user.UserName;
            this.FullName = user.FullName;
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}