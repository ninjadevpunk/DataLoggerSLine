using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class graphicCreateViewModel : LoggerCreateViewModel
    {

        public override string LogType { get => "GRAPHICS LOG"; }


        private readonly ObservableCollection<string> _mediums;
        private readonly ObservableCollection<string> _formats;
        private readonly ObservableCollection<string> _units;
        private readonly ObservableCollection<string> _sizes;

        public IEnumerable<string> Mediums => _mediums;

        public IEnumerable<string> Formats => _formats;

		public IEnumerable<string> Units => _units;

		public IEnumerable<string> Sizes => _sizes;


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




		public ICommand BrowseCommand { get; set; }

        public graphicCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel) : base(navigationService, logCacheViewModel)
        {
            //
        }

    }
}
