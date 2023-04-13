using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Users
{
    public class UserDAL : ContextBase, IUserDAL
    {
        public UserDAL()
        {
            _dbPosition = DBPosition.Master;
        }

        public UserOnList GetById(int userID)
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Users_GetById")
                    .Parameter("Id", userID, DataTypes.Int32)
                    .QuerySingle<UserOnList>();
            }
        }

        public User GetByUserName(string userName)
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Users_GetByUserName")
                    .Parameter("UserName", userName, DataTypes.String)
                    .QuerySingle<User>();
            }
        }

        public User GetByUserNameAndPassword(string userName, string password)
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Users_GetByUserNameAndPassword")
                    .Parameter("UserName", userName, DataTypes.String)
                    .Parameter("Password", password, DataTypes.String)
                    .QuerySingle<User>();
            }
        }

        public User GetUserInfoByEmail(string email)
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Users_GetByEmail")
                    .Parameter("Email", email)
                    .QuerySingle<User>();
            }
        }

        public List<User> GetAll()
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Users_GetAll")
                    .QueryMany<User>();
            }
        }

        public List<User> GetListByKeyword(string keyword, int status, int top = 10)
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Users_GetListByKeyword")
                    .Parameter("Keyword", keyword, DataTypes.String)
                    .Parameter("Status", status, DataTypes.Int32)
                    .Parameter("Top", top, DataTypes.Int32)
                    .QueryMany<User>();
            }
        }

        public int Update(User user)
        {
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure("Users_Update")
                        .Parameter("UserId", user.UserId, DataTypes.Int32)
                        .Parameter("UserName", user.UserName, DataTypes.String)
                        .Parameter("Password", user.Password, DataTypes.String)
                        .Parameter("PasswordQuestion", user.PasswordQuestion, DataTypes.String)
                        .Parameter("PasswordAnswer", user.PasswordAnswer, DataTypes.String)
                        .Parameter("Email", user.Email, DataTypes.String)
                        .Parameter("Mobile", user.Mobile, DataTypes.String)
                        .Parameter("FullName", user.FullName, DataTypes.String)
                        .Parameter("Avatar", user.Avatar, DataTypes.String)
                        //.Parameter("RankId", user.RankId, DataTypes.Int32)
                        .Parameter("Gender", user.Gender, DataTypes.Boolean)
                        .Parameter("Desciption", user.Desciption, DataTypes.String)
                        .Parameter("Signature", user.Signature, DataTypes.String)
                        .Parameter("Address", user.Address, DataTypes.String)
                        .Parameter("CityId", user.CityId, DataTypes.Byte)
                        .Parameter("Birthday", user.Birthday, DataTypes.Int64)
                        .Parameter("Skype", user.Skype, DataTypes.String)
                        .Parameter("Facebook", user.Facebook, DataTypes.String)
                        .Parameter("Google", user.Google, DataTypes.String)
                        .Parameter("GoogleId", user.GoogleId, DataTypes.String)
                        .Parameter("FacebookId", user.FacebookId, DataTypes.String)
                        .Parameter("Transporter", user.Transporter, DataTypes.String)
                        .Parameter("Status", user.Status, DataTypes.Int32)
                        .Parameter("UserType", user.UserType, DataTypes.Int64)
                        .Parameter("CreatedDate", user.CreatedDate, DataTypes.DateTime)
                        .Parameter("CreatedDateSpan", user.CreatedDateSpan, DataTypes.Int64)
                        .QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public int UpdateLastLogin(int userId, long lastLogin)
        {
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure("Users_Update_LastLogin")
                        .Parameter("UserId", userId, DataTypes.Int32)
                        .Parameter("LastLogin", lastLogin, DataTypes.Int64)
                        .QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public IEnumerable<UserOnList> GetList(string keyword, int? authGroupId = 0, int? pageIndex = 1, int? pageSize = 15)
        {
            var storeName = "Users_GetList_201806051200";
            using (var context = Context())
            {
                return context.StoredProcedure(storeName)
                    .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                    .Parameter("PageSize", pageSize, DataTypes.Int32)
                    .Parameter("Keyword", keyword, DataTypes.String)
                    .Parameter("AuthGroupId", authGroupId, DataTypes.Int32)
                    .QueryMany<UserOnList>();
            }
        }

        public IEnumerable<User> GetListInBank(int bankId, int status, int userType)
        {
            string storeName = "FE_Users_GetListInBank";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("BankId", bankId, DataTypes.Int32)
                        .Parameter("Status", status, DataTypes.Int32)
                        .Parameter("Type", userType, DataTypes.Int32)
                        .QueryMany<User>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }
    }
}
