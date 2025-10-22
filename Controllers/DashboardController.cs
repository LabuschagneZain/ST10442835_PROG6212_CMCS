using Microsoft.AspNetCore.Mvc;
using ST10442835_PROG6212_CMCS.Services;
using ST10442835_PROG6212_CMCS.Models;

namespace ST10442835_PROG6212_CMCS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IClaimService _claimService;

        public DashboardController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        public async Task<IActionResult> ManagementIndex()
        {
            try
            {
                var allClaims = await _claimService.GetAllClaimsAsync();
                return View(allClaims);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading claims: {ex.Message}";
                return View(new List<Claim>());
            }
        }

        public async Task<IActionResult> LecturerIndex()
        {
            try
            {
                var lecturerName = User.Identity?.Name ??
                                 HttpContext.Session.GetString("Username") ??
                                 "Unknown Lecturer";

                var lecturerClaims = await _claimService.GetClaimsByLecturerAsync(lecturerName);
                return View(lecturerClaims);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading your claims: {ex.Message}";
                return View(new List<Claim>());
            }
        }

        public async Task<IActionResult> CoordinatorIndex()
        {
            try
            {
                var allClaims = await _claimService.GetAllClaimsAsync();
                var pendingClaims = allClaims.Where(c => c.Status == "Pending").ToList();
                return View(pendingClaims);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading claims: {ex.Message}";
                return View(new List<Claim>());
            }
        }
    }
}