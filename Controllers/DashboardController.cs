// Controllers/DashboardController.cs
using Microsoft.AspNetCore.Mvc;
using ST10442835_PROG6212_CMCS.Services;

namespace ST10442835_PROG6212_CMCS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly TableStorageService _tableStorageService;

        public DashboardController(TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        // Manager dashboard
        public async Task<IActionResult> ManagementIndex()
        {
            var allClaims = await _tableStorageService.GetAllClaimsAsync();
            return View(allClaims);
        }

        // Lecturer dashboard
        public async Task<IActionResult> LecturerIndex()
        {
            var lecturerName = User.Identity?.Name ?? "Unknown Lecturer";
            var lecturerClaims = await _tableStorageService.GetClaimsByLecturerAsync(lecturerName);
            return View(lecturerClaims);
        }
    }
}