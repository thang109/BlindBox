using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.ViewModels
{
    public class VnPayRequestModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters.")]
        [RegularExpression(@"^\S.*\S$|^\S$", ErrorMessage = "Full name cannot start or end with whitespace.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]
        [RegularExpression(@"^\S.*\S$|^\S$", ErrorMessage = "City cannot start or end with whitespace.")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "District is required.")]
        [StringLength(50, ErrorMessage = "District cannot be longer than 50 characters.")]
        [RegularExpression(@"^\S.*\S$|^\S$", ErrorMessage = "District cannot start or end with whitespace.")]
        [Display(Name = "District")]
        public string District { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        [RegularExpression(@"^\S.*\S$|^\S$", ErrorMessage = "Address cannot start or end with whitespace.")]
        [Display(Name = "Address")] 
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot be longer than 15 characters.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain only digits and 10 numbers.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        public double Amount { get; set; }

        public string OrderDescription { get; set; }

        public string OrderType { get; set; }

        public int BlindBoxId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
