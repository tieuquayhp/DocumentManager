using DocumentManager.API.Helpers;
using DocumentManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DocumentManager.MVC.Controllers
{
    public class RelatedProjectsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;

        public RelatedProjectsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("ApiClient");
        }

        // Action Index sử dụng IndexViewModel
        public async Task<IActionResult> Index(string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var apiUrl = $"api/relatedprojects?searchQuery={Uri.EscapeDataString(searchQuery ?? "")}&pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl);

            var viewModel = new RelatedProjectIndexViewModel
            {
                SearchQuery = searchQuery,
                PagedProjects = new PagedResult<RelatedProjectViewModel>(new List<RelatedProjectViewModel>(), 0, 1, pageSize)
            };

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                viewModel.PagedProjects = JsonConvert.DeserializeObject<PagedResult<RelatedProjectViewModel>>(jsonString);
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/relatedprojects/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var project = JsonConvert.DeserializeObject<RelatedProjectViewModel>(await response.Content.ReadAsStringAsync());
            return View(project);
        }

        // GET: /RelatedProjects/Create
        public IActionResult Create()
        {
            // Luôn truyền một model trống cho View để tránh lỗi null
            return View(new RelatedProjectViewModel());
        }

        // POST: /RelatedProjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RelatedProjectViewModel project) // Tham số là RelatedProjectViewModel
        {
            // Xóa validation cho ID vì đây là tạo mới, ID luôn bằng 0
            ModelState.Remove("ID");

            if (ModelState.IsValid)
            {
                // Tạo một đối tượng ανώνυμος chỉ chứa các trường cần thiết để gửi đến API
                // Điều này khớp chính xác với DTO `RelatedProjectForCreationDto` của API
                var creationPayload = new { project.RelatedProjectName };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(creationPayload),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _client.PostAsync("api/relatedprojects", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Đọc lỗi từ API và hiển thị cho người dùng
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Lỗi từ API: {response.ReasonPhrase} - {errorContent}");
                }
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/relatedprojects/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var project = JsonConvert.DeserializeObject<RelatedProjectViewModel>(await response.Content.ReadAsStringAsync());
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RelatedProjectViewModel project)
        {
            if (id != project.ID) return BadRequest();
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"api/relatedprojects/{id}", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(project);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/relatedprojects/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var project = JsonConvert.DeserializeObject<RelatedProjectViewModel>(await response.Content.ReadAsStringAsync());
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"api/relatedprojects/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}