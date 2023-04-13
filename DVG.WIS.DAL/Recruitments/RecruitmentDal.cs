using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Recruitments
{
    public class RecruitmentDal : ContextBase, IRecruitmentDal
    {
        public IEnumerable<Entities.Recruitment> GetList(string position, string cateName, int status, int pageIndex, int pageSize, out int totalRows)
        {
            string storeName = "Admin_Recruitment_GetList";
            IEnumerable<Entities.Recruitment> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Keyword", position, DataTypes.String)
                        .Parameter("CateName", cateName, DataTypes.String)
                        .Parameter("Status", status, DataTypes.String)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Recruitment>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public Entities.Recruitment GetById(int id)
        {
            string storeName = "Admin_Recruitment_GetById";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.Recruitment>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.Recruitment banner)
        {
            string storeName = "Admin_Recruitment_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", banner.Id, DataTypes.Int32)
                        .Parameter("CateName", banner.CateName, DataTypes.String)
                        .Parameter("Position", banner.Position, DataTypes.String)
                        .Parameter("Address", banner.Address, DataTypes.String)
                        .Parameter("EndDate", banner.EndDate, DataTypes.DateTime)
                        .Parameter("Description", banner.Description, DataTypes.String)
                        .Parameter("Status", banner.Status, DataTypes.Int32)
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
            string storeName = "Admin_Recruitment_DeleteById";
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

        public IEnumerable<Recruitment> GetListFE()
        {
            string storeName = "FE_Recruitment_GetList";
            IEnumerable<Entities.Recruitment> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("EndDate", DateTime.Now, DataTypes.DateTime);
                    lstRet = cmd.QueryMany<Entities.Recruitment>();
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
