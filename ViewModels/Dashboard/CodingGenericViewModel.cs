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


        public CodingGenericViewModel(NavigationService navigationService) : base(navigationService)
        {
            CacheItems = new ObservableCollection<CodeLOGViewModel>();

            CreateLogCommand = new CreateCodingLogCommand(_navigationService);
            ReportLogCommand = new ReportCodingLogCommand(_navigationService);
        }

        public CodingGenericViewModel(string logCount, NavigationService navigationService) : base(navigationService)
        {
            CacheItems = new ObservableCollection<CodeLOGViewModel>();

            LogCount = logCount;

            CreateLogCommand = new CreateCodingLogCommand(_navigationService);
            ReportLogCommand = new ReportCodingLogCommand(_navigationService);
        }

    }
}
