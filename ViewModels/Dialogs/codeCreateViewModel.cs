using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class codeCreateViewModel : LoggerCreateViewModel
    {
        private readonly CodingGenericViewModel _viewModel;
        private readonly CodingQtViewModel? _QtviewModel;

        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            AnnotateCommand = new AnnotateCommand(LogType, _navigationService, this, _logCacheViewModel, _dataService);

            var items = _dataService.FIREBASE_PROJECTS;


            foreach (ProjectClass project in items)
            {
                if(project.Category == LOG.CATEGORY.CODING && project.Application.Name != "Qt Creator")
                    _projects.Add(project.Name);
            }

            var apps = _dataService.FIREBASE_APPLICATIONS;

            foreach(ApplicationClass app in apps)
            {
                if(app.Category == LOG.CATEGORY.CODING && app.Name != "Qt Creator" && app.Name != "Android Studio Hedgehog 2023.1.1")
                    _applications.Add(app.Name);
            }

            BugsFound = 0;
            ApplicationOpened = false;

            //_viewModel = (CodingGenericViewModel)logCacheViewModel;
        }

        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, string app, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            if (app == "Qt")
            {
                ApplicationName = "Qt Creator";
                
                //AppFieldEnabled = false;
                _QtviewModel = (CodingQtViewModel)logCacheViewModel;

                var items = _dataService.FIREBASE_PROJECTS;


                foreach (ProjectClass project in items)
                {
                    if (project.Category == LOG.CATEGORY.CODING && project.Application.Name == "Qt Creator")
                        _projects.Add(project.Name);
                }
            }
            else
                _viewModel = (CodingGenericViewModel)logCacheViewModel;


            AnnotateCommand = new AnnotateCommand(LogType, _navigationService, this, _logCacheViewModel, _dataService);

        }

        public async Task setup()
        {
            await _dataService.InitialiseApplicationsLIST();
            await _dataService.InitialiseProjectsLIST();
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



        public void UpdateLogCount()
        {
            if(ApplicationName == "Qt Creator" && _QtviewModel is not null)
                LogCount = _QtviewModel.CacheItems.Count.ToString() + " Logs Cached";
            else if(_viewModel is not null)
                LogCount = _viewModel.CacheItems.Count.ToString() + " Logs Cached";
        }











        #endregion


    }
}
