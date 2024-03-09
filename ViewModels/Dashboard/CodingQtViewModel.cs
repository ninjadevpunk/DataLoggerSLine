using Data_Logger_1._3.Commands.QtCommands;
using Data_Logger_1._3.Messages;
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
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public CodingQtViewModel(NavigationService navigationService) : base(navigationService)
        {
            CreateLogCommand = new CreateQtLogCommand(_navigationService);
            ReportLogCommand = new ReportQtLogCommand(_navigationService);
        }

        public CodingQtViewModel(string logCount, NavigationService navigationService) : base(navigationService)
        {
            LogCount = logCount;

            CreateLogCommand = new CreateQtLogCommand(_navigationService);
            ReportLogCommand = new ReportQtLogCommand(_navigationService);
        }

        public override void RemoveItemMethod(RemoveItemMessage item)
        {
            throw new NotImplementedException();
        }
    }
}
