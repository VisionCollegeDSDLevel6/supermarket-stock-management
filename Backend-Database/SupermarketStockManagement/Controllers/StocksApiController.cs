using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

[Route("api/stocks")]
[ApiController]
public class StocksApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StocksApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/stocks
    // Customers can view product availability without logging in
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
    {
        return await _context.Stocks
            .Include(stock => stock.Product)
            .OrderBy(stock => stock.Product!.Name)
            .ToListAsync();
    }

    // GET: api/stocks/5
    [AllowAnonymous]
    [HttpGet("{stockid}")]
    public async Task<ActionResult<Stock>> GetStock(int stockid)
    {
        var stock = await _context.Stocks
            .Include(item => item.Product)
            .FirstOrDefaultAsync(item =>
                item.StockId == stockid);

        if (stock == null)
        {
            return NotFound();
        }

        return stock;
    }

    // PUT: api/stocks/5
    [Authorize(Roles = "Admin,Manager,Staff")]
    [HttpPut("{stockid}")]
    public async Task<IActionResult> PutStock(
        int stockid,
        Stock stock)
    {
        if (stockid != stock.StockId)
        {
            return BadRequest();
        }

        if (stock.Quantity < 0)
        {
            return BadRequest(
                "Quantity cannot be negative."
            );
        }

        if (stock.LowStockThreshold < 0)
        {
            return BadRequest(
                "Low-stock threshold cannot be negative."
            );
        }

        _context.Entry(stock).State =
            EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StockExists(stockid))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/stocks
    [Authorize(Roles = "Admin,Manager,Staff")]
    [HttpPost]
    public async Task<ActionResult<Stock>> PostStock(
        Stock stock)
    {
        if (stock.Quantity < 0)
        {
            return BadRequest(
                "Quantity cannot be negative."
            );
        }

        if (stock.LowStockThreshold < 0)
        {
            return BadRequest(
                "Low-stock threshold cannot be negative."
            );
        }

        var stockAlreadyExists =
            await _context.Stocks.AnyAsync(existing =>
                existing.ProductId == stock.ProductId);

        if (stockAlreadyExists)
        {
            return Conflict(
                "This product already has a stock record."
            );
        }

        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetStock),
            new
            {
                stockid = stock.StockId
            },
            stock
        );
    }

    // DELETE: api/stocks/5
    [Authorize(Roles = "Admin,Manager,Staff")]
    [HttpDelete("{stockid}")]
    public async Task<IActionResult> DeleteStock(int stockid)
    {
        var stock = await _context.Stocks
            .FindAsync(stockid);

        if (stock == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StockExists(int stockid)
    {
        return _context.Stocks.Any(stock =>
            stock.StockId == stockid);
    }
}