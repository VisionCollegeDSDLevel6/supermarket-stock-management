using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketStockManagement.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation Property
        public Category? Category { get; set; }

        public Stock? Stock { get; set; }

        public ICollection<StockHistory> StockHistories { get; set; }
            = new List<StockHistory>();
    }
}