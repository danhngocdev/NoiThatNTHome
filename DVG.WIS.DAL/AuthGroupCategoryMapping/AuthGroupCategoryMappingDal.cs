using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;

namespace DVG.WIS.DAL.AuthGroupCategoryMapping
{
    public class AuthGroupCategoryMappingDal : ContextBase, IAuthGroupCategoryMappingDal
    {
        public IEnumerable<Entities.AuthGroupCategoryMapping> GetByGrouId(int id)
        {
            string storeName = "Admin_AuthGroupCategoryMapping_GetByGroupId";
            IEnumerable<Entities.AuthGroupCategoryMapping> lst;
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName);
                    lst = cmd.Parameter("AuthGroupId", id, DataTypes.Int32)
                        .QueryMany<Entities.AuthGroupCategoryMapping>();
                    return lst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public bool Insert(Entities.AuthGroupCategoryMapping obj)
        {
            string storeName = "Admin_AuthGroupCategoryMapping_Insert";
            try
            {
                using (var context = Context())
                {
                    var cmd = context.StoredProcedure(storeName)
                        .Parameter("AuthGroupId", obj.AuthGroupId)
                        .Parameter("NewsType", obj.NewsType)
                        .Parameter("CategoryId ", obj.CategoryId)
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
            string storeName = "Admin_AuthGroupCategoryMapping_DeleteByGroupId";
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
