using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class LoginCommand : AsyncCommandBase
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

        protected override async Task ExecuteAsync(object parameter)
        {           

            try
            {
                bool signInResult = await Task.Run(() => _authService.SignIn(_loginViewModel.Username, _loginViewModel.Password));

                if (signInResult)
                {
                    _navigationService.NavigateToMainWindow();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
