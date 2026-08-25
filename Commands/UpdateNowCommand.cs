using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Windows;
using Velopack;

namespace Data_Logger_1._3.Commands
{
    public class UpdateNowCommand : AsyncCommandBase
    {
        private readonly IDataService _dataService;
        private readonly VelopackService _velopackService;
        private readonly UpdateInfo _updateInfo;
        private readonly Task _waitForUserRestartTask;


        public UpdateNowCommand(IDataService dataService, VelopackService velopackService, UpdateInfo updateInfo)
        {
            _dataService = dataService;
            _velopackService = velopackService;
            _updateInfo = updateInfo;
            _waitForUserRestartTask = Task.CompletedTask;
        }

        public UpdateNowCommand(IDataService dataService, VelopackService velopackService, UpdateInfo updateInfo, Task waitForUserRestartTask)
        {
            _dataService = dataService;
            _velopackService = velopackService;
            _updateInfo = updateInfo;
            _waitForUserRestartTask = waitForUserRestartTask;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _velopackService.DownloadUpdateAsync(_updateInfo);

                if (parameter is bool isManualDownload)
                {
                    // Install Now Button
                    if(isManualDownload)
                        await _waitForUserRestartTask;
                }

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
