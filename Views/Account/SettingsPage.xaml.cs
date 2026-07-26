using Data_Logger_1._3.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Data_Logger_1._3.Views.Account
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly Storyboard _open;
        private readonly Storyboard _close;

        public SettingsPage()
        {
            InitializeComponent();

            _open = CreateHeightAnimation(0, 130, 200);
            _close = CreateHeightAnimation(130, 0, 200);

            Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel vm)
            {
                vm.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SettingsViewModel.ResetStage))
                return;

            var vm = (SettingsViewModel)sender!;

            switch (vm.ResetStage)
            {
                case SettingsViewModel.PasswordResetStage.RequestReset:
                    break;

                case SettingsViewModel.PasswordResetStage.EnterVerificationCode:
                    _open.Begin(this.stackPanel_PASSWORD_RESET_SECTION);
                    break;

                case SettingsViewModel.PasswordResetStage.ChangePassword:
                    _open.Begin(this.stackPanel_NEW_PASSWORD_SECTION);
                    break;
                    case SettingsViewModel.PasswordResetStage.PasswordChanged:
                    _close.Begin(this.stackPanel_PASSWORD_RESET_SECTION);
                    _close.Begin(this.stackPanel_NEW_PASSWORD_SECTION);
                    break;
            }
        }


        #region Animations 


        private static Storyboard CreateHeightAnimation(double from, double to, int durationMs)
        {
            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(durationMs)
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);

            Storyboard.SetTargetProperty(animation, new PropertyPath(StackPanel.HeightProperty));

            return storyboard;
        }



        #endregion
    }
}
