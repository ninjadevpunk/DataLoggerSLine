using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteQtCacheItemCommand : CommandBase
    {
        private readonly LogCacheViewModel _viewModel;
        private readonly DataService _dataService;

        public DeleteQtCacheItemCommand(LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            try
            {
                _viewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
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
                CodingQtViewModel list = (CodingQtViewModel)_viewModel;
                var item = parameter as QtLOGViewModel;

                var isLogged = await _dataService.StoreQtCodingLog(item);

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
