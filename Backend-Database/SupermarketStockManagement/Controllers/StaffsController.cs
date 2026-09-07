using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

public class StaffsController : Controller
{
    private readonly ApplicationDbContext _context;

    public StaffsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(
            await _context.Staff
                .OrderBy(staff => staff.Name)
                .ToListAsync()
        );
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var staff = await _context.Staff
            .FirstOrDefaultAsync(staff =>
                staff.StaffId == id);

        if (staff == null)
        {
            return NotFound();
        }

        return View(staff);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("StaffId,Name,Email,Role")] Staff staff)
    {
        if (ModelState.IsValid)
        {
            _context.Add(staff);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(staff);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var staff = await _context.Staff.FindAsync(id);

        if (staff == null)
        {
            return NotFound();
        }

        return View(staff);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("StaffId,Name,Email,Role")] Staff staff)
    {
        if (id != staff.StaffId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(staff);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(staff.StaffId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(staff);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var staff = await _context.Staff
            .FirstOrDefaultAsync(staff =>
                staff.StaffId == id);

        if (staff == null)
        {
            return NotFound();
        }

        return View(staff);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var staff = await _context.Staff.FindAsync(id);

        if (staff != null)
        {
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool StaffExists(int id)
    {
        return _context.Staff.Any(staff =>
            staff.StaffId == id);
    }
}