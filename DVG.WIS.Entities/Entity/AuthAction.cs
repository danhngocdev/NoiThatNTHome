using DVG.WIS.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DVG.WIS.Entities
{
    [Table("AuthAction")]
    public class AuthAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required]
        [MaxLength(256)]
        public string KeyName { set; get; }

        [MaxLength(1000)]
        public string Description { set; get; }

        [Required]
        [MaxLength(256)]
        public string Controller { set; get; }

        [Required]
        [MaxLength(256)]
        public string Action { set; get; }

        [Required]
        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
