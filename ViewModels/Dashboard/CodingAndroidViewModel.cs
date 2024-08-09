using Data_Logger_1._3.Commands.LogCacheCommands.ASCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Windows;


namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class CodingAndroidViewModel : CodingViewModel
    {


        private ObservableCollection<AndroidLOGViewModel> cacheItems;
        public ObservableCollection<AndroidLOGViewModel> CacheItems
        {
            get
            {
                return cacheItems;
            }
            set
            {
                cacheItems = value;

                NoLogsMessageVisibility = CacheItems.Count == 0 ? Visibility.Visible : Visibility.Hidden;
                if (CacheItems is not null)
                    UpdateLogCount();

                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public CodingAndroidViewModel(NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
            CacheItems = new ObservableCollection<AndroidLOGViewModel>();

            CreateLogCommand = new CreateASLogCommand(_navigationService);
            ReportLogCommand = new ReportASLogCommand(_navigationService);
        }

        public CodingAndroidViewModel(string logCount, NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
            CacheItems = new ObservableCollection<AndroidLOGViewModel>();

            LogCount = logCount;

            CreateLogCommand = new CreateASLogCommand(_navigationService);
            ReportLogCommand = new ReportASLogCommand(_navigationService);
        }

        public override void UpdateLogCount()
        {
            if (CacheItems is not null)
            {
                var count = _dataService.ASLogCount();
                LogCount = $"{CacheItems.Count} android logs cached | {count} total logs";
            }
        }

    }
}
