using DVG.WIS.LogES.Models;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel.CMS
{
    public class LogExceptionParamModel
    {
        public LogExceptionParamModel()
        {
            this.ListLogSourceType = EnumHelper.Instance.ConvertEnumToList<LogSourceTypeEnums>().ToList();
            this.ListLogErrorSeverity = EnumHelper.Instance.ConvertEnumToList<LogErrorSeverityEnums>().ToList();
        }

        public List<EnumHelper.Enums> ListLogSourceType { get; set; }
        public List<EnumHelper.Enums> ListLogErrorSeverity { get; set; }
    }

    public class LogExceptionSearchModel
    {
        public string UserName { get; set; }
        public int SourceType { get; set; }
        public string ErrorCode { get; set; }
        public int ErrorSeverity { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class LogExceptionModelOnList
    {
        public LogExceptionModelOnList(ExceptionModel model)
        {
            logId = model.logId;
            createdTime = model.createdTime;
            message = model.message;
            stackTrace = model.stackTrace;
            userName = model.userName;
            jsonInput = model.jsonInput;
            className = model.className;
            methodName = model.methodName;
            errorCode = model.errorCode;
            errorSeverity = model.errorSeverity;
            sourceType = model.sourceType;
        }
        public string logId { get; set; }
        public long createdTime { get; set; }
        public string createdTimeStr
        {
            get
            {
                return Utils.UnixTimeStampToDateTime(createdTime).ToString();
            }
        }
        public string message { get; set; }
        public string stackTrace { get; set; }
        public string userName { get; set; }
        public string jsonInput { get; set; }
        public string className { get; set; }
        public string methodName { get; set; }
        public int errorSeverity { get; set; }
        public string errorSeverityStr
        {
            get
            {
                switch (errorSeverity)
                {
                    case (int)LogErrorSeverityEnums.Debug:
                        return StringUtils.GetEnumDescription(LogErrorSeverityEnums.Debug);
                    case (int)LogErrorSeverityEnums.Error:
                        return StringUtils.GetEnumDescription(LogErrorSeverityEnums.Error);
                    case (int)LogErrorSeverityEnums.Fatal:
                        return StringUtils.GetEnumDescription(LogErrorSeverityEnums.Fatal);
                    case (int)LogErrorSeverityEnums.Info:
                        return StringUtils.GetEnumDescription(LogErrorSeverityEnums.Info);
                    case (int)LogErrorSeverityEnums.Warning:
                        return StringUtils.GetEnumDescription(LogErrorSeverityEnums.Warning);
                }
                return "Không xác định";
            }
        }
        public string errorCode { get; set; }
        public int sourceType { get; set; }
        public string sourceTypeStr
        {
            get
            {
                switch (sourceType)
                {
                    case (int)LogSourceTypeEnums.CMS:
                        return StringUtils.GetEnumDescription(LogSourceTypeEnums.CMS);
                    case (int)LogSourceTypeEnums.FE:
                        return StringUtils.GetEnumDescription(LogSourceTypeEnums.FE);
                    case (int)LogSourceTypeEnums.WS:
                        return StringUtils.GetEnumDescription(LogSourceTypeEnums.WS);
                    case (int)LogSourceTypeEnums.PrivateAPI:
                        return StringUtils.GetEnumDescription(LogSourceTypeEnums.PrivateAPI);
                    case (int)LogSourceTypeEnums.APDSite:
                        return StringUtils.GetEnumDescription(LogSourceTypeEnums.APDSite);
                    case (int)LogSourceTypeEnums.APDWorker:
                        return StringUtils.GetEnumDescription(LogSourceTypeEnums.APDWorker);
                }
                return "Không xác định";
            }
        }
    }
}
