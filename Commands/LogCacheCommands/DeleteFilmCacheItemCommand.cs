using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteFilmCacheItemCommand : CommandBase
    {
        private readonly LogCacheViewModel _viewModel;
        private readonly DataService _dataService;
        private readonly bool _timeUp;

        public DeleteFilmCacheItemCommand(LogCacheViewModel logCacheViewModel, DataService dataService, bool timeUp)
        {
            try
            {
                _viewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _timeUp = timeUp;
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                FilmViewModel FilmDashboard = (FilmViewModel)_viewModel;
                var item = parameter as FilmLOGViewModel;
                bool isLogged = false;

                if (FilmDashboard is not null && item is not null)
                {
                    var tempFilmDashboardItems = FilmDashboard.CacheItems;

                    // Remove item
                    if (_timeUp)
                    {
                        // Send data to Firebase first
                        isLogged = _dataService.StoreLog(item._FilmLOG);

                        if (isLogged)
                        {
                            tempFilmDashboardItems.Remove(item);
                            FilmDashboard.CacheItems = tempFilmDashboardItems;
                        }
                    }
                    else
                    {
                        tempFilmDashboardItems.Remove(item);
                        FilmDashboard.CacheItems = tempFilmDashboardItems;
                    }

                }
            }
            catch (Exception e)
            {
                // TODO
            }
        }
    }
}
