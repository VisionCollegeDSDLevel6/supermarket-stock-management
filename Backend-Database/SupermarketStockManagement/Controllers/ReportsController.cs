using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

namespace SupermarketStockManagement.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var stockItems = await _context.Stocks
                .Include(stock => stock.Product)
                .ThenInclude(product => product!.Category)
                .OrderBy(stock => stock.Product!.Name)
                .ToListAsync();


            var viewModel = new InventoryReportViewModel
            {
                TotalProducts =
                    await _context.Products.CountAsync(),

                TotalStockQuantity =
                    stockItems.Sum(stock =>
                        stock.Quantity),

                // Low stock excludes products already out of stock
                LowStockProducts =
                    stockItems.Count(stock =>
                        stock.Quantity > 0 &&
                        stock.Quantity <=
                        stock.LowStockThreshold),

                OutOfStockProducts =
                    stockItems.Count(stock =>
                        stock.Quantity == 0),

                TotalInventoryValue =
                    stockItems.Sum(stock =>
                        (stock.Product?.Price ?? 0) *
                        stock.Quantity),

                StockItems = stockItems
            };


            return View(viewModel);
        }
    }
}