using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.DAL.Recruitments;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;

namespace DVG.WIS.Business.Recruitments
{
    public class RecruitmentBo : IRecruitmentBo
    {
        private IRecruitmentDal _recruitmentDal;

        public RecruitmentBo(IRecruitmentDal recruitmentDal)
        {
            this._recruitmentDal = recruitmentDal;
        }
        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _recruitmentDal.Delete(id);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }
        public IEnumerable<RecuitmentFEModel> GetListFE()
        {
            try
            {
                var model = _recruitmentDal.GetListFE();
                IEnumerable<RecuitmentFEModel> lstModel = model.Select(x => new RecuitmentFEModel(x));
                return lstModel;
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }
        public Recruitment GetById(int id)
        {
            try
            {
                return _recruitmentDal.GetById(id);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public IEnumerable<Recruitment> GetList(string position, string cateName, int status, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                return _recruitmentDal.GetList(position, cateName, status, pageIndex, pageSize, out totalRows);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, keyword, position, pageId, blockId, pageIndex, pageSize);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public ErrorCodes Update(Recruitment banner)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (banner == null || banner.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }

                int result = _recruitmentDal.Update(banner);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, banner);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }
    }
}
