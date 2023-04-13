using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Banner;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;

namespace DVG.CMS.Controllers
{
    public class BannerController : Controller
    {
        private IBannerBo _bannerBo;

        public BannerController(IBannerBo bannerBo)
        {
            this._bannerBo = bannerBo;
        }

        [IsValidUrlRequest(KeyName = "BannerController.Index", Description = "Banner - Danh sách")]
        public ActionResult Index(int? bannerId)
        {
            BannerSearchModel model = new BannerSearchModel();
            model.ListPosition = EnumHelper.Instance.ConvertEnumToList<BannerPositionEnum>();
            model.ListPage = EnumHelper.Instance.ConvertEnumToList<BannerPageEnum>();
            model.ListStatus = EnumHelper.Instance.ConvertEnumToList<BannerStatusEnum>();
            model.ListPlatform = EnumHelper.Instance.ConvertEnumToList<BannerPlatformEnum>();
            if (bannerId > 0)
            {
                model.BannerId = (int)bannerId;
            }
            return View(model);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "BannerController.Search", Description = "Banner - Danh sách")]
        public ActionResult Search(BannerSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRows = 0;
            var lstRet = _bannerBo.GetList(searchModel.Keyword, searchModel.Platform, searchModel.Position, searchModel.PageId, searchModel.Status,
                searchModel.PageIndex, searchModel.PageSize, out totalRows);
            if (null != lstRet)
            {
                searchModel.ListPosition = EnumHelper.Instance.ConvertEnumToList<BannerPositionEnum>();
                searchModel.ListPage = EnumHelper.Instance.ConvertEnumToList<BannerPageEnum>();
                searchModel.ListStatus = EnumHelper.Instance.ConvertEnumToList<BannerStatusEnum>();
                searchModel.EditItem = new BannerModel();
                searchModel.ListData = lstRet.Select(item => new BannerModel(item)).ToList();
                foreach (var item in searchModel.ListData)
                {
                    if (item.Position > 0)
                        item.PositionName = searchModel.ListPosition.ToList().Find(x => x.Id == item.Position).Name;
                    if (item.PageId > 0)
                        item.PageName = searchModel.ListPage.ToList().Find(x => x.Id == item.PageId).Name;
                    if (item.Status > 0)
                        item.StatusName = searchModel.ListStatus.ToList().Find(x => x.Id == item.Status).Name;
                }

                responseData.Data = searchModel;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
            }
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "BannerController.Update", Description = "Banner - Cập nhật")]
        public ActionResult Update(BannerModel model)
        {
            ResponseData responseData = new ResponseData();
            if (null != model)
            {

                Banner banner = model.Id > 0 ? _bannerBo.GetById(model.Id) : new Banner();
                banner.Name = model.Name;
                banner.Title = model.Title;
                banner.Embed = model.Embed.Replace(StaticVariable.DomainImage, string.Empty);
                banner.Status = model.Status;
                banner.Position = model.Position;
                banner.PageId = model.PageId;
                banner.TargetLink = model.TargetLink;
                banner.Platform = model.Platform;

                if (string.IsNullOrEmpty(model.FromDateStr))
                    banner.FromDate = null;
                else
                    banner.FromDate = Utils.ConvertStringToDateTime(model.FromDateStr, Const.NormalDateFormat);
                if (string.IsNullOrEmpty(model.UntilDateStr))
                    banner.UntilDate = null;
                else
                    banner.UntilDate = Utils.ConvertStringToDateTime(model.UntilDateStr, Const.NormalDateFormat);



                int roww = 0;
                var lstBanner = _bannerBo.GetList(string.Empty, model.Platform, model.Position.ToInt(), model.PageId.ToInt(), (int)BannerStatusEnum.Show, 1, 15, out roww);
                bool flag = false;
                if (lstBanner != null && lstBanner.Any())
                {
                    var now = DateTime.Now.Ticks;
                    if (model.Id > 0)
                        lstBanner = lstBanner.Where(x => x.Id != model.Id);
                    foreach (var item in lstBanner)
                    {
                        if (item.FromDate.HasValue && item.UntilDate.HasValue && banner.FromDate.HasValue && banner.UntilDate.HasValue)
                        {
                            if (item.FromDate.Value.Ticks <= banner.FromDate.Value.Ticks && banner.FromDate.Value.Ticks <= item.UntilDate.Value.Ticks)
                            {
                                flag = true;
                            }
                            if (item.FromDate.Value.Ticks <= banner.UntilDate.Value.Ticks && banner.UntilDate.Value.Ticks <= item.UntilDate.Value.Ticks)
                            {
                                flag = true;
                            }
                            if (flag == true && banner.PageId == BannerPageEnum.HomePage.GetHashCode() && (banner.Position == BannerPositionEnum.Main.GetHashCode() || banner.Position == BannerPositionEnum.Mid1.GetHashCode()))
                            {
                                flag = false;
                            }
                        }
                    }
                }
                if (flag)
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BannerExits);
                    return Json(responseData);
                }
                if (model.Id == 0)
                {
                    banner.CreatedDate = DateTime.Now;
                }
                banner.ModifiedDate = DateTime.Now;
                string userName = AuthenService.GetUserLogin().UserName;
                banner.ModifiedBy = userName;
                banner.CreatedBy = userName;
                //banner.Embed = banner.Embed.Replace(StaticVariable.DomainImage, string.Empty);

                //Update banner
                ErrorCodes result = _bannerBo.Update(banner);
                responseData.Success = result == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(result);
            }

            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "BannerController.Delete", Description = "Banner - Xóa")]
        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _bannerBo.Delete(id);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "BannerController.GetBanner", Description = "Banner - Xem chi tiết")]
        public ActionResult GetBanner(int bannerId)
        {
            ResponseData responseData = new ResponseData();

            responseData.Success = true;
            responseData.Data = _bannerBo.GetById(bannerId);

            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "BannerController.UpdateStatusBanner", Description = "Banner - Cập nhật trạng thái")]
        public ActionResult UpdateStatusBanner(int id)
        {
            ResponseData responseData = new ResponseData();

            responseData.Success = true;
            var banner = _bannerBo.GetById(id);
            if (banner.Status == (int)BannerStatusEnum.Show)
                banner.Status = (int)BannerStatusEnum.Hide;
            else
                banner.Status = (int)BannerStatusEnum.Show;
            string userName = AuthenService.GetUserLogin().UserName;
            banner.ModifiedBy = userName;
            banner.ModifiedDate = DateTime.Now;

            if (banner.Status == (int)BannerStatusEnum.Show)
            {
                int row = 0;
                var lstBanner = _bannerBo.GetList(string.Empty, banner.Platform, banner.Position.ToInt(), banner.PageId.ToInt(), (int)BannerStatusEnum.Show, 1, 15, out row);
                bool flag = false;
                if (lstBanner != null && lstBanner.Any())
                {
                    foreach (var item in lstBanner)
                    {
                        if (item.FromDate.HasValue && item.UntilDate.HasValue && banner.FromDate.HasValue && banner.UntilDate.HasValue)
                        {
                            if (item.FromDate.Value.Ticks <= banner.FromDate.Value.Ticks && banner.FromDate.Value.Ticks <= item.UntilDate.Value.Ticks)
                            {
                                flag = true;
                            }
                            if (item.FromDate.Value.Ticks <= banner.UntilDate.Value.Ticks && banner.UntilDate.Value.Ticks <= item.UntilDate.Value.Ticks)
                            {
                                flag = true;
                            }
                        }
                    }
                }
                if (flag)
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BannerExits);
                    return Json(responseData);
                }
            }

            ErrorCodes result = _bannerBo.UpdateStatus(banner);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }
    }
}