using System;
using DVG.WIS.Utilities.SignalrHelper;

namespace DVG.WIS.Entities
{
    [Serializable]
    public class UserLogin
    {
        public UserLogin() { }

        public UserLogin(User userInfo)
        {
            if (userInfo != null)
            {
                this.UserId = userInfo.UserId;
                this.UserName = userInfo.UserName;
                this.Email = userInfo.Email;
                this.FullName = userInfo.FullName;
                this.Mobile = userInfo.Mobile;
                this.UserName = userInfo.UserName;
                this.Avatar = userInfo.Avatar;
                this.DisplayName = userInfo.FullName;
                this.CreatedDate = userInfo.CreatedDate;
                this.CreatedDateSpan = userInfo.CreatedDateSpan;
                this.Signature = userInfo.Signature;
                this.RoleId = userInfo.RoleId;
            }

        }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string Signature { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<long> CreatedDateSpan { get; set; }
        public string ConnectionId { get; set; }
        public string Url { get; set; }
        public string EncryptId
        {
            get { return UserId > 0 ? EncryptUtils.Encrypt(this.UserId.ToString()) : string.Empty; }
        }
    }

    public class UserOnList : User
    {
        public int BankId { get; set; }
        public int BankBranchId { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int TotalRows { get; set; }
    }
}
