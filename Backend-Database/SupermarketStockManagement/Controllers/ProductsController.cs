using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;


[Authorize(Roles = "Admin,Manager,Staff")]
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;


    public ProductsController(
        ApplicationDbContext context)
    {
        _context = context;
    }


    // GET: Products
    // Display products with optional name search
    // and category filter
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


        // Preserve the current search value
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


    // GET: Products/Details/5
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


    // GET: Products/Create
    public async Task<IActionResult> Create()
    {
        await LoadCategories();

        return View();
    }


    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "ProductId," +
            "Name," +
            "Description," +
            "Price," +
            "ImageUrl," +
            "CategoryId")]
        Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                $"Product '{product.Name}' was created successfully.";


            return RedirectToAction(nameof(Index));
        }


        await LoadCategories(product.CategoryId);

        return View(product);
    }


    // GET: Products/Edit/5
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


        await LoadCategories(product.CategoryId);

        return View(product);
    }


    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? productid,
        [Bind(
            "ProductId," +
            "Name," +
            "Description," +
            "Price," +
            "ImageUrl," +
            "CategoryId")]
        Product product)
    {
        if (productid != product.ProductId)
        {
            return NotFound();
        }


        if (ModelState.IsValid)
        {
            try
            {
                _context.Products.Update(product);

                await _context.SaveChangesAsync();


                TempData["SuccessMessage"] =
                    $"Product '{product.Name}' was updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId))
                {
                    return NotFound();
                }

                throw;
            }


            return RedirectToAction(nameof(Index));
        }


        await LoadCategories(product.CategoryId);

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
    public async Task<IActionResult> DeleteConfirmed(
        int id)
    {
        var product = await _context.Products.FindAsync(id);


        if (product == null)
        {
            TempData["ErrorMessage"] =
                "The product could not be found.";

            return RedirectToAction(nameof(Index));
        }


        var productName = product.Name;


        _context.Products.Remove(product);

        await _context.SaveChangesAsync();


        TempData["SuccessMessage"] =
            $"Product '{productName}' was deleted successfully.";


        return RedirectToAction(nameof(Index));
    }


    // Load categories for Create and Edit forms
    private async Task LoadCategories(
        int? selectedCategoryId = null)
    {
        var categories = await _context.Categories
            .OrderBy(category => category.Name)
            .ToListAsync();


        ViewData["CategoryId"] = new SelectList(
            categories,
            "CategoryId",
            "Name",
            selectedCategoryId
        );
    }


    private bool ProductExists(int id)
    {
        return _context.Products.Any(product =>
            product.ProductId == id);
    }
}