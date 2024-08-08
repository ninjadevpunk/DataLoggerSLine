using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    public class ClearPostItListCommand : CommandBase
    {
        private readonly LoggerCreateViewModel _viewModel;

        public ClearPostItListCommand(LoggerCreateViewModel viewModel)
        {
            try
            {
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public ClearPostItListCommand()
        {

        }

        public override void Execute(object parameter)
        {
            try
            {
                _viewModel.PostIts.Clear();
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
