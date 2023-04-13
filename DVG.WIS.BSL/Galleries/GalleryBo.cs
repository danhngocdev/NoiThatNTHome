using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Core;
using DVG.WIS.DAL.Galleries;
using DVG.WIS.DAL.Recruitments;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;

namespace DVG.WIS.Business.Galleries
{
    public class GalleryBo : IGalleryBo
    {
        private IGalleryDal _galleryDal;

        public GalleryBo(IGalleryDal GalleryDal)
        {
            _galleryDal = GalleryDal;
        }

        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _galleryDal.Delete(id);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public Gallery GetById(int id)
        {
            try
            {
                return _galleryDal.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public IEnumerable<Entities.Gallery> GetList(int status, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                return _galleryDal.GetList(status, pageIndex, pageSize, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public IEnumerable<Gallery> GetListFE(int status, int pageSize)
        {
            try
            {
                return _galleryDal.GetListFE(status, pageSize);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public ErrorCodes Update(Gallery banner)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (banner == null || banner.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }

                int result = _galleryDal.Update(banner);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }
    }
}
