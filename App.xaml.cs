using Data_Logger_1._3.Services;
using Data_Logger_1._3.Views;
using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Data_Logger_1._3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, service) =>
                {
                    var firebaseApiKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY");
                    var firebaseDomain = context.Configuration.GetValue<string>("FIREBASE_DOMAIN");

                    service.AddTransient<SignUp>();
                    service.AddTransient<Splashscreen>();
                    service.AddTransient<Login>();
                    service.AddSingleton<MainWindow>();
                    service.AddSingleton(new AuthService(firebaseApiKey, firebaseDomain));
                    service.AddSingleton<NavigationService>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            var navigationService = _host.Services.GetRequiredService<NavigationService>();
            navigationService.SetHost(_host);
            navigationService.NavigateToLogin();







            base.OnStartup(e);
        }
    }

}
