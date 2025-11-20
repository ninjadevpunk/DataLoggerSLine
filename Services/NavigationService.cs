using Data_Logger_1._3.Components.Subcontrols;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.Views;
using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.LogPages;
using Microsoft.Extensions.DependencyInjection;
using MVVMEssentials.ViewModels;
using System.Windows.Controls;
using System.Windows.Markup;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Services
{
    public class NavigationService
    {


        /// <summary>
        /// Used to track the current page the MainWindow's frame is on.
        /// Use it in the MainWindowViewModel to authorise or deny navigation paths.
        /// </summary>
        public enum NavContext
        {
            DASHBOARD, LOGGER, POSTIT,
            VIEWER,
            EDITOR, EDITOR_POSTIT,
            REPORTER_DASHBOARD, UPDATER, UPDATER_POSTIT
        }

        private readonly IServiceProvider _serviceProvider;

        // MAIN FRAME
        private Frame _mainwindowFrame;

        // LOGGER CREATOR SUBCLASS FRAME
        private Frame _loggerFrame;

        // LOGGER CREATOR ANDROID STUDIO FRAME
        private Frame _AndroidStudioFrame;


        public NavContext NavigationContext { get; set; } = NavContext.DASHBOARD;
        public CacheContext CacheContext { get; set; } = CacheContext.Qt;







        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }






        /// <summary>
        /// Set the MainWindow's frame to _mainwindowFrame.
        /// </summary>
        /// <param name="frame">The frame inside MainWindow window.</param>
        public void SetMainFrame(Frame frame) => _mainwindowFrame = frame;

        /// <summary>
        /// Set the logger's subclass frame.
        /// </summary>
        /// <param name="frame">The Frame UI instance.</param>
        public void SetLoggerFrame(Frame frame) => _loggerFrame = frame;

        /// <summary>
        /// Set the Android Studio frame.
        /// </summary>
        /// <param name="frame">The Frame UI instance.</param>
        public void SetAndroidStudioFrame(Frame frame) => _AndroidStudioFrame = frame;



        public async void NavigateToLogin(bool isSignOut)
        {
            if (isSignOut)
            {
                var dataService = _serviceProvider.GetRequiredService<DataService>();
                await dataService.SignOutUser();
            }

            var loginWindow = _serviceProvider.GetRequiredService<Login>();

            var page01 = _serviceProvider.GetRequiredService<loginPage01>();

            loginWindow.frame_LOGIN.Navigate(page01);
            loginWindow.Show();
        }

        public void NavigateToSignUp()
        {
            var signUpWindow = _serviceProvider.GetRequiredService<SignUp>();
            signUpWindow.Show();
        }

        public async void NavigateToMainWindow()
        {
            using var scope = _serviceProvider.CreateScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();


            try
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

                // Store the main frame reference
                SetMainFrame(mainWindow.frame_MAINWINDOW);

                _serviceProvider.GetRequiredService<InitialService>().Initialise();

                mainWindow.DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                mainWindow.Show();


                NavigationContext = NavContext.DASHBOARD;
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "NavigateToMainWindow()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "NavigateToMainWindow()");
            }
        }



        public async void NavigateToPage<TViewModel>(Page page, UserControl subPage = null)
            where TViewModel : ViewModelBase
        {
            using var scope = _serviceProvider.CreateScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();


            try
            {
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                if (NavigationContext == NavContext.LOGGER)
                {
                    if (_loggerFrame is null)
                        throw new InvalidOperationException("Logger frame not set!");

                    if (CacheContext == CacheContext.AndroidStudio && _AndroidStudioFrame is null)
                        throw new InvalidOperationException("Android Studio frame not set!");
                }

                var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

                const string displayPic = "/Assets/login/user.png";
                var userProfilePic = _serviceProvider.GetRequiredService<AuthService>().Account.ProfilePic;

                if (viewModel is AScodeCreateViewModel ASviewModel)
                {
                    ASviewModel.SignUpImage = userProfilePic;

                    if (ASviewModel.SignUpImage == displayPic)
                        ASviewModel.SignUpImage = string.Empty;

                    page.DataContext = ASviewModel;
                }
                else if(viewModel is graphicCreateViewModel GFXviewModel)
                {
                    GFXviewModel.SignUpImage = userProfilePic;

                    if (GFXviewModel.SignUpImage == displayPic)
                        GFXviewModel.SignUpImage = string.Empty;

                    page.DataContext = GFXviewModel;
                }
                else if(viewModel is filmCreateViewModel FLviewModel)
                {
                    FLviewModel.SignUpImage = userProfilePic;

                    if (FLviewModel.SignUpImage == displayPic)
                        FLviewModel.SignUpImage = string.Empty;

                    page.DataContext = FLviewModel;
                }
                else if(viewModel is flexiCreateViewModel FLEXviewModel)
                {
                    FLEXviewModel.SignUpImage = userProfilePic;

                    if (FLEXviewModel.SignUpImage == displayPic)
                        FLEXviewModel.SignUpImage = string.Empty;

                    page.DataContext = FLEXviewModel;
                }
                else
                    page.DataContext = viewModel;

                if (subPage is not null)
                {
                    subPage.DataContext = viewModel;
                    _loggerFrame.Navigate(subPage);
                }

                _mainwindowFrame.Navigate(page);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "NavigateToPage<TViewModel>(page)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "NavigateToPage<TViewModel>(page)");
            }
        }

        public async void NavigateToPage(Page page, ViewModelBase viewModel, UserControl subPage = null)
        {
            using var scope = _serviceProvider.CreateScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();


            try
            {
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                if (NavigationContext == NavContext.LOGGER)
                {
                    if (_loggerFrame is null)
                        throw new InvalidOperationException("Logger frame not set!");

                    if (CacheContext == CacheContext.AndroidStudio && _AndroidStudioFrame is null)
                        throw new InvalidOperationException("Android Studio frame not set!");
                }

                page.DataContext = viewModel;

                if (subPage is not null)
                {
                    subPage.DataContext = viewModel;
                    _loggerFrame.Navigate(subPage);
                }

                _mainwindowFrame.Navigate(page);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "NavigateToPage(page, viewModel)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "NavigateToPage(page, viewModel)");
            }
        }

        public async void NavigateToPage<TPage, TViewModel>()
            where TPage : Page, new()
            where TViewModel : ViewModelBase
        {
            using var scope = _serviceProvider.CreateScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();


            try
            {
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                var page = _serviceProvider.GetRequiredService<TPage>();
                page.DataContext = _serviceProvider.GetRequiredService<TViewModel>();

                _mainwindowFrame.Navigate(page);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "NavigateToPage(page, viewModel)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "NavigateToPage(page, viewModel)");
            }
        }

        public async void NavigateToPage<TPage>()
            where TPage : Page, new()
        {
            using var scope = _serviceProvider.CreateScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                var vmFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
                var qtLogger = vmFactory.CreateQtCodeCreateViewModel();

                var page = new TPage();
                page.DataContext = qtLogger;

                _mainwindowFrame.Navigate(page);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "NavigateToPage<TPage>()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "NavigateToPage<TPage>()");
            }
        }

        public void NavigateToLogCachePage()
        {
            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();

            var logCachePage = uiFactory.CreatePage<LogCachePage>();
            logCachePage.DataContext = _serviceProvider.GetRequiredService<CodingQtViewModel>();

            _mainwindowFrame.Navigate(logCachePage);

            NavigationContext = NavContext.DASHBOARD;
        }

        public void NavigateToLogCachePage(CacheContext cacheContext)
        {
            NavigationContext = NavContext.DASHBOARD;
            CacheContext = cacheContext;

            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();

            var logCachePage = uiFactory.CreatePage<LogCachePage>();

            switch (cacheContext)
            {
                case CacheContext.Qt:
                    {
                        NavigateToPage<CodingQtViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<CodingQtViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.AndroidStudio:
                    {
                        NavigateToPage<CodingAndroidViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<CodingAndroidViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.Graphics:
                    {
                        NavigateToPage<GraphicsViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<GraphicsViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.Film:
                    {
                        NavigateToPage<FilmViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<FilmViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.Flexi:
                    {
                        NavigateToPage<FlexiViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<FlexiViewModel>().UpdateLogCount();

                        break;
                    }
                default:
                    {
                        NavigateToPage<CodingViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<CodingViewModel>().UpdateLogCount();

                        break;
                    }
            }


        }

        public void NavigateToLogCachePage<TViewModel>(CacheContext cacheContext) where TViewModel : LogCacheViewModel
        {
            NavigateToPage<LogCachePage, TViewModel>();

            NavigationContext = NavContext.DASHBOARD;
        }



        public async void NavigateToLoggerCreator()
        {
            using var scope = _serviceProvider.CreateScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                NavigationContext = NavContext.LOGGER;

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
                var loggerCreatePage = uiFactory.CreatePage<LoggerCreatePage>();
                const string displayPic = "/Assets/login/user.png";
                var userProfilePic = _serviceProvider.GetRequiredService<AuthService>().Account.ProfilePic;


                SetLoggerFrame(loggerCreatePage.frame_VARIATIONS);

                if (CacheContext == CacheContext.AndroidStudio)
                    SetAndroidStudioFrame(loggerCreatePage.frame_ANDROIDSTUDIO);
                else
                    SetAndroidStudioFrame(null);

                switch (CacheContext)
                {
                    case CacheContext.Qt:
                        {
                            var qtLogger = viewModelFactory.CreateQtCodeCreateViewModel();
                            qtLogger.SignUpImage = userProfilePic;

                            if (qtLogger.SignUpImage == displayPic)
                                qtLogger.SignUpImage = string.Empty;

                            NavigateToPage(loggerCreatePage, qtLogger, new coding_UserControl());

                            break;
                        }
                    case CacheContext.AndroidStudio:
                        {
                            NavigateToPage<AScodeCreateViewModel>(loggerCreatePage, new coding_UserControl());

                            var androidPage = new androidStudio_UserControl();
                            androidPage.DataContext = _serviceProvider.GetRequiredService<AScodeCreateViewModel>();
                            _AndroidStudioFrame.Navigate(androidPage);

                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            NavigateToPage<graphicCreateViewModel>(loggerCreatePage, new graphics_UserControl());

                            break;
                        }
                    case CacheContext.Film:
                        {
                            NavigateToPage<filmCreateViewModel>(loggerCreatePage, new film_UserControl());

                            break;
                        }
                    case CacheContext.Flexi:
                        {
                            NavigateToPage<flexiCreateViewModel>(loggerCreatePage, new flexi_UserControl());

                            break;
                        }
                    default:
                        {
                            var codingLogger = viewModelFactory.CreateCodeCreateViewModel();
                            codingLogger.SignUpImage = userProfilePic;

                            if (codingLogger.SignUpImage == displayPic)
                                codingLogger.SignUpImage = string.Empty;

                            NavigateToPage(loggerCreatePage, codingLogger, new coding_UserControl());

                            break;
                        }
                }
            }
            catch (XamlParseException xamlx)
            {
                await master.HandleExceptionAsync(xamlx, "NavigateToLoggerCreator()", "XamlParseException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "NavigateToLoggerCreator()");
            }

        }

        public void NavigateToQtLoggerCreator<TViewModel>() where TViewModel : LoggerCreateViewModel
        {
            var vmFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();

            var qtLogger = vmFactory.CreateQtCodeCreateViewModel();

            NavigateToPage<LoggerCreatePage>();

            NavigationContext = NavContext.LOGGER;
        }

        public void NavigateToNOTESDashboard()
        {
            NavigateToPage<NOTESPage, NOTESViewModel>();
            NavigationContext = NavContext.DASHBOARD;
        }

        public void SetGenericNotesChecked(bool isChecked = false)
        {
            var mainwindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainwindowViewModel.SetGenericNotesChecked(isChecked);
        }

        public void SetChecklistNotesChecked(bool isChecked = false)
        {
            var mainwindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainwindowViewModel.SetChecklistNotesChecked(isChecked);
        }
    }

}
