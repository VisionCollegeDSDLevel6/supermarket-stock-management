using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

[Authorize(Roles = "Admin,Manager")]
[Route("api/staffs")]
[ApiController]
public class StaffsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StaffsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/staffs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Staff>>> GetStaff()
    {
        var query = _context.Staff.AsQueryable();

        // Managers can only view Staff accounts
        if (User.IsInRole("Manager"))
        {
            query = query.Where(staff =>
                staff.Role == "Staff");
        }

        var staffList = await query
            .OrderBy(staff => staff.Name)
            .ToListAsync();

        return Ok(staffList);
    }

    // GET: api/staffs/5
    [HttpGet("{staffid}")]
    public async Task<ActionResult<Staff>> GetStaff(int staffid)
    {
        var staff = await _context.Staff
            .FirstOrDefaultAsync(item =>
                item.StaffId == staffid);

        if (staff == null)
        {
            return NotFound();
        }

        // Managers cannot view other Manager accounts
        if (User.IsInRole("Manager") &&
            staff.Role != "Staff")
        {
            return Forbid();
        }

        return Ok(staff);
    }
}