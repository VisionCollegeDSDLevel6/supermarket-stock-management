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

            // Default administrator accounts. Add new administrator emails here.
            var adminAccounts = new[]
            {
                new { Email = "admin@stockflow.co.nz", Password = "Admin123!" },
                new { Email = "ngthanh123426@gmail.com", Password = "Admin123!" },
            };

            foreach (var account in adminAccounts)
            {
                var adminUser =
                    await userManager.FindByEmailAsync(account.Email);

                // Create the administrator account if it does not exist
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = account.Email,
                        Email = account.Email,
                        EmailConfirmed = true
                    };

                    var createResult =
                        await userManager.CreateAsync(
                            adminUser,
                            account.Password);

                    if (!createResult.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            createResult.Errors.Select(error =>
                                error.Description));

                        throw new Exception(
                            $"Cannot create the administrator account {account.Email}: {errors}");
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
}