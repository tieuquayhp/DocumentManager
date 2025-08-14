using DocumentManager.API.Helpers;

namespace DocumentManager.MVC.ViewModels
{
    public class OutgoingDocumentFormatIndexViewModel
    {
        public PagedResult<OutgoingDocumentFormatViewModel> PagedFormats { get; set; }
        public string? SearchQuery { get; set; }
    }
}
