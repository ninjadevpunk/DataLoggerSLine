using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class codeCreateViewModel : LoggerCreateViewModel
    {


        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel) : base(navigationService, logCacheViewModel)
        {
            AnnotateCommand = new AnnotateCommand(LogType, _navigationService, this, _logCacheViewModel);
        }

        public codeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, string app) : base(navigationService, logCacheViewModel)
        {
            if (app == "Qt")
            {
                ApplicationName = "Qt Creator";
                //AppFieldEnabled = false;
            }


            AnnotateCommand = new AnnotateCommand(LogType, _navigationService, this, _logCacheViewModel);
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

        
    }
}
