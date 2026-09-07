using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

namespace SupermarketStockManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        // Dashboard
        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                // Summary card information
                TotalProducts =
                    await _context.Products.CountAsync(),

                TotalCategories =
                    await _context.Categories.CountAsync(),

                TotalStaff =
                    await _context.Staff.CountAsync(),

                TotalStockQuantity =
                    await _context.Stocks
                        .SumAsync(
                            stock =>
                                (int?)stock.Quantity
                        ) ?? 0,

                LowStockProducts =
                    await _context.Stocks
                        .CountAsync(
                            stock =>
                                stock.Quantity <=
                                stock.LowStockThreshold
                        ),


                // Five most recently added products
                RecentProducts =
                    await _context.Products
                        .Include(product =>
                            product.Category)
                        .Include(product =>
                            product.Stock)
                        .OrderByDescending(product =>
                            product.ProductId)
                        .Take(5)
                        .ToListAsync(),


                // Products that have low stock
                LowStockItems =
                    await _context.Stocks
                        .Include(stock =>
                            stock.Product)
                        .Where(stock =>
                            stock.Quantity <=
                            stock.LowStockThreshold)
                        .OrderBy(stock =>
                            stock.Quantity)
                        .Take(5)
                        .ToListAsync()
            };


            // Stock quantity chart data
            var stockChartData =
                await _context.Stocks
                    .Include(stock =>
                        stock.Product)
                    .OrderByDescending(stock =>
                        stock.Quantity)
                    .Take(8)
                    .Select(stock => new
                    {
                        ProductName =
                            stock.Product != null
                                ? stock.Product.Name
                                : "Unknown",

                        stock.Quantity
                    })
                    .ToListAsync();

            viewModel.ProductNames =
                stockChartData
                    .Select(item =>
                        item.ProductName)
                    .ToList();

            viewModel.StockQuantities =
                stockChartData
                    .Select(item =>
                        item.Quantity)
                    .ToList();


            // Product count by category chart data
            var categoryChartData =
                await _context.Categories
                    .Select(category => new
                    {
                        category.Name,

                        ProductCount =
                            category.Products.Count()
                    })
                    .OrderByDescending(item =>
                        item.ProductCount)
                    .ToListAsync();

            viewModel.CategoryNames =
                categoryChartData
                    .Select(item =>
                        item.Name)
                    .ToList();

            viewModel.CategoryProductCounts =
                categoryChartData
                    .Select(item =>
                        item.ProductCount)
                    .ToList();


            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id ??
                        HttpContext.TraceIdentifier
                }
            );
        }
    }
}