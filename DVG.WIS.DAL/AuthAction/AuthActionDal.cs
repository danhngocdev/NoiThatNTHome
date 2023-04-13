using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthAction
{
    public class AuthActionDal : ContextBase, IAuthActionDal
    {
        public IEnumerable<Entities.AuthAction> GetAll()
        {
            string storeName = "Admin_AuthAction_GetAll";
            IEnumerable<Entities.AuthAction> lst;
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName);
                    lst = cmd.QueryMany<Entities.AuthAction>();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public bool Insert(Entities.AuthAction obj)
        {
            string storeName = "Admin_AuthAction_Insert";
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName)
                        .Parameter("KeyName", obj.KeyName)
                        .Parameter("Description", obj.Description)
                        .Parameter("Controller", obj.Controller)
                        .Parameter("Action", obj.Action)
                        .Parameter("Status ", obj.Status)
                        .Parameter("CreatedDate ", DateTime.Now);
                    return cmd.Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
    }
}
