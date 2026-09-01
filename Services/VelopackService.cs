using System.Diagnostics;
using Velopack;
using Velopack.Sources;

namespace Data_Logger_1._3.Services
{
    public class VelopackService
    {
        private readonly UpdateManager _updateManager;

        public VelopackService()
        {
            _updateManager = new UpdateManager(new GithubSource("https://github.com/ninjadevpunk/DataLoggerSLine", accessToken: null, prerelease: true));
        }

        public async Task<UpdateInfo?> CheckForUpdatesAsync()
        {
            try
            {
                return await _updateManager.CheckForUpdatesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to check for updates: {ex.Message}");
                return null;
            }
        }

        public async Task DownloadUpdateAsync(UpdateInfo updateInfo)
        {
            await _updateManager.DownloadUpdatesAsync(updateInfo);
        }

        public void ApplyUpdateAndRestart(UpdateInfo updateInfo)
        {
            _updateManager.ApplyUpdatesAndRestart(updateInfo);
        }

        
    }
}
