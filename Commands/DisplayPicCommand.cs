using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    public class DisplayPicCommand : CommandBase
    {

        private readonly SignUpViewModel _signUpViewModel;
        private readonly AuthService _authService;

        public DisplayPicCommand(SignUpViewModel signUpViewModel)
        {
            _signUpViewModel = signUpViewModel;
        }

        public DisplayPicCommand(SignUpViewModel signUpViewModel, AuthService authService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
        }

        public override void Execute(object parameter)
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.DefaultExt = ".png"; // Default file extension
                dialog.Filter = "Portable Network Graphics (.png)|*.png|JPEG Images (.jpg)|*.jpg;"; // Filter files by extension

                // Show open file dialog box
                bool? result = dialog.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    _signUpViewModel.ShowDefault = Visibility.Collapsed;
                    _signUpViewModel.SignUpImage = dialog.FileName;
                    _authService.Account.ProfilePic = dialog.FileName;
                }


            }
            catch (Exception)
            {
                _signUpViewModel.ShowDefault = Visibility.Visible;
                _signUpViewModel.SignUpImage = "";
                _authService.Account.ProfilePic = "";
            }

        }
    }
}
