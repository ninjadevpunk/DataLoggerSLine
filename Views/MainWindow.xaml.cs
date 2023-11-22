using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void on_MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Calculate the new left position to center the window horizontally
            double windowWidth = this.Width;
            double newLeft = (SystemParameters.PrimaryScreenWidth - windowWidth) / 2.0;

            // Calculate the new top position to center the window vertically
            double windowHeight = this.Height;
            double newTop = (SystemParameters.PrimaryScreenHeight - windowHeight) / 2.0;

            // Set the new left and top positions
            this.Left = newLeft;
            this.Top = newTop;
        }













        private void on_MINIMISE_clicked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void on_MAXIMISE_clicked(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // If the window is currently maximized, restore it
                WindowState = WindowState.Normal;
            }
            else
            {
                // If the window is not maximized, maximize it
                WindowState = WindowState.Maximized;
            }
        }

        private void on_CLOSE_clicked(object sender, RoutedEventArgs e)
        {
            // Close the entire application
            Application.Current.Shutdown();
        }

        private void on_customTitleBar_pressed(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


    }
}