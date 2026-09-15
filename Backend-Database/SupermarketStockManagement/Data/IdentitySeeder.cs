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

            // Default accounts used by the system. Each account is created
            // only if missing, so existing accounts (and their passwords)
            // are never overwritten.
            var accounts = new[]
            {
                new { Email = "admin@stockflow.co.nz", Password = "Admin123!", Role = "Admin" },
                new { Email = "ngthanh123426@gmail.com", Password = "Admin123!", Role = "Admin" },
                new { Email = "manager@stockflow.com", Password = "Manager@123", Role = "Manager" },
                new { Email = "staff@stockflow.com", Password = "Staff@123", Role = "Staff" },
            };

            foreach (var account in accounts)
            {
                var user =
                    await userManager.FindByEmailAsync(account.Email);

                // Create the account if it does not exist
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = account.Email,
                        Email = account.Email,
                        EmailConfirmed = true
                    };

                    var createResult =
                        await userManager.CreateAsync(
                            user,
                            account.Password);

                    if (!createResult.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            createResult.Errors.Select(error =>
                                error.Description));

                        throw new Exception(
                            $"Cannot create the account {account.Email}: {errors}");
                    }
                }

                // Assign the matching role to the account
                if (!await userManager.IsInRoleAsync(user, account.Role))
                {
                    await userManager.AddToRoleAsync(user, account.Role);
                }
            }
        }
    }
}