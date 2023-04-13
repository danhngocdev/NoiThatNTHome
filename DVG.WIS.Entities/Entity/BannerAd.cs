using DVG.WIS.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Table("BannerAds")]
    public class BannerAd : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(500)]
        public string Title { set; get; }

        [Required]
        public int PageId { set; get; }

        [Required]
        public int Position { set; get; }

        [Required]
        [MaxLength(256)]
        public string ImageUrl { set; get; }

        [Required]
        public DateTime StartDate { set; get; }

        [Required]
        public DateTime EndDate { set; get; }
    }
}
