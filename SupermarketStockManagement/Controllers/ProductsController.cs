
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Models;
using SupermarketStockManagement.Data;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: PRODUCTS
    // Supports search/filter: ?searchTerm=&categoryId=&minPrice=&maxPrice=&sortBy=&sortOrder=
    public async Task<IActionResult> Index(
        string? searchTerm = null,
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? sortBy = null,
        string sortOrder = "asc")
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

        // Sorting
        query = (sortBy?.ToLower()) switch
        {
            "name" => sortOrder == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "price" => sortOrder == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "category" => sortOrder == "desc" ? query.OrderByDescending(p => p.Category!.Name) : query.OrderBy(p => p.Category!.Name),
            "stock" => sortOrder == "desc" ? query.OrderByDescending(p => p.Stock!.Quantity) : query.OrderBy(p => p.Stock!.Quantity),
            _ => query.OrderBy(p => p.Name)
        };

        // Pass filter values to view for form persistence
        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.MinPrice = minPrice;
        ViewBag.MaxPrice = maxPrice;
        ViewBag.SortBy = sortBy;
        ViewBag.SortOrder = sortOrder;
        ViewBag.Categories = new SelectList(
            await _context.Categories.ToListAsync(), "CategoryId", "Name", categoryId);

        return View(await query.ToListAsync());
    }

    // GET: PRODUCTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: PRODUCTS/Create
    public IActionResult Create()
    {
        ViewBag.CategoryId = new SelectList(
            _context.Categories.ToList(), "CategoryId", "Name");
        return View();
    }

    // POST: PRODUCTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,ImageUrl,CategoryId")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();

            // Auto-create a stock entry for the new product
            var stock = new Stock
            {
                ProductId = product.ProductId,
                Quantity = 0,
                LowStockThreshold = 5
            };
            _context.Add(stock);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        ViewBag.CategoryId = new SelectList(
            _context.Categories.ToList(), "CategoryId", "Name", product.CategoryId);
        return View(product);
    }

    // GET: PRODUCTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewBag.CategoryId = new SelectList(
            _context.Categories.ToList(), "CategoryId", "Name", product.CategoryId);
        return View(product);
    }

    // POST: PRODUCTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ProductId,Name,Description,Price,ImageUrl,CategoryId")] Product product)
    {
        if (id != product.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId))
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
        ViewBag.CategoryId = new SelectList(
            _context.Categories.ToList(), "CategoryId", "Name", product.CategoryId);
        return View(product);
    }

    // GET: PRODUCTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: PRODUCTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int? id)
    {
        return _context.Products.Any(e => e.ProductId == id);
    }
}
