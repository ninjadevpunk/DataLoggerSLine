using Data_Logger_1._3.Services;
using System.Windows;
using System.Windows.Media;

namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly Brush? enabledColor = TryParseBrush("iconCOLOURAccent02");
        private readonly Brush? disabledColor = TryParseBrush("AccentColour");
        private readonly NavigationService _navigationService;


        public Login()
        {
            InitializeComponent();
        }

        public Login(NavigationService navigationService)
        {
            InitializeComponent();

            _navigationService = navigationService;
        }

        public void Navigator(string pageUri)
        {
            frame_LOGIN.NavigationService.Navigate(new Uri(pageUri, UriKind.Relative));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Calculate the new left position to center the window horizontally
            double windowWidth = this.Width;
            double newLeft = (SystemParameters.PrimaryScreenWidth - windowWidth) / 2.0;

            // Calculate the new top position to center the window vertically
            double windowHeight = this.Height;
            double newTop = (SystemParameters.PrimaryScreenHeight - windowHeight) / 2.0;

            // Set the new left and top positions
            this.Left = newLeft;
            this.Top = newTop;
        }






        private void on_MINIMISE_clicked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void on_CLOSE_clicked(object sender, RoutedEventArgs e)
        {
            // Close the entire application
            Application.Current.Shutdown();
        }

        private void on_Navigation(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            UpdateNavigationButtons();
        }

        private void UpdateNavigationButtons()
        {
            button_BACK.IsEnabled = frame_LOGIN.NavigationService.CanGoBack;            
            button_FORWARD.IsEnabled = frame_LOGIN.NavigationService.CanGoForward;

            icon_BACK.Fill = button_BACK.IsEnabled ? enabledColor! : disabledColor!;
            icon_FORWARD.Fill = button_FORWARD.IsEnabled ? enabledColor! : disabledColor!;
        }

        private void on_BACK_clicked(object sender, RoutedEventArgs e)
        {
            if (frame_LOGIN.NavigationService?.CanGoBack ?? false)
                frame_LOGIN.NavigationService?.GoBack();
        }

        private void on_FORWARD_clicked(object sender, RoutedEventArgs e)
        {
            if (frame_LOGIN.NavigationService?.CanGoForward ?? false)
                frame_LOGIN.NavigationService?.GoForward();
        }

        private static Brush? TryParseBrush(string value)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            try
            {
                return (Brush)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return Brushes.Transparent;
            }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
    }
}
