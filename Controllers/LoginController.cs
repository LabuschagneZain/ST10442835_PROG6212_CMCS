// Controllers/LoginController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ST10442835_PROG6212_CMCS.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Username and password cannot be empty.";
                return View();
            }

            // Simple authentication - in production, use proper authentication
            if ((username.Equals("lecturer", StringComparison.OrdinalIgnoreCase) && password == "password") ||
                (username.Equals("management", StringComparison.OrdinalIgnoreCase) && password == "password") ||
                (username.Equals("coordinator", StringComparison.OrdinalIgnoreCase) && password == "password"))
            {
                // Store username in session
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Role", role ?? "lecturer");

                // Redirect based on role
                if (username.Equals("lecturer", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("LecturerIndex", "Dashboard");
                }
                else if (username.Equals("management", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("ManagementIndex", "Dashboard");
                }
                else if (username.Equals("coordinator", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("CoordinatorIndex", "Dashboard");
                }
            }

            ViewBag.ErrorMessage = "Invalid username or password. Try 'lecturer', 'coordinator', or 'management' with password 'password'.";
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}