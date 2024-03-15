using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class flexiCreateViewModel : LoggerCreateViewModel
    {
        public override string LogType { get => "FLEXIBLE LOG"; }

        private readonly ObservableCollection<string> _mediums;
        private readonly ObservableCollection<string> _formats;

        public IEnumerable<string> Mediums => _mediums;
        public IEnumerable<string> Formats => _formats;


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



        public ICommand BrowseCommand { get; set; }

        public flexiCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel) : base(navigationService, logCacheViewModel)
        {
            //
        }
    }
}
