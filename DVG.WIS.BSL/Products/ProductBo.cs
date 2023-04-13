using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Business.Category.Cached;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.DAL.Products;
using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;

namespace DVG.WIS.Business.Products
{
    public class ProductBo : IProductBo
    {
        private IProductDal _productDal;

        public ProductBo(IProductDal ProductDal)
        {
            _productDal = ProductDal;
        }

        public ErrorCodes ChangeStatusProduct(int id, int statusProduct, string changeBy)
        {
            ErrorCodes errorCode = ErrorCodes.Success;
            try
            {
                int numberRecords = _productDal.ChangeStatusProduct(id, statusProduct, changeBy);
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.Exception;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id, statusProduct, userName, lastModifiedDate, lastModifiedDateSpan, distributionDate);
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", statusProduct, ex.ToString()));
            }
            return errorCode;
        }

        public IEnumerable<WIS.Entities.NewsImage> GetListImageByProductId(int ProductId)
        {
            try
            {
                var lstRet = _productDal.GetListImageByProductId(ProductId);
                foreach (var item in lstRet)
                {
                    item.ImageUrlCrop = StaticVariable.DomainImage.TrimEnd('/') + AppSettings.Instance.GetString(Const.CropSizeCMS).TrimEnd('/') + "/" + item.ImageUrl.TrimStart('/');
                    item.ImageUrl = StaticVariable.DomainImage.TrimEnd('/') + "/" + item.ImageUrl.TrimStart('/');
                }
                return lstRet;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", ProductId, ex.ToString()));
            }
            return null;
        }
        public Entities.Product GetById(int id)
        {
            try
            {
                return _productDal.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public IEnumerable<Entities.Product> GetList(ProductSearch productSearch, out int totalRows)
        {
            try
            {
                return _productDal.GetList(productSearch, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public ErrorCodes Update(Entities.Product Product, List<Entities.NewsImage> listNewsImage)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (Product == null || Product.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }
                //Xử lý ảnh
                if (listNewsImage != null)
                {
                    foreach (var item in listNewsImage)
                    {
                        item.ImageUrl = item.ImageUrl.Replace(StaticVariable.DomainImage.TrimEnd('/'), string.Empty)
                                                    .Replace(AppSettings.Instance.GetString("CropSizeCMS"), string.Empty)
                                                    .TrimStart('/');
                    }
                }
                int result = _productDal.Update(Product, listNewsImage);
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

        public ErrorCodes UpdateOrder(Order order, List<OrderDetail> orderDetails)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (order == null || orderDetails == null || !orderDetails.Any())
                {
                    return ErrorCodes.BusinessError;
                }
                //Xử lý ảnh
                int result = _productDal.UpdateOrder(order, orderDetails);
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

        public IEnumerable<Product> GetListProductNewest(int languageId, int limit)
        {
            try
            {
                return _productDal.GetListProductNewest(languageId, limit);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }
        public IEnumerable<Product> GetListProductHot(int limit)
        {
            try
            {
                return _productDal.GetListProductHot(limit);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }
        public IEnumerable<Product> GetListProducByCateId(int cateId, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                return _productDal.GetListProducByCateId(cateId, pageIndex, pageSize, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }
        public IEnumerable<Product> GetListProducByListProductId(List<int> lstID)
        {
            try
            {
                return _productDal.GetListProducByListProductId(lstID);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }
        public IEnumerable<Product> GetListProducByKeyword(string keyword, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                return _productDal.GetListProducByKeyword(keyword, pageIndex, pageSize, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }
    }
}
