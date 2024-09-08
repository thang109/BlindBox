using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.DTO.AccountDTOs
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
