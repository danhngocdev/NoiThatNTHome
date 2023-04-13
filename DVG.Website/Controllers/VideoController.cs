using DVG.WIS.Business.Video;
using DVG.WIS.Core;
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
    public class VideoController : BaseController
    {
        private  readonly IVideoBo _videoBo;
        public VideoController(IVideoBo videoBo)
        {
            _videoBo = videoBo;
        }

        // GET: Video
        public ActionResult Index(int pageIndex = 1)
        {
            int totalRows = 0;
            int pageSize = 7;
            var result = new List<VideoModel>();
            var lstModel = _videoBo.GetList(string.Empty, pageIndex, pageSize,1, out totalRows).ToList();
            if (lstModel != null)
            {
                 result = lstModel.Select(item => new VideoModel(item)).ToList();
            }

            #region Redirect Permanent 301
            string standardUrl = Resource.NewsAlias;
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
            return View(result);
        }

        public ActionResult VideoHightLight(int top = 5)
        {
            List<Video> lstModel = _videoBo.GetListVideoTop(top).ToList();
            var result = lstModel.Select(item => new VideoModel(item)).ToList();
            return PartialView("_VideoHightLight", result);
        }
    }
}