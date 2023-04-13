using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.DAL.AuthGroupActionMapping;

namespace DVG.WIS.Business.AuthGroupActionMapping
{
	public class AuthGroupActionMappingBo : IAuthGroupActionMappingBo
    {
		private IAuthGroupActionMappingDal _authGroupActionMappingDal;

		public AuthGroupActionMappingBo(IAuthGroupActionMappingDal AuthGroupActionMappingDal)
		{
			this._authGroupActionMappingDal = AuthGroupActionMappingDal;
		}

		public IEnumerable<Entities.AuthGroupActionMapping> GetByGrouId(int id)
		{
			try
			{
				return _authGroupActionMappingDal.GetByGrouId(id);
			}
			catch (Exception ex)
			{
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
				return null;
			}
		}

        public bool Insert(Entities.AuthGroupActionMapping obj)
        {
            try
            {
                return _authGroupActionMappingDal.Insert(obj);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, obj);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return false;
        }

        public ErrorCodes DeleteByGroupId(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _authGroupActionMappingDal.DeleteByGroupId(id);
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
