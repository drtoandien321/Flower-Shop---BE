using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnnFlowerProject.Models
{
    [Table("PRODUCT")]
    public class Product
    {
        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }

        [Required]
        [Column("ProductName")]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Column("UnitPrice", TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column("StockQuantity")]
        public int StockQuantity { get; set; }

        [Required]
        [Column("ImageURL")]
        [StringLength(500)]
        public string ImageURL { get; set; } = string.Empty;

        [Required]
        [Column("Description")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [ForeignKey("Category")]
        [Column("CategoryID")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
