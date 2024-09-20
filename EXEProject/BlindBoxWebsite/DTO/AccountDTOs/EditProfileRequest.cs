using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.DTO.AccountDTOs
{
    public class EditProfileRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [RegularExpression(@"^\S.*\S$|^\S$", ErrorMessage = "Username cannot start or end with whitespace.")]
        public string? FullName { get; set; }

        [RegularExpression(@"^\S.*\S$|^\S$", ErrorMessage = "Username cannot start or end with whitespace.")]
        public string? Address { get; set; }

        [RegularExpression(@"^\S.*\S$|^\S$", ErrorMessage = "Username cannot start or end with whitespace.")]
        public string? PhoneNumber { get; set; }
    }
}
