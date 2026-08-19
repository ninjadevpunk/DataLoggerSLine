using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class ChangePasswordCommand : AsyncCommandBase
    {
        SettingsViewModel _settingsViewModel;
        AuthService _authService;


        public ChangePasswordCommand(SettingsViewModel settingsViewModel, AuthService authService)
        {
            _settingsViewModel = settingsViewModel;
            _authService = authService;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            // Implement the logic to change the password here.
            await _authService.ChangePassword(_settingsViewModel.NewPassword, _settingsViewModel.Email);
        }
    }
}
