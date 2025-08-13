using DocumentManager.API.Helpers; // Bạn cần tham chiếu đến project API để dùng PagedResult
using DocumentManager.MVC.ViewModels; // Thêm using này

namespace DocumentManager.MVC.ViewModels // Đảm bảo namespace là chính xác
{
    public class EmployeeIndexViewModel
    {
        // Lớp này chứa kết quả phân trang của các nhân viên
        public PagedResult<EmployeeViewModel> PagedEmployees { get; set; }

        // Lớp này cũng lưu lại từ khóa tìm kiếm để hiển thị lại trên View
        public string? SearchQuery { get; set; }
    }
}