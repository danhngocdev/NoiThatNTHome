using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.DAL.AuthAction;

namespace DVG.WIS.Business.AuthAction
{
	public class AuthActionBo : IAuthActionBo
    {
		private IAuthActionDal _authActionDal;

		public AuthActionBo(IAuthActionDal authActionDal)
		{
			this._authActionDal = authActionDal;
		}

		public IEnumerable<Entities.AuthAction> GetAll()
		{
			try
			{
				return _authActionDal.GetAll();
			}
			catch (Exception ex)
			{
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
				return null;
			}
		}

        public bool Insert(Entities.AuthAction obj)
        {
            try
            {
                return _authActionDal.Insert(obj);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return false;
        }
    }
}
