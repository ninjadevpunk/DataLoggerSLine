using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows.Input;
using Velopack;

namespace Data_Logger_1._3.ViewModels
{
    public class UpdaterViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly SettingsService _settingsService;
        private Settings _settings;
        private readonly UpdateInfo _updateInfo;
        public ICommand UpdateNowCommand { get; set; }
        public ICommand CancelUpdateCommand { get; set; }

        public UpdaterViewModel(IDataService dataService, AppSettingsService appSettingsService, SettingsService settingsService, VelopackService velopackService, 
            UpdateInfo updateInfo)
        {
            _dataService = dataService;
            _updateInfo = updateInfo;

            _settingsService = settingsService;
            _settings = _settingsService.Load(UserID) ?? new Settings();

            UpdateNowCommand = new UpdateNowCommand(dataService, _settingsService, _settings, velopackService, updateInfo);
            CancelUpdateCommand = new CancelUpdateCommand(dataService);
        }





        #region Properties



        public string UpdateVersion => $"Latest: {_updateInfo.TargetFullRelease.Version}";

        private bool showUpdatePopup;
        public bool ShowUpdatePopup
        {
            get => showUpdatePopup;
            private set
            {
                if (showUpdatePopup == value)
                    return;

                showUpdatePopup = value;
                _settings.ShowUpdatePopup = value;
                SaveSettings();
                OnPropertyChanged(nameof(ShowUpdatePopup));
            }
        }



        #endregion







        #region Methods



        private int UserID => _dataService.GetUser().accountID;

        private void SaveSettings()
        {
            _settingsService.Save(UserID, _settings);
        }





        #endregion
    }
}
