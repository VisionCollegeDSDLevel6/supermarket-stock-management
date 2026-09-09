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
}