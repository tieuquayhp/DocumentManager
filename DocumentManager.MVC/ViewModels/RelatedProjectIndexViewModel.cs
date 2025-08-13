using DocumentManager.API.Helpers;

namespace DocumentManager.MVC.ViewModels
{
    public class RelatedProjectIndexViewModel
    {
        public PagedResult<RelatedProjectViewModel> PagedProjects { get; set; }
        public string? SearchQuery { get; set; }
    }
}
