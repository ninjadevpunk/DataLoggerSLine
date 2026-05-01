using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.IO;
using System.Windows;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class SaveSettingsProfilePicCommand : AsyncCommandBase
    {
        private readonly AuthService _authService;
        private readonly IDataService _dataService;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public SaveSettingsProfilePicCommand(AuthService authService, IDataService dataService, SettingsViewModel settingsViewModel, 
            MainWindowViewModel mainWindowViewModel)
        {
            _authService = authService;
            _dataService = dataService;
            _settingsViewModel = settingsViewModel;
            _mainWindowViewModel = mainWindowViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                try
                {
                    var dialog = new Microsoft.Win32.OpenFileDialog();
                    dialog.DefaultExt = ".png";
                    dialog.Filter = "Portable Network Graphics (.png)|*.png|JPEG Images (.jpg)|*.jpg;";

                    // Show open file dialog box
                    bool? result = dialog.ShowDialog();

                    // Process open file dialog box results
                    if (result == true)
                    {
                        var fileInfo = new FileInfo(dialog.FileName);

                        // Limit to 2MB
                        if (fileInfo.Length > 2 * 1024 * 1024)
                        {
                            MessageBox.Show("Image is too large.");
                            return;
                        }

                        // Save resized image to AppData
                        string optimized = BitmapService.SaveResizedImage(dialog.FileName);

                        _settingsViewModel.DisplayPicPath = optimized;
                        _settingsViewModel.SignUpImage = BitmapService.LoadImage(optimized);

                        // Update MainWindow profile picture
                        _mainWindowViewModel.SignUpImage = BitmapService.LoadImage(optimized);

                        if (_authService?.Account != null)
                            _authService.Account.ProfilePic = optimized;

                        // Delete old profile pic

                    }


                }
                catch (Exception)
                {
                    _settingsViewModel.DefaultPicVisibility = Visibility.Visible;


                    _settingsViewModel.SignUpImage = BitmapService.LoadImage("/Assets/login/user.png");

                    if (_authService?.Account != null)
                        _authService.Account.ProfilePic = "";
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"Exception occurred in SaveSettingsProfilePicCommand.ExecuteAsync(): {ex.Message}");
            }
        }
    }
}
