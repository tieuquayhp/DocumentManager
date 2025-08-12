using DocumentManager.MVC.Models;
using DocumentManager.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DocumentManager.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        // Tiêm IHttpClientFactory
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Sửa lại Action Index
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var dashboardViewModel = new DashboardViewModel();

            // Lấy 5 tài liệu đến mới nhất (giả sử API có endpoint này)
            // Chúng ta sẽ tạo một endpoint API mới để lấy giới hạn số lượng
            var incomingResponse = await client.GetAsync("api/incomingdocuments?limit=5");
            if (incomingResponse.IsSuccessStatusCode)
            {
                var jsonString = await incomingResponse.Content.ReadAsStringAsync();
                dashboardViewModel.RecentIncomingDocuments = JsonConvert.DeserializeObject<List<IncomingDocumentViewModel>>(jsonString);
            }

            // Lấy 5 tài liệu đi mới nhất
            var outgoingResponse = await client.GetAsync("api/outgoingdocuments?limit=5");
            if (outgoingResponse.IsSuccessStatusCode)
            {
                var jsonString = await outgoingResponse.Content.ReadAsStringAsync();
                dashboardViewModel.RecentOutgoingDocuments = JsonConvert.DeserializeObject<List<OutgoingDocumentViewModel>>(jsonString);
            }

            return View(dashboardViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
