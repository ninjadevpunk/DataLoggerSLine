using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Commands.PostIt;
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
				UpdateDateControl();
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
				UpdateDateControl();
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
				UpdateDateControl();
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
				UpdateDateControl();
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
				UpdateEndDateControl();
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
				UpdateEndDateControl();
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
				UpdateEndDateControl();
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
				UpdateEndDateControl();
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


		private ObservableCollection<CreatePostItViewModel> postIts;
		public ObservableCollection<CreatePostItViewModel> PostIts
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


        public ICommand CurrentStartDateCommand { get; set; }

		public ICommand CurrentEndDateCommand { get; set; }

		public ICommand SaveCommand { get; set; }


		public ICommand AnnotateCommand { get; set; }


		public ICommand ClearPostItListCommand { get; set; }

		public ICommand AddPostItCommand { get; set; }





        protected LoggerCreateViewModel(NavigationService navigationService)
        {
			_navigationService = navigationService;

			AppFieldEnabled = true;

			_applications = new ObservableCollection<string>();
			_applications.Add("Visual Studio");
			_applications.Add("Komodo Edit IDE");
			_applications.Add("Blender 3D");

			StartHours = DateTime.Now.Hour;
			StartMinutes = DateTime.Now.Minute;
			StartSeconds = DateTime.Now.Second;
			StartMilliseconds = DateTime.Now.Millisecond;

			StartDate = DateTime.Now;

			EndHours = DateTime.Now.Hour;
			EndMinutes = DateTime.Now.Minute;
			EndSeconds = DateTime.Now.Second;
			EndMilliseconds = DateTime.Now.Millisecond;

			EndDate = DateTime.Now;

			CurrentStartDateCommand = new CurrentStartDateCommand(this);
			CurrentEndDateCommand = new CurrentEndDateCommand(this);
			AddPostItCommand = new AddPostItCommand(_navigationService, this);
        }

        #region Member Functions 



		public void UpdateDateControl()
		{
            StartDate = DateTime.Parse(StartDate.Date.ToLongDateString() + " " + StartHours + ":" + StartMinutes + ":" + StartSeconds + "." + StartMilliseconds);
        }

		public void UpdateEndDateControl()
		{
            EndDate = DateTime.Parse(EndDate.Date.ToLongDateString() + " " + EndHours + ":" + EndMinutes + ":" + EndSeconds + "." + EndMilliseconds);
        }



        #endregion
    }
}
