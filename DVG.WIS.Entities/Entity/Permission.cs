using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Table("Permission")]
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionID { get; set; }
        [Required]
        public string PermissionName { get; set; }
        [Required]
        public int GroupID { get; set; }
        [Required]
        public bool IsHidden { get; set; }
    }
}
