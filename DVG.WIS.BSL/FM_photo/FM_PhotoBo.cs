using DVG.WIS.DAL.FM_Photo;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.FM_photo
{
    public class FM_PhotoBo : IFM_PhotoBo
    {
        private IFM_PhotoDal _fmPhotoDal;
        public FM_PhotoBo(IFM_PhotoDal fmPhotoDal)
        {
            this._fmPhotoDal = fmPhotoDal;
        }

        public IEnumerable<WIS.Entities.FM_PhotoOnList> GetList(string keyword, string userName, int pageIndex, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                IEnumerable<FM_PhotoOnList> lst = _fmPhotoDal.GetList(keyword,  userName, pageIndex, pageSize, fromDate, toDate);
                return lst;
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, keyword, userName, pageIndex, pageSize, fromDate, toDate);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public ErrorCodes Update(WIS.Entities.FM_Photo obj)
        {
            try
            {
                int numberRow = _fmPhotoDal.Update(obj);
                return numberRow > 0 ? ErrorCodes.Success : ErrorCodes.BusinessError;
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, obj);
                Logger.WriteLog(Logger.LogType.Error, ex);
                return ErrorCodes.Exception;
            }
        }

        public WIS.Entities.FM_Photo GetByFileName(string fileName)
        {
            try
            {
                return _fmPhotoDal.GetByFileName(fileName);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, fileName);
                Logger.WriteLog(Logger.LogType.Error, ex);
                return new WIS.Entities.FM_Photo();
            }
        }
    }
}
