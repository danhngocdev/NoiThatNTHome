using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using FluentData;
using DVG.WIS.Utilities;

namespace DVG.WIS.DAL.Category
{
    public class CategoryDalFE : ContextBase, ICategoryDalFE
    {
        public WIS.Entities.Category GetById(int cateId)
        {
            string storeName = "FE_Category_GetById";

            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", cateId, DataTypes.Int32)
                        .QuerySingle<WIS.Entities.Category>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        public IEnumerable<WIS.Entities.Category> GetAllByParent(int parentId, int status)
        {
            string storeName = "FE_Category_GetAllByParent";

            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("ParentId", parentId, DataTypes.Int32)
                        .Parameter("Status", status, DataTypes.Int32)
                        .QueryMany<WIS.Entities.Category>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        public IEnumerable<WIS.Entities.Category> GetListByParent(int parentId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WIS.Entities.Category> GetListAll()
        {
            string storeName = "FE_Category_GetAll";

            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .QueryMany<WIS.Entities.Category>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }
    }
}
