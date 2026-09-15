using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

[Route("api/contact")]
[ApiController]
public class ContactApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ContactApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    public class ContactRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    // GET: api/contact
    // Lists all contact messages.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetMessages(
        [FromQuery] bool unreadOnly = false,
        [FromQuery] int? count = null)
    {
        var query = _context.ContactMessages
            .OrderByDescending(m => m.SubmittedAt)
            .AsQueryable();

        if (unreadOnly)
        {
            query = query.Where(m => !m.IsRead);
        }

        if (count.HasValue && count.Value > 0)
        {
            query = query.Take(count.Value);
        }

        var messages = await query
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.Email,
                m.Phone,
                m.Subject,
                m.Message,
                m.SubmittedAt,
                m.IsRead
            })
            .ToListAsync();

        return Ok(messages);
    }

    // GET: api/contact/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetMessage(int id)
    {
        var message = await _context.ContactMessages
            .Where(m => m.Id == id)
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.Email,
                m.Phone,
                m.Subject,
                m.Message,
                m.SubmittedAt,
                m.IsRead
            })
            .FirstOrDefaultAsync();

        if (message == null)
        {
            return NotFound();
        }

        return Ok(message);
    }

    // POST: api/contact
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] ContactRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Subject) ||
            string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new { success = false, message = "All required fields must be filled." });
        }

        var contact = new ContactMessage
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Phone = request.Phone?.Trim(),
            Subject = request.Subject.Trim(),
            Message = request.Message.Trim(),
            SubmittedAt = DateTime.UtcNow,
            IsRead = false
        };

        _context.ContactMessages.Add(contact);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Your message has been sent. We'll get back to you soon!" });
    }

    // PUT: api/contact/{id}/read
    // Marks a message as read.
    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        message.IsRead = true;
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Message marked as read." });
    }
}