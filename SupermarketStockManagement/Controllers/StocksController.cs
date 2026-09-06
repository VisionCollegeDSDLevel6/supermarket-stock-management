using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

public class StocksController : Controller
{
    private readonly ApplicationDbContext _context;

    public StocksController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var stocks = await _context.Stocks
            .Include(stock => stock.Product)
            .OrderBy(stock => stock.Product!.Name)
            .ToListAsync();

        return View(stocks);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var stock = await _context.Stocks
            .Include(stock => stock.Product)
            .FirstOrDefaultAsync(stock =>
                stock.StockId == id);

        if (stock == null)
        {
            return NotFound();
        }

        return View(stock);
    }

    public async Task<IActionResult> Create()
    {
        await LoadProducts();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("StockId,ProductId,Quantity,LowStockThreshold")]
        Stock stock)
    {
        var stockAlreadyExists = await _context.Stocks
            .AnyAsync(existing =>
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
            _context.Add(stock);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await LoadProducts(stock.ProductId);
        return View(stock);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var stock = await _context.Stocks.FindAsync(id);

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

        var productUsedByAnotherStock =
            await _context.Stocks.AnyAsync(existing =>
                existing.ProductId == stock.ProductId &&
                existing.StockId != stock.StockId);

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
            try
            {
                _context.Update(stock);
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

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var stock = await _context.Stocks
            .Include(stock => stock.Product)
            .FirstOrDefaultAsync(stock =>
                stock.StockId == id);

        if (stock == null)
        {
            return NotFound();
        }

        return View(stock);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock != null)
        {
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadProducts(
        int? selectedProductId = null,
        int? currentStockId = null)
    {
        var products = await _context.Products
            .Where(product =>
                !_context.Stocks.Any(stock =>
                    stock.ProductId == product.ProductId) ||
                _context.Stocks.Any(stock =>
                    stock.StockId == currentStockId &&
                    stock.ProductId == product.ProductId))
            .OrderBy(product => product.Name)
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