using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;

namespace DVG.WIS.DAL.Activities
{
    public interface IActivityDal
    {
        bool Insert(ActivityModel activityModel);
        ActivityModel GetById(long id);
        IEnumerable<ActivityModel> GetListByUser(int userId, int pageIndex, int pageSize, out int totalRow);
        IEnumerable<ActivityModel> GetList(string lstUserId, int actionType, long fromDate, long toDate, int pageIndex, int pageSize, out int totalRow);
    }
}
