using Microsoft.AspNetCore.Mvc.Rendering;

namespace DocumentManager.MVC.ViewModels
{
    public class RecipientGroupMembersViewModel
    {
        // Thông tin của nhóm đang được quản lý
        public int GroupID { get; set; }
        public string GroupName { get; set; } = string.Empty;

        // Danh sách các nhân viên đã có trong nhóm
        public List<EmployeeViewModel> Members { get; set; } = new List<EmployeeViewModel>();

        // --- Dành cho form "Thêm thành viên mới" ---

        // ID của nhân viên được chọn từ dropdown để thêm vào
        public int EmployeeIdToAdd { get; set; }

        // Danh sách tất cả các nhân viên chưa có trong nhóm để hiển thị trong dropdown
        public IEnumerable<SelectListItem> AvailableEmployees { get; set; } = new List<SelectListItem>();
    }
}