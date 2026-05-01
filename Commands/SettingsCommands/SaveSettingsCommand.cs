using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class SaveSettingsCommand : AsyncCommandBase
    {
        private readonly IDataService _dataService;
        private readonly SettingsService _settingsService;
        private readonly BitmapService _bitmapService;
        private readonly Settings _settings;

        public SaveSettingsCommand(IDataService dataService, SettingsService settingsService, BitmapService bitmapService, Settings settings)
        {
            _settingsService = settingsService;
            _dataService = dataService;
            _bitmapService = bitmapService;
            _settings = settings;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // Update user details in the database


                // Save new dp if profile pic has changed


                // Save new settings to JSON
                _settingsService.Save(_settings.User.Id, _settings);

            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, $"Exception occurred in SaveSettingsCommand.ExecuteAsync(): {ex.Message}");

                // Delete new profile pic if needed

                // Revert logic if needed

            }
        }
    }
}
