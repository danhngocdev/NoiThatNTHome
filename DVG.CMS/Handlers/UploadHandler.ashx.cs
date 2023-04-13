using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using DVG.WIS.Utilities;

namespace DVG.WIS.CMS
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
        private static String[] allowedExtension = { ".jpg", ".jpeg", ".bmp", ".gif", ".png", ".ico", ".emf" };

        public void ProcessRequest(HttpContext context)
        {
            ResponseUpload result = new ResponseUpload();

            var uploadPath = initDirectory(context);

            var uploadedFile = context.Request.Files["fileToUpload"];
            string uploadType = !string.IsNullOrEmpty(context.Request.Form["UploadType"]) ? context.Request.Form["UploadType"] : "upload";

            switch (uploadType.ToLower())
            {
                case "download":
                    result = UploadFromUrl(uploadPath, context);
                    break;
                case "upload":
                default:
                    result = UploadForm(uploadPath, context);
                    break;
            }

            string jsonResult = NewtonJson.Serialize(result);

            context.Response.ContentType = "application/json";
            context.Response.Write(jsonResult);
        }

        private bool IsValidFileType(string extensionFile)
        {
            var fileIsValid = allowedExtension.Any(ext => extensionFile != null && ext.ToLower().Equals(extensionFile.ToLower()));
            return fileIsValid;
        }

        private string initDirectory(HttpContext context)
        {
            var uploadPath = AppSettings.Instance.GetString(FileServerUpload) + DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "\\");

            if (!Directory.Exists(uploadPath))
            {
                try
                {
                    Directory.CreateDirectory(uploadPath);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.LogType.Fatal, string.Format(":{0}", ex));
                }
            }

            return uploadPath;
        }

        private ResponseUpload UploadForm(string uploadPath, HttpContext context)
        {
            ResponseUpload result = new ResponseUpload();

            var uploadedFile = context.Request.Files["fileToUpload"];
            var fileName = CreateHash() + Path.GetExtension(uploadedFile.FileName);

            if (uploadedFile.ContentLength > 0)
            {
                if (IsValidFileType(Path.GetExtension(fileName)))
                {
                    uploadedFile.SaveAs(uploadPath + "\\" + fileName);
                    Image img = Image.FromFile(uploadPath + "\\" + fileName);
                    result.Width = img.Width;
                    result.Height = img.Height;
                    result.FileSize = new FileInfo(uploadPath + "\\" + fileName).Length;

                    result.ErrorCode = 200;
                    result.Message = "Upload success";
                    result.Url = $"/photo/{DateTime.Now.ToString("yyyy/MM/dd")}/{ fileName}";

                }
                else
                {
                    result.ErrorCode = 403;
                    result.Message = "Invalid file type";
                }
            }
            else
            {
                result.ErrorCode = 500;
                result.Message = "Invalid file";
            }

            return result;
        }

        private ResponseUpload UploadFromUrl(string uploadPath, HttpContext context)
        {
            ResponseUpload result = new ResponseUpload();

            string imageURL = context.Request.Form["fileToUpload"];

            if (string.IsNullOrEmpty(imageURL)) return result;

            var fileName = imageURL.Substring(imageURL.LastIndexOf('/') + 1, imageURL.Length - imageURL.LastIndexOf('/') - 1);
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(imageURL);
                using (var httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebReponse.GetResponseStream())
                    {
                        if (null != stream)
                        {
                            var image = Image.FromStream(stream);

                            image.Save(uploadPath + "\\" + fileName);

                            Image img = Image.FromFile(uploadPath + "\\" + fileName);
                            result.Width = img.Width;
                            result.Height = img.Height;
                            result.FileSize = new FileInfo(uploadPath + "\\" + fileName).Length;

                            result.ErrorCode = 200;
                            result.Message = "Upload success";
                            result.Url = $"/photo/{DateTime.Now.ToString("yyyy/MM/dd")}/{ fileName}";
                        }
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        private static String FileServerUpload
        {
            get { return "FileServer.Upload"; }
        }

        private static String FileServerDownload
        {
            get
            {
                var serverUrl = "";
                try
                {
                    serverUrl = AppSettings.Instance.GetString(FileServerUpload.Replace("Upload", "Download"));
                }
                catch
                {
                }
                return serverUrl;
            }
        }

        private static String CreateHash()
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Guid.NewGuid().ToByteArray());
            var value = BitConverter.ToString(hash).Replace("-", "");
            return value;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }

    public class RequestUpload
    {

    }

    public class ResponseUpload
    {
        public ResponseUpload()
        {
            ErrorCode = 400;
            Message = string.Empty;
            Url = string.Empty;
        }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long FileSize { get; set; }
        public int MimeType { get; set; }

    }
}