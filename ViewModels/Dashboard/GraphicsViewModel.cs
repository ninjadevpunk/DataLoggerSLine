using Data_Logger_1._3.Commands.GraphicsCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class GraphicsViewModel : LogCacheViewModel
    {
        private ObservableCollection<GraphicsLOGViewModel> cacheItems;
        public ObservableCollection<GraphicsLOGViewModel> CacheItems
        {
            get
            {
                return cacheItems;
            }
            set
            {
                cacheItems = value;
                LogCount = CacheItems.Count.ToString() + " graphics logs cached | x total logs";
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public GraphicsViewModel(NavigationService navigationService, DataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<GraphicsLOGViewModel>();

            CreateLogCommand = new CreateGraphicsLogCommand(_navigationService);
            ReportLogCommand = new ReportGraphicsLogCommand(_navigationService);
        }


        public GraphicsViewModel(string logCount, NavigationService navigationService, DataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<GraphicsLOGViewModel>();

            LogCount = logCount;


            CreateLogCommand = new CreateGraphicsLogCommand(_navigationService);
            ReportLogCommand = new ReportGraphicsLogCommand(_navigationService);

        }

    }
}
