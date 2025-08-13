using DocumentManager.API.Helpers;
using DocumentManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DocumentManager.MVC.Controllers
{
    public class OutgoingDocumentTypesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;
        public OutgoingDocumentTypesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("ApiClient");
        }
        // Action Index sử dụng IndexViewModel
        public async Task<IActionResult> Index(string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var apiUrl = $"api/outgoingdocumenttypes?searchQuery={Uri.EscapeDataString(searchQuery ?? "")}&pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl);
            var viewModel = new OutgoingDocumentTypeIndexViewModel
            {
                SearchQuery = searchQuery,
                PagedOutgoingDocumentTypes = new PagedResult<OutgoingDocumentTypeViewModel>(new List<OutgoingDocumentTypeViewModel>(), 0, 1, pageSize)
            };
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                viewModel.PagedOutgoingDocumentTypes = JsonConvert.DeserializeObject<PagedResult<OutgoingDocumentTypeViewModel>>(jsonString);
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/outgoingdocumenttypes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var outgoingDocumentType = JsonConvert.DeserializeObject<OutgoingDocumentTypeViewModel>(await response.Content.ReadAsStringAsync());
            return View(outgoingDocumentType);
        }
        // GET: /OutgoingDocumentType/Create
        public IActionResult Create()
        {
            // Luôn truyền một model trống cho View để tránh lỗi null
            return View(new OutgoingDocumentTypeViewModel());
        }
        // POST: /OutgoingDocumentType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OutgoingDocumentTypeViewModel outgoingDocumentType) // Tham số là OutgoingDocumentTypeViewModel
        {
            ModelState.Remove("ID"); // Loại bỏ ID khỏi ModelState để tránh lỗi khi tạo mới
            if (ModelState.IsValid)
            {
                // Tạo một đối tượng ẩn danh chỉ chứa các trường cần thiết để gửi đến API
                // Điều này khớp chính xác với DTO `OutgoingDocumentTypeForCreationDto` của API
                var newOutgoingDocumentType = new
                {
                    outgoingDocumentType.OutgoingDocumentTypeName
                };
                var content = new StringContent(JsonConvert.SerializeObject(newOutgoingDocumentType), System.Text.Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("api/outgoingdocumenttypes", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không thể tạo loại văn bản đi mới. Vui lòng thử lại.");
                }
            }
            return View(outgoingDocumentType);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/outgoingdocumenttypes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var outgoingDocumentType = JsonConvert.DeserializeObject<OutgoingDocumentTypeViewModel>(await response.Content.ReadAsStringAsync());
            return View(outgoingDocumentType);
        }
        // POST: /OutgoingDocumentType/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OutgoingDocumentTypeViewModel outgoingDocumentType) // Tham số là OutgoingDocumentTypeViewModel
        {
            if (id != outgoingDocumentType.ID) return NotFound();
            if (ModelState.IsValid)
            {
                // Tạo một đối tượng ẩn danh chỉ chứa các trường cần thiết để gửi đến API
                // Điều này khớp chính xác với DTO `OutgoingDocumentTypeForUpdateDto` của API
                var jsonContent = new StringContent(JsonConvert.SerializeObject(new
                {
                    outgoingDocumentType.OutgoingDocumentTypeName
                }), System.Text.Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"api/outgoingdocumenttypes/{id}", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Lỗi khi cập nhật loại văn bản đi: {errorContent}");
                }
            }
            return View(outgoingDocumentType);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/outgoingdocumenttypes/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var outgoingDocumentType = JsonConvert.DeserializeObject<OutgoingDocumentTypeViewModel>(await response.Content.ReadAsStringAsync());
            return View(outgoingDocumentType);
        }
        // POST: /OutgoingDocumentType/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"api/outgoingdocumenttypes/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
