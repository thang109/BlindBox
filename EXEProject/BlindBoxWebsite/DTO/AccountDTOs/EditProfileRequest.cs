using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.DTO.AccountDTOs
{
    public class EditProfileRequest
    {
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters.")]
        [RegularExpression(@"^(?!\s*$)(?!^\s+|\s+$)[a-zA-Z\s]+$", ErrorMessage = "Full name cannot contain leading or trailing spaces or any numbers.")]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        [RegularExpression(@"^(?!\s*$)(?!^\s+|\s+$).*", ErrorMessage = "Address cannot start or end with whitespace.")]
        [Display(Name = "Address")] 
        public string? Address { get; set; }

        [StringLength(15, ErrorMessage = "Phone number cannot be longer than 15 characters.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must contain 10 numbers.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Phone Number")] 
        public string? PhoneNumber { get; set; }
    }
}
