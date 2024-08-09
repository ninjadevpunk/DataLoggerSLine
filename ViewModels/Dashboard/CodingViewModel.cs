using Data_Logger_1._3.Commands.LogCacheCommands.CodingCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class CodingViewModel : LogCacheViewModel
    {
        private ObservableCollection<CodeLOGViewModel> cacheItems;
        public ObservableCollection<CodeLOGViewModel> CacheItems
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


        public CodingViewModel(NavigationService navigationService, DataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<CodeLOGViewModel>();

            CreateLogCommand = new CreateCodingLogCommand(_navigationService);
            ReportLogCommand = new ReportCodingLogCommand(_navigationService);
        }

        public CodingViewModel(string logCount, NavigationService navigationService, DataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<CodeLOGViewModel>();

            LogCount = logCount;

            CreateLogCommand = new CreateCodingLogCommand(_navigationService);
            ReportLogCommand = new ReportCodingLogCommand(_navigationService);
        }

        public virtual void UpdateLogCount()
        {
            var count = _dataService.LogCount(LOG.CATEGORY.CODING);
            LogCount = $"{CacheItems.Count} coding logs cached | {count} total logs";
        }
    }
}
