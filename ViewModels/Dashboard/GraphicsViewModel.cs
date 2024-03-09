using Data_Logger_1._3.Commands.GraphicsCommands;
using Data_Logger_1._3.Messages;
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
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public GraphicsViewModel(NavigationService navigationService) : base(navigationService)
        {
            CreateLogCommand = new CreateGraphicsLogCommand(_navigationService);
            ReportLogCommand = new ReportGraphicsLogCommand(_navigationService);
        }


        public GraphicsViewModel(string logCount, NavigationService navigationService) : base(navigationService)
        {
            LogCount = logCount;


            CreateLogCommand = new CreateGraphicsLogCommand(_navigationService);
            ReportLogCommand = new ReportGraphicsLogCommand(_navigationService);

        }

        public override void RemoveItemMethod(RemoveItemMessage item)
        {
            throw new NotImplementedException();
        }
    }
}
