
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Models;
using SupermarketStockManagement.Data;

public class StaffsController : Controller
{
    private readonly ApplicationDbContext _context;

    public StaffsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: STAFFS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Staff.ToListAsync());
    }

    // GET: STAFFS/Details/5
    public async Task<IActionResult> Details(int? staffid)
    {
        if (staffid == null)
        {
            return NotFound();
        }

        var staff = await _context.Staff
            .FirstOrDefaultAsync(m => m.StaffId == staffid);
        if (staff == null)
        {
            return NotFound();
        }

        return View(staff);
    }

    // GET: STAFFS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: STAFFS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StaffId,Name,Email,Role")] Staff staff)
    {
        if (ModelState.IsValid)
        {
            _context.Add(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(staff);
    }

    // GET: STAFFS/Edit/5
    public async Task<IActionResult> Edit(int? staffid)
    {
        if (staffid == null)
        {
            return NotFound();
        }

        var staff = await _context.Staff.FindAsync(staffid);
        if (staff == null)
        {
            return NotFound();
        }
        return View(staff);
    }

    // POST: STAFFS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? staffid, [Bind("StaffId,Name,Email,Role")] Staff staff)
    {
        if (staffid != staff.StaffId)
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
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(staff);
    }

    // GET: STAFFS/Delete/5
    public async Task<IActionResult> Delete(int? staffid)
    {
        if (staffid == null)
        {
            return NotFound();
        }

        var staff = await _context.Staff
            .FirstOrDefaultAsync(m => m.StaffId == staffid);
        if (staff == null)
        {
            return NotFound();
        }

        return View(staff);
    }

    // POST: STAFFS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? staffid)
    {
        var staff = await _context.Staff.FindAsync(staffid);
        if (staff != null)
        {
            _context.Staff.Remove(staff);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StaffExists(int? staffid)
    {
        return _context.Staff.Any(e => e.StaffId == staffid);
    }
}
