using System.Windows.Controls;

namespace Installer.Views.Pages
{
    /// <summary>
    /// Interaction logic for InstallingPage.xaml
    /// </summary>
    public partial class InstallingPage : UserControl
    {
        private readonly MainWindow _mainWindow;

        public InstallingPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }
    }
}
