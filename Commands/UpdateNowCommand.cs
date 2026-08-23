using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Windows;
using Velopack;

namespace Data_Logger_1._3.Commands
{
    public class UpdateNowCommand : AsyncCommandBase
    {
        private readonly SettingsService _settingsService;
        private readonly Settings _settings;
        private readonly IDataService _dataService;
        private readonly VelopackService _velopackService;
        private readonly UpdateInfo _updateInfo;


        public UpdateNowCommand(IDataService dataService, SettingsService settingsService, Settings settings, VelopackService velopackService, UpdateInfo updateInfo)
        {
            _dataService = dataService;
            _settingsService = settingsService;
            _settings = settings;
            _velopackService = velopackService;
            _updateInfo = updateInfo;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _velopackService.DownloadUpdateAsync(_updateInfo);
                await _dataService.SignOutUser();
                _velopackService.ApplyUpdateAndRestart(_updateInfo);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, "Exception occurred in UpdateNowCommand.ExecuteAsync");
                MessageBox.Show("An unexpected error occurred while trying to update the application. We apologise for any inconvenience caused. Please try again later.", 
                    "Update Failed", MessageBoxButton.OK, MessageBoxImage.Error);

                System.Diagnostics.Process.Start(Environment.ProcessPath!);
                Application.Current.Shutdown();
            }

        }
    }
}
