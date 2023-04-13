using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVG.WIS.CMS.FileManager.Entity
{
    public class FileInfo : FM_Photo
    {
        public string CreatedDateStr { get { return this.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"); } }
        public string imageDomain = DVG.WIS.Utilities.AppSettings.Instance.GetString("View-Domain").TrimEnd('/');
        public string Crop105x105 { get { return string.Format("{0}/crop/105x105/{1}", imageDomain, FileUrl.Replace(imageDomain, string.Empty).TrimStart('/')); } }
    }

    public class FileInfoOld
    {
        public bool Result { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public string FullOriginalPath { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public string Message { get; set; }
    }
}