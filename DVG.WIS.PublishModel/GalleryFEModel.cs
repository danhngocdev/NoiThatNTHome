using DVG.WIS.Core;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class GalleryFEModel
    {
        public GalleryFEModel(Gallery model)
        {
            Id = model.Id;
            Url = model.Url;
            Priority = model.Priority;
            Status = model.Status;
        }
        public int Id { set; get; }
        public string Url { set; get; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public string GetAvatar(string crop)
        {
            return CoreUtils.BuildCropAvatar(this.Url, string.Empty, crop);
        }
    }
}
