using DVG.WIS.Business.Category.Cached;
using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.PublicModel;
using DVG.WIS.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVG.WIS.Business.Menu
{
    public class MenuBo : IMenuBo
    {
        public static List<MenuTop> ListMenuTopStatic;

        private ICategoryBoCached _categoryBoCached;

        public MenuBo(ICategoryBoCached categoryBoCached)
        {
            this._categoryBoCached = categoryBoCached;
        }

        public List<MenuTop> GetListMenuTop()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request.UserAgent != null)
            {
                if (HttpContext.Current.Request.UserAgent.Contains("refreshcache"))
                {
                    ListMenuTopStatic = null;
                }
            }
            if (ListMenuTopStatic != null && ListMenuTopStatic.Any())
            {
                return ListMenuTopStatic;
            }

            // xử lý tạo list menu top
            List<MenuTop> listOfMenu = new List<MenuTop>();
            MenuTop menuLevel1 = new MenuTop();
            MenuModel menuInfo = new MenuModel();

            IEnumerable<Entities.Category> cateList = _categoryBoCached.GetListAll();

            if (cateList != null && cateList.Any())
            {
                #region Tin tức

                IEnumerable<Entities.Category> newsCategoriesParent = cateList.Where(x => x.ParentId == 0).ToList();

                if (newsCategoriesParent != null && newsCategoriesParent.Any())
                {
                    menuLevel1 = new MenuTop();

                    foreach (Entities.Category itemOfLevel1 in newsCategoriesParent)
                    {
                        menuLevel1 = new MenuTop
                        {
                            Parent = new MenuModel(itemOfLevel1, BuildLinkHelper.BuildURLForCategory)
                        };

                        IEnumerable<Entities.Category> newsCategoriesSub = cateList.Where(itemInListOfLevel2 => itemInListOfLevel2.ParentId == itemOfLevel1.Id).ToList();
                        if (newsCategoriesSub != null && newsCategoriesSub.Any())
                        {
                            MenuTop menuLevel2 = new MenuTop();
                            List<MenuTop> lstMenu2 = new List<MenuTop>();

                            foreach (var itemOfLevel2 in newsCategoriesSub)
                            {
                                menuLevel2 = new MenuTop
                                {
                                    Parent = new MenuModel(itemOfLevel2, BuildLinkHelper.BuildURLForCategory)
                                };

                                IEnumerable<Entities.Category> newsCategoriesSub2 = cateList.Where(itemInListOfLevel3 => itemInListOfLevel3.ParentId == itemOfLevel2.Id).ToList();
                                if (newsCategoriesSub != null && newsCategoriesSub.Any())
                                {
                                    MenuTop menuLevel3 = new MenuTop();
                                    List<MenuTop> lstMenu3 = new List<MenuTop>();

                                    foreach (var itemOfLevel3 in newsCategoriesSub2)
                                    {
                                        menuLevel3 = new MenuTop
                                        {
                                            Parent = new MenuModel(itemOfLevel3, BuildLinkHelper.BuildURLForCategory)
                                        };
                                        lstMenu3.Add(menuLevel3);
                                    }

                                    menuLevel2.HasSubMenu = lstMenu3 != null && lstMenu3.Any();
                                    menuLevel2.SubMenu = lstMenu3;
                                }
                                else menuLevel2.HasSubMenu = false;

                                lstMenu2.Add(menuLevel2);
                            }

                            menuLevel1.HasSubMenu = lstMenu2 != null && lstMenu2.Any();
                            menuLevel1.SubMenu = lstMenu2;
                        }
                        else menuLevel1.HasSubMenu = false;

                        listOfMenu.Add(menuLevel1);
                    }
                }

                #endregion
            }

            ListMenuTopStatic = listOfMenu;
            return listOfMenu;
        }
    }
}
