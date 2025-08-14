using DocumentManager.API.Helpers;
using DocumentManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace DocumentManager.MVC.Controllers
{
    public class OutgoingDocumentFormatsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;

        public OutgoingDocumentFormatsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("ApiClient");
        }

        // --- HÀM TRỢ GIÚP ĐỂ LẤY DỮ LIỆU DROPDOWN ---
        private async Task<IEnumerable<SelectListItem>> GetDocumentTypesAsync()
        {
            var response = await _client.GetAsync("api/outgoingdocumenttypes?pageSize=1000"); // Lấy tất cả
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var pagedResult = JsonConvert.DeserializeObject<PagedResult<OutgoingDocumentTypeViewModel>>(jsonString);
                return pagedResult.Items.Select(t => new SelectListItem
                {
                    Value = t.ID.ToString(),
                    Text = t.OutgoingDocumentTypeName
                });
            }
            return new List<SelectListItem>();
        }

        public async Task<IActionResult> Index(string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var apiUrl = $"api/outgoingdocumentformats?searchQuery={Uri.EscapeDataString(searchQuery ?? "")}&pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl);

            var viewModel = new OutgoingDocumentFormatIndexViewModel
            {
                SearchQuery = searchQuery,
                PagedFormats = new PagedResult<OutgoingDocumentFormatViewModel>(new List<OutgoingDocumentFormatViewModel>(), 0, 1, pageSize)
            };

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                viewModel.PagedFormats = JsonConvert.DeserializeObject<PagedResult<OutgoingDocumentFormatViewModel>>(jsonString);
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/outgoingdocumentformats/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var formatDto = JsonConvert.DeserializeObject<OutgoingDocumentFormatViewModel>(await response.Content.ReadAsStringAsync());
            if (formatDto == null) return NotFound();
            return View(formatDto);
        }

        // GET: OutgoingDocumentFormats/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new OutgoingDocumentFormatCreateViewModel
            {
                OutgoingDocumentType = await GetDocumentTypesAsync()
            };
            return View(viewModel);
        }

        // POST: OutgoingDocumentFormats/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OutgoingDocumentFormatCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("api/outgoingdocumentformats", jsonContent);
                return RedirectToAction(nameof(Index));
                //if (response.IsSuccessStatusCode)
                //{
                //    return RedirectToAction(nameof(Index));
                //}
                //ModelState.AddModelError(string.Empty, "Lỗi khi tạo mới từ API.");
            }
            // Nếu model không hợp lệ, phải load lại dropdown list
            viewModel.OutgoingDocumentType = await GetDocumentTypesAsync();
            return View(viewModel);
        }

        // GET: OutgoingDocumentFormats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var response = await _client.GetAsync($"api/outgoingdocumentformats/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var formatDto = JsonConvert.DeserializeObject<OutgoingDocumentFormatViewModel>(await response.Content.ReadAsStringAsync());
            if (formatDto == null) return NotFound();

            var viewModel = new OutgoingDocumentFormatCreateViewModel
            {
                ID = formatDto.ID,
                OutgoingDocumentFormatName = formatDto.OutgoingDocumentFormatName,
                OutgoingDocumentTypeID = formatDto.OutgoingDocumentTypeID,
                OutgoingDocumentType = await GetDocumentTypesAsync() // Lấy danh sách cho dropdown
            };

            return View(viewModel);
        }

        // POST: OutgoingDocumentFormats/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OutgoingDocumentFormatCreateViewModel viewModel)
        {
            if (id != viewModel.ID) return BadRequest();
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"api/outgoingdocumentformats/{id}", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Lỗi khi cập nhật từ API.");
            }
            // Nếu model không hợp lệ, phải load lại dropdown list
            viewModel.OutgoingDocumentType = await GetDocumentTypesAsync();
            return View(viewModel);
        }
        // GET: OutgoingDocumentFormats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/outgoingdocumentformats/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var formatDto = JsonConvert.DeserializeObject<OutgoingDocumentFormatViewModel>(await response.Content.ReadAsStringAsync());
            if (formatDto == null) return NotFound();
            return View(formatDto);
        }
        // POST: OutgoingDocumentFormats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"api/outgoingdocumentformats/{id}");
            return RedirectToAction(nameof(Index));
        }


    }
}