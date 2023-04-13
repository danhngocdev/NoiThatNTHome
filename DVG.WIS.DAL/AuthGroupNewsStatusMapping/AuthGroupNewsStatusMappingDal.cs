using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroupNewsStatusMapping
{
    public class AuthGroupNewsStatusMappingDal : ContextBase, IAuthGroupNewsStatusMappingDal
    {
        public IEnumerable<Entities.AuthGroupNewsStatusMapping> GetByGrouId(int id)
        {
            string storeName = "Admin_AuthGroupNewsStatusMapping_GetByGroupId";
            IEnumerable<Entities.AuthGroupNewsStatusMapping> lst;
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName);
                    lst = cmd.Parameter("AuthGroupId", id, DataTypes.Int32)
                        .QueryMany<Entities.AuthGroupNewsStatusMapping>();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public bool Insert(Entities.AuthGroupNewsStatusMapping obj)
        {
            string storeName = "Admin_AuthGroupNewsStatusMapping_Insert";
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName)
                        .Parameter("AuthGroupId", obj.AuthGroupId)
                        .Parameter("Status ", obj.Status)
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
            string storeName = "Admin_AuthGroupNewsStatusMapping_DeleteByGroupId";
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
