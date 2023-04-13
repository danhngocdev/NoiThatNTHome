using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Table("FM_Photo")]
    public class FM_Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string FileName { get; set; }
        [Required]
        [MaxLength(512)]
        public string FileUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Alternate { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> Capacity { get; set; }
        public Nullable<int> MimeType { get; set; }
        public string MimeTypeName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<long> CreatedDateSpan { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedDateSpan { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> DisplayPosition { get; set; }
        public Nullable<int> DisplayStyle { get; set; }
        public Nullable<long> FileSize { get; set; }
    }

    public class FM_PhotoOnList : FM_Photo
    {
        public int TotalRows { get; set; }
        public int Ordinal { get; set; }
        public int RN { get; set; }
    }
}
