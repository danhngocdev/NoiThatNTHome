using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Category;
using DVG.WIS.Business.Products;
using DVG.WIS.Business.Users;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DVG.CMS.Controllers
{
    public class ProductController : Controller
    {
        private IUserBo _userBo;
        private readonly IProductBo _productBo;
        private ICategoryBo _categoryBo;

        public ProductController(IUserBo userBo, IProductBo ProductBo, ICategoryBo categoryBo)
        {
            _userBo = userBo;
            _productBo = ProductBo;
            _categoryBo = categoryBo;
        }

        [IsValidUrlRequest(KeyName = "ProductController.Index", Description = "Sản phẩm - Danh sách")]
        public ActionResult Index()
        {
            var userId = AuthenService.GetUserLogin().UserId;
            ProductSearchModel ProductSearchModel = new ProductSearchModel()
            {
                ListStatus = EnumHelper.Instance.ConvertEnumToList<ProductStatusEnum>()
            };
            return View(ProductSearchModel);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "ProductController.ProductView", Description = "Sản phẩm - Xem chi tiết")]
        public ActionResult ProductView(string ProductId)
        {
            ResponseData responseData = new ResponseData();

            int id = !string.IsNullOrEmpty(ProductId) ? EncryptUtility.DecryptIdToInt(ProductId) : 0;

            Product Product = _productBo.GetById(id);

            if (null != Product)
            {
                ProductModel ProductModel = new ProductModel(Product);
                responseData.Data = ProductModel;
                responseData.Success = true;

            }
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "CustomerController.Search", Description = "Sản phẩm - Danh sách")]
        public ActionResult Search(ProductSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRows = 0;
            ProductSearch productSearch = new ProductSearch()
            {
                Status = searchModel.Status,
                Keyword = searchModel.Keyword,
                PageIndex = searchModel.PageIndex,
                PageSize = searchModel.PageSize
            };
            var lstRet = _productBo.GetList(productSearch, out totalRows);
            if (null != lstRet)
            {
                searchModel.EditItem = new ProductModel();
                searchModel.ListData = lstRet.Select(item => new ProductModel(item)).ToList();
                responseData.Data = searchModel;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
            }
            return Json(responseData);
        }


        [IsValidUrlRequest(KeyName = "ProductController.UpdateProduct", Description = "Sản phẩm - Cập nhật")]
        public ActionResult UpdateProduct(int? ProductType, string ProductId = "", bool autosave = false)
        {
            string encript = Request.QueryString["ProductId"];
            int id = !string.IsNullOrEmpty(encript) ? EncryptUtility.DecryptIdToInt(ProductId) : 0;
            string userName = AuthenService.GetUserLogin().UserName;
            ViewBag.PendingApproved = (int)ProductStatusEnum.PendingApproved;
            ViewBag.Published = (int)ProductStatusEnum.Published;
            ViewBag.UnPublished = (int)ProductStatusEnum.UnPublished;
            ViewBag.Deleted = (int)ProductStatusEnum.Deleted;
            ViewBag.Status = 0;
            ViewBag.Title = "Thêm mới sản phẩm";
            ViewBag.TitleAction = "Thêm mới";
            //var userId = AuthenService.GetUserLogin().UserId;
            //var statusOfProductPermission = AuthenService.ProcessPermissionStatusOfProduct(userId);
            //ViewBag.StatusOfProductPermission = statusOfProductPermission;
            ProductModel ProductModel = new ProductModel();
            if (id != 0)
            {
                var Product = _productBo.GetById(id);
                // check phân quyền theo trạng thái tin
                //var ProductDetail = _productBo.GetById(id);
                ProductModel = new ProductModel(Product);
                var lstImage = _productBo.GetListImageByProductId(id).ToList();
                if (lstImage != null && lstImage.Any())
                    ProductModel.ListImage = lstImage;
                else
                    ProductModel.ListImage = new List<NewsImage>();
                ViewBag.Title = "Cập nhật sản phẩm";
                ViewBag.TitleAction = "Cập nhật";
                ViewBag.Status = Product.Status;
                ViewBag.Idstr = ProductModel.EncryptProductId;
            }
            else
            {
                ProductModel.CreatedBy = userName;
                ProductModel.PublishedDate = DateTime.Now;
                ViewBag.Idstr = string.Empty;
            }
            //ViewBag.ListCategoryDB = _categoryBo.GetListAll();
            ProductModel.ListCategory = _categoryBo.GetListAll().ToList();
            return View(ProductModel);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "ProductController.UpdateProduct", Description = "Sản phẩm - Cập nhật")]
        public ActionResult UpdateProduct(ProductModel Product, int currentStatus)
        {
            ResponseData responseData = this.UpdateProductProcess(Product, currentStatus);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "ProductController.DoPendingApprove", Description = "Sản phẩm - Gửi chờ duyệt")]
        public ActionResult DoPendingApprove(string ProductId)
        {
            ResponseData responseData = this.UpdateStatus(ProductId, (int)ProductStatusEnum.PendingApproved);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "ProductController.DoPublish", Description = "Sản phẩm - Xuất bản")]
        public ActionResult DoPublish(string ProductId)
        {
            ResponseData responseData = this.UpdateStatus(ProductId, (int)ProductStatusEnum.Published);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "ProductController.DoUnPublish", Description = "Sản phẩm - Gỡ bài")]
        public ActionResult DoUnPublish(string ProductId)
        {
            ResponseData responseData = this.UpdateStatus(ProductId, (int)ProductStatusEnum.UnPublished);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "ProductController.DoDelete", Description = "Sản phẩm - Xóa bài")]
        public ActionResult DoDelete(string ProductId)
        {
            ResponseData responseData = this.UpdateStatus(ProductId, (int)ProductStatusEnum.Deleted);
            return Json(responseData);
        }

        #region private

        private ResponseData UpdateProductProcess(ProductModel Product, int currentStatus)
        {
            ResponseData responseData = new ResponseData();
            if (!AuthenService.IsLogin())
            {
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.NotLogin);
                return responseData;
            }

            Product ProductInfo = new Product();
            if (Product != null && Product.Id > 0)
            {
                ProductInfo = _productBo.GetById(Product.Id);
            }

            if (Product != null)
            {

                string userName = AuthenService.GetUserLogin().UserName;
                ProductInfo.CreatedBy = userName;
                ProductInfo.ModifiedBy = userName;
                // nếu bài chuyển trạng thái xuất bản và PulishDate < DateTime.Now thì gán bằng now, còn không thì giữ nguyên

                ProductInfo.Name = Product.Name;
                ProductInfo.Sapo = Product.Sapo;
                ProductInfo.Avatar = Product.Avatar.Replace(StaticVariable.DomainImage.TrimEnd('/'), string.Empty).Trim('/');
                ProductInfo.Price = Product.Price;
                ProductInfo.PricePromotion = Product.PricePromotion;
                ProductInfo.Code = Product.Code;
                ProductInfo.Capacity = Product.Capacity;
                ProductInfo.MadeIn = Product.MadeIn;
                ProductInfo.IsOutStock = Product.IsOutStock;
                ProductInfo.Description = Product.Description;
                ProductInfo.Status = Product.Status;
                ProductInfo.IsHighLight = Product.IsHighLight;
                ProductInfo.CategoryId = Product.CategoryId;
                ProductInfo.TextSearch = string.Format("{0} {1} {2} {3}", ProductInfo.Name, ProductInfo.Sapo, ProductInfo.Price, ProductInfo.PricePromotion, ProductInfo.Description);
                try
                {
                    string avatar = string.Empty;
                    ProductInfo.Description = StringUtils.UploadImageIncontent(Product.Description, out avatar, StaticVariable.ImageRootPath, string.Empty, StaticVariable.DomainImage, FileStorage.DownloadImageFromURL);
                    ProductInfo.Description = Regex.Replace(ProductInfo.Description, @"(<p([^>]+)?>([\s\r\n]+)?<(strong|b)>([\s\r\n]+)?Xem thêm(\s+)?:([\s\r\n]+)?</(strong|b)>([\s\r\n]+)?</p>.+)", string.Empty, RegexOptions.Singleline);
                }
                catch (Exception ex)
                {
                    ProductInfo.Description = Product.Description;
                    Logger.ErrorLog(ex);
                }
                ProductInfo.SEODescription = Product.SEODescription;
                ProductInfo.SEOTitle = Product.SEOTitle;
                ProductInfo.SEOKeyword = Product.SEOKeyword;
                ErrorCodes errorCode = _productBo.Update(ProductInfo, Product.ListImage);
                responseData.Success = errorCode == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCode);
            }
            return responseData;
        }

        private ResponseData UpdateStatus(string ProductId, int status)
        {
            ResponseData responseData = new ResponseData();
            if (AuthenService.IsLogin() && !string.IsNullOrEmpty(ProductId))
            {
                int id = EncryptUtility.DecryptIdToInt(ProductId);
                var ProductInfo = _productBo.GetById(id);
                if (ProductInfo != null && ProductInfo.Id > 0)
                {
                    string userName = AuthenService.GetUserLogin().UserName;
                    ErrorCodes errorCode = _productBo.ChangeStatusProduct(id, status, userName);


                    responseData.ErrorCode = Utils.ConvertEnumToInt(errorCode);
                    responseData.Success = errorCode == ErrorCodes.Success;
                    responseData.Message = StringUtils.GetEnumDescription(errorCode);
                }
            }
            return responseData;
        }

        #endregion
    }
}