using DVG.WIS.CMS.FileManager.Entity;
using FileManager.Common;
using FileManager.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FileManager.Handler
{
	/// <summary>
	/// Summary description for LoadFile
	/// </summary>
	public class LoadFile : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			ResultReturn rr = new ResultReturn()
			{
				Result = true
			};

			try
			{
				if (!HttpContext.Current.User.Identity.IsAuthenticated)
				{
					throw new Exception("Bạn không có quyền truy cập.");
				}
				DateTime dtNow = DateTime.Now;
				var path = context.Request["path"];
				if (string.IsNullOrEmpty(path))
				{
					path = dtNow.ToString("yyyy/MM/dd");
				}
				string userName = HttpContext.Current.User.Identity.Name;
				string key = FileStorage.AESEncrypt(userName + "|" + dtNow.ToString("yyyy-MM-dd HH:mm"));
				string folder = string.Concat(path, "/", FileStorage.EncriptUsername(userName));
				NameValueCollection nvc = new NameValueCollection()
				{
					{ "project", Config.UploadProject },
					{ "folder", folder },
					{ "StringDecypt", key },
					{ "submit", "Check" }
				};
				string result = FileStorage.SendRequestWithParram(Config.FullLoadFileApi, nvc);

				if (!string.IsNullOrWhiteSpace(result))
				{
					IList<string> images = new JavaScriptSerializer().Deserialize<IList<string>>(result);
					rr.FileInfos = new List<FileInfoOld>();

					foreach (string image in images)
					{
						string virtualPath = string.Concat("/", folder, "/", image);
						string fullPath = string.Concat(Config.ViewDomain, virtualPath);

						rr.FileInfos.Add(new FileInfoOld()
						{
							Result = true,
							Name = image,
							Path = virtualPath,
							FullPath = fullPath,
							FullOriginalPath = fullPath
						});
					}
				}
			}
			catch (Exception ex)
			{
                // DVG.WIS.// LogES.LogES.Instance.LogException(DVG.WIS.LogES.Models.LogSourceTypeEnums.CMS, DVG.WIS.LogES.Models.LogErrorSeverityEnums.Error, ex);
                rr.Result = false;
				rr.Message = ex.Message;
			}

			context.Response.ContentType = "application/json; charset=utf-8";
			context.Response.Write(new JavaScriptSerializer().Serialize(rr));
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}