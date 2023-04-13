using DVG.WIS.CMS.FileManager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileManager.Entity
{
    public class ResultReturn
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public IList<FolderInfo> FolderInfos { get; set; }
        public IList<FileInfoOld> FileInfos { get; set; }
    }
}