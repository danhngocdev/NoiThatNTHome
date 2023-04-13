using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVG.WIS.Services.Monitor
{
    //public class MonitorService
    //{
    //    private static List<ProjectModel> lstProject;
    //    private const string desktopUserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
    //    private const string mobileUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1";

    //    public MonitorService()
    //    {
    //        try
    //        {
    //            lstProject = GetXMLConfig();
    //            if (lstProject != null && lstProject.Any())
    //            {
    //                foreach (var project in lstProject)
    //                {
    //                    // khởi tạo file log
    //                    string absolutePath = CreateLog.LogPath(project.FolderLog);
    //                    if (!File.Exists(absolutePath))
    //                    {
    //                        File.Create(absolutePath).Close();
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.WriteLog(Logger.LogType.Error, ex);
    //        }
    //    }

    //    public void CheckProjects()
    //    {
    //        List<Thread> listOfThreadProcess = new List<Thread>();
    //        if (lstProject != null && lstProject.Any())
    //        {
    //            foreach (var project in lstProject)
    //            {
    //                ThreadStart threadStart = new ThreadStart(() =>
    //                {
    //                    project.UserAgent = desktopUserAgent;
    //                    if (CheckProjects(project))
    //                    {
    //                        // check wap nếu cùng 1 url với web
    //                        if (project.SameUrl)
    //                        {
    //                            project.UserAgent = mobileUserAgent;
    //                            CheckProjects(project);
    //                        }
    //                    }
    //                });
    //                listOfThreadProcess.Add(new Thread(threadStart));
    //            }
    //        }
    //        if (listOfThreadProcess != null && listOfThreadProcess.Any())
    //        {
    //            foreach (Thread thread in listOfThreadProcess)
    //            {
    //                thread.Start();
    //            }
    //        }
    //    }

    //    private bool CheckProjects(ProjectModel project)
    //    {
    //        if (project.IsPublish)
    //        {
    //            foreach (var item in project.Urls)
    //            {
    //                // Gặp lỗi thì dừng check
    //                if (!LoadPage(project, item.Url, item.RegexUrlDetail))
    //                {
    //                    return false;
    //                }
    //            }
    //        }
    //        ClearLog(project);
    //        return true;
    //    }

    //    private bool LoadPage(ProjectModel project, string url, string regexUrlDetail)
    //    {
    //        string baseUrl = project.BaseUrl;
    //        string folderLog = project.FolderLog;
    //        try
    //        {
    //            //ServicePointManager.Expect100Continue = true;
    //            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    //            var request = (HttpWebRequest)WebRequest.Create(url);
    //            request.UserAgent = project.UserAgent;
    //            //request.Headers.Add("Cookie", "ASP.NET_SessionId=zx354ydgcbx51lma5p3itm3d; __RequestVerificationToken=r2jioh9P3bRWfBudrhYnj5O_rBoVVZFM3GTB92fxYVOzFnSZ-PHNJr5Ikl90qVXKBFpKNeEBZVyjyqMmDXKjFj9F1F6tqh5eebfCQcQ7HEM1; _ga=GA1.2.595859067.1508555117; _gid=GA1.2.1462184684.1508555117; _gat=1");
    //            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
    //            {
    //                if (response.StatusCode != HttpStatusCode.OK)
    //                {
    //                    int code = (int)response.StatusCode;
    //                    if (code >= 400 && code <= 502)
    //                    {
    //                        CreateLog.WriteLog(string.Format("{0}: {1} {2}", url, code, response.StatusDescription), folderLog);
    //                        return false;
    //                    }
    //                }
    //                // Application_Error đá link về trang chủ hoặc trang upgrade.html
    //                else if ((url != baseUrl && baseUrl.TrimEnd('/') == response.ResponseUri.AbsoluteUri.TrimEnd('/')) || response.ResponseUri.AbsoluteUri.Contains("/upgrade.html"))
    //                {
    //                    CreateLog.WriteLog(string.Format("Error {0} Redirect {1}", url, response.ResponseUri.AbsoluteUri), folderLog);
    //                    return false;
    //                }
    //                // get link detail from page list
    //                else if (!string.IsNullOrEmpty(regexUrlDetail))
    //                {
    //                    return GetLinkDetailFromPageList(project, baseUrl, response, regexUrlDetail);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string strError = ex.Message;
    //            if (strError.Contains("The remote server returned an error"))
    //                strError = strError.Replace("The remote server returned an error", url);
    //            else
    //                strError = string.Format("{0}: {1}", project.Value, strError);
    //            CreateLog.WriteLog(strError, folderLog);
    //            return false;
    //        }
    //        return true;
    //    }

    //    private List<ProjectModel> GetXMLConfig()
    //    {
    //        try
    //        {
    //            string absolutePath = new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).AbsolutePath;

    //            string fileProjects = AppSettings.Instance.GetString("MonitorWebsitePath");

    //            string xmlPath = !string.IsNullOrEmpty(fileProjects) ? fileProjects : (absolutePath.Substring(0, absolutePath.IndexOf("/bin") + 1));

    //            fileProjects = string.Concat(xmlPath, "/BundleConfigs.xml");


    //            XDocument xdoc = XDocument.Load(fileProjects);
    //            var lst = (from project in xdoc.Element("Projects").Elements()
    //                       select new ProjectModel
    //                       {
    //                           Key = project.Element("Key").Value,
    //                           Value = project.Element("Value").Value,
    //                           SameUrl = project.Element("SameUrl").Value.ToBool(false),
    //                           FolderLog = project.Element("FolderLog").Value,
    //                           BaseUrl = project.Element("BaseUrl").Value,

    //                           Urls = (from obj in project.Element("Urls").Elements()
    //                                   select new ItemUrlModel
    //                                   {
    //                                       Url = obj.Element("Url").Value,
    //                                       RegexUrlDetail = obj.Element("RegexUrlDetail").Value
    //                                   }).ToList(),

    //                           IsPublish = project.Element("IsPublish").Value.ToBool(false)
    //                       }).ToList();

    //            return lst;
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.ErrorLog(ex);
    //            return new List<ProjectModel>();
    //        }
    //    }

    //    private bool GetLinkDetailFromPageList(ProjectModel project, string baseUrl, HttpWebResponse response, string RegexUrlDetail)
    //    {
    //        using (Stream respStream = response.GetResponseStream())
    //        {
    //            using (StreamReader ioStream = new StreamReader(respStream))
    //            {
    //                string pageContent = ioStream.ReadToEnd();
    //                string urlDetail = string.Empty;
    //                foreach (Match match in Regex.Matches(pageContent, RegexUrlDetail))
    //                {
    //                    if (!string.IsNullOrEmpty(match.Groups["LinkDetail"].Value))
    //                    {
    //                        urlDetail = string.Concat(baseUrl, match.Groups["LinkDetail"].Value);
    //                        return LoadPage(project, urlDetail, string.Empty);
    //                    }
    //                }
    //            }
    //        }
    //        return false;
    //    }


    //    private static void ClearLog(ProjectModel project)
    //    {
    //        //không có lỗi thì xóa log cũ để không nhắn tin cảnh báo
    //        string absolutePath = CreateLog.LogPath(project.FolderLog);
    //        if (File.Exists(absolutePath))
    //        {
    //            File.Create(absolutePath).Close();
    //        }
    //    }

    //}
}
