using DVG.WIS.Business.News;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Entities;
using DVG.WIS.Local;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class NewsController : BaseController
    {
        private INewsBo _newsBo;
        public NewsController(INewsBo newsBo)
        {
            _newsBo = newsBo;
        }

        // GET: News
        public ActionResult Index(int pageIndex = 1)
        {
            int totalRows = 0;
            int pageSize = 18;
            List<NewsInListModel> lstModel = _newsBo.GetListFE(LanguageId, 0, pageIndex, pageSize, out totalRows).ToList();

            #region Redirect Permanent 301
            string standardUrl = string.Concat("/", ConstUrl.News); 
            string standardUrlNoPagging = standardUrl;
            if (pageIndex > 1)
            {
                standardUrl = standardUrl.TrimEnd('/') + "/p" + pageIndex;
            }
            string currentUrl = Request != null && Request.RawUrl != null ? Request.RawUrl : standardUrl;
            if (!currentUrl.Equals(standardUrl))
            {
                return Redirect301(standardUrl);
            }
            #endregion

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
            string title = Resource.News;
            if (pageIndex > 1)
                title = string.Concat(title, " - p", pageIndex);
            ViewBag.Title = title;
            ViewBag.TitlePage = SEO.AddTitle(Resource.NewsTitle);
            ViewBag.MetaDescription = SEO.AddMeta("description", Resource.NewsDesc);
            ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner1.jpg");
            ViewBag.IsNews = true;
            return View(lstModel);
        }

        public ActionResult List(string category, int pageIndex = 1)
        {
            ViewBag.IsNews = true;
            int cateId = 0;
            string title = string.Empty;
            string desc = string.Empty;
            string standardUrl = string.Empty;
            switch (category)
            {
                case ConstUrl.BeautyKnowledge:
                    cateId = 1;
                    title = ConstMeta.BeautyKnowledgeTitle;
                    desc = ConstMeta.BeautyKnowledgeDesc;
                    standardUrl = $"/{ConstUrl.News}/{ConstUrl.BeautyKnowledge}";
                    break;
                case ConstUrl.CompanyNews:
                    cateId = 2;
                    title = ConstMeta.CompanyNewsTitle;
                    desc = ConstMeta.CompanyNewsDesc;
                    standardUrl = $"/{ConstUrl.News}/{ConstUrl.CompanyNews}";
                    break;
                case ConstUrl.CareerNews:
                    cateId = 3;
                    title = ConstMeta.CareerNewsTitle;
                    desc = ConstMeta.CareerNewsDesc;
                    standardUrl = $"/{ConstUrl.News}/{ConstUrl.CareerNews}";
                    break;
                case ConstUrl.CustomerStory:
                    cateId = 4;
                    title = ConstMeta.CustomerStoryTitle;
                    desc = ConstMeta.CustomerStoryDesc;
                    standardUrl = $"/{ConstUrl.News}/{ConstUrl.CustomerStory}";
                    break;
            }

            int totalRows = 0;
            int pageSize = 10;
            List<NewsInListModel> lstModel = _newsBo.GetListFE(LanguageId, cateId, pageIndex, pageSize, out totalRows).ToList();

            #region Redirect Permanent 301
            string standardUrlNoPagging = standardUrl;
            if (pageIndex > 1)
            {
                standardUrl = standardUrl.TrimEnd('/') + "/p" + pageIndex;
            }

            string currentUrl = Request != null && Request.RawUrl != null ? Request.RawUrl : standardUrl;

            if (!currentUrl.Equals(standardUrl))
            {
                return Redirect301(standardUrl);
            }

            #endregion

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
            ViewBag.Title = title;
            ViewBag.TitlePage = SEO.AddTitle(title);
            ViewBag.MetaDescription = SEO.AddMeta("description", desc);
            ViewBag.MetaFacebook = SEO.AddMetaImages("photo/2020/03/03/banner1.jpg");
            ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + standardUrl);
            return View(lstModel);
        }

        public ActionResult Detail(int newsId)
        {
            ViewBag.IsNews = true;
            var news = _newsBo.GetById(newsId);
            if (news == null && news.Id == 0)
            {
                return Redirect(CoreUtils.BuildURL(AppSettings.Instance.GetString(Const.UrlNotfoundContent)));
            }
            ViewBag.CateName = news.CategoryId == 1 ? Resource.CompanyNews : Resource.Blogs;
            ViewBag.CateUrl = news.CategoryId == 1 ? Resource.CompanyNewsAlias : Resource.BlogsAlias;
            string metaTags = SEO301.Instance.BindingMeta("", news.Title, news.Sapo, string.Empty);
            ViewBag.TitlePage = SEO.AddTitle(news.Title);
            ViewBag.MetaDescription = SEO.AddMeta("description", news.Sapo);
            ViewBag.MetaFacebook = SEO.AddMetaImages(news.Avatar);
            ViewBag.MetaCanonical = SEO.MetaCanonical(StaticVariable.BaseUrl.TrimEnd('/') + Request.RawUrl.ToString());
            return View(news);
        }


        public ActionResult HighlightNews(int top = 10, int newsId = 0)
        {
            List<NewsInListModel> lstModel = _newsBo.GetListNewsHighlight(top + 1).ToList();
            lstModel = lstModel.Where(x => x.CategoryId != newsId).Take(top).ToList();
            return PartialView("_HighlightNews", lstModel);
        }



        public ActionResult ArticleSidebar(int top = 5, int newsId = 0)
        {
            List<NewsInListModel> lstModel = _newsBo.GetListNewsHighlight(top + 1).ToList();
            lstModel = lstModel.Where(x => x.CategoryId != newsId).Take(top).ToList();
            return PartialView("_ArticleSidebar", lstModel);
        }

        public ActionResult NewsSearch()
        {
            return PartialView();
        }

        public ActionResult SuccessStoryFirst()
        {
            List<NewsInListModel> lstModel = _newsBo.GetListNewsHighlightByCate(4, 1).ToList();
            return PartialView("_SuccessStoryFirst", lstModel);
        }


        public ActionResult Construction()
        {
            List<NewsInListModel> lstModel = _newsBo.GetListNewsHighlightByCate(2,10).ToList();
            return PartialView("_Construction", lstModel);
        }

    }
}