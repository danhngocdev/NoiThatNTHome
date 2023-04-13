using DVG.WIS.DAL.PriceList;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business.PriceList
{
    public class PriceListBo : IPriceListBo
    {
        private IPriceListDal _priceListDal;

        public PriceListBo(IPriceListDal priceListDal)
        {
            _priceListDal = priceListDal;
        }

        public ErrorCodes Delete(int id)
        {
            ErrorCodes errorCodes = ErrorCodes.Success;
            try
            {
                int result = _priceListDal.Delete(id);
                if (result < 1)
                {
                    return ErrorCodes.BusinessError;
                }
            }
            catch (Exception ex)
            {
                errorCodes = ErrorCodes.BusinessError;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, id);
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }
            return errorCodes;
        }

        public Entities.PriceList GetById(int id)
        {
            try
            {
                return _priceListDal.GetById(id);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                return null;
            }
        }

        public IEnumerable<Entities.PriceList> GetList(string keyword, int pageIndex, int pageSize,int status, out int totalRows)
        {
            try
            {
                return _priceListDal.GetList( keyword,  pageIndex,  pageSize,status, out totalRows);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
                totalRows = 0;
                return null;
            }
        }

        public IEnumerable<PriceListModel> GetListAllFe()
        {
            throw new NotImplementedException();
        }

        public ErrorCodes Update(Entities.PriceList priceList)
        {
            ErrorCodes errorCode = ErrorCodes.Success;
            try
            {
                // Validate
                if (null != priceList && !string.IsNullOrEmpty(priceList.Name))
                {
                    WIS.Entities.PriceList priceListObj = new WIS.Entities.PriceList();
                    if (priceList.Id == 0)
                    {
                        priceList.CreateDate = DateTime.Now;
                    }
                    else
                    {
                        priceListObj = _priceListDal.GetById(priceList.Id);
                        priceListObj.Name = priceList.Name;
                        priceListObj.Price =priceList.Price;  
                        priceListObj.Status = priceList.Status;
                        priceListObj.Note = priceList.Note;
                        priceListObj.Unit = priceList.Unit;
                        priceList = priceListObj;
                    }
                    priceList.ModifiedDate = DateTime.Now;
                    // Insert/Update
                    int numberRecords = _priceListDal.Update(priceList);
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCodes.Exception;
                //DVG.WIS.LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex, category);
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", priceList.Id, ex.ToString()));
            }
            return errorCode;
        }
    }
}
