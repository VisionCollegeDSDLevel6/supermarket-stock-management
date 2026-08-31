using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Models;
using SupermarketStockManagement.Data;

[Route("api/[controller]")]
[ApiController]
public class StocksController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public StocksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Stock
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stock>>> GetStock()
    {
        return await _context.Stocks.ToListAsync();
    }

    // GET: api/Stock/5
    [HttpGet("{stockid}")]
    public async Task<ActionResult<Stock>> GetStock(int stockid)
    {
        var stock = await _context.Stocks.FindAsync(stockid);

        if (stock == null)
        {
            return NotFound();
        }

        return stock;
    }

    // PUT: api/Stock/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{stockid}")]
    public async Task<IActionResult> PutStock(int? stockid, Stock stock)
    {
        if (stockid != stock.StockId)
        {
            return BadRequest();
        }

        _context.Entry(stock).State = EntityState.Modified;

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
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Stock
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Stock>> PostStock(Stock stock)
    {
        _context.Stocks.Add(stock);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetStock", new { stockid = stock.StockId }, stock);
    }

    // DELETE: api/Stock/5
    [HttpDelete("{stockid}")]
    public async Task<IActionResult> DeleteStock(int? stockid)
    {
        var stock = await _context.Stocks.FindAsync(stockid);
        if (stock == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StockExists(int? stockid)
    {
        return _context.Stocks.Any(e => e.StockId == stockid);
    }
}
