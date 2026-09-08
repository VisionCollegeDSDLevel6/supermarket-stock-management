using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

[Route("api/products")]
[ApiController]
public class ProductsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/products
    // Supports searching, filtering and sorting through query parameters
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string sortOrder = "asc",
        [FromQuery] bool lowStockOnly = false)
    {
        var query = _context.Products
            .Include(product => product.Category)
            .Include(product => product.Stock)
            .AsQueryable();

        // Search by product name or description
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();

            query = query.Where(product =>
                product.Name.Contains(searchTerm) ||
                (product.Description != null &&
                 product.Description.Contains(searchTerm)));
        }

        // Filter by category
        if (categoryId.HasValue)
        {
            query = query.Where(product =>
                product.CategoryId == categoryId.Value);
        }

        // Filter by minimum price
        if (minPrice.HasValue)
        {
            query = query.Where(product =>
                product.Price >= minPrice.Value);
        }

        // Filter by maximum price
        if (maxPrice.HasValue)
        {
            query = query.Where(product =>
                product.Price <= maxPrice.Value);
        }

        // Show only products with low stock
        if (lowStockOnly)
        {
            query = query.Where(product =>
                product.Stock != null &&
                product.Stock.Quantity <=
                product.Stock.LowStockThreshold);
        }

        // Sort the product results
        query = sortBy?.ToLower() switch
        {
            "name" when sortOrder == "desc" =>
                query.OrderByDescending(product =>
                    product.Name),

            "name" =>
                query.OrderBy(product =>
                    product.Name),

            "price" when sortOrder == "desc" =>
                query.OrderByDescending(product =>
                    product.Price),

            "price" =>
                query.OrderBy(product =>
                    product.Price),

            "category" when sortOrder == "desc" =>
                query.OrderByDescending(product =>
                    product.Category!.Name),

            "category" =>
                query.OrderBy(product =>
                    product.Category!.Name),

            "stock" when sortOrder == "desc" =>
                query.OrderByDescending(product =>
                    product.Stock!.Quantity),

            "stock" =>
                query.OrderBy(product =>
                    product.Stock!.Quantity),

            _ => query.OrderBy(product =>
                product.Name)
        };

        return await query.ToListAsync();
    }

    // GET: api/products/5
    [AllowAnonymous]
    [HttpGet("{productid}")]
    public async Task<ActionResult<Product>> GetProduct(int productid)
    {
        var product = await _context.Products
            .Include(item => item.Category)
            .Include(item => item.Stock)
            .FirstOrDefaultAsync(item =>
                item.ProductId == productid);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // PUT: api/products/5
    [Authorize(Roles = "Admin,Manager,Staff")]
    [HttpPut("{productid}")]
    public async Task<IActionResult> PutProduct(
        int productid,
        Product product)
    {
        if (productid != product.ProductId)
        {
            return BadRequest();
        }

        _context.Entry(product).State =
            EntityState.Modified;

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

            throw;
        }

        return NoContent();
    }

    // POST: api/products
    [Authorize(Roles = "Admin,Manager,Staff")]
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(
        Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetProduct),
            new
            {
                productid = product.ProductId
            },
            product
        );
    }

    // DELETE: api/products/5
    [Authorize(Roles = "Admin,Manager,Staff")]
    [HttpDelete("{productid}")]
    public async Task<IActionResult> DeleteProduct(int productid)
    {
        var product = await _context.Products
            .FindAsync(productid);

        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int productid)
    {
        return _context.Products.Any(product =>
            product.ProductId == productid);
    }
}