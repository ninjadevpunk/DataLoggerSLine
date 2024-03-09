using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Windows;
using System.Security;

namespace Data_Logger_1._3.Commands
{
    public class EmailSignUpCommand : AsyncCommandBase
    {


        private readonly SignUpViewModel _signUpViewModel;
        private readonly AuthService _authService;

        public EmailSignUpCommand(SignUpViewModel signUpViewModel)
        {
            _signUpViewModel = signUpViewModel;
        }

        public EmailSignUpCommand(SignUpViewModel signUpViewModel, AuthService authService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // Retrieve user input from the view model
                string email = _signUpViewModel.Email;
                SecureString password = _signUpViewModel.Password;
                string displayName = _signUpViewModel.Name;

                // Call the SignUp method in AuthService to handle user registration
                var isSignedUp = _authService.SignUp(email, password, displayName);

                if(isSignedUp)
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred on our side. We apologise for any inconvenience. Feedback will be automatically sent to our support desk.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
