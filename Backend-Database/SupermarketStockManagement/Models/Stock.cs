using System.ComponentModel.DataAnnotations;

namespace SupermarketStockManagement.Models
{
    public class Stock
    {
        public int StockId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int LowStockThreshold { get; set; } = 5;

        // Navigation Property
        public Product? Product { get; set; }
    }
}