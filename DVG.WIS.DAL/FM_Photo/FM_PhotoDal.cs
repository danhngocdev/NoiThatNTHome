using DVG.WIS.Utilities;
using FluentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.FM_Photo
{
    public class FM_PhotoDal : ContextBase, IFM_PhotoDal
    {
        public FM_PhotoDal()
        {
            this._dbPosition = DBPosition.Master;
        }

        public IEnumerable<WIS.Entities.FM_PhotoOnList> GetList(string keyword, string userName, int pageIndex, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            IEnumerable<WIS.Entities.FM_PhotoOnList> lst;
            try
            {
                using (var context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure("Admin_FM_Photo_GetList")
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .Parameter("Keyword", keyword, DataTypes.String)
                        .Parameter("UserName", userName, DataTypes.String)
                        .Parameter("FromDate", fromDate, DataTypes.DateTime)
                        .Parameter("ToDate", toDate, DataTypes.DateTime);
                    lst = cmd.QueryMany<WIS.Entities.FM_PhotoOnList>();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lst;
        }

        public int Update(WIS.Entities.FM_Photo obj)
        {
            string storeName = "Admin_FM_Photo_Update";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", obj.Id, DataTypes.Int32)
                        .Parameter("FileName", obj.FileName, DataTypes.String)
                        .Parameter("FileUrl", obj.FileUrl, DataTypes.String)
                        .Parameter("Title", obj.Title, DataTypes.String)
                        .Parameter("Description", obj.Description, DataTypes.String)
                        .Parameter("Alternate", obj.Alternate, DataTypes.String)
                        .Parameter("Width", obj.Width, DataTypes.Int32)
                        .Parameter("Height", obj.Height, DataTypes.Int32)
                        .Parameter("Capacity", obj.Capacity, DataTypes.Int32)
                        .Parameter("MimeType", obj.MimeType, DataTypes.Int32)
                        .Parameter("MimeTypeName", obj.MimeTypeName, DataTypes.String)
                        .Parameter("CreatedBy", obj.CreatedBy, DataTypes.String)
                        .Parameter("ModifiedBy", obj.ModifiedBy, DataTypes.String)
                        .Parameter("CreatedDate", obj.CreatedDate, DataTypes.DateTime)
                        .Parameter("CreatedDateSpan", obj.CreatedDateSpan, DataTypes.Int64)
                        .Parameter("ModifiedDate", obj.ModifiedDate, DataTypes.DateTime)
                        .Parameter("ModifiedDateSpan", obj.ModifiedDateSpan, DataTypes.Int64)
                        .Parameter("Status", obj.Status, DataTypes.Int32)
                        .Parameter("DisplayPosition", obj.DisplayPosition, DataTypes.Int32)
                        .Parameter("DisplayStyle", obj.DisplayStyle, DataTypes.Int32)
                        .Parameter("FileSize", obj.FileSize, DataTypes.Int64)
                        .Execute();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }

        public WIS.Entities.FM_Photo GetByFileName(string fileName)
        {
            string storeName = "Admin_FM_Photo_GetByFileName";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("fileName", fileName, DataTypes.String)
                        .QuerySingle<WIS.Entities.FM_Photo>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex));
            }
        }
    }
}
