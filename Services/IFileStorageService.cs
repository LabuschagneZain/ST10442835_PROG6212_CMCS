using ST10442835_PROG6212_CMCS.Models;

namespace ST10442835_PROG6212_CMCS.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<byte[]> DownloadFileAsync(string fileName);
        Task<bool> DeleteFileAsync(string fileName);
    }
}