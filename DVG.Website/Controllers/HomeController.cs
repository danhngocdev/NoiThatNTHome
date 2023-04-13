using DVG.Website.Models;
using DVG.WIS.Business.Category;
using DVG.WIS.Business.Menu;
using DVG.WIS.Business.News;
using DVG.WIS.Business.Persons;
using DVG.WIS.Business.Products;
using DVG.WIS.Business.ProductShowHome;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Local;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class HomeController : BaseController
    {
        private INewsBo _newsBo;
        private ICategoryBo _categoryBo;
        private IProductBo _productBo;
        private IProductShowHomeBo _productShowHomeBo;
        private IMenuBo _menuBo;

        public HomeController(INewsBo newsBo, ICategoryBo categoryBo, IProductBo productBo, IPersonBo personBo, IProductShowHomeBo productShowHomeBo, IMenuBo menuBo)
        {
            _newsBo = newsBo;
            _categoryBo = categoryBo;
            _productBo = productBo;
            _productShowHomeBo = productShowHomeBo;
            _menuBo = menuBo;
        }

        public ActionResult Index()
        {
            ViewBag.TitlePage = SEO.AddTitle(ConstMeta.HomeTitle);
            ViewBag.MetaDescription = SEO.AddMeta("description", ConstMeta.HomeDesc);
            ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner.jpg");
            ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl);
            List<HomePageViewModel> homePageViewModel = new List<HomePageViewModel>();
            var listproductShowHome = _productShowHomeBo.GetAllProductShowHome();
            var results = listproductShowHome.GroupBy(x => x.CategoryId).Select(y => y.First()).ToList();
            if (results != null && results.Any())
            {
                int totalProduct = 0;
                foreach (var item in results)
                {
                    var homePage = new HomePageViewModel();
                    homePage.Title = item.Title;
                    var listProductDto = _productBo.GetListProducByCateId(item.CategoryId, 1, item.Limit,out totalProduct);
                    homePage.ListProduct = listProductDto.Select(x => new ProductFEListModel(x)).ToList();
                    if (homePage.ListProduct.Count >0 && homePage.ListProduct.Any())
                    {
                        var cate = _categoryBo.GetById(item.CategoryId);
                        homePage.Link = string.Format("danh-muc/{0}", cate.ShortURL);
                        homePageViewModel.Add(homePage);
                    }
                }
            }
     
            return View(homePageViewModel);
        }

        public ActionResult ProductMenu()
        {
            var listMenu = _menuBo.GetListMenuTop();
            //var lstCate = _categoryBo.GetListCateHome();
            return PartialView("_ProductMenu", listMenu);
        }



        public ActionResult DynamicStaticPage(string category)
        {
            int pageId = 0;
            int pageEnum = 0;
            switch (category)
            {
                case ConstUrl.AboutUs:
                    pageId = 1;
                    pageEnum = BannerPageEnum.Introduce.GetHashCode();
                    break;
                case ConstUrl.SuccessStory:
                    pageId = 3;
                    pageEnum = BannerPageEnum.SuccessStories.GetHashCode();
                    break;
            }
            if (pageId > 0)
            {
                var page = _newsBo.GetPageById(pageId);
                ViewBag.TitlePage = SEO.AddTitle(page.SEOTitle);
                ViewBag.PageEnum = pageEnum;
                ViewBag.MetaDescription = SEO.AddMeta("description", page.SEODescription);
                ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner.jpg");
                ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + "/" + ConstUrl.AboutUs);
                return View(page);
            }
            else
            {
                return Redirect("/");
            }
        }

        public ActionResult Procedure()
        {
            var page = _newsBo.GetPageById((int)PageEnum.Procedure);
            ViewBag.TitlePage = SEO.AddTitle(ConstMeta.CoreTitle);
            ViewBag.MetaDescription = SEO.AddMeta("description", ConstMeta.CoreDesc);
            ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner.jpg");
            ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + "/" + ConstUrl.Procedure);
            return View(page);
        }

        //public ActionResult SuccessStory()
        //{
        //    var page = _newsBo.GetPageById(Page_SuccessStory);
        //    ViewBag.TitlePage = SEO.AddTitle(ConstMeta.SuccessStoryTitle);
        //    ViewBag.MetaDescription = SEO.AddMeta("description", ConstMeta.SuccessStoryDesc);
        //    ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner.jpg");
        //    ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + "/" + ConstUrl.AboutUs);
        //    return View(page);
        //}

        public ActionResult Contact()
        {
            ViewBag.TitlePage = SEO.AddTitle(ConstMeta.ContactTitle);
            ViewBag.MetaDescription = SEO.AddMeta("description", ConstMeta.ContactDesc);
            ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner.jpg");
            ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + "/" + ConstUrl.Contact);
            return View();
        }
    }
}