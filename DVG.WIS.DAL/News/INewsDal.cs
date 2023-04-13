using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.DAL.News
{
    public interface INewsDal
    {
        IEnumerable<Entities.News> GetList(int cateId, int status, string keyword, int pageIndex, int pageSize, out int totalRows);
        Entities.News GetById(int id);
        Entities.Page GetPageById(int id);
        Entities.Video GetVideoById(int id);
        int Update(Entities.News banner, List<Entities.NewsImage> listNewsImage);
        int UpdatePage(Entities.Page banner);
        int UpdateVideo(Entities.Video banner);
        int ChangeStatusNews(int id, int statusNews, string changeBy, DateTime publishedDate);
        IEnumerable<Entities.News> GetListFE(int languageId, int cateId, int pageIndex, int pageSize, out int totalRows);
        IEnumerable<WIS.Entities.NewsImage> GetListImageByNewsId(int newsId);

        IEnumerable<Entities.News> GetListNewsByCateId(int languageId, int cateId, int top);
        IEnumerable<Entities.News> GetListNewsHighlight(int top);
        IEnumerable<Entities.News> GetListNewsHighlightByCate(int cateId,int top);

    }
}
