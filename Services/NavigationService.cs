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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Data_Logger_1._3.Services
{
    public enum CacheContext
    {
        Qt,
        AndroidStudio,
        Coding,
        Graphics,
        Film,
        NOTES,
        Flexi
    }

    public class NavigationService
    {
        private readonly AuthService _authService;
        private readonly DataService _dataService;
        bool editControlsLoaded = false, arrowFillSet = false;

        public LOG.CATEGORY CurrentCategory { get; set; } = LOG.CATEGORY.CODING;

        private const string Qt = "Qt Creator";
        protected const string AndroidStudio = "Android Studio Hedgehog 2023.1.1";
        protected const string VisualStudio = "Visual Studio Community 2022";

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

        public CodingViewModel codingDashboard { get; set; }

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

        private bool editorProfilePictureSet = false;

        public AScodeEditViewModel ASCodingEditor { get; set; }

        public codeEditViewModel CodingEditor { get; set; }


        public graphicsEditViewModel GraphicsEditor { get; set; }

        public filmEditViewModel FilmEditor { get; set; }

        public flexiEditViewModel FlexiEditor { get; set; }



        #endregion


        #region Frames



        public coding_UserControl CodingFrame { get; set; }

        public androidStudio_UserControl AndroidStudioFrame { get; set; }

        public graphics_UserControl GraphicsFrame { get; set; }

        public film_UserControl FilmFrame { get; set; }

        public flexi_UserControl FlexiFrame { get; set; }


        public coding_UserControl EditCodingFrame { get; set; }
        public androidStudio_UserControl EditAndroidStudioFrame { get; set; }
        public graphics_UserControl EditGraphicsFrame { get; set; }
        public film_UserControl EditFilmFrame { get; set; }
        public flexi_UserControl EditFlexiFrame { get; set; }


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

                codingQtDashboard = new CodingQtViewModel(this, _dataService);
                codingAndroidDashboard = new CodingAndroidViewModel(this, _dataService);
                codingDashboard = new CodingViewModel(this, _dataService);
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

                createNotesPage = new CreateNotePage();
                createNotesPage.DataContext = new CreateNoteViewModel(this);
                createCheckListPage = new CreateCheckListPage();
                createCheckListPage.DataContext = new CreateCheckListViewModel(this);


                CodingFrame = new coding_UserControl();
                AndroidStudioFrame = new androidStudio_UserControl();
                AndroidStudioFrame.DataContext = ASCodingLogger;
                GraphicsFrame = new graphics_UserControl();
                FilmFrame = new film_UserControl();
                FlexiFrame = new flexi_UserControl();



                Dashboard = new LogCachePage();
                Logger = new LoggerCreatePage();
                Editor = new LoggerEditPage();
                _frame = Logger.frame_VARIATIONS;
                _ASframe = Logger.frame_ANDROIDSTUDIO;
                _EDS_frame = Editor.frame_VARIATIONS;
                _EDS_ASframe = Editor.frame_ANDROIDSTUDIO;

                NOTESList = new NOTESPage();
                NOTESList.DataContext = notesDashboard;
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void LoadEditControls()
        {
            if (!editControlsLoaded)
            {
                EditCodingFrame = new coding_UserControl();
                EditAndroidStudioFrame = new androidStudio_UserControl();
                EditAndroidStudioFrame.DataContext = ASCodingEditor;
                EditGraphicsFrame = new graphics_UserControl();
                EditFilmFrame = new film_UserControl();
                EditFlexiFrame = new flexi_UserControl();

                editControlsLoaded = true;
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
                Main.BackEnabled = _MainFrame.CanGoBack;
                Main.ForwardEnabled = _MainFrame.CanGoForward;
                return;
            }

            Main.BackEnabled = false;

        }

        public void GoForward()
        {
            if (_MainFrame.CanGoForward)
            {
                _MainFrame.GoForward();
                Main.ForwardEnabled = _MainFrame.CanGoForward;
                Main.BackEnabled = _MainFrame.CanGoBack;
                return;
            }

            Main.ForwardEnabled = false;
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
            if (IsLogin)
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

            var qtList = _dataService.RetrieveQtCache(codingQtDashboard, _dataService);
            int cachedItems = 0, cachedPostIts = 0;

            if (qtList is not null && qtList.Count > 0)
            {
                codingQtDashboard.CacheItems = qtList;

                cachedItems += qtList.Count;


                foreach (QtLOGViewModel item in qtList)
                {
                    cachedPostIts += item._QtcodingLOG.PostItList.Count;
                }
            }



            var asList = _dataService.RetrieveASCache(codingAndroidDashboard, _dataService);

            if (asList is not null && asList.Count > 0)
            {
                codingAndroidDashboard.CacheItems = asList;

                cachedItems += asList.Count;

                foreach (AndroidLOGViewModel item in asList)
                {
                    cachedPostIts += item._AndroidCodingLOG.PostItList.Count;
                }
            }

            var cdeList = _dataService.RetrieveCodeCache(codingDashboard, _dataService);

            if (cdeList is not null && cdeList.Count > 0)
            {
                codingDashboard.CacheItems = cdeList;

                cachedItems += cdeList.Count;

                foreach (CodeLOGViewModel item in cdeList)
                {
                    cachedPostIts += item._CodeLOG.PostItList.Count;
                }
            }



            // TODO
            // Do for the other log types.

            _dataService.UpdateWatcher(cachedItems, cachedPostIts);
            _dataService.UpdateAvailablePostItIDs(_dataService.RetrieveSubjectIndex());
            _dataService.UpdateAvailableSubjectIDs(_dataService.RetrieveSubjectIndex());

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
            if (SignUpWindow is not null)
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
                Main.BackEnabled = _MainFrame.CanGoBack;
                Main.ForwardEnabled = _MainFrame.CanGoForward;
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


                CurrentCategory = LOG.CATEGORY.CODING;
                var profilePic = _authService.Account.ProfilePic; ;

                switch (context)
                {
                    case CacheContext.Qt:
                        QtCodingLogger = new codeCreateViewModel(this, codingQtDashboard, "Qt", _dataService);
                        QtCodingLogger.SignUpImage = profilePic;
                        QtCodingLogger.UpdateLogCount();
                        Dashboard.DataContext = codingQtDashboard;
                        Logger.DataContext = QtCodingLogger;
                        CodingFrame.DataContext = QtCodingLogger;
                        _frame.Navigate(CodingFrame);
                        _ASframe.Navigate(null);
                        break;

                    case CacheContext.AndroidStudio:
                        Dashboard.DataContext = codingAndroidDashboard;
                        ASCodingLogger = new(this, codingAndroidDashboard, _dataService);
                        ASCodingLogger.SignUpImage = profilePic;
                        ASCodingLogger.UpdateLogCount();
                        Logger.DataContext = ASCodingLogger;
                        CodingFrame.DataContext = ASCodingLogger;
                        _frame.Navigate(CodingFrame);
                        _ASframe.Navigate(AndroidStudioFrame);
                        break;
                    case CacheContext.Coding:
                        Dashboard.DataContext = codingDashboard;
                        CodingLogger = new(this, codingDashboard, _dataService);
                        CodingLogger.SignUpImage = profilePic;
                        CodingLogger.UpdateLogCount();
                        Logger.DataContext = CodingLogger;
                        CodingFrame.DataContext = CodingLogger;
                        _frame.Navigate(CodingFrame);
                        _ASframe.Navigate(null);
                        break;
                    case CacheContext.Graphics:
                        CurrentCategory = LOG.CATEGORY.GRAPHICS;
                        Dashboard.DataContext = graphicsDashboard;
                        GraphicsLogger = new(this, graphicsDashboard, _dataService);
                        GraphicsLogger.SignUpImage = profilePic;
                        GraphicsLogger.UpdateLogCount();
                        Logger.DataContext = GraphicsLogger;
                        GraphicsFrame.DataContext = GraphicsLogger;
                        _frame.Navigate(GraphicsFrame);
                        _ASframe.Navigate(null);
                        break;
                    case CacheContext.Film:
                        CurrentCategory = LOG.CATEGORY.FILM;
                        Dashboard.DataContext = filmDashboard;
                        FilmLogger = new(this, filmDashboard, _dataService);
                        FilmLogger.SignUpImage = profilePic;
                        FilmLogger.UpdateLogCount();
                        Logger.DataContext = FilmLogger;
                        FilmFrame.DataContext = FilmLogger;
                        _frame.Navigate(FilmFrame);
                        _ASframe.Navigate(null);
                        break;
                    case CacheContext.Flexi:
                        CurrentCategory = LOG.CATEGORY.NOTES;
                        Dashboard.DataContext = flexiDashboard;
                        FlexiLogger = new(this, flexiDashboard, _dataService);
                        FlexiLogger.SignUpImage = profilePic;
                        FlexiLogger.UpdateLogCount();
                        Logger.DataContext = FlexiLogger;
                        FlexiFrame.DataContext = FlexiLogger;
                        _frame.Navigate(FlexiFrame);
                        _ASframe.Navigate(null);
                        break;

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near ChangeData(context): {ex.Message}");
                
                // TODO


            if (showApp)
            {
                Logger.inputText_APP.showPlaceholderText();
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

            Main.BackEnabled = true;
            GoForward();
        }

        /* Use this for EditCommand */
        public void NavigateToLoggerEditor(LogCacheViewModel logCacheViewModel, ViewModelBase viewModelBase, CacheContext Category)
        {

            switch (Category)
            {
                case CacheContext.Qt:
                    {

                        var log = (QtLOGViewModel)viewModelBase;
                        QtCodingEditor = new(this, logCacheViewModel, "Qt", _dataService, log);

                        QtCodingEditor.SignUpImage = _authService.Account.ProfilePic;

                        QtCodingEditor.ProjectName = log._QtcodingLOG.Project.Name;
                        QtCodingEditor.ApplicationName = log._QtcodingLOG.Application.Name;

                        QtCodingEditor.StartDate = log._QtcodingLOG.Start;
                        QtCodingEditor.StartHours = log._QtcodingLOG.Start.Hour;
                        QtCodingEditor.StartMinutes = log._QtcodingLOG.Start.Minute;
                        QtCodingEditor.StartSeconds = log._QtcodingLOG.Start.Second;
                        QtCodingEditor.StartMilliseconds = log._QtcodingLOG.Start.Millisecond;

                        QtCodingEditor.EndDate = log._QtcodingLOG.End;
                        QtCodingEditor.EndHours = log._QtcodingLOG.End.Hour;
                        QtCodingEditor.EndMinutes = log._QtcodingLOG.End.Minute;
                        QtCodingEditor.EndSeconds = log._QtcodingLOG.End.Second;
                        QtCodingEditor.EndMilliseconds = log._QtcodingLOG.End.Millisecond;

                        QtCodingEditor.Output = log._QtcodingLOG.Output.Name;
                        QtCodingEditor.Type = log._QtcodingLOG.Type.Name;

                        foreach (PostIt p in log._QtcodingLOG.PostItList)
                        {
                            QtCodingEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, QtCodingEditor, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        QtCodingEditor.BugsFound = log._QtcodingLOG.Bugs;
                        QtCodingEditor.ApplicationOpened = log._QtcodingLOG.Success;

                        Dashboard.DataContext = codingQtDashboard;
                        Editor.DataContext = QtCodingEditor;
                        LoadEditControls();
                        _EDS_frame.Navigate(EditCodingFrame);
                        _EDS_ASframe.Navigate(null);
                        EditCodingFrame.DataContext = QtCodingEditor;

                        break;
                    }
                case CacheContext.AndroidStudio:
                    {
                        var log = (AndroidLOGViewModel)viewModelBase;
                        ASCodingEditor = new(this, logCacheViewModel, _dataService, log);

                        ASCodingEditor.SignUpImage = _authService.Account.ProfilePic;

                        ASCodingEditor.ProjectName = log._AndroidCodingLOG.Project.Name;
                        ASCodingEditor.ApplicationName = log._AndroidCodingLOG.Application.Name;

                        ASCodingEditor.StartDate = log._AndroidCodingLOG.Start;
                        ASCodingEditor.StartHours = log._AndroidCodingLOG.Start.Hour;
                        ASCodingEditor.StartMinutes = log._AndroidCodingLOG.Start.Minute;
                        ASCodingEditor.StartSeconds = log._AndroidCodingLOG.Start.Second;
                        ASCodingEditor.StartMilliseconds = log._AndroidCodingLOG.Start.Millisecond;

                        ASCodingEditor.EndDate = log._AndroidCodingLOG.End;
                        ASCodingEditor.EndHours = log._AndroidCodingLOG.End.Hour;
                        ASCodingEditor.EndMinutes = log._AndroidCodingLOG.End.Minute;
                        ASCodingEditor.EndSeconds = log._AndroidCodingLOG.End.Second;
                        ASCodingEditor.EndMilliseconds = log._AndroidCodingLOG.End.Millisecond;

                        ASCodingEditor.Output = log._AndroidCodingLOG.Output.Name;
                        ASCodingEditor.Type = log._AndroidCodingLOG.Type.Name;

                        foreach (PostIt p in log._AndroidCodingLOG.PostItList)
                        {
                            ASCodingEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, ASCodingEditor, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        ASCodingEditor.BugsFound = log._AndroidCodingLOG.Bugs;
                        ASCodingEditor.ApplicationOpened = log._AndroidCodingLOG.Success;

                        ASCodingEditor.FullORSimple = !log._AndroidCodingLOG.Scope.Equals(AndroidCodingLOG.SCOPE.FULL);
                        ASCodingEditor.SyncTime = log._AndroidCodingLOG.Sync;
                        ASCodingEditor.SyncHours = log._AndroidCodingLOG.Sync.Hour;
                        ASCodingEditor.SyncMinutes = log._AndroidCodingLOG.Sync.Minute;
                        ASCodingEditor.SyncSeconds = log._AndroidCodingLOG.Sync.Second;
                        ASCodingEditor.SyncMilliseconds = log._AndroidCodingLOG.Sync.Millisecond;

                        ASCodingEditor.GradleDaemonVisibility = ASCodingEditor.FullORSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASCodingEditor.GradleDaemonTime = log._AndroidCodingLOG.StartingGradleDaemon;
                        ASCodingEditor.GradleDaemonHours = log._AndroidCodingLOG.StartingGradleDaemon.Hour;
                        ASCodingEditor.GradleDaemonMinutes = log._AndroidCodingLOG.StartingGradleDaemon.Minute;
                        ASCodingEditor.GradleDaemonSeconds = log._AndroidCodingLOG.StartingGradleDaemon.Second;
                        ASCodingEditor.GradleDaemonMilliseconds = log._AndroidCodingLOG.StartingGradleDaemon.Millisecond;

                        ASCodingEditor.RunBuildVisibility = ASCodingEditor.FullORSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASCodingEditor.RunBuildTime = log._AndroidCodingLOG.RunBuild;
                        ASCodingEditor.RunBuildHours = log._AndroidCodingLOG.RunBuild.Hour;
                        ASCodingEditor.RunBuildMinutes = log._AndroidCodingLOG.RunBuild.Minute;
                        ASCodingEditor.RunBuildSeconds = log._AndroidCodingLOG.RunBuild.Second;
                        ASCodingEditor.RunBuildMilliseconds = log._AndroidCodingLOG.RunBuild.Millisecond;

                        ASCodingEditor.LoadBuildVisibility = ASCodingEditor.FullORSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASCodingEditor.LoadBuildTime = log._AndroidCodingLOG.LoadBuild;
                        ASCodingEditor.LoadBuildHours = log._AndroidCodingLOG.LoadBuild.Hour;
                        ASCodingEditor.LoadBuildMinutes = log._AndroidCodingLOG.LoadBuild.Minute;
                        ASCodingEditor.LoadBuildSeconds = log._AndroidCodingLOG.LoadBuild.Second;
                        ASCodingEditor.LoadBuildMilliseconds = log._AndroidCodingLOG.LoadBuild.Millisecond;

                        ASCodingEditor.ConfigureBuildVisibility = ASCodingEditor.FullORSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASCodingEditor.ConfigureBuildTime = log._AndroidCodingLOG.ConfigureBuild;
                        ASCodingEditor.ConfigureBuildHours = log._AndroidCodingLOG.ConfigureBuild.Hour;
                        ASCodingEditor.ConfigureBuildMinutes = log._AndroidCodingLOG.ConfigureBuild.Minute;
                        ASCodingEditor.ConfigureBuildSeconds = log._AndroidCodingLOG.ConfigureBuild.Second;
                        ASCodingEditor.ConfigureBuildMilliseconds = log._AndroidCodingLOG.ConfigureBuild.Millisecond;
                        
                        ASCodingEditor.AllProjectsVisibility = ASCodingEditor.FullORSimple? Visibility.Collapsed: Visibility.Visible;
                        ASCodingEditor.AllProjectsTime = log._AndroidCodingLOG.AllProjects;
                        ASCodingEditor.AllProjectsHours = log._AndroidCodingLOG.AllProjects.Hour;
                        ASCodingEditor.AllProjectsMinutes = log._AndroidCodingLOG.AllProjects.Minute;
                        ASCodingEditor.AllProjectsSeconds = log._AndroidCodingLOG.AllProjects.Second;
                        ASCodingEditor.AllProjectsMilliseconds = log._AndroidCodingLOG.AllProjects.Millisecond;

                        Dashboard.DataContext = codingAndroidDashboard;
                        Editor.DataContext = ASCodingEditor;
                        LoadEditControls();
                        _EDS_frame.Navigate(EditCodingFrame);
                        _EDS_ASframe.Navigate(EditAndroidStudioFrame);
                        EditCodingFrame.DataContext = ASCodingEditor;

                        break;
                    }
                case CacheContext.Coding:
                    {

                        var log = (CodeLOGViewModel)viewModelBase;
                        CodingEditor = new(this, logCacheViewModel, _dataService, log);

                        CodingEditor.SignUpImage = _authService.Account.ProfilePic;

                        CodingEditor.ProjectName = log._CodeLOG.Project.Name;
                        CodingEditor.ApplicationName = log._CodeLOG.Application.Name;

                        CodingEditor.StartDate = log._CodeLOG.Start;
                        CodingEditor.StartHours = log._CodeLOG.Start.Hour;
                        CodingEditor.StartMinutes = log._CodeLOG.Start.Minute;
                        CodingEditor.StartSeconds = log._CodeLOG.Start.Second;
                        CodingEditor.StartMilliseconds = log._CodeLOG.Start.Millisecond;

                        CodingEditor.EndDate = log._CodeLOG.End;
                        CodingEditor.EndHours = log._CodeLOG.End.Hour;
                        CodingEditor.EndMinutes = log._CodeLOG.End.Minute;
                        CodingEditor.EndSeconds = log._CodeLOG.End.Second;
                        CodingEditor.EndMilliseconds = log._CodeLOG.End.Millisecond;

                        CodingEditor.Output = log._CodeLOG.Output.Name;
                        CodingEditor.Type = log._CodeLOG.Type.Name;

                        foreach (PostIt p in log._CodeLOG.PostItList)
                        {
                            CodingEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, CodingEditor, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        CodingEditor.BugsFound = log._CodeLOG.Bugs;
                        CodingEditor.ApplicationOpened = log._CodeLOG.Success;

                        Dashboard.DataContext = codingDashboard;
                        Editor.DataContext = CodingEditor;
                        LoadEditControls();
                        _EDS_frame.Navigate(EditCodingFrame);
                        _EDS_ASframe.Navigate(null);
                        EditCodingFrame.DataContext = CodingEditor;

                        break;
                    }
                case CacheContext.Graphics:
                    {
                        var log = (GraphicsLOGViewModel)viewModelBase;
                        GraphicsEditor = new(this, logCacheViewModel, _dataService, log);

                        GraphicsEditor.SignUpImage = _authService.Account.ProfilePic;

                        GraphicsEditor.ProjectName = log._GraphicsLOG.Project.Name;
                        GraphicsEditor.ApplicationName = log._GraphicsLOG.Application.Name;

                        GraphicsEditor.StartDate = log._GraphicsLOG.Start;
                        GraphicsEditor.StartHours = log._GraphicsLOG.Start.Hour;
                        GraphicsEditor.StartMinutes = log._GraphicsLOG.Start.Minute;
                        GraphicsEditor.StartSeconds = log._GraphicsLOG.Start.Second;
                        GraphicsEditor.StartMilliseconds = log._GraphicsLOG.Start.Millisecond;

                        GraphicsEditor.EndDate = log._GraphicsLOG.End;
                        GraphicsEditor.EndHours = log._GraphicsLOG.End.Hour;
                        GraphicsEditor.EndMinutes = log._GraphicsLOG.End.Minute;
                        GraphicsEditor.EndSeconds = log._GraphicsLOG.End.Second;
                        GraphicsEditor.EndMilliseconds = log._GraphicsLOG.End.Millisecond;

                        GraphicsEditor.Output = log._GraphicsLOG.Output.Name;
                        GraphicsEditor.Type = log._GraphicsLOG.Type.Name;

                        foreach (PostIt p in log._GraphicsLOG.PostItList)
                        {
                            GraphicsEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, GraphicsEditor, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }



                        Dashboard.DataContext = codingDashboard;
                        Editor.DataContext = CodingEditor;
                        LoadEditControls();
                        _EDS_frame.Navigate(EditCodingFrame);
                        _EDS_ASframe.Navigate(null);
                        EditCodingFrame.DataContext = CodingEditor;

                        break;
                    }
            }

            _MainFrame.Navigate(Editor);
        }

        public void NavigateToPostItCreator(LoggerCreateViewModel loggerCreator)
        {
            PostItPage postItPage = _dataService.CurrentProject is null ? new PostItPage(new CreatePostItViewModel(this, _dataService, loggerCreator, CurrentCategory)) :
                new PostItPage(new CreatePostItViewModel(this, _dataService, loggerCreator, _dataService.CurrentProject));

            _MainFrame.Navigate(postItPage);
        }

        public void NavigateToPostItEditor(LoggerCreateViewModel loggerCreator, CreatePostItViewModel createPostItViewModel)
        {
            var editPostItViewModel = new EditPostItViewModel(this, _dataService, loggerCreator, CurrentCategory, createPostItViewModel);

            PostItPage postItPage = new PostItPage(editPostItViewModel);

            _MainFrame.Navigate(postItPage);
        }

        public void NavigateToReporter()
        {
            //
        }



        #endregion




        #region DataService Functions




        public string UpdateProfilePic(string email)
        {
            if (DataService.IsValidEmail(email))
            {
                return _dataService.UpdateProfilePic(email);
            }

            return string.Empty;

        }














        #endregion
    }
}
