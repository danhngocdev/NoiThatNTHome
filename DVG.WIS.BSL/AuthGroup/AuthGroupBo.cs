using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.DAL.AuthGroup;

namespace DVG.WIS.Business.AuthGroup
{
	public class AuthGroupBo : IAuthGroupBo
    {
		private IAuthGroupDal _authGroupDal;

		public AuthGroupBo(IAuthGroupDal AuthGroupDal)
		{
			this._authGroupDal = AuthGroupDal;
		}

		public IEnumerable<Entities.AuthGroup> GetAll()
		{
			try
			{
				return _authGroupDal.GetAll();
			}
			catch (Exception ex)
			{
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
				return null;
			}
		}

        public int Insert(Entities.AuthGroup obj)
        {
            try
            {
                return _authGroupDal.Insert(obj);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, obj);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return 0;
        }

        public Entities.AuthGroup GetById(int id)
        {
            try
            {
                return _authGroupDal.GetById(id);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public ErrorCodes Update(Entities.AuthGroup obj)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (obj == null || obj.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }

                int result = _authGroupDal.Update(obj);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, obj);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _authGroupDal.Delete(id);
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
    }
}
