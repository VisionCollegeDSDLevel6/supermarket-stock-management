namespace SupermarketStockManagement.Models
{
    public class InventoryReportViewModel
    {
        // Summary information
        public int TotalProducts { get; set; }

        public int TotalStockQuantity { get; set; }

        public int LowStockProducts { get; set; }

        public int OutOfStockProducts { get; set; }

        public decimal TotalInventoryValue { get; set; }


        // Complete inventory list
        public List<Stock> StockItems { get; set; }
            = new List<Stock>();
    }
}