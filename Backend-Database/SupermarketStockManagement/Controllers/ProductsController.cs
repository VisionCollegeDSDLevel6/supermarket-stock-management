using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Models;
using SupermarketStockManagement.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize(Roles = "Admin,Manager,Staff")]
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: PRODUCTS

    // Display products with optional name search and category filter
    public async Task<IActionResult> Index(
        string? searchTerm,
        int? categoryId)
    {
        var productsQuery = _context.Products
            .Include(product => product.Category)
            .Include(product => product.Stock)
            .AsQueryable();

        // Search products by name
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();

            productsQuery = productsQuery.Where(product =>
                product.Name.Contains(searchTerm));
        }

        // Filter products by category
        if (categoryId.HasValue)
        {
            productsQuery = productsQuery.Where(product =>
                product.CategoryId == categoryId.Value);
        }

        // Preserve the current search value in the view
        ViewData["SearchTerm"] = searchTerm;

        // Load categories for the filter dropdown
        ViewData["CategoryId"] = new SelectList(
            await _context.Categories
                .OrderBy(category => category.Name)
                .ToListAsync(),
            "CategoryId",
            "Name",
            categoryId
        );

        var products = await productsQuery
            .OrderBy(product => product.Name)
            .ToListAsync();

        return View(products);
    }

    // GET: PRODUCTS/Details/5

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(product => product.Category)
            .Include(product => product.Stock)
            .FirstOrDefaultAsync(product =>
                product.ProductId == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: PRODUCTS/Create
    // GET: Products/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(
            _context.Categories,
            "CategoryId",
            "Name"
        );

        return View();
    }

    // POST: PRODUCTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,ImageUrl,CategoryId,Category,Stock,StockHistories")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
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

        ViewData["CategoryId"] = new SelectList(
            _context.Categories,
            "CategoryId",
            "Name",
            product.CategoryId
        );

        return View(product);
    }

    // POST: PRODUCTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? productid, [Bind("ProductId,Name,Description,Price,ImageUrl,CategoryId,Category,Stock,StockHistories")] Product product)
    {
        if (productid != product.ProductId)
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
        return View(product);
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(product => product.Category)
            .Include(product => product.Stock)
            .FirstOrDefaultAsync(product =>
                product.ProductId == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }


    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products
            .FindAsync(id);

        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
    private bool ProductExists(int id)
    {
        return _context.Products.Any(
            product => product.ProductId == id
        );
    }
}
