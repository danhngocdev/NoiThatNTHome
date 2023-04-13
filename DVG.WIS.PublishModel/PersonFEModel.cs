using DVG.WIS.Core;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.PublicModel
{
    public class PersonFEModel
    {
        public PersonFEModel(Person model)
        {
            Id = model.Id;
            Name = model.Name;
            Avatar = model.Avatar;
            Description = model.Description;
            Position = model.Position;
            Age = model.Age;
            Score = model.Score;
            PageId = model.PageId;
            Priority = model.Priority;
        }

        public int Id { set; get; }
        public string Name { set; get; }
        public string Avatar { set; get; }
        public string Description { set; get; }
        public string Position { get; set; }
        public int Age { get; set; }
        public string Score { get; set; }
        public int PageId { get; set; }
        public int Priority { get; set; }

        public string GetAvatar(string crop)
        {
            return CoreUtils.BuildCropAvatar(this.Avatar, string.Empty, crop);
        }
    }
}
