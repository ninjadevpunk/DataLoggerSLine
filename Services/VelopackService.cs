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
            return await _updateManager.CheckForUpdatesAsync();
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
