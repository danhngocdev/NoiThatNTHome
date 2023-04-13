using System.Collections.Generic;

namespace DVG.WIS.DAL.Category
{
    public interface ICategoryDal
    {
        int Update(WIS.Entities.Category category);

        bool Delete(int id, string deletedBy);

        WIS.Entities.Category GetById(int id);

        IEnumerable<WIS.Entities.Category> GetListByParent(int parentId = 0);

        IEnumerable<WIS.Entities.Category> GetList(string keyword, int parentId);

        IEnumerable<WIS.Entities.CategoryModel> GetListPaging(string keyword, int parentId, int newsType, int pageIndex, int pageSize, out int totalRows);
        IEnumerable<WIS.Entities.CategoryModel> GetListByType(int type, int status);
		IEnumerable<WIS.Entities.Category> GetByStatus(int status);
        IEnumerable<WIS.Entities.CategoryModel> GetByStatusV2(int status);

        IEnumerable<WIS.Entities.NewsCategory> GetListNewsCategoryByNewsId(long newsId);

        IEnumerable<WIS.Entities.Category> GetListAll();
    }
}
