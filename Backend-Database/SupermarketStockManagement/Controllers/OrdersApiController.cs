using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;

[Route("api/orders")]
[ApiController]
public class OrdersApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    public class OrderItemRequest
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public List<OrderItemRequest> Items { get; set; } = new();
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new
            {
                o.Id,
                o.CustomerName,
                o.CustomerEmail,
                o.CustomerPhone,
                o.DeliveryAddress,
                o.OrderDate,
                o.TotalAmount,
                o.Status,
                Items = o.Items.Select(i => new
                {
                    i.ProductId,
                    i.ProductName,
                    i.UnitPrice,
                    i.Quantity
                })
            })
            .ToListAsync();

        return Ok(orders);
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.Id == id)
            .Select(o => new
            {
                o.Id,
                o.CustomerName,
                o.CustomerEmail,
                o.CustomerPhone,
                o.DeliveryAddress,
                o.OrderDate,
                o.TotalAmount,
                o.Status,
                Items = o.Items.Select(i => new
                {
                    i.ProductId,
                    i.ProductName,
                    i.UnitPrice,
                    i.Quantity
                })
            })
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<object>> CreateOrder([FromBody] OrderRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName) ||
            string.IsNullOrWhiteSpace(request.CustomerEmail) ||
            string.IsNullOrWhiteSpace(request.DeliveryAddress) ||
            request.Items == null ||
            request.Items.Count == 0)
        {
            return BadRequest(new { success = false, message = "Customer details and at least one item are required." });
        }

        var order = new Order
        {
            CustomerName = request.CustomerName.Trim(),
            CustomerEmail = request.CustomerEmail.Trim(),
            CustomerPhone = request.CustomerPhone?.Trim(),
            DeliveryAddress = request.DeliveryAddress.Trim(),
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            TotalAmount = request.Items.Sum(i => i.UnitPrice * i.Quantity)
        };

        foreach (var item in request.Items)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName.Trim(),
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Order placed successfully!", orderId = order.Id, total = order.TotalAmount });
    }
}