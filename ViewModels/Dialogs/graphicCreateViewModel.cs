using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class graphicCreateViewModel : LoggerCreateViewModel
    {

        public override LOG.CATEGORY Category { get => LOG.CATEGORY.GRAPHICS; }
        public override CacheContext Context { get => CacheContext.Graphics; }
        public override string LogType { get => "GRAPHICS LOG"; }

        private readonly GraphicsViewModel _graphicsViewModel;
        private readonly ObservableCollection<string> _mediums;
        private readonly ObservableCollection<string> _formats;
        private readonly ObservableCollection<string> _units;
        private readonly ObservableCollection<string> _sizes;

        public IEnumerable<string> Mediums => _mediums;

        public IEnumerable<string> Formats => _formats;

        public IEnumerable<string> Units => _units;

        public IEnumerable<string> Sizes => _sizes;

        public graphicCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            AppFieldEnabled = true;
            ApplicationName = "Krita";

            _graphicsViewModel = (GraphicsViewModel)logCacheViewModel;

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


            Output = "Portable Network Graphics (*.PNG)";
            _outputs.Add("Portable Network Graphics (*.PNG)");
            _outputs.Add("Joint Pictures Experts Group (*.JPG) | (*.jpg)");
            _outputs.Add("Scalable Vector Graphics (*.svg)");
            _outputs.Add("Graphics Interchange Format (*.gif)");
            _outputs.Add("Portable Document Format (*.pdf)");
            _outputs.Add("Tag Image File Format (*.tiff)");
            _outputs.Add("Adobe Photoshop (*.psd)");
            _outputs.Add("WebP (*.webp)");


            Type = "NONE";
            _types.Add("NONE");
            _types.Add("Artwork");
            _types.Add("Doodle");
            _types.Add("Graphic Design Project");
            _types.Add("Resource");
            _types.Add("Portfolio");

            Medium = "Pencil";
            _mediums = new();
            _mediums.Add("Pencil");
            _mediums.Add("Paint");
            _mediums.Add("Pen");
            _mediums.Add("Pencil Crayon");
            _mediums.Add("Kokie");
            _mediums.Add("Crayon");
            _mediums.Add("Oil Pastel");
            _mediums.Add("Chalk");

            Format = "Paper";
            _formats = new();
            _formats.Add("Paper");
            _formats.Add("Digital Canvas");
            _formats.Add("Cardboard");
            _formats.Add("Wall/Street");

            MeasuringUnit = "cm";
            _units = new();
            _units.Add("cm");
            _units.Add("mm");
            _units.Add("px");
            _units.Add("in");
            _units.Add("m");

            Size = "A4 (29,7 cm x 21 cm)";
            _sizes = new();
            _sizes.Add("A4 (29,7 cm x 21 cm)");
            _sizes.Add("A5 (21 cm x 14,8 cm)");
            _sizes.Add("Letter (27,9 cm x 21,6 cm)");
            Height = string.Empty;
            Width = string.Empty;
            DPI = string.Empty;
            ColourDepth = string.Empty;
            IsCompleted = false;
            Source = string.Empty;

            SaveCommand = new SaveCommand(this, _dataService);
            AnnotateCommand = new AnnotateCommand(Context, _navigationService, this, _graphicsViewModel, _dataService);
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

        private string brush;
        public string Brush
        {
            get
            {
                return brush;
            }
            set
            {
                brush = value;
                OnPropertyChanged(nameof(Brush));
            }
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

        private string measuringUnit;
        public string MeasuringUnit
        {
            get
            {
                return measuringUnit;
            }
            set
            {
                measuringUnit = value;
                OnPropertyChanged(nameof(MeasuringUnit));
            }
        }

        private string size;
        public string Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        private string dpi;
        public string DPI
        {
            get
            {
                return dpi;
            }
            set
            {
                dpi = value;
                OnPropertyChanged(nameof(DPI));
            }
        }

        private string colourDepth;
        public string ColourDepth
        {
            get
            {
                return colourDepth;
            }
            set
            {
                colourDepth = value;
                OnPropertyChanged(nameof(ColourDepth));
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
            if (_graphicsViewModel is not null)
            {
                var count = _dataService.LogCount(Category);
                LogCount = $"{_graphicsViewModel.CacheItems.Count} Logs Cached";
                _graphicsViewModel.LogCount = $"{_graphicsViewModel.CacheItems.Count} graphics logs cached | {count} total logs";
            }
        }











        #endregion
    }
}
