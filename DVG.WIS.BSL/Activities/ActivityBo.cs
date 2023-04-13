using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.DAL.Activities;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;

namespace DVG.WIS.Business.Activities
{
    public class ActivityBo : IActivityBo
    {
        private IActivityDal _activityDal;

        public ActivityBo(IActivityDal activityDal)
        {
            this._activityDal = activityDal;
        }

        public bool Insert(ActivityModel activityModel)
        {
            try
            {
                return _activityDal.Insert(activityModel);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return false;
        }

        public ActivityModel GetById(long id)
        {
            return _activityDal.GetById(id);
        }

        public IEnumerable<ActivityModel> GetListByUser(int userId, int pageIndex, int pageSize, out int totalRow)
        {
            return _activityDal.GetListByUser(userId, pageIndex, pageSize, out totalRow);
        }

        public IEnumerable<ActivityModel> GetList(string lstUserId, int actionType, long fromDate, long toDate, int pageIndex, int pageSize, out int totalRow)
        {
            return _activityDal.GetList(lstUserId, actionType, fromDate, toDate, pageIndex, pageSize, out totalRow);
        }
    }
}
