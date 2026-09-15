using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using System.Globalization;

var cultureInfo = new CultureInfo("en-NZ");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization
                .ReferenceHandler.IgnoreCycles;
    });

// Ensure antiforgery cookies are transmitted only over HTTPS
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy =
        CookieSecurePolicy.Always;

    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Allow requests only from the React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Swagger / REST API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Create the default roles and administrator account
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await IdentitySeeder
            .SeedRolesAndAdminAsync(services);
    }
    catch (Exception exception)
    {
        var logger =
            services.GetRequiredService<ILogger<Program>>();

        logger.LogError(
            exception,
            "An error occurred while seeding Identity data.");
    }

    // Fill in default product images when none is set
    try
    {
        await ProductImageSeeder
            .SeedProductImagesAsync(services);
    }
    catch (Exception exception)
    {
        var logger =
            services.GetRequiredService<ILogger<Program>>();

        logger.LogError(
            exception,
            "An error occurred while seeding product images.");
    }
}

// Enable Swagger and database error pages in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Add security response headers
// This middleware must run before static files and endpoints.
app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd(
        "X-Content-Type-Options",
        "nosniff");

    context.Response.Headers.TryAdd(
        "X-Frame-Options",
        "DENY");

    context.Response.Headers.TryAdd(
        "Referrer-Policy",
        "strict-origin-when-cross-origin");

    context.Response.Headers.TryAdd(
        "Content-Security-Policy",
        "default-src 'self'; " +
        "object-src 'none'; " +
        "base-uri 'self'; " +
        "form-action 'self'; " +
        "script-src 'self' 'unsafe-inline'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data: https:; " +
        "font-src 'self' data:; " +
        "connect-src 'self' http://localhost:3000; " +
        "frame-ancestors 'none';");

    await next();
});

// Serve files from wwwroot, including the local Chart.js file
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();