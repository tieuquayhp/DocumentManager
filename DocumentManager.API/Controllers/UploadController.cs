using Microsoft.AspNetCore.Mvc;

namespace DocumentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        // Tiêm IWebHostEnvironment để có thể lấy đường dẫn đến thư mục wwwroot
        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> PostFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Không có file nào được chọn.");
            }

            try
            {
                // 1. Tạo một đường dẫn lưu file an toàn
                // Thư mục lưu trữ sẽ là: wwwroot/uploads
                var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                // 2. Tạo một tên file duy nhất để tránh trùng lặp
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                // 3. Lưu file vào server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 4. Trả về đường dẫn tương đối của file đã lưu
                // Ví dụ: /uploads/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx_tentep.pdf
                var fileUrl = $"/uploads/{uniqueFileName}";

                // Trả về một đối tượng JSON chứa đường dẫn
                return Ok(new { FilePath = fileUrl });
            }
            catch (Exception ex)
            {
                // Log lỗi ra console hoặc hệ thống log
                Console.WriteLine($"Lỗi upload file: {ex.Message}");
                return StatusCode(500, $"Lỗi server nội bộ: {ex.Message}");
            }
        }
    }
}