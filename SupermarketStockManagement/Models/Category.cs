using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace SupermarketStockManagement.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        // One Category can have many Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}