using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using FluentData;

namespace DVG.WIS.DAL.Galleries
{
    public class GalleryDal : ContextBase, IGalleryDal
    {
        public IEnumerable<Entities.Gallery> GetList(int status, int pageIndex, int pageSize, out int totalRows)
        {
            string storeName = "Admin_Gallery_GetList";
            IEnumerable<Entities.Gallery> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Status", status, DataTypes.Int32)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Gallery>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public Entities.Gallery GetById(int id)
        {
            string storeName = "Admin_Gallery_GetById";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.Gallery>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.Gallery banner)
        {
            string storeName = "Admin_Gallery_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", banner.Id, DataTypes.Int32)
                        .Parameter("Url", banner.Url, DataTypes.String)
                        .Parameter("Priority", banner.Priority, DataTypes.Int32)
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
            string storeName = "Admin_Gallery_DeleteById";
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

        public IEnumerable<Gallery> GetListFE(int status, int pageSize)
        {
            string storeName = "FE_Gallery_GetList";
            IEnumerable<Entities.Gallery> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Status", status, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Gallery>();
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
