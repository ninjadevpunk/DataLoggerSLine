/*using Data_Logger_1._3.Components.Subcontrols;
using Data_Logger_1._3.Components.Subcontrols_View;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.Dialogs.Edit;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Data_Logger_1._3.ViewModels.Reporter;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.Reporter.Logs;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using Data_Logger_1._3.ViewModels.ViewerViewModels;
using Data_Logger_1._3.Views;
using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.LogPages;
using Data_Logger_1._3.Views.ReportPages;
using Microsoft.Extensions.DependencyInjection;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
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

    public class OLDNavigationService
    {
        private readonly AuthService _authService;
        private readonly DataService _dataService;
        private readonly PDFService _pdfService;
        bool editControlsLoaded = false, arrowFillSet = false;
        bool viewerControlsLoaded = false;
        bool reportEditControlsLoaded = false;
        bool reportViewerControlsLoaded = false;

        public LOG.CATEGORY CurrentCategory { get; set; } = LOG.CATEGORY.CODING;
        public CacheContext Context { get; set; } = CacheContext.Coding;

        private const string Qt = "Qt Creator";
        protected const string AndroidStudio = "Android Studio Meerkat 2024.3.1";
        protected const string VisualStudio = "Visual Studio Community 2022";

        private IServiceProvider _serviceProvider;
        private Frame _MainFrame; // MainWindow Frame
        private Frame _frame; // Sub class Logs Logger Frame
        private Frame _ASframe; // Android Studio Logger Frame
        private Frame _EDS_frame; // Sub class Logs Editor Frame
        private Frame _EDS_ASframe; // Android Studio Logs Editor Frame
        private Frame _VIEW_frame; // Sub class Logs Viewer Frame
        private Frame _VIEW_ASframe; // Android Studio Logs Viewer Frame

        private Frame _UPD_frame;
        private Frame _UPD_ASframe;
        private Frame _VIEW_frame_Report;
        private Frame _VIEW_ASframe_Report;

        public Login LoginWindow { get; set; }

        public SignUp SignUpWindow { get; set; }

        public MainWindowViewModel Main { get; set; }

        public LogCachePage Dashboard { get; set; }

        public LoggerCreatePage Logger { get; set; }

        public ReporterDashboard Reporter { get; set; }

        public LoggerEditPage Editor { get; set; }

        public ReporterEditPage Updater { get; set; }

        public LoggerViewPage Viewer { get; set; }

        public NOTESPage NOTESList { get; set; }



        #region Dashboard Contexts



        public CodingQtViewModel codingQtDashboard { get; set; }

        public CodingAndroidViewModel codingAndroidDashboard { get; set; }

        public CodingViewModel codingDashboard { get; set; }

        public GraphicsViewModel graphicsDashboard { get; set; }

        public FilmViewModel filmDashboard { get; set; }

        public NOTESViewModel notesDashboard { get; set; }

        public FlexiViewModel flexiDashboard { get; set; }


        public QtReportDeskViewModel QtReportDesk { get; set; }

        public ASReportDeskViewModel ASReportDesk { get; set; }

        public CodeReportDeskViewModel CodeReportDesk { get; set; }

        public GraphicsReportDeskViewModel GraphicsReportDesk { get; set; }

        public FilmReportDeskViewModel FilmReportDesk { get; set; }

        public FlexiReportDeskViewModel FlexiReportDesk { get; set; }


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


        // REPORTER

        public codeUpdateViewModel QtUpdater { get; set; }

        public AScodeUpdateViewModel ASUpdater { get; set; }

        public codeUpdateViewModel CodingUpdater { get; set; }

        public graphicsUpdateViewModel GraphicsUpdater { get; set; }

        public filmUpdateViewModel FilmUpdater { get; set; }

        public flexiUpdateViewModel FlexiUpdater { get; set; }





        #endregion


        #region Viewer Contexts




        public codeViewerViewModel QtCodingViewer { get; set; }

        public bool viewerProfilePictureSet = false;

        public AScodeViewerViewModel ASCodingViewer { get; set; }

        public codeViewerViewModel CodingViewer { get; set; }

        public graphicsViewerViewModel GraphicsViewer { get; set; }

        public filmViewerViewModel FilmViewer { get; set; }

        public flexiViewerViewModel FlexiViewer { get; set; }




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

        public coding_UserControl_View ViewerCodingFrame { get; set; }
        public androidStudio_UserControl_View ViewerAndroidStudioFrame { get; set; }
        public graphics_UserControl_View ViewerGraphicsFrame { get; set; }
        public film_UserControl_View ViewerFilmFrame { get; set; }
        public flexi_UserControl_View ViewerFlexiFrame { get; set; }


        public coding_UserControl UpdateCodingFrame { get; set; }
        public androidStudio_UserControl UpdateAndroidStudioFrame { get; set; }
        public graphics_UserControl UpdateGraphicsFrame { get; set; }
        public film_UserControl UpdateFilmFrame { get; set; }
        public flexi_UserControl UpdateFlexiFrame { get; set; }


        public coding_UserControl_View ReportViewerCodingFrame { get; set; }
        public androidStudio_UserControl_View ReportViewerAndroidStudioFrame { get; set; }
        public graphics_UserControl_View ReportViewerGraphicsFrame { get; set; }
        public film_UserControl_View ReportViewerFilmFrame { get; set; }
        public flexi_UserControl_View ReportViewerFlexiFrame { get; set; }



        #endregion

        public CreateNotePage createNotesPage { get; set; }

        public CreateCheckListPage createCheckListPage { get; set; }




        #region Constructors




        public OLDNavigationService(AuthService service, DataService dataService)
        {
            _authService = service;
            _dataService = dataService;
        }

        public OLDNavigationService(AuthService service, DataService dataService, PDFService pdfService)
        {
            _authService = service;
            _dataService = dataService;
            _pdfService = pdfService;
        }


        public OLDNavigationService(Frame frame, Frame aSframe)
        {
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

                QtReportDesk = new(this, _dataService, _pdfService);
                ASReportDesk = new(this, _dataService, _pdfService);
                CodeReportDesk = new(this, _dataService, _pdfService);
                GraphicsReportDesk = new(this, _dataService, _pdfService);
                FilmReportDesk = new(this, _dataService, _pdfService);
                FlexiReportDesk = new(this, _dataService, _pdfService);

                notesDashboard = new NOTESViewModel(this, _dataService);

                QtCodingLogger = new codeCreateViewModel(this, codingQtDashboard, "Qt", _dataService);
                ASCodingLogger = new AScodeCreateViewModel(this, codingAndroidDashboard, _dataService);
                CodingLogger = new codeCreateViewModel(this, codingDashboard, _dataService);
                GraphicsLogger = new graphicCreateViewModel(this, graphicsDashboard, _dataService);
                FilmLogger = new filmCreateViewModel(this, filmDashboard, _dataService);
                FlexiLogger = new flexiCreateViewModel(this, flexiDashboard, _dataService);

                createNotesPage = new CreateNotePage();
                createNotesPage.DataContext = new CreateNoteViewModel(this, _dataService, notesDashboard);
                createCheckListPage = new CreateCheckListPage();
                createCheckListPage.DataContext = new CreateCheckListViewModel(this, _dataService, notesDashboard);


                CodingFrame = new coding_UserControl();
                AndroidStudioFrame = new androidStudio_UserControl();
                AndroidStudioFrame.DataContext = ASCodingLogger;
                GraphicsFrame = new graphics_UserControl();
                FilmFrame = new film_UserControl();
                FlexiFrame = new flexi_UserControl();



                Dashboard = new LogCachePage();
                Logger = new LoggerCreatePage();
                Reporter = new ReporterDashboard();
                Editor = new LoggerEditPage();
                Viewer = new LoggerViewPage();

                Updater = new ReporterEditPage();

                _frame = Logger.frame_VARIATIONS;
                _ASframe = Logger.frame_ANDROIDSTUDIO;

                _EDS_frame = Editor.frame_VARIATIONS;
                _EDS_ASframe = Editor.frame_ANDROIDSTUDIO;

                _VIEW_frame = Viewer.frame_VARIATIONS;
                _VIEW_ASframe = Viewer.frame_ANDROIDSTUDIO;

                _UPD_frame = Updater.frame_VARIATIONS;
                _UPD_ASframe = Updater.frame_ANDROIDSTUDIO;

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

        public void LoadViewerControls()
        {
            if (!viewerControlsLoaded)
            {
                ViewerCodingFrame = new coding_UserControl_View();
                ViewerAndroidStudioFrame = new androidStudio_UserControl_View();
                ViewerAndroidStudioFrame.DataContext = ASCodingViewer;
                ViewerGraphicsFrame = new graphics_UserControl_View();
                ViewerFilmFrame = new film_UserControl_View();
                ViewerFlexiFrame = new flexi_UserControl_View();

                viewerControlsLoaded = true;
            }
        }

        public void LoadReportViewerControls()
        {
            if (!reportViewerControlsLoaded)
            {
                ReportViewerCodingFrame = new coding_UserControl_View();
                ReportViewerAndroidStudioFrame = new androidStudio_UserControl_View();
                ReportViewerAndroidStudioFrame.DataContext = ASCodingViewer;
                ReportViewerGraphicsFrame = new graphics_UserControl_View();
                ReportViewerFilmFrame = new film_UserControl_View();
                ReportViewerFlexiFrame = new flexi_UserControl_View();

                reportViewerControlsLoaded = true;
            }
        }

        public void LoadReportEditControls()
        {
            if (!reportEditControlsLoaded)
            {
                UpdateCodingFrame = new coding_UserControl();
                UpdateAndroidStudioFrame = new androidStudio_UserControl();
                UpdateAndroidStudioFrame.DataContext = ASCodingEditor;
                UpdateGraphicsFrame = new graphics_UserControl();
                UpdateFilmFrame = new film_UserControl();
                UpdateFlexiFrame = new flexi_UserControl();

                reportEditControlsLoaded = true;
            }
        }


        #endregion









        #region Functions




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
            LoginWindow = _serviceProvider.GetRequiredService<Login>();
            var page = _serviceProvider.GetRequiredService<loginPage01>();


            LoginWindow.frame_LOGIN.Navigate(page);
            LoginWindow.Show();
        }

        public void NavigateToLogin(bool IsLogin)
        {
            if (IsLogin)
            {
                var page = _serviceProvider.GetRequiredService<loginPage01>();
                LoginWindow.frame_LOGIN.Navigate(page);
            }
            else
            {
                var page = _serviceProvider.GetRequiredService<loginPage02>();
                LoginWindow.frame_LOGIN.Navigate(page);
            }


            LoginWindow.Show();
        }

        public void NavigateToSignUp()
        {
            SignUpWindow = _serviceProvider.GetRequiredService<SignUp>();
            SignUpWindow.Show();
            LoginWindow.Hide();
        }

        public void NavigateToMainWindow()
        {
            if (_dataService is null)
            {
                MessageBox.Show("An error occurred on our end. We aplogise for any inconvenience.", "Error");

                Debug.WriteLine("DataService is null!");

                return;
            }

            _dataService.SetAccount(_authService.Account);

            _dataService.InitialiseApplicationsLISTAsync();
            _dataService.InitialiseProjectsLISTAsync();

            setup();


            if (LoginWindow is null)
            {
                MessageBox.Show("An error occurred on our end. We aplogise for any inconvenience.", "Error");

                Debug.WriteLine("LoginWindow is null!");

                return;
            }

            // Retrieve Qt Logs that weren't stored

            var qtList = _dataService.RetrieveQtCache(codingQtDashboard, _dataService);
            int cachedItemsCount = 0, cachedPostItsCount = 0;

            if (qtList is not null && qtList.Count > 0)
            {
                codingQtDashboard.CacheItems = qtList;

                cachedItemsCount += qtList.Count;


                foreach (QtLOGViewModel item in qtList)
                {
                    cachedPostItsCount += item._QtcodingLOG.PostItList.Count;
                }
            }

            // Retrieve AS Logs that weren't stored

            var asList = _dataService.RetrieveASCache(codingAndroidDashboard);

            if (asList is not null && asList.Count > 0)
            {
                codingAndroidDashboard.CacheItems = asList;

                cachedItemsCount += asList.Count;

                foreach (AndroidLOGViewModel item in asList)
                {
                    cachedPostItsCount += item._AndroidCodingLOG.PostItList.Count;
                }
            }

            // Retrieve Coding Logs that weren't stored

            var cdeList = _dataService.RetrieveCodeCache(codingDashboard);

            if (cdeList is not null && cdeList.Count > 0)
            {
                codingDashboard.CacheItems = cdeList;

                cachedItemsCount += cdeList.Count;

                foreach (CodeLOGViewModel item in cdeList)
                {
                    cachedPostItsCount += item._CodeLOG.PostItList.Count;
                }
            }


            // Retrieve Graphics Logs that weren't stored

            var graList = _dataService.RetrieveGraphicsCache(graphicsDashboard);

            if (graList is not null && graList.Count > 0)
            {
                graphicsDashboard.CacheItems = graList;

                cachedItemsCount += graList.Count;

                foreach (GraphicsLOGViewModel item in graList)
                {
                    cachedPostItsCount += item._GraphicsLOG.PostItList.Count;
                }
            }

            // Retrieve Film Logs that weren't stored

            var flmList = _dataService.RetrieveFilmCache(filmDashboard);

            if (flmList is not null && flmList.Count > 0)
            {
                filmDashboard.CacheItems = flmList;

                cachedItemsCount += flmList.Count;

                foreach (FilmLOGViewModel item in flmList)
                {
                    cachedPostItsCount += item._FilmLOG.PostItList.Count;
                }
            }

            // Retrieve Flexible Logs that weren't stored

            var flxList = _dataService.RetrieveFlexibleCache(flexiDashboard);

            if (flxList is not null && flxList.Count > 0)
            {
                flexiDashboard.CacheItems = flxList;

                cachedItemsCount += flxList.Count;

                foreach (FlexiLOGViewModel item in flxList)
                {
                    cachedPostItsCount += item._FlexiLOG.PostItList.Count;
                }
            }

            _dataService.UpdateWatcher(cachedItemsCount, cachedPostItsCount);
            _dataService.UpdateAvailablePostItIDs(_dataService.RetrieveSubjectIndex());
            _dataService.UpdateAvailableSubjectIDs(_dataService.RetrieveSubjectIndex());



            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
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
                Context = context;
                var profilePic = _authService.Account.ProfilePic; ;

                switch (context)
                {
                    case CacheContext.Qt:
                        CurrentCategory = LOG.CATEGORY.CODING;
                        QtCodingLogger = new codeCreateViewModel(this, codingQtDashboard, "Qt", _dataService);
                        QtCodingLogger.SignUpImage = profilePic;
                        QtCodingLogger.UpdateLogCount();

                        Dashboard.DataContext = codingQtDashboard;
                        Reporter.DataContext = QtReportDesk;

                        Logger.DataContext = QtCodingLogger;
                        CodingFrame.DataContext = QtCodingLogger;
                        _frame.Navigate(CodingFrame);
                        _ASframe.Navigate(null);
                        break;

                    case CacheContext.AndroidStudio:
                        CurrentCategory = LOG.CATEGORY.CODING;
                        ASCodingLogger = new(this, codingAndroidDashboard, _dataService);
                        ASCodingLogger.SignUpImage = profilePic;
                        ASCodingLogger.UpdateLogCount();

                        Dashboard.DataContext = codingAndroidDashboard;



                        Logger.DataContext = ASCodingLogger;
                        CodingFrame.DataContext = ASCodingLogger;
                        AndroidStudioFrame.DataContext = ASCodingLogger;
                        _frame.Navigate(CodingFrame);
                        _ASframe.Navigate(AndroidStudioFrame);
                        break;
                    case CacheContext.Coding:
                        CurrentCategory = LOG.CATEGORY.CODING;
                        CodingLogger = new(this, codingDashboard, _dataService);
                        CodingLogger.SignUpImage = profilePic;

                        Dashboard.DataContext = codingDashboard;



                        CodingLogger.UpdateLogCount();
                        Logger.DataContext = CodingLogger;
                        CodingFrame.DataContext = CodingLogger;
                        _frame.Navigate(CodingFrame);
                        _ASframe.Navigate(null);
                        break;
                    case CacheContext.Graphics:
                        CurrentCategory = LOG.CATEGORY.GRAPHICS;
                        GraphicsLogger = new(this, graphicsDashboard, _dataService);
                        GraphicsLogger.SignUpImage = profilePic;
                        GraphicsLogger.UpdateLogCount();

                        Dashboard.DataContext = graphicsDashboard;



                        Logger.DataContext = GraphicsLogger;
                        GraphicsFrame.DataContext = GraphicsLogger;
                        _frame.Navigate(GraphicsFrame);
                        _ASframe.Navigate(null);
                        break;
                    case CacheContext.Film:
                        CurrentCategory = LOG.CATEGORY.FILM;
                        FilmLogger = new(this, filmDashboard, _dataService);
                        FilmLogger.SignUpImage = profilePic;
                        FilmLogger.UpdateLogCount();

                        Dashboard.DataContext = filmDashboard;



                        Logger.DataContext = FilmLogger;
                        FilmFrame.DataContext = FilmLogger;
                        _frame.Navigate(FilmFrame);
                        _ASframe.Navigate(null);
                        break;
                    case CacheContext.Flexi:
                        CurrentCategory = LOG.CATEGORY.NOTES;
                        FlexiLogger = new(this, flexiDashboard, _dataService);
                        FlexiLogger.SignUpImage = profilePic;
                        FlexiLogger.UpdateLogCount();

                        Dashboard.DataContext = flexiDashboard;



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


                return;
            }
        }


        public void NavigateToNOTESDashboard()
        {

            try
            {
                try
                {
                    var list = notesDashboard.NoteItems;
                    list.Clear();

                    foreach (var item in _dataService.RetrieveNotes())
                    {
                        list.Add(new NoteLOGViewModel(_dataService, notesDashboard, (NoteItem)item));
                    }

                    notesDashboard.NoteItems = list;
                }
                catch (Exception ex)
                {
                    //
                }

                
                _MainFrame.Navigate(NOTESList);
            }
            catch (Exception ex)
            {
                //
            }
        }

        public void NavigateToCreateNotesPage()
        {
            createNotesPage.DataContext = new CreateNoteViewModel(this, _dataService, notesDashboard);
            Main.GenericNotesChecked = true;
            _MainFrame.Navigate(createNotesPage);
        }

        public void NavigateToCreateCheckListPage()
        {
            createCheckListPage.DataContext = new CreateCheckListViewModel(this, _dataService, notesDashboard);

            Main.ChecklistNotesChecked = true;
            _MainFrame.Navigate(createCheckListPage);
        }

        public void NavigateToLoggerCreator()
        {

            _MainFrame.Navigate(Logger);

            Main.BackEnabled = true;
            //GoForward();
        }

        *//* Use this for EditCommand *//*
        public void NavigateToLoggerEditor(LogCacheViewModel logCacheViewModel, ViewModelBase viewModelBase, CacheContext Category)
        {

            switch (Context)
            {
                case CacheContext.Qt:
                    {

                        var log = (QtLOGViewModel)viewModelBase;
                        QtCodingEditor = new(this, logCacheViewModel, "Qt", _dataService, log);

                        QtCodingEditor.SignUpImage = _authService.Account.ProfilePic;

                        QtCodingEditor.Author = $"{log._QtcodingLOG.Author.FirstName} {log._QtcodingLOG.Author.LastName}";
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
                            QtCodingEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, QtCodingEditor, log._QtcodingLOG.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
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

                        ASCodingEditor.Author = $"{log._AndroidCodingLOG.Author.FirstName} {log._AndroidCodingLOG.Author.LastName}";
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
                            ASCodingEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, ASCodingEditor, log._AndroidCodingLOG.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        ASCodingEditor.BugsFound = log._AndroidCodingLOG.Bugs;
                        ASCodingEditor.ApplicationOpened = log._AndroidCodingLOG.Success;

                        ASCodingEditor.IsSimple = log._AndroidCodingLOG.Scope.Equals(AndroidCodingLOG.SCOPE.SIMPLE);
                        ASCodingEditor.SyncTime = log._AndroidCodingLOG.Sync;
                        ASCodingEditor.SyncHours = log._AndroidCodingLOG.Sync.Hour;
                        ASCodingEditor.SyncMinutes = log._AndroidCodingLOG.Sync.Minute;
                        ASCodingEditor.SyncSeconds = log._AndroidCodingLOG.Sync.Second;
                        ASCodingEditor.SyncMilliseconds = log._AndroidCodingLOG.Sync.Millisecond;

                        ASCodingEditor.GradleDaemonVisibility = ASCodingEditor.IsSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASCodingEditor.GradleDaemonTime = log._AndroidCodingLOG.StartingGradleDaemon;
                        ASCodingEditor.GradleDaemonHours = log._AndroidCodingLOG.StartingGradleDaemon.Hour;
                        ASCodingEditor.GradleDaemonMinutes = log._AndroidCodingLOG.StartingGradleDaemon.Minute;
                        ASCodingEditor.GradleDaemonSeconds = log._AndroidCodingLOG.StartingGradleDaemon.Second;
                        ASCodingEditor.GradleDaemonMilliseconds = log._AndroidCodingLOG.StartingGradleDaemon.Millisecond;

                        ASCodingEditor.RunBuildVisibility = ASCodingEditor.IsSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASCodingEditor.RunBuildTime = log._AndroidCodingLOG.RunBuild;
                        ASCodingEditor.RunBuildHours = log._AndroidCodingLOG.RunBuild.Hour;
                        ASCodingEditor.RunBuildMinutes = log._AndroidCodingLOG.RunBuild.Minute;
                        ASCodingEditor.RunBuildSeconds = log._AndroidCodingLOG.RunBuild.Second;
                        ASCodingEditor.RunBuildMilliseconds = log._AndroidCodingLOG.RunBuild.Millisecond;

                        ASCodingEditor.LoadBuildVisibility = ASCodingEditor.IsSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASCodingEditor.LoadBuildTime = log._AndroidCodingLOG.LoadBuild;
                        ASCodingEditor.LoadBuildHours = log._AndroidCodingLOG.LoadBuild.Hour;
                        ASCodingEditor.LoadBuildMinutes = log._AndroidCodingLOG.LoadBuild.Minute;
                        ASCodingEditor.LoadBuildSeconds = log._AndroidCodingLOG.LoadBuild.Second;
                        ASCodingEditor.LoadBuildMilliseconds = log._AndroidCodingLOG.LoadBuild.Millisecond;

                        ASCodingEditor.ConfigureBuildVisibility = ASCodingEditor.IsSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASCodingEditor.ConfigureBuildTime = log._AndroidCodingLOG.ConfigureBuild;
                        ASCodingEditor.ConfigureBuildHours = log._AndroidCodingLOG.ConfigureBuild.Hour;
                        ASCodingEditor.ConfigureBuildMinutes = log._AndroidCodingLOG.ConfigureBuild.Minute;
                        ASCodingEditor.ConfigureBuildSeconds = log._AndroidCodingLOG.ConfigureBuild.Second;
                        ASCodingEditor.ConfigureBuildMilliseconds = log._AndroidCodingLOG.ConfigureBuild.Millisecond;

                        ASCodingEditor.AllProjectsVisibility = ASCodingEditor.IsSimple ? Visibility.Collapsed : Visibility.Visible;
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
                        EditAndroidStudioFrame.DataContext = ASCodingEditor;

                        break;
                    }
                case CacheContext.Graphics:
                    {
                        var log = (GraphicsLOGViewModel)viewModelBase;
                        GraphicsEditor = new(this, logCacheViewModel, _dataService, log);

                        GraphicsEditor.SignUpImage = _authService.Account.ProfilePic;

                        GraphicsEditor.Author = $"{log._GraphicsLOG.Author.FirstName} {log._GraphicsLOG.Author.LastName}";
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
                            GraphicsEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, GraphicsEditor, log._GraphicsLOG.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        GraphicsEditor.Height = log._GraphicsLOG.Height.ToString();
                        GraphicsEditor.Width = log._GraphicsLOG.Width.ToString();
                        GraphicsEditor.Medium = log._GraphicsLOG.Medium.Medium;
                        GraphicsEditor.Format = log._GraphicsLOG.Format.Format;
                        GraphicsEditor.Brush = log._GraphicsLOG.Brush;
                        GraphicsEditor.MeasuringUnit = log._GraphicsLOG.Unit.Unit;
                        GraphicsEditor.Size = log._GraphicsLOG.Size;
                        GraphicsEditor.DPI = log._GraphicsLOG.DPI.ToString();
                        GraphicsEditor.ColourDepth = log._GraphicsLOG.Depth;
                        GraphicsEditor.IsCompleted = log._GraphicsLOG.IsCompleted;
                        GraphicsEditor.Source = log._GraphicsLOG.Source;


                        Dashboard.DataContext = graphicsDashboard;
                        Editor.DataContext = GraphicsEditor;
                        LoadEditControls();
                        _EDS_frame.Navigate(EditGraphicsFrame);
                        _EDS_ASframe.Navigate(null);
                        EditGraphicsFrame.DataContext = GraphicsEditor;

                        break;
                    }
                case CacheContext.Film:
                    {
                        var log = (FilmLOGViewModel)viewModelBase;
                        FilmEditor = new(this, logCacheViewModel, _dataService, log);

                        FilmEditor.SignUpImage = _authService.Account.ProfilePic;

                        FilmEditor.Author = $"{log._FilmLOG.Author.FirstName} {log._FilmLOG.Author.LastName}";
                        FilmEditor.ProjectName = log._FilmLOG.Project.Name;
                        FilmEditor.ApplicationName = log._FilmLOG.Application.Name;

                        FilmEditor.StartDate = log._FilmLOG.Start;
                        FilmEditor.StartHours = log._FilmLOG.Start.Hour;
                        FilmEditor.StartMinutes = log._FilmLOG.Start.Minute;
                        FilmEditor.StartSeconds = log._FilmLOG.Start.Second;
                        FilmEditor.StartMilliseconds = log._FilmLOG.Start.Millisecond;

                        FilmEditor.EndDate = log._FilmLOG.End;
                        FilmEditor.EndHours = log._FilmLOG.End.Hour;
                        FilmEditor.EndMinutes = log._FilmLOG.End.Minute;
                        FilmEditor.EndSeconds = log._FilmLOG.End.Second;
                        FilmEditor.EndMilliseconds = log._FilmLOG.End.Millisecond;

                        FilmEditor.Output = log._FilmLOG.Output.Name;
                        FilmEditor.Type = log._FilmLOG.Type.Name;

                        foreach (PostIt p in log._FilmLOG.PostItList)
                        {
                            FilmEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, FilmEditor, log._FilmLOG.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        FilmEditor.Height = log._FilmLOG.Height.ToString();
                        FilmEditor.Width = log._FilmLOG.Width.ToString();
                        FilmEditor.IsCompleted = log._FilmLOG.IsCompleted;
                        FilmEditor.Source = log._FilmLOG.Source;


                        Dashboard.DataContext = filmDashboard;
                        Editor.DataContext = FilmEditor;
                        LoadEditControls();
                        _EDS_frame.Navigate(EditFilmFrame);
                        _EDS_ASframe.Navigate(null);
                        EditFilmFrame.DataContext = FilmEditor;

                        break;
                    }
                case CacheContext.Flexi:
                    {
                        var log = (FlexiLOGViewModel)viewModelBase;
                        FlexiEditor = new(this, logCacheViewModel, _dataService, log);

                        FlexiEditor.SignUpImage = _authService.Account.ProfilePic;

                        FlexiEditor.Author = $"{log._FlexiLOG.Author.FirstName} {log._FlexiLOG.Author.LastName}";
                        FlexiEditor.ProjectName = log._FlexiLOG.Project.Name;
                        FlexiEditor.ApplicationName = log._FlexiLOG.Application.Name;

                        FlexiEditor.StartDate = log._FlexiLOG.Start;
                        FlexiEditor.StartHours = log._FlexiLOG.Start.Hour;
                        FlexiEditor.StartMinutes = log._FlexiLOG.Start.Minute;
                        FlexiEditor.StartSeconds = log._FlexiLOG.Start.Second;
                        FlexiEditor.StartMilliseconds = log._FlexiLOG.Start.Millisecond;

                        FlexiEditor.EndDate = log._FlexiLOG.End;
                        FlexiEditor.EndHours = log._FlexiLOG.End.Hour;
                        FlexiEditor.EndMinutes = log._FlexiLOG.End.Minute;
                        FlexiEditor.EndSeconds = log._FlexiLOG.End.Second;
                        FlexiEditor.EndMilliseconds = log._FlexiLOG.End.Millisecond;

                        FlexiEditor.Output = log._FlexiLOG.Output.Name;
                        FlexiEditor.Type = log._FlexiLOG.Type.Name;

                        foreach (PostIt p in log._FlexiLOG.PostItList)
                        {
                            FlexiEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, FlexiEditor, log._FlexiLOG.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        FlexiEditor.FlexibleLogCategory = log._FlexiLOG.flexinotetype.ToString();
                        FlexiEditor.Medium = log._FlexiLOG.Medium;
                        FlexiEditor.Format = log._FlexiLOG.Format;
                        FlexiEditor.Bitrate = log._FlexiLOG.Bitrate.ToString();
                        FlexiEditor.Duration = log._FlexiLOG.Length;
                        FlexiEditor.IsCompleted = log._FlexiLOG.IsCompleted;
                        FlexiEditor.Source = log._FlexiLOG.Source;


                        Dashboard.DataContext = flexiDashboard;
                        Editor.DataContext = FlexiEditor;
                        LoadEditControls();
                        _EDS_frame.Navigate(EditFlexiFrame);
                        _EDS_ASframe.Navigate(null);
                        EditFlexiFrame.DataContext = FlexiEditor;

                        break;
                    }
                default:
                    {

                        var log = (CodeLOGViewModel)viewModelBase;
                        CodingEditor = new(this, logCacheViewModel, _dataService, log);

                        CodingEditor.SignUpImage = _authService.Account.ProfilePic;

                        CodingEditor.Author = $"{log._CodeLOG.Author.FirstName} {log._CodeLOG.Author.LastName}";
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
                            CodingEditor.PostIts.Add(new CreatePostItViewModel(this, _dataService, CodingEditor, log._CodeLOG.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
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
            }

            _MainFrame.Navigate(Editor);
        }

        public void NavigateToReporterUpdater(REPORTViewModel reportViewModel, ReportDeskViewModel reportDeskViewModel)
        {
            switch (Context)
            {
                case CacheContext.Qt:
                    {

                        var qtReport = (qtREPORTViewModel)reportViewModel;
                        var log = qtReport.GetQtCodingLog;

                        QtUpdater = new(this, reportDeskViewModel, Qt, _dataService, qtReport, _pdfService);

                        QtUpdater.SignUpImage = _authService.Account.ProfilePic;

                        QtUpdater.Author = $"{log.Author.FirstName} {log.Author.LastName}";
                        QtUpdater.ProjectName = log.Project.Name;
                        QtUpdater.ApplicationName = log.Application.Name;

                        QtUpdater.StartDate = log.Start;
                        QtUpdater.StartHours = log.Start.Hour;
                        QtUpdater.StartMinutes = log.Start.Minute;
                        QtUpdater.StartSeconds = log.Start.Second;
                        QtUpdater.StartMilliseconds = log.Start.Millisecond;

                        QtUpdater.EndDate = log.End;
                        QtUpdater.EndHours = log.End.Hour;
                        QtUpdater.EndMinutes = log.End.Minute;
                        QtUpdater.EndSeconds = log.End.Second;
                        QtUpdater.EndMilliseconds = log.End.Millisecond;

                        QtUpdater.Output = log.Output.Name;
                        QtUpdater.Type = log.Type.Name;

                        foreach (PostIt p in log.PostItList)
                        {
                            QtUpdater.PostIts.Add(new CreateReporterPostItViewModel(this, _dataService, QtUpdater, log.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        QtUpdater.BugsFound = log.Bugs;
                        QtUpdater.ApplicationOpened = log.Success;

                        Dashboard.DataContext = codingQtDashboard;
                        Updater.DataContext = QtUpdater;
                        LoadReportEditControls();
                        _UPD_frame.Navigate(UpdateCodingFrame);
                        _UPD_ASframe.Navigate(null);
                        UpdateCodingFrame.DataContext = QtUpdater;

                        break;
                    }
                case CacheContext.AndroidStudio:
                    {
                        var asReport = (asREPORTViewModel)reportViewModel;
                        var log = asReport.GetAndroidCodingLog;

                        ASUpdater = new(this, reportDeskViewModel, _dataService, asReport, _pdfService);

                        ASUpdater.SignUpImage = _authService.Account.ProfilePic;

                        ASUpdater.Author = $"{log.Author.FirstName} {log.Author.LastName}";
                        ASUpdater.ProjectName = log.Project.Name;
                        ASUpdater.ApplicationName = log.Application.Name;

                        ASUpdater.StartDate = log.Start;
                        ASUpdater.StartHours = log.Start.Hour;
                        ASUpdater.StartMinutes = log.Start.Minute;
                        ASUpdater.StartSeconds = log.Start.Second;
                        ASUpdater.StartMilliseconds = log.Start.Millisecond;

                        ASUpdater.EndDate = log.End;
                        ASUpdater.EndHours = log.End.Hour;
                        ASUpdater.EndMinutes = log.End.Minute;
                        ASUpdater.EndSeconds = log.End.Second;
                        ASUpdater.EndMilliseconds = log.End.Millisecond;

                        ASUpdater.Output = log.Output.Name;
                        ASUpdater.Type = log.Type.Name;

                        foreach (PostIt p in log.PostItList)
                        {
                            ASUpdater.PostIts.Add(new CreateReporterPostItViewModel(this, _dataService, ASUpdater, log.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        ASUpdater.BugsFound = log.Bugs;
                        ASUpdater.ApplicationOpened = log.Success;

                        ASUpdater.IsSimple = log.Scope.Equals(AndroidCodingLOG.SCOPE.SIMPLE);
                        ASUpdater.SyncTime = log.Sync;
                        ASUpdater.SyncHours = log.Sync.Hour;
                        ASUpdater.SyncMinutes = log.Sync.Minute;
                        ASUpdater.SyncSeconds = log.Sync.Second;
                        ASUpdater.SyncMilliseconds = log.Sync.Millisecond;

                        ASUpdater.GradleDaemonVisibility = ASUpdater.IsSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASUpdater.GradleDaemonTime = log.StartingGradleDaemon;
                        ASUpdater.GradleDaemonHours = log.StartingGradleDaemon.Hour;
                        ASUpdater.GradleDaemonMinutes = log.StartingGradleDaemon.Minute;
                        ASUpdater.GradleDaemonSeconds = log.StartingGradleDaemon.Second;
                        ASUpdater.GradleDaemonMilliseconds = log.StartingGradleDaemon.Millisecond;

                        ASUpdater.RunBuildVisibility = ASUpdater.IsSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASUpdater.RunBuildTime = log.RunBuild;
                        ASUpdater.RunBuildHours = log.RunBuild.Hour;
                        ASUpdater.RunBuildMinutes = log.RunBuild.Minute;
                        ASUpdater.RunBuildSeconds = log.RunBuild.Second;
                        ASUpdater.RunBuildMilliseconds = log.RunBuild.Millisecond;

                        ASUpdater.LoadBuildVisibility = ASUpdater.IsSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASUpdater.LoadBuildTime = log.LoadBuild;
                        ASUpdater.LoadBuildHours = log.LoadBuild.Hour;
                        ASUpdater.LoadBuildMinutes = log.LoadBuild.Minute;
                        ASUpdater.LoadBuildSeconds = log.LoadBuild.Second;
                        ASUpdater.LoadBuildMilliseconds = log.LoadBuild.Millisecond;

                        ASUpdater.ConfigureBuildVisibility = ASUpdater.IsSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASUpdater.ConfigureBuildTime = log.ConfigureBuild;
                        ASUpdater.ConfigureBuildHours = log.ConfigureBuild.Hour;
                        ASUpdater.ConfigureBuildMinutes = log.ConfigureBuild.Minute;
                        ASUpdater.ConfigureBuildSeconds = log.ConfigureBuild.Second;
                        ASUpdater.ConfigureBuildMilliseconds = log.ConfigureBuild.Millisecond;

                        ASUpdater.AllProjectsVisibility = ASUpdater.IsSimple ? Visibility.Collapsed : Visibility.Visible;
                        ASUpdater.AllProjectsTime = log.AllProjects;
                        ASUpdater.AllProjectsHours = log.AllProjects.Hour;
                        ASUpdater.AllProjectsMinutes = log.AllProjects.Minute;
                        ASUpdater.AllProjectsSeconds = log.AllProjects.Second;
                        ASUpdater.AllProjectsMilliseconds = log.AllProjects.Millisecond;

                        Dashboard.DataContext = codingQtDashboard;
                        Updater.DataContext = QtUpdater;
                        LoadReportEditControls();
                        _UPD_frame.Navigate(UpdateCodingFrame);
                        _UPD_ASframe.Navigate(null);
                        ReportViewerCodingFrame.DataContext = QtUpdater;

                        Dashboard.DataContext = codingAndroidDashboard;
                        Updater.DataContext = ASUpdater;
                        LoadReportEditControls();
                        _UPD_frame.Navigate(UpdateCodingFrame);
                        _UPD_ASframe.Navigate(UpdateAndroidStudioFrame);
                        UpdateCodingFrame.DataContext = ASUpdater;
                        UpdateAndroidStudioFrame.DataContext = ASUpdater;

                        break;
                    }
                case CacheContext.Graphics:
                    {
                        var graphicsReport = (graphicsREPORTViewModel)reportViewModel;
                        var log = graphicsReport.GetGraphicsLog;

                        GraphicsUpdater = new(this, reportDeskViewModel, _dataService, graphicsReport, _pdfService);

                        GraphicsUpdater.SignUpImage = _authService.Account.ProfilePic;

                        GraphicsUpdater.Author = $"{log.Author.FirstName} {log.Author.LastName}";
                        GraphicsUpdater.ProjectName = log.Project.Name;
                        GraphicsUpdater.ApplicationName = log.Application.Name;

                        GraphicsUpdater.StartDate = log.Start;
                        GraphicsUpdater.StartHours = log.Start.Hour;
                        GraphicsUpdater.StartMinutes = log.Start.Minute;
                        GraphicsUpdater.StartSeconds = log.Start.Second;
                        GraphicsUpdater.StartMilliseconds = log.Start.Millisecond;

                        GraphicsUpdater.EndDate = log.End;
                        GraphicsUpdater.EndHours = log.End.Hour;
                        GraphicsUpdater.EndMinutes = log.End.Minute;
                        GraphicsUpdater.EndSeconds = log.End.Second;
                        GraphicsUpdater.EndMilliseconds = log.End.Millisecond;

                        GraphicsUpdater.Output = log.Output.Name;
                        GraphicsUpdater.Type = log.Type.Name;

                        foreach (PostIt p in log.PostItList)
                        {
                            GraphicsUpdater.PostIts.Add(new CreateReporterPostItViewModel(this, _dataService, GraphicsUpdater, log.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        GraphicsUpdater.Height = log.Height.ToString();
                        GraphicsUpdater.Width = log.Width.ToString();
                        GraphicsUpdater.Medium = log.Medium.Medium;
                        GraphicsUpdater.Format = log.Format;
                        GraphicsUpdater.Brush = log.Brush;
                        GraphicsUpdater.MeasuringUnit = log.Unit;
                        GraphicsUpdater.Size = log.Size;
                        GraphicsUpdater.DPI = log.DPI.ToString();
                        GraphicsUpdater.ColourDepth = log.Depth;
                        GraphicsUpdater.IsCompleted = log.IsCompleted;
                        GraphicsUpdater.Source = log.Source;

                        Dashboard.DataContext = codingQtDashboard;
                        Updater.DataContext = QtUpdater;
                        LoadReportEditControls();
                        _UPD_frame.Navigate(UpdateCodingFrame);
                        _UPD_ASframe.Navigate(null);
                        UpdateCodingFrame.DataContext = QtUpdater;

                        Dashboard.DataContext = graphicsDashboard;
                        Updater.DataContext = GraphicsUpdater;
                        LoadReportEditControls();
                        _UPD_frame.Navigate(UpdateGraphicsFrame);
                        _UPD_ASframe.Navigate(null);
                        UpdateGraphicsFrame.DataContext = GraphicsUpdater;

                        break;
                    }
                case CacheContext.Film:
                    {
                        var filmReport = (filmREPORTViewModel)reportViewModel;
                        var log = filmReport.GetFilmLog;

                        FilmUpdater = new(this, reportDeskViewModel, _dataService, filmReport, _pdfService);

                        FilmEditor.SignUpImage = _authService.Account.ProfilePic;

                        FilmUpdater.Author = $"{log.Author.FirstName} {log.Author.LastName}";
                        FilmUpdater.ProjectName = log.Project.Name;
                        FilmUpdater.ApplicationName = log.Application.Name;

                        FilmUpdater.StartDate = log.Start;
                        FilmUpdater.StartHours = log.Start.Hour;
                        FilmUpdater.StartMinutes = log.Start.Minute;
                        FilmUpdater.StartSeconds = log.Start.Second;
                        FilmUpdater.StartMilliseconds = log.Start.Millisecond;

                        FilmUpdater.EndDate = log.End;
                        FilmUpdater.EndHours = log.End.Hour;
                        FilmUpdater.EndMinutes = log.End.Minute;
                        FilmUpdater.EndSeconds = log.End.Second;
                        FilmUpdater.EndMilliseconds = log.End.Millisecond;

                        FilmUpdater.Output = log.Output.Name;
                        FilmUpdater.Type = log.Type.Name;

                        foreach (PostIt p in log.PostItList)
                        {
                            FilmUpdater.PostIts.Add(new CreateReporterPostItViewModel(this, _dataService, FilmUpdater, log.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        FilmUpdater.Height = log.Height.ToString();
                        FilmUpdater.Width = log.Width.ToString();
                        FilmUpdater.IsCompleted = log.IsCompleted;
                        FilmUpdater.Source = log.Source;


                        Dashboard.DataContext = filmDashboard;
                        Updater.DataContext = FilmUpdater;
                        LoadReportEditControls();
                        _UPD_frame.Navigate(UpdateFilmFrame);
                        _UPD_ASframe.Navigate(null);
                        UpdateFilmFrame.DataContext = FilmUpdater;

                        break;
                    }
                case CacheContext.Flexi:
                    {
                        var flexiReport = (flexiREPORTViewModel)reportViewModel;
                        var log = flexiReport.GetFlexiLog;

                        FlexiUpdater = new(this, reportDeskViewModel, _dataService, flexiReport, _pdfService);

                        FlexiUpdater.SignUpImage = _authService.Account.ProfilePic;

                        FlexiUpdater.Author = $"{log.Author.FirstName} {log.Author.LastName}";
                        FlexiUpdater.ProjectName = log.Project.Name;
                        FlexiUpdater.ApplicationName = log.Application.Name;

                        FlexiUpdater.StartDate = log.Start;
                        FlexiUpdater.StartHours = log.Start.Hour;
                        FlexiUpdater.StartMinutes = log.Start.Minute;
                        FlexiUpdater.StartSeconds = log.Start.Second;
                        FlexiUpdater.StartMilliseconds = log.Start.Millisecond;

                        FlexiUpdater.EndDate = log.End;
                        FlexiUpdater.EndHours = log.End.Hour;
                        FlexiUpdater.EndMinutes = log.End.Minute;
                        FlexiUpdater.EndSeconds = log.End.Second;
                        FlexiUpdater.EndMilliseconds = log.End.Millisecond;

                        FlexiUpdater.Output = log.Output.Name;
                        FlexiUpdater.Type = log.Type.Name;

                        foreach (PostIt p in log.PostItList)
                        {
                            FlexiUpdater.PostIts.Add(new CreateReporterPostItViewModel(this, _dataService, FlexiUpdater, log.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        FlexiUpdater.FlexibleLogCategory = log.flexinotetype.ToString();
                        FlexiUpdater.Medium = log.Medium.Medium;
                        FlexiUpdater.Format = log.Format;
                        FlexiUpdater.Bitrate = log.Bitrate.ToString();
                        FlexiUpdater.Duration = log.Length;
                        FlexiUpdater.IsCompleted = log.IsCompleted;
                        FlexiUpdater.Source = log.Source;


                        Dashboard.DataContext = flexiDashboard;
                        Updater.DataContext = FlexiUpdater;
                        LoadReportEditControls();
                        _UPD_frame.Navigate(UpdateFlexiFrame);
                        _UPD_ASframe.Navigate(null);
                        UpdateFlexiFrame.DataContext = FlexiUpdater;

                        break;
                    }
                default:
                    {
                        var codeReport = (codeREPORTViewModel)reportViewModel;
                        var log = codeReport.GetCodingLog;

                        CodingUpdater = new(this, reportDeskViewModel, _dataService, codeReport, _pdfService);

                        CodingUpdater.SignUpImage = _authService.Account.ProfilePic;

                        CodingUpdater.Author = $"{log.Author.FirstName} {log.Author.LastName}";
                        CodingUpdater.ProjectName = log.Project.Name;
                        CodingUpdater.ApplicationName = log.Application.Name;

                        CodingUpdater.StartDate = log.Start;
                        CodingUpdater.StartHours = log.Start.Hour;
                        CodingUpdater.StartMinutes = log.Start.Minute;
                        CodingUpdater.StartSeconds = log.Start.Second;
                        CodingUpdater.StartMilliseconds = log.Start.Millisecond;

                        CodingUpdater.EndDate = log.End;
                        CodingUpdater.EndHours = log.End.Hour;
                        CodingUpdater.EndMinutes = log.End.Minute;
                        CodingUpdater.EndSeconds = log.End.Second;
                        CodingUpdater.EndMilliseconds = log.End.Millisecond;

                        CodingUpdater.Output = log.Output.Name;
                        CodingUpdater.Type = log.Type.Name;

                        foreach (PostIt p in log.PostItList)
                        {
                            CodingUpdater.PostIts.Add(new CreateReporterPostItViewModel(this, _dataService, CodingUpdater, log.Project, p.Subject.Subject, p.Error, p.ERCaptureTime, p.Solution,
                                p.SOCaptureTime, p.Suggestion, p.Comment));
                        }

                        CodingUpdater.BugsFound = log.Bugs;
                        CodingUpdater.ApplicationOpened = log.Success;

                        Dashboard.DataContext = codingDashboard;
                        Updater.DataContext = CodingUpdater;
                        LoadReportEditControls();
                        _UPD_frame.Navigate(UpdateCodingFrame);
                        _UPD_ASframe.Navigate(null);
                        UpdateCodingFrame.DataContext = CodingUpdater;

                        break;
                    }
            }

            _MainFrame.Navigate(Updater);
        }



        public void NavigateToViewer(ViewModelBase viewModelBase, CacheContext cacheContext)
        {
            try
            {
                switch (cacheContext)
                {
                    case CacheContext.Qt:
                        {
                            var log = (QtLOGViewModel)viewModelBase;
                            QtCodingViewer = new(this, "Qt");

                            QtCodingViewer.SignUpImage = _authService.Account.ProfilePic;
                            QtCodingViewer.DisplayPicVisibility = QtCodingViewer.SignUpImage != "" ? Visibility.Collapsed : Visibility.Visible;

                            QtCodingViewer.Author = $"{log._QtcodingLOG.Author.FirstName} {log._QtcodingLOG.Author.LastName}";
                            QtCodingViewer.ProjectName = log._QtcodingLOG.Project.Name;
                            QtCodingViewer.ApplicationName = log._QtcodingLOG.Application.Name;

                            QtCodingViewer.Date = log._QtcodingLOG.Start.ToString("d MMMM yyyy HH:mm");

                            QtCodingViewer.Output = log._QtcodingLOG.Output.Name;
                            QtCodingViewer.Type = log._QtcodingLOG.Type.Name;

                            foreach (PostIt p in log._QtcodingLOG.PostItList)
                            {
                                QtCodingViewer.AddPostIt(new CreatePostItViewModel(this, log._QtcodingLOG.Project, p.Subject.Subject, p.Error, p.Solution,
                                    p.Suggestion, p.Comment));
                            }

                            QtCodingViewer.BugsFound = $"{log._QtcodingLOG.Bugs} Bugs found";
                            QtCodingViewer.ApplicationOpened = log._QtcodingLOG.Success ? "Launch Successful" : "Unsuccessful Launch";

                            Dashboard.DataContext = codingQtDashboard;
                            Viewer.DataContext = QtCodingViewer;
                            LoadViewerControls();
                            _VIEW_frame.Navigate(ViewerCodingFrame);
                            _VIEW_ASframe.Navigate(null);
                            ViewerCodingFrame.DataContext = QtCodingViewer;

                            break;
                        }
                    case CacheContext.AndroidStudio:
                        {
                            var log = (AndroidLOGViewModel)viewModelBase;
                            ASCodingViewer = new(this, log._AndroidCodingLOG.Scope.Equals(AndroidCodingLOG.SCOPE.SIMPLE));

                            ASCodingViewer.SignUpImage = _authService.Account.ProfilePic;
                            ASCodingViewer.DisplayPicVisibility = ASCodingViewer.SignUpImage != "" ? Visibility.Collapsed : Visibility.Visible;

                            ASCodingViewer.Author = $"{log._AndroidCodingLOG.Author.FirstName} {log._AndroidCodingLOG.Author.LastName}";
                            ASCodingViewer.ProjectName = log._AndroidCodingLOG.Project.Name;
                            ASCodingViewer.ApplicationName = log._AndroidCodingLOG.Application.Name;

                            ASCodingViewer.Date = log._AndroidCodingLOG.Start.ToString("d MMMM yyyy HH:mm"); ;

                            ASCodingViewer.Output = log._AndroidCodingLOG.Output.Name;
                            ASCodingViewer.Type = log._AndroidCodingLOG.Type.Name;

                            foreach (PostIt p in log._AndroidCodingLOG.PostItList)
                            {
                                ASCodingViewer.AddPostIt(new CreatePostItViewModel(this, log._AndroidCodingLOG.Project, p.Subject.Subject, p.Error, p.Solution,
                                    p.Suggestion, p.Comment));
                            }

                            ASCodingViewer.BugsFound = $"{log._AndroidCodingLOG.Bugs} Bugs found";
                            ASCodingViewer.ApplicationOpened = log._AndroidCodingLOG.Success ? "Launch Successful" : "Unsuccessful Launch";

                            ASCodingViewer.SyncTime = log._AndroidCodingLOG.Sync.ToString("HH:mm:ss.fff"); ;

                            ASCodingViewer.GradleDaemonTime = log._AndroidCodingLOG.StartingGradleDaemon.ToString("HH:mm:ss.fff");

                            ASCodingViewer.RunBuildTime = log._AndroidCodingLOG.RunBuild.ToString("HH:mm:ss.fff");

                            ASCodingViewer.LoadBuildTime = log._AndroidCodingLOG.LoadBuild.ToString("HH:mm:ss.fff");

                            ASCodingViewer.ConfigureBuildTime = log._AndroidCodingLOG.ConfigureBuild.ToString("HH:mm:ss.fff");

                            ASCodingViewer.AllProjectsTime = log._AndroidCodingLOG.AllProjects.ToString("HH:mm:ss.fff");

                            Dashboard.DataContext = codingAndroidDashboard;
                            Viewer.DataContext = ASCodingViewer;
                            LoadViewerControls();
                            _VIEW_frame.Navigate(ViewerCodingFrame);
                            _VIEW_ASframe.Navigate(ViewerAndroidStudioFrame);
                            ViewerCodingFrame.DataContext = ASCodingViewer;
                            ViewerAndroidStudioFrame.DataContext = ASCodingViewer;

                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            var log = (GraphicsLOGViewModel)viewModelBase;
                            GraphicsViewer = new(this);

                            GraphicsViewer.SignUpImage = _authService.Account.ProfilePic;
                            GraphicsViewer.DisplayPicVisibility = GraphicsViewer.SignUpImage != "" ? Visibility.Collapsed : Visibility.Visible;

                            GraphicsViewer.Author = $"{log._GraphicsLOG.Author.FirstName} {log._GraphicsLOG.Author.LastName}";
                            GraphicsViewer.ProjectName = log._GraphicsLOG.Project.Name;
                            GraphicsViewer.ApplicationName = log._GraphicsLOG.Application.Name;

                            GraphicsViewer.Date = log._GraphicsLOG.Start.ToString("d MMMM yyyy HH:mm");

                            GraphicsViewer.Output = log._GraphicsLOG.Output.Name;
                            GraphicsViewer.Type = log._GraphicsLOG.Type.Name;

                            foreach (PostIt p in log._GraphicsLOG.PostItList)
                            {
                                GraphicsViewer.AddPostIt(new CreatePostItViewModel(this, log._GraphicsLOG.Project, p.Subject.Subject, p.Error, p.Solution,
                                    p.Suggestion, p.Comment));
                            }

                            GraphicsViewer.HxW = $"{log._GraphicsLOG.Height.ToString()} x {log._GraphicsLOG.Width.ToString()}";
                            GraphicsViewer.Medium = log._GraphicsLOG.Medium;
                            GraphicsViewer.Format = log._GraphicsLOG.Format;
                            GraphicsViewer.Brush = log._GraphicsLOG.Brush;
                            GraphicsViewer.MeasuringUnit = log._GraphicsLOG.Unit;
                            GraphicsViewer.Size = log._GraphicsLOG.Size;
                            GraphicsViewer.DPI = log._GraphicsLOG.DPI.ToString();
                            GraphicsViewer.ColourDepth = log._GraphicsLOG.Depth;
                            GraphicsViewer.IsCompleted = log._GraphicsLOG.IsCompleted ? "Project Completed" : "Project Ongoing...";
                            GraphicsViewer.Source = $"Location: {log._GraphicsLOG.Source}";


                            Dashboard.DataContext = graphicsDashboard;
                            Viewer.DataContext = GraphicsViewer;
                            LoadViewerControls();
                            _VIEW_frame.Navigate(ViewerGraphicsFrame);
                            _VIEW_ASframe.Navigate(null);
                            ViewerGraphicsFrame.DataContext = GraphicsViewer;

                            break;
                        }
                    case CacheContext.Film:
                        {
                            var log = (FilmLOGViewModel)viewModelBase;
                            FilmViewer = new(this);

                            FilmViewer.SignUpImage = _authService.Account.ProfilePic;
                            FilmViewer.DisplayPicVisibility = FilmViewer.SignUpImage != "" ? Visibility.Collapsed : Visibility.Visible;

                            FilmViewer.Author = $"{log._FilmLOG.Author.FirstName} {log._FilmLOG.Author.LastName}";
                            FilmViewer.ProjectName = log._FilmLOG.Project.Name;
                            FilmViewer.ApplicationName = log._FilmLOG.Application.Name;

                            FilmViewer.Date = log._FilmLOG.Start.ToString("d MMMM yyyy HH:mm");

                            FilmViewer.Output = log._FilmLOG.Output.Name;
                            FilmViewer.Type = log._FilmLOG.Type.Name;

                            foreach (PostIt p in log._FilmLOG.PostItList)
                            {
                                FilmViewer.AddPostIt(new CreatePostItViewModel(this, log._FilmLOG.Project, p.Subject.Subject, p.Error, p.Solution,
                                    p.Suggestion, p.Comment));
                            }

                            FilmViewer.HxW = $"{log._FilmLOG.Height.ToString()} x {log._FilmLOG.Width.ToString()}";
                            FilmViewer.Length = log._FilmLOG.Length;
                            FilmViewer.IsCompleted = log._FilmLOG.IsCompleted ? "Project Completed" : "Project Ongoing...";
                            FilmViewer.Source = $"Location: {log._FilmLOG.Source}";


                            Dashboard.DataContext = filmDashboard;
                            Viewer.DataContext = FilmViewer;
                            LoadViewerControls();
                            _VIEW_frame.Navigate(ViewerFilmFrame);
                            _VIEW_ASframe.Navigate(null);
                            ViewerFilmFrame.DataContext = FilmViewer;

                            break;
                        }
                    case CacheContext.Flexi:
                        {
                            var log = (FlexiLOGViewModel)viewModelBase;
                            FlexiViewer = new(this);

                            FlexiViewer.SignUpImage = _authService.Account.ProfilePic;
                            FlexiViewer.DisplayPicVisibility = FlexiViewer.SignUpImage != "" ? Visibility.Collapsed : Visibility.Visible;

                            FlexiViewer.Author = $"{log._FlexiLOG.Author.FirstName} {log._FlexiLOG.Author.LastName}";
                            FlexiViewer.ProjectName = log._FlexiLOG.Project.Name;
                            FlexiViewer.ApplicationName = log._FlexiLOG.Application.Name;

                            FlexiViewer.Date = log._FlexiLOG.Start.ToString("d MMMM yyyy HH:mm");

                            FlexiViewer.Output = log._FlexiLOG.Output.Name;
                            FlexiViewer.Type = log._FlexiLOG.Type.Name;

                            foreach (PostIt p in log._FlexiLOG.PostItList)
                            {
                                FlexiViewer.AddPostIt(new CreatePostItViewModel(this, log._FlexiLOG.Project, p.Subject.Subject, p.Error, p.Solution,
                                    p.Suggestion, p.Comment));
                            }

                            FlexiViewer.FlexiNoteCategory = $"Flexible Log Type: {log._FlexiLOG.flexinotetype.ToString()}";
                            FlexiViewer.Medium = log._FlexiLOG.Medium.Medium;
                            FlexiViewer.Format = log._FlexiLOG.Format;
                            FlexiViewer.Bitrate = log._FlexiLOG.Bitrate.ToString();
                            FlexiViewer.Duration = log._FlexiLOG.Length;
                            FlexiViewer.IsCompleted = log._FlexiLOG.IsCompleted ? "Project Completed" : "Project Ongoing...";
                            FlexiViewer.Source = $"Location: {log._FlexiLOG.Source}";


                            Dashboard.DataContext = flexiDashboard;
                            Viewer.DataContext = FlexiViewer;
                            LoadViewerControls();
                            _VIEW_frame.Navigate(ViewerFlexiFrame);
                            _VIEW_ASframe.Navigate(null);
                            ViewerFlexiFrame.DataContext = FlexiViewer;

                            break;
                        }
                    default:
                        {
                            var log = (CodeLOGViewModel)viewModelBase;
                            CodingViewer = new(this);

                            CodingViewer.SignUpImage = _authService.Account.ProfilePic;
                            CodingViewer.DisplayPicVisibility = CodingViewer.SignUpImage != "" ? Visibility.Collapsed : Visibility.Visible;

                            CodingViewer.Author = $"{log._CodeLOG.Author.FirstName} {log._CodeLOG.Author.LastName}";
                            CodingViewer.ProjectName = log._CodeLOG.Project.Name;
                            CodingViewer.ApplicationName = log._CodeLOG.Application.Name;

                            CodingViewer.Date = log._CodeLOG.Start.ToString("d MMMM yyyy HH:mm");

                            CodingViewer.Output = log._CodeLOG.Output.Name;
                            CodingViewer.Type = log._CodeLOG.Type.Name;

                            foreach (PostIt p in log._CodeLOG.PostItList)
                            {
                                CodingViewer.AddPostIt(new CreatePostItViewModel(this, log._CodeLOG.Project, p.Subject.Subject, p.Error, p.Solution,
                                    p.Suggestion, p.Comment));
                            }

                            CodingViewer.BugsFound = $"{log._CodeLOG.Bugs} Bugs found";
                            CodingViewer.ApplicationOpened = log._CodeLOG.Success ? "Launch Successful" : "Unsuccessful Launch";

                            Dashboard.DataContext = codingDashboard;
                            Viewer.DataContext = CodingViewer;
                            LoadViewerControls();
                            _VIEW_frame.Navigate(ViewerCodingFrame);
                            _VIEW_ASframe.Navigate(null);
                            ViewerCodingFrame.DataContext = CodingViewer;

                            break;
                        }

                }


                _MainFrame.Navigate(Viewer);
            }
            catch (InvalidOperationException invex)
            {
                Debug.WriteLine($"An invalid operation exception has been found: {invex.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found: {e.Message}");
            }
        }

        public void NavigateToPostItCreator(LoggerCreateViewModel loggerCreator)
        {
            PostItPage postItPage = _dataService.CurrentProject is null ? new PostItPage(new CreatePostItViewModel(this, _dataService, loggerCreator, CurrentCategory)) :
                new PostItPage(new CreatePostItViewModel(this, _dataService, loggerCreator, _dataService.CurrentProject));

            _MainFrame.Navigate(postItPage);
        }

        public void NavigateToReporterPostItCreator(ReporterUpdaterViewModel reporterUpdaterViewModel)
        {
            PostItPage postItPage = _dataService.CurrentProject is null ? new PostItPage(new CreateReporterPostItViewModel(this, _dataService, reporterUpdaterViewModel, CurrentCategory)) :
                new PostItPage(new CreateReporterPostItViewModel(this, _dataService, reporterUpdaterViewModel, _dataService.CurrentProject));

            _MainFrame.Navigate(postItPage);
        }


        public void NavigateToPostItEditor(LoggerCreateViewModel loggerCreator, PostItViewModel createPostItViewModel)
        {
            var editPostItViewModel = new EditPostItViewModel(this, _dataService, loggerCreator, CurrentCategory, createPostItViewModel);

            PostItPage postItPage = new PostItPage(editPostItViewModel);

            _MainFrame.Navigate(postItPage);
        }

        public void NavigateToReporterPostItEditor(ReporterUpdaterViewModel reporterUpdaterViewModel, CreateReporterPostItViewModel postItViewModel)
        {
            var editPostItViewModel = new UpdatePostItViewModel(this, _dataService, reporterUpdaterViewModel, CurrentCategory, postItViewModel);

            PostItPage postItPage = new PostItPage(editPostItViewModel);

            _MainFrame.Navigate(postItPage);
        }

        public void NavigateToReporter()
        {
            _dataService.InitialiseProjectsLISTAsync(CurrentCategory);
            var list = new ObservableCollection<string>();

            foreach (var item in _dataService.SQLITE_PROJECTS)
            {
                list.Add(item.Name);

                try
                {
                    if (string.IsNullOrEmpty(QtReportDesk.Project))
                    {
                        QtReportDesk.Project = QtReportDesk.Projects.ElementAt(0);
                    }
                }
                catch (ArgumentException index)
                {
                    Debug.WriteLine($"ArgumentException near NavigateToReporter(): {index.Message}");
                }
            }

            switch (Context)
            {
                case CacheContext.Qt:
                    {
                        if (_dataService.QtLogCount() != 0)
                        {

                            QtReportDesk.Projects = list;
                            _MainFrame.Navigate(Reporter);
                        }
                        else
                            MessageBox.Show("Please create a log if you need to make a report.", "No logs Found",
                                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                        break;
                    }
                case CacheContext.AndroidStudio:
                    {
                        if (_dataService.ASLogCount() != 0)
                        {
                            _MainFrame.Navigate(Reporter);
                        }
                        else
                            MessageBox.Show("Please create a log if you need to make a report.", "No logs Found",
                                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

                        break;
                    }
                case CacheContext.Graphics:
                    {
                        if (_dataService.LogCount(LOG.CATEGORY.GRAPHICS) != 0)
                        {
                            _MainFrame.Navigate(Reporter);
                        }
                        else
                            MessageBox.Show("Please create a log if you need to make a report.", "No logs Found",
                                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

                        break;
                    }
                case CacheContext.Film:
                    {
                        if (_dataService.LogCount(LOG.CATEGORY.FILM) != 0)
                        {
                            _MainFrame.Navigate(Reporter);
                        }
                        else
                            MessageBox.Show("Please create a log if you need to make a report.", "No logs Found",
                                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

                        break;
                    }
                case CacheContext.Flexi:
                    {
                        if (_dataService.FlexiLogCount() != 0)
                        {
                            _MainFrame.Navigate(Reporter);
                        }
                        else
                            MessageBox.Show("Please create a log if you need to make a report.", "No logs Found",
                                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

                        break;
                    }
                default:
                    {
                        if (_dataService.LogCount(LOG.CATEGORY.CODING) != 0)
                        {
                            _MainFrame.Navigate(Reporter);
                        }
                        else
                            MessageBox.Show("Please create a log if you need to make a report.", "No logs Found",
                                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

                        break;
                    }
            }


            Main.BackEnabled = true;
        }



        private LOG.CATEGORY ConvertToLOGEnum(CacheContext category)
        {
            switch (Context)
            {
                case CacheContext.Qt:
                    {
                        return LOG.CATEGORY.CODING;
                    }
                case CacheContext.AndroidStudio:
                    {
                        return LOG.CATEGORY.CODING;
                    }
                case CacheContext.Graphics:
                    {
                        return LOG.CATEGORY.GRAPHICS;
                    }
                case CacheContext.Film:
                    {
                        return LOG.CATEGORY.FILM;
                    }
                case CacheContext.Flexi:
                    {
                        return LOG.CATEGORY.NOTES;
                    }
                default:
                    {
                        return LOG.CATEGORY.CODING;
                    }
            }
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
*/