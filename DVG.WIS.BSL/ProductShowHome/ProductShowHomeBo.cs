using DVG.WIS.DAL.ProductShowHome;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.ProductShowHome
{
   public class ProductShowHomeBo :IProductShowHomeBo
    {
        private IProductShowHomeDal _productShowHomeDal;

        public ProductShowHomeBo(IProductShowHomeDal productShowHomeDal)
        {
            _productShowHomeDal = productShowHomeDal;
        }

        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _productShowHomeDal.Delete(id);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public IEnumerable<Entities.ProductShowHome> GetAllProductShowHome()
        {
            try
            {
                return _productShowHomeDal.GetListFE();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public Entities.ProductShowHome GetById(int id)
        {
            try
            {
                return _productShowHomeDal.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public IEnumerable<Entities.ProductShowHome> GetList(string keyword, int pageIndex, int pageSize, int status, out int totalRows)
        {
            try
            {
                return _productShowHomeDal.GetList(keyword, pageIndex, pageSize, status, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public ErrorCodes Update(Entities.ProductShowHome productShowHome)
        {
            ErrorCodes errorCode = ErrorCodes.Success;
            try
            {
                // Validate
                if (null != productShowHome && !string.IsNullOrEmpty(productShowHome.Title))
                {
                    WIS.Entities.ProductShowHome productShowHomeObj = new WIS.Entities.ProductShowHome();
                    if (productShowHome.Id > 0)
                    {
                        productShowHomeObj = _productShowHomeDal.GetById(productShowHome.Id);
                        productShowHomeObj.Title = productShowHome.Title;
                        productShowHomeObj.CategoryId = productShowHome.CategoryId;
                        productShowHomeObj.Status = productShowHome.Status;
                        productShowHomeObj.Limit = productShowHome.Limit;
                        productShowHome = productShowHomeObj;
                    }
                    // Insert/Update
                    int numberRecords = _productShowHomeDal.Update(productShowHome);
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.Exception;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, category);
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", productShowHome.Id, ex.ToString()));
            }
            return errorCode;
        }
    }
}
