using Data_Logger_1._3.Commands.CodingCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class CodingGenericViewModel : LogCacheViewModel
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
                LogCount = CacheItems.Count.ToString() + " coding logs cached | x total logs";
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public CodingGenericViewModel(NavigationService navigationService, DataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<CodeLOGViewModel>();

            CreateLogCommand = new CreateCodingLogCommand(_navigationService);
            ReportLogCommand = new ReportCodingLogCommand(_navigationService);
        }

        public CodingGenericViewModel(string logCount, NavigationService navigationService, DataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<CodeLOGViewModel>();

            LogCount = logCount;

            CreateLogCommand = new CreateCodingLogCommand(_navigationService);
            ReportLogCommand = new ReportCodingLogCommand(_navigationService);
        }

        public async void UpdateLogCount()
        {
            var temp = await _dataService.RetrieveCodingLogs(this);
            var count = temp.Count;
            LogCount = $"{CacheItems.Count} qt logs cached | {count} total logs";
        }
    }
}
