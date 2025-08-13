using DocumentManager.API.Helpers;

namespace DocumentManager.MVC.ViewModels
{
    public class OutgoingDocumentTypeIndexViewModel
    {
        public PagedResult<OutgoingDocumentTypeViewModel> PagedOutgoingDocumentTypes { get; set; }
        public string? SearchQuery { get; set; }
    }
}
