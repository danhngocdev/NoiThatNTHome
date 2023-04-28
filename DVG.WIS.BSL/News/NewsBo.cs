using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Business.Category.Cached;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.DAL.News;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;

namespace DVG.WIS.Business.News
{
    public class NewsBo : INewsBo
    {
        private INewsDal _newsDal;
        public NewsBo(INewsDal newsDal)
        {
            _newsDal = newsDal;
        }

        public ErrorCodes ChangeStatusNews(int id, int statusNews, string changeBy, DateTime publishedDate)
        {
            ErrorCodes errorCode = ErrorCodes.Success;
            try
            {
                int numberRecords = _newsDal.ChangeStatusNews(id, statusNews, changeBy, publishedDate);
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.Exception;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id, statusNews, userName, lastModifiedDate, lastModifiedDateSpan, distributionDate);
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", statusNews, ex.ToString()));
            }
            return errorCode;
        }

        public IEnumerable<WIS.Entities.NewsImage> GetListImageByNewsId(int newsId)
        {
            try
            {
                var lstRet = _newsDal.GetListImageByNewsId(newsId);
                foreach (var item in lstRet)
                {
                    item.ImageUrlCrop = StaticVariable.DomainImage.TrimEnd('/') + AppSettings.Instance.GetString(Const.CropSizeCMS).TrimEnd('/') + "/" + item.ImageUrl.TrimStart('/');
                    item.ImageUrl = StaticVariable.DomainImage.TrimEnd('/') + "/" + item.ImageUrl.TrimStart('/');
                }
                return lstRet;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", newsId, ex.ToString()));
            }
            return null;
        }
        public Entities.News GetById(int id)
        {
            try
            {
                return _newsDal.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public Entities.Page GetPageById(int id)
        {
            try
            {
                return _newsDal.GetPageById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }
         public Entities.Video GetVideoById(int id)
        {
            try
            {
                return _newsDal.GetVideoById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public IEnumerable<Entities.News> GetList(int cateId, int status, string keyword, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                return _newsDal.GetList(cateId, status, keyword, pageIndex, pageSize, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public IEnumerable<NewsInListModel> GetListFE(int languageId, int cateId, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                var lstModel = _newsDal.GetListFE(languageId, cateId, pageIndex, pageSize, out totalRows);
                IEnumerable<NewsInListModel> lstNews = lstModel.Select(x => new NewsInListModel(x));
                return lstNews;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public IEnumerable<NewsInListModel> GetListNewsByCateId(int languageId,int cateId, int top)
        {
            try
            {
                var lstModel = _newsDal.GetListNewsByCateId(languageId,cateId, top);
                IEnumerable<NewsInListModel> lstNews = lstModel.Select(x => new NewsInListModel(x));
                return lstNews;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }    
        public IEnumerable<NewsInListModel> GetListNewsHighlight(int top)
        {
            try
            {
                var lstModel = _newsDal.GetListNewsHighlight( top);
                IEnumerable<NewsInListModel> lstNews = lstModel.Select(x => new NewsInListModel(x));
                return lstNews;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }
        public IEnumerable<NewsInListModel> GetListNewsHighlightByCate(int cateId, int top)
        {
            try
            {
                var lstModel = _newsDal.GetListNewsHighlightByCate(cateId,top);
                IEnumerable<NewsInListModel> lstNews = lstModel.Select(x => new NewsInListModel(x));
                return lstNews;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public ErrorCodes Update(Entities.News news, List<Entities.NewsImage> listNewsImage)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (news == null || news.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }
                //Xử lý ảnh
                if (listNewsImage != null)
                {
                    foreach (var item in listNewsImage)
                    {
                        item.ImageUrl = item.ImageUrl.Replace(StaticVariable.DomainImage, string.Empty).TrimStart('/');
                    }
                }
                int result = _newsDal.Update(news, listNewsImage);
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
        public ErrorCodes UpdatePage(Entities.Page news)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (news == null || news.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }
                //Xử lý ảnh
                
                int result = _newsDal.UpdatePage(news);
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
        public ErrorCodes UpdateVideo(Entities.Video news)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (news == null || news.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }
                //Xử lý ảnh
                
                int result = _newsDal.UpdateVideo(news);
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

        public IEnumerable<NewsInListModel> GetListArticleSiteMap()
        {
            try
            {
                var lstModel = _newsDal.GetListArticleSiteMap();
                IEnumerable<NewsInListModel> lstNews = lstModel.Select(x => new NewsInListModel(x));
                return lstNews;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }
    }
}
