using Data_Logger_1._3.Commands.QtCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class CodingQtViewModel : LogCacheViewModel
    {


        public CodingQtViewModel(NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
            CacheItems = new ObservableCollection<QtLOGViewModel>();

            CreateLogCommand = new CreateQtLogCommand(_navigationService);
            ReportLogCommand = new ReportQtLogCommand(_navigationService);
        }

        public CodingQtViewModel(string logCount, NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
            CacheItems = new ObservableCollection<QtLOGViewModel>();
            LogCount = logCount;

            CreateLogCommand = new CreateQtLogCommand(_navigationService);
            ReportLogCommand = new ReportQtLogCommand(_navigationService);
        }


        private ObservableCollection<QtLOGViewModel> cacheItems;
        public ObservableCollection<QtLOGViewModel> CacheItems
        {
            get
            {
                return cacheItems;
            }
            set
            {
                cacheItems = value;

                NoLogsMessageVisibility = CacheItems.Count == 0 ? Visibility.Visible : Visibility.Hidden;
                UpdateLogCount();

                OnPropertyChanged(nameof(CacheItems));
            }
        }

        private Visibility noLogsMessageVisibility;
        public Visibility NoLogsMessageVisibility
        {
            get
            {
                return noLogsMessageVisibility;
            }
            set
            {
                noLogsMessageVisibility = value;
                OnPropertyChanged(nameof(NoLogsMessageVisibility));
            }
        }

        private async void UpdateLogCount()
        {
            var temp = await _dataService.RetrieveQtCodingLogs(this);
            var count = temp.Count;
            LogCount = $"{CacheItems.Count} qt logs cached | {count} total logs";
        }

    }
}
