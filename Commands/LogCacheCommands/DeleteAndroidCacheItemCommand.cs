using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteAndroidCacheItemCommand : AsyncCommandBase
    {
        private readonly LogCacheViewModel _viewModel;
        private readonly DataService _dataService;
        private readonly bool _timeUp;

        public DeleteAndroidCacheItemCommand(LogCacheViewModel logCacheViewModel, DataService dataService, bool timeIsUp)
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

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                CodingAndroidViewModel ASDashboard = (CodingAndroidViewModel)_viewModel;
                var item = parameter as AndroidLOGViewModel;
                bool isLogged = false;

                if (ASDashboard is not null && item is not null)
                {
                    var tempASDashboardItems = ASDashboard.CacheItems;

                    // Remove item
                    if (_timeUp)
                    {
                        // Send data to database first
                        isLogged = await _dataService.StoreLog(item._AndroidCodingLOG);

                        if (isLogged)
                        {
                            tempASDashboardItems.Remove(item);
                            ASDashboard.CacheItems = tempASDashboardItems;
                        }
                    }
                    else
                    {
                        _dataService.DeleteCacheFile(item.StartAsID, CacheContext.AndroidStudio);
                        item._timer.Dispose();
                        tempASDashboardItems.Remove(item);
                        ASDashboard.CacheItems = tempASDashboardItems;
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
