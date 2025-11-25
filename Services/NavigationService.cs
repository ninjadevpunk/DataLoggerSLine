using System.Windows;
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
using Data_Logger_1._3.ViewModels.Dialogs;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Services
{
    public class NavigationService
    {
        public void CloseApp() => Application.Current.Shutdown(1);

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

        // MAIN WINDOW
        private MainWindow _mainWindow;

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
        /// Navigate back in the main frame's history, if possible.
        /// </summary>
        /// <param name="isPost"></param>
        public void GoBack(bool isPost)
        {
            if (_mainwindowFrame != null && _mainwindowFrame.CanGoBack)
            {
                if (!isPost)
                {
                    if (NavigationContext == NavContext.POSTIT)
                    {
                        var result = MessageBox.Show(
                            "Your post it content will not be saved. Are you sure you would like to go back to the logger?",
                            "You Sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                        if (result == MessageBoxResult.No)
                            return;
                    }
                    else if (NavigationContext == NavContext.LOGGER)
                    {
                        var result = MessageBox.Show(
                            "Your log will not be saved. Are you sure you would like to go back to the dashboard?",
                            "You Sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                        if (result == MessageBoxResult.No)
                            return;

                    }
                }

                _mainwindowFrame.GoBack();
            }

            NavigationContext = NavigationContext switch
            {
                NavContext.POSTIT => NavContext.LOGGER,
                NavContext.LOGGER => NavContext.DASHBOARD,
                _ => NavigationContext
            };

            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            CheckBackNavigationButton(mainWindowViewModel);
        }

        public void CheckBackNavigationButton(MainWindowViewModel window)
        {
            if (window != null && _mainwindowFrame != null)
            {
                window.BackEnabled = _mainwindowFrame.CanGoBack;
            }

            if (NavigationContext == NavContext.DASHBOARD)
                window.BackEnabled = false;
        }

        /// <summary>
        /// Navigate forward in the main frame's history, if possible.
        /// </summary>
        public void GoForward()
        {
            if (_mainwindowFrame != null && _mainwindowFrame.CanGoForward)
                _mainwindowFrame.GoForward();
        }


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

        /// <summary>
        /// Sets NavigationContext to DASHBOARD specifically.
        /// </summary>
        public void SetDashboardContext()
        {
            NavigationContext = NavContext.DASHBOARD;
        }



        public async Task NavigateToLogin(bool isSignOut)
        {
            var loginWindow = _serviceProvider.GetRequiredService<Login>();
            var page01 = _serviceProvider.GetRequiredService<loginPage01>();

            if (isSignOut)
            {
                var dataService = _serviceProvider.GetRequiredService<DataService>();
                await dataService.SignOutUser();

                var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
                loginViewModel.Username = "";
                loginViewModel.Password = "";
                loginViewModel.StatusMessage = "";


                loginWindow.frame_LOGIN.Navigate(page01);
                loginWindow.Show();

                _mainWindow.Close();

                return;
            }

            loginWindow = _serviceProvider.GetRequiredService<Login>();

            page01 = _serviceProvider.GetRequiredService<loginPage01>();

            loginWindow.frame_LOGIN.Navigate(page01);
            loginWindow.Show();
        }

        public void NavigateToSignUp()
        {
            var signUpWindow = _serviceProvider.GetRequiredService<SignUp>();
            signUpWindow.Show();
        }

        public async Task NavigateToMainWindow()
        {
            using var scope = _serviceProvider.CreateScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();


            try
            {
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

                // Store the main frame reference
                SetMainFrame(mainWindow.frame_MAINWINDOW);

                var dataService = _serviceProvider.GetRequiredService<DataService>();
                await dataService.SignInUser();

                await _serviceProvider.GetRequiredService<InitialService>().Initialise(dataService.GetUser().accountID);

                var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();

                mainWindow.DataContext = mainWindowViewModel;
                mainWindow.Show();

                _mainWindow = mainWindow;
                CheckBackNavigationButton(mainWindowViewModel);


                SetDashboardContext();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "NavigateToMainWindow()", "InvalidOperationException");

                CloseApp();
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "NavigateToMainWindow()");

                CloseApp();
            }
        }















        public async Task NavigateToPage<TViewModel>(Page page, UserControl subPage = null)
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

                if (viewModel is AScodeCreateViewModel aSviewModel)
                {
                    aSviewModel.SignUpImage = userProfilePic;
                    await aSviewModel.AutoStartAsync();

                    if (aSviewModel.SignUpImage == displayPic)
                        aSviewModel.SignUpImage = string.Empty;

                    page.DataContext = aSviewModel;
                }
                else if (viewModel is graphicCreateViewModel gfXviewModel)
                {
                    gfXviewModel.SignUpImage = userProfilePic;
                    await gfXviewModel.AutoStartAsync();

                    if (gfXviewModel.SignUpImage == displayPic)
                        gfXviewModel.SignUpImage = string.Empty;

                    page.DataContext = gfXviewModel;
                }
                else if (viewModel is filmCreateViewModel fLviewModel)
                {
                    fLviewModel.SignUpImage = userProfilePic;
                    await fLviewModel.AutoStartAsync();

                    if (fLviewModel.SignUpImage == displayPic)
                        fLviewModel.SignUpImage = string.Empty;

                    page.DataContext = fLviewModel;
                }
                else if (viewModel is flexiCreateViewModel fleXviewModel)
                {
                    fleXviewModel.SignUpImage = userProfilePic;

                    if (fleXviewModel.SignUpImage == displayPic)
                        fleXviewModel.SignUpImage = string.Empty;

                    page.DataContext = fleXviewModel;
                }
                else
                    page.DataContext = viewModel;

                if (subPage != null)
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



        public async Task NavigateToPage(Page page, ViewModelBase viewModel, UserControl subPage = null)
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

                if (subPage != null)
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

        public async Task NavigateToPage<TPage, TViewModel>()
            where TPage : Page, new()
            where TViewModel : ViewModelBase
        {
            using var scope = _serviceProvider.CreateScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();


            try
            {
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

                var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
                var page = uiFactory.CreatePage<TPage>();
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

        public async Task NavigateToPage(Page page)
        {
            using var scope = _serviceProvider.CreateScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                if (_mainwindowFrame is null)
                    throw new InvalidOperationException("Main frame not set!");

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

            SetDashboardContext();
        }

        public async Task NavigateToLogCachePage(CacheContext cacheContext)
        {
            SetDashboardContext();
            CacheContext = cacheContext;

            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();

            var logCachePage = uiFactory.CreatePage<LogCachePage>();

            switch (cacheContext)
            {
                case CacheContext.Qt:
                    {
                        await NavigateToPage<CodingQtViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<CodingQtViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.AndroidStudio:
                    {
                        await NavigateToPage<CodingAndroidViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<CodingAndroidViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.Graphics:
                    {
                        await NavigateToPage<GraphicsViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<GraphicsViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.Film:
                    {
                        await NavigateToPage<FilmViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<FilmViewModel>().UpdateLogCount();

                        break;
                    }
                case CacheContext.Flexi:
                    {
                        await NavigateToPage<FlexiViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<FlexiViewModel>().UpdateLogCount();

                        break;
                    }
                default:
                    {
                        await NavigateToPage<CodingViewModel>(logCachePage);
                        _serviceProvider.GetRequiredService<CodingViewModel>().UpdateLogCount();

                        break;
                    }
            }

            //var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            //CheckBackNavigationButton(mainWindowViewModel);

        }

        public async Task NavigateToLogCachePage<TViewModel>(CacheContext cacheContext) where TViewModel : LogCacheViewModel
        {
            await NavigateToPage<LogCachePage, TViewModel>();

            SetDashboardContext();
        }















        public async Task NavigateToLoggerCreator()
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
                            await qtLogger.AutoStartAsync(true);
                            qtLogger.SignUpImage = userProfilePic;

                            if (qtLogger.SignUpImage == displayPic)
                                qtLogger.SignUpImage = string.Empty;

                            await NavigateToPage(loggerCreatePage, qtLogger, new coding_UserControl());

                            break;
                        }
                    case CacheContext.AndroidStudio:
                        {
                            await NavigateToPage<AScodeCreateViewModel>(loggerCreatePage, new coding_UserControl());

                            var androidPage = new androidStudio_UserControl();
                            androidPage.DataContext = _serviceProvider.GetRequiredService<AScodeCreateViewModel>();
                            _AndroidStudioFrame.Navigate(androidPage);

                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            await NavigateToPage<graphicCreateViewModel>(loggerCreatePage, new graphics_UserControl());

                            break;
                        }
                    case CacheContext.Film:
                        {
                            await NavigateToPage<filmCreateViewModel>(loggerCreatePage, new film_UserControl());

                            break;
                        }
                    case CacheContext.Flexi:
                        {
                            await NavigateToPage<flexiCreateViewModel>(loggerCreatePage, new flexi_UserControl());

                            break;
                        }
                    default:
                        {
                            var codingLogger = viewModelFactory.CreateCodeCreateViewModel();
                            await codingLogger.AutoStartAsync();
                            codingLogger.SignUpImage = userProfilePic;

                            if (codingLogger.SignUpImage == displayPic)
                                codingLogger.SignUpImage = string.Empty;

                            await NavigateToPage(loggerCreatePage, codingLogger, new coding_UserControl());

                            break;
                        }
                }

                var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                CheckBackNavigationButton(mainWindowViewModel);
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















        public async Task NavigateToPostItCreator(LoggerCreateViewModel logger)
        {
            NavigationContext = NavContext.POSTIT;
            var uiFactory = _serviceProvider.GetRequiredService<UIFactory>();
            var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
            var postItPageViewModel = await viewModelFactory.CreatePostItViewModel(logger, logger.Category);
            var postItPage = uiFactory.CreatePostItPage(postItPageViewModel);

            await NavigateToPage(postItPage);
        }

        public async Task NavigateToNOTESDashboard()
        {
            await NavigateToPage<NOTESPage, NOTESViewModel>();
            SetDashboardContext();
        }

        public async Task NavigateToCreateNotesPage()
        {
            SetDashboardContext();
            CacheContext = CacheContext.NOTES;

            await NavigateToPage<CreateNotePage, CreateNoteViewModel>();
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
