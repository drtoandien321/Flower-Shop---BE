using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnnFlowerProject.Models
{
    [Table("ROLE")]
    public class Role
    {
        [Key]
        [Column("RoleID")]
        public int RoleId { get; set; }

        [Required]
        [Column("RoleName")]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
