using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteFlexiCacheItemCommand : CommandBase
    {
        private readonly LogCacheViewModel _viewModel;
        private readonly DataService _dataService;
        private readonly bool _timeUp;

        public DeleteFlexiCacheItemCommand(LogCacheViewModel viewModel, DataService dataService, bool timeIsUp)
        {
            try
            {
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _dataService = dataService ?? throw new ArgumentNullException( nameof(dataService));
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
                FlexiViewModel list = (FlexiViewModel)_viewModel;
                var item = parameter as FlexiLOGViewModel;
                bool isLogged = false;

                if (list is not null && item is not null)
                {
                    var temp = list.CacheItems;

                    // Remove item
                    if (_timeUp)
                    {
                        // Send data to Firebase first
                        isLogged = await _dataService.StoreFlexibleLog(item._FlexiLOG);

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
            catch (Exception e)
            {
                // TODO
            }
        }
    }
}
