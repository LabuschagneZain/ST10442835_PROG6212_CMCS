using Microsoft.AspNetCore.Mvc;

namespace ST10442835_PROG6212_CMCS.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string username, string password)
        {
            // Temporary: hardcoded role logic
            if (username == "lecturer" && password == "123")
            {
                return RedirectToAction("LecturerIndex", "Dashboard");
            }
            else if (username == "manager" && password == "123")
            {
                return RedirectToAction("ManagementIndex", "Dashboard");
            }
            else
            {
                ViewBag.Error = "Invalid username or password";
                return View("Index");
            }
        }

    }
}
