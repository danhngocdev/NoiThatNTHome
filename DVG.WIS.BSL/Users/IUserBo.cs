using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Users
{
    public interface IUserBo
    {
        UserOnList GetById(int userId);
        Entities.User GetByUserName(string userName);
        Entities.User ValidateLogin(string userName, string password);
        Entities.User GetUserInfoByEmail(string email);
        Entities.User GetUserInfoByAccountName(string accountName);
        ErrorCodes Update(Entities.User user, ref int userId);
        ErrorCodes Update(Entities.User user);
        ErrorCodes UpdateLastLogin(int userId);
        ErrorCodes ChangePassword(string username, string currentPassword, string passsword, string confirmPassword);
        IEnumerable<UserOnList> GetList(string keyword, int? authGroupId = 0, int? pageIndex = 1, int? pageSize = 15);
        IEnumerable<Entities.User> GetListInBank(int bankId, int status, int userType);

        string GenerateEmailCreateAcount(string loginLink, string fullName, string userName, string pass,
            string accountType);
        List<Entities.User> GetAll();
    }
}
