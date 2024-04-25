
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Commands;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {



        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;





        public LoginViewModel(AuthService authService, NavigationService navigationService)
        {

            _authService = authService;
            _navigationService = navigationService;

            ShowDefault = Visibility.Visible;

            // Set initial button states
            IsButtonBackEnabled = true;
            IsButtonForwardEnabled = false;

            LoginCommand = new LoginCommand(this, _authService, _navigationService);
            GoogleLoginCommand = new GoogleLoginCommand(this, _authService);
        }

        public LoginViewModel(AuthService authService, NavigationService navigationService, string displayPicSource)
        {

            _authService = authService;
            _navigationService = navigationService;

            SignUpImage = displayPicSource;
            ShowDefault = Visibility.Visible;

            // Set initial button states
            IsButtonBackEnabled = true;
            IsButtonForwardEnabled = false;

            LoginCommand = new LoginCommand(this, _authService, _navigationService);
            GoogleLoginCommand = new GoogleLoginCommand(this, _authService);
        }






        /* Member Variables */

        private string signUpImage;
        public string SignUpImage
        {
            get
            {
                return signUpImage;
            }
            set
            {
                signUpImage = value;
                OnPropertyChanged(nameof(SignUpImage));
            }
        }

        private Visibility showDefault;
        public Visibility ShowDefault
        {
            get
            {
                return showDefault;
            }
            set
            {
                showDefault = value;
                //ShowDefault = SignUpImage != "" ? Visibility.Collapsed : Visibility.Visible;
                OnPropertyChanged(nameof(ShowDefault));
            }
        }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }



        private static readonly Brush? EnabledColor = TryParseBrush("iconColourAccent02");
        private static readonly Brush? DisabledColor = TryParseBrush("AccentColour");

        private bool _isButtonBackEnabled;
        public bool IsButtonBackEnabled
        {
            get => _isButtonBackEnabled;
            set
            {
                _isButtonBackEnabled = value;
                OnPropertyChanged(nameof(IsButtonBackEnabled));
            }
        }

        private bool _isButtonForwardEnabled;
        public bool IsButtonForwardEnabled
        {
            get => _isButtonForwardEnabled;
            set
            {
                _isButtonForwardEnabled = value;
                OnPropertyChanged(nameof(IsButtonForwardEnabled));
            }
        }

        public Brush? IconBackFill => IsButtonBackEnabled ? EnabledColor : DisabledColor;
        public Brush? IconForwardFill => IsButtonForwardEnabled ? EnabledColor : DisabledColor;

        public ICommand LoginCommand { get; set; }

        public ICommand GoogleLoginCommand { get; set; }

        public void UpdateNavigationButtons()
        {
            
        }

        private static Brush? TryParseBrush(string value)
        {
            try
            {
                return (Brush)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return Brushes.Transparent;
            }
        }
    }
}
