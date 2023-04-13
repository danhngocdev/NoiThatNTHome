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
    //[Table("Categories")]
    //public class Category : Auditable
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { set; get; }

    //    [Required]
    //    [MaxLength(256)]
    //    public string Name { set; get; }

    //    [Required]
    //    [MaxLength(256)]
    //    public string Alias { set; get; }

    //    [Required]
    //    public int ParentId { get; set; }

    //    [Required]
    //    public int Priority { get; set; }
    //}
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ShortURL { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public int ParentId { get; set; }
        public bool Invisibled { get; set; }
        public int Status { get; set; }
        public Nullable<bool> AllowComment { get; set; }
        public int Type { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
    }

    public class CategoryModel : Category
    {
        public int Level { get; set; }
        public int TotalRow { get; set; }
        public int CountNews { get; set; }
    }
}
