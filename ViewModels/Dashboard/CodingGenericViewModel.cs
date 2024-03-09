using Data_Logger_1._3.Commands.CodingCommands;
using Data_Logger_1._3.Messages;
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
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public CodingGenericViewModel(NavigationService navigationService) : base(navigationService)
        {
            CreateLogCommand = new CreateCodingLogCommand(_navigationService);
            ReportLogCommand = new ReportCodingLogCommand(_navigationService);
        }

        public CodingGenericViewModel(string logCount, NavigationService navigationService) : base(navigationService)
        {
            LogCount = logCount;

            CreateLogCommand = new CreateCodingLogCommand(_navigationService);
            ReportLogCommand = new ReportCodingLogCommand(_navigationService);
        }

        public override void RemoveItemMethod(RemoveItemMessage item)
        {
            throw new NotImplementedException();
        }
    }
}
