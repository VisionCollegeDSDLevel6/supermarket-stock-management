using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/auth")]
[ApiController]
public class AuthApiController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthApiController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FullName { get; set; }
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserId { get; set; }
        public string[]? Roles { get; set; }
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new AuthResponse
            {
                Success = false,
                Message = "Email and password are required."
            });
        }

        var email = request.Email.Trim();

        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null)
        {
            return BadRequest(new AuthResponse
            {
                Success = false,
                Message = "An account with this email already exists."
            });
        }

        var user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors.Select(e => e.Description);
            return BadRequest(new AuthResponse
            {
                Success = false,
                Message = "Registration failed: " + string.Join(" ", errors)
            });
        }

        // Make sure the "Customer" role exists, then assign it.
        if (!await _roleManager.RoleExistsAsync("Customer"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Customer"));
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        return Ok(new AuthResponse
        {
            Success = true,
            Message = "Registration successful. You can now log in.",
            Email = user.Email,
            UserId = user.Id,
            Roles = new[] { "Customer" }
        });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new AuthResponse
            {
                Success = false,
                Message = "Email and password are required."
            });
        }

        var user = await _userManager.FindByEmailAsync(request.Email.Trim());
        if (user == null)
        {
            return Unauthorized(new AuthResponse
            {
                Success = false,
                Message = "Invalid email or password."
            });
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            return Unauthorized(new AuthResponse
            {
                Success = false,
                Message = "Invalid email or password."
            });
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new AuthResponse
        {
            Success = true,
            Message = "Login successful.",
            Email = user.Email,
            UserId = user.Id,
            Roles = roles.ToArray()
        });
    }
}