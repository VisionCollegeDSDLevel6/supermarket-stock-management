using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketStockManagement.Data;
using SupermarketStockManagement.Models;
using SupermarketStockManagement.ViewModels;

[Authorize(Roles = "Admin,Manager")]
public class StaffsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public StaffsController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Staffs
    public async Task<IActionResult> Index()
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

        return View(staffList);
    }
    // GET: Staffs/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var staff = await _context.Staff
            .FirstOrDefaultAsync(staff =>
                staff.StaffId == id);

        if (staff == null)
        {
            return NotFound();
        }

        // Managers cannot view another Manager account
        if (User.IsInRole("Manager") &&
            staff.Role == "Manager")
        {
            return Forbid();
        }

        return View(staff);
    }

    // GET: Staffs/Create
    public IActionResult Create()
    {
        return View(new StaffCreateViewModel());
    }

    // POST: Staffs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        StaffCreateViewModel model)
    {
        // Managers can only create Staff accounts
        if (User.IsInRole("Manager") &&
            model.Role != "Staff")
        {
            ModelState.AddModelError(
                nameof(model.Role),
                "Managers can only create Staff accounts.");
        }

        var existingStaff = await _context.Staff
            .AnyAsync(staff => staff.Email == model.Email);

        if (existingStaff)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "A staff account with this email already exists.");
        }

        var existingIdentityUser =
            await _userManager.FindByEmailAsync(model.Email);

        if (existingIdentityUser != null)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "A login account with this email already exists.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var identityUser = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var createUserResult =
                await _userManager.CreateAsync(
                    identityUser,
                    model.Password);

            if (!createUserResult.Succeeded)
            {
                AddIdentityErrors(createUserResult);

                await transaction.RollbackAsync();

                return View(model);
            }

            var addRoleResult =
                await _userManager.AddToRoleAsync(
                    identityUser,
                    model.Role);

            if (!addRoleResult.Succeeded)
            {
                AddIdentityErrors(addRoleResult);

                await transaction.RollbackAsync();

                return View(model);
            }

            var staff = new Staff
            {
                IdentityUserId = identityUser.Id,
                Name = model.Name,
                Email = model.Email,
                Role = model.Role
            };

            _context.Staff.Add(staff);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["SuccessMessage"] =
                "The staff account was created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            ModelState.AddModelError(
                string.Empty,
                "The staff account could not be created.");

            return View(model);
        }
    }

    // GET: Staffs/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var staff = await _context.Staff.FindAsync(id);

        if (staff == null)
        {
            return NotFound();
        }

        // Managers can only edit Staff accounts
        if (User.IsInRole("Manager") &&
            staff.Role != "Staff")
        {
            return Forbid();
        }

        var model = new StaffEditViewModel
        {
            StaffId = staff.StaffId,
            IdentityUserId = staff.IdentityUserId,
            Name = staff.Name,
            Email = staff.Email,
            Role = staff.Role
        };

        return View(model);
    }

    // POST: Staffs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        StaffEditViewModel model)
    {
        if (id != model.StaffId)
        {
            return NotFound();
        }

        var staff = await _context.Staff.FindAsync(id);

        if (staff == null)
        {
            return NotFound();
        }

        // Managers can only edit Staff accounts
        if (User.IsInRole("Manager") &&
            (staff.Role != "Staff" ||
             model.Role != "Staff"))
        {
            return Forbid();
        }

        var identityUser =
            await FindIdentityUserAsync(staff);

        // Existing Staff rows may not have a linked Identity account
        if (identityUser == null &&
            string.IsNullOrWhiteSpace(model.NewPassword))
        {
            ModelState.AddModelError(
                nameof(model.NewPassword),
                "Enter a password to create a login account for this staff member.");
        }

        var duplicateStaffEmail = await _context.Staff
            .AnyAsync(existingStaff =>
                existingStaff.Email == model.Email &&
                existingStaff.StaffId != model.StaffId);

        if (duplicateStaffEmail)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Another staff account already uses this email.");
        }

        var userWithEmail =
            await _userManager.FindByEmailAsync(model.Email);

        if (userWithEmail != null &&
            userWithEmail.Id != identityUser?.Id)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Another login account already uses this email.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            if (identityUser == null)
            {
                identityUser = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var createUserResult =
                    await _userManager.CreateAsync(
                        identityUser,
                        model.NewPassword!);

                if (!createUserResult.Succeeded)
                {
                    AddIdentityErrors(createUserResult);

                    await transaction.RollbackAsync();

                    return View(model);
                }

                var addRoleResult =
                    await _userManager.AddToRoleAsync(
                        identityUser,
                        model.Role);

                if (!addRoleResult.Succeeded)
                {
                    AddIdentityErrors(addRoleResult);

                    await transaction.RollbackAsync();

                    return View(model);
                }

                staff.IdentityUserId = identityUser.Id;
            }
            else
            {
                var emailResult =
                    await _userManager.SetEmailAsync(
                        identityUser,
                        model.Email);

                if (!emailResult.Succeeded)
                {
                    AddIdentityErrors(emailResult);

                    await transaction.RollbackAsync();

                    return View(model);
                }

                var userNameResult =
                    await _userManager.SetUserNameAsync(
                        identityUser,
                        model.Email);

                if (!userNameResult.Succeeded)
                {
                    AddIdentityErrors(userNameResult);

                    await transaction.RollbackAsync();

                    return View(model);
                }

                var currentRoles =
                    await _userManager.GetRolesAsync(
                        identityUser);

                if (!currentRoles.Contains(model.Role))
                {
                    if (currentRoles.Count > 0)
                    {
                        var removeRolesResult =
                            await _userManager.RemoveFromRolesAsync(
                                identityUser,
                                currentRoles);

                        if (!removeRolesResult.Succeeded)
                        {
                            AddIdentityErrors(removeRolesResult);

                            await transaction.RollbackAsync();

                            return View(model);
                        }
                    }

                    var addRoleResult =
                        await _userManager.AddToRoleAsync(
                            identityUser,
                            model.Role);

                    if (!addRoleResult.Succeeded)
                    {
                        AddIdentityErrors(addRoleResult);

                        await transaction.RollbackAsync();

                        return View(model);
                    }
                }

                if (!string.IsNullOrWhiteSpace(
                        model.NewPassword))
                {
                    var passwordToken =
                        await _userManager
                            .GeneratePasswordResetTokenAsync(
                                identityUser);

                    var passwordResult =
                        await _userManager.ResetPasswordAsync(
                            identityUser,
                            passwordToken,
                            model.NewPassword);

                    if (!passwordResult.Succeeded)
                    {
                        AddIdentityErrors(passwordResult);

                        await transaction.RollbackAsync();

                        return View(model);
                    }
                }
            }

            staff.Name = model.Name;
            staff.Email = model.Email;
            staff.Role = model.Role;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["SuccessMessage"] =
                "The staff account was updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync();

            if (!StaffExists(model.StaffId))
            {
                return NotFound();
            }

            throw;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            ModelState.AddModelError(
                string.Empty,
                "The staff account could not be updated.");

            return View(model);
        }
    }

    // GET: Staffs/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var staff = await _context.Staff
            .FirstOrDefaultAsync(staff =>
                staff.StaffId == id);

        if (staff == null)
        {
            return NotFound();
        }

        // Managers can only delete Staff accounts
        if (User.IsInRole("Manager") &&
            staff.Role != "Staff")
        {
            return Forbid();
        }

        return View(staff);
    }

    // POST: Staffs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var staff = await _context.Staff.FindAsync(id);

        if (staff == null)
        {
            return RedirectToAction(nameof(Index));
        }

        // Managers can only delete Staff accounts
        if (User.IsInRole("Manager") &&
            staff.Role != "Staff")
        {
            return Forbid();
        }

        var identityUser =
            await FindIdentityUserAsync(staff);

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            if (identityUser != null)
            {
                var deleteUserResult =
                    await _userManager.DeleteAsync(
                        identityUser);

                if (!deleteUserResult.Succeeded)
                {
                    TempData["ErrorMessage"] =
                        "The login account could not be deleted.";

                    await transaction.RollbackAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            _context.Staff.Remove(staff);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["SuccessMessage"] =
                "The staff account was deleted successfully.";
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            TempData["ErrorMessage"] =
                "The staff account could not be deleted.";
        }

        return RedirectToAction(nameof(Index));
    }

    // Finds the Identity account linked to a Staff record
    private async Task<IdentityUser?> FindIdentityUserAsync(
        Staff staff)
    {
        if (!string.IsNullOrWhiteSpace(
                staff.IdentityUserId))
        {
            var userById =
                await _userManager.FindByIdAsync(
                    staff.IdentityUserId);

            if (userById != null)
            {
                return userById;
            }
        }

        return await _userManager.FindByEmailAsync(
            staff.Email);
    }

    // Adds Identity validation errors to ModelState
    private void AddIdentityErrors(
        IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(
                string.Empty,
                error.Description);
        }
    }

    private bool StaffExists(int id)
    {
        return _context.Staff.Any(staff =>
            staff.StaffId == id);
    }
}