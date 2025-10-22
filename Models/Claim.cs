using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ST10442835_PROG6212_CMCS.Models
{
    public class Claim
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Lecturer name is required")]
        public string LecturerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Month is required")]
        public string Month { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hours worked is required")]
        [Range(1, 744, ErrorMessage = "Hours worked must be between 1 and 744")]
        public int HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly rate is required")]
        [Range(1, 1000, ErrorMessage = "Hourly rate must be between 1 and 1000")]
        public double HourlyRate { get; set; }

        public string Status { get; set; } = "Pending";
        public string DocumentUrl { get; set; } = string.Empty;
        public string DocumentFileName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }

        public IFormFile? DocumentFile { get; set; }

        public double TotalAmount => HoursWorked * HourlyRate;

        public double CalculateTotalAmount()
        {
            return HoursWorked * HourlyRate;
        }
    }
}