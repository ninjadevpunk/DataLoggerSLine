using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteGraphicsCacheItemCommand : CommandBase
    {
        private readonly LogCacheViewModel _viewModel;

        public DeleteGraphicsCacheItemCommand(LogCacheViewModel logCacheViewModel)
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
                var item = parameter as GraphicsLOGViewModel;
                GraphicsViewModel list = (GraphicsViewModel)_viewModel;

                list.CacheItems.Remove(item);
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}
