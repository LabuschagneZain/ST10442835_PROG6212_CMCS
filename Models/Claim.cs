// Models/Claim.cs
using Microsoft.AspNetCore.Http;
using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10442835_PROG6212_CMCS.Models
{
    public class Claim : ITableEntity
    {
        public string PartitionKey { get; set; } = "claims";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Id { get => RowKey; set => RowKey = value; }
        public string LecturerName { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public int HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; } = "Pending";
        public string DocumentUrl { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // For file uploads only; not stored in table
        [NotMapped]
        public IFormFile? DocumentFile { get; set; }

        [NotMapped]
        public double TotalAmount => HoursWorked * HourlyRate;

    }
}