using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class filmCreateViewModel : LoggerCreateViewModel
    {
        public override LOG.CATEGORY Category { get => LOG.CATEGORY.FILM; }
        public override CacheContext Context { get => CacheContext.Film; }
        public override string LogType { get => "FILM LOG"; }


        private readonly FilmViewModel _filmViewModel;

        public filmCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            AppFieldEnabled = true;
            ApplicationName = "Blender";

            _filmViewModel = (FilmViewModel)logCacheViewModel;

            _dataService.InitialiseProjectsLIST(Category);
            var items = _dataService.SQLITE_PROJECTS;

            var apps = _dataService.SQLITE_APPLICATIONS;

            foreach (ProjectClass project in items)
            {
                _projects.Add(project.Name);
            }

            _dataService.InitialiseApplicationsLIST(Category);
            foreach (ApplicationClass app in apps)
            {
                _applications.Add(app.Name);
            }


            Output = "Motion Picture Experts Group (*.mp4)";
            _outputs.Add("Motion Picture Experts Group Layer 4 (*.mp4)");
            _outputs.Add("AVI (*.avi");
            _outputs.Add("Matroska (*.mkv)");
            _outputs.Add("Windows Media Video (*.wmv)");
            _outputs.Add("Transfer Stream (*.ts)");
            _outputs.Add("WebM (*.webm)");


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

            SaveCommand = new SaveCommand(this, _dataService);
            AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _filmViewModel, _dataService);
        }


        private string height;
        public string Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private string width;
        public string Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private string resolution;
        public string Resolution
        {
            get
            {
                return resolution;
            }
            set
            {
                resolution = value;
                OnPropertyChanged(nameof(Resolution));
            }
        }

        private string length;
        public string Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        private bool isCompleted;
        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
            set
            {
                isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        private string source;
        public string Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                OnPropertyChanged(nameof(Source));
            }
        }


        #region Member Functions



        public void UpdateLogCount()
        {
            if (_filmViewModel is not null)
            {
                var count = _dataService.LogCount(Category);
                LogCount = $"{_filmViewModel.CacheItems.Count} Logs Cached";
                _filmViewModel.LogCount = $"{_filmViewModel.CacheItems.Count} film logs cached | {count} total logs";
            }
        }











        #endregion
    }
}
