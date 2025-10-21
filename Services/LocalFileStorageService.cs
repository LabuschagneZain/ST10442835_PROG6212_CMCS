using System.Security.Cryptography;
using System.Text;
using ST10442835_PROG6212_CMCS.Models;

namespace ST10442835_PROG6212_CMCS.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _uploadPath;
        private readonly byte[] _encryptionKey;

        public LocalFileStorageService(IConfiguration configuration)
        {
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }

            // Generate or use a fixed encryption key (in production, use secure key management)
            var keyString = configuration["EncryptionKey"] ?? "0123456789ABCDEF0123456789ABCDEF";
            _encryptionKey = Encoding.UTF8.GetBytes(keyString.PadRight(32).Substring(0, 32));
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            // Validate file type
            var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                throw new InvalidOperationException("Only PDF, DOCX, and XLSX files are allowed");

            // Validate file size (5MB max)
            if (file.Length > 5 * 1024 * 1024)
                throw new InvalidOperationException("File size cannot exceed 5MB");

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Encrypt the file
                using (var aes = Aes.Create())
                {
                    aes.Key = _encryptionKey;
                    aes.GenerateIV();

                    // Write IV to the beginning of the file
                    await stream.WriteAsync(aes.IV, 0, aes.IV.Length);

                    using (var cryptoStream = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        await file.CopyToAsync(cryptoStream);
                    }
                }
            }

            return fileName;
        }

        public async Task<byte[]> DownloadFileAsync(string fileName)
        {
            var filePath = Path.Combine(_uploadPath, fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found");

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = _encryptionKey;

                    // Read IV from the beginning of the file
                    var iv = new byte[16];
                    await stream.ReadAsync(iv, 0, iv.Length);
                    aes.IV = iv;

                    using (var cryptoStream = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var memoryStream = new MemoryStream())
                    {
                        await cryptoStream.CopyToAsync(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        public Task<bool> DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_uploadPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}