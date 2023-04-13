using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace DVG.WIS.Caching
{
    public class CacheSettings
    {
        public static CacheSettings GetCurrentSettings()
        {
            string cacheName = "Read_CacheSettings";
            if (null != HttpContext.Current.Cache[cacheName])
            {
                try
                {
                    return (CacheSettings)HttpContext.Current.Cache[cacheName];
                }
                catch
                {
                    return new CacheSettings();
                }
            }
            else
            {
                try
                {
                    string configFilePath = HttpContext.Current.Server.MapPath("/Config/CacheSettings.config");
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(configFilePath);

                    CacheSettings settings = new CacheSettings();

                    XmlNode nodeFileSettingCacheExpire = xmlDoc.DocumentElement.SelectSingleNode("//Configuration/CacheSettingsFile");
                    settings.FileSettingCacheExpire = nodeFileSettingCacheExpire.Attributes["cacheExpire"].Value.ToLong();
                    if (settings.FileSettingCacheExpire <= 0)
                    {
                        settings.FileSettingCacheExpire = 3600;// default 1h
                    }

                    XmlNode nodeEnableCache = xmlDoc.DocumentElement.SelectSingleNode("//Configuration/Cache");
                    settings.EnableCache = nodeEnableCache.Attributes["enable"].Value.ToBool();

                    List<PageSetting> pageSettings = new List<PageSetting>();

                    XmlNodeList pages = xmlDoc.DocumentElement.SelectNodes("//Pages/Page");

                    for (int i = 0; i < pages.Count; i++)
                    {
                        PageSetting pageSetting = new PageSetting();
                        pageSetting.CacheName = pages[i].Attributes["name"].Value;
                        pageSetting.FilePath = pages[i].Attributes["filePath"].Value;
                        pageSetting.CacheExpire = pages[i].Attributes["cacheExpire"].Value.ToLong();

                        pageSettings.Add(pageSetting);
                    }

                    settings.PageSettings = pageSettings.ToArray();

                    CacheDependency fileDependency = new CacheDependency(configFilePath);
                    HttpContext.Current.Cache.Insert(cacheName, settings, fileDependency, DateTime.Now.AddSeconds(settings.FileSettingCacheExpire), TimeSpan.Zero, CacheItemPriority.Normal, null);

                    return settings;
                }
                catch (Exception ex)
                {
                    return new CacheSettings();
                }
            }
        }

        public PageSetting GetPageSetting(string filePath)
        {
            string url = (filePath.IndexOf("?") > 0 ? filePath.Substring(0, filePath.IndexOf("?")) : filePath);
            url = url.ToLower();
            url = url.Replace("//", "/");
            if (m_PageSettings != null)
            {
                for (int i = 0; i < this.m_PageSettings.Length; i++)
                {
                    PageSetting setting = this.m_PageSettings[i];
                    if (string.Compare(setting.FilePath, url, true) == 0)
                    {
                        return setting;
                    }
                }
            }
            return new PageSetting();
        }

        #region Properties
        private PageSetting[] m_PageSettings;
        public PageSetting[] PageSettings
        {
            set { this.m_PageSettings = value; }
            get { return this.m_PageSettings; }
        }

        private long m_FileSettingCacheExpire;
        public long FileSettingCacheExpire
        {
            set { this.m_FileSettingCacheExpire = value; }
            get { return this.m_FileSettingCacheExpire; }
        }

        private bool m_EnableCache;
        public bool EnableCache
        {
            set { this.m_EnableCache = value; }
            get { return this.m_EnableCache; }
        }
        #endregion

        #region struct
        public class PageSetting
        {
            public PageSetting()
            {
                this.CacheName = "";
                this.FilePath = "";
                this.CacheExpire = AppSettings.Instance.GetInt64("DefaultDurationCache", 300);
            }
            public PageSetting(string cacheName, string actionName, string filePath, long cacheExpire)
            {
                this.CacheName = cacheName;
                this.FilePath = filePath;
                this.CacheExpire = cacheExpire;
                this.ActionName = actionName;
            }
            public string CacheName, FilePath;
            /// <summary>
            /// Seconds
            /// </summary>
            public long CacheExpire;
            public string ActionName;
        }
        #endregion
    }
}
