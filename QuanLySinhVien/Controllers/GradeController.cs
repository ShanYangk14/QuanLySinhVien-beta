using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using QuanLySinhVien.Data;
using QuanLySinhVien.Models;
using System.Threading.Tasks;


namespace QuanLySinhVien.Controllers
{
    public class GradeController : Controller
    {
        private readonly SchoolDbContext _context;
        public GradeController(SchoolDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ScoreReview()
        {
            var students = await _context.Students
				.Include(s => s.Teacher)  
		        .Include(s => s.Reviews)
		        .ToListAsync();

			Console.WriteLine($"Access students Score Review");
            return View(students);
        }
        public async Task<IActionResult> StudentAssessment() 
        {
            var teacher = await _context.Teachers
				 .Include(t => t.Students)  
		         .Include(t => t.Reviews)
		         .ToListAsync();

			Console.WriteLine($"Access of students assessment");
            return View(teacher);
        }
    }
}
