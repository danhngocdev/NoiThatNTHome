using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Users
{
    public interface IUserDAL
    {
        UserOnList GetById(int userId);
        Entities.User GetByUserName(string userName);
        Entities.User GetByUserNameAndPassword(string userName, string password);
        Entities.User GetUserInfoByEmail(string email);
        List<Entities.User> GetAll();
        List<Entities.User> GetListByKeyword(string keyword, int status, int top = 10);
        int Update(Entities.User user);
        int UpdateLastLogin(int userId, long lastLogin);
        IEnumerable<UserOnList> GetList(string keyword, int? authGroupId = 0, int? pageIndex = 1, int? pageSize = 15);
        IEnumerable<Entities.User> GetListInBank(int bankId, int status, int userType);
    }
}
