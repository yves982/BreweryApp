using BreweryApp.ViewModels.Entities;
using BreweryApp.ViewModels.Utils;

namespace BreweryApp.ViewModels
{
    /// <summary>
    /// Used for ViewModels handling Paged collections
    /// </summary>
    interface IPagedViewModel
    {
        int CurrentPage { get; set; }
        int PageCount { get; }
        int ItemCount { get; set; }
        int ItemsPerPage { get; set; }

        DelegateCommandAsync<Page> SelectPageCommand { get; set; }
    }
}
