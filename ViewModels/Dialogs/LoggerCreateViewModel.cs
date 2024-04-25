using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public abstract class LoggerCreateViewModel : ViewModelBase
    {

        protected readonly AuthService _authService;
        protected readonly DataService _dataService;
		protected readonly NavigationService _navigationService;
        protected readonly LogCacheViewModel _logCacheViewModel;
        private Timer timer;


        protected LoggerCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService)
        {
			_navigationService = navigationService;
			_logCacheViewModel = logCacheViewModel;
			_dataService = dataService;

			Author = _dataService.GetUser();

			DisplayPicVisibility = Visibility.Visible;

			AppFieldEnabled = true;

			ProjectName = "";
			_projects = new();

			ApplicationName = "";
			_applications = new();

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

			Output = "";
			Type = "";

			PostIts = new ObservableCollection<CreatePostItViewModel>();

			NetworkStatusMonitor();

			CurrentStartDateCommand = new CurrentStartDateCommand(this);
			CurrentEndDateCommand = new CurrentEndDateCommand(this);
			AddPostItCommand = new AddPostItCommand(_navigationService, this);
        }


        protected readonly ObservableCollection<string> _projects;
        protected readonly ObservableCollection<string> _applications;
        protected readonly ObservableCollection<string> _outputs;
        protected readonly ObservableCollection<string> _types;

        public IEnumerable<string> Projects => _projects;

        public IEnumerable<string> Applications => _applications;

        public IEnumerable<string> Outputs => _outputs;

        public IEnumerable<string> Types => _types;






        public abstract string LogType { get; }

        public Visibility DisplayPicVisibility { get; set; }

        public int LogID { get; set; }

        public string Author { get; set; }


		private string signupImage;
		public string SignUpImage
		{
			get
			{
				return signupImage;
			}
			set
			{
				signupImage = value;

                if (SignUpImage != "")
                    DisplayPicVisibility = Visibility.Collapsed;
                else
                    DisplayPicVisibility = Visibility.Visible;

                OnPropertyChanged(nameof(SignUpImage));
			}
		}

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

		private bool isFirebaseOnline;
		public bool IsFirebaseOnline
		{
			get
			{
				return isFirebaseOnline;
			}
			set
			{
				isFirebaseOnline = value;
				FirebaseStatusFill = IsFirebaseOnline ? Brushes.Green : Brushes.Red;
				FirebaseStatus = IsFirebaseOnline ? "Connected" : "Not Connected";
				OnPropertyChanged(nameof(IsFirebaseOnline));
			}
		}



		private string logCount;
		public string LogCount
		{
			get
			{
				return logCount;
			}
			set
			{
				logCount = value;
				OnPropertyChanged(nameof(LogCount));
			}
		}


		private Brush firebaseStatusFill;
		public Brush FirebaseStatusFill
		{
			get
			{
				return firebaseStatusFill;
			}
			set
			{
				firebaseStatusFill = value;
				OnPropertyChanged(nameof(FirebaseStatusFill));
			}
		}

		private string firebaseStatus;
		public string FirebaseStatus
		{
			get
			{
				return firebaseStatus;
			}
			set
			{
				firebaseStatus = value;
				OnPropertyChanged(nameof(FirebaseStatus));
			}
		}


		public ICommand CurrentStartDateCommand { get; set; }

		public ICommand CurrentEndDateCommand { get; set; }



		public ICommand SaveCommand { get; set; }

		public ICommand AnnotateCommand { get; set; }



		public ICommand ClearPostItListCommand { get; set; }

		public ICommand AddPostItCommand { get; set; }





        #region Member Functions 



        public void UpdateDateControl()
		{
            StartDate = DateTime.Parse(StartDate.Date.ToLongDateString() + " " + StartHours + ":" + StartMinutes + ":" + StartSeconds + "." + StartMilliseconds);
        }

		public void UpdateEndDateControl()
		{
            EndDate = DateTime.Parse(EndDate.Date.ToLongDateString() + " " + EndHours + ":" + EndMinutes + ":" + EndSeconds + "." + EndMilliseconds);
        }

		public static bool IsInternetAvailable()
		{
			return NetworkInterface.GetIsNetworkAvailable();
        }

        public void NetworkStatusMonitor()
        {
            // Initialize the timer to check for network status every second
            timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void TimerCallback(object? state)
        {
			try
			{
                if(Application.Current != null)
				{
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        IsFirebaseOnline = IsInternetAvailable();
                    });
                }
            }
			catch (Exception)
			{
				//
			}
        }

        #endregion
    }
}
