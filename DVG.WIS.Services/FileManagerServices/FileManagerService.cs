using DVG.WIS.Business.FM_photo;
using DVG.WIS.DAL.FM_Photo;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Services.FileManagerServices
{
    public class FileManagerService
    {
        private IFM_PhotoDal _fmPhotoDal;
        private IFM_PhotoBo _fmPhotoBo;
        public FileManagerService()
        {
            _fmPhotoDal = new FM_PhotoDal();
            _fmPhotoBo = new FM_PhotoBo(_fmPhotoDal);
        }

        public ResponseData GetList(string keyword, string userName, int pageIndex, int pageSize, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var lst = _fmPhotoBo.GetList(keyword, userName, pageIndex, pageSize, fromDate, toDate);
            var response = new ResponseData
            {
                Data = lst,
                Success = true
            };
            return response;
        }

        public ResponseData GetFileInfo(string fileName)
        {
            var data = _fmPhotoBo.GetByFileName(fileName);
            var response = new ResponseData
            {
                Data = data,
                Success = true
            };
            return response;
        }

        public ResponseData UpdateFileInfo(DVG.WIS.Entities.FM_Photo obj)
        {
            var res = _fmPhotoBo.Update(obj);
            var response = new ResponseData
            {
                Success = true,
                ErrorCode = (int)res
            };
            return response;
        }

    }
}
