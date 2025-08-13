using DocumentManager.API.Helpers;
using DocumentManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DocumentManager.MVC.Controllers
{
    public class IssuingUnitsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;
        public IssuingUnitsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("ApiClient");
        }
        // Action Index sử dụng IndexViewModel
        public async Task<IActionResult> Index(string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var apiUrl = $"api/issuingunits?searchQuery={Uri.EscapeDataString(searchQuery ?? "")}&pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl);
            var viewModel = new IssuingUnitIndexViewModel
            {
                SearchQuery = searchQuery,
                PagedIssuingUnits = new PagedResult<IssuingUnitViewModel>(new List<IssuingUnitViewModel>(), 0, 1, pageSize)
            };
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                viewModel.PagedIssuingUnits = JsonConvert.DeserializeObject<PagedResult<IssuingUnitViewModel>>(jsonString);
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/issuingunits/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var issuingUnit = JsonConvert.DeserializeObject<IssuingUnitViewModel>(await response.Content.ReadAsStringAsync());
            return View(issuingUnit);
        }
        // GET: /IssuingUnit/Create
        public IActionResult Create()
        {
            // Luôn truyền một model trống cho View để tránh lỗi null
            return View(new IssuingUnitViewModel());
        }
        // POST: /IssuingUnit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IssuingUnitViewModel issuingUnit) // Tham số là IssuingUnitViewModel
        {
            ModelState.Remove("ID"); // Loại bỏ ID khỏi ModelState để tránh lỗi khi tạo mới
            if (ModelState.IsValid)
            {
                // Tạo một đối tượng ẩn danh chỉ chứa các trường cần thiết để gửi đến API
                // Điều này khớp chính xác với DTO `IssuingUnitForCreationDto` của API
                var newIssuingUnit = new
                {
                    issuingUnit.IssuingUnitName
                };
                var content = new StringContent(JsonConvert.SerializeObject(newIssuingUnit), System.Text.Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("api/issuingunits", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Lỗi khi tạo đơn vị phát hành: {errorContent}");
                }
            }
            return View(issuingUnit); // Trả về View với model để hiển thị lỗi
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/issuingunits/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var issuingUnit = JsonConvert.DeserializeObject<IssuingUnitViewModel>(await response.Content.ReadAsStringAsync());
            return View(issuingUnit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IssuingUnitViewModel issuingUnit)
        {
            if (id != issuingUnit.ID) return BadRequest();
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(issuingUnit),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
                var response = await _client.PutAsync($"api/issuingunits/{id}", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Lỗi khi cập nhật đơn vị phát hành: {errorContent}");
                }
            }
            return View(issuingUnit); // Trả về View với model để hiển thị lỗi
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/issuingunits/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var issuingUnit = JsonConvert.DeserializeObject<IssuingUnitViewModel>(await response.Content.ReadAsStringAsync());
            return View(issuingUnit);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"api/issuingunits/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
