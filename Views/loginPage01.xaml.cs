using Data_Logger_1._3.Services;
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
        private readonly NavigationService _navigationService;


        public loginPage01()
        {
            InitializeComponent();
        }

        public loginPage01(NavigationService navigationService)
        {
            InitializeComponent();

            _navigationService = navigationService;
        }

        private void on_FORGOTPASSWORD_clicked(object sender, MouseButtonEventArgs e)
        {
            _navigationService.NavigateToLogin(false);
        }

        private void on_SIGNUP_clicked(object sender, MouseButtonEventArgs e)
        {

            try
            {

                _navigationService.NavigateToSignUp();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
