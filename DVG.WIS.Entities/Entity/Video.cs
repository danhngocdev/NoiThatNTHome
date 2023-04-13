using DVG.WIS.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DVG.WIS.Entities
{

    [Table("Page")]
    public class Page
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(500)]
        public string Title { set; get; }

        public string Description { set; get; }

        [MaxLength(300)]
        public string SEOTitle { set; get; }
        [MaxLength(500)]
        public string SEODescription { set; get; }
        [MaxLength(500)]
        public string SEOKeyword { set; get; }

    }
}
