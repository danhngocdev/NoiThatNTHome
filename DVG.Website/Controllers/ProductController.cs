using DVG.Website.Models;
using DVG.WIS.Business.Category;
using DVG.WIS.Business.Products;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Entities;
using DVG.WIS.Local;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class ProductController : BaseController
    {
        private IProductBo _productBo;
        private ICategoryBo _categoryBo;

        public ProductController(IProductBo productBo, ICategoryBo categoryBo)
        {
            _productBo = productBo;
            _categoryBo = categoryBo;
        }

        // GET: Product

        public ActionResult Index()
        {
            int pageSize = 6;
            ProductPageViewModel productPageViewModel = new ProductPageViewModel();
            //ProductForFace
            var lstCategory = _categoryBo.GetListAll();
            string cateFace = ConstUrl.ProductFace;
            int cateFaceId = lstCategory.Where(x => x.ShortURL.Equals(cateFace)).FirstOrDefault().Id;
            int outFace = 0;
            var lstProductForFace = _productBo.GetListProducByCateId(cateFaceId, 1, pageSize, out outFace);
            List<ProductFEListModel> lstProductFaceModel = new List<ProductFEListModel>();
            if (lstProductForFace != null && lstProductForFace.Any())
                lstProductFaceModel = lstProductForFace.Select(x => new ProductFEListModel(x)).ToList();
            productPageViewModel.ListProductForFace = lstProductFaceModel;

            //ProductForBody
            string cateBody = ConstUrl.ProductBody;
            int cateBodyId = lstCategory.Where(x => x.ShortURL.Equals(cateBody)).FirstOrDefault().Id;
            int outBody = 0;
            var lstProductBody = _productBo.GetListProducByCateId(cateBodyId, 1, pageSize, out outBody);
            List<ProductFEListModel> lstProductBodyModel = new List<ProductFEListModel>();
            if (lstProductBody != null && lstProductBody.Any())
                lstProductBodyModel = lstProductBody.Select(x => new ProductFEListModel(x)).ToList();
            productPageViewModel.ListProductForBody = lstProductBodyModel;


            ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + "/" + ConstUrl.Product);
            ViewBag.IsProduct = true;
            ViewBag.TitlePage = SEO.AddTitle(ConstMeta.ProductTitle);
            ViewBag.MetaDescription = SEO.AddMeta("description", ConstMeta.ProductDesc);
            ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner1.jpg");
            return View(productPageViewModel);
        }

        public ActionResult List(string category, int pageIndex = 1,int loadmore = 0)
        {
            var lstCategory = _categoryBo.GetListAll();
            var cate = lstCategory.Where(x => x.ShortURL.Equals(category)).FirstOrDefault();
            int cateId = 0;
            string title = string.Empty;
            string standardUrl = string.Empty;
            string desc = string.Empty;
            if (cate != null && cate.Id > 0)
            {
                cateId = cate.Id;
                standardUrl = $"/{ConstUrl.Product}/{cate.ShortURL}";
                title = cate.Name;
                ViewBag.DescCate = cate.Description;
            }
            if (cateId == 0)
            {
                return Redirect("/");
            }
            int totalRows = 0;
            int pageSize = 4;
            IEnumerable<Product> lstProduct = _productBo.GetListProducByCateId(cateId, pageIndex, pageSize, out totalRows);
            List<ProductFEListModel> lstProductModel = new List<ProductFEListModel>();
            if (lstProduct != null && lstProduct.Any())
                lstProductModel = lstProduct.Select(x => new ProductFEListModel(x)).ToList();


            ViewBag.TotalRecord = totalRows;
            if (loadmore == 1)
            {
                return PartialView("_ListItem", lstProductModel);
            }


            if (pageIndex > 1)
                title = string.Concat(title, " - p", pageIndex);
            ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + Request.RawUrl.ToString());
            ViewBag.IsProduct = true;
            ViewBag.Title = title;
            ViewBag.TitlePage = SEO.AddTitle(!string.IsNullOrEmpty(cate.MetaTitle) ? cate.MetaTitle : cate.Name);
            ViewBag.MetaDescription = SEO.AddMeta("description", !string.IsNullOrEmpty(cate.MetaDescription) ? cate.MetaDescription : cate.Description);
            ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner1.jpg");
            return View(lstProductModel);
        }

        public ActionResult Search(string keyword, int pageIndex = 1)
        {


            //Regex regex = new Regex("(~|!|#|\\$|%|\\^|&|\\*|\\(|\\)|_|\\+|\\{|\\}|\\||\"|:|\\?|>|<|,|\\.|\\/|;|'|\\\\|[|\\]|=|-)", RegexOptions.IgnoreCase);
           //keyword = keyword.Replace(regex, string.Empty);
            string title = "Tìm kiếm sản phẩm: " + keyword;
            string standardUrl = Request.RawUrl.ToString();


            int totalRows = 0;
            int pageSize = 6;
            IEnumerable<Product> lstProduct = _productBo.GetListProducByKeyword(keyword, pageIndex, pageSize, out totalRows);
            List<ProductFEListModel> lstProductModel = new List<ProductFEListModel>();
            if (lstProduct != null && lstProduct.Any())
                lstProductModel = lstProduct.Select(x => new ProductFEListModel(x)).ToList();

            #region paging
            if (totalRows > 0)
            {
                Pagings pageModel = new Pagings
                {
                    PageIndex = pageIndex,
                    Count = totalRows,
                    LinkSite = Utils.GetCurrentURL(standardUrl, pageIndex),
                    PageSize = pageSize
                };
                ViewBag.PagingInfo = pageModel;
            }
            #endregion

            if (pageIndex > 1)
                title = string.Concat(title, " - p", pageIndex);
            ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + Request.RawUrl.ToString());
            ViewBag.IsProduct = true;
            ViewBag.Title = title;
            ViewBag.TitlePage = SEO.AddTitle(Resource.ProductTitle);
            ViewBag.MetaDescription = SEO.AddMeta("description", Resource.ProductDesc);
            ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner1.jpg");
            return View("List", lstProductModel);
        }

        public ActionResult Detail(int productId)
        {
            ViewBag.IsProduct = true;
            var news = _productBo.GetById(productId);
            if (news == null && news.Id == 0)
            {
                return Redirect(CoreUtils.BuildURL(AppSettings.Instance.GetString(Const.UrlNotfoundContent)));
            }

            //string linkCur = CoreUtils.BuildURL("{0}/{1}-pid{2}", "san-pham", StringUtils.UnicodeToUnsignCharAndDash(news.Name), news.Id);

            List<NewsImageFEModel> lstNewsImage = new List<NewsImageFEModel>();
            var lstImage = _productBo.GetListImageByProductId(productId).ToList();
            if (lstImage != null && lstImage.Any())
            {
                var newImg = new NewsImage();
                newImg.ImageUrl = news.Avatar;
                newImg.Title = news.Name;
                lstImage.Add(newImg);
                lstNewsImage = lstImage.Select(x => new NewsImageFEModel(x)).ToList();
            }
            ViewBag.ListImage = lstNewsImage;
            ViewBag.TitlePage = SEO.AddTitle(news.Name + "Nội Thất NT Home");
            ViewBag.MetaDescription = SEO.AddMeta("description", news.Sapo);
            ViewBag.MetaFacebook = SEO.AddMetaImages(news.Avatar);
            ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + Request.RawUrl.ToString());
            return View(news);
        }

        public ActionResult ProductHighLight(int productId = 0)
        {
            IEnumerable<Product> lstProduct = _productBo.GetListProductHot(4);
            List<ProductFEListModel> lstProductModel = new List<ProductFEListModel>();
            if (lstProduct != null && lstProduct.Any())
                lstProductModel = lstProduct.Where(x => x.Id != productId).Select(x => new ProductFEListModel(x)).ToList();
            return PartialView("_ProductHighLight", lstProductModel);
        }

        public ActionResult ProductNewest(int productId = 0)
        {
            IEnumerable<Product> lstProduct = _productBo.GetListProductNewest(LanguageId, 4);
            List<ProductFEListModel> lstProductModel = new List<ProductFEListModel>();
            if (lstProduct != null && lstProduct.Any())
                lstProductModel = lstProduct.Where(x => x.Id != productId).Select(x => new ProductFEListModel(x)).ToList();
            return PartialView("_ProductNewest", lstProductModel);
        }


        public ActionResult ProductSameTopic(int categoryId = 0,int productId = 0)
        {
            int totalCount = 0;
            IEnumerable<Product> lstProduct = _productBo.GetListProducByCateId(categoryId,1,16,out totalCount);
            List<ProductFEListModel> lstProductModel = new List<ProductFEListModel>();
            if (lstProduct != null && lstProduct.Any())
                lstProductModel = lstProduct.Where(x => x.Id != productId).Select(x => new ProductFEListModel(x)).ToList();
            return PartialView("_ProductHighLight", lstProductModel);
        }



        public ActionResult ProductMenu()
        {
            IEnumerable<Product> lstProduct = _productBo.GetListProductNewest(LanguageId, 6);
            List<ProductFEListModel> lstProductModel = new List<ProductFEListModel>();
            if (lstProduct != null && lstProduct.Any())
                lstProductModel = lstProduct.Select(x => new ProductFEListModel(x)).ToList();
            return PartialView("_ProductMenu", lstProductModel);
        }
    }
}