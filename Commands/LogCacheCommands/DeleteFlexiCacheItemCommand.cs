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

        public DeleteFlexiCacheItemCommand(LogCacheViewModel viewModel, DataService dataService)
        {
            try
            {
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _dataService = dataService ?? throw new ArgumentNullException( nameof(dataService));
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
                // Send data to Firebase first
                var item = parameter as FlexiLOGViewModel;
                FlexiViewModel list = (FlexiViewModel)_viewModel;

                var isLogged = await _dataService.StoreFlexibleLog(item);

                // Remove item
                if(isLogged)
                    list.CacheItems.Remove(item);
            }
            catch (Exception e)
            {
                // TODO
            }
        }
    }
}
