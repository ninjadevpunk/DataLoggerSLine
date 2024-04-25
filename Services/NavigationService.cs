using Data_Logger_1._3.Components.Subcontrols;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Data_Logger_1._3.Views;
using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.LogPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Services
{
    public enum CacheContext
    {
        Qt,
        AndroidStudio,
        Generic,
        Graphics,
        Film,
        NOTES,
        Flexi
    }

    public class NavigationService
    {
        private readonly AuthService _authService;
        private readonly DataService _dataService;

        private IHost _host;
        private Frame _MainFrame; // MainWindow Frame
        private Frame _frame; // Sub class Logs Logger Frame
        private Frame _ASframe; // Android Studio Logger Frame

        public Login LoginWindow { get; set; }

        public SignUp SignUpWindow { get; set; }

        public MainWindowViewModel Main { get; set; }

        public LogCachePage Dashboard { get; set; }

        public LoggerCreatePage Logger { get; set; }

        public NOTESPage NOTESList { get; set; }



        #region Dashboard Contexts



        public CodingQtViewModel codingQtDashboard { get; set; }

        public CodingAndroidViewModel codingAndroidDashboard { get; set; }

        public CodingGenericViewModel codingDashboard { get; set; }

        public GraphicsViewModel graphicsDashboard { get; set; }

        public FilmViewModel filmDashboard { get; set; }

        public NOTESViewModel notesDashboard { get; set; }

        public FlexiViewModel flexiDashboard { get; set; }


        #endregion


        #region Logger Contexts



        public codeCreateViewModel QtCodingLogger { get; set; }

        public AScodeCreateViewModel ASCodingLogger { get; set; }

        public codeCreateViewModel CodingLogger { get; set; }


        public graphicCreateViewModel GraphicsLogger { get; set; }

        public filmCreateViewModel FilmLogger { get; set; }

        public flexiCreateViewModel FlexiLogger { get; set; }




        #endregion



        #region Frames



        public coding_UserControl CodingFrame { get; set; }

        public androidStudio_UserControl AndroidStudioFrame { get; set; }

        public graphics_UserControl GraphicsFrame { get; set; }

        public film_UserControl FilmFrame { get; set; }

        public flexi_UserControl FlexiFrame { get; set; }





        #endregion

        public CreateNotePage createNotesPage { get; set; }

        public CreateCheckListPage createCheckListPage { get; set; }




        #region Constructors




        public NavigationService(AuthService service, DataService dataService)
        {
            _authService = service;
            _dataService = dataService;


            
        }


        public NavigationService(IHost host, Frame frame, Frame aSframe) 
        {
            _host = host;
            _frame = aSframe;
            _ASframe = aSframe;

        }

        public void setup()
        {
            Main = new MainWindowViewModel(this, _authService);
            Dashboard = new LogCachePage();
            Logger = new LoggerCreatePage();
            _frame = Logger.frame_VARIATIONS;
            _ASframe = Logger.frame_ANDROIDSTUDIO;

            codingQtDashboard = new CodingQtViewModel(this);
            codingAndroidDashboard = new CodingAndroidViewModel(this);
            codingDashboard = new CodingGenericViewModel(this);
            graphicsDashboard = new GraphicsViewModel(this);
            filmDashboard = new FilmViewModel(this);
            flexiDashboard = new FlexiViewModel(this);

            notesDashboard = new NOTESViewModel(this);

            QtCodingLogger = new codeCreateViewModel(this, codingQtDashboard, "Qt", _dataService);
            ASCodingLogger = new AScodeCreateViewModel(this, codingAndroidDashboard, _dataService);
            CodingLogger = new codeCreateViewModel(this, codingDashboard, _dataService);
            GraphicsLogger = new graphicCreateViewModel(this, graphicsDashboard, _dataService);
            FilmLogger = new filmCreateViewModel(this, filmDashboard, _dataService);
            FlexiLogger = new flexiCreateViewModel(this, flexiDashboard, _dataService);

            CodingFrame = new coding_UserControl();
            AndroidStudioFrame = new androidStudio_UserControl();
            AndroidStudioFrame.DataContext = ASCodingLogger;
            GraphicsFrame = new graphics_UserControl();
            FilmFrame = new film_UserControl();
            FlexiFrame = new flexi_UserControl();

            NOTESList = new NOTESPage();
            NOTESList.DataContext = notesDashboard;
            createNotesPage = new CreateNotePage();
            createNotesPage.DataContext = new CreateNoteViewModel(this);
            createCheckListPage = new CreateCheckListPage();
            createCheckListPage.DataContext = new CreateCheckListViewModel(this);
        }




        #endregion









        #region Functions


        public void SetHost(IHost host)
        {
            _host = host;
        }


        public void GoBack()
        {
            if (_MainFrame.CanGoBack)
            {
                _MainFrame.GoBack();
            }
        }

        public void GoForward()
        {
            if (_MainFrame.CanGoForward)
            {
                _MainFrame.GoForward();
            }
        }





        public void NavigateToLogin()
        {
            LoginWindow = _host.Services.GetRequiredService<Login>();
            var page = _host.Services.GetRequiredService<loginPage01>();


            LoginWindow.frame_LOGIN.Navigate(page);
            LoginWindow.Show();
        }

        public void NavigateToLogin(bool IsLogin)
        {
            if(IsLogin)
            {
                var page = _host.Services.GetRequiredService<loginPage01>();
                LoginWindow.frame_LOGIN.Navigate(page);
            }
            else
            {
                var page = _host.Services.GetRequiredService<loginPage02>();
                LoginWindow.frame_LOGIN.Navigate(page);
            }


            LoginWindow.Show();
        }

        public void NavigateToSignUp()
        {
            SignUpWindow = _host.Services.GetRequiredService<SignUp>();
            SignUpWindow.Show();
            LoginWindow.Hide();
        }

        public void NavigateToMainWindow()
        {
            setup();

            if (LoginWindow is null)
                return;


            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            var displayPic = "/Assets/login/user.png";

            mainWindow.DataContext = Main;

            QtCodingLogger.SignUpImage = _authService.Account.ProfilePic;

            if (QtCodingLogger.SignUpImage == displayPic)
                QtCodingLogger.SignUpImage = "";

            QtCodingLogger.Author = $"{_authService.Account.FirstName} {_authService.Account.LastName}";

            ASCodingLogger.SignUpImage = _authService.Account.ProfilePic;

            if (ASCodingLogger.SignUpImage == displayPic)
                ASCodingLogger.SignUpImage = "";

            ASCodingLogger.Author = $"{_authService.Account.FirstName} {_authService.Account.LastName}";

            CodingLogger.SignUpImage = _authService.Account.ProfilePic;

            if (CodingLogger.SignUpImage == displayPic)
                CodingLogger.SignUpImage = "";

            CodingLogger.Author = $"{_authService.Account.FirstName} {_authService.Account.LastName}";

            GraphicsLogger.SignUpImage = _authService.Account.ProfilePic;

            if (GraphicsLogger.SignUpImage == displayPic)
                GraphicsLogger.SignUpImage = "";

            GraphicsLogger.Author = $"{_authService.Account.FirstName} {_authService.Account.LastName}";

            FilmLogger.SignUpImage = _authService.Account.ProfilePic;

            if (FilmLogger.SignUpImage == displayPic)
                FilmLogger.SignUpImage = "";

            FilmLogger.Author = $"{_authService.Account.FirstName} {_authService.Account.LastName}";

            FlexiLogger.SignUpImage = _authService.Account.ProfilePic;

            if (FlexiLogger.SignUpImage == displayPic)
                FlexiLogger.SignUpImage = "";

            FlexiLogger.Author = $"{_authService.Account.FirstName} {_authService.Account.LastName}";

            Main.SignUpImage = _authService.Account.ProfilePic;

            if (Main.SignUpImage == displayPic)
                Main.SignUpImage = "";

            mainWindow.Show();
            _MainFrame = mainWindow.frame_MAINWINDOW;
            NavigateToLogCachePage();
            LoginWindow.Close();
            if(SignUpWindow is not null)
                SignUpWindow.Close();
        }

        public void NavigateToLogCachePage()
        {
            NavigateToLogCachePage(CacheContext.Qt);
        }

        public void NavigateToLogCachePage(CacheContext context)
        {
            if (codingQtDashboard is null)
                return;

            ChangeData(context);
            _MainFrame.Navigate(Dashboard);
        }

        public void ChangeData(CacheContext context)
        {

            if (codingQtDashboard is null || _frame is null)
                return;

            Logger.inputText_APP.UserComboInput = "";
            Logger.inputText_APP.showPlaceholderText();

            switch (context)
            {
                case CacheContext.Qt:
                    Dashboard.DataContext = codingQtDashboard;
                    QtCodingLogger.UpdateLogCount();
                    Logger.DataContext = QtCodingLogger;
                    Logger.inputText_APP.UserComboInput = "Qt Creator";
                    CodingFrame.DataContext = QtCodingLogger;
                    _frame.Navigate(CodingFrame);
                    _ASframe.Navigate(null);
                    break;

                case CacheContext.AndroidStudio:
                    Dashboard.DataContext = codingAndroidDashboard;
                    Logger.DataContext = ASCodingLogger;
                    Logger.inputText_APP.UserComboInput = "Android Studio Hedgehog 2023.1.1";
                    CodingFrame.DataContext = ASCodingLogger;
                    _frame.Navigate(CodingFrame);
                    _ASframe.Navigate(AndroidStudioFrame);
                    break;
                case CacheContext.Generic:
                    Dashboard.DataContext = codingDashboard;
                    Logger.DataContext = CodingLogger;
                    CodingFrame.DataContext = CodingLogger;
                    _frame.Navigate(CodingFrame);
                    _ASframe.Navigate(null);
                    break;
                case CacheContext.Graphics:
                    Dashboard.DataContext = graphicsDashboard;
                    Logger.DataContext = GraphicsLogger;
                    GraphicsFrame.DataContext = GraphicsLogger;
                    _frame.Navigate(GraphicsFrame);
                    _ASframe.Navigate(null);
                    break;
                case CacheContext.Film:
                    Dashboard.DataContext = filmDashboard;
                    Logger.DataContext = FilmLogger;
                    FilmFrame.DataContext = FilmLogger;
                    _frame.Navigate(FilmFrame);
                    _ASframe.Navigate(null);
                    break;
                case CacheContext.Flexi:
                    Dashboard.DataContext = flexiDashboard;
                    Logger.DataContext = FlexiLogger;
                    FlexiFrame.DataContext = FlexiLogger;
                    _frame.Navigate(FlexiFrame);
                    _ASframe.Navigate(null);
                    break;

            }
        }

        public void NavigateToNOTESDashboard()
        {
            _MainFrame.Navigate(NOTESList);
        }

        public void NavigateToCreateNotesPage()
        {
            Main.GenericNotesChecked = true;
            _MainFrame.Navigate(createNotesPage);
        }

        public void NavigateToCreateCheckListPage()
        {
            Main.GenericNotesChecked = true;
            _MainFrame.Navigate(createCheckListPage);
        }

        public void NavigateToLoggerCreator()
        {

            _MainFrame.Navigate(Logger);
        }

        public void NavigateToPostItCreator(LoggerCreateViewModel loggerCreator)
        {
            var postItPage = new PostItPage(new CreatePostItViewModel(this, loggerCreator));

            _MainFrame.Navigate(postItPage);
        }

        public void NavigateToReporter()
        {
            _MainFrame.Navigate(new Uri("/Views/Dialogs/ReportPage.xaml", UriKind.Relative));
        }



        #endregion
    }
}
