using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketStockManagement.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string CustomerEmail { get; set; } = string.Empty;

        [StringLength(20)]
        public string? CustomerPhone { get; set; }

        [Required]
        [StringLength(500)]
        public string DeliveryAddress { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        // Navigation
        public List<OrderItem> Items { get; set; } = new();
    }
}