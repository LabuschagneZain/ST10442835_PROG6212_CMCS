using Microsoft.AspNetCore.Mvc;
using ST10442835_PROG6212_CMCS.Models;
using ST10442835_PROG6212_CMCS.Services;

namespace ST10442835_PROG6212_CMCS.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly IClaimService _claimService;
        private readonly IFileStorageService _fileStorageService;

        public ClaimsController(IClaimService claimService, IFileStorageService fileStorageService)
        {
            _claimService = claimService;
            _fileStorageService = fileStorageService;
        }

        // POST: Claims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Month, int HoursWorked, double HourlyRate, string Notes, IFormFile DocumentFile)
        {
            try
            {
                // Manual validation
                if (string.IsNullOrEmpty(Month) || HoursWorked <= 0 || HourlyRate <= 0)
                {
                    TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
                    return RedirectToAction("LecturerIndex", "Dashboard");
                }

                string fileUrl = string.Empty;
                string fileName = string.Empty;

                if (DocumentFile != null && DocumentFile.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                    var fileExtension = Path.GetExtension(DocumentFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["ErrorMessage"] = "Only PDF, DOCX, and XLSX files are allowed.";
                        return RedirectToAction("LecturerIndex", "Dashboard");
                    }

                    // Validate file size
                    if (DocumentFile.Length > 5 * 1024 * 1024)
                    {
                        TempData["ErrorMessage"] = "File size cannot exceed 5MB.";
                        return RedirectToAction("LecturerIndex", "Dashboard");
                    }

                    try
                    {
                        fileName = await _fileStorageService.UploadFileAsync(DocumentFile);
                        fileUrl = fileName;
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = $"Error uploading file: {ex.Message}";
                        return RedirectToAction("LecturerIndex", "Dashboard");
                    }
                }

                // Create a new Claim object for storage
                var storageClaim = new Claim
                {
                    Id = Guid.NewGuid().ToString(),
                    LecturerName = HttpContext.Session.GetString("Username") ?? "Unknown Lecturer",
                    Month = Month,
                    HoursWorked = HoursWorked,
                    HourlyRate = HourlyRate,
                    Status = "Pending",
                    DocumentUrl = fileUrl,
                    DocumentFileName = DocumentFile?.FileName ?? string.Empty,
                    Notes = Notes,
                    CreatedDate = DateTime.UtcNow
                };

                await _claimService.AddClaimAsync(storageClaim);

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

            var claim = await _claimService.GetClaimByIdAsync(id);

            if (claim == null || string.IsNullOrEmpty(claim.DocumentUrl))
                return NotFound();

            try
            {
                var fileBytes = await _fileStorageService.DownloadFileAsync(claim.DocumentUrl);
                var contentType = GetContentType(claim.DocumentFileName);
                return File(fileBytes, contentType, claim.DocumentFileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error downloading file: {ex.Message}";
                return RedirectToAction("LecturerIndex", "Dashboard");
            }
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid claim ID.";
                return RedirectToAction("ManagementIndex", "Dashboard");
            }

            try
            {
                await _claimService.UpdateClaimStatusAsync(id, "Approved");
                TempData["SuccessMessage"] = "Claim approved successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error approving claim: {ex.Message}";
            }

            return RedirectToAction("ManagementIndex", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid claim ID.";
                return RedirectToAction("ManagementIndex", "Dashboard");
            }

            try
            {
                await _claimService.UpdateClaimStatusAsync(id, "Rejected");
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