using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Student_Record_Management_System.ViewModels;
using Student_Record_Management_System.Service;

namespace Student_Record_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IStudentService _userService;
        public AccountController(IStudentService userService) => _userService = userService;

        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                TempData["LoginError"] = "Invalid username or password. Try Again.";
                return View(model);
            }

            var claims = new List<Claim>
    {
        new Claim("UserId", user.UserId.ToString()),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim(ClaimTypes.Name, user.Username)
    };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)).Wait();

            return user.Role == "Invigilator"
                ? RedirectToAction("Dashboard", "Invigilator")
                : RedirectToAction("MyRecord", "Student");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
