using System.ComponentModel.DataAnnotations;

namespace BlindBoxWebsite.ViewModels
{
    public class VnPayRequestModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thành phố.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập quận/huyện.")]
        public string District { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression(@"\d{10,11}", ErrorMessage = "Số điện thoại phải là 10-11 chữ số.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tiền.")]
        [Range(1, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả đơn hàng.")]
        public string OrderDescription { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập loại đơn hàng.")]
        public string OrderType { get; set; }

        public int BlindBoxId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
    }
}
