using Data_Logger_1._3.Commands.FilmCommands;
using Data_Logger_1._3.Messages;
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
                OnPropertyChanged(nameof(CacheItems));
            }
        }


        public FilmViewModel(NavigationService navigationService) : base(navigationService)
        {
            CreateLogCommand = new CreateFilmLogCommand(_navigationService);
            ReportLogCommand = new ReportFilmLogCommand(_navigationService);
        }

        public FilmViewModel(string logCount, NavigationService navigationService) : base(navigationService)
        {
            LogCount = logCount;

            CreateLogCommand = new CreateFilmLogCommand(_navigationService);
            ReportLogCommand = new ReportFilmLogCommand(_navigationService);
        }

        public override void RemoveItemMethod(RemoveItemMessage item)
        {
            throw new NotImplementedException();
        }
    }
}
