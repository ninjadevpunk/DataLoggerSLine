using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class DisplayPicCommand : AsyncCommandBase
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

        protected override Task ExecuteAsync(object parameter)
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
                    _signUpViewModel.SignUpImage = dialog.FileName;
                }


                return Task.CompletedTask;
            }
            catch (Exception)
            {

                _signUpViewModel.SignUpImage = @"C:\Users\STUDIO\OneDrive\Pictures\Application Icons\png";
            }

            return Task.CompletedTask;
        }
    }
}
