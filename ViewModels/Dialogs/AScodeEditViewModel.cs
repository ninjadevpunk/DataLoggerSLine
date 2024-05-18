using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class AScodeEditViewModel : AScodeCreateViewModel
    {
        public AScodeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
        }
    }
}
