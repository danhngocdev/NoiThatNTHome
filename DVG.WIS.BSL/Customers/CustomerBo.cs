using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVG.WIS.Core;
using DVG.WIS.DAL.Customers;
using DVG.WIS.DAL.Recruitments;
using DVG.WIS.Entities;
using DVG.WIS.Entities.Conditions;
using DVG.WIS.Utilities;

namespace DVG.WIS.Business.Customers
{
    public class CustomerBo : ICustomerBo
    {
        private ICustomerDal _customerDal;

        public CustomerBo(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _customerDal.Delete(id);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public IEnumerable<Customer> ExportExcel(string startDateStr = null, string endDateStr = null)
        {
            try
            {
                DateTime? startDate;
                if (string.IsNullOrEmpty(startDateStr))
                    startDate = null;
                else
                    startDate = Utils.ConvertStringToDateTime(startDateStr, Const.NormalDateFormat);
                DateTime? endDate;
                if (string.IsNullOrEmpty(endDateStr))
                    endDate = null;
                else
                    endDate = Utils.ConvertStringToDateTime(endDateStr, Const.NormalDateFormat);

                return _customerDal.ExportExcel(startDate, endDate);

            }
            catch(Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public Customer GetById(int id)
        {
            try
            {
                return _customerDal.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return null;
        }

        public IEnumerable<Entities.Customer> GetList(CustomerCondition customer, out int totalRows)
        {
            try
            {
                return _customerDal.GetList(customer, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public ErrorCodes Update(Customer banner)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (banner == null || banner.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }

                int result = _customerDal.Update(banner);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public ErrorCodes UpdateSubcribe(Entities.Subscribe banner)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                if (banner == null || banner.Id < 0)
                {
                    return ErrorCodes.BusinessError;
                }

                int result = _customerDal.UpdateSubcribe(banner);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }
    }
}
