using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Updater
{
    public class filmUpdateViewModel : ReporterUpdaterViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.FILM;
        public override CacheContext Context { get => CacheContext.Film; }
        public override string LogType => "FILM LOG";

        private readonly FilmReportDeskViewModel _filmViewModel;

        public filmUpdateViewModel(NavigationService navigationService, ReportDeskViewModel reportDeskViewModel, DataService dataService, REPORTViewModel reportViewModel, PDFService pdfService) : 
            base(navigationService, reportDeskViewModel, dataService)
        {
            AppFieldEnabled = true;
            ApplicationName = "Blender";

            _filmViewModel = (FilmReportDeskViewModel)reportDeskViewModel;


            InitializeProjectsAndApplications();
            InitializeDefaults();

            BrowseCommand = new BrowseCommand(Context, this);
            UpdateCommand = new UpdateCommand(Context, this, _filmViewModel, _navigationService, _dataService, reportViewModel, pdfService);
            ClearLoggerCommand = new ResetLoggerCommand(this, Category);
        }



        private void InitializeProjectsAndApplications()
        {
            _dataService.InitialiseProjectsLISTAsync(Category);
            var items = _dataService.SQLITE_PROJECTS;

            var apps = _dataService.SQLITE_APPLICATIONS;

            foreach (ProjectClass project in items)
            {
                _projects.Add(project.Name);
            }

            _dataService.InitialiseApplicationsLISTAsync(Category);
            foreach (ApplicationClass app in apps)
            {
                _applications.Add(app.Name);
            }
        }

        private void InitializeDefaults()
        {
            Output = "MP4";
            _outputs.Add("MP4");
            _outputs.Add("AVI");
            _outputs.Add("MKV");
            _outputs.Add("TS");
            _outputs.Add("WEBM");

            Type = "Film";
            _types.Add("Film");
            _types.Add("NONE");
            _types.Add("Doodle");
            _types.Add("Assignment");

            Height = string.Empty;
            Width = string.Empty;
            Resolution = string.Empty;
            Length = string.Empty;
            IsCompleted = false;
            Source = string.Empty;
        }













        #region Properties




        private string height;
        public string Height
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private string width;
        public string Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private string resolution;
        public string Resolution
        {
            get => resolution;
            set
            {
                resolution = value;
                OnPropertyChanged(nameof(Resolution));
            }
        }

        private string length;
        public string Length
        {
            get => length;
            set
            {
                length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        private bool isCompleted;
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        private string source;
        public string Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged(nameof(Source));
            }
        }



        #endregion







        #region Member Functions




        public void UpdateLogCount()
        {
            if (_filmViewModel is not null)
            {
                var count = _dataService.LogCount(Category);
                LogCount = $"{_filmViewModel.Logs.Count} Logs";
            }
        }

        #endregion



    }
}
