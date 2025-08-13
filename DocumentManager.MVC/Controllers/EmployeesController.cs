using DocumentManager.API.Helpers; // Cần tham chiếu đến project API
using DocumentManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DocumentManager.MVC.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;

        public EmployeesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("ApiClient");
        }

        // GET: /Employees?searchQuery=...&pageNumber=...
        public async Task<IActionResult> Index(string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var apiUrl = $"api/employees?searchQuery={Uri.EscapeDataString(searchQuery ?? "")}&pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _client.GetAsync(apiUrl);

            var viewModel = new EmployeeIndexViewModel
            {
                SearchQuery = searchQuery,
                PagedEmployees = new PagedResult<EmployeeViewModel>(new List<EmployeeViewModel>(), 0, 1, pageSize)
            };

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var pagedResult = JsonConvert.DeserializeObject<PagedResult<EmployeeViewModel>>(jsonString);
                if (pagedResult != null)
                {
                    viewModel.PagedEmployees = pagedResult;
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không thể tải dữ liệu nhân viên từ API.");
            }

            return View(viewModel);
        }

        // GET: /Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var response = await _client.GetAsync($"api/employees/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonString = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<EmployeeViewModel>(jsonString);

            if (employee == null) return NotFound();

            return View(employee);
        }

        // GET: /Employees/Create
        public IActionResult Create()
        {
            // Truyền một ViewModel trống cho form
            return View(new EmployeeCreateViewModel());
        }

        // POST: /Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(employeeViewModel),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _client.PostAsync("api/employees", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Lỗi khi tạo mới nhân viên từ API.");
                }
            }
            return View(employeeViewModel);
        }

        // GET: /Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var response = await _client.GetAsync($"api/employees/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonString = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<EmployeeCreateViewModel>(jsonString); // Dùng CreateViewModel vì form edit và create giống nhau

            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST: /Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeCreateViewModel employeeViewModel)
        {
            if (id != employeeViewModel.ID) return BadRequest();

            if (ModelState.IsValid)
            {
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(employeeViewModel),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _client.PutAsync($"api/employees/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Lỗi khi cập nhật nhân viên từ API.");
                }
            }
            return View(employeeViewModel);
        }

        // GET: /Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var response = await _client.GetAsync($"api/employees/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonString = await response.Content.ReadAsStringAsync();
            var employee = JsonConvert.DeserializeObject<EmployeeViewModel>(jsonString);

            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST: /Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"api/employees/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}