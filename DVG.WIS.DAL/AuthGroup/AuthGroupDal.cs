using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroup
{
    public class AuthGroupDal : ContextBase, IAuthGroupDal
    {
        public IEnumerable<Entities.AuthGroup> GetAll()
        {
            string storeName = "Admin_AuthGroup_GetAll";
            IEnumerable<Entities.AuthGroup> lst;
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName);
                    lst = cmd.QueryMany<Entities.AuthGroup>();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Insert(Entities.AuthGroup obj)
        {
            string storeName = "Admin_AuthGroup_Insert";
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName)
                        .Parameter("Name", obj.Name)
                        .Parameter("Status ", obj.Status)
                        .Parameter("CreatedDate ", DateTime.Now)
                        .Parameter("ModifiedDate ", DateTime.Now)
                        .Parameter("CreatedBy ", obj.CreatedBy)
                        .Parameter("ModifiedBy ", obj.CreatedBy);
                    return cmd.QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public Entities.AuthGroup GetById(int id)
        {
            string storeName = "Admin_AuthGroup_GetById";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.AuthGroup>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.AuthGroup obj)
        {
            string storeName = "Admin_AuthGroup_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", obj.Id, DataTypes.Int32)
                        .Parameter("Name", obj.Name, DataTypes.String)
                        .Parameter("Status ", obj.Status, DataTypes.Int16)
                        .Parameter("ModifiedDate ", DateTime.Now)
                        .Parameter("ModifiedBy ", obj.CreatedBy)
                        .QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
        public int Delete(int id)
        {
            string storeName = "Admin_AuthGroup_Delete";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
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
