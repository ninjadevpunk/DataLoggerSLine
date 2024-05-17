using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class LogCacheViewModel : ViewModelBase
    {
        protected readonly DataService _dataService;
        public NavigationService _navigationService;


        private string logCount = "0 logs cached | 0 total logs";
        public string LogCount
        {
            get
            {
                return logCount;
            }
            set
            {
                logCount = value;
                OnPropertyChanged(nameof(LogCount));
            }
        }

        public ICommand CreateLogCommand { get; set; }
        public ICommand ReportLogCommand { get; set; }


        public LogCacheViewModel(NavigationService navigationService, DataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;

        }


    }
}
