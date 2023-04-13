using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DVG.WIS.Encrypt
{
    public enum ErrorCode
    {
        [Description("Lỗi nghiệp vụ")]
        BusinessError = 500,
        [Description("Lỗi chưa xác định")]
        UnknowError = 501,
        [Description("Yêu cầu không hợp lệ")]
        InvalidRequest = 502,
        [Description("Lỗi ngoại lệ (Exception)")]
        Exception = 503,
        [Description("Thành công")]
        Success = 0,
    }
}