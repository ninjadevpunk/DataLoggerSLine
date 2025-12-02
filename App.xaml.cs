using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.Dialogs.Edit;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.ViewerViewModels;
using Data_Logger_1._3.Views;
using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.LogPages;
using Data_Logger_1._3.Views.ReportPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Data_Logger_1._3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private readonly IServiceProvider _serviceProvider;
        public static IConfiguration? Configuration { get; private set; }

        public App()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    var env = hostingContext.HostingEnvironment.EnvironmentName;

                    if (env == "DevMode")
                        config.AddJsonFile($"appsettings.{env}.json", optional: true);
                })
                .ConfigureServices((context, service) =>
                {
                    service.AddSingleton<UIFactory>(services => new UIFactory(services));
                    service.AddSingleton((services) => new InitialService(services));
                    service.AddTransient<PDFService>();
                    service.AddTransient<ViewModelFactory>(services => new ViewModelFactory(services));


                    var env = context.HostingEnvironment.EnvironmentName;
                    //string key;

                    //if (env == "DevMode")
                    //{
                    //    key = Environment.GetEnvironmentVariable("SQLITE_KEY");
                    //}
                    //else
                    //{
                    //    key = context.Configuration["Database:EncryptionKey"];
                    //}

                    //var connectionString = new SqliteConnectionStringBuilder
                    //{
                    //    DataSource = context.Configuration.GetConnectionString("DefaultConnection"),
                    //    Mode = SqliteOpenMode.ReadWriteCreate,
                    //    Password = key
                    //}.ToString();

                    service.AddDbContext<ENTITYMASTER>(options =>
                    {
                        options.UseSqlite(context.Configuration.GetConnectionString("DefaultConnection"))
                        .LogTo(Console.WriteLine, LogLevel.Information);


                        if (env == "DevMode")
                            options.EnableSensitiveDataLogging();
                    });

                    service.AddSingleton<Cachemaster>();
                    service.AddScoped((services) => new ENTITYREADER(services.GetRequiredService<ENTITYMASTER>()));
                    service.AddScoped((services) => new ENTITYWRITER(services.GetRequiredService<ENTITYREADER>(), services.GetRequiredService<ENTITYMASTER>()));
                    service.AddScoped<ENTITYHANDLER>();

                    service.AddSingleton((services) => new AuthService(services.GetRequiredService<ENTITYWRITER>()));

                    service.AddSingleton((services) => new DataService(services.GetRequiredService<ENTITYWRITER>(), services.GetRequiredService<ENTITYREADER>(),
                        services.GetRequiredService<ENTITYHANDLER>(),
                        services.GetRequiredService<Cachemaster>(),
                        services.GetRequiredService<AuthService>()));

                    service.AddSingleton((services) => new NavigationService(services));


                    service.AddTransient<Splashscreen>();
                    service.AddTransient<LogCachePage>();
                    service.AddTransient<LoggerCreatePage>();
                    service.AddTransient<PostItPage>();
                    service.AddTransient<NOTESPage>();
                    service.AddTransient<CreateNotePage>();
                    service.AddTransient<CreateCheckListPage>();
                    service.AddTransient<LoggerEditPage>();
                    service.AddTransient<LoggerViewPage>();


                    service.AddTransient<ReporterEditPage>();

                    service.AddSingleton((services) => new LoginViewModel(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>(),
                        services.GetRequiredService<UIFactory>()));
                    service.AddSingleton((services) => new ForgotPasswordViewModel(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>()));
                    service.AddSingleton((services) => new SignUpViewModel(services.GetRequiredService<AuthService>(), services.GetRequiredService<NavigationService>(),
                        services.GetRequiredService<UIFactory>()));
                    service.AddSingleton((services) => new MainWindowViewModel(services.GetRequiredService<NavigationService>()));

                    service.AddSingleton((services) => new CodingQtViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>()));
                    service.AddSingleton((services) => new CodingAndroidViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>()));
                    service.AddSingleton((services) => new CodingViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>()));
                    service.AddSingleton((services) => new GraphicsViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>()));
                    service.AddSingleton((services) => new FilmViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>()));
                    service.AddSingleton((services) => new FlexiViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>()));
                    service.AddSingleton((services) => new NOTESViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>()));

                    service.AddSingleton((services) => new QtReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new ASReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new CodeReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new GraphicsReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new FilmReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>(),
                        services.GetRequiredService<PDFService>()));
                    service.AddSingleton((services) => new FlexiReportDeskViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>(),
                        services.GetRequiredService<PDFService>()));

                    // Register a factory that returns ReporterDashboard given a key
                    service.AddTransient<Func<string, ReporterDashboard>>(sp => key =>
                    {
                        ReportDeskViewModel vm = key switch
                        {
                            "Code" => sp.GetRequiredService<CodeReportDeskViewModel>(),
                            "Android" => sp.GetRequiredService<ASReportDeskViewModel>(),
                            "Qt" => sp.GetRequiredService<QtReportDeskViewModel>(),
                            "Graphics" => sp.GetRequiredService<GraphicsReportDeskViewModel>(),
                            "Film" => sp.GetRequiredService<FilmReportDeskViewModel>(),
                            "Flexi" => sp.GetRequiredService<FlexiReportDeskViewModel>(),
                            _ => throw new ArgumentException($"Unknown report type '{key}'")
                        };

                        return new ReporterDashboard(vm);
                    });


                    service.AddTransient<codeCreateViewModel>();
                    service.AddTransient((services) => new AScodeCreateViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<CodingAndroidViewModel>(),
                        services.GetRequiredService<DataService>()));
                    service.AddTransient((services) => new graphicCreateViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<GraphicsViewModel>(),
                        services.GetRequiredService<DataService>()));
                    service.AddTransient((services) => new filmCreateViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<FilmViewModel>(),
                        services.GetRequiredService<DataService>()));
                    service.AddTransient((services) => new flexiCreateViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<FlexiViewModel>(),
                        services.GetRequiredService<DataService>()));

                    service.AddTransient<codeEditViewModel>();
                    service.AddTransient((services) => new codeViewerViewModel(services.GetRequiredService<NavigationService>()));

                    // REPORTER


                    service.AddTransient<PostItViewModel>();
                    service.AddTransient((services) => new CreateNoteViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>(),
                        services.GetRequiredService<NOTESViewModel>()));
                    service.AddTransient((services) => new CreateCheckListViewModel(services.GetRequiredService<NavigationService>(), services.GetRequiredService<DataService>(),
                        services.GetRequiredService<NOTESViewModel>(), services.GetRequiredService<ViewModelFactory>()));
                    service.AddSingleton<CreateChecklistItemViewModel>();

                    service.AddTransient<Login>();
                    service.AddTransient((services) => new loginPage01(services.GetRequiredService<LoginViewModel>()));
                    service.AddTransient<loginPage02>();
                    service.AddTransient((services) => new SignUp(services.GetRequiredService<NavigationService>(), services.GetRequiredService<SignUpViewModel>()));
                    service.AddTransient((services) => new MainWindow(services.GetRequiredService<MainWindowViewModel>(), services.GetRequiredService<NavigationService>()));
                })
                .Build();

            _serviceProvider = host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            Configuration = _serviceProvider.GetRequiredService<IConfiguration>();

            var cachemaster = _serviceProvider.GetRequiredService<Cachemaster>();




            var splash = _serviceProvider.GetRequiredService<Splashscreen>();
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Release";

            switch (env)
            {
                case "AlphaBeta":
                    DeleteDevIcon();
                    break;
                case "Release":
                    DeleteDevIcon();
                    break;
            }

            splash.Show();


            await AnimateProgressBar(splash.progressBar_splashscreen, 33, splash.text_progress);


            var navigationService = _serviceProvider.GetRequiredService<NavigationService>();

            await AnimateProgressBar(splash.progressBar_splashscreen, 66, splash.text_progress);

            using (var scope = _serviceProvider.CreateScope())
            {
                await AnimateProgressBar(splash.progressBar_splashscreen, 75, splash.text_progress);
                var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();
                await AnimateProgressBar(splash.progressBar_splashscreen, 80, splash.text_progress);
                await master.Database.EnsureCreatedAsync();
            }


            await AnimateProgressBar(splash.progressBar_splashscreen, 100, splash.text_progress);

            await navigationService.NavigateToLogin(false);

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

        private void DeleteDevIcon()
        {
            string exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            string devIconPath = Path.Combine(exeFolder, "DevIcon.ico");

            if (File.Exists(devIconPath))
            {
                try
                {
                    File.Delete(devIconPath);
                    Console.WriteLine("DevIcon.ico deleted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete DevIcon.ico: {ex.Message}");
                }
            }
        }










        protected override async void OnExit(ExitEventArgs e)
        {
            var dataService = _serviceProvider.GetRequiredService<DataService>();
            await dataService.SignOutUser();

            base.OnExit(e);
        }


    }

}
