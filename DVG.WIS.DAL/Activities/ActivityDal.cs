using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using FluentData;

namespace DVG.WIS.DAL.Activities
{
    public class ActivityDal : ContextBase, IActivityDal
    {
        public ActivityDal()
        {
            this._dbPosition = DBPosition.Master;
        }

        /// <summary>
        /// Insert news object for Activity
        /// </summary>
        /// <param name="activityModel">Activity Object</param>
        /// <returns>Status insert => true or false</returns>
        public bool Insert(ActivityModel activityModel)
        {
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure("admin_Activity_Insert")
                        .Parameter("ActionTimeStamp", activityModel.ActionTimeStamp)
                        .Parameter("ActionDate", activityModel.ActionDate)
                        .Parameter("UserId", activityModel.UserId)
                        .Parameter("ActionType", activityModel.ActionType)
                        .Parameter("ObjectId ", activityModel.ObjectId)
                        .Parameter("ActionText", activityModel.ActionText);
                    return cmd.Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Activity object by id
        /// </summary>
        /// <param name="id">Activity id</param>
        /// <returns>Activity object</returns>
        public ActivityModel GetById(long id)
        {
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure("admin_Activity_GetById")
                           .Parameter("ActionTimeStamp", id)
                           .QuerySingle<ActivityModel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get list Activity by userid
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="pageIndex">current page index for paging</param>
        /// <param name="pageSize">Item for each page</param>
        /// <param name="totalRow"></param>
        /// <returns>List activity</returns>
        public IEnumerable<ActivityModel> GetListByUser(int userId, int pageIndex, int pageSize, out int totalRow)
        {
            try
            {
                using (var context = Context())
                {
                    var lstActivity = new List<ActivityModel>();
                    IStoredProcedureBuilder cmd = context.StoredProcedure("admin_Activity_GetListByUser")
                        .Parameter("UserId", userId)
                        .Parameter("PageIndex", pageIndex)
                        .Parameter("PageSize", pageSize)
                        .ParameterOut("TotalRow", DataTypes.Int32);

                    lstActivity = cmd.QueryMany<ActivityModel>();
                    totalRow = cmd.ParameterValue<int>("TotalRow");

                    return lstActivity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get list Activity with filter
        /// </summary>
        /// <param name="lstUserId">list user id</param>
        /// <param name="actionType">Action type of user </param>
        /// <param name="fromDate">From Create Date</param>
        /// <param name="toDate">To Create Date</param>
        /// <param name="pageIndex">current page index for paging</param>
        /// <param name="pageSize">Item for each page</param>
        /// <param name="totalRow">Total row found</param>
        /// <returns>List activity</returns>
        public IEnumerable<ActivityModel> GetList(string lstUserId, int actionType, long fromDate, long toDate, int pageIndex, int pageSize, out int totalRow)
        {
            try
            {
                using (var context = Context())
                {
                    var lstActivity = new List<ActivityModel>();
                    IStoredProcedureBuilder cmd = context.StoredProcedure("admin_Activity_GetList_V2")
                        .Parameter("LstUserId", lstUserId)
                        .Parameter("ActionType", actionType)
                        .Parameter("FromDate", fromDate)
                        .Parameter("ToDate", toDate)
                        .Parameter("PageIndex", pageIndex)
                        .Parameter("PageSize", pageSize)
                        .ParameterOut("TotalRow", DataTypes.Int32);

                    lstActivity = cmd.QueryMany<ActivityModel>();
                    totalRow = cmd.ParameterValue<int>("TotalRow");

                    return lstActivity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
