using System.Windows.Controls;

namespace Installer.Views.Pages
{
    /// <summary>
    /// Interaction logic for IntroPage.xaml
    /// </summary>
    public partial class IntroPage : UserControl
    {
        private readonly MainWindow _mainWindow;

        public IntroPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void on_NEXT_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.ShowSetupPage();
        }
    }
}
