using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for loginPage01.xaml
    /// </summary>
    public partial class loginPage01 : Page
    {
        public loginPage01()
        {
            InitializeComponent();
        }

        private void on_FORGOTPASSWORD_clicked(object sender, MouseButtonEventArgs e)
        {
            var loginWindow = Window.GetWindow(this) as Login;

            // Navigate to Forgot Password Page
            loginWindow?.Navigator("Views/loginPage02.xaml");
        }

        private void on_SIGNUP_clicked(object sender, MouseButtonEventArgs e)
        {

            try
            {

                // Open SignUp window
                SignUp signUpWindow = new SignUp();
                signUpWindow.Show();

                var loginWindow = Window.GetWindow(this) as Login;

                // Close the current window
                loginWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
