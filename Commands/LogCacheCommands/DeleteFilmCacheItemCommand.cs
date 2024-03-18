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

        public DeleteFilmCacheItemCommand(LogCacheViewModel logCacheViewModel)
        {
            try
            {
                _viewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));

            }
            catch (Exception)
            {
                //
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                // Send data to Firebase first
                // TODO


                // Remove item
                var item = parameter as FilmLOGViewModel;
                FilmViewModel list = (FilmViewModel)_viewModel;

                list.CacheItems.Remove(item);
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}
