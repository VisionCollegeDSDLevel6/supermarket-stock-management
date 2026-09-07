using Microsoft.AspNetCore.Identity;

namespace SupermarketStockManagement.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Define the roles used in the system
            string[] roles =
                {
                 "Admin",
                 "Manager",
                 "Staff"
                };

            // Create each role if it does not already exist
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(roleName));
                }
            }

            // Define the default administrator account
            const string adminEmail = "admin@stockflow.co.nz";
            const string adminPassword = "Admin123!";

            var adminUser =
                await userManager.FindByEmailAsync(adminEmail);

            // Create the administrator account if it does not exist
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult =
                    await userManager.CreateAsync(
                        adminUser,
                        adminPassword);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        createResult.Errors.Select(error =>
                            error.Description));

                    throw new Exception(
                        $"Cannot create the administrator account: {errors}");
                }
            }

            // Assign the Admin role to the administrator account
            if (!await userManager.IsInRoleAsync(
                    adminUser,
                    "Admin"))
            {
                await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin");
            }
        }
    }
}