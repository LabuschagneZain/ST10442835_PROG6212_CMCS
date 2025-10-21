using System.Text.Json;
using ST10442835_PROG6212_CMCS.Models;

namespace ST10442835_PROG6212_CMCS.Services
{
    public class JsonClaimService : IClaimService
    {
        private readonly string _dataPath;
        private List<Claim> _claims;
        private readonly object _lock = new object();

        public JsonClaimService(IConfiguration configuration)
        {
            _dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "claims.json");
            var directory = Path.GetDirectoryName(_dataPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
            _claims = LoadClaims();
        }

        private List<Claim> LoadClaims()
        {
            lock (_lock)
            {
                if (!File.Exists(_dataPath))
                {
                    return new List<Claim>();
                }

                var json = File.ReadAllText(_dataPath);
                return JsonSerializer.Deserialize<List<Claim>>(json) ?? new List<Claim>();
            }
        }

        private void SaveClaims()
        {
            lock (_lock)
            {
                var json = JsonSerializer.Serialize(_claims, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_dataPath, json);
            }
        }

        public Task AddClaimAsync(Claim claim)
        {
            _claims.Add(claim);
            SaveClaims();
            return Task.CompletedTask;
        }

        public Task<List<Claim>> GetClaimsByLecturerAsync(string lecturerName)
        {
            var lecturerClaims = _claims.Where(c => c.LecturerName.Equals(lecturerName, StringComparison.OrdinalIgnoreCase)).ToList();
            return Task.FromResult(lecturerClaims);
        }

        public Task<List<Claim>> GetAllClaimsAsync()
        {
            return Task.FromResult(_claims);
        }

        public Task UpdateClaimStatusAsync(string claimId, string status)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = status;
                SaveClaims();
            }
            return Task.CompletedTask;
        }

        public Task<Claim?> GetClaimByIdAsync(string claimId)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == claimId);
            return Task.FromResult(claim);
        }
    }
}