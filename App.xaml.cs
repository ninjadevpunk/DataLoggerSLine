using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Data_Logger_1._3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly IHost _host;
        private static readonly Random random = new();
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, service) =>
                {
                    var firebaseApiKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY");
                    var firebaseDomain = context.Configuration.GetValue<string>("FIREBASE_DOMAIN");
                    service.AddSingleton(new AuthService(firebaseApiKey, firebaseDomain));
                    service.AddSingleton((services) => new DataService(firebaseApiKey, firebaseDomain, services.GetRequiredService<AuthService>()));
                    service.AddSingleton((services) => new NavigationService(services.GetRequiredService<AuthService>(), services.GetRequiredService<DataService>()));
                    service.AddTransient<Splashscreen>();
                    service.AddSingleton<Login>();
                    service.AddTransient((services) => new loginPage01(services.GetRequiredService<NavigationService>())
                    {
                        DataContext = new LoginViewModel(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>())
                    }
                    );
                    service.AddTransient((services) => new loginPage02(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>())
                    {
                        DataContext = new ForgotPasswordViewModel(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>())
                    }
                    );
                    service.AddSingleton((services) => new SignUp(services.GetRequiredService<NavigationService>())
                    {
                        DataContext = new SignUpViewModel(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>())
                    }
                    );
                    service.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var splash = _host.Services.GetRequiredService<Splashscreen>();
            splash.Show();


            await AnimateProgressBar(splash.progressBar_splashscreen, 33);


            var navigationService = _host.Services.GetRequiredService<NavigationService>();

            navigationService.SetHost(_host);
            await AnimateProgressBar(splash.progressBar_splashscreen, 66);
            await AnimateProgressBar(splash.progressBar_splashscreen, 100);

            navigationService.NavigateToLogin();

            splash.Close();







            base.OnStartup(e);
        }

        private async Task AnimateProgressBar(ProgressBar progressBar, double targetValue)
        {
            DoubleAnimation animation = new DoubleAnimation(targetValue, TimeSpan.FromSeconds(1));

            var tcs = new TaskCompletionSource<bool>();
            animation.Completed += (s, e) => tcs.SetResult(true);

            progressBar.BeginAnimation(ProgressBar.ValueProperty, animation);

            await tcs.Task;
            await Task.Delay(random.Next(350, 1300)); // Wait for a short duration after each animation
        }
    }

}
