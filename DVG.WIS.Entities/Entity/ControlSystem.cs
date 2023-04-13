using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Entities
{
    [Table("ControlSystem")]
    public class ControlSystem
    {
        public ControlSystem()
        {
            this.IsEnabled = true;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(256)]
        public string KeyName { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(256)]
        public string Controller { get; set; }

        public bool IsEnabled { get; set; }
    }
}
