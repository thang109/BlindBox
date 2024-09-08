using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.DTO.AccountDTOs
{
    public class ConfirmEmailRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
