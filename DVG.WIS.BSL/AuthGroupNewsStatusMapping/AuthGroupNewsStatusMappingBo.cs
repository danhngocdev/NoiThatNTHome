using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.DAL.AuthGroupNewsStatusMapping;

namespace DVG.WIS.Business.AuthGroupNewsStatusMapping
{
	public class AuthGroupNewsStatusMappingBo : IAuthGroupNewsStatusMappingBo
    {
		private IAuthGroupNewsStatusMappingDal _AuthGroupNewsStatusMappingDal;

		public AuthGroupNewsStatusMappingBo(IAuthGroupNewsStatusMappingDal AuthGroupNewsStatusMappingDal)
		{
			this._AuthGroupNewsStatusMappingDal = AuthGroupNewsStatusMappingDal;
		}

		public IEnumerable<Entities.AuthGroupNewsStatusMapping> GetByGrouId(int id)
		{
			try
			{
				return _AuthGroupNewsStatusMappingDal.GetByGrouId(id);
			}
			catch (Exception ex)
			{
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
				return null;
			}
		}

        public bool Insert(Entities.AuthGroupNewsStatusMapping obj)
        {
            try
            {
                return _AuthGroupNewsStatusMappingDal.Insert(obj);
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
                int result = _AuthGroupNewsStatusMappingDal.DeleteByGroupId(id);
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
