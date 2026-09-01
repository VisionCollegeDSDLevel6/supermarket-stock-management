
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Models;
using SupermarketStockManagement.Data;

public class StocksController : Controller
{
    private readonly ApplicationDbContext _context;

    public StocksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: STOCKS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Stocks.ToListAsync());
    }

    // GET: STOCKS/Details/5
    public async Task<IActionResult> Details(int? stockid)
    {
        if (stockid == null)
        {
            return NotFound();
        }

        var stock = await _context.Stocks
            .FirstOrDefaultAsync(m => m.StockId == stockid);
        if (stock == null)
        {
            return NotFound();
        }

        return View(stock);
    }

    // GET: STOCKS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: STOCKS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StockId,ProductId,Quantity,LowStockThreshold,Product")] Stock stock)
    {
        if (ModelState.IsValid)
        {
            _context.Add(stock);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(stock);
    }

    // GET: STOCKS/Edit/5
    public async Task<IActionResult> Edit(int? stockid)
    {
        if (stockid == null)
        {
            return NotFound();
        }

        var stock = await _context.Stocks.FindAsync(stockid);
        if (stock == null)
        {
            return NotFound();
        }
        return View(stock);
    }

    // POST: STOCKS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? stockid, [Bind("StockId,ProductId,Quantity,LowStockThreshold,Product")] Stock stock)
    {
        if (stockid != stock.StockId)
        {
            return NotFound();
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
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(stock);
    }

    // GET: STOCKS/Delete/5
    public async Task<IActionResult> Delete(int? stockid)
    {
        if (stockid == null)
        {
            return NotFound();
        }

        var stock = await _context.Stocks
            .FirstOrDefaultAsync(m => m.StockId == stockid);
        if (stock == null)
        {
            return NotFound();
        }

        return View(stock);
    }

    // POST: STOCKS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? stockid)
    {
        var stock = await _context.Stocks.FindAsync(stockid);
        if (stock != null)
        {
            _context.Stocks.Remove(stock);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StockExists(int? stockid)
    {
        return _context.Stocks.Any(e => e.StockId == stockid);
    }
}
