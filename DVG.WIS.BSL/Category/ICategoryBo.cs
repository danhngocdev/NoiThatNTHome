using DVG.WIS.Entities;
using System.Collections.Generic;
using DVG.WIS.Core.Enums;
using DVG.WIS.PublicModel;

namespace DVG.WIS.Business.Category
{
    public interface ICategoryBo
    {
        ErrorCodes Update(WIS.Entities.Category category);
        ErrorCodes Delete(int id, string deletedBy);
        WIS.Entities.Category GetById(int id);
        IEnumerable<WIS.Entities.Category> GetListByParent(int parentId = 0);
        IEnumerable<WIS.Entities.Category> GetList(string keyword, int parentId);
        IEnumerable<WIS.Entities.CategoryModel> GetListPaging(string keyword, int parentId, int newsType, int pageIndex, int pageSize, out int totalRows);
        IEnumerable<CategoryModel> GetListByType(int type, int status = (int)CategoryState.CategoryStatusEnum.Active);
		IEnumerable<WIS.Entities.Category> GetByStatus(int status = (int)CategoryState.CategoryStatusEnum.Active);
        IEnumerable<Entities.CategoryModel> GetByStatusV2(int status = (int)CategoryState.CategoryStatusEnum.Active);
        IEnumerable<WIS.Entities.NewsCategory> GetListNewsCategoryByNewsId(long newsId);
        IEnumerable<WIS.Entities.Category> GetListAll();
        List<MenuTop> GetListCateHome();

    }
}
