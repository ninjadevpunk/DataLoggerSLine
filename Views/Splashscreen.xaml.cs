using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for Splashscreen.xaml
    /// </summary>
    public partial class Splashscreen : Window
    {
        public Splashscreen()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Calculate the new left position to center the window in the imaginary 3rd column
            double fourthColumnWidth = SystemParameters.PrimaryScreenWidth / 4.0;
            double windowWidth = this.Width;
            double newLeft = (3 * fourthColumnWidth) - windowWidth;

            // Calculate the new top position to center the window vertically
            double windowHeight = this.Height;
            double newTop = (SystemParameters.PrimaryScreenHeight - windowHeight) / 2.0;

            // Set the new left and top positions
            this.Left = newLeft;
            this.Top = newTop;

            // Simulate some time-consuming task
            //for (int i = 0; i <= 100; i++)
            //{
            //    // Update the progress bar
            //    progressBar_splashscreen.Dispatcher.Invoke(() => progressBar_splashscreen.Value = i);

            //    // Update the text block
            //    text_progress.Dispatcher.Invoke(() => text_progress.Text = $"{i}%");

            //    // Simulate work being done
            //    await Task.Delay(50);
            //}
        }
    }
}
