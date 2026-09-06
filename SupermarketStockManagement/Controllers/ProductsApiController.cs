using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Models;
using SupermarketStockManagement.Data;

[Route("api/products")]
[ApiController]
public class ProductsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ProductsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Product
    // Supports search/filter via query parameters:
    //   ?searchTerm=&categoryId=&minPrice=&maxPrice=&sortBy=name&sortOrder=asc&lowStockOnly=false
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct(
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortOrder = "asc",
        [FromQuery] bool lowStockOnly = false)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Stock)
            .AsQueryable();

        // Search by name or description
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                (p.Description != null && p.Description.ToLower().Contains(term)));
        }

        // Filter by category
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        // Filter by price range
        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        // Filter low stock only
        if (lowStockOnly)
        {
            query = query.Where(p => p.Stock != null && p.Stock.Quantity <= p.Stock.LowStockThreshold);
        }

        // Sorting
        query = (sortBy?.ToLower()) switch
        {
            "name" => sortOrder == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "price" => sortOrder == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "category" => sortOrder == "desc" ? query.OrderByDescending(p => p.Category!.Name) : query.OrderBy(p => p.Category!.Name),
            "stock" => sortOrder == "desc" ? query.OrderByDescending(p => p.Stock!.Quantity) : query.OrderBy(p => p.Stock!.Quantity),
            _ => query.OrderBy(p => p.Name)
        };

        return await query.ToListAsync();
    }

    // GET: api/Product/5
    [HttpGet("{productid}")]
    public async Task<ActionResult<Product>> GetProduct(int productid)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Stock)
            .FirstOrDefaultAsync(p => p.ProductId == productid);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // PUT: api/Product/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{productid}")]
    public async Task<IActionResult> PutProduct(int? productid, Product product)
    {
        if (productid != product.ProductId)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(productid))
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

    // POST: api/Product
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProduct", new { productid = product.ProductId }, product);
    }

    // DELETE: api/Product/5
    [HttpDelete("{productid}")]
    public async Task<IActionResult> DeleteProduct(int? productid)
    {
        var product = await _context.Products.FindAsync(productid);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int? productid)
    {
        return _context.Products.Any(e => e.ProductId == productid);
    }
}
