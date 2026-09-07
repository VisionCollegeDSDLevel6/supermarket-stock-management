using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Models;
using SupermarketStockManagement.Data;

[Route("api/staffs")]
[ApiController]
public class StaffsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public StaffsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Staff
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Staff>>> GetStaff()
    {
        return await _context.Staff.ToListAsync();
    }

    // GET: api/Staff/5
    [HttpGet("{staffid}")]
    public async Task<ActionResult<Staff>> GetStaff(int staffid)
    {
        var staff = await _context.Staff.FindAsync(staffid);

        if (staff == null)
        {
            return NotFound();
        }

        return staff;
    }

    // PUT: api/Staff/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{staffid}")]
    public async Task<IActionResult> PutStaff(int? staffid, Staff staff)
    {
        if (staffid != staff.StaffId)
        {
            return BadRequest();
        }

        _context.Entry(staff).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StaffExists(staffid))
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

    // POST: api/Staff
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Staff>> PostStaff(Staff staff)
    {
        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetStaff", new { staffid = staff.StaffId }, staff);
    }

    // DELETE: api/Staff/5
    [HttpDelete("{staffid}")]
    public async Task<IActionResult> DeleteStaff(int? staffid)
    {
        var staff = await _context.Staff.FindAsync(staffid);
        if (staff == null)
        {
            return NotFound();
        }

        _context.Staff.Remove(staff);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StaffExists(int? staffid)
    {
        return _context.Staff.Any(e => e.StaffId == staffid);
    }
}
