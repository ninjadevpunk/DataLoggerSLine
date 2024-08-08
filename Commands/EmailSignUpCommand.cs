using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    public class EmailSignUpCommand : CommandBase
    {


        private readonly SignUpViewModel _signUpViewModel;
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        public EmailSignUpCommand(SignUpViewModel signUpViewModel)
        {
            _signUpViewModel = signUpViewModel;
        }

        public EmailSignUpCommand(SignUpViewModel signUpViewModel, AuthService authService, NavigationService navigationService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            try
            {
                // Retrieve user input from the view model
                string email = _signUpViewModel.Email;
                string password = _signUpViewModel.Password;
                string displayName = _signUpViewModel.Name;
                string surname = _signUpViewModel.Surname;
                var IsHired = _signUpViewModel.YesBox;
                var company = _signUpViewModel.CompanyName;
                var address = _signUpViewModel.CompanyAddress;
                var logo = _signUpViewModel.CompanyLogo;

                _authService.Account.ProfilePic = _signUpViewModel.SignUpImage;

                // Call the SignUp method in AuthService to handle user registration
                var isSignedUp = _authService.SignUp(email, password, displayName, surname, IsHired, company, address, logo);

                if (isSignedUp)
                {
                    _navigationService.NavigateToMainWindow();
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
