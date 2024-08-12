using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class flexiCreateViewModel : LoggerCreateViewModel
    {
        public override LOG.CATEGORY Category { get => LOG.CATEGORY.NOTES; }
        public override CacheContext Context { get => CacheContext.Flexi; }
        public override string LogType { get => "FLEXIBLE LOG"; }

        private readonly FlexiViewModel _flexiViewModel;
        private readonly ObservableCollection<string> _mediums;
        private readonly ObservableCollection<string> _formats;
        private readonly ObservableCollection<string> _flexiLogCategories;

        public IEnumerable<string> Mediums => _mediums;
        public IEnumerable<string> Formats => _formats;
        public IEnumerable<string> FlexiLogCategories => _flexiLogCategories;

        public flexiCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            AppFieldEnabled = true;
            ApplicationName = "Blender";

            _flexiViewModel = (FlexiViewModel)logCacheViewModel;

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


            Output = "Portable Document Format (*.pdf)";
            _outputs.Add("Portable Document Format (*.pdf)");
            _outputs.Add("Joint Pictures Experts Group (*.JPG) | (*.jpg)");
            _outputs.Add("Scalable Vector Graphics (*.svg)");
            _outputs.Add("Microsoft Word (*.docx) | (*.doc)");
            _outputs.Add("Text File (*.txt)");
            _outputs.Add("HTML (*.html)");
            _outputs.Add("XPS (*.xps)");
            _outputs.Add("Microsoft Excel (*.xslx)");
            _outputs.Add("Microsoft Access (*.accdb)");
            _outputs.Add("Motion Picture Experts Group Layer 3 (*.mp3)");
            _outputs.Add("WAV File (*.wav)");
            _outputs.Add("AAC File (*.aac)");
            _outputs.Add("M4A (*.m4a)");
            _outputs.Add("OGG (*.ogg)");


            Type = "Document";
            _types.Add("Document");
            _types.Add("NONE");
            _types.Add("Invitation");
            _types.Add("Mail");
            _types.Add("Doodle");
            _types.Add("Report");
            _types.Add("Novel");
            _types.Add("Questionnaire");
            _types.Add("Curriculum Vitae");
            _types.Add("Website");
            _types.Add("Presentation");
            _types.Add("Spreadsheet");
            _types.Add("Notes");
            _types.Add("Database");
            _types.Add("Boardgame");
            _types.Add("Video Game");
            _types.Add("Card Game");
            _types.Add("Game");
            _types.Add("Music");
            _types.Add("Mantra");
            _types.Add("Poetry");

            Medium = "Song";
            _mediums = new();
            _mediums.Add("Song");
            _mediums.Add("A Cappella");
            _mediums.Add("Orchestral");

            Format = "CD";
            _formats = new();
            _formats.Add("CD");
            _formats.Add("MIDI");
            _formats.Add("Digital");
            _formats.Add("Tape");
            _formats.Add("LP");
            _formats.Add("EP");
            _formats.Add("Gramophone");

            FlexibleLogCategory = "Document";
            _flexiLogCategories = new();
            _flexiLogCategories.Add("Document");
            _flexiLogCategories.Add("Gaming");
            _flexiLogCategories.Add("Music");

            BitRate = string.Empty;
            Duration = string.Empty;
            IsCompleted = false;
            Source = string.Empty;

            BrowseCommand = new BrowseCommand(Context, this);

            SaveCommand = new SaveCommand(this, _dataService);
            AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _flexiViewModel, _dataService);
        }


        private string flexibleLogCategory;
        public string FlexibleLogCategory
        {
            get
            {
                return flexibleLogCategory;
            }
            set
            {
                flexibleLogCategory = value;
                OnPropertyChanged(nameof(FlexibleLogCategory));
            }
        }

        private string medium;
        public string Medium
        {
            get
            {
                return medium;
            }
            set
            {
                medium = value;
                OnPropertyChanged(nameof(Medium));
            }
        }


        private string format;
        public string Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
                OnPropertyChanged(nameof(Format));
            }
        }

        private string bitRate;
        public string BitRate
        {
            get
            {
                return bitRate;
            }
            set
            {
                bitRate = value;
                OnPropertyChanged(nameof(BitRate));
            }
        }

        private string duration;
        public string Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
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
            if (_flexiViewModel is not null)
            {
                var count = _dataService.LogCount(Category);
                LogCount = $"{_flexiViewModel.CacheItems.Count} Logs Cached";
                _flexiViewModel.LogCount = $"{_flexiViewModel.CacheItems.Count} flexible logs cached | {count} total logs";
            }
        }











        #endregion
    }
}
