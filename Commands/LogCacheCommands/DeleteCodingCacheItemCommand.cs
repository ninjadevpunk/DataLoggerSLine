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

        public DeleteCodingCacheItemCommand(LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            try
            {
                _viewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
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
                var item = parameter as CodeLOGViewModel;
                CodingGenericViewModel list = (CodingGenericViewModel)_viewModel;

                var isLogged = await _dataService.StoreCodingLog(item);

                // Remove item
                if(isLogged)
                    list.CacheItems.Remove(item);
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
