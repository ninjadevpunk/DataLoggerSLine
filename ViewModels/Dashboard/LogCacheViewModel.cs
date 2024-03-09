using Data_Logger_1._3.Messages;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public abstract class LogCacheViewModel : ViewModelBase
    {
        private readonly AuthService _authService;
        private readonly DataService _dataService;
        public NavigationService _navigationService;

        public string LogCount { get; set; } = "0 logs cached | 0 total logs";

        public ICommand CreateLogCommand { get; set; }
        public ICommand ReportLogCommand { get; set; }


        public LogCacheViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            // Subscribe to RemoveItemMessage
            MessagingService.Subscribe<RemoveItemMessage>(RemoveItemMethod);
        }


        public abstract void RemoveItemMethod(RemoveItemMessage item);
    }
}
