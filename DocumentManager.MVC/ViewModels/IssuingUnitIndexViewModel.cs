using DocumentManager.API.Helpers;
using DocumentManager.DAL.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class IssuingUnitIndexViewModel
    {
        public PagedResult<IssuingUnitViewModel> PagedIssuingUnits { get; set; }
        public string SearchQuery { get; set; }
    }
    }
