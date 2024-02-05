using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Scores()
        {
            var students = await _context.Students
                .Include(s => s.Individual)
                .Include(s => s.Team)
                .Include(s => s.TeacherEvaluation)
                .Include(s => s.Grades) 
                .ToListAsync();

            Console.WriteLine($"Number of students: {students.Count}");

            return View("Scores", students);
        }
    }
}
