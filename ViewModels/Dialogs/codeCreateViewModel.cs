using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class codeCreateViewModel : LoggerCreateViewModel
    {
        public override LOG.CATEGORY Category { get => LOG.CATEGORY.CODING; }
        public override CacheContext Context { get => CacheContext.Coding; }
        public override string LogType { get => "CODING LOG"; }

        protected readonly CodingViewModel _viewModel;
        private readonly CodingQtViewModel? _QtviewModel;
        private const string Qt = "Qt Creator";
        protected const string AndroidStudio = "Android Studio Hedgehog 2023.1.1";
        protected const string VisualStudio = "Visual Studio Community 2022";
        private bool QtOnly = false;
        protected bool ASOnly = false;

        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            AppFieldEnabled = true;
            ApplicationName = VisualStudio;
            _viewModel = (CodingViewModel)logCacheViewModel;


            _dataService.InitialiseProjectsLIST(Category);
            var items = _dataService.SQLITE_PROJECTS;


            foreach (ProjectClass project in items)
            {
                if (project.Application.Name != Qt && project.Application.Name != AndroidStudio)
                    _projects.Add(project.Name);
            }

            var apps = _dataService.SQLITE_APPLICATIONS;

            foreach (ApplicationClass app in apps)
            {
                var IsGenericApp = app.Name != Qt && app.Name != AndroidStudio;

                if (app.Category == LOG.CATEGORY.CODING && IsGenericApp)
                    _applications.Add(app.Name);


            }


            Output = "C# Application (*.exe)";
            _outputs.Add("C# Application (*.exe)");
            _outputs.Add("C++ Application (*.exe)");
            _outputs.Add("C Application (*.exe)");
            _outputs.Add("Java Application (*.exe)");
            _outputs.Add("Database (*.db)");
            _outputs.Add("Database (SQL, Oracle)");


            Type = "Exception";
            _types.Add("NONE");
            _types.Add("Compilation");
            _types.Add("Runtime");
            _types.Add("Exception");

            BugsFound = 0;
            ApplicationOpened = false;

            SaveCommand = new SaveCommand(this, _dataService);
            AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _viewModel, _dataService);
            ClearLoggerCommand = new ResetLoggerCommand(this, Category);

            UpdateLogCount();

        }

        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, string application, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            _dataService.InitialiseProjectsLIST(Category);
            var items = _dataService.SQLITE_PROJECTS;

            var apps = _dataService.SQLITE_APPLICATIONS;

            if (application == "Qt")
            {
                AppFieldEnabled = false;
                ApplicationToolTip = "This field cannot be changed. Only Qt logs are allowed in this tab.";
                ApplicationName = Qt;
                QtOnly = true;

                _QtviewModel = (CodingQtViewModel)logCacheViewModel;


                foreach (ProjectClass project in items)
                {
                    if (project.Application.Name == Qt)
                        _projects.Add(project.Name);
                }

                Output = "Widgets Application";
                _outputs.Add("Console Application");
                _outputs.Add("Widgets Application");
                _outputs.Add("QtQuick Application");

                Type = "Build";
                _types.Add("NONE");
                _types.Add("Build");
                _types.Add("Runtime");
                _types.Add("Exception");


                AnnotateCommand = new AnnotateCommand(CacheContext.Qt, _navigationService, this, _QtviewModel, _dataService);
            }
            else
            {
                AppFieldEnabled = true;
                ApplicationName = VisualStudio;

                _viewModel = (CodingViewModel)logCacheViewModel;


                foreach (ProjectClass project in items)
                {
                    if (project.Application.Name != Qt && project.Application.Name != AndroidStudio)
                        _projects.Add(project.Name);
                }

                foreach (ApplicationClass app in apps)
                {
                    var IsGenericApp = app.Name != Qt && app.Name != AndroidStudio;

                    if (app.Category == LOG.CATEGORY.CODING && IsGenericApp)
                        _applications.Add(app.Name);

                }


                Output = "C# Application (*.exe)";
                _outputs.Add("C# Application (*.exe)");
                _outputs.Add("C++ Application (*.exe)");
                _outputs.Add("C Application (*.exe)");
                _outputs.Add("Java Application (*.exe)");
                _outputs.Add("Database (*.db)");
                _outputs.Add("Database (SQL, Oracle)");


                Type = "Exception";
                _types.Add("NONE");
                _types.Add("Compilation");
                _types.Add("Runtime");
                _types.Add("Exception");

                AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _viewModel, _dataService);
            }

            SaveCommand = new SaveCommand(this, _dataService);
            ClearLoggerCommand = new ResetLoggerCommand(this, Category);

            UpdateLogCount();

        }

        public string ApplicationToolTip { get; set; } = "The application's name.";
        public string TimeStartToolTip { get; set; } = "Updates the current start time AND end time. Does not update automatically!";
        public string TimeEndToolTip { get; set; } = "Updates the END TIME ONLY. Does not update automatically.";
        public string OutputToolTip { get; set; } = "Please select from this list. Items cannot be modified.";
        public string TypeToolTip { get; set; } = "Please select from this list. Items cannot be modified.";


        private int bugsFound;
        public int BugsFound
        {
            get
            {
                return bugsFound;
            }
            set
            {
                bugsFound = value;
                OnPropertyChanged(nameof(BugsFound));
            }
        }

        private bool applicationOpened;
        public bool ApplicationOpened
        {
            get
            {
                return applicationOpened;
            }
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
                LogCount = $"{_QtviewModel.CacheItems.Count} Logs Cached";
                _QtviewModel.LogCount = $"{_QtviewModel.CacheItems.Count} qt logs cached | {count} total logs";
            }
            else if (_viewModel is not null)
            {
                var count = _dataService.LogCount(Category);
                LogCount = $"{_viewModel.CacheItems.Count} Logs Cached";
                _viewModel.LogCount = $"{_viewModel.CacheItems.Count} coding logs cached | {count} total logs";
            }
        }











        #endregion


    }
}
