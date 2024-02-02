using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var students = await _context.Students.Include(s => s.Grades).ToListAsync();
            return View("Scores", students);
        }
    }
}
