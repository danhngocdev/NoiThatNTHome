using DVG.WIS.Core;
using DVG.WIS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVG.WIS.PublicModel
{
    public class CategoryFEModel
    {
        public CategoryFEModel(DVG.WIS.Entities.Category cate, string formatUrl = "")
        {
            this.Id = cate.Id;
            this.ParentId = cate.ParentId;
            this.Name = cate.Name;
            this.ShortURL = cate.ShortURL;
            this.FormatUrl = formatUrl;
            this.Type = cate.Type;
        }
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string ShortURL { get; set; }
        public string FormatUrl { get; set; }
    }

    public class CategoryMenuModel
    {
        public CategoryFEModel Parrent { get; set; }
        public List<CategoryFEModel> SubMenu { get; set; }
    }

    public class MenuModel
    {
        public MenuModel()
        {

        }
        public MenuModel(Entities.Category cateInfo, Func<string, int, int, string> funcBuildLink)
        {
            Id = cateInfo.Id;
            ParentId = cateInfo.ParentId;
            Name = cateInfo.Name;
            Url = funcBuildLink != null ? funcBuildLink.Invoke(cateInfo.ShortURL, cateInfo.Type, cateInfo.Id) : cateInfo.ShortURL;
            Type = cateInfo.Type;
        }
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }

    }

    public class MenuTop
    {
        public MenuModel Parent { get; set; }
        //public List<MenuModel> SubMenu { get; set; }
        public List<MenuTop> SubMenu { get; set; }

        public bool HasSubMenu { get; set; }
    }
}