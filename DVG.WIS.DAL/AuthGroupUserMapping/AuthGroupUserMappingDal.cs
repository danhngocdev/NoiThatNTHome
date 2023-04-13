using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroupUserMapping
{
    public class AuthGroupUserMappingDal : ContextBase, IAuthGroupUserMappingDal
    {
        public IEnumerable<Entities.AuthGroupUserMapping> GetByUserId(int id)
        {
            string storeName = "Admin_AuthGroupUserMapping_GetByUserId";
            IEnumerable<Entities.AuthGroupUserMapping> lst;
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName);
                    lst = cmd.Parameter("UserId", id, DataTypes.Int32)
                        .QueryMany<Entities.AuthGroupUserMapping>();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public bool Insert(Entities.AuthGroupUserMapping obj)
        {
            string storeName = "Admin_AuthGroupUserMapping_Insert";
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName)
                        .Parameter("AuthGroupId", obj.AuthGroupId)
                        .Parameter("UserId ", obj.UserId)
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

        public int DeleteByUserId(int id)
        {
            string storeName = "Admin_AuthGroupUserMapping_DeleteByUserId";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("UserId", id, DataTypes.Int32)
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
