using FileManager.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace DVG.WIS.CMS.FileManager.Handler
{

    /// <summary>
    /// Summary description for CKEditorUpload
    /// </summary>
    public class CKEditorUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            var response = new CKEditorUploadResponse();
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    response.message = "Authenticated Failed!";
                }

                string userName = HttpContext.Current.User.Identity.Name;
                FileStorage.AESIV = Config.AESIV;
                FileStorage.AESKey = Config.AESKey;
                var hfToken = FileStorage.AESEncrypt(userName + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                var file = context.Request.Files[0];

                var nvc = new NameValueCollection
                {
                    { "project", Utilities.FileStorage.FileStorage._UploadProject },
                    { "UploadType", "upload" },
                    { "StringDecypt", hfToken },
                    { "submit", "Upload Image" }
                };

                var kq = Utilities.FileStorage.FileStorage.HttpUploadFile(string.Concat(Utilities.FileStorage.FileStorage._UploadDomain, Utilities.FileStorage.FileStorage._UploadHandler),
                    file, "fileToUpload", file.ContentType, nvc);
                var obj = Utilities.NewtonJson.Deserialize<CKEditorUploadResponseObj>(kq);

                //// lấy thông tin về ảnh
                //if (!string.IsNullOrEmpty(obj.OK))
                //{
                //    nvc = new NameValueCollection
                //    {
                //        { "project", Utilities.FileStorage.FileStorage._UploadProject },
                //        { "UploadType", "info" },
                //        { "StringDecypt", hfToken },
                //        { "submit", "Check" },
                //        { "FileTemp", obj.OK }
                //    };

                //    var info = Utilities.FileStorage.FileStorage.HttpUploadFile(string.Concat(Utilities.FileStorage.FileStorage._UploadDomain, Utilities.FileStorage.FileStorage._UploadHandler),
                //        file, "fileToUpload", file.ContentType, nvc);
                //    //var data = new FormData();
                //    //data.append('project', uploadProject);
                //    //data.append('UploadType', 'info');
                //    //data.append('StringDecypt', $('#hfToken').val());
                //    //data.append('submit', 'Check');
                //    //data.append('FileTemp', path);
                //}

                response.filename = file.FileName;
                response.uploaded = 1;
                response.url = Utilities.FileStorage.FileStorage._ViewDomain + obj.OK;

            }
            catch (Exception ex)
            {
                // DVG.WIS.// LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex);
                response.uploaded = 0;
                response.message = ex.Message;
            }

            var result = Utilities.NewtonJson.Serialize(response);
            context.Response.Write(result);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
    //{"fileName":"hinh-anh-gau-bong-38.jpg","uploaded":1,"url":"\/userfiles\/images\/hinh-anh-gau-bong-38.jpg"}
    public class CKEditorUploadResponse
    {
        public string filename { get; set; }
        public int uploaded { get; set; }
        public string url { get; set; }
        public string message { get; set; }
    }

    [Serializable]
    public class CKEditorUploadResponseObj
    {
        public string OK { get; set; }
    }

    //{"Success":true,"Width":960,"Height":1280,"MimeType":"image\/jpeg","FileSize":323763}
    [Serializable]
    public class CKEditorUploadFileInfoObj
    {
        public bool Success { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string MimeType { get; set; }
        public int FileSize { get; set; }
    }
}
