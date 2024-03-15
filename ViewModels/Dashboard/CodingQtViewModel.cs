using Data_Logger_1._3.Commands.QtCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class CodingQtViewModel : LogCacheViewModel
    {
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
                LogCount = CacheItems.Count.ToString() + " qt logs cached | x total logs";
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public CodingQtViewModel(NavigationService navigationService) : base(navigationService)
        {
            CacheItems = new ObservableCollection<QtLOGViewModel>();

            CreateLogCommand = new CreateQtLogCommand(_navigationService);
            ReportLogCommand = new ReportQtLogCommand(_navigationService);
        }

        public CodingQtViewModel(string logCount, NavigationService navigationService) : base(navigationService)
        {
            CacheItems = new ObservableCollection<QtLOGViewModel>();
            LogCount = logCount;

            CreateLogCommand = new CreateQtLogCommand(_navigationService);
            ReportLogCommand = new ReportQtLogCommand(_navigationService);
        }

    }
}
