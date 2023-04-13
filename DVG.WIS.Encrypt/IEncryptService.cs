using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DVG.WIS.Encrypt
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Name = "EncryptService")]
    public interface IEncryptService
    {

        [OperationContract(Name = "Encrypt")]
        [WebInvoke(Method = "POST", UriTemplate = "encode", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseData Encrypt(RequestData requestData);

        [OperationContract(Name = "Decrypt")]
        [WebInvoke(Method = "POST", UriTemplate = "decode", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseData Decrypt(RequestData requestData);

        [OperationContract(Name = "EncryptList")]
        [WebInvoke(Method = "POST", UriTemplate = "encode_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMultiData Encrypt(List<RequestData> requestData);

        [OperationContract(Name = "DecryptList")]
        [WebInvoke(Method = "POST", UriTemplate = "decode_list", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResponseMultiData Decrypt(List<RequestData> requestData);

        [OperationContract(Name = "GetDate")]
        [WebInvoke(Method = "GET", UriTemplate = "getdate", ResponseFormat = WebMessageFormat.Json)]
        string GetDate();
    }

}
