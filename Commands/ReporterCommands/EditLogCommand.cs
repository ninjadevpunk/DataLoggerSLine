using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Allows the user to edit a log retrieved from the database.
    /// </summary>
    public class EditLogCommand : CommandBase
    {
        private readonly REPORTViewModel _viewModel;
        private readonly ReportDeskViewModel _reportDeskViewModel;
        private readonly NavigationService _navigationService;

        public EditLogCommand()
        {

        }

        public EditLogCommand(REPORTViewModel reportViewModel, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService)
        {

        }

        public override void Execute(object parameter)
        {
            try
            {
                //_navigationService.NavigateToReporterUpdater(_viewModel, _reportDeskViewModel);


            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occurred near EditCommand.Execute(): {e.Message}");
            }
        }
    }

}
