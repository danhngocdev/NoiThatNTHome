using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.DAL.Persons;
using DVG.WIS.DAL.Recruitments;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;

namespace DVG.WIS.Business.Persons
{
    public class PersonBo : IPersonBo
    {
        private IPersonDal _personDal;

        public PersonBo(IPersonDal personDal)
        {
            _personDal = personDal;
        }

        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _personDal.Delete(id);
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
        public Person GetById(int id)
        {
            try
            {
                return _personDal.GetById(id);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public IEnumerable<Person> GetList(string name, int? pageId, int? status, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                return _personDal.GetList(name,pageId, status, pageIndex, pageSize, out totalRows);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, keyword, position, pageId, blockId, pageIndex, pageSize);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public IEnumerable<Person> GetListFE(int pageId, int status, int limit)
        {
            try
            {
                return _personDal.GetListFE(pageId,status, limit);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, keyword, position, pageId, blockId, pageIndex, pageSize);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public ErrorCodes Update(Person banner)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (banner == null || banner.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }

                int result = _personDal.Update(banner);
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
