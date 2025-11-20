using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class RequestForgotPasswordCommand : CommandBase
    {
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        public RequestForgotPasswordCommand(AuthService authService, NavigationService navigationService)
        {
            try
            {
                _authService = authService;
                _navigationService = navigationService;
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
                _authService.ForgotPasswordRequest();

                _navigationService.NavigateToLogin(false);
            }
            catch (Exception)
            {

                throw;
            }

        }


    }
}
