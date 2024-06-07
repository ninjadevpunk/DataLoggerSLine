using Data_Logger_1._3.Components.Subcontrols;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Data_Logger_1._3.Views;
using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.LogPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.ViewModels;
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

        public LOG.CATEGORY CurrentCategory { get; set; } = LOG.CATEGORY.CODING;

        private IHost _host;
        private Frame _MainFrame; // MainWindow Frame
        private Frame _frame; // Sub class Logs Logger Frame
        private Frame _ASframe; // Android Studio Logger Frame
        private Frame _EDS_frame; // Sub class Logs Editor Frame
        private Frame _EDS_ASframe; // Android Studio Logs Editor Frame

        public Login LoginWindow { get; set; }

        public SignUp SignUpWindow { get; set; }

        public MainWindowViewModel Main { get; set; }

        public LogCachePage Dashboard { get; set; }

        public LoggerCreatePage Logger { get; set; }

        public LoggerEditPage Editor { get; set; }

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


        #region Editor Contexts


        public codeEditViewModel QtCodingEditor { get; set; }

        public AScodeEditViewModel ASCodingEditor { get; set; }

        public codeEditViewModel CodingEditor { get; set; }


        public graphicsEditViewModel GraphicsEditor { get; set; }

        public filmEditViewModel FilmEditor { get; set; }

        public flexiEditViewModel FlexiEditor{ get; set; }



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
            try
            {
                Main = new MainWindowViewModel(this, _authService);
                Dashboard = new LogCachePage();
                Logger = new LoggerCreatePage();
                Editor = new LoggerEditPage();
                _frame = Logger.frame_VARIATIONS;
                _ASframe = Logger.frame_ANDROIDSTUDIO;
                _EDS_frame = Editor.frame_VARIATIONS;
                _EDS_ASframe = Editor.frame_ANDROIDSTUDIO;

                codingQtDashboard = new CodingQtViewModel(this, _dataService);
                codingAndroidDashboard = new CodingAndroidViewModel(this, _dataService);
                codingDashboard = new CodingGenericViewModel(this, _dataService);
                graphicsDashboard = new GraphicsViewModel(this, _dataService);
                filmDashboard = new FilmViewModel(this, _dataService);
                flexiDashboard = new FlexiViewModel(this, _dataService);

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
            catch (Exception)
            {
                // TODO
            }
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

            _dataService.SetAccount(_authService.Account);

            _dataService.InitialiseApplicationsLIST();
            _dataService.InitialiseProjectsLIST();

            setup();


            if (LoginWindow is null)
                return;

            var list = _dataService.RetrieveQtCache(codingQtDashboard, _dataService);

            if(list is not null && list.Count > 0)
                codingQtDashboard.CacheItems = list;

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
            try
            {
                if (codingQtDashboard is null)
                    return;

                ChangeData(context);
                _MainFrame.Navigate(Dashboard);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void ChangeData(CacheContext context)
        {


            try
            {

                if (codingQtDashboard is null || _frame is null)
                    return;

                _frame.Navigate(CodingFrame);

                Logger.inputText_APP.UserComboInput = "";
                Logger.inputText_APP.showPlaceholderText();
                CurrentCategory = LOG.CATEGORY.CODING;

                switch (context)
                {
                    case CacheContext.Qt:
                        Dashboard.DataContext = codingQtDashboard;
                        QtCodingLogger.UpdateLogCount();
                        QtCodingLogger.Setup();
                        Logger.DataContext = QtCodingLogger;
                        showLOGGERPLACEHOLDERS(false);
                        CodingFrame.DataContext = QtCodingLogger;
                        _ASframe.Navigate(null);
                        break;

                    case CacheContext.AndroidStudio:
                        Dashboard.DataContext = codingAndroidDashboard;
                        ASCodingLogger.UpdateLogCount();
                        ASCodingLogger.Setup();
                        Logger.DataContext = ASCodingLogger;
                        showLOGGERPLACEHOLDERS(false);
                        CodingFrame.DataContext = ASCodingLogger;
                        _ASframe.Navigate(AndroidStudioFrame);
                        break;
                    case CacheContext.Generic:
                        Dashboard.DataContext = codingDashboard;
                        CodingLogger.UpdateLogCount();
                        CodingLogger.Setup();
                        Logger.DataContext = CodingLogger;
                        showLOGGERPLACEHOLDERS(true);
                        CodingFrame.DataContext = CodingLogger;
                        _ASframe.Navigate(null);
                        break;
                    case CacheContext.Graphics:
                        CurrentCategory = LOG.CATEGORY.GRAPHICS;
                        Dashboard.DataContext = graphicsDashboard;
                        Logger.DataContext = GraphicsLogger;
                        GraphicsFrame.DataContext = GraphicsLogger;
                        _frame.Navigate(GraphicsFrame);
                        _ASframe.Navigate(null);
                        break;
                    case CacheContext.Film:
                        CurrentCategory = LOG.CATEGORY.FILM;
                        Dashboard.DataContext = filmDashboard;
                        Logger.DataContext = FilmLogger;
                        FilmFrame.DataContext = FilmLogger;
                        _frame.Navigate(FilmFrame);
                        _ASframe.Navigate(null);
                        break;
                    case CacheContext.Flexi:
                        CurrentCategory = LOG.CATEGORY.NOTES;
                        Dashboard.DataContext = flexiDashboard;
                        Logger.DataContext = FlexiLogger;
                        FlexiFrame.DataContext = FlexiLogger;
                        _frame.Navigate(FlexiFrame);
                        _ASframe.Navigate(null);
                        break;

                }
            }
            catch (Exception)
            {
                // TODO
                return;
            }
        }

        public void showLOGGERPLACEHOLDERS(bool showApp)
        {
            Logger.inputText_PROJECT.showPlaceholderText();

            if(showApp)
            {
                Logger.inputText_APP.showPlaceholderText();
            }

            Logger.inputText_OUTPUT.showPlaceholderText();
            Logger.inputText_TYPE.showPlaceholderText();
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

        /* Use this for EditCommand */
        public void NavigateToLoggerEditor(LogCacheViewModel logCacheViewModel, ViewModelBase viewModelBase, CacheContext Category)
        {

            switch(Category)
            {
                case CacheContext.Qt:
                    {
                        CodingFrame = new();
                        CodingFrame.DataContext = QtCodingEditor;
                        _EDS_frame.Navigate(CodingFrame);
                        _EDS_ASframe.Navigate(null);

                        var log = (QtLOGViewModel)viewModelBase;
                        QtCodingEditor = new(this, logCacheViewModel, "Qt", _dataService, log);


                        QtCodingEditor.ProjectName = log._QtcodingLOG.Project.Name;
                        QtCodingEditor.ApplicationName = log._QtcodingLOG.Application.Name;
                        QtCodingEditor.StartDate = log._QtcodingLOG.StartTime;
                        QtCodingEditor.EndDate = log._QtcodingLOG.EndTime;
                        QtCodingEditor.Output = log._QtcodingLOG.Output.Name;
                        QtCodingEditor.Type = log._QtcodingLOG.Type.Name;

                        foreach (PostIt p in log._QtcodingLOG.PostItList)
                        {
                            QtCodingEditor.PostIts.Add(new CreatePostItViewModel(this, QtCodingEditor, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        QtCodingEditor.BugsFound = log._QtcodingLOG.Bugs;
                        QtCodingEditor.ApplicationOpened = log._QtcodingLOG.Success;

                        Editor.DataContext = QtCodingEditor;
                        Dashboard.DataContext = codingQtDashboard;

                        break;
                    }
            }

            _MainFrame.Navigate(Editor);
        }

        public void NavigateToPostItCreator(LoggerCreateViewModel loggerCreator)
        {
            PostItPage postItPage = _dataService.CurrentProject is null ? postItPage = new PostItPage(new CreatePostItViewModel(this, _dataService, loggerCreator, CurrentCategory)) :
                postItPage = new PostItPage(new CreatePostItViewModel(this, _dataService, loggerCreator, _dataService.CurrentProject));

            _MainFrame.Navigate(postItPage);
        }

        public void NavigateToReporter()
        {
            //
        }



        #endregion
    }
}
