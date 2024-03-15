using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class filmCreateViewModel : LoggerCreateViewModel
    {
        public override string LogType { get => "FILM LOG"; }



        private readonly ObservableCollection<string> _mediums;
        private readonly ObservableCollection<string> _formats;

        public IEnumerable<string> Mediums => _mediums;
        public IEnumerable<string> Formats => _formats;

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

        public ICommand BrowseCommand { get; set; }

        public filmCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel) : base(navigationService, logCacheViewModel)
        {
            //
        }
    }
}
