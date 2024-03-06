using Humanizer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.DependencyResolver;
using NuGet.Protocol;
using QuanLySinhVien.Data;
using QuanLySinhVien.Models;
using System.Diagnostics;
using System.Security.Claims;
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

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "StudentPolicy")]
        public async Task<IActionResult> ScoreReview()
        {
            var student = await _context.Students
                    .Include(s => s.Reviews)
                    .Include(r => r.Teacher)
                    .ToListAsync();


            Console.WriteLine($"Access of students assessment");
            return View(student);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "TeacherPolicy")]
        public async Task<IActionResult> StudentAssessment()
        {
            var teacher = await _context.Teachers
                 .Include(t => t.Students)
                 .Include(t => t.Reviews)
                 .ToListAsync();

            Console.WriteLine($"Access of students assessment");
            return View(teacher);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "TeacherPolicy")]
        public async Task<IActionResult> EnterScore(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.MSSV == studentId);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "TeacherPolicy")]
        public async Task<IActionResult> EnterScore(int studentId, int score)
        {
            var student = await _context.Students
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.MSSV == studentId);

            if (student == null)
            {
                return NotFound();
            }

            var review = student.Reviews.FirstOrDefault();

            if (review != null)
            {
                var teacher = review.Teacher;

                if (teacher != null)
                {
                    teacher.Score = score;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("StudentAssessment");
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        public async Task<IActionResult> ManageStudentScores()
        {
            var students = await _context.Students
                .Include(s => s.Reviews)
                .ToListAsync();

            return View("ManageStudentScores", students);
        }
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        public async Task<IActionResult> EditScore(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.MSSV == studentId);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditScore(int studentId, int score)
        {
            var student = await _context.Students
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.MSSV == studentId);

            if (student == null)
            {
                return NotFound();
            }

            if (student.Reviews != null)
            {
                var review = student.Reviews.FirstOrDefault();
                if (review != null)
                {
                    var teacher = review.Teacher;

                    if (teacher != null)
                    {
                        teacher.Score = score;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction("ManageStudentScores");
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteScore(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.MSSV == studentId);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        [HttpPost, ActionName("DeleteScore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteScore(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync(s => s.MSSV == studentId);

            if (student == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(student.Reviews.FirstOrDefault());
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageStudentScores");
        }

    }
}
