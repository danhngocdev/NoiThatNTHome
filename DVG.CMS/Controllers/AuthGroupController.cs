using DVG.WIS.Business.AuthAction;
using DVG.WIS.Business.AuthGroup;
using DVG.WIS.Business.AuthGroupActionMapping;
using DVG.WIS.Business.AuthGroupCategoryMapping;
using DVG.WIS.Business.AuthGroupNewsStatusMapping;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVG.WIS.Utilities;
using DVG.WIS.Core.Enums;
using DVG.CMS.Models;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Category;

namespace DVG.CMS.Controllers
{
    public class AuthGroupController : Controller
    {
        private IAuthGroupBo _authGroupBo;
        private IAuthActionBo _authActionpBo;
        private ICategoryBo _categoryBo;
        private IAuthGroupCategoryMappingBo _authGroupCategoryMappingBo;
        private IAuthGroupActionMappingBo _authGroupActionMappingBo;
        private IAuthGroupNewsStatusMappingBo _authGroupNewsStatusMappingBo;
        private IEnumerable<AuthAction> _lstAuthAction;
        private IEnumerable<Category> _lstCategory;
        private IEnumerable<EnumHelper.Enums> _lstNewsType;
        private IEnumerable<EnumHelper.Enums> _lstAuthGroupStatus;
        private int _bufferNewsTypeId = 10000;
        public AuthGroupController(IAuthGroupBo authGroupBo, IAuthActionBo authActionpBo, ICategoryBo categoryBo
            , IAuthGroupCategoryMappingBo authGroupCategoryMappingBo, IAuthGroupActionMappingBo authGroupActionMappingBo, IAuthGroupNewsStatusMappingBo authGroupNewsStatusMappingBo)
        {
            _authGroupBo = authGroupBo;
            _authActionpBo = authActionpBo;
            _categoryBo = categoryBo;
            _authGroupCategoryMappingBo = authGroupCategoryMappingBo;
            _authGroupActionMappingBo = authGroupActionMappingBo;
            _authGroupNewsStatusMappingBo = authGroupNewsStatusMappingBo;
            _lstNewsType = EnumHelper.Instance.ConvertEnumToList<NewsTypeEnum>();
            _lstCategory = _categoryBo.GetByStatus();
            _lstAuthAction = _authActionpBo.GetAll();
            _lstAuthGroupStatus = EnumHelper.Instance.ConvertEnumToList<AuthGroupStatusEnum>().ToList();
        }


        [IsValidUrlRequestAttribute(KeyName = "AuthGroupController.Index", Description = "Phân quyền - Danh sách")]
        public ActionResult Index()
        {
            var lstAuthActionDropdown = _lstAuthAction.Select(x => new DropdownTreeCheckboxModel
            {
                Id = x.Id,
                Name = x.Description + " [" + x.KeyName + "]",
                Checked = false
            });

            var lstNewsStatusDropdown = EnumHelper.Instance.ConvertEnumToList<NewsStatusAuthenEnum>().Select(x => new DropdownTreeCheckboxModel
            {
                Id = x.Id,
                Name = x.Name,
                Checked = false
            });

            var model = new AuthGroupInitModel(lstAuthActionDropdown, GetAuthGroupCategoryTreeCheckboxModel(), _lstAuthGroupStatus, lstNewsStatusDropdown);
            return View(model);
        }

