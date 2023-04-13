using DVG.WIS.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DVG.WIS.Entities
{

    [Table("Products")]
    public class Product : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(500)]
        public string Name { set; get; }

        [Required]
        [MaxLength(500)]
        public string Sapo { set; get; }
        //public string Information { set; get; }
        //public string Standard { set; get; }
        //public string Certificate { set; get; }
        public string Description { get; set; }

        [Required]
        [MaxLength(256)]
        public string Avatar { set; get; }

        [MaxLength(300)]
        public string SEOTitle { set; get; }
        [MaxLength(500)]
        public string SEODescription { set; get; }
        [MaxLength(500)]
        public string SEOKeyword { set; get; }
        public string TextSearch { set; get; }
        public double Price { get; set; }
        public double? PricePromotion { get; set; }
        public string Code { get; set; }
        public string Capacity { get; set; }
        public string MadeIn { get; set; }
        public int IsOutStock { get; set; }
        public int IsHighLight { get; set; }
        public int CategoryId { get; set; }
    }
}
