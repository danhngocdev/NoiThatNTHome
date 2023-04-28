using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using FluentData;

namespace DVG.WIS.DAL.News
{
    public class NewsDal : ContextBase, INewsDal
    {
        public int ChangeStatusNews(int id, int statusNews, string changeBy, DateTime publishedDate)
        {
            string storeName = "Admin_News_UpdateStatus";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .Parameter("Status", statusNews, DataTypes.Int32)
                        .Parameter("ModifiedBy", changeBy, DataTypes.String)
                        .Parameter("ModifiedDate", DateTime.Now, DataTypes.DateTime)
                        .Parameter("PublishedDate", publishedDate, DataTypes.DateTime)
                        .QuerySingle<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public Entities.News GetById(int id)
        {
            string storeName = "Admin_News_GetById_20200526";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.News>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
        public Entities.Page GetPageById(int id)
        {
            string storeName = "Admin_Page_GetById";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.Page>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
          public Entities.Video GetVideoById(int id)
        {
            string storeName = "Admin_Video_GetById";
            try
            {
                using (IDbContext context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("Id", id, DataTypes.Int32)
                        .QuerySingle<Entities.Video>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.News> GetList(int cateId, int status, string keyword, int pageIndex, int pageSize, out int totalRows)
        {
            string storeName = "Admin_News_GetList";
            IEnumerable<Entities.News> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("CategoryId", cateId, DataTypes.Int32)
                        .Parameter("Status", status, DataTypes.Int32)
                        .Parameter("Keyword", keyword, DataTypes.String)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.News>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.News> GetListFE(int languageId, int cateId, int pageIndex, int pageSize, out int totalRows)
        {
            string storeName = "FE_News_GetList_20200526";
            IEnumerable<Entities.News> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("LanguageId", 0, DataTypes.Int32)
                        .Parameter("CategoryId", cateId, DataTypes.Int32)
                        .Parameter("PageIndex", pageIndex, DataTypes.Int32)
                        .Parameter("PageSize", pageSize, DataTypes.Int32)
                        .ParameterOut("TotalRows", DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.News>();
                    totalRows = cmd.ParameterValue<int>("TotalRows");
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.News> GetListNewsByCateId(int languageId, int cateId, int top)
        {
            string storeName = "FE_News_GetListNewsByCateId_20200526";
            IEnumerable<Entities.News> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("LanguageId", 0, DataTypes.Int32)
                        .Parameter("CategoryId", cateId, DataTypes.Int32)
                        .Parameter("Top", top, DataTypes.Int32)
                        .Parameter("Status", (int)NewsStatusEnum.Published, DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.News>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
        public IEnumerable<Entities.News> GetListNewsHighlight(int top)
        {
            string storeName = "FE_News_GetListNews_Highlight";
            IEnumerable<Entities.News> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("Top", top, DataTypes.Int32)
                        .Parameter("Status", (int)NewsStatusEnum.Published, DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.News>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }
        public IEnumerable<Entities.News> GetListNewsHighlightByCate(int cateId, int top)
        {
            string storeName = "FE_News_GetListNews_HighlightByCate";
            IEnumerable<Entities.News> lstRet;
            try
            {
                using (IDbContext context = Context())
                {
                    IStoredProcedureBuilder cmd = context.StoredProcedure(storeName)
                        .Parameter("CateId", cateId, DataTypes.Int32)
                        .Parameter("Top", top, DataTypes.Int32)
                        .Parameter("Status", (int)NewsStatusEnum.Published, DataTypes.Int32);
                    lstRet = cmd.QueryMany<Entities.News>();
                    return lstRet;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public int Update(Entities.News news, List<Entities.NewsImage> listNewsImage)
        {
            string storeName = "Admin_News_Update_20220818";
            string storeInsertNewsImage = "Admin_NewsImage_Insert";
            string storeDeleteNewsImage = "Admin_NewsImage_DeleteByNewsId";
            int numberRecords = 0;
            var errors = string.Empty;

            using (IDbContext context = Context().UseTransaction(true))
            {
                try
                {
                    numberRecords = context.StoredProcedure(storeName)
                        .Parameter("Id", news.Id, DataTypes.Int32)
                        .Parameter("Type", news.Type, DataTypes.Int32)
                        .Parameter("Title", news.Title, DataTypes.String)
                        .Parameter("Sapo", news.Sapo, DataTypes.String)
                        .Parameter("Description", news.Description, DataTypes.String)
                        .Parameter("Avatar", news.Avatar, DataTypes.String)
                        .Parameter("Source", news.Source, DataTypes.String)
                        .Parameter("CategoryId", news.CategoryId, DataTypes.Int32)
                        .Parameter("Status", news.Status, DataTypes.Int32)
                        .Parameter("CreatedBy", news.CreatedBy, DataTypes.String)
                        .Parameter("ModifiedDate", DateTime.Now, DataTypes.DateTime)
                        .Parameter("ModifiedBy", news.ModifiedBy, DataTypes.String)
                        .Parameter("SEOTitle", news.SEOTitle, DataTypes.String)
                        .Parameter("SEODescription", news.SEODescription, DataTypes.String)
                        .Parameter("SEOKeyword", news.SEOKeyword, DataTypes.String)
                        .Parameter("TextSearch", news.TextSearch, DataTypes.String)
                        .Parameter("PublishedDate", news.PublishedDate, DataTypes.DateTime)
                        .Parameter("LanguageId", 0, DataTypes.Int32)
                        .Parameter("IsHighLight", news.IsHighLight, DataTypes.Int32)
                        .QuerySingle<int>();
                    if (numberRecords < 1)
                    {
                        errors = "Có lỗi ở sp " + storeName;
                    }
                    #region NewsImage
                    if (numberRecords > 0 && string.IsNullOrEmpty(errors))
                    {
                        //Xóa ds image cũ
                        context.StoredProcedure(storeDeleteNewsImage)
                            .Parameter("NewsId", news.Id, DataTypes.Int32)
                            .Execute();
                        //Insert ds image mới
                        if (listNewsImage != null && listNewsImage.Count > 0)
                        {
                            foreach (var image in listNewsImage)
                            {
                                numberRecords = context.StoredProcedure(storeInsertNewsImage)
                                    .Parameter("NewsId", news.Id, DataTypes.Int32)
                                    .Parameter("ImageUrl", image.ImageUrl, DataTypes.String)
                                    .Parameter("Title", image.Title, DataTypes.String)
                                    .QuerySingle<int>();
                                if (numberRecords < 1)
                                {
                                    errors = "Có lỗi ở sp " + storeInsertNewsImage;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    if (numberRecords > 0 && string.IsNullOrEmpty(errors))
                        context.Commit();
                    else
                        context.Rollback();
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
                }
            }
            return numberRecords;
        }

        public int UpdatePage(Entities.Page news)
        {
            string storeName = "Admin_Page_Update";

            int numberRecords = 0;
            var errors = string.Empty;

            using (IDbContext context = Context().UseTransaction(true))
            {
                try
                {
                    numberRecords = context.StoredProcedure(storeName)
                        .Parameter("Id", news.Id, DataTypes.Int32)
                        .Parameter("Title", news.Title, DataTypes.String)
                        .Parameter("Description", news.Description, DataTypes.String)
                        .Parameter("SEOTitle", news.SEOTitle, DataTypes.String)
                        .Parameter("SEODescription", news.SEODescription, DataTypes.String)
                        .Parameter("SEOKeyword", news.SEOKeyword, DataTypes.String)
                        .QuerySingle<int>();

                    if (numberRecords > 0 && string.IsNullOrEmpty(errors))
                        context.Commit();
                    else
                        context.Rollback();
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
                }
            }
            return numberRecords;
        } 
        public int UpdateVideo(Entities.Video news)
        {
            string storeName = "Admin_Video_Update";

            int numberRecords = 0;
            var errors = string.Empty;

            using (IDbContext context = Context().UseTransaction(true))
            {
                try
                {
                    numberRecords = context.StoredProcedure(storeName)
                        .Parameter("Id", news.Id, DataTypes.Int32)
                        .Parameter("Title", news.Title, DataTypes.String)
                        .Parameter("VideoUrl", news.VideoUrl, DataTypes.String)
                        .Parameter("Avatar", news.Avatar, DataTypes.String)
                        .Parameter("Link", news.Link, DataTypes.String)
                        .QuerySingle<int>();

                    if (numberRecords > 0 && string.IsNullOrEmpty(errors))
                        context.Commit();
                    else
                        context.Rollback();
                }
                catch (Exception ex)
                {
                    context.Rollback();
                    throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
                }
            }
            return numberRecords;
        }

        public IEnumerable<WIS.Entities.NewsImage> GetListImageByNewsId(int newsId)
        {
            string storeName = "Admin_NewsImage_GetByNewsId";
            try
            {
                using (var context = Context())
                {
                    return context.StoredProcedure(storeName)
                        .Parameter("NewsId", newsId, DataTypes.Int32)
                        .QueryMany<WIS.Entities.NewsImage>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} => {1}", storeName, ex.ToString()));
            }
        }

        public IEnumerable<Entities.News> GetListArticleSiteMap()
        {
            var listNews = new List<Entities.News>();
            try
            {
                using (var context = Context())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append(" select n.id,n.Title,n.PublishedDate,n.Avatar from News n where n.Status = 1 ");
                    listNews = context.Sql(query.ToString()).QueryMany<Entities.News>();
                    return listNews;
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
