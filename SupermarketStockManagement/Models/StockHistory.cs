using System.ComponentModel.DataAnnotations;

namespace SupermarketStockManagement.Models
{
    public class StockHistory
    {
        public int StockHistoryId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int PreviousQuantity { get; set; }

        public int NewQuantity { get; set; }

        public DateTime ChangeDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string? ChangedBy { get; set; }

        // Navigation Property
        public Product? Product { get; set; }
    }
}