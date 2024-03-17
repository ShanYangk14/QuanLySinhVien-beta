using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Data;
using QuanLySinhVien.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QuanLySinhVien.Controllers
{
    public class GradeController : Controller
    {
        private readonly SchoolDbContext _context;
        private readonly UserManager<User> _userManager;

        public GradeController(SchoolDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Action for teachers to view grades of students in their class
        [HttpGet]
        public async Task<IActionResult> ViewGradesForTeacher()
        {
            var currentUser = await _context.Users
                .Include(u => u.Teacher)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (currentUser == null || currentUser.Teacher == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var teacherClass = await _context.Classes
                .Include(c => c.Students)
                .ThenInclude(s => s.Grades)
                .FirstOrDefaultAsync(c => c.TeacherId == currentUser.Teacher.Id);

            return View(teacherClass);
        }

        [HttpPost]
        public async Task<IActionResult> PostGradeForStudent(int studentId, Grades grade)
        {
            if (!ModelState.IsValid)
            {
                return View(grade);
            }

            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            grade.StudentId = studentId;
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewOwnGrades");
        }

        // Action for students to view their own grades
        [HttpGet]
        public async Task<IActionResult> ViewOwnGrades()
        {
            // Get the currently logged-in user
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || currentUser.Student == null)
            {
                // Handle the case where the user is not a student
                return RedirectToAction("AccessDenied", "Home");
            }

            // Get the student's grades
            var studentGrades = await _context.Grades
                .Where(g => g.StudentId == currentUser.Student.Id)
                .ToListAsync();

            return View(studentGrades);
        }
    }
}
