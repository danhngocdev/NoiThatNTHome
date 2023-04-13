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
                    item.LastModifiedString = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }
    }

    [XmlRoot("sitemapindex", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SiteMap
    {
        private ArrayList map;
        public int Length { get { return map != null ? map.Count : 0; } }

        public SiteMap()
        {
            map = new ArrayList();
        }

        public int Add(SiteMapLoc item)
        {
            if (!IsExists(item))
                return map.Add(item);

            return 0;
        }

        public int Insert(SiteMapLoc item)
        {
            if (!IsExists(item))
            {
                map.Insert(0, item);
                return 1;
            }
            return 0;
        }
        public bool IsExists(SiteMapLoc siteMapLoc)
        {
            if (map != null && map.Count > 0)
            {
                foreach (SiteMapLoc item in map)
                {
                    if (item.Url.Equals(siteMapLoc.Url)) return true;
                }
            }

            return false;
        }
        public void UpdateLastModAll()
        {
            if (map != null && map.Count > 0)
            {
                foreach (SiteMapLoc item in map)
                {
                    item.LastModified = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }

        [XmlElement("sitemap")]
        public SiteMapLoc[] Locations
        {
            get
            {
                SiteMapLoc[] items = new SiteMapLoc[map.Count];
                map.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null)
                    return;

                SiteMapLoc[] items = (SiteMapLoc[])value;
                map.Clear();
                foreach (SiteMapLoc item in items)
                    map.Add(item);
            }
        }
    }

    public class SiteMapLoc
    {
        [XmlElement("loc")]
        public string Url { get; set; }

        [XmlElement("priority")]
        public double? Priority { get; set; }
        [XmlElement("lastmod")]
        public string LastModified { get; set; }
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

        [XmlIgnore]
        public DateTime? LastModified { get; set; }

        [XmlElement("lastmod")]
        public string LastModifiedString
        {
            get { return this.LastModified.HasValue ? this.LastModified.Value.ToString("yyyy-MM-ddTHH:mm:ss+07:00") : string.Empty; }
            set { this.LastModified = DateTime.Parse(value); }
        }
        public bool ShouldSerializeLastModified() { return LastModified.HasValue; }

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
        public DateTime? LastModifiedDate { get; set; }
    }

    //[XmlRoot("image:image")]
    public class ImageNode
    {
        [XmlElement("image:loc")]
        public string ImageLoc { get; set; }

        [XmlElement("image:title")]
        public string ImageTitle { get; set; }
    }
}
