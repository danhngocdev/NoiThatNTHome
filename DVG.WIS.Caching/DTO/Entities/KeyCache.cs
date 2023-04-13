using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Caching.DTO.Entities
{
    [Table("KeyCache")]
    public class KeyCacheModel
    {
        public KeyCacheModel()
        {
            CreatedDate = DateTime.Now;
            CreatedDateSpan = Utils.DateTimeToUnixTime(CreatedDate);
        }

        private string _namespace = string.Empty;

        [Key]
        [Column("key", TypeName = "nvarchar")]
        [StringLength(200)]
        public string Key { get; set; }

        [Column("name_space", TypeName = "nvarchar")]
        [StringLength(200)]
        public string Namespace
        {
            get { return !string.IsNullOrEmpty(_namespace) ? _namespace : "dvg"; }
            set { _namespace = value; }
        }

        [Column("created_date", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column("created_datespan", TypeName = "bigint")]
        public long CreatedDateSpan { get; set; }
    }
}
