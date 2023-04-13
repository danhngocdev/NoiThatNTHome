using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.DAL.AuthGroupUserMapping;

namespace DVG.WIS.Business.AuthGroupUserMapping
{
	public class AuthGroupUserMappingBo : IAuthGroupUserMappingBo
    {
		private IAuthGroupUserMappingDal _authGroupUserMappingDal;

		public AuthGroupUserMappingBo(IAuthGroupUserMappingDal AuthGroupUserMappingDal)
		{
			this._authGroupUserMappingDal = AuthGroupUserMappingDal;
		}

		public IEnumerable<Entities.AuthGroupUserMapping> GetByUserId(int id)
		{
			try
			{
				return _authGroupUserMappingDal.GetByUserId(id);
			}
			catch (Exception ex)
			{
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
				return null;
			}
		}

        public bool Insert(Entities.AuthGroupUserMapping obj)
        {
            try
            {
                return _authGroupUserMappingDal.Insert(obj);
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
                int result = _authGroupUserMappingDal.DeleteByUserId(id);
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
