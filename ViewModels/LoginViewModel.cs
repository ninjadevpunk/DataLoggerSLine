
using System.Windows.Media;
using System.Windows;

namespace Data_Logger_1._3.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
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
                OnPropertyChanged(nameof(IconBackFill));
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
                OnPropertyChanged(nameof(IconForwardFill));
            }
        }

        public Brush? IconBackFill => IsButtonBackEnabled ? EnabledColor : DisabledColor;
        public Brush? IconForwardFill => IsButtonForwardEnabled ? EnabledColor : DisabledColor;

        public LoginViewModel()
        {
            // Set initial button states
            IsButtonBackEnabled = true;
            IsButtonForwardEnabled = false;
        }

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
