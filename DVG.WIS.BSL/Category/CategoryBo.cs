using DVG.WIS.DAL.Category;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using DVG.WIS.Core.Enums;
using DVG.WIS.PublicModel;
using DVG.WIS.Business.Category.Cached;
using System.Web;
using System.Linq;

namespace DVG.WIS.Business.Category
{
    public class CategoryBo : ICategoryBo
    {
        private ICategoryDal _categoryDal; 
        public CategoryBo(ICategoryDal categoryDal)
        {
            this._categoryDal = categoryDal;
        }
        public ErrorCodes Update(WIS.Entities.Category category)
        {
            ErrorCodes errorCode = ErrorCodes.Success;
            try
            {
                // Validate
                if (null != category && !string.IsNullOrEmpty(category.Name))
                {
                    WIS.Entities.Category categoryObj = new WIS.Entities.Category();
                    if (category.Id == 0)
                    {
                        category.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        categoryObj = _categoryDal.GetById(category.Id);
                        categoryObj.Name = category.Name;
                        categoryObj.ParentId = category.ParentId;
                        categoryObj.SortOrder = category.SortOrder;
                        categoryObj.Description = category.Description;
                        categoryObj.Invisibled = category.Invisibled;
                        categoryObj.AllowComment = category.AllowComment;
                        categoryObj.Status = category.Status;
                        categoryObj.Type = category.Type;
                        categoryObj.MetaTitle = !string.IsNullOrEmpty(category.MetaTitle) ? category.MetaTitle : category.Name;
                        categoryObj.MetaDescription = !string.IsNullOrEmpty(category.MetaDescription) ? category.MetaDescription : category.Description;
                        category = categoryObj;
                    }
                    category.ModifiedDate = DateTime.Now;
                    category.ShortURL = StringUtils.UnicodeToUnsignCharAndDash(category.Name);
                    // Insert/Update
                    int numberRecords = _categoryDal.Update(category);
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.Exception;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, category);
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", category.Id, ex.ToString()));
            }
            return errorCode;
        }

        public ErrorCodes Delete(int id, string deletedBy)
        {
            try
            {
                bool result = false;
                if (id > 0 && !string.IsNullOrEmpty(deletedBy))
                {
                    result = _categoryDal.Delete(id, deletedBy);
                }
                if (result) return ErrorCodes.Success;
                return ErrorCodes.UnknowError;
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id, deletedBy);
                return ErrorCodes.Exception;
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
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public IEnumerable<WIS.Entities.Category> GetListByParent(int parentId = 0)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WIS.Entities.Category> GetList(string keyword, int parentId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WIS.Entities.CategoryModel> GetListPaging(string keyword, int parentId, int newsType, int pageIndex, int pageSize, out int totalRows)
        {
            try
            {
                IEnumerable<WIS.Entities.CategoryModel> listCategories = _categoryDal.GetListPaging(keyword, parentId, newsType, pageIndex, pageSize, out totalRows);
                return listCategories;
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, keyword, parentId, newsType, pageIndex, pageSize);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public IEnumerable<CategoryModel> GetListByType(int type, int status = (int)CategoryState.CategoryStatusEnum.Active)
        {
            try
            {
                IEnumerable<CategoryModel> listCategories = _categoryDal.GetListByType(type, status);
                return listCategories;
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, type, status);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public IEnumerable<Entities.Category> GetByStatus(int status = (int)CategoryState.CategoryStatusEnum.Active)
        {
            try
            {
                IEnumerable<Entities.Category> listCategories = _categoryDal.GetByStatus(status);
                return listCategories;
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, status);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public IEnumerable<Entities.CategoryModel> GetByStatusV2(int status = (int)CategoryState.CategoryStatusEnum.Active)
        {
            try
            {
                IEnumerable<Entities.CategoryModel> listCategories = _categoryDal.GetByStatusV2(status);
                return listCategories;
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, status);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }


        public IEnumerable<WIS.Entities.NewsCategory> GetListNewsCategoryByNewsId(long newsId)
        {
            try
            {
                return _categoryDal.GetListNewsCategoryByNewsId(newsId);
            }
            catch (Exception ex)
            {
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, newsId);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public IEnumerable<Entities.Category> GetListAll()
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

        public List<MenuTop> GetListCateHome()
        {
            try
            {
                List<MenuTop> listOfMenu = new List<MenuTop>();
                MenuTop menuLevel1 = new MenuTop();
                MenuModel menuInfo = new MenuModel();
                var lstCate = _categoryDal.GetListAll();
                var listMenuParent = lstCate.Where(x => x.ParentId == 0).ToList();
                if (listMenuParent != null && listMenuParent.Any())
                {
                    menuLevel1 = new MenuTop();
                    foreach (var itemLevel1 in listMenuParent)
                    {
                        menuLevel1 = new MenuTop
                        {
                            Parent = new MenuModel(itemLevel1, BuildLinkHelper.BuildURLForCategory)
                        };
                        var newsCategoriesSub = lstCate.Where(itemInListOfLevel2 => itemInListOfLevel2.ParentId == itemLevel1.Id).ToList();
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

                                IEnumerable<Entities.Category> newsCategoriesSub2 = lstCate.Where(itemInListOfLevel3 => itemInListOfLevel3.ParentId == itemOfLevel2.Id).ToList();
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
                return listOfMenu;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }
    }
}
