using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLySinhVien.Models
{
    public class Role : IdentityRole<int>
    {
        [Key]
        public int Id { get; set; }
        public string rolename { get; set; }
        public User User { get; set; }
    }
    
}
