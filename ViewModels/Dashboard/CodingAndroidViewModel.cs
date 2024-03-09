using Data_Logger_1._3.Commands.ASCommands;
using Data_Logger_1._3.Messages;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;


namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class CodingAndroidViewModel : LogCacheViewModel
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
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public CodingAndroidViewModel(NavigationService navigationService) : base(navigationService)
        {
            CreateLogCommand = new CreateASLogCommand(_navigationService);
            ReportLogCommand = new ReportASLogCommand(_navigationService);
        }

        public CodingAndroidViewModel(string logCount, NavigationService navigationService) : base(navigationService)
        {

            LogCount = logCount;

            CreateLogCommand = new CreateASLogCommand(_navigationService);
            ReportLogCommand = new ReportASLogCommand(_navigationService);
        }

        public override void RemoveItemMethod(RemoveItemMessage item)
        {
            throw new NotImplementedException();
        }
    }
}
