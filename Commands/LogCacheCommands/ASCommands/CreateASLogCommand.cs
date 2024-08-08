using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands.ASCommands
{
    public class CreateASLogCommand : CommandBase
    {
        private readonly NavigationService _navigationService;

        public CreateASLogCommand(NavigationService navigationService)
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
