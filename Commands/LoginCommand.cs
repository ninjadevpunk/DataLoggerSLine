using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class LoginCommand : CommandBase
    {

        private readonly LoginViewModel _loginViewModel;
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;


        public LoginCommand(LoginViewModel loginViewModel, AuthService authService, NavigationService navigationService)
        {
            _loginViewModel = loginViewModel;
            _authService = authService;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {

            try
            {
                bool SignInSuccessful = _authService.SignIn(_loginViewModel.Username, _loginViewModel.Password);

                if (SignInSuccessful)
                {
                    _navigationService.NavigateToMainWindow();
                }

            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
