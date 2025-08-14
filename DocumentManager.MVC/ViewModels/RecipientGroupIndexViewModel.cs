using DocumentManager.API.Helpers;

namespace DocumentManager.MVC.ViewModels
{
    public class RecipientGroupIndexViewModel
    {
        public PagedResult<RecipientGroupViewModel> PagedGroups { get; set; }
        public string? SearchQuery { get; set; }
    }
}
