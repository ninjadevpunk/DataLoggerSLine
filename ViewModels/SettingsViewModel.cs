using Data_Logger_1._3.Commands.SettingsCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsService _settingsService;
        private Settings _settings;
        public ICommand SaveSettingsCommand;
        public ICommand OpenImageCommand { get; set; }
        public ICommand PasswordResetCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }
        public ICommand ReturnToDashboardCommand { get; set; }

        public SettingsViewModel(AuthService authService, IDataService dataService, SettingsService settingsService, BitmapService bitmapService,
            MainWindowViewModel mainWindowViewModel)
        {
            var id = dataService.GetUser().accountID;

            _settingsService = settingsService;
            _settings = _settingsService.Load(id);

            var author = _settings.User;
            author.Id = id;

            SignUpImage = BitmapService.LoadImage(author.ProfilePic) ?? null;
            DisplayPicPath = author.ProfilePic;
            DefaultPicVisibility = SignUpImage == null ? Visibility.Visible : Visibility.Collapsed;

            Name = author.Name;
            Surname = author.Surname;
            Email = author.Email;
            IsCompanyEmployee = author.IsCompanyEmployee;

            if (IsCompanyEmployee)
            {
                CompanyName = author.CompanyName;
                CompanyAddress = author.CompanyAddress;
            }

            // AlphaBeta builds are Grey themed only
            // TODO
            Theme = "Grey";

            OpenImageCommand = new SaveSettingsProfilePicCommand(authService, dataService, this, mainWindowViewModel);
            // Request password reset command here...
            // Delete account command here...
            SaveSettingsCommand = new SaveSettingsCommand(dataService, settingsService, bitmapService, _settings);
            SaveIsEnabled = SettingsService.FieldsAcceptable(Email, IsCompanyEmployee, CompanyName);
            //Return to dashboard command here...
        }

        private BitmapImage? signUpImage;
        public BitmapImage? SignUpImage
        {
            get
            {
                return signUpImage;
            }
            set
            {
                signUpImage = value;
                DefaultPicVisibility = value == null ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(SignUpImage));
            }
        }

        private string displayPicPath;
        public string DisplayPicPath
        {
            get
            {
                return displayPicPath;
            }
            set
            {
                displayPicPath = value;
                _settings.User.ProfilePic = value;
                SignUpImage = BitmapService.LoadImage(displayPicPath) ?? null;
                OnPropertyChanged(nameof(DisplayPicPath));
            }
        }

        private Visibility defaultPicVisibility;
        public Visibility DefaultPicVisibility
        {
            get
            {
                return defaultPicVisibility;
            }
            set
            {
                defaultPicVisibility = value;
                OnPropertyChanged(nameof(DefaultPicVisibility));
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                _settings.User.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string surname;
        public string Surname
        {
            get
            {
                return surname;
            }
            set
            {
                surname = value;
                _settings.User.Surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private string email;
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                _settings.User.Email = value;
                SaveIsEnabled = SettingsService.FieldsAcceptable(Email, IsCompanyEmployee, CompanyName);
                OnPropertyChanged(nameof(Email));
            }
        }

        private bool isCompanyEmployee;
        public bool IsCompanyEmployee
        {
            get
            {
                return isCompanyEmployee;
            }
            set
            {
                isCompanyEmployee = value;
                isNotCompanyEmployee = !value;

                _settings.User.IsCompanyEmployee = value;
                SaveIsEnabled = SettingsService.FieldsAcceptable(Email, IsCompanyEmployee, CompanyName);
                OnPropertyChanged(nameof(IsCompanyEmployee));
            }
        }

        private bool isNotCompanyEmployee;
        public bool IsNotCompanyEmployee
        {
            get
            {
                return isNotCompanyEmployee;
            }
            set
            {
                isNotCompanyEmployee = value;
                isCompanyEmployee = !value;

                SaveIsEnabled = SettingsService.FieldsAcceptable(Email, IsCompanyEmployee, CompanyName);
                OnPropertyChanged(nameof(IsNotCompanyEmployee));
            }
        }

        private string? companyName;
        public string? CompanyName
        {
            get
            {
                return companyName;
            }
            set
            {
                companyName = value;
                _settings.User.CompanyName = value;
                SaveIsEnabled = SettingsService.FieldsAcceptable(Email, IsCompanyEmployee, CompanyName);
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private string? companyAddress;
        public string? CompanyAddress
        {
            get
            {
                return companyAddress;
            }
            set
            {
                companyAddress = value;
                _settings.User.CompanyAddress = value;
                OnPropertyChanged(nameof(CompanyAddress));
            }
        }

        // THEME

        private string theme;
        public string Theme
        {
            get
            {
                return theme;
            }
            set
            {
                if (Enum.TryParse<Settings.Theme>(value, true, out var result))
                {
                    _settings.AppTheme = result;
                }
                theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        private bool saveIsEnabled;
        public bool SaveIsEnabled
        {
            get
            {
                return saveIsEnabled;
            }
            set
            {
                saveIsEnabled = value;
                OnPropertyChanged(nameof(SaveIsEnabled));
            }
        }


    }
}
