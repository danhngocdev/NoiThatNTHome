using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.Subscribe;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Subscribe
{
    public class SubscribeBo : ISubscribeBo
    {
        private ISubscribeDal _subscribeBoDal;
        public SubscribeBo(ISubscribeDal subscribeDal)
        {
            this._subscribeBoDal = subscribeDal;
        }
        public List<Entities.Subscribe> GetList(string email, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                return _subscribeBoDal.GetList(email, pageIndex, pageSize, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }
        //public List<Entities.SubcribeDetail> GetDetailById(int id)
        //{
        //	try
        //	{
        //		return _subscribeBoDal.GetDetailById(id);
        //	}
        //	catch (Exception ex)
        //	{
        //		Logger.WriteLog(Logger.LogType.Error, ex.ToString());
        //		return null;
        //	}
        //}
        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _subscribeBoDal.Delete(id);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }
        public ErrorCodes UpdateStatus(int id, string action)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                var status = 0;
                if (action == "ban")
                {
                    status = SubscribeStatusEnum.Banned.GetHashCode();
                }
                else if (action == "unban")
                {
                    status = SubscribeStatusEnum.Active.GetHashCode();
                }
                int result = _subscribeBoDal.UpdateStatus(id, status);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }
    }
}
