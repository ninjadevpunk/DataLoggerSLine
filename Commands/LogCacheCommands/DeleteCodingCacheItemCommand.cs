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

        public override async void Execute(object parameter)
        {
            try
            {
                var item = parameter as CodeLOGViewModel;
                CodingGenericViewModel list = (CodingGenericViewModel)_viewModel;
                bool isLogged = false;

                if (list is not null && item is not null)
                {
                    var temp = list.CacheItems;

                    // Remove item
                    if (_timeUp)
                    {
                        // Send data to Firebase first
                        isLogged = await _dataService.StoreCodingLog(item._CodeLOG);

                        if (isLogged)
                        {
                            temp.Remove(item);
                            list.CacheItems = temp;
                        }
                    }
                    else
                    {
                        temp.Remove(item);
                        list.CacheItems = temp;
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