        [HttpPost]
        [IsValidUrlRequestAttribute(KeyName = "AuthGroupController.GetAll", Description = "Phân quyền - Danh sách")]
        public ActionResult GetAll()
        {
            ResponseData responseData = new ResponseData();
            var lstAuthGroup = _authGroupBo.GetAll();

            if (lstAuthGroup != null)
            {
                var models = lstAuthGroup.Select(x => new AuthGroupModel(x));
                responseData.Data = models;
                responseData.Success = true;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
            }
            else
            {
                responseData.Data = null;
                responseData.Success = false;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Exception);
            }
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequestAttribute(KeyName = "AuthGroupController.GetById", Description = "Phân quyền - Xem chi tiết")]
        public ActionResult GetById(int id)
        {
            ResponseData responseData = new ResponseData();
            if (id > 0)
            {
                var authGroup = _authGroupBo.GetById(id);
                var lstActionMapping = _authGroupActionMappingBo.GetByGrouId(id);
                var lstCategoryMapping = _authGroupCategoryMappingBo.GetByGrouId(id);
                var lstNewsStatusMapping = _authGroupNewsStatusMappingBo.GetByGrouId(id);
                if (authGroup != null)
                {
                    var lstSelectedCategory = GetAuthGroupCategoryTreeCheckboxModel();
                    foreach (var type in lstSelectedCategory)
                    {
                        if (lstCategoryMapping.Where(x => x.CategoryId == 0 && x.NewsType + _bufferNewsTypeId == type.Id).Any())
                        {
                            type.Checked = true;
                        }
                        foreach (var cat in type.Children)
                        {
                            if (lstCategoryMapping.Where(x => x.CategoryId == cat.Id).Any())
                            {
                                cat.Checked = true;
                            }
                        }
                    }

                    var lstAuthActionDropdown = _lstAuthAction.Select(x => new DropdownTreeCheckboxModel
                    {
                        Id = x.Id,
                        Name = x.Description + " [" + x.KeyName + "]",
                        Checked = lstActionMapping.Any(y => y.AthActionId == x.Id)
                    });


                    var lstNewsStatusDropdown = EnumHelper.Instance.ConvertEnumToList<NewsStatusAuthenEnum>().Select(x => new DropdownTreeCheckboxModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Checked = lstNewsStatusMapping.Any(y => y.Status == x.Id)
                    });


                    responseData.Data = new AuthGroupViewModel(authGroup, lstAuthActionDropdown, lstSelectedCategory, lstNewsStatusDropdown);
                    responseData.Success = true;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BusinessError);
                }
            }
            else
            {
                responseData.Success = false;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.InvalidRequest);
            }

            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequestAttribute(KeyName = "AuthGroupController.Create", Description = "Phân quyền - Tạo mới")]
        public ActionResult Create(AuthGroupUpdateModel model)
        {
            ResponseData responseData = new ResponseData();
            responseData.Success = true;
            responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);

            if (model != null && model.AuthGroup != null)
            {
                string userName = AuthenService.GetUserLogin().UserName;
                model.AuthGroup.CreatedBy = userName;
                model.AuthGroup.ModifiedBy = userName;

                var id = _authGroupBo.Insert(model.AuthGroup);
                if (id > 0)
                {
                    if (model.LstAuthAction != null && model.LstAuthAction.Any())
                    {
                        foreach (var item in model.LstAuthAction)
                        {
                            var actionResult = _authGroupActionMappingBo.Insert(new AuthGroupActionMapping
                            {
                                AuthGroupId = id,
                                AthActionId = item.Id,
                                CreatedBy = userName
                            });
                        }
                    }

                    if (model.LstCategoryModel != null && model.LstCategoryModel.Any())
                    {
                        foreach (var type in model.LstCategoryModel)
                        {
                            if (type.Checked)
                            {
                                var typeResult = _authGroupCategoryMappingBo.Insert(new AuthGroupCategoryMapping
                                {
                                    AuthGroupId = id,
                                    NewsType = (short)(type.Id - _bufferNewsTypeId),
                                    CategoryId = 0,
                                    CreatedBy = userName
                                });
                            }

                            if (type.Children != null && type.Children.Any())
                            {
                                foreach (var cat in type.Children)
                                {
                                    if (cat.Checked)
                                    {
                                        var catResult = _authGroupCategoryMappingBo.Insert(new AuthGroupCategoryMapping
                                        {
                                            AuthGroupId = id,
                                            NewsType = (short)(type.Id - _bufferNewsTypeId),
                                            CategoryId = cat.Id,
                                            CreatedBy = userName
                                        });
                                    }
                                }
                            }
                        }
                    }

                    if (model.LstNewsStatus != null && model.LstNewsStatus.Any())
                    {
                        foreach (var item in model.LstNewsStatus)
                        {
                            var actionResult = _authGroupNewsStatusMappingBo.Insert(new AuthGroupNewsStatusMapping
                            {
                                AuthGroupId = id,
                                Status = item.Id,
                                CreatedBy = userName
                            });
                        }
                    }
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BusinessError);
                }
            }
            else
            {
                responseData.Success = false;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.InvalidRequest);
            }
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequestAttribute(KeyName = "AuthGroupController.Update", Description = "Phân quyền - Cập nhật")]
        public ActionResult Update(AuthGroupUpdateModel model)
        {
            ResponseData responseData = new ResponseData();
            responseData.Success = true;
            responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);

            if (model != null && model.AuthGroup != null && model.AuthGroup.Id > 0)
            {
                string userName = AuthenService.GetUserLogin().UserName;
                model.AuthGroup.CreatedBy = userName;
                model.AuthGroup.ModifiedBy = userName;
                var result = _authGroupBo.Update(model.AuthGroup);
                if (result == ErrorCodes.Success)
                {
                    var id = model.AuthGroup.Id;

                    // action
                    var resultDeleteActionMapping = _authGroupActionMappingBo.DeleteByGroupId(id);
                    if (resultDeleteActionMapping == ErrorCodes.Success)
                    {
                        if (model.LstAuthAction != null && model.LstAuthAction.Any())
                        {
                            foreach (var item in model.LstAuthAction)
                            {
                                var actionResult = _authGroupActionMappingBo.Insert(new AuthGroupActionMapping
                                {
                                    AuthGroupId = id,
                                    AthActionId = item.Id,
                                    CreatedBy = userName
                                });
                            }
                        }
                    }
                    else
                    {
                        responseData.Success = false;
                        responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BusinessError);
                    }

                    //category
                    var resultDeleteCategoryMapping = _authGroupCategoryMappingBo.DeleteByGroupId(id);
                    if (resultDeleteCategoryMapping == ErrorCodes.Success)
                    {
                        if (model.LstCategoryModel != null && model.LstCategoryModel.Any())
                        {
                            foreach (var type in model.LstCategoryModel)
                            {
                                if (type.Checked)
                                {
                                    var typeResult = _authGroupCategoryMappingBo.Insert(new AuthGroupCategoryMapping
                                    {
                                        AuthGroupId = id,
                                        NewsType = (short)(type.Id - _bufferNewsTypeId),
                                        CategoryId = 0,
                                        CreatedBy = userName
                                    });
                                }

                                if (type.Children != null && type.Children.Any())
                                {
                                    foreach (var cat in type.Children)
                                    {
                                        if (cat.Checked)
                                        {
                                            var catResult = _authGroupCategoryMappingBo.Insert(new AuthGroupCategoryMapping
                                            {
                                                AuthGroupId = id,
                                                NewsType = (short)(type.Id - _bufferNewsTypeId),
                                                CategoryId = cat.Id,
                                                CreatedBy = userName
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        responseData.Success = false;
                        responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BusinessError);
                    }

                    // news status
                    var resultDeleteNewsStatusMapping = _authGroupNewsStatusMappingBo.DeleteByGroupId(id);
                    if (resultDeleteNewsStatusMapping == ErrorCodes.Success)
                    {
                        if (model.LstNewsStatus != null && model.LstNewsStatus.Any())
                        {
                            foreach (var item in model.LstNewsStatus)
                            {

                                var typeResult = _authGroupNewsStatusMappingBo.Insert(new AuthGroupNewsStatusMapping
                                {
                                    AuthGroupId = id,
                                    Status = item.Id,
                                    CreatedBy = userName
                                });

                            }
                        }
                    }
                    else
                    {
                        responseData.Success = false;
                        responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BusinessError);
                    }
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BusinessError);
                }
            }
            else
            {
                responseData.Success = false;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.InvalidRequest);
            }
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequestAttribute(KeyName = "AuthGroupController.Delete", Description = "Phân quyền - Xóa")]
        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            if (id > 0)
            {
                var result = _authGroupBo.Delete(id);
                if (result == ErrorCodes.Success)
                {
                    var resultDeleteActionMapping = _authGroupActionMappingBo.DeleteByGroupId(id);
                    var resultDeleteCategoryMapping = _authGroupCategoryMappingBo.DeleteByGroupId(id);
                    var resultDeleteNewsStatusMapping = _authGroupNewsStatusMappingBo.DeleteByGroupId(id);
                    responseData.Success = true;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BusinessError);
                }
            }
            else
            {
                responseData.Success = false;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.InvalidRequest);
            }
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequestAttribute(KeyName = "AuthGroupController.ChangeStatus", Description = "Phân quyền - Đổi trạng thái")]
        public ActionResult ChangeStatus(int id)
        {
            ResponseData responseData = new ResponseData();
            responseData.Success = true;
            responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
            if (id > 0)
            {
                var authGroup = _authGroupBo.GetById(id);
                if (authGroup != null)
                {
                    authGroup.Status = authGroup.Status == (short)AuthGroupStatusEnum.Active ? (short)AuthGroupStatusEnum.Deactive : (short)AuthGroupStatusEnum.Active;
                    var result = _authGroupBo.Update(authGroup);
                    if (result != ErrorCodes.Success)
                    {
                        responseData.Success = false;
                        responseData.Message = StringUtils.GetEnumDescription(result);
                    }
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.BusinessError);
                }
            }
            else
            {
                responseData.Success = false;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.InvalidRequest);
            }

            return Json(responseData);
        }

        #region private
        private List<DropdownTreeCheckboxModel> GetAuthGroupCategoryTreeCheckboxModel()
        {
            var lstCategoryTreeView = new List<DropdownTreeCheckboxModel>();
            foreach (var type in _lstNewsType)
            {
                var t = new DropdownTreeCheckboxModel
                {
                    Id = type.Id + _bufferNewsTypeId,
                    Name = type.Name
                };
                var lstCategory = _lstCategory.Where(x => x.Type == type.Id);
                if (lstCategory != null && lstCategory.Any())
                {
                    t.Children = lstCategory.Select(y => new DropdownTreeCheckboxModel { Id = y.Id, Name = y.Name }).ToList();
                }
                lstCategoryTreeView.Add(t);
            }
            return lstCategoryTreeView;
        }
        #endregion
    }
}