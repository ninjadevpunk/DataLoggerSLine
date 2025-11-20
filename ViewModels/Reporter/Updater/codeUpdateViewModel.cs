using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Updater
{
    public class codeUpdateViewModel : ReporterUpdaterViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.CODING;
        public override CacheContext Context => CacheContext.Coding;
        public override string LogType => "CODING LOG";

        protected readonly CodeReportDeskViewModel _viewModel;
        private readonly QtReportDeskViewModel? _QtviewModel;

        private const string Qt = "Qt Creator";
        protected const string AndroidStudio = "Android Studio Meerkat 2024.3.1";
        protected const string VisualStudio = "Visual Studio Community 2022";

        private bool QtOnly = false;
        protected bool ASOnly = false;

        public codeUpdateViewModel(NavigationService navigationService, ReportDeskViewModel reportDeskViewModel, DataService dataService, REPORTViewModel reportViewModel, PDFService pdfService)
            : base(navigationService, reportDeskViewModel, dataService)
        {
            _viewModel = (CodeReportDeskViewModel)reportDeskViewModel;
            InitializeCommonFields(reportViewModel, pdfService);
            LoadProjectsAndApplications();
            LoadDefaultOutputs();
            LoadDefaultTypes();

            BugsFound = 0;
            ApplicationOpened = false;

            ClearLoggerCommand = new ResetLoggerCommand();

            UpdateLogCount();
        }

        public codeUpdateViewModel(NavigationService navigationService, ReportDeskViewModel reportDeskViewModel, string application, DataService dataService, REPORTViewModel reportViewModel, PDFService pdfService)
            : base(navigationService, reportDeskViewModel, dataService)
        {
            if (application == Qt)
            {
                _QtviewModel = (QtReportDeskViewModel)reportDeskViewModel;
                InitializeQtFields(reportViewModel, pdfService);

            }
            else
            {
                _viewModel = (CodeReportDeskViewModel)reportDeskViewModel;
                InitializeCommonFields(reportViewModel, pdfService);
                LoadProjectsAndApplications();
                LoadDefaultOutputs();
                LoadDefaultTypes();

            }

            ClearLoggerCommand = new ResetLoggerCommand();
            UpdateLogCount();
        }

        private void InitializeCommonFields(REPORTViewModel reportViewModel, PDFService pdfService)
        {
            AppFieldEnabled = true;
            ApplicationName = VisualStudio;

            UpdateCommand = new UpdateCommand(Context, this, _viewModel, _navigationService, _dataService, reportViewModel, pdfService);
        }

        private void InitializeQtFields(REPORTViewModel reportViewModel, PDFService pdfService)
        {
            AppFieldEnabled = false;
            ApplicationToolTip = "This field cannot be changed. Only Qt logs are allowed in this tab.";
            ApplicationName = Qt;
            QtOnly = true;

            LoadQtProjects();
            LoadQtOutputs();
            LoadQtTypes();

            UpdateCommand = new UpdateCommand(CacheContext.Qt, this, _QtviewModel, _navigationService, _dataService, reportViewModel, pdfService);
        }

        private void LoadProjectsAndApplications()
        {
            var items = _dataService.SQLITE_PROJECTS;
            var apps = _dataService.SQLITE_APPLICATIONS;

            foreach (ProjectClass project in items)
            {
                if (project.Application.Name != Qt && project.Application.Name != AndroidStudio)
                    _projects.Add(project.Name);
            }

            foreach (ApplicationClass app in apps)
            {
                if (app.Category == LOG.CATEGORY.CODING && app.Name != Qt && app.Name != AndroidStudio)
                    _applications.Add(app.Name);
            }
        }

        private void LoadQtProjects()
        {
            var items = _dataService.SQLITE_PROJECTS;

            foreach (ProjectClass project in items)
            {
                if (project.Application.Name == Qt)
                    _projects.Add(project.Name);
            }
        }

        private void LoadDefaultOutputs()
        {
            Output = "C# Application (*.exe)";
            _outputs.Add("C# Application (*.exe)");
            _outputs.Add("C++ Application (*.exe)");
            _outputs.Add("C Application (*.exe)");
            _outputs.Add("Java Application (*.exe)");
            _outputs.Add("Database (*.db)");
            _outputs.Add("Database (SQL, Oracle)");
        }

        private void LoadQtOutputs()
        {
            Output = "Widgets Application";
            _outputs.Add("Console Application");
            _outputs.Add("Widgets Application");
            _outputs.Add("QtQuick Application");
        }

        private void LoadDefaultTypes()
        {
            Type = "Exception";
            _types.Add("NONE");
            _types.Add("Compilation");
            _types.Add("Runtime");
            _types.Add("Exception");
        }

        private void LoadQtTypes()
        {
            Type = "Build";
            _types.Add("NONE");
            _types.Add("Build");
            _types.Add("Runtime");
            _types.Add("Exception");
        }

        public string ApplicationToolTip { get; set; } = "The application's name.";
        public string TimeStartToolTip { get; set; } = "Updates the current start time AND end time. Does not update automatically!";
        public string TimeEndToolTip { get; set; } = "Updates the END TIME ONLY. Does not update automatically.";
        public string OutputToolTip { get; set; } = "Please select from this list. Checklist cannot be modified.";
        public string TypeToolTip { get; set; } = "Please select from this list. Checklist cannot be modified.";



        private int bugsFound;
        public int BugsFound
        {
            get => bugsFound;
            set
            {
                bugsFound = value;
                OnPropertyChanged(nameof(BugsFound));
            }
        }

        private bool applicationOpened;
        public bool ApplicationOpened
        {
            get => applicationOpened;
            set
            {
                applicationOpened = value;
                OnPropertyChanged(nameof(ApplicationOpened));
            }
        }

        #region Member Functions

        public virtual void UpdateLogCount()
        {
            if (QtOnly && _QtviewModel is not null)
            {
                var count = _dataService.QtLogCount();
                LogCount = $"{_QtviewModel.Logs.Count} Logs";
            }
            else if (_viewModel is not null)
            {
                var count = _dataService.LogCount(Category);
                LogCount = $"{_viewModel.Logs.Count} Logs";
            }
        }

        #endregion
    }
}
