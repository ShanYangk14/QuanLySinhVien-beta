using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLySinhVien.Data;
using QuanLySinhVien.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace QuanLySinhVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchoolDbContext _db;
        public HomeController(ILogger<HomeController> logger, SchoolDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "StudentPolicy")]
        public IActionResult Student()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    _logger.LogInformation("User is authenticated. Access granted to Student page. User: {user}", User.Identity.Name);
                    return View();
                }
                else
                {
                    _logger.LogWarning("Access to Student page denied. User not authenticated.");
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Student action.");
                return RedirectToAction("Error");
            }
        }
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.FullName = HttpContext.Session.GetString("FullName");
                ViewBag.Email = HttpContext.Session.GetString("Email");
                return View();
            }
            else
            {
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            {
                var check = _db.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _db.Users.Add(_user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = GetMD5(password);
                var user = _db.Users.FirstOrDefault(s => s.Email == email && s.Password == hashedPassword);

                if (user != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction(nameof(Student));
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction(nameof(Login));
                }
            }

            return View();
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
         
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
