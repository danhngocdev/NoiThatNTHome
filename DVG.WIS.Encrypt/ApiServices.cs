using System;
using System.Collections.Generic;
using DVG.WIS.Utilities;

namespace DVG.WIS.Encrypt
{
    public class ApiServices : IApiServices
    {
        private string _privateKey = string.Empty;
        private string _encryptKey;

        #region constructor

        public ApiServices()
        {
            this._encryptKey = AppSettings.Instance.GetString("EncryptKey");
        }

        public ApiServices(string key)
        {
            this._encryptKey = key;
        }

        #endregion

        public ResponseData Encrypt(string input)
        {
            ResponseData responseData = new ResponseData();
            ErrorCode errorCode;
            string result = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    errorCode = ErrorCode.InvalidRequest;
                    result = string.Empty;
                }
                else
                {
                    result = Crypton.EncryptByKey(input, _encryptKey);
                    errorCode = ErrorCode.Success;
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCode.Exception;
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", input, ex));
            }

            if (string.IsNullOrEmpty(result))
                result = input;

            responseData.Success = errorCode == ErrorCode.Success;
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            responseData.Result = result;

            return responseData;
        }

        public ResponseData Decrypt(string input)
        {
            ResponseData responseData = new ResponseData();
            ErrorCode errorCode;
            string result = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    errorCode = ErrorCode.InvalidRequest;
                    result = string.Empty;
                }
                else
                {
                    result = Crypton.DecryptByKey(input, _encryptKey);
                    errorCode = ErrorCode.Success;
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCode.Exception;
                Logger.WriteLog(Logger.LogType.Error, string.Format("{0} => {1}", input, ex));
            }

            if (string.IsNullOrEmpty(result))
                result = input;

            responseData.Success = errorCode == ErrorCode.Success;
            responseData.Message = StringUtils.GetEnumDescription(errorCode);
            responseData.Result = result;

            return responseData;
        }

        public ResponseMultiData Encrypt(List<RequestData> requestData)
        {
            ResponseMultiData responseData = new ResponseMultiData();
            ErrorCode errorCode;

            try
            {
                if (requestData == null || requestData.Count <= 0)
                {
                    errorCode = ErrorCode.InvalidRequest;
                }
                else
                {
                    foreach (RequestData request in requestData)
                    {
                        responseData.ResponseDatas.Add(Encrypt(request.Input));
                    }
                    errorCode = ErrorCode.Success;
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCode.Exception;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            responseData.Success = errorCode == ErrorCode.Success;
            responseData.Message = StringUtils.GetEnumDescription(errorCode);

            return responseData;
        }

        public ResponseMultiData Decrypt(List<RequestData> requestData)
        {
            ResponseMultiData responseData = new ResponseMultiData();
            ErrorCode errorCode;

            try
            {
                if (requestData == null || requestData.Count <= 0)
                {
                    errorCode = ErrorCode.InvalidRequest;
                }
                else
                {
                    foreach (RequestData request in requestData)
                    {
                        responseData.ResponseDatas.Add(Decrypt(request.Input));
                    }
                    errorCode = ErrorCode.Success;
                }
            }
            catch (Exception ex)
            {
                errorCode = ErrorCode.Exception;
                Logger.WriteLog(Logger.LogType.Error, ex.ToString());
            }

            responseData.Success = errorCode == ErrorCode.Success;
            responseData.Message = StringUtils.GetEnumDescription(errorCode);

            return responseData;
        }
    }
}