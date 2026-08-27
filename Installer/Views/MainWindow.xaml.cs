using Installer.Views.Pages;
using System.Windows;

namespace Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.transit_TRANSITIONER.Content = new IntroPage(this);
        }

        private void on_MINIMISE_clicked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void on_CLOSE_clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void on_customTitleBar_pressed(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        internal void ShowEULAPage()
        {
            this.transit_TRANSITIONER.Content = new EULAPage(this);
        }

        internal void ShowSetupPage()
        {
            this.transit_TRANSITIONER.Content = new SetupPage(this);
        }

        internal void ShowInstallingPage()
        {
            this.transit_TRANSITIONER.Content = new InstallingPage(this);
        }

        internal void ShowLaunchPage()
        {
            this.transit_TRANSITIONER.Content = new LaunchPage(this);
        }

        internal void ShowFailedPage(string errorMessage = "", bool isSecondFailure = false)
        {
            if (string.IsNullOrEmpty(errorMessage))
                this.transit_TRANSITIONER.Content = new FailedPage(this, isSecondFailure: isSecondFailure);
            else
                this.transit_TRANSITIONER.Content = new FailedPage(this, errorMessage, isSecondFailure: isSecondFailure);
        }
    }
}