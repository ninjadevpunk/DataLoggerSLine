using Data_Logger_1._3.Commands.ASCommands;
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
                LogCount = CacheItems.Count.ToString() + " android logs cached | x total logs";
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

    }
}
