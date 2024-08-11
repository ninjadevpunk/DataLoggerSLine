using Data_Logger_1._3.Services;
using System.Windows.Controls;

namespace Data_Logger_1._3.Views
{
    /// <summary>
    /// Interaction logic for loginPage02.xaml
    /// </summary>
    public partial class loginPage02 : Page
    {
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        public loginPage02()
        {
            InitializeComponent();
        }

        public loginPage02(AuthService authService, NavigationService navigationService)
        {
            InitializeComponent();

            _authService = authService;
            _navigationService = navigationService;

        }
    }
}
