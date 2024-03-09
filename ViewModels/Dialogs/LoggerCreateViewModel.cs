using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public abstract class LoggerCreateViewModel : ViewModelBase
    {

        private readonly AuthService _authService;
        private readonly DataService _dataService;
		private readonly NavigationService _navigationService;




        private readonly ObservableCollection<string> _projects;
        private readonly ObservableCollection<string> _applications;
        private readonly ObservableCollection<string> _outputs;
        private readonly ObservableCollection<string> _types;

        public IEnumerable<string> Projects => _projects;

        public IEnumerable<string> Applications => _applications;

        public IEnumerable<string> Outputs => _outputs;

        public IEnumerable<string> Types => _types;






        public abstract string LogType { get; }

        public Visibility DisplayPicVisibility { get; set; }

        public int LogID { get; set; }

        public string Author { get; set; }

		private string projectName;
		public string ProjectName
		{
			get
			{
				return projectName;
			}
			set
			{
				projectName = value;
				OnPropertyChanged(nameof(ProjectName));
			}
		}

		private string applicationName;
		public string ApplicationName
		{
			get
			{
				return applicationName;
			}
			set
			{
				applicationName = value;
				OnPropertyChanged(nameof(ApplicationName));
			}
		}

		private bool appFieldEnabled;
		public bool AppFieldEnabled
        {
			get
			{
				return appFieldEnabled;
			}
			set
			{
				appFieldEnabled = value;
				OnPropertyChanged(nameof(AppFieldEnabled));
			}
		}


		#region START



		private DateTime startDate;
		public DateTime StartDate
		{
			get
			{
				return startDate;
			}
			set
			{
				startDate = value;
				OnPropertyChanged(nameof(StartDate));
			}
		}

		private DateTime startTime;
		public DateTime StartTime
		{
			get
			{
				return startTime;
			}
			set
			{
				startTime = value;
				OnPropertyChanged(nameof(StartTime));
			}
		}

		private int startHours;
		public int StartHours
		{
			get
			{
				return startHours;
			}
			set
			{
				startHours = value;
				StartDate = DateTime.Parse(StartDate.Date.ToLongDateString() + " " + StartHours + ":" + StartDate.Minute + ":" + StartDate.Second + "." + StartDate.Millisecond);
				OnPropertyChanged(nameof(StartHours));
			}
		}

		private int startMinutes;
		public int StartMinutes
		{
			get
			{
				return startMinutes;
			}
			set
			{
				startMinutes = value;
				OnPropertyChanged(nameof(StartMinutes));
			}
		}

		private int startSeconds;
		public int StartSeconds
		{
			get
			{
				return startSeconds;
			}
			set
			{
				startSeconds = value;
				OnPropertyChanged(nameof(StartSeconds));
			}
		}

		private int startMilliseconds;
		public int StartMilliseconds
		{
			get
			{
				return startMilliseconds;
			}
			set
			{
				startMilliseconds = value;
				OnPropertyChanged(nameof(StartMilliseconds));
			}
		}


        #endregion




        #region END




        private DateTime endDate;
		public DateTime EndDate
		{
			get
			{
				return endDate;
			}
			set
			{
				endDate = value;
				OnPropertyChanged(nameof(EndDate));
			}
		}

		private DateTime endTime;
		public DateTime EndTime
		{
			get
			{
				return endTime;
			}
			set
			{
				endTime = value;
				OnPropertyChanged(nameof(EndTime));
			}
		}

		private int endHours;
		public int EndHours
		{
			get
			{
				return endHours;
			}
			set
			{
				endHours = value;
				OnPropertyChanged(nameof(EndHours));
			}
		}

		private int endMinutes;
		public int EndMinutes
		{
			get
			{
				return endMinutes;
			}
			set
			{
				endMinutes = value;
				OnPropertyChanged(nameof(EndMinutes));
			}
		}

		private int endSeconds;
		public int EndSeconds
		{
			get
			{
				return endSeconds;
			}
			set
			{
				endSeconds = value;
				OnPropertyChanged(nameof(EndSeconds));
			}
		}

		private int endMilliseconds;
		public int EndMilliseconds
		{
			get
			{
				return endMilliseconds;
			}
			set
			{
				endMilliseconds = value;
				OnPropertyChanged(nameof(EndMilliseconds));
			}
		}



        #endregion




        private string output;
		public string Output
		{
			get
			{
				return output;
			}
			set
			{
				output = value;
				OnPropertyChanged(nameof(Output));
			}
		}

		private string type;
		public string Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
				OnPropertyChanged(nameof(Type));
			}
		}


		private ObservableCollection<PostItViewModel> postIts;
		public ObservableCollection<PostItViewModel> PostIts
		{
			get
			{
				return postIts;
			}
			set
			{
                postIts = value;
				OnPropertyChanged(nameof(PostIts));
			}
		}



		public string LogCount { get; set; } = "0 Logs Cached";

		public Brush FirebaseStatusFill { get; set; } = Brushes.Red;

		public string FirebaseStatus { get; set; } = "Not Connected";


        ICommand CurrentStartDateCommand { get; set; }

		ICommand CurrentEndDateCommand { get; set; }

		ICommand SaveCommand { get; set; }


		ICommand AnnotateCommand { get; set; }


		ICommand ClearPostItListCommand { get; set; }

		ICommand AddPostItCommand { get; set; }





        protected LoggerCreateViewModel()
        {
			AppFieldEnabled = true;

			_applications = new ObservableCollection<string>();
			_applications.Add("Visual Studio");
			_applications.Add("Komodo Edit IDE");
			_applications.Add("Blender 3D");
        }
    }
}
