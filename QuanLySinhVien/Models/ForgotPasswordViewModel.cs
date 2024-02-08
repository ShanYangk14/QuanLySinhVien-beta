using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
