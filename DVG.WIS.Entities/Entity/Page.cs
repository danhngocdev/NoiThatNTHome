using DVG.WIS.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DVG.WIS.Entities
{

    [Table("Video")]
    public class Video
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        public string Title { set; get; }
        public string VideoUrl { set; get; }
        public string Avatar { set; get; }
        public string Link { set; get; }
        public int Status { get; set; }
    }
}
