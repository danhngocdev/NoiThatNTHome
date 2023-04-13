using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Persons;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVG.CMS.Controllers
{
    public class PersonController : Controller
    {
        private IPersonBo _personBo;
        public PersonController(IPersonBo personBo)
        {
            _personBo = personBo;
        }

        // GET: Person
        [IsValidUrlRequest(KeyName = "PersonController.Index", Description = "Person - Danh sách")]
        public ActionResult Index(int? bannerId)
        {
            PersonSearchModel model = new PersonSearchModel();
            model.ListPage = EnumHelper.Instance.ConvertEnumToList<CustomerSourceEnum>().Where(x => x.Id != 5 && x.Id != 6);
            return View(model);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "PersonController.Search", Description = "Person - Danh sách")]
        public ActionResult Search(PersonSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRows = 0;
            var lstRet = _personBo.GetList(searchModel.Name, searchModel.PageId, searchModel.Status,
                searchModel.PageIndex, searchModel.PageSize, out totalRows);
            if (null != lstRet)
            {
                searchModel.EditItem = new PersonModel();
                searchModel.ListData = lstRet.Select(item => new PersonModel(item)).ToList();
                responseData.Data = searchModel;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
            }
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "PersonController.Search", Description = "Person - Cập nhật")]
        public ActionResult Update(PersonModel model)
        {
            ResponseData responseData = new ResponseData();

            if (null != model)
            {
                Person person = model.Id > 0 ? _personBo.GetById(model.Id) : new Person();
                person.Name = model.Name;
                person.Avatar = model.AvatarStr
                    .Replace(StaticVariable.DomainImage.TrimEnd('/'), string.Empty)
                    .Replace(AppSettings.Instance.GetString("CropSizeCMS"), string.Empty).TrimStart('/');
                person.Description = model.Description;
                person.Position = !string.IsNullOrEmpty(model.Position) ? model.Position : string.Empty;
                person.Age = model.Age;
                person.Score = !string.IsNullOrEmpty(model.Score) ? model.Score : string.Empty;
                person.Status = model.Status;
                person.PageId = model.PageId;
                person.Priority = model.Priority;
                if (model.Id == 0)
                {
                    person.CreatedDate = DateTime.Now;
                }
                person.ModifiedDate = DateTime.Now;
                string userName = AuthenService.GetUserLogin().UserName;
                person.ModifiedBy = userName;
                person.CreatedBy = userName;

                //Update banner
                ErrorCodes result = _personBo.Update(person);
                responseData.Success = result == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(result);
            }
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "PersonController.Delete", Description = "Person - Xóa")]
        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _personBo.Delete(id);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "PersonController.GetPerson", Description = "Person - Xem chi tiết")]
        public ActionResult GetPerson(int bannerId)
        {
            ResponseData responseData = new ResponseData();

            responseData.Success = true;
            responseData.Data = new PersonModel(_personBo.GetById(bannerId));

            return Json(responseData);
        }
    }
}