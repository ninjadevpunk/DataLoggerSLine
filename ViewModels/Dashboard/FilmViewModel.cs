using Data_Logger_1._3.Commands.FilmCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class FilmViewModel : LogCacheViewModel
    {
        private ObservableCollection<FilmLOGViewModel> cacheItems;
        public ObservableCollection<FilmLOGViewModel> CacheItems
        {
            get
            {
                return cacheItems;
            }
            set
            {
                cacheItems = value;
                LogCount = CacheItems.Count.ToString() + " film logs cached | x total logs";
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public FilmViewModel(NavigationService navigationService, DataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<FilmLOGViewModel>();

            CreateLogCommand = new CreateFilmLogCommand(_navigationService);
            ReportLogCommand = new ReportFilmLogCommand(_navigationService);
        }

        public FilmViewModel(string logCount, NavigationService navigationService, DataService _dataService) : base(navigationService, _dataService)
        {
            CacheItems = new ObservableCollection<FilmLOGViewModel>();

            LogCount = logCount;

            CreateLogCommand = new CreateFilmLogCommand(_navigationService);
            ReportLogCommand = new ReportFilmLogCommand(_navigationService);
        }

    }
}
