using System.Windows;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3
{
    public class DLSWindow : Window
    {
        protected string AppVersion { get; } = "alpha 5";

        public DLSWindow()
        {
            Application.Current.MainWindow = this;

            string env;

            #if DEBUG
                env = "DevMode";
            #else
                env = "AlphaBeta";
            #endif

            env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? env;

            Uri iconUri = env switch
            {
                "DevMode" => new Uri("pack://application:,,,/DevIcon.ico"),
                "AlphaBeta" => new Uri("pack://application:,,,/AlphaBetaIcon.ico"),
                _ => new Uri("pack://application:,,,/ReleaseIcon.ico")
            };

            this.Icon = BitmapFrame.Create(iconUri);
        }
    }
}
