using Xunit;
using ST10442835_PROG6212_CMCS.Models;

namespace ST10442835_PROG6212_CMCS.Tests
{
    public class ClaimTest
    {
        [Fact]
        public void CalculateTotalAmount()
        {
            // Arrange
            var claim = new Claim();

            claim.HoursWorked = 20;
            claim.HourlyRate = 670;
            
            // Act
            var result = claim.CalculateTotalAmount();

            // Assert
            Assert.Equal(13400, result);
        }

        [Fact]
        public void AdditionalNotes_Simulation()
        {
            // Arrange
            var claim = new Claim();

            claim.Notes = "This is a test note claim.";

            // Act
            var notes = claim.Notes;

            // Assert
            Assert.Equal("This is a test note claim.", notes);
        }

        [Fact]
        public void FileProperties_IsStoredCorrectly()
        {
            // Arrange
            var claim = new Claim();

            claim.DocumentFileName = "invoice.pdf";
            claim.DocumentUrl = "encrypted_file_123.pdf";

            // Act
            var fileName = claim.DocumentFileName;
            var fileUrl = claim.DocumentUrl;

            // Assert
            Assert.Equal("invoice.pdf", fileName);
            Assert.Equal("encrypted_file_123.pdf", fileUrl);
        }


        //Test if status is pending on a new claim
        [Fact]
        public void NewClaim_HasDefaultValues()
        {
            // Arrange & Act
            var claim = new Claim();

            // Assert
            Assert.Equal("Pending", claim.Status);
            Assert.NotEmpty(claim.Id);
        }


        //Test status change
        [Fact]
        public void Claim_StatusChanges_ShouldReflectCorrectly()
        {
            // Arrange
            var claim = new Claim
            {
                Status = "Pending"
            };

            // Act & Assert - Test status transitions
            claim.Status = "Approved";
            Assert.Equal("Approved", claim.Status);

            claim.Status = "Rejected";
            Assert.Equal("Rejected", claim.Status);
        }
    }
}
