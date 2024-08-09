using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class flexiEditViewModel : flexiCreateViewModel
    {
        public flexiEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
        }
    }
}
