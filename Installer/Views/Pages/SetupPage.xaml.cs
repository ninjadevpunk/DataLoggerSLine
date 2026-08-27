using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Installer.Views.Pages
{
    /// <summary>
    /// Interaction logic for SetupPage.xaml
    /// </summary>
    public partial class SetupPage : UserControl
    {
        private readonly MainWindow _mainWindow;
        string installPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Data Logger");
        const int APP_SIZE = 170;
        public bool ShortcutIsCreated { get; set; } = false;
        private readonly bool _isReinstall;
        public SetupPage(MainWindow mainWindow, bool isReinstall = false)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _isReinstall = isReinstall;


            this.textBlock_INSTALL_LOCATION.Text = installPath;
            this.textBlock_APP_SIZE.Text = $"{APP_SIZE}MB";
        }






        private bool CreateDesktopShortcut()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutPath = Path.Combine(desktopPath, "Data Logger.lnk");
                string exePath = Path.Combine(installPath, "Data Logger.exe");

                Type shellType = Type.GetTypeFromProgID("WScript.Shell")!;
                dynamic shell = Activator.CreateInstance(shellType)!;
                dynamic shortcut = shell.CreateShortcut(shortcutPath);

                shortcut.TargetPath = exePath;
                shortcut.WorkingDirectory = installPath;
                shortcut.Description = "Data Logger";
                shortcut.IconLocation = exePath;
                shortcut.Save();

                Marshal.FinalReleaseComObject(shortcut);
                Marshal.FinalReleaseComObject(shell);

                return File.Exists(shortcutPath);
            }
            catch
            {
                return false;
            }
        }

        private bool PinToTaskbar()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutPath = Path.Combine(desktopPath, "Data Logger.lnk");

                if (!File.Exists(shortcutPath))
                    return false;

                Type shellType = Type.GetTypeFromProgID("Shell.Application")!;
                dynamic shell = Activator.CreateInstance(shellType)!;

                dynamic folder = shell.Namespace(desktopPath);
                dynamic item = folder.ParseName("Data Logger.lnk");

                bool pinned = false;

                foreach (dynamic verb in item.Verbs())
                {
                    string name = verb.Name;

                    if (name.Contains("taskbar", StringComparison.OrdinalIgnoreCase))
                    {
                        verb.DoIt();
                        pinned = true;
                        break;
                    }
                }

                Marshal.FinalReleaseComObject(item);
                Marshal.FinalReleaseComObject(folder);
                Marshal.FinalReleaseComObject(shell);

                return pinned;
            }
            catch
            {
                return false;
            }
        }

        private bool AssociateDlsFiles()
        {
            string progId = "DataLogger.File";
            string exePath = Path.Combine(installPath, "Data Logger.exe");
            string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "dls_icon.ico");

            try
            {
                if (!File.Exists(exePath))
                    return false;

                using RegistryKey extensionKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\.dls") ?? throw new InvalidOperationException("Failed to create .dls registry key.");
                extensionKey.SetValue("", progId);

                using RegistryKey progIdKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{progId}") ?? throw new InvalidOperationException("Failed to create DataLogger.File registry key.");
                progIdKey.SetValue("", "Data Logger File");

                using RegistryKey iconKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{progId}\DefaultIcon") ?? throw new InvalidOperationException("Failed to create DefaultIcon registry key.");
                iconKey.SetValue("", $"\"{iconPath}\",0");

                using RegistryKey commandKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{progId}\shell\open\command") ?? throw new InvalidOperationException("Failed to create open command registry key.");
                commandKey.SetValue("", $"\"{exePath}\" \"%1\"");

                NotifyFileAssociationChanged();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to associate .dls files: {ex.Message}");

                try
                {
                    Registry.CurrentUser.DeleteSubKeyTree($@"Software\Classes\{progId}", false);

                    using RegistryKey? extensionKey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\.dls", true);

                    if (extensionKey?.GetValue("")?.ToString() == progId)
                        Registry.CurrentUser.DeleteSubKey(@"Software\Classes\.dls", false);

                    NotifyFileAssociationChanged();
                }
                catch (Exception cleanupEx)
                {
                    Debug.WriteLine($"Failed to clean up .dls association: {cleanupEx.Message}");
                }

                return false;
            }
        }

        [DllImport("shell32.dll")]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private void NotifyFileAssociationChanged()
        {
            const uint SHCNE_ASSOCCHANGED = 0x08000000;
            const uint SHCNF_IDLIST = 0x0000;

            SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
        }







        private void on_CREATE_SHORTCUT_Checked(object sender, RoutedEventArgs e)
        {
            this.grid_SETUP_TASKBAR_PIN.Visibility = Visibility.Visible;
        }

        private void on_CREATE_SHORTCUT_Unchecked(object sender, RoutedEventArgs e)
        {
            this.grid_SETUP_TASKBAR_PIN.Visibility = Visibility.Collapsed;
            this.checkBox_PIN_TASKBAR.IsChecked = false;
        }

        private async void on_INSTALL_ClickedAsync(object sender, RoutedEventArgs e)
        {
            _mainWindow.ShowInstallingPage();

            try
            {
                // Add system check for storage space on C: drive
                DriveInfo drive = new DriveInfo("C:");
                if (drive.AvailableFreeSpace < APP_SIZE * 1024 * 1024)
                {
                    _mainWindow.ShowFailedPage("Insufficient storage space available.");
                    return;
                }

                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(AppContext.BaseDirectory, "Setup.exe"),
                    Arguments = $"--silent --installto \"{installPath}\"",
                    UseShellExecute = true
                };

                Process? process = Process.Start(processInfo);

                if (process == null)
                    throw new InvalidOperationException("Process failed to start.");

                using (process)
                {
                    await process.WaitForExitAsync();
                }

                if (process.ExitCode != 0)
                {
                    _mainWindow.ShowFailedPage(isSecondFailure: _isReinstall);
                    return;
                }

                // ============================================================
                // POST-INSTALLATION SETUP
                // ============================================================

                // Desktop shortcut
                if (this.checkBox_CREATE_SHORTCUT.IsChecked ?? false)
                {
                    ShortcutIsCreated = CreateDesktopShortcut();
                }


                // Taskbar
                if (this.checkBox_PIN_TASKBAR.IsChecked ?? false)
                {
                    if (ShortcutIsCreated)
                        PinToTaskbar();
                }


                // .dls file association
                if (this.checkBox_ASSOCIATE_DLS.IsChecked ?? false)
                {
                    AssociateDlsFiles();
                }

                // Always associate .index and cache files here
                // TODO: Implement association for .index and cache files if possible

                _mainWindow.ShowLaunchPage();
                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in on_INSTALL_ClickedAsync(): {ex.Message}");
            }

            _mainWindow.ShowFailedPage(isSecondFailure: _isReinstall);
        }


    }
}
