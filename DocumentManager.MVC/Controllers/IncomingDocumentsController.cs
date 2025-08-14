using DocumentManager.API.DTOs;
using DocumentManager.API.Helpers;
using DocumentManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DocumentManager.MVC.Controllers
{
    public class IncomingDocumentsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;

        public IncomingDocumentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("ApiClient");
        }

        // ... Action Index của bạn ở đây ...

        // --- HÀM TRỢ GIÚP ĐỂ LẤY DỮ LIỆU CHO CÁC DROPDOWN ---
        private async Task PopulateDropdowns(IncomingDocumentCreateViewModel viewModel)
        {
            // Lấy danh sách Đơn vị phát hành
            var issuingUnitsResponse = await _client.GetAsync("api/issuingunits?pageSize=1000");
            if (issuingUnitsResponse.IsSuccessStatusCode)
            {
                var pagedResult = JsonConvert.DeserializeObject<PagedResult<IssuingUnitViewModel>>(await issuingUnitsResponse.Content.ReadAsStringAsync());
                viewModel.IssuingUnits = pagedResult.Items.Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.IssuingUnitName });
            }

            // Lấy danh sách Dự án
            var projectsResponse = await _client.GetAsync("api/relatedprojects?pageSize=1000");
            if (projectsResponse.IsSuccessStatusCode)
            {
                var pagedResult = JsonConvert.DeserializeObject<PagedResult<RelatedProjectViewModel>>(await projectsResponse.Content.ReadAsStringAsync());
                viewModel.RelatedProjects = pagedResult.Items.Select(p => new SelectListItem { Value = p.ID.ToString(), Text = p.RelatedProjectName });
            }

            // Lấy danh sách Nhóm nhận
            var groupsResponse = await _client.GetAsync("api/recipientgroups?pageSize=1000");
            if (groupsResponse.IsSuccessStatusCode)
            {
                var pagedResult = JsonConvert.DeserializeObject<PagedResult<RecipientGroupViewModel>>(await groupsResponse.Content.ReadAsStringAsync());
                viewModel.AllRecipientGroups = pagedResult.Items.Select(g => new SelectListItem { Value = g.ID.ToString(), Text = g.RecipientGroupName });
            }
        }

        // GET: /IncomingDocuments/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new IncomingDocumentCreateViewModel();
            await PopulateDropdowns(viewModel); // Gọi hàm để điền dữ liệu
            return View(viewModel);
        }

        // POST: /IncomingDocuments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncomingDocumentCreateViewModel viewModel, IFormFile? documentFile)
        {
            // --- BƯỚC 1: VALIDATION PHÍA CLIENT ---
            if (documentFile == null || documentFile.Length == 0)
            {
                ModelState.AddModelError("DocumentFile", "Vui lòng chọn một tệp đính kèm.");
            }

            // ... (Xóa các validation không cần thiết) ...
            ModelState.Remove("ID");
            ModelState.Remove("IssuingUnits");
            ModelState.Remove("RelatedProjects");
            ModelState.Remove("AllRecipientGroups");

            if (ModelState.IsValid)
            {
                try
                {
                    // --- BƯỚC 2: TẠO YÊU CẦU MULTIPART ---
                    using (var content = new MultipartFormDataContent())
                    {
                        // a. Thêm các dữ liệu từ form vào content
                        // Tên của các StringContent phải khớp chính xác với tên thuộc tính trong DTO của API
                        content.Add(new StringContent(viewModel.IncomingDocumentNumber), nameof(IncomingDocumentForCreationDto.IncomingDocumentNumber));
                        content.Add(new StringContent(viewModel.ReleaseDate.ToString("o")), nameof(IncomingDocumentForCreationDto.ReleaseDate));
                        content.Add(new StringContent(viewModel.DocumentCodeFromIssuer ?? ""), nameof(IncomingDocumentForCreationDto.DocumentCodeFromIssuer));
                        if (viewModel.ReleaseDateFromIssuer.HasValue)
                            content.Add(new StringContent(viewModel.ReleaseDateFromIssuer.Value.ToString("o")), nameof(IncomingDocumentForCreationDto.ReleaseDateFromIssuer));
                        content.Add(new StringContent(viewModel.DocumentContent), nameof(IncomingDocumentForCreationDto.DocumentContent));
                        content.Add(new StringContent(viewModel.IssuingUnitID.ToString()), nameof(IncomingDocumentForCreationDto.IssuingUnitID));
                        content.Add(new StringContent(viewModel.RelatedProjectID.ToString()), nameof(IncomingDocumentForCreationDto.RelatedProjectID));

                        // b. Thêm danh sách ID
                        // Tên phải khớp với thuộc tính trong DTO của API: "RecipientGroupIDs"
                        foreach (var groupId in viewModel.SelectedRecipientGroupIDs)
                        {
                            content.Add(new StringContent(groupId.ToString()), nameof(IncomingDocumentForCreationDto.RecipientGroupIDs));
                        }

                        // c. Thêm file
                        // Tên phải khớp với tên tham số trong API: "file"
                        content.Add(new StreamContent(documentFile.OpenReadStream()), "file", documentFile.FileName);


                        // --- BƯỚC 3: GỬI TOÀN BỘ YÊU CẦU ĐẾN API ---
                        var response = await _client.PostAsync("api/incomingdocuments", content);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            ModelState.AddModelError(string.Empty, $"Lỗi từ API: {errorContent}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Đã có lỗi xảy ra: {ex.Message}");
                }
            }

            // Nếu có lỗi, load lại dropdowns và hiển thị lại form
            await PopulateDropdowns(viewModel);
            return View(viewModel);
        }
        // Tạo một lớp nhỏ để nhận kết quả từ API Upload
        public class UploadResult
        {
            public string FilePath { get; set; } = string.Empty;
        }
        // GET: /IncomingDocuments
        public async Task<IActionResult> Index(string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            var apiUrl = $"api/incomingdocuments?searchQuery={Uri.EscapeDataString(searchQuery ?? "")}&pageNumber={pageNumber}&pageSize={pageSize}";

            var response = await _client.GetAsync(apiUrl);

            var viewModel = new IncomingDocumentIndexViewModel
            {
                SearchQuery = searchQuery,
                // Khởi tạo giá trị mặc định để tránh lỗi null trong View
                PagedDocuments = new PagedResult<IncomingDocumentViewModel>(new List<IncomingDocumentViewModel>(), 0, 1, pageSize)
            };

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var pagedResult = JsonConvert.DeserializeObject<PagedResult<IncomingDocumentViewModel>>(jsonString);
                if (pagedResult != null)
                {
                    viewModel.PagedDocuments = pagedResult;
                }
            }
            else
            {
                // Xử lý trường hợp API trả về lỗi
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách tài liệu từ API.");
            }

            return View(viewModel);
        }
        // GET: /IncomingDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _client.GetAsync($"api/incomingdocuments/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Có thể trả về một trang lỗi tùy chỉnh
                return NotFound();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var viewModel = JsonConvert.DeserializeObject<IncomingDocumentViewModel>(jsonString);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        // GET: /IncomingDocuments/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 1. Lấy dữ liệu hiện tại của tài liệu từ API
            var response = await _client.GetAsync($"api/incomingdocuments/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            // API trả về DTO hiển thị, chứa danh sách tên nhóm
            var documentDto = JsonConvert.DeserializeObject<IncomingDocumentViewModel>(await response.Content.ReadAsStringAsync());
            if (documentDto == null)
            {
                return NotFound();
            }

            // 2. Lấy danh sách ID các nhóm nhận hiện tại của tài liệu
            // Chúng ta cần gọi API để lấy RecipientGroups và lọc ra ID dựa trên tên
            // (Đây là một cách làm, cách tốt hơn là API nên trả về cả ID và Tên nhóm)
            var allGroupsResponse = await _client.GetAsync("api/recipientgroups?pageSize=1000");
            var allGroups = new List<RecipientGroupViewModel>();
            if (allGroupsResponse.IsSuccessStatusCode)
            {
                allGroups = (JsonConvert.DeserializeObject<PagedResult<RecipientGroupViewModel>>(await allGroupsResponse.Content.ReadAsStringAsync())).Items;
            }

            var selectedGroupIds = allGroups
                .Where(g => documentDto.RecipientGroupNames.Contains(g.RecipientGroupName))
                .Select(g => g.ID)
                .ToList();


            // 3. Chuẩn bị ViewModel cho form Edit
            var viewModel = new IncomingDocumentCreateViewModel
            {
                ID = documentDto.ID,
                IncomingDocumentNumber = documentDto.IncomingDocumentNumber,
                ReleaseDate = documentDto.ReleaseDate,
                DocumentCodeFromIssuer = documentDto.DocumentCodeFromIssuer,
                ReleaseDateFromIssuer = documentDto.ReleaseDateFromIssuer,
                DocumentContent = documentDto.DocumentContent,
                DocumentFile = documentDto.DocumentFile,
                IssuingUnitID = documentDto.IssuingUnitID,
                RelatedProjectID = documentDto.RelatedProjectID,
                SelectedRecipientGroupIDs = selectedGroupIds
            };

            // 4. Điền dữ liệu cho các dropdown
            await PopulateDropdowns(viewModel);

            return View(viewModel);
        }

        // POST: /IncomingDocuments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IncomingDocumentCreateViewModel viewModel, IFormFile? documentFile)
        {
            if (id != viewModel.ID)
            {
                return BadRequest();
            }

            // Xóa validation cho các thuộc tính không được bind từ form
            ModelState.Remove("IssuingUnits");
            ModelState.Remove("RelatedProjects");
            ModelState.Remove("AllRecipientGroups");
            // Khi chỉnh sửa, file không còn bắt buộc nữa. Chúng ta sẽ không validate nó ở đây.
            // Logic của API sẽ tự xử lý việc giữ lại file cũ nếu không có file mới.
            ModelState.Remove("documentFile");


            if (ModelState.IsValid)
            {
                using (var content = new MultipartFormDataContent())
                {
                    // --- Đóng gói dữ liệu form ---
                    content.Add(new StringContent(viewModel.IncomingDocumentNumber), nameof(IncomingDocumentForUpdateDto.IncomingDocumentNumber));
                    // ... (Thêm các trường khác tương tự như action Create)
                    content.Add(new StringContent(viewModel.DocumentContent), nameof(IncomingDocumentForUpdateDto.DocumentContent));
                    content.Add(new StringContent(viewModel.IssuingUnitID.ToString()), nameof(IncomingDocumentForUpdateDto.IssuingUnitID));
                    content.Add(new StringContent(viewModel.RelatedProjectID.ToString()), nameof(IncomingDocumentForUpdateDto.RelatedProjectID));

                    foreach (var groupId in viewModel.SelectedRecipientGroupIDs)
                    {
                        content.Add(new StringContent(groupId.ToString()), "recipientGroupIDs");
                    }

                    // --- Đóng gói file (nếu có) ---
                    if (documentFile != null && documentFile.Length > 0)
                    {
                        content.Add(new StreamContent(documentFile.OpenReadStream()), "file", documentFile.FileName);
                    }

                    // --- Gửi yêu cầu PUT đến API ---
                    var response = await _client.PutAsync($"api/incomingdocuments/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, $"Lỗi từ API: {errorContent}");
                    }
                }
            }

            // Nếu model không hợp lệ, load lại dropdowns và hiển thị lại form
            await PopulateDropdowns(viewModel);
            return View(viewModel);
        }
    }
}