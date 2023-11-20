using System.Windows;

namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
    }
}
