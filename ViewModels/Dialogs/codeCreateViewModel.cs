using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class codeCreateViewModel : LoggerCreateViewModel
    {
        protected readonly CodingGenericViewModel _viewModel;
        private readonly CodingQtViewModel? _QtviewModel;
        private const string Qt = "Qt Creator";
        protected const string AndroidStudio = "Android Studio Hedgehog 2023.1.1";
        private bool QtOnly = false;
        protected bool ASOnly = false;

        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            AppFieldEnabled = true;
            ApplicationName = "";
            _viewModel = (CodingGenericViewModel)logCacheViewModel;


            _dataService.InitialiseProjectsLIST(LOG.CATEGORY.CODING);
            var items = _dataService.SQLITE_PROJECTS;


            foreach (ProjectClass project in items)
            {
                if(project.Application.Name != Qt && project.Application.Name != AndroidStudio)
                    _projects.Add(project.Name);
            }

            var apps = _dataService.SQLITE_APPLICATIONS;

            foreach(ApplicationClass app in apps)
            {
                var IsGenericApp = app.Name != Qt && app.Name != AndroidStudio;

                if (app.Category == LOG.CATEGORY.CODING && IsGenericApp)
                    _applications.Add(app.Name);

                
            }

            _outputs.Add("C# Application (*.exe)");
            _outputs.Add("C++ Application (*.exe)");
            _outputs.Add("C Application (*.exe)");
            _outputs.Add("Java Application (*.exe)");
            _outputs.Add("Database (*.db)");
            _outputs.Add("Database (SQL, Oracle)");

            _types.Add("NONE");
            _types.Add("Compilation");
            _types.Add("Runtime");

            BugsFound = 0;
            ApplicationOpened = false;

            AnnotateCommand = new AnnotateCommand(LogType, _navigationService, this, _viewModel, _dataService);

        }

        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, string application, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            _dataService.InitialiseProjectsLIST(LOG.CATEGORY.CODING);
            var items = _dataService.SQLITE_PROJECTS;

            var apps = _dataService.SQLITE_APPLICATIONS;

            if (application == "Qt")
            {
                AppFieldEnabled = false;
                ApplicationName = Qt;
                QtOnly = true;
                
                _QtviewModel = (CodingQtViewModel)logCacheViewModel;


                foreach (ProjectClass project in items)
                {
                    if (project.Application.Name == Qt)
                        _projects.Add(project.Name);
                }

                _outputs.Add("Console Application");
                _outputs.Add("Widgets Application");
                _outputs.Add("QtQuick Application");

                _types.Add("NONE");
                _types.Add("Build");
                _types.Add("Runtime");


                AnnotateCommand = new AnnotateCommand(LogType, _navigationService, this, _QtviewModel, _dataService);
            }
            else
            {
                AppFieldEnabled = true;
                ApplicationName = "";
                _viewModel = (CodingGenericViewModel)logCacheViewModel;


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



                _outputs.Add("C# Application (*.exe)");
                _outputs.Add("C++ Application (*.exe)");
                _outputs.Add("C Application (*.exe)");
                _outputs.Add("Java Application (*.exe)");
                _outputs.Add("Database (*.db)");
                _outputs.Add("Database (SQL, Oracle)");

                _types.Add("NONE");
                _types.Add("Compilation");
                _types.Add("Runtime");

                AnnotateCommand = new AnnotateCommand(LogType, _navigationService, this, _viewModel, _dataService);
            }



        }


        public override void Setup()
        {
            base.Setup();

            ApplicationName = "";
            AppFieldEnabled = true;

            BugsFound = 0;
            ApplicationOpened = false;

            if (QtOnly)
            {
                ApplicationName = Qt;
                AppFieldEnabled = false;
            }
        }

        public override string LogType { get => "CODING LOG"; }

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
            if(QtOnly && _QtviewModel is not null)
            {
                var count = _dataService.QtLogCount();
                LogCount = $"{_QtviewModel.CacheItems.Count} Logs Cached";
                _QtviewModel.LogCount = $"{_QtviewModel.CacheItems.Count} qt logs cached | {count} total logs";
            }
            else if(_viewModel is not null)
            {
                var count = _dataService.LogCount(LOG.CATEGORY.CODING);
                LogCount = $"{_viewModel.CacheItems.Count} Logs Cached";
                _viewModel.LogCount = $"{_viewModel.CacheItems.Count} coding logs cached | {count} total logs";
            }
        }











        #endregion


    }
}
