using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class CreateLogCommand : CommandBase
    {
        private readonly NavigationService _navigationService;

        public CreateLogCommand(NavigationService navigationService)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

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
                _navigationService.NavigateToLoggerCreator();
            }
            catch (Exception)
            {

                //
            }

        }
    }
}
