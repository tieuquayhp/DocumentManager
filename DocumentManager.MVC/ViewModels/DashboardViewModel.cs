namespace DocumentManager.MVC.ViewModels
{
    public class DashboardViewModel
    {
        // Danh sách 5 tài liệu đến mới nhất
        public IEnumerable<IncomingDocumentViewModel> RecentIncomingDocuments { get; set; }

        // Danh sách 5 tài liệu đi mới nhất
        public IEnumerable<OutgoingDocumentViewModel> RecentOutgoingDocuments { get; set; }

        public DashboardViewModel()
        {
            RecentIncomingDocuments = new List<IncomingDocumentViewModel>();
            RecentOutgoingDocuments = new List<OutgoingDocumentViewModel>();
        }
    }
}
