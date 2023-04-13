using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroupActionMapping
{
    public class AuthGroupActionMappingDal : ContextBase, IAuthGroupActionMappingDal
    {
        public IEnumerable<Entities.AuthGroupActionMapping> GetByGrouId(int id)
        {
            string storeName = "Admin_AuthGroupActionMapping_GetByGroupId";
            IEnumerable<Entities.AuthGroupActionMapping> lst;
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName);
                    lst = cmd.Parameter("AuthGroupId", id, DataTypes.Int32)
                        .QueryMany<Entities.AuthGroupActionMapping>();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public bool Insert(Entities.AuthGroupActionMapping obj)
        {
            string storeName = "Admin_AuthGroupActionMapping_Insert";
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName)
                        .Parameter("AuthGroupId", obj.AuthGroupId)
                        .Parameter("AthActionId ", obj.AthActionId)
                        .Parameter("CreatedDate ", DateTime.Now)
                        .Parameter("CreatedBy ", obj.CreatedBy);
                    return cmd.Execute() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int DeleteByGroupId(int id)
        {
            string storeName = "Admin_AuthGroupActionMapping_DeleteByGroupId";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("AuthGroupId", id, DataTypes.Int32)
                        .QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
    }
}
