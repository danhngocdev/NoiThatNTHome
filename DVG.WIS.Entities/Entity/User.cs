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
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        public string Mobile { get; set; }
        [MaxLength(256)]
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedDateSpan { get; set; }
        public string Desciption { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }
        public long LastLogin { get; set; }
        public long LastPasswordChange { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        [MaxLength(256)]
        public string Avatar { get; set; }
        [Required]
        public int Status { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string Signature { get; set; }
        public Nullable<byte> CityId { get; set; }
        public Nullable<long> Birthday { get; set; }
        public Nullable<long> LastUpdate { get; set; }
        [MaxLength(256)]
        public string Skype { get; set; }
        [MaxLength(256)]
        public string Facebook { get; set; }
        [MaxLength(256)]
        public string Google { get; set; }
        [MaxLength(256)]
        public string GoogleId { get; set; }
        [MaxLength(256)]
        public string FacebookId { get; set; }
        [MaxLength(256)]
        public string Transporter { get; set; }
        public Nullable<long> UserType { get; set; }

        [NotMapped]
        public Nullable<int> RoleId { get; set; }
    }
}
