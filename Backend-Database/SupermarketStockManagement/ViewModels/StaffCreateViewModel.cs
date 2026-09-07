using System.ComponentModel.DataAnnotations;

namespace SupermarketStockManagement.ViewModels
{
    public class StaffCreateViewModel
    {
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

        [Required]
        [DataType(DataType.Password)]
        [StringLength(
            100,
            MinimumLength = 6,
            ErrorMessage = "The password must contain at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(
            nameof(Password),
            ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}