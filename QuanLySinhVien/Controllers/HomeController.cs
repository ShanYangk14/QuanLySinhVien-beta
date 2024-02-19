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
using QuanLySinhVien.Attributes;

namespace QuanLySinhVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchoolDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public HomeController(ILogger<HomeController> logger, SchoolDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
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
                var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
                if (roles.Contains("Student"))
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
                return View("Index");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterAsync(User _user)
        {
            try
            {
                var check = _db.Users.FirstOrDefault(s => s.Email == _user.Email);

                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);


                    _user.ResetToken = string.Empty;
                    _user.ResetTokenExpiration = DateTime.UtcNow;

                    _db.Users.Add(_user);
                    _db.SaveChanges();

                    var user = new User { FirstName = _user.FirstName, LastName = _user.LastName, Email = _user.Email };
                    var result = await _userManager.CreateAsync(user, _user.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            catch (Exception ex)
            {

                ViewBag.error = "An error occurred during registration.";
                return View();
            }
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

                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction(nameof(Admin));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Student"));
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction(nameof(Student));
                    }
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction(nameof(Login));
                }
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminLogin(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null && (await _userManager.IsInRoleAsync(user, "Admin") || user.IsAdmin))
                {
                    var result = await _signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Admin));
                    }
                    else
                    {
                        ViewBag.error = "Admin login failed";
                        return View(nameof(AccessDenied));
                    }
                }
                else
                {
                    ViewBag.error = "Admin login failed";
                    return View(nameof(Login));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the AdminLogin action.");
                return RedirectToAction("Error");
            }
        }



        public IActionResult AdminLogin()
        {
            return View();
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
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
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {

                user.ResetToken = Guid.NewGuid().ToString();
                user.ResetTokenExpiration = DateTime.UtcNow.AddHours(1);

                _db.SaveChanges();

                return RedirectToAction("ResetPassword", new { token = user.ResetToken });
            }


            ViewBag.Error = "Email address not found.";
            return View("ForgotPassword");
        }

        public IActionResult ResetPassword(string token)
        {
            var user = _db.Users.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiration > DateTime.UtcNow);

            if (user != null)
            {
                return View(new ResetPasswordViewModel { Token = token });
            }

            return RedirectToAction("InvalidToken");
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var user = _db.Users.FirstOrDefault(u => u.ResetToken == model.Token && u.ResetTokenExpiration > DateTime.UtcNow);

            if (user != null)
            {

                user.Password = GetMD5(model.NewPassword);
                user.ResetToken = string.Empty;
                user.ResetTokenExpiration = DateTime.UtcNow;

                _db.SaveChanges();

                return RedirectToAction("Login");
            }

            return RedirectToAction("InvalidToken");
        }

        public async Task<IActionResult> ManageUsers()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(currentUser, "Admin"))
            {
                var users = _db.Users.ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("AccessDenied");
            }

        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "AdminPolicy")]
        public IActionResult Admin()
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            Console.WriteLine("User Roles: " + string.Join(", ", roles));

            if (User.Identity.IsAuthenticated)
            {
                Console.WriteLine("User is authenticated.");
            }
            else
            {
                Console.WriteLine("User is NOT authenticated.");
            }
            Console.WriteLine("Reached Admin action.");
            return View();
        }


        public IActionResult DisplayRoles()
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            return Content("Roles: " + string.Join(", ", roles));
        }

        public IActionResult AccessDenied()
        {
            if (User.Identity.IsAuthenticated)
            {
               
                return View("AccessDenied");
            }
            else
            {
                
                return RedirectToAction("Login");
            }
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