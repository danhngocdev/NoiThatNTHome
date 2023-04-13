using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Persons
{
    public class PersonDal : ContextBase, IPersonDal
    {
        public IEnumerable<Entities.Person> GetList(string name, int? pageId, int? status, int pageIndex, int pageSize, out int totalRows)
        {
            string storeName = "Admin_Person_GetList";
            IEnumerable<Entities.Person> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Name", name, DataTypes.String)
                        .Parameter("PageId", pageId, DataTypes.Int32)
                        .Parameter("Status", status, DataTypes.String)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Person>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public Entities.Person GetById(int id)
        {
            string storeName = "Admin_Person_GetById";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.Person>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.Person banner)
        {
            string storeName = "Admin_Person_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", banner.Id, DataTypes.Int32)
                        .Parameter("Name", banner.Name, DataTypes.String)
                        .Parameter("Avatar", banner.Avatar, DataTypes.String)
                        .Parameter("Description", banner.Description, DataTypes.String)
                        .Parameter("Position", banner.Position, DataTypes.String)
                        .Parameter("Age", banner.Age, DataTypes.Int32)
                        .Parameter("Score", banner.Score, DataTypes.String)
                        .Parameter("Status", banner.Status, DataTypes.Int32)
                        .Parameter("CreatedBy", banner.CreatedBy, DataTypes.String)
                        .Parameter("ModifiedDate", banner.ModifiedDate, DataTypes.DateTime)
                        .Parameter("ModifiedBy", banner.ModifiedBy, DataTypes.String)
                        .Parameter("PageId", banner.PageId, DataTypes.Int32)
                        .Parameter("Priority", banner.Priority, DataTypes.Int32)
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
            string storeName = "Admin_Person_DeleteById";
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

        public IEnumerable<Person> GetListFE()
        {
            string storeName = "FE_Person_GetList";
            IEnumerable<Entities.Person> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("EndDate", DateTime.Now, DataTypes.DateTime);
                    lstRet = cmd.QueryMany<Entities.Person>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Person> GetListFE(int pageId, int status, int limit)
        {
            string storeName = "FE_Person_GetList";
            IEnumerable<Entities.Person> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("PageId", pageId, DataTypes.Int32)
                        .Parameter("Status", status, DataTypes.String)
                        .Parameter("PageSize", limit, DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Person>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
    }
}
