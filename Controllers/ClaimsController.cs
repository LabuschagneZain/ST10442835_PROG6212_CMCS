using Microsoft.AspNetCore.Mvc;
using ST10442835_PROG6212_CMCS.Models;
using ST10442835_PROG6212_CMCS.Services;

namespace ST10442835_PROG6212_CMCS.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly TableStorageService _tableStorageService;
        private readonly BlobService _blobService;

        public ClaimsController(TableStorageService tableStorageService, BlobService blobService)
        {
            _tableStorageService = tableStorageService ?? throw new ArgumentNullException(nameof(tableStorageService));
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
        }

        // GET: Lecturer Dashboard (fetch latest claims)
        public async Task<IActionResult> LecturerIndex()
        {
            var lecturerName = User.Identity?.Name ?? "Unknown Lecturer";
            var claims = await _tableStorageService.GetClaimsByLecturerAsync(lecturerName);
            return View(claims); // pass claims to Razor view
        }

        // POST: Claims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Claim claim)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Failed to submit claim. Please check your input.";
                return RedirectToAction("LecturerIndex", "Dashboard");
            }

            try
            {
                string fileUrl = string.Empty;
                if (claim.DocumentFile != null && claim.DocumentFile.Length > 0)
                {
                    fileUrl = await _blobService.UploadFileAsync(claim.DocumentFile);
                }

                // Create a new Claim object for Table Storage, excluding IFormFile
                var storageClaim = new Claim
                {
                    RowKey = Guid.NewGuid().ToString(),
                    LecturerName = User.Identity?.Name ?? "Unknown Lecturer",
                    Month = claim.Month,
                    HoursWorked = claim.HoursWorked,
                    HourlyRate = claim.HourlyRate,
                    Status = "Pending",
                    DocumentUrl = fileUrl,
                    CreatedDate = DateTime.UtcNow
                };

                await _tableStorageService.AddClaimAsync(storageClaim);

                TempData["SuccessMessage"] = "Claim submitted successfully!";
                return RedirectToAction("LecturerIndex", "Dashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error submitting claim: {ex.Message}";
                return RedirectToAction("LecturerIndex", "Dashboard");
            }
        }

        // GET: Claims/ViewDocument/5
        public async Task<IActionResult> ViewDocument(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Invalid claim ID.");

            var claims = await _tableStorageService.GetAllClaimsAsync();
            var claim = claims.FirstOrDefault(c => c.Id == id);

            if (claim == null || string.IsNullOrEmpty(claim.DocumentUrl))
                return NotFound();

            return Redirect(claim.DocumentUrl);
        }

        // Management actions - only for managers
        [HttpPost]
        public async Task<IActionResult> Approve(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid claim ID.";
                return RedirectToAction("ManagementIndex", "Dashboard");
            }

            try
            {
                await _tableStorageService.UpdateClaimStatusAsync(id, "Approved");
                TempData["SuccessMessage"] = "Claim approved successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error approving claim: {ex.Message}";
            }

            return RedirectToAction("ManagementIndex", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid claim ID.";
                return RedirectToAction("ManagementIndex", "Dashboard");
            }

            try
            {
                await _tableStorageService.UpdateClaimStatusAsync(id, "Rejected");
                TempData["SuccessMessage"] = "Claim rejected successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error rejecting claim: {ex.Message}";
            }

            return RedirectToAction("ManagementIndex", "Dashboard");
        }
    }
}
