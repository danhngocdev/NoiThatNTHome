using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.PriceList
{
    public class PriceListDal : ContextBase,IPriceListDal
    {

        public PriceListDal()
        {
            _dbPosition = DBPosition.Master;
        }

    
        public int Delete(int id)
        {
            string storeName = "Admin_PriceList_DeleteById";
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

     

        public Entities.PriceList GetById(int id)
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Admin_PriceList_GetById")
                    .Parameter("Id", id)
                    .QuerySingle<WIS.Entities.PriceList>();
            }
        }

        public IEnumerable<Entities.PriceList> GetList(string keyword, int pageIndex, int pageSize,int staus, out int totalRows)
        {
            string storeName = "Admin_PriceList_GetList";
            IEnumerable<Entities.PriceList> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Keyword", keyword, DataTypes.String)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .Parameter("Status", staus, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.PriceList>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.PriceList> GetListFe()
        {
            string storeName = "FE_PriceList_GetList";
            IEnumerable<Entities.PriceList> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName);
                    lstRet = cmd.QueryMany<Entities.PriceList>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.PriceList priceList)
        {
            int numberRecords;
            using (IDbContext context = Context())
            {
                numberRecords = context.StoredProcedure("Admin_PriceList_Update")
                    .Parameter("Id", priceList.Id, DataTypes.Int32)
                    .Parameter("Name", priceList.Name, DataTypes.String)
                    .Parameter("Price", priceList.Price, DataTypes.Decimal)
                    .Parameter("Status", priceList.Status, DataTypes.Int32)
                    .Parameter("Note", priceList.Note, DataTypes.String)
                    .Parameter("Unit", priceList.Unit, DataTypes.Int32)
                    .Execute();
            }
            return numberRecords;
        }

        public int UpdateStatus(Entities.PriceList slider)
        {
            throw new NotImplementedException();
        }
    }
}
