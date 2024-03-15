using System.Collections.ObjectModel;
using Data_Logger_1._3.Commands.FlexiCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public class FlexiViewModel : LogCacheViewModel
    {
        private ObservableCollection<FlexiLOGViewModel> cacheItems;
        public ObservableCollection<FlexiLOGViewModel> CacheItems
        {
            get
            {
                return cacheItems;
            }
            set
            {
                cacheItems = value;
                LogCount = CacheItems.Count.ToString() + " flexible logs cached | x total logs";
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public FlexiViewModel(NavigationService navigationService) : base(navigationService)
        {
            CacheItems = new ObservableCollection<FlexiLOGViewModel>();

            _navigationService = navigationService;

            CreateLogCommand = new CreateFlexiNotesLogCommand(_navigationService);
            ReportLogCommand = new ReportFlexiNotesLogCommand(_navigationService);
        }


        public FlexiViewModel(string logCount, NavigationService navigationService) : base(navigationService)
        {
            CacheItems = new ObservableCollection<FlexiLOGViewModel>();

            _navigationService = navigationService;

            LogCount = logCount;


            CreateLogCommand = new CreateFlexiNotesLogCommand(_navigationService);
            ReportLogCommand = new ReportFlexiNotesLogCommand(_navigationService);
        }

    }
}
