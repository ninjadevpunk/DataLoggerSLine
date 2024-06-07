using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
                    
                    service.AddSingleton<Cachemaster>((services) => new());
                    service.AddSingleton<DATAWRITER>((services) => new());
                    service.AddSingleton<DATAREADER>();
                    service.AddSingleton((services) => new AuthService(services.GetRequiredService<DATAWRITER>(), services.GetRequiredService<DATAREADER>()));
                    service.AddSingleton((services) => new DataService(services.GetRequiredService<DATAWRITER>(), services.GetRequiredService<DATAREADER>(), services.GetRequiredService<Cachemaster>(),
                        services.GetRequiredService<AuthService>()));
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


            await AnimateProgressBar(splash.progressBar_splashscreen, 33, splash.text_progress);


            var navigationService = _host.Services.GetRequiredService<NavigationService>();

            navigationService.SetHost(_host);
            await AnimateProgressBar(splash.progressBar_splashscreen, 66, splash.text_progress);
            await AnimateProgressBar(splash.progressBar_splashscreen, 100, splash.text_progress);

            navigationService.NavigateToLogin();

            splash.Close();







            base.OnStartup(e);
        }

        private async Task AnimateProgressBar(ProgressBar progressBar, double targetValue, TextBlock textBlock)
        {
            var duration = TimeSpan.FromSeconds(1);
            var animation = new DoubleAnimation(targetValue, duration);
            var textAnimation = new DoubleAnimation(targetValue, duration);

            var tcs = new TaskCompletionSource<bool>();

            animation.Completed += (s, e) => tcs.SetResult(true);
            progressBar.BeginAnimation(ProgressBar.ValueProperty, animation);

            var currentValue = progressBar.Value;
            var stepValue = (targetValue - currentValue) / (duration.TotalMilliseconds / 100);

            for (var i = currentValue; i <= targetValue; i += stepValue)
            {
                textBlock.Text = $"{(int)i}%";
                await Task.Delay(100);
            }

            await tcs.Task;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var dataService = _host.Services.GetRequiredService<DataService>();
            dataService.SignOutUser();

            base.OnExit(e);
        }
    }

}
