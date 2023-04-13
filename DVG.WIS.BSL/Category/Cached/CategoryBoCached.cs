using System.Collections.Generic;
using System.Linq;
using DVG.WIS.PublicModel;
using System.Web;
using DVG.WIS.Caching.Cached;
using DVG.WIS.Core.Constants;

namespace DVG.WIS.Business.Category.Cached
{
    public class CategoryBoCached : ICategoryBoCached
    {
        private ICached _cacheClient;

        private ICategoryBoFE _categoryBo;
        private int _longExpiredInMinute = StaticVariable.LongCacheTime;
        private int _shortExpiredInMinute = StaticVariable.ShortCacheTime;
        private int _mediumExpiredInMinute = StaticVariable.MediumCacheTime;

        public static IEnumerable<WIS.Entities.Category> ListAllOfCategory;

        public CategoryBoCached(ICategoryBoFE categoryBo, ICached cacheClient)
        {
            this._categoryBo = categoryBo;
            this._cacheClient = cacheClient;
        }

        public IEnumerable<WIS.Entities.Category> GetListAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request.UserAgent != null)
            {
                if (HttpContext.Current.Request.UserAgent.Contains("refreshcache"))
                {
                    ListAllOfCategory = null;
                }
            }
            if (ListAllOfCategory != null && ListAllOfCategory.Any())
            {
                return ListAllOfCategory;
            }

            IEnumerable<WIS.Entities.Category> listCategory = _categoryBo.GetListAll();

            if (listCategory != null)
            {
                ListAllOfCategory = listCategory;
            }

            return listCategory;
        }

        public Entities.Category GetById(int id)
        {
            IEnumerable<WIS.Entities.Category> categoryStatic = GetListAll();
            if (categoryStatic != null && categoryStatic.Any())
            {
                return categoryStatic.FirstOrDefault(x => x.Id == id);
            }
            return null;
        }

        public Entities.Category GetByUrl(string url)
        {
            IEnumerable<WIS.Entities.Category> categoryStatic = GetListAll();

            if (ListAllOfCategory != null && ListAllOfCategory.Any())
            {
                categoryStatic = ListAllOfCategory.Where(x => x.ShortURL == url);

                if (categoryStatic != null && categoryStatic.Any())
                {
                    return categoryStatic.FirstOrDefault();
                }
            }

            return null;
        }


    }
}
