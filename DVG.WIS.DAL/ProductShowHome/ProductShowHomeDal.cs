using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.ProductShowHome
{
    public class ProductShowHomeDal : ContextBase,IProductShowHomeDal
    {
        public ProductShowHomeDal()
        {
            _dbPosition = DBPosition.Master;

        }
        public int Delete(int id)
        {
            string storeName = "Admin_ProductShowHome_DeleteById";
            try
            {
                using (IDbContext context = Context())
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

        public Entities.ProductShowHome GetById(int id)
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Admin_ProductShowHome_GetById")
                    .Parameter("Id", id)
                    .QuerySingle<WIS.Entities.ProductShowHome>();
            }
        }

        public IEnumerable<Entities.ProductShowHome> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows)
        {
            string storeName = "Admin_ProductShowHome_GetList";
            IEnumerable<Entities.ProductShowHome> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Keyword", keyword, DataTypes.String)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .Parameter("Status", status, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.ProductShowHome>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.ProductShowHome> GetListFE()
        {
            string storeName = "FE_ProductShowHome_GetAll";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .QueryMany<WIS.Entities.ProductShowHome>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.ProductShowHome productShowHome)
        {
            int numberRecords;
            using (IDbContext context = Context())
            {
                numberRecords = context.StoredProcedure("Admin_ProductShowHome_Update")
                    .Parameter("Id", productShowHome.Id, DataTypes.Int32)
                    .Parameter("Title", productShowHome.Title, DataTypes.String)
                    .Parameter("CategoryId", productShowHome.CategoryId, DataTypes.Int32)
                    .Parameter("Status", productShowHome.Status, DataTypes.Int32)
                    .Parameter("Limit", productShowHome.Limit, DataTypes.Int32)
                    .Execute();
            }
            return numberRecords;
        }
    }
}
