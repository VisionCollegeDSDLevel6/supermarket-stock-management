namespace SupermarketStockManagement.Models
{
    public class DashboardViewModel
    {
        // Summary cards
        public int TotalProducts { get; set; }

        public int TotalCategories { get; set; }

        public int TotalStockQuantity { get; set; }

        public int LowStockProducts { get; set; }

        public int TotalStaff { get; set; }


        // Stock quantity chart
        public List<string> ProductNames { get; set; }
            = new List<string>();

        public List<int> StockQuantities { get; set; }
            = new List<int>();


        // Category chart
        public List<string> CategoryNames { get; set; }
            = new List<string>();

        public List<int> CategoryProductCounts { get; set; }
            = new List<int>();


        // Dashboard tables
        public List<Product> RecentProducts { get; set; }
            = new List<Product>();

        public List<Stock> LowStockItems { get; set; }
            = new List<Stock>();
    }
}