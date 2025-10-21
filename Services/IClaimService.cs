using ST10442835_PROG6212_CMCS.Models;

namespace ST10442835_PROG6212_CMCS.Services
{
    public interface IClaimService
    {
        Task AddClaimAsync(Claim claim);
        Task<List<Claim>> GetClaimsByLecturerAsync(string lecturerName);
        Task<List<Claim>> GetAllClaimsAsync();
        Task UpdateClaimStatusAsync(string claimId, string status);
        Task<Claim?> GetClaimByIdAsync(string claimId);
    }
}