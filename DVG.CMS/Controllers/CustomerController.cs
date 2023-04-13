using DVG.WIS.Business.Customers;
using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DVG.CMS.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerBo _customerBo;

        public CustomerController(ICustomerBo CustomerBo)
        {
            _customerBo = CustomerBo;
        }

        [IsValidUrlRequest(KeyName = "CustomerController.Index", Description = "Customer - Danh sách")]
        public ActionResult Index(int? bannerId)
        {
            return View();
        }

        [HttpPost]
        [IsValidUrlRequest(KeyName = "CustomerController.Search", Description = "Customer - Danh sách")]
        public ActionResult Search(CustomerSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            int totalRows = 0;
            if (string.IsNullOrEmpty(searchModel.StartDateStr))
                searchModel.StartDate = null;
            else
                searchModel.StartDate = Utils.ConvertStringToDateTime(searchModel.StartDateStr, Const.NormalDateFormat);
            if (string.IsNullOrEmpty(searchModel.EndDateStr))
                searchModel.EndDate = null;
            else
                searchModel.EndDate = Utils.ConvertStringToDateTime(searchModel.EndDateStr, Const.NormalDateFormat);

            CustomerCondition customerCondition = new CustomerCondition()
            {
                StartDate = searchModel.StartDate.HasValue ? searchModel.StartDate.Value.Ticks : 0,
                EndDate = searchModel.EndDate.HasValue ? searchModel.EndDate.Value.Ticks : 0,
                PageIndex = searchModel.PageIndex,
                PageSize = searchModel.PageSize
            };
            var lstRet = _customerBo.GetList(customerCondition, out totalRows);
            if (null != lstRet)
            {
                searchModel.EditItem = new CustomerModel();
                searchModel.ListData = lstRet.Select(item => new CustomerModel(item)).ToList();
                responseData.Data = searchModel;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
            }
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "CustomerController.Search", Description = "Customer - Cập nhật")]
        public ActionResult Update(CustomerModel model)
        {
            ResponseData responseData = new ResponseData();

            if (null != model)
            {
                Customer banner = model.Id > 0 ? _customerBo.GetById(model.Id) : new Customer();
                banner.Name = model.Name;
                banner.Address = model.Address;
                banner.Phone = model.Phone;
                banner.Email = model.Email;
                banner.Title = !string.IsNullOrEmpty(model.Title) ? model.Title : string.Empty;
                banner.Description = !string.IsNullOrEmpty(model.Description) ? model.Description : string.Empty;
                //Update banner
                ErrorCodes result = _customerBo.Update(banner);
                responseData.Success = result == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(result);
            }
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "CustomerController.Delete", Description = "Customer - Xóa")]
        public ActionResult Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            ErrorCodes result = _customerBo.Delete(id);
            responseData.Success = result == ErrorCodes.Success;
            responseData.Message = StringUtils.GetEnumDescription(result);
            return Json(responseData);
        }
        [HttpPost]
        [IsValidUrlRequest(KeyName = "CustomerController.GetCustomer", Description = "Customer - Xem chi tiết")]
        public ActionResult GetCustomer(int bannerId)
        {
            ResponseData responseData = new ResponseData();

            responseData.Success = true;
            responseData.Data = _customerBo.GetById(bannerId);

            return Json(responseData);
        }
        [HttpGet]
        [IsValidUrlRequest(KeyName = "CustomerController.ExportExcel", Description = "Customer - Export")]
        public ActionResult ExportExcel(string startDate = null, string endDate = null)
        {
            ResponseData responseData = new ResponseData();
            #region tạo bảng
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "STT";
            Sheet.Cells["B1"].Value = "Họ Tên";
            Sheet.Cells["C1"].Value = "Địa chỉ";
            Sheet.Cells["D1"].Value = "Số điện thoại";
            Sheet.Cells["E1"].Value = "Email";
            Sheet.Cells["F1"].Value = "Tiêu đề";
            Sheet.Cells["G1"].Value = "Nội dung";
            Sheet.Cells["H1"].Value = "Ngày tạo";
            #endregion

            var list = _customerBo.ExportExcel(startDate, endDate);
            if (null != list && list.Any())
            {
                int count = 0;
                int row = 2;
                foreach (var item in list)
                {
                    count++;
                    var obj = new ExportExcelResponseModel(item);
                    Sheet.Cells[string.Format("A{0}", row)].Value = count;
                    Sheet.Cells[string.Format("B{0}", row)].Value = obj.Name;
                    Sheet.Cells[string.Format("C{0}", row)].Value = obj.Address;
                    Sheet.Cells[string.Format("D{0}", row)].Value = obj.Phone;
                    Sheet.Cells[string.Format("E{0}", row)].Value = obj.Email;
                    Sheet.Cells[string.Format("F{0}", row)].Value = obj.Title;
                    Sheet.Cells[string.Format("G{0}", row)].Value = obj.Description;
                    Sheet.Cells[string.Format("H{0}", row)].Value = obj.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                    row++;
                }
                Sheet.Cells["A:AZ"].AutoFitColumns();
            }

            #region xuất dữ liệu
            Response.Clear();
            var _head = string.Format("attachment; filename={0}{1}.xlsx", "ExportListCustomer", DateTime.Now.ToString("-yyyyMMddHHmmss"));
            Response.AddHeader("Content-Disposition", _head);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.Flush();
            Response.End();
            #endregion
            responseData.Success = true;
            return Json(responseData);
        }
    }
}