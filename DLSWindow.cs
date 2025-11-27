using System.Windows;
using System.Windows.Media.Imaging;

namespace Data_Logger_1._3
{
    public class DLSWindow : Window
    {
        public DLSWindow()
        {
            string env;

            #if DEBUG
                env = "AlphaBeta";
            #else
                env = "Release";
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
