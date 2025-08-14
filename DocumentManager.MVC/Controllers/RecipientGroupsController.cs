using DocumentManager.API.Helpers;
using DocumentManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace DocumentManager.MVC.Controllers
{
    public class RecipientGroupsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;

        public RecipientGroupsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> Index(string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var apiUrl = $"api/recipientgroups?searchQuery={Uri.EscapeDataString(searchQuery ?? "")}&pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await _client.GetAsync(apiUrl);

            var viewModel = new RecipientGroupIndexViewModel
            {
                SearchQuery = searchQuery,
                PagedGroups = new PagedResult<RecipientGroupViewModel>(new List<RecipientGroupViewModel>(), 0, 1, pageSize)
            };

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                viewModel.PagedGroups = JsonConvert.DeserializeObject<PagedResult<RecipientGroupViewModel>>(jsonString);
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/recipientgroups/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var group = JsonConvert.DeserializeObject<RecipientGroupViewModel>(await response.Content.ReadAsStringAsync());
            return View(group);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipientGroupViewModel group)
        {
            if (ModelState.IsValid)
            {
                var creationPayload = new { group.RecipientGroupName };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(creationPayload), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("api/recipientgroups", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(group);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/recipientgroups/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var group = JsonConvert.DeserializeObject<RecipientGroupViewModel>(await response.Content.ReadAsStringAsync());
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipientGroupViewModel group)
        {
            if (id != group.ID) return BadRequest();
            if (ModelState.IsValid)
            {
                var updatePayload = new { group.RecipientGroupName };
                var jsonContent = new StringContent(JsonConvert.SerializeObject(updatePayload), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"api/recipientgroups/{id}", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(group);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.GetAsync($"api/recipientgroups/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();
            var group = JsonConvert.DeserializeObject<RecipientGroupViewModel>(await response.Content.ReadAsStringAsync());
            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _client.DeleteAsync($"api/recipientgroups/{id}");
            return RedirectToAction(nameof(Index));
        }
        // GET: /RecipientGroups/ManageMembers/5
        [HttpGet]
        public async Task<IActionResult> ManageMembers(int id)
        {
            // Lấy thông tin nhóm
            var groupResponse = await _client.GetAsync($"api/recipientgroups/{id}");
            if (!groupResponse.IsSuccessStatusCode) return NotFound();
            var group = JsonConvert.DeserializeObject<RecipientGroupViewModel>(await groupResponse.Content.ReadAsStringAsync());
            if (group == null) return NotFound();

            // Lấy danh sách thành viên HIỆN TẠI trong nhóm
            var membersResponse = await _client.GetAsync($"api/group-management/group/{id}/employees");
            var currentMembers = new List<EmployeeViewModel>();
            if (membersResponse.IsSuccessStatusCode)
            {
                currentMembers = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(await membersResponse.Content.ReadAsStringAsync()) ?? new List<EmployeeViewModel>();
            }

            // Lấy TẤT CẢ nhân viên trong hệ thống
            var allEmployeesResponse = await _client.GetAsync("api/employees?pageSize=1000"); // Lấy tối đa 1000
            var allEmployees = new List<EmployeeViewModel>();
            if (allEmployeesResponse.IsSuccessStatusCode)
            {
                var pagedResult = JsonConvert.DeserializeObject<PagedResult<EmployeeViewModel>>(await allEmployeesResponse.Content.ReadAsStringAsync());
                if (pagedResult != null)
                {
                    allEmployees = pagedResult.Items;
                }
            }

            // Lọc ra danh sách nhân viên CHƯA có trong nhóm để điền vào dropdown
            var currentMemberIds = new HashSet<int>(currentMembers.Select(m => m.ID));
            var availableEmployees = allEmployees
                .Where(e => !currentMemberIds.Contains(e.ID))
                .Select(e => new SelectListItem
                {
                    Value = e.ID.ToString(),
                    Text = $"{e.Name} ({e.Email})"
                });

            var viewModel = new RecipientGroupMembersViewModel
            {
                GroupID = group.ID,
                GroupName = group.RecipientGroupName,
                Members = currentMembers,
                AvailableEmployees = availableEmployees
            };

            return View(viewModel);
        }
        // POST: /RecipientGroups/AddMemberToGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMemberToGroup(RecipientGroupMembersViewModel viewModel)
        {
            if (viewModel.EmployeeIdToAdd > 0)
            {
                var assignPayload = new
                {
                    EmployeeID = viewModel.EmployeeIdToAdd,
                    GroupID = viewModel.GroupID
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(assignPayload), Encoding.UTF8, "application/json");
                await _client.PostAsync("api/group-management/assign", jsonContent);
                // Có thể thêm kiểm tra response ở đây để xử lý lỗi
            }

            return RedirectToAction("ManageMembers", new { id = viewModel.GroupID });
        }
        // POST: /RecipientGroups/RemoveMemberFromGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMemberFromGroup(int groupId, int employeeId)
        {
            var unassignPayload = new
            {
                EmployeeID = employeeId,
                GroupID = groupId
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(unassignPayload), Encoding.UTF8, "application/json");
            await _client.PostAsync("api/group-management/unassign", jsonContent);

            return RedirectToAction("ManageMembers", new { id = groupId });
        }
    }
}