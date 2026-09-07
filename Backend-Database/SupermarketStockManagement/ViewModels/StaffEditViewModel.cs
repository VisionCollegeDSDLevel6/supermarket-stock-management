using System.ComponentModel.DataAnnotations;

namespace SupermarketStockManagement.ViewModels
{
    public class StaffEditViewModel
    {
        public int StaffId { get; set; }

        public string? IdentityUserId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            "^(Manager|Staff)$",
            ErrorMessage = "Please select a valid role.")]
        public string Role { get; set; } = "Staff";

        [DataType(DataType.Password)]
        [StringLength(
            100,
            MinimumLength = 6,
            ErrorMessage = "The password must contain at least 6 characters.")]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare(
            nameof(NewPassword),
            ErrorMessage = "The passwords do not match.")]
        public string? ConfirmNewPassword { get; set; }
    }
}