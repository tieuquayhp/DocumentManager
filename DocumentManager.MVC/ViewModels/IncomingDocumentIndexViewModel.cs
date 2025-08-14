using DocumentManager.API.Helpers;
using DocumentManager.MVC.ViewModels;

namespace DocumentManager.MVC.ViewModels
{
    public class IncomingDocumentIndexViewModel
    {
        public PagedResult<IncomingDocumentViewModel> PagedDocuments { get; set; }
        public string? SearchQuery { get; set; }
    }
}