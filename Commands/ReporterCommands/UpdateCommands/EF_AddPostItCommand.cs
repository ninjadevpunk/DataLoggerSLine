using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class EF_AddPostItCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly ReporterUpdaterViewModel _reporterUpdaterViewModel;

        public EF_AddPostItCommand(NavigationService navigationService, ReporterUpdaterViewModel reporterUpdaterViewModel)
        {


            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _reporterUpdaterViewModel = reporterUpdaterViewModel ?? throw new ArgumentNullException(nameof(reporterUpdaterViewModel));
            }
            catch (Exception)
            {
                //
            }


        }

        public override void Execute(object parameter)
        {
            //_navigationService.NavigateToReporterPostItCreator(_reporterUpdaterViewModel);
        }
    }
}
