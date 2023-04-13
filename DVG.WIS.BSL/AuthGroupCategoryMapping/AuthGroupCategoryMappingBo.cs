using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.DAL.AuthGroupCategoryMapping;

namespace DVG.WIS.Business.AuthGroupCategoryMapping
{
	public class AuthGroupCategoryMappingBo : IAuthGroupCategoryMappingBo
    {
		private IAuthGroupCategoryMappingDal _authGroupCategoryMappingDal;

		public AuthGroupCategoryMappingBo(IAuthGroupCategoryMappingDal AuthGroupCategoryMappingDal)
		{
			this._authGroupCategoryMappingDal = AuthGroupCategoryMappingDal;
		}

		public IEnumerable<Entities.AuthGroupCategoryMapping> GetByGrouId(int id)
		{
			try
			{
				return _authGroupCategoryMappingDal.GetByGrouId(id);
			}
			catch (Exception ex)
			{
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
				return null;
			}
		}

        public bool Insert(Entities.AuthGroupCategoryMapping obj)
        {
            try
            {
                return _authGroupCategoryMappingDal.Insert(obj);
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
                int result = _authGroupCategoryMappingDal.DeleteByGroupId(id);
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
