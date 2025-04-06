using Microsoft.Extensions.Configuration;
using System.IO;

namespace Repositories.Common
{
    public static class FileStorageConfig
    {
        private static readonly IConfiguration _configuration;

        static FileStorageConfig()
        {
            // Khởi tạo ConfigurationBuilder với đường dẫn đầy đủ
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: true, reloadOnChange: true)
                .Build();
        }

        // Lấy đường dẫn thư mục từ cấu hình
        public static string UploadDirectory => Path.Combine(Directory.GetCurrentDirectory(), _configuration["FileStorage:UploadDirectory"]);
    }
}
