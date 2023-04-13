using DVG.WIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class BannerFEModel
    {
        public BannerFEModel(Entities.Banner banner)
        {
            this.Id = banner.Id;
            this.Name = banner.Name;
            this.Title = banner.Title;
            this.Embed = banner.Embed.IndexOf("http") == -1 ? CoreUtils.BuildCropAvatar(banner.Embed) : banner.Embed;
            this.Status = banner.Status;
            this.Position = banner.Position;
            this.PageId = banner.PageId;
            this.TargetLink = banner.TargetLink;
            this.FromDate = banner.FromDate;
            this.UntilDate = banner.UntilDate;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Embed { get; set; }
        public int Status { get; set; }
        public int? Position { get; set; }
        public int? PageId { get; set; }
        public string TargetLink { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? UntilDate { get; set; }
    }
}
