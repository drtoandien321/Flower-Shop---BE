using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnnFlowerProject.Models
{
    [Table("USERS")]
    public class User
    {
        [Key]
        [Column("UserID")]
        public int UserId { get; set; }

        [Required]
        [Column("FullName")]
        [StringLength(100)]
        public string Fullname { get; set; } = string.Empty;

        [Required]
        [Column("Email")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("Password")]
        [StringLength(200)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Column("Phone")]
        [StringLength(200)]
        public string Phone { get; set; } = string.Empty;

        [ForeignKey("Role")]
        [Column("RoleID")]
        public int RoleId { get; set; }

        public Role? Role { get; set; }
    }
}
