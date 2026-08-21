using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class DeleteAccountCommand : AsyncCommandBase
    {
        private readonly AuthService _authService;
        private readonly IDataService _dataService;


        public DeleteAccountCommand(AuthService authService, IDataService dataService)
        {
            _authService = authService;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var result = MessageBox.Show("Are you sure you would like to delete your account? This action cannot be undone and you will be logged out automatically.", 
                    "Confirm Account Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var accountDeleted = await _authService.DeleteAccountAsync();

                    if(accountDeleted)
                    {
                        Application.Current.Shutdown();
                        return;
                    }

                    MessageBox.Show("An unexpected error occurred and we were unable to delete your account. Please try again later.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"Exception occurred in DeleteAccountCommand.ExecuteAsync(): {ex.Message}");
            }
        }
    }
}
