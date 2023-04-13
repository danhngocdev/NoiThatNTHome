using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using DVG.WIS.Utilities;

namespace DVG.WIS.Encrypt
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, AddressFilterMode = AddressFilterMode.Any)]
    public class EncryptService : IEncryptService
    {
        private IApiServices _apiServices;

        public EncryptService()
        {
            string key = AppSettings.Instance.GetString("EncryptKey");
            _apiServices = new ApiServices(key);
        }

        public ResponseData Encrypt(RequestData requestData)
        {
            return _apiServices.Encrypt(requestData.Input);
        }

        public ResponseData Decrypt(RequestData requestData)
        {
            return _apiServices.Decrypt(requestData.Input);
        }

        public ResponseMultiData Encrypt(List<RequestData> requestData)
        {
            return _apiServices.Encrypt(requestData);
        }

        public ResponseMultiData Decrypt(List<RequestData> requestData)
        {
            return _apiServices.Decrypt(requestData);
        }

        public string GetDate()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
