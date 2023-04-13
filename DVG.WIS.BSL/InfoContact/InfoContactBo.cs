using DVG.WIS.DAL.InfoContact;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.InfoContact
{
    public class InfoContactBo : IInfoContactBo
    {
        private IInfoContactDal _InfoContact;

        public InfoContactBo(IInfoContactDal infoContact)
        {
            _InfoContact = infoContact;
        }

        public IEnumerable<Entities.InfoContact> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows)
        {
            try
            {
                return _InfoContact.GetListPaging(keyword:keyword,pageIndex:pageIndex,pageSize:pageSize,status:status, totalRows: out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public ErrorCodes Update(Entities.InfoContact infoContact)
        {
            ErrorCodes errorCode = ErrorCodes.Success;
            try
            {
                // Validate
                if (null != infoContact && !string.IsNullOrEmpty(infoContact.Name))
                {
                    WIS.Entities.InfoContact infoContactObj = new WIS.Entities.InfoContact();
                    // Insert/Update
                    int numberRecords = _InfoContact.Update(infoContact);
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.Exception;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, category);
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", infoContact.Id, ex.ToString()));
            }
            return errorCode;
        }
    }
}
