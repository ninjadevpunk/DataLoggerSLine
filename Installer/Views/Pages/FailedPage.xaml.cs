using System.Windows.Controls;

namespace Installer.Views.Pages
{
    /// <summary>
    /// Interaction logic for FailedPage.xaml
    /// </summary>
    public partial class FailedPage : UserControl
    {
        private readonly MainWindow _mainWindow;
        private readonly bool _isSecondFailure;

        public FailedPage(MainWindow mainWindow, string errorMessage = "", bool isSecondFailure = false)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _isSecondFailure = isSecondFailure;

            if (isSecondFailure)
            {
                this.textBlock_RETRY.Text = "Exit";
                this.textBlock_ERROR_MESSAGE.Text = "An unexpected error has continued to occur and we apologise for any inconvenience.";
            }
            else
                this.textBlock_ERROR_MESSAGE.Text = string.IsNullOrEmpty(errorMessage)
                    ? "An unexpected error occurred and we apologise for any inconvenience. Please try installing again at another time or click retry below."
                    : errorMessage;
        }

        private void on_RETRY_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if(_isSecondFailure)
            {
                _mainWindow.Close();
                return;
            }

            _mainWindow.transit_TRANSITIONER.Content = new SetupPage(_mainWindow, true);
        }
    }
}
