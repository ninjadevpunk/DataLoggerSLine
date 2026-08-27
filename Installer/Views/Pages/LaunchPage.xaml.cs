using System.Windows.Controls;

namespace Installer.Views.Pages
{
    /// <summary>
    /// Interaction logic for LaunchPage.xaml
    /// </summary>
    public partial class LaunchPage : UserControl
    {
        private readonly MainWindow _mainWindow;

        public LaunchPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }
    }
}
