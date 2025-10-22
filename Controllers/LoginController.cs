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
            if (string.IsNullOrWhiteSpace(username))
            {
                ViewBag.ErrorMessage = "Username cannot be empty.";
                return View();
            }

            if (username.Equals("lecturer", StringComparison.OrdinalIgnoreCase) ||
                username.Equals("management", StringComparison.OrdinalIgnoreCase) ||
                username.Equals("coordinator", StringComparison.OrdinalIgnoreCase))
            {

                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("Role", role ?? "lecturer");

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

            ViewBag.ErrorMessage = "Invalid username. Try 'lecturer', 'coordinator', or 'management'. Password can be anything.";
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