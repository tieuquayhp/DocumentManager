
using DocumentManager.API.Helpers;
using DocumentManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; // Thư viện để làm việc với JSON
using System.Text;

namespace DocumentManagement.Mvc.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;

        // Tiêm IHttpClientFactory và lấy một HttpClient được cấu hình sẵn
        public DepartmentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("ApiClient"); // "ApiClient" là tên đã đăng ký trong Program.cs
        }

        // GET: /Departments
    public async Task<IActionResult> Index(string? searchQuery, int pageNumber = 1, int pageSize = 5)
        {
            /// Xây dựng URL động để gọi API
            // URLSearchParams sẽ tự động mã hóa các giá trị cho bạn
            var apiUrl = $"api/departments?searchQuery={searchQuery}&pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _client.GetAsync(apiUrl);

            var viewModel = new DepartmentIndexViewModel
            {
                SearchQuery = searchQuery,
                // Khởi tạo một PagedResult trống để tránh lỗi null trong View nếu API thất bại
                PagedDepartments = new PagedResult<DepartmentViewModel>(new List<DepartmentViewModel>(), 0, 1, pageSize)
            };

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var pagedResult = JsonConvert.DeserializeObject<PagedResult<DepartmentViewModel>>(jsonString);
                if (pagedResult != null)
                {
                    viewModel.PagedDepartments = pagedResult;
                }
            }
            else
            {
                // Xử lý lỗi từ API, ví dụ log lỗi hoặc hiển thị thông báo
                ModelState.AddModelError(string.Empty, "Không thể tải dữ liệu từ API.");
            }

            return View(viewModel);
        }

        // GET: /Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var response = await _client.GetAsync($"api/departments/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonString = await response.Content.ReadAsStringAsync();
            var department = JsonConvert.DeserializeObject<DepartmentViewModel>(jsonString);

            if (department == null) return NotFound();

            return View(department);
        }

        // GET: /Departments/Create
        public IActionResult Create()
        {
            // Chỉ cần hiển thị một form trống
            return View();
        }

        // POST: /Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ chống lại tấn công CSRF
        public async Task<IActionResult> Create([Bind("DepartmentName")] DepartmentViewModel department)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(department),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _client.PostAsync("api/departments", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index)); // Chuyển về trang danh sách nếu thành công
                }
                else
                {
                    // Có thể thêm lỗi từ API vào ModelState để hiển thị cho người dùng
                    ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra từ API.");
                }
            }
            return View(department); // Hiển thị lại form với lỗi validation
        }

        // GET: /Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var response = await _client.GetAsync($"api/departments/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonString = await response.Content.ReadAsStringAsync();
            var department = JsonConvert.DeserializeObject<DepartmentViewModel>(jsonString);

            if (department == null) return NotFound();

            return View(department);
        }

        // POST: /Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel department)
        {
            if (id != department.ID) return BadRequest();

            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(department),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _client.PutAsync($"api/departments/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Lỗi cập nhật từ API.");
                }
            }
            return View(department);
        }

        // GET: /Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var response = await _client.GetAsync($"api/departments/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonString = await response.Content.ReadAsStringAsync();
            var department = JsonConvert.DeserializeObject<DepartmentViewModel>(jsonString);

            if (department == null) return NotFound();

            return View(department);
        }

        // POST: /Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _client.DeleteAsync($"api/departments/{id}");

            // Thậm chí nếu xóa không thành công, chúng ta vẫn chuyển hướng về trang chủ.
            // Một hệ thống nâng cao hơn có thể hiển thị thông báo lỗi.
            return RedirectToAction(nameof(Index));
        }
    }
}