using Microsoft.AspNetCore.Mvc;

namespace ST10442835_PROG6212_CMCS.Controllers
{
    public class DashboardController : Controller
    {
        // Manager dashboard
        public IActionResult ManagementIndex()
        {
            return View(); // Views/Dashboard/ManagementIndex.cshtml
        }

        // Lecturer dashboard
        public IActionResult LecturerIndex()
        {
            return View(); // Views/Dashboard/LecturerIndex.cshtml
        }
    }
}