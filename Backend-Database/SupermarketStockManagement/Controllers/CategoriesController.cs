using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

[Authorize(Roles = "Admin,Manager,Staff")]
public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Include(category => category.Products)
            .OrderBy(category => category.Name)
            .ToListAsync();

        return View(categories);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .Include(category => category.Products)
            .ThenInclude(product => product.Stock)
            .FirstOrDefaultAsync(category =>
                category.CategoryId == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("CategoryId,Name,Description")]
        Category category)
    {
        var duplicateName = await _context.Categories
            .AnyAsync(existing =>
                existing.Name.ToLower() ==
                category.Name.ToLower());

        if (duplicateName)
        {
            ModelState.AddModelError(
                "Name",
                "A category with this name already exists."
            );
        }

        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("CategoryId,Name,Description")]
        Category category)
    {
        if (id != category.CategoryId)
        {
            return NotFound();
        }

        var duplicateName = await _context.Categories
            .AnyAsync(existing =>
                existing.CategoryId != category.CategoryId &&
                existing.Name.ToLower() ==
                category.Name.ToLower());

        if (duplicateName)
        {
            ModelState.AddModelError(
                "Name",
                "A category with this name already exists."
            );
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.CategoryId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .Include(category => category.Products)
            .FirstOrDefaultAsync(category =>
                category.CategoryId == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Categories
            .Include(category => category.Products)
            .FirstOrDefaultAsync(category =>
                category.CategoryId == id);

        if (category == null)
        {
            return NotFound();
        }

        if (category.Products.Any())
        {
            TempData["CategoryDeleteError"] =
                "This category cannot be deleted because it still contains products.";

            return RedirectToAction(
                nameof(Delete),
                new { id }
            );
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(category =>
            category.CategoryId == id);
    }
}