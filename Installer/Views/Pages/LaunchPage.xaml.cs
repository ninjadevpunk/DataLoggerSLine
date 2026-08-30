using System.Diagnostics;
using System.IO;
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




        private void on_FINISH_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            // Start Data Logger if Launch checkbox is checked
            if (checkBox_LAUNCH.IsChecked == true)
            {
                try
                {
                    string installPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Data Logger");
                    string exePath = Path.Combine(installPath, "Data Logger.exe");

                    if (File.Exists(exePath))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = exePath,
                            UseShellExecute = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to launch Data Logger: {ex.Message}");
                }
            }

            _mainWindow.Close();
        }

    }
}
