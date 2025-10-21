// Services/TableStorageService.cs
using Azure;
using Azure.Data.Tables;
using ST10442835_PROG6212_CMCS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST10442835_PROG6212_CMCS.Services
{
    public class TableStorageService
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableClient _tableClient;

        public TableStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            _tableServiceClient = new TableServiceClient(connectionString);
            _tableClient = _tableServiceClient.GetTableClient("Claims");
            _tableClient.CreateIfNotExists();
        }

        public async Task AddClaimAsync(Claim claim)
        {
            await _tableClient.AddEntityAsync(claim);
        }

        public async Task<List<Claim>> GetClaimsByLecturerAsync(string lecturerName)
        {
            var claims = new List<Claim>();
            var query = _tableClient.QueryAsync<Claim>(filter: $"PartitionKey eq 'claims' and LecturerName eq '{lecturerName}'");

            await foreach (var claim in query)
            {
                claims.Add(claim);
            }
            return claims;
        }

        public async Task<List<Claim>> GetAllClaimsAsync()
        {
            var claims = new List<Claim>();
            var query = _tableClient.QueryAsync<Claim>(filter: "PartitionKey eq 'claims'");

            await foreach (var claim in query)
            {
                claims.Add(claim);
            }
            return claims;
        }

        public async Task UpdateClaimStatusAsync(string claimId, string status)
        {
            var claim = await _tableClient.GetEntityAsync<Claim>("claims", claimId);
            if (claim != null)
            {
                claim.Value.Status = status;
                await _tableClient.UpdateEntityAsync(claim.Value, ETag.All);
            }
        }
    }
}