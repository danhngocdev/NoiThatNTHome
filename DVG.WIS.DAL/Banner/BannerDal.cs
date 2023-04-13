using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Banner
{
    public class BannerDal : ContextBase, IBannerDal
    {
        public IEnumerable<Entities.Banner> GetList(string keyword, int platform, int position, int pageId, int status, int pageIndex, int pageSize, out int totalRows)
        {
            string storeName = "Admin_Banner_GetList_20220901";
            IEnumerable<Entities.Banner> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Keyword", keyword, DataTypes.String)
                        .Parameter("Platform", platform, DataTypes.Int32)
                        .Parameter("Position", position, DataTypes.Int32)
                        .Parameter("PageId", pageId, DataTypes.Int32)
                        .Parameter("Status", status, DataTypes.Int32)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Banner>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");

                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
        public IEnumerable<Entities.Banner> GetAllBanner()
        {
            string storeName = "Admin_Banner_GetAll_20220908";
            IEnumerable<Entities.Banner> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Status", BannerStatusEnum.Show.GetHashCode(), DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Banner>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public Entities.Banner GetById(int id)
        {
            string storeName = "Admin_Banner_GetById";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.Banner>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.Banner banner)
        {
            string storeName = "Admin_Banner_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", banner.Id, DataTypes.Int32)
                        .Parameter("Name", banner.Name, DataTypes.String)
                        .Parameter("Title", banner.Title, DataTypes.String)
                        .Parameter("Embed", banner.Embed, DataTypes.String)
                        .Parameter("Status", banner.Status, DataTypes.Int32)
                        .Parameter("Platform", banner.Platform, DataTypes.Int32)
                        .Parameter("Position", banner.Position, DataTypes.Int32)
                        .Parameter("PageId", banner.PageId, DataTypes.Int32)
                        .Parameter("TargetLink", banner.TargetLink, DataTypes.String)
                        .Parameter("FromDate", banner.FromDate, DataTypes.DateTime)
                        .Parameter("UntilDate", banner.UntilDate, DataTypes.DateTime)
                        .Parameter("CreatedBy", banner.CreatedBy, DataTypes.String)
                        .Parameter("ModifiedDate", banner.ModifiedDate, DataTypes.DateTime)
                        .Parameter("ModifiedBy", banner.ModifiedBy, DataTypes.String)
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
            string storeName = "Admin_Banner_DeleteById";
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

        public int UpdateStatus(Entities.Banner banner)
        {
            string storeName = "Admin_Banner_Update_Status";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", banner.Id, DataTypes.Int32)
                        .Parameter("Status", banner.Status, DataTypes.Int32)
                        .Parameter("ModifiedDate", banner.ModifiedDate, DataTypes.DateTime)
                        .Parameter("ModifiedBy", banner.ModifiedBy, DataTypes.String)
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
