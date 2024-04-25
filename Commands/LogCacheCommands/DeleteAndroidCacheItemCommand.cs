using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteAndroidCacheItemCommand : CommandBase
    {
        private readonly LogCacheViewModel _viewModel;
        private readonly DataService _dataService;

        public DeleteAndroidCacheItemCommand(LogCacheViewModel logCacheViewModel, DataService dataService)
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
                CodingAndroidViewModel list = (CodingAndroidViewModel)_viewModel;
                var item = parameter as AndroidLOGViewModel;

                var isLogged = await _dataService.StoreASCodingLog(item);

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
