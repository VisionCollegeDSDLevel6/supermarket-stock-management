using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

[Authorize(Roles = "Admin,Manager,Staff")]
public class StocksController : Controller
{
    private readonly ApplicationDbContext _context;

    public StocksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Stocks
    public async Task<IActionResult> Index()
    {
        var stocks = await _context.Stocks
            .Include(stock => stock.Product)
            .OrderBy(stock => stock.Product!.Name)
            .ToListAsync();

        return View(stocks);
    }

    // GET: Stocks/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var stock = await _context.Stocks
            .Include(item => item.Product)
            .FirstOrDefaultAsync(item =>
                item.StockId == id);

        if (stock == null)
        {
            return NotFound();
        }

        return View(stock);
    }

    // GET: Stocks/History
    // Only Admin and Manager can view stock history
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> History()
    {
        var history = await _context.StockHistories
            .Include(item => item.Product)
            .OrderByDescending(item =>
                item.ChangeDate)
            .ToListAsync();

        return View(history);
    }

    // GET: Stocks/Create
    public async Task<IActionResult> Create()
    {
        await LoadProducts();

        return View();
    }

    // POST: Stocks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("StockId,ProductId,Quantity,LowStockThreshold")]
        Stock stock)
    {
        var stockAlreadyExists =
            await _context.Stocks.AnyAsync(existing =>
                existing.ProductId == stock.ProductId);

        if (stockAlreadyExists)
        {
            ModelState.AddModelError(
                "ProductId",
                "This product already has a stock record."
            );
        }

        if (stock.Quantity < 0)
        {
            ModelState.AddModelError(
                "Quantity",
                "Quantity cannot be negative."
            );
        }

        if (stock.LowStockThreshold < 0)
        {
            ModelState.AddModelError(
                "LowStockThreshold",
                "Low-stock threshold cannot be negative."
            );
        }

        if (ModelState.IsValid)
        {
            _context.Stocks.Add(stock);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await LoadProducts(stock.ProductId);

        return View(stock);
    }

    // GET: Stocks/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var stock = await _context.Stocks
            .FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        await LoadProducts(
            stock.ProductId,
            stock.StockId
        );

        return View(stock);
    }

    // POST: Stocks/Edit/5
    // Updates stock and records quantity changes
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("StockId,ProductId,Quantity,LowStockThreshold")]
        Stock stock)
    {
        if (id != stock.StockId)
        {
            return NotFound();
        }

        var existingStock = await _context.Stocks
            .FirstOrDefaultAsync(item =>
                item.StockId == id);

        if (existingStock == null)
        {
            return NotFound();
        }

        var productUsedByAnotherStock =
            await _context.Stocks.AnyAsync(item =>
                item.ProductId == stock.ProductId &&
                item.StockId != stock.StockId);

        if (productUsedByAnotherStock)
        {
            ModelState.AddModelError(
                "ProductId",
                "This product already has another stock record."
            );
        }

        if (stock.Quantity < 0)
        {
            ModelState.AddModelError(
                "Quantity",
                "Quantity cannot be negative."
            );
        }

        if (stock.LowStockThreshold < 0)
        {
            ModelState.AddModelError(
                "LowStockThreshold",
                "Low-stock threshold cannot be negative."
            );
        }

        if (ModelState.IsValid)
        {
            var previousQuantity =
                existingStock.Quantity;

            existingStock.ProductId =
                stock.ProductId;

            existingStock.Quantity =
                stock.Quantity;

            existingStock.LowStockThreshold =
                stock.LowStockThreshold;

            // Save history only when quantity changes
            if (previousQuantity != stock.Quantity)
            {
                var stockHistory = new StockHistory
                {
                    ProductId = stock.ProductId,
                    PreviousQuantity = previousQuantity,
                    NewQuantity = stock.Quantity,
                    ChangeDate = DateTime.Now,
                    ChangedBy =
                        User.Identity?.Name ?? "Unknown"
                };

                _context.StockHistories.Add(
                    stockHistory);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(stock.StockId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        await LoadProducts(
            stock.ProductId,
            stock.StockId
        );

        return View(stock);
    }

    // GET: Stocks/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var stock = await _context.Stocks
            .Include(item => item.Product)
            .FirstOrDefaultAsync(item =>
                item.StockId == id);

        if (stock == null)
        {
            return NotFound();
        }

        return View(stock);
    }

    // POST: Stocks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var stock = await _context.Stocks
            .FindAsync(id);

        if (stock != null)
        {
            _context.Stocks.Remove(stock);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // Loads products that do not already have a stock record
    private async Task LoadProducts(
        int? selectedProductId = null,
        int? currentStockId = null)
    {
        var products = await _context.Products
            .Where(product =>
                !_context.Stocks.Any(stock =>
                    stock.ProductId ==
                    product.ProductId) ||
                _context.Stocks.Any(stock =>
                    stock.StockId ==
                    currentStockId &&
                    stock.ProductId ==
                    product.ProductId))
            .OrderBy(product =>
                product.Name)
            .ToListAsync();

        ViewData["ProductId"] = new SelectList(
            products,
            "ProductId",
            "Name",
            selectedProductId
        );
    }

    private bool StockExists(int id)
    {
        return _context.Stocks.Any(stock =>
            stock.StockId == id);
    }
}