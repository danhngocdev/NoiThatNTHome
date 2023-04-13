using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.Video
{
    public class VideoDal : ContextBase, IVideoDal
    {

        public VideoDal()
        {
            _dbPosition = DBPosition.Master;
        }

        public int Delete(int id)
        {
            string storeName = "Admin_Video_DeleteById";
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

        public Entities.Video GetById(int id)
        {
            using (var context = Context())
            {
                return context.StoredProcedure("Admin_Video_GetById")
                    .Parameter("Id", id)
                    .QuerySingle<WIS.Entities.Video>();
            }
        }

        public IEnumerable<Entities.Video> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows)
        {
            string storeName = "Admin_Video_GetList";
            IEnumerable<Entities.Video> lstRet;
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
                    lstRet = cmd.QueryMany<Entities.Video>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.Video> GetListVideoTop(int top)
        {
            string storeName = "FE_Video_GetTop";
            IEnumerable<Entities.Video> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Top", top, DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.Video>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.Video video)
        {
            int numberRecords;
            string storeName = "Admin_Video_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    numberRecords = context.StoredProcedure("Admin_Video_Update")
                        .Parameter("Id", video.Id, DataTypes.Int32)
                        .Parameter("Title", video.Title, DataTypes.String)
                        .Parameter("VideoUrl", video.VideoUrl, DataTypes.String)
                        .Parameter("Avatar", video.Avatar ?? string.Empty, DataTypes.String)
                        .Parameter("Link", video.Link ?? string.Empty, DataTypes.String)
                        .Parameter("Status", video.Status, DataTypes.Int32)
                        .Execute();
                }
                return numberRecords;
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
           
        }

        public int UpdateStatus(Entities.Video video)
        {
            throw new NotImplementedException();
        }
    }
}
