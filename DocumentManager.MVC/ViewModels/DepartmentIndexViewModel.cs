using DocumentManager.API.Helpers;
namespace DocumentManager.MVC.ViewModels
{
    public class DepartmentIndexViewModel
    {
        // PagedResult bây giờ sẽ chứa danh sách các phòng ban và thông tin phân trang
        public PagedResult<DepartmentViewModel> PagedDepartments { get; set; }

        // Lưu lại từ khóa tìm kiếm để hiển thị lại trong ô tìm kiếm
        public string? SearchQuery { get; set; }
    }
}
