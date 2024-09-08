using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.DTO.AccountDTOs
{
    public class EditProfileRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [RegularExpression(@"^\S.*\S$|^\S$", ErrorMessage = "Username cannot start or end with whitespace.")]
        public string? UserName { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
