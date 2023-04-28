using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.News
{
    public interface INewsBo
    {
        IEnumerable<Entities.News> GetList(int cateId, int status, string keyword, int pageIndex, int pageSize, out int totalRows);
        Entities.News GetById(int id);
        Entities.Page GetPageById(int id);
        Entities.Video GetVideoById(int id);
        ErrorCodes Update(Entities.News news, List<Entities.NewsImage> listNewsImage);
        ErrorCodes UpdatePage(Entities.Page news);
        ErrorCodes UpdateVideo(Entities.Video news);
        ErrorCodes ChangeStatusNews(int id, int statusNews, string changeBy, DateTime publishedDate);
        IEnumerable<NewsInListModel> GetListFE(int languageId, int cateId, int pageIndex, int pageSize, out int totalRows);
        IEnumerable<WIS.Entities.NewsImage> GetListImageByNewsId(int newsId);

        IEnumerable<NewsInListModel> GetListNewsByCateId(int languageId,int cateId, int top);
        IEnumerable<NewsInListModel> GetListNewsHighlight(int top);
        IEnumerable<NewsInListModel> GetListNewsHighlightByCate(int cateId,int top);

        IEnumerable<NewsInListModel> GetListArticleSiteMap();

    }
}
