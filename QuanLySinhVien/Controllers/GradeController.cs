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
                .Include(s => s.MSSV)
                .Include(s => s.Reviews)
                .Include(s => s.GvDanhGia)
                .ToListAsync();

            Console.WriteLine($"Access students Score Review");
            return View();
        }
        public async Task<IActionResult> StudentAssessment() 
        {
            var teacher = await _context.Teachers
                .Include(t => t.MSGV)
                .Include(t => t.Reviews)
                .Include(t => t.GvDanhGia)
                .ToListAsync();

            Console.WriteLine($"Access of students assessment");
            return View();
        }
    }
}
