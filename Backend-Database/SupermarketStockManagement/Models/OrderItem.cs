using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketStockManagement.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        // Navigation
        public Order? Order { get; set; }
    }
}