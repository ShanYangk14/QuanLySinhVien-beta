using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace QuanLySinhVien.Models
{
	public class User : IdentityUser
	{
		[Key, Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int idUser { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string FirstName { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string LastName { get; set; }

		[Required]
		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
		public new string Email { get; set; }

		[Required]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
		public string Password { get; set; }

		[NotMapped]
		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
		public string FullName()
		{
			return this.FirstName + " " + this.LastName;
		}
		public string ResetToken { get; set; }
		public DateTime? ResetTokenExpiration { get; set; }
		public bool IsAdmin { get; set; }
		public bool IsTeacher { get; set; }
		public string EmailConfirmationToken { get; set; }
	}
}