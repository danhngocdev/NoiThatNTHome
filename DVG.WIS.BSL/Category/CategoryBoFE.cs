using DVG.WIS.DAL.Category;
using DVG.WIS.Utilities;
using DVG.WIS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.PublicModel;

namespace DVG.WIS.Business.Category
{
    public class CategoryBoFE : ICategoryBoFE
    {
        private ICategoryDalFE _categoryDal;

        public CategoryBoFE(ICategoryDalFE categoryDal)
        {
            this._categoryDal = categoryDal;
        }

        public IEnumerable<WIS.Entities.Category> GetList(int parentId)
        {
            try
            {
                int status = (int) CategoryState.CategoryStatusEnum.Active;
                return _categoryDal.GetAllByParent(parentId, status);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }
        public WIS.Entities.Category GetById(int id)
        {
            try
            {
                return _categoryDal.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public IEnumerable<WIS.Entities.Category> GetListAll()
        {
            try
            {
                return _categoryDal.GetListAll();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

    }
}
