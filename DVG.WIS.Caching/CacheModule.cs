using DVG.WIS.Caching.Cached;
using DVG.WIS.Caching.Cached.Implements;
using DVG.WIS.Caching.DTO.Entities;
using DVG.WIS.Utilities;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace DVG.WIS.Caching
{
    public class CacheModule : IHttpModule
    {
        private ICached _cacheClient;
        private static object lockedObject = new object();
        private const string StreamFilterName = "mStreamFilterForCachePage";
        private string _device = "desktop";
        public CacheModule()
        {
            CachingConfigModel config = new CachingConfigModel()
            {
                IpServer = AppSettings.Instance.GetString("RedisIP"),
                Port = AppSettings.Instance.GetInt32("RedisPort"),
                DB = AppSettings.Instance.GetInt32("RedisDBForCachePage", 1),
                ConnectTimeout = AppSettings.Instance.GetInt32("RedisTimeout", 600),
                RedisSlotNameInMemory = AppSettings.Instance.GetString("RedisSlotNameForCachePage", "RedisTinxeForCachePage")
            };
            _cacheClient = new RedisCached(config);
        }

        // The stored application

        // In the Init function, register for HttpApplication 
        public void Init(HttpApplication application)
        {
            if (!AppSettings.Instance.GetBool("AllowCachePage")) return;

            //Store off the application object
            // event
            application.ResolveRequestCache += ResolveRequestCache;
            application.UpdateRequestCache += UpdateRequestCache;
            application.Error += ErrorHandler;
        }

        private void ResolveRequestCache(object sender, System.EventArgs e)
        {
            HttpApplication mApplication = (HttpApplication)sender;

            string rawUrl = mApplication.Request.RawUrl;

            bool flag = false;

            if (ValidContent(mApplication))
            {
                string objKey = GenCacheKey(mApplication);
                string content = _cacheClient.Get<string>(objKey);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    mApplication.Response.Write(content);
                    mApplication.Response.ContentType = "text/html; charset=utf-8";
                    mApplication.CompleteRequest();
                    mApplication.Response.AddHeader("X-Cache-Hit", "true");
                    mApplication.Response.AddHeader("X-Cache-Url", rawUrl);
                    mApplication.Response.AddHeader("X-Cache-Device", _device);
                    flag = true;
                }
                else
                {
                    // Create a new filter
                    CacheStream mStreamFilter = new CacheStream(mApplication.Response.Filter);
                    // Insert it onto the page
                    mApplication.Response.Filter = mStreamFilter;
                    // Save a reference to the filter in the request context so we can grab it in UpdateRequestCache
                    mApplication.Context.Items.Add(StreamFilterName, mStreamFilter);

                }

                if (flag)
                {
                    //gzip
                    mApplication.Context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
                    string acceptedTypes = mApplication.Request.Headers["Accept-Encoding"];

                    // if we couldn't find the header, bail out 
                    if (acceptedTypes == null)
                        return;

                    // Current response stream 
                    Stream baseStream = mApplication.Response.Filter;

                    // If there are more than one possibility offered by the browser default to the preffered one from the web.config 
                    // If nothing is specified in the web.config default to GZip 
                    acceptedTypes = acceptedTypes.ToLower();

                    if ((acceptedTypes.Contains("gzip") || acceptedTypes.Contains("x-gzip") || acceptedTypes.Contains("*")))
                    {
                        mApplication.Response.Filter = new GZipStream(baseStream, CompressionMode.Compress);
                        //This won't show up in a trace log but if you use fiddler or nikhil kothari's web dev helper BHO you can see it appended 
                        mApplication.Response.AppendHeader("Content-Encoding", "gzip");
                    }
                    else if (acceptedTypes.Contains("deflate"))
                    {
                        mApplication.Response.Filter = new DeflateStream(baseStream, CompressionMode.Compress);
                        //This won't show up in a trace log but if you use fiddler or nikhil kothari's web dev helper BHO you can see it appended 
                        mApplication.Response.AppendHeader("Content-Encoding", "deflate");
                    }
                }
            }
        }

        private void UpdateRequestCache(object sender, System.EventArgs e)
        {
            HttpApplication mApplication = (HttpApplication)sender;

            int statusResponse = HttpContext.Current.Response.StatusCode;
            if (!ValidStatus(statusResponse)) return;

            var objKey = GenCacheKey(mApplication);

            // Grab the CacheStream out of the context
            CacheStream mStreamFilter = (CacheStream)mApplication.Context.Items[StreamFilterName];
            if (mStreamFilter != null)
            {
                // Remove the reference to the filter
                mApplication.Context.Items.Remove(StreamFilterName);
                // Create a buffer
                byte[] bBuffer = new byte[mStreamFilter.Length];
                // Rewind the stream
                mStreamFilter.Position = 0;
                // Get the bytes
                mStreamFilter.Read(bBuffer, 0, (int)mStreamFilter.Length);
                // Convert to a string
                var utf8 = new UTF8Encoding();
                var strBuff = new StringBuilder(utf8.GetString(bBuffer));
                //strBuff.Insert(0, "<!-- Cached: " + DateTime.Now.ToString("r") + " -->");
                // Save it away
                string content = strBuff.ToString();

                content = BasicCompression(content);

                //add cache page
                if (mApplication.Response.ContentType == "text/html")
                {
                    var context = new HttpContextWrapper(mApplication.Context);

                    var route = RouteTable.Routes.GetRouteData(context);
                    var controllerName = "";
                    var actionName = "";
                    if (route?.Values.Count > 0)
                    {
                        controllerName = route.GetRequiredString("controller");
                        actionName = route.GetRequiredString("action");
                        var rawUrl = mApplication.Request.RawUrl;

                        if (!string.IsNullOrEmpty(rawUrl) && !string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
                        {
                            var settings = CacheSettings.GetCurrentSettings();
                            var pageConfig = settings.GetPageSetting(string.Concat("/", controllerName, "/", actionName));
                            if (pageConfig.FilePath != null)
                                _cacheClient.Add(objKey, content, (int)pageConfig.CacheExpire / 60);
                        }
                    }
                }
            }
        }

        private void ErrorHandler(object sender, EventArgs e)
        {
            HttpApplication mApplication = (HttpApplication)sender;

            try
            {
                string strErrorMessage = "Error\r\nUrl: "
                        + mApplication.Request.Url + "\r\nMessage:"
                        + mApplication.Context.Error.Message + "\r\nStack Trace: "
                        + mApplication.Context.Error.StackTrace;

                Logger.WriteLog(Logger.LogType.Error, strErrorMessage);
            }
            catch { }
        }

        private void RewritePathHandler(object sender, EventArgs e)
        {
            //mApplication.Context.RewritePath(mApplication.Request.ApplicationPath + "/page");
        }

        private string GenCacheKey(HttpApplication mApplication)
        {
            try
            {
                _device = DetectDevice.Instance.BrowserIsMobile() || mApplication.Request.Browser.IsMobileDevice ? "mobile" : "desktop";

                _device = string.Concat(_device, ":", AppSettings.Instance.GetString("CacheByDomain", "OnDesktop"));

                string url = mApplication.Request.RawUrl;

                if (url.Equals("/")) url = "home";

                url = url.TrimStart('/').Replace("/", ":");

                string keyCache = string.Concat(url, ":", _device);

                keyCache = KeyCacheHelper.GenCacheKey(keyCache);

                return keyCache;
            }
            catch
            {
                string url = mApplication.Request.RawUrl;

                if (url.Equals("/")) url = "home";

                url = url.TrimStart('/').Replace("/", ":");

                string keyCache = string.Concat(url, ":", _device);

                keyCache = KeyCacheHelper.GenCacheKey(keyCache);

                return keyCache;

            }
        }

        private bool ValidStatus(int status)
        {
            return status == 200;
        }

        private bool ValidContent(HttpApplication application)
        {
            var rawUrl = application.Request.RawUrl;

            if (rawUrl.Contains("notfound")) return false;

            int statusResponse = application.Response.StatusCode;

            string method = application.Request.HttpMethod;

            if (method.ToLower().Equals("post")) return false;

            string patternMatchExtension = @"\.(txt|css|js|ico|jpg|jpeg|png|bmp|gif|eot|ttf|woff|woff2|aspx|xml|html|mp4|mp3|ico|map|config)$";

            Regex regexMatchExtensionUrl = new Regex(patternMatchExtension, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            Match matchExtension = regexMatchExtensionUrl.Match(rawUrl);

            return !matchExtension.Success && !rawUrl.Contains("/WIS/") && ValidStatus(statusResponse);

        }

        public void Dispose() { }

        private string BasicCompression(string content)
        {
            string newContent = string.Empty;

            newContent = Regex.Replace(content, ">\\s{2,}<", "> <", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            // strip whitespaces after tags, except space
            newContent = Regex.Replace(newContent, "\\>[^\\S]+", "> ", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            // strip whitespaces before tags, except space
            newContent = Regex.Replace(newContent, "[^\\S ]+\\<", " <", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            // shorten multiple whitespace sequences
            //newContent = Regex.Replace(newContent, "(\\s){2,}", "$1", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            // Remove HTML comments
            newContent = Regex.Replace(newContent, "<!--(.|\\s)*?-->", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return newContent;
        }
    }

    public class CacheStream : System.IO.Stream
    {
        private System.IO.MemoryStream moMemoryStream = new System.IO.MemoryStream();
        private System.IO.Stream moStream;

        public CacheStream(System.IO.Stream stream)
        {
            moStream = stream;
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return moMemoryStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return moMemoryStream.Position;
            }
            set
            {
                moMemoryStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return moMemoryStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, System.IO.SeekOrigin direction)
        {
            return moMemoryStream.Seek(offset, direction);
        }

        public override void SetLength(long length)
        {
            moMemoryStream.SetLength(length);
        }

        public override void Close()
        {
            moStream.Close();
        }

        public override void Flush()
        {
            moStream.Flush();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            moStream.Write(buffer, offset, count);
            moMemoryStream.Write(buffer, offset, count);
        }
    }

    public class ResponseCapture : IDisposable
    {
        private readonly HttpResponseBase response;
        private readonly TextWriter originalWriter;
        private StringWriter localWriter;
        public ResponseCapture(HttpResponseBase response)
        {
            this.response = response;
            originalWriter = response.Output;
            localWriter = new StringWriter();
            response.Output = localWriter;
        }
        public override string ToString()
        {
            localWriter.Flush();
            return localWriter.ToString();
        }
        public void Dispose()
        {
            if (localWriter != null)
            {
                localWriter.Dispose();
                localWriter = null;
                response.Output = originalWriter;
            }
        }
    }
}