using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using DVG.WIS.Utilities;
using FluentData;

namespace DVG.WIS.DAL.Customers
{
    public class CustomerDal : ContextBase, ICustomerDal
    {
        public IEnumerable<Entities.Customer> GetList(CustomerCondition customer, out int totalRows)
        {
            string storeName = "Admin_Customer_GetList";
            IEnumerable<Entities.Customer> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("StartDate", customer.StartDate, DataTypes.Int64)
                        .Parameter("EndDate", customer.EndDate, DataTypes.Int64)
                        .Parameter("PageIndex", customer.PageIndex, DataTypes.Int32)
                        .Parameter("PageSize", customer.PageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Customer>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public Entities.Customer GetById(int id)
        {
            string storeName = "Admin_Customer_GetById";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.Customer>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.Customer banner)
        {
            string storeName = "Admin_Customer_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", banner.Id, DataTypes.Int32)
                        .Parameter("Name", banner.Name, DataTypes.String)
                        .Parameter("Address", banner.Address, DataTypes.String)
                        .Parameter("Phone", banner.Phone, DataTypes.String)
                        .Parameter("Email", banner.Email, DataTypes.String)
                        .Parameter("Title", banner.Title, DataTypes.String)
                        .Parameter("Description", banner.Description, DataTypes.String)
                        .Parameter("CreatedDateSpan", DateTime.Now.Ticks, DataTypes.String)
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
            string storeName = "Admin_Customer_DeleteById";
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

        public IEnumerable<Customer> ExportExcel(DateTime? startDate = null, DateTime? endDate = null)
        {
            string storeName = "Admin_Customer_Export";
            IEnumerable<Entities.Customer> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("StartDate", startDate.HasValue ? startDate.Value.Ticks : 0, DataTypes.Int64)
                        .Parameter("EndDate", endDate.HasValue ? endDate.Value.Ticks : 0, DataTypes.Int64);

                    lstRet = cmd.QueryMany<Entities.Customer>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int UpdateSubcribe(Entities.Subscribe banner)
        {
            string storeName = "FE_Subscribe_Insert";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Email", banner.Email, DataTypes.String)
                        .Parameter("CreatedDateSpan", DateTime.Now.Ticks, DataTypes.String)
                        .Parameter("Status", banner.Id, DataTypes.Int32)
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
