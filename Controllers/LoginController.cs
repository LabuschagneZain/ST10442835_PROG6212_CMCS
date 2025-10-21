using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

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
        public IActionResult Index(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Username and password cannot be empty.";
                return View();
            }

            if (username.Equals("lecturer", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("LecturerIndex", "Dashboard");
            }
            else if (username.Equals("management", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("ManagementIndex", "Dashboard");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username. Try 'lecturer' or 'management'.";
                return View();
            }
        }
    }
}