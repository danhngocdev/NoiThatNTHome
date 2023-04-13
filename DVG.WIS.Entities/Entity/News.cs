using DVG.WIS.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DVG.WIS.Entities
{

    [Table("News")]
    public class News : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        public int Type { set; get; }

        [Required]
        [MaxLength(500)]
        public string Title { set; get; }

        [Required]
        [MaxLength(500)]
        public string Sapo { set; get; }
       
        public string Description { set; get; }
        [Required]
        [MaxLength(256)]
        public string Avatar { set; get; }

        [MaxLength(256)]
        public string Source { set; get; }

        [Required]
        public int CategoryId { set; get; }


        [MaxLength(300)]
        public string SEOTitle { set; get; }
        [MaxLength(500)]
        public string SEODescription { set; get; }
        [MaxLength(500)]
        public string SEOKeyword { set; get; }

        public string TextSearch { set; get; }

        public DateTime PublishedDate { get; set; }
        public int LanguageId { get; set; }
        public int IsHighLight { get; set; }
    }
}
