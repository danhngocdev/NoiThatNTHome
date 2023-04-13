using DVG.WIS.Business.Authenticator;
using DVG.WIS.Entities;
using DVG.WIS.Services.FileManagerServices;
using DVG.WIS.Utilities;
using FileManager.Common;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace DVG.WIS.CMS.FileManager.Handler
{
    /// <summary>
    /// Summary description for FileAction
    /// </summary>
    public class FileAction : IHttpHandler
    {
        private static FileManagerService _fileManagerService = new FileManagerService();
        public void ProcessRequest(HttpContext context)
        {
            string command = context.Request.Params["action"];
            var response = new ResponseData();
            try
            {

                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(command))
                    {

                        switch (command)
                        {
                            case "crawlImagesFromUrl":
                                response = CrawlImagesFromUrl(context);
                                break;
                            case "convertUrlImagesToBase64":
                                response = ConvertUrlImagesToBase64(context);
                                break;
                            case "convertMultiUrlImagesToBase64":
                                response = ConvertMultiUrlImagesToBase64(context);
                                break;
                            case "searchFile":
                                response = SearchFile(context);
                                break;
                            case "getFileinfo":
                                response = GetFileinfo(context);
                                break;
                            case "updateFileinfo":
                                response = UpdateFileinfo(context);
                                break;
                            case "addLogoToImage":
                                response = AddLogoToImage(context);
                                break;

                        }
                    }
                }
                else
                {
                    response.Message = "Bạn không có quyền truy cập.";
                }
            }
            catch (Exception ex)
            {
                // DVG.WIS.// LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex);
                response.Message = ex.Message;
            }
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(new JavaScriptSerializer().Serialize(response));
        }

        public ResponseData CrawlImagesFromUrl(HttpContext context)
        {
            var response = new ResponseData();
            string url = context.Request.Form["url"] != null ? context.Request.Form["url"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse res = (HttpWebResponse)request.GetResponse();

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = res.GetResponseStream();
                    StreamReader readStream = null;

                    if (res.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(res.CharacterSet));
                    }

                    string data = readStream.ReadToEnd();

                    res.Close();
                    readStream.Close();

                    if (!string.IsNullOrEmpty(data))
                    {
                        string pattern = @"<img.*?src=""(?<url>.*?)"".*?>";
                        Regex rx = new Regex(pattern);
                        var lst = new List<string>();
                        foreach (Match m in rx.Matches(data))
                        {
                            lst.Add(m.Groups["url"].Value);
                        }

                        if (lst.Count > 0) lst = lst.Where(x => x.Contains("http")).ToList();

                        response.Data = lst;
                        response.Success = true;
                    }
                }
            }
            return response;
        }

        public ResponseData ConvertUrlImagesToBase64(HttpContext context)
        {
            var response = new ResponseData();
            try
            {
                string url = context.Request.Form["url"] != null ? context.Request.Form["url"].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(url))
                {
                    var result = string.Empty;
                    var success = ConvertUrlImagesToBase64(url, ref result);
                    response.Data = result;
                    response.Success = success;
                }
            }
            catch (Exception ex)
            {
                // DVG.WIS.// LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseData ConvertMultiUrlImagesToBase64(HttpContext context)
        {
            var response = new ResponseData();
            try
            {
                string urls = context.Request.Form["urls"] != null ? context.Request.Form["urls"].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(urls))
                {
                    var split = urls.Split(';');
                    var lst = new List<string>();
                    foreach (var url in split)
                    {
                        var result = string.Empty;
                        var success = ConvertUrlImagesToBase64(url, ref result);
                        lst.Add(result);
                    }
                    response.Data = lst;
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                // DVG.WIS.// LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex);
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseData SearchFile(HttpContext context)
        {
            var response = new ResponseData();
            string keyword = context.Request.Form["keyword"] != null ? context.Request.Form["keyword"].ToString() : string.Empty;
            string fromDateStr = context.Request.Form["from"] != null ? context.Request.Form["from"].ToString() : string.Empty;
            string toDateStr = context.Request.Form["to"] != null ? context.Request.Form["to"].ToString() : string.Empty;
            int PageSize = context.Request.Form["pagesize"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["pagesize"]) : 1000;
            int PageIndex = context.Request.Form["pageindex"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["pageindex"]) : 1;

            UserLogin userInfo = AuthenService.GetUserLogin();
            var now = DateTime.Now;
            var fromDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(-7);
            var toDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            if (!string.IsNullOrEmpty(fromDateStr)) fromDate = DVG.WIS.Utilities.Extensions.AsDateTimeVn(fromDateStr, fromDate);
            if (!string.IsNullOrEmpty(toDateStr))
            {
                toDate = DVG.WIS.Utilities.Extensions.AsDateTimeVn(toDateStr, toDate);
                toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
            }
            var result = _fileManagerService.GetList(keyword, userInfo.UserName, PageIndex, PageSize, fromDate, toDate);

            if (result != null && result.Data != null)
            {
                var lstPhoto = (List<FM_PhotoOnList>)result.Data;
                var d = TinyMapper.Map<List<DVG.WIS.CMS.FileManager.Entity.FileInfo>>(lstPhoto);
                response.Data = d;
                response.Success = true;
            }

            return response;
        }

        public ResponseData GetFileinfo(HttpContext context)
        {
            var response = new ResponseData();
            string fileName = context.Request.Form["filename"] != null ? context.Request.Form["filename"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                var result = _fileManagerService.GetFileInfo(fileName);
                if (result != null && result.Data != null)
                {
                    var data = (FM_Photo)result.Data;
                    if (data.Id > 0)
                    {
                        response.Data = data;
                        response.Success = true;
                    }
                }
            }
            return response;
        }

        public ResponseData UpdateFileinfo(HttpContext context)
        {
            var response = new ResponseData();
            string fileName = context.Request.Form["filename"] != null ? context.Request.Form["filename"].ToString() : string.Empty;
            string fileUrl = context.Request.Form["fileurl"] != null ? context.Request.Form["fileurl"].ToString() : string.Empty;
            string Title = context.Request.Form["title"] != null ? context.Request.Form["title"].ToString() : string.Empty;
            string Description = context.Request.Form["description"] != null ? context.Request.Form["description"].ToString() : string.Empty;
            string Alternate = context.Request.Form["alternate"] != null ? context.Request.Form["alternate"].ToString() : string.Empty;
            int Width = context.Request.Form["width"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["width"]) : 0;
            int Height = context.Request.Form["height"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["height"]) : 0;
            int Capacity = context.Request.Form["capacity"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["capacity"]) : 0;
            int MimeType = context.Request.Form["mimetype"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["mimetype"]) : 0;
            string MimeTypeName = context.Request.Form["mimetypename"] != null ? context.Request.Form["mimetypename"].ToString() : string.Empty;
            int DisplayPosition = context.Request.Form["displayposition"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["displayposition"]) : 0;
            int DisplayStyle = context.Request.Form["displaystyle"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["displaystyle"]) : 0;
            long FileSize = context.Request.Form["filesize"] != null ? DVG.WIS.Utilities.Extensions.ToLong(context.Request.Form["filesize"]) : 0;
            if (!string.IsNullOrEmpty(fileName)
                || !string.IsNullOrEmpty(fileUrl))
            {
                string userName = HttpContext.Current.User.Identity.Name;
                UserLogin userInfo = AuthenService.GetUserLogin();
                var now = DateTime.Now;
                var obj = new FM_Photo
                {
                    FileName = fileName,
                    FileUrl = fileUrl,
                    Title = Title.Replace("\"", string.Empty).Replace("\'", string.Empty),
                    Description = Description,
                    Alternate = Alternate,
                    Width = Width,
                    Height = Height,
                    Capacity = Capacity,
                    MimeType = MimeType,
                    MimeTypeName = MimeTypeName,
                    DisplayPosition = DisplayPosition,
                    DisplayStyle = DisplayStyle,
                    FileSize = FileSize,
                    CreatedDate = now,
                    ModifiedDate = now,
                    CreatedDateSpan = now.Ticks,
                    ModifiedDateSpan = now.Ticks,
                    Status = 1,
                    CreatedBy = userInfo.UserName,
                    ModifiedBy = userInfo.UserName
                };

                var result = _fileManagerService.UpdateFileInfo(obj);

                if (result.ErrorCode == (int)ErrorCodes.Success)
                {
                    var photoInfos = _fileManagerService.GetFileInfo(fileName);
                    if (photoInfos != null && photoInfos.Data != null)
                    {
                        var photoInfo = (FM_Photo)photoInfos.Data;
                        if (photoInfo.Id > 0)
                        {
                            response.Data = photoInfo;
                        }
                    }

                    response.Success = true;
                }
            }
            return response;
        }

        /// <summary>
        /// Đang dùng js, chưa dùng hàm này
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ResponseData AddLogoToImage(HttpContext context)
        {
            var response = new ResponseData();
            try
            {
                string imageUrl = context.Request.Form["imageUrl"] != null ? context.Request.Form["imageUrl"].ToString() : string.Empty;
                string logoUrl = context.Request.Form["logoUrl"] != null ? context.Request.Form["logoUrl"].ToString() : string.Empty;
                int xPos = context.Request.Form["xPos"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["xPos"]) : 0;
                int yPos = context.Request.Form["yPos"] != null ? DVG.WIS.Utilities.Extensions.ToInt(context.Request.Form["yPos"]) : 0;
                if (!string.IsNullOrEmpty(imageUrl) && !string.IsNullOrEmpty(logoUrl))
                {
                    var stream = WaterMark.AddLogoToImage(imageUrl, logoUrl, xPos, yPos);
                    if (stream != null)
                    {
                        byte[] imageBytes = stream.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        response.Data = base64String;
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // DVG.WIS.// LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex);
                response.Message = ex.Message;
            }
            return response;
        }


        private bool ConvertUrlImagesToBase64(string url, ref string output)
        {
            var tuple = DVG.WIS.Utilities.FileStorage.FileStorage.SaveImage(url);
            if (tuple.Item1 != null)
            {
                byte[] imageBytes = tuple.Item1.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                output = base64String;
                return true;
            }
            else
            {
                output = tuple.Item2;
            }
            return false;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    [Serializable]
    public class ResponseData
    {
        public object Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}