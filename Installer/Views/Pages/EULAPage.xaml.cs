using System.Windows.Controls;

namespace Installer.Views.Pages
{
    /// <summary>
    /// Interaction logic for EULAPage.xaml
    /// </summary>
    public partial class EULAPage : UserControl
    {
        private readonly MainWindow _mainWindow;
        public EULAPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }
    }
}
