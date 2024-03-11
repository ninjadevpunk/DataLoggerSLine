using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteFlexiCacheItemCommand : CommandBase
    {
        private readonly LogCacheViewModel _viewModel;

        public DeleteFlexiCacheItemCommand(LogCacheViewModel viewModel)
        {
            try
            {
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
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
                var item = parameter as FlexiLOGViewModel;
                FlexiViewModel list = (FlexiViewModel)_viewModel;

                list.CacheItems.Remove(item);
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}
