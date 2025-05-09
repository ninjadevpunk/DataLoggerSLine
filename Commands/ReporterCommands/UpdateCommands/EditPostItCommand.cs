using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class EditPostItCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly ReporterUpdaterViewModel _viewModel;

        public EditPostItCommand(NavigationService navigationService, ReporterUpdaterViewModel reporterEditorViewModel)
        {


            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewModel = reporterEditorViewModel ?? throw new ArgumentNullException(nameof(reporterEditorViewModel));
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
                //_navigationService.NavigateToReporterPostItEditor(_viewModel, parameter as CreateReporterPostItViewModel);
            }
            catch (ArgumentNullException nullex)
            {
                //
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
