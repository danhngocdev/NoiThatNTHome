using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DVG.WIS.Utilities.XmlSiteMap
{

    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class UrlSet
    {
        private string FormatDateSitemap = "yyyy-MM-dd";
        private ArrayList map;
        public int Length { get { return map != null ? map.Count : 0; } }

        public UrlSet()
        {
            map = new ArrayList();
        }

        [XmlElement("url")]
        public Location[] Locations
        {
            get
            {
                Location[] items = new Location[map.Count];
                map.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null)
                    return;
                Location[] items = (Location[])value;
                map.Clear();
                foreach (Location item in items)
                    map.Add(item);
            }
        }

        [XmlAttribute("NSPImage")]
        public string NamespaceImage { get; set; } = "http://www.google.com/schemas/sitemap-image/1.1";

        public int Add(Location item)
        {
            if (!map.Contains(item))
                return map.Add(item);

            return 0;
        }

        public void UpdateLastModAll()
        {
            if (map != null && map.Count > 0)
            {
                foreach (Location item in map)
                {
                    item.LastModified = DateTime.Now.ToString(FormatDateSitemap);
                }
            }
        }
    }

    [XmlRoot("sitemapindex", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")] //urlset là tên thẻ mở đầu trong file sitemap.xml (phải trùng tên)
    public class Sitemap
    {
        private string FormatDateSitemap = "yyyy-MM-dd";
        private ArrayList map;
        public int Length { get { return map != null ? map.Count : 0; } }

        public Sitemap()
        {
            map = new ArrayList();
        }


        [XmlElement("sitemap")]
        public SiteMapLocation[] Locations
        {
            get
            {
                SiteMapLocation[] items = new SiteMapLocation[map.Count];
                map.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null)
                    return;
                SiteMapLocation[] items = (SiteMapLocation[])value;
                map.Clear();
                foreach (SiteMapLocation item in items)
                    map.Add(item);
            }
        }

        public int Add(Location item)
        {
            if (!map.Contains(item))
                return map.Add(item);

            return 0;
        }


        public int Add(SiteMapLocation item)
        {
            if (!IsExists(item))
                return map.Add(item);

            return 0;
        }

        public int Insert(SiteMapLocation item)
        {
            if (!IsExists(item))
            {
                map.Insert(0, item);
                return 1;
            }
            return 0;
        }

        public bool IsExists(SiteMapLocation siteMapLoc)
        {
            if (map != null && map.Count > 0)
            {
                foreach (SiteMapLocation item in map)
                {
                    if (item.Url.Equals(siteMapLoc.Url)) return true;
                }
            }

            return false;
        }

        public void UpdateLastMod(string url)
        {
            if (map != null && map.Count > 0)
            {
                foreach (SiteMapLocation item in map)
                {
                    if (item.Url.Equals(url))
                    {
                        item.LastModified = DateTime.Now.ToString(FormatDateSitemap);
                    }
                }
            }
        }
        public void UpdateLastModAll()
        {
            if (map != null && map.Count > 0)
            {
                foreach (SiteMapLocation item in map)
                {
                    item.LastModified = DateTime.Now.ToString(FormatDateSitemap);
                }
            }
        }
    }

    public class SiteMapLocation
    {
        public enum eChangeFrequency
        {
            always,
            hourly,
            daily,
            weekly,
            monthly,
            yearly,
            never
        }

        [XmlElement("loc")]
        public string Url { get; set; }

        [XmlElement("lastmod")]
        public string LastModified { get; set; }

        [XmlElement("changefreq")]
        public eChangeFrequency? ChangeFrequency { get; set; }

        [XmlElement("priority")]
        public double? Priority { get; set; }

        public bool ShouldSerializeChangeFrequency()
        {
            return ChangeFrequency.HasValue;
        }

        public bool ShouldSerializePriority()
        {
            return Priority.HasValue;
        }

    }

    // Items in the shopping list
    public class Location
    {
        public enum eChangeFrequency
        {
            always,
            hourly,
            daily,
            weekly,
            monthly,
            yearly,
            never
        }

        [XmlElement("loc")]
        public string Url { get; set; }

        [XmlElement("changefreq")]
        public eChangeFrequency? ChangeFrequency { get; set; }
        public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

        [XmlElement("lastmod")]
        public string LastModified { get; set; }

        [XmlElement("priority")]
        public double? Priority { get; set; }

        [XmlElement("image:image")]
        public ImageNode ImageNode { get; set; }

        public bool ShouldSerializePriority() { return Priority.HasValue; }
    }

    [XmlRoot("settings")]
    public class SiteMapSettings
    {
        [XmlElement("lastmod")]
        public System.DateTime LastModifiedDate { get; set; }
    }

    [XmlRoot("image:image")]
    public class ImageNode
    {
        [XmlElement("image:loc")]
        public string ImageLoc { get; set; }

        [XmlElement("image:title")]
        public string ImageTitle { get; set; }
    }



    //[XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    //public class Sitemap
    //{
    //    private ArrayList map;
    //    public int Length { get { return map != null ? map.Count : 0; } }

    //    public Sitemap()
    //    {
    //        map = new ArrayList();
    //    }

    //    [XmlElement("url")]
    //    public Location[] Locations
    //    {
    //        get
    //        {
    //            Location[] items = new Location[map.Count];
    //            map.CopyTo(items);
    //            return items;
    //        }
    //        set
    //        {
    //            if (value == null)
    //                return;
    //            Location[] items = (Location[])value;
    //            map.Clear();
    //            foreach (Location item in items)
    //                map.Add(item);
    //        }
    //    }

    //    public int Add(Location item)
    //    {
    //        if (!map.Contains(item))
    //            return map.Add(item);

    //        return 0;
    //    }
    //}

    //// Items in the shopping list
    //public class Location
    //{
    //    public enum eChangeFrequency
    //    {
    //        always,
    //        hourly,
    //        daily,
    //        weekly,
    //        monthly,
    //        yearly,
    //        never
    //    }

    //    [XmlElement("loc")]
    //    public string Url { get; set; }

    //    [XmlElement("changefreq")]
    //    public eChangeFrequency? ChangeFrequency { get; set; }
    //    public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

    //    [XmlElement("lastmod")]
    //    public DateTime? LastModified { get; set; }
    //    public bool ShouldSerializeLastModified() { return LastModified.HasValue; }

    //    [XmlElement("priority")]
    //    public double? Priority { get; set; }
    //    public bool ShouldSerializePriority() { return Priority.HasValue; }
    //}

    //[XmlRoot("settings")]
    //public class SiteMapSettings
    //{
    //    [XmlElement("lastmod")]
    //    public DateTime? LastModifiedDate { get; set; }
    //}
}
