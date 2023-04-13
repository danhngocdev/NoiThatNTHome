using DVG.WIS.Core;
using DVG.WIS.Local;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class NewsInListModel
    {
        public NewsInListModel() { }
        public NewsInListModel(Entities.News model)
        {
            this.Id = model.Id;
            this.Title = model.Title;
            this.Sapo = model.Sapo;
            this.Avatar = model.Avatar;
            this.CategoryId = model.CategoryId;
            this.Id = model.Id;
            this.PublishedDate = model.PublishedDate;
            this.Url = CoreUtils.BuildURL("/{0}/{1}-newsId{2}", ConstUrl.News, StringUtils.UnicodeToUnsignCharAndDash(Title), Id);
        }
        public int Id { set; get; }
        public string Title { set; get; }
        public string Sapo { set; get; }
        public string Avatar { set; get; }
        public int CategoryId { set; get; }
        public string CategoryName { set; get; }
        public string CategoryUrl { set; get; }
        public DateTime PublishedDate { get; set; }
        public string Url { get; set; }
        public string GetAvatar(string crop)
        {
            return CoreUtils.BuildCropAvatar(this.Avatar, string.Empty, crop);
        }
        public string PublishedDateStr
        {
            get
            {
                return PublishedDate.ToString("dd/MM/yyyy");
            }
        }
    }

    public class NewsImageFEModel
    {
        public NewsImageFEModel(Entities.NewsImage model)
        {
            this.Id = model.Id;
            this.ImageUrl = model.ImageUrl;
            this.Title = model.Title;
        }
        public int Id { get; set; }
        public long NewsId { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string GetAvatar(string crop)
        {
            return CoreUtils.BuildCropAvatar(this.ImageUrl, string.Empty, crop);
        }
    }
}
