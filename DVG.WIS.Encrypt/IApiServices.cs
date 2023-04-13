using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DVG.WIS.Encrypt
{
    public interface IApiServices
    {
        ResponseData Encrypt(string input);

        ResponseData Decrypt(string input);

        ResponseMultiData Encrypt(List<RequestData> requestData);

        ResponseMultiData Decrypt(List<RequestData> requestData);
    }
}
