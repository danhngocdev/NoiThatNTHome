using System;
using DVG.WIS.Entities;

namespace DVG.CMS.Models
{
    public class AccountModel
    {
    }

    public class UserModel
    {
        public UserModel() { }

        public UserModel(User user)
        {
            this.UserId = user.UserId;
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public int Status { get; set; }
        public int RankId { get; set; }
        public int NumPosts { get; set; }
        public int Points { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedDateSpan { get; set; }
        public bool Gender { get; set; }
        public string Desciption { get; set; }
        public string Signature { get; set; }
        public string Address { get; set; }
        public Nullable<byte> CityId { get; set; }
        public Nullable<long> Birthday { get; set; }
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
    }

    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}