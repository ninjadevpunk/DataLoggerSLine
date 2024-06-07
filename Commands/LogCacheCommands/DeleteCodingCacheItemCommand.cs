using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteCodingCacheItemCommand : CommandBase
    {
        private readonly LogCacheViewModel _viewModel;
        private readonly DataService _dataService;
        private readonly bool _timeUp;

        public DeleteCodingCacheItemCommand(LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            try
            {
                _viewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException( nameof(dataService));
                _timeUp = true;
            }
            catch (Exception)
            {
                // TODO
            }
            
        }

        public DeleteCodingCacheItemCommand(LogCacheViewModel logCacheViewModel, DataService dataService, bool timeIsUp)
        {
            try
            {
                _viewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _timeUp = timeIsUp;
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
                CodingGenericViewModel CodingDashboard = (CodingGenericViewModel)_viewModel;
                var item = parameter as CodeLOGViewModel;
                bool isLogged = false;

                if (CodingDashboard is not null && item is not null)
                {
                    var tempCodingDashboardItems = CodingDashboard.CacheItems;

                    // Remove item
                    if (_timeUp)
                    {
                        // Send data to Firebase first
                        isLogged = _dataService.StoreLog(item._CodeLOG);

                        if (isLogged)
                        {
                            tempCodingDashboardItems.Remove(item);
                            CodingDashboard.CacheItems = tempCodingDashboardItems;
                        }
                    }
                    else
                    {
                        tempCodingDashboardItems.Remove(item);
                        CodingDashboard.CacheItems = tempCodingDashboardItems;
                    }

                }
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
