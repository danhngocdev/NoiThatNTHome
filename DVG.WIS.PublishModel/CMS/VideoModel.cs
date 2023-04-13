using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class VideoModel
    {
        public VideoModel() { }
        public VideoModel(Entities.Video model)
        {
            this.Id = model.Id;
            this.Title = model.Title;
            this.VideoUrl = model.VideoUrl;
            this.Avatar = model.Avatar;
            this.Link = model.Link;
            this.Status = model.Status;
        }
        public int Id { set; get; }
        public string Title { set; get; }
        public string VideoUrl { set; get; }
        public string Avatar { set; get; }
        public string Link { set; get; }
        public int Status { get; set; }
        public string EncryptPageId
        {
            get { return EncryptUtility.EncryptId(this.Id); }
        }
        public string IdStr
        {
            get { return Id.ToString(); }
            set
            {
                Id = !string.IsNullOrEmpty(value) ? Convert.ToInt32(value) : 0;
            }
        }


        public string StatusName
        {
            get
            {
                return EnumHelper.Instance.ConvertEnumToList<ListStatusEnum>().Where(x => x.Id == Status).FirstOrDefault().Name;
            }
        }



        public class VideoModelSearchModel
        {
            public VideoModelSearchModel()
            {
                this.EditItem = new VideoModel();
            }

            public int Status { get; set; }
            public string Keyword { get; set; }
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public List<VideoModel> ListData { get; set; }
            public VideoModel EditItem { get; set; }
            public IEnumerable<EnumHelper.Enums> ListStatus { get; set; }

        }

    }

}
