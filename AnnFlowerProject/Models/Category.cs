using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnnFlowerProject.Models
{
    [Table("CATEGORY")]
    public class Category
    {
        [Key]
        [Column("CategoryID")]
        public int CategoryId { get; set; }

        [Required]
        [Column("CategoryName")]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
