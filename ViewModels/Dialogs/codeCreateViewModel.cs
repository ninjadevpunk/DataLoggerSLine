namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class codeCreateViewModel : LoggerCreateViewModel
    {


        public codeCreateViewModel() { }

        public codeCreateViewModel(string app) 
        {
            if (app == "Qt")
            {
                ApplicationName = "Qt Creator";
                //AppFieldEnabled = false;
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

        
    }
}
