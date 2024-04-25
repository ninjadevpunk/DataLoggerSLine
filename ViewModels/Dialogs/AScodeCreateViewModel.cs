using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class AScodeCreateViewModel : codeCreateViewModel
    {
		private readonly CodingAndroidViewModel _ASviewModel;

        public AScodeCreateViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService) : base(navigationService, logCacheViewModel, dataService)
        {
            ApplicationName = "Android Studio Hedgehog 2023.1.1";
			//AppFieldEnabled = false;

			FullORSimple = false;

            AnnotateCommand = new AnnotateCommand(LogType, _navigationService, this, _logCacheViewModel, _dataService);

            _ASviewModel = (CodingAndroidViewModel)logCacheViewModel;

			_applications.Clear();
        }

        private bool fullORsimple;
		public bool FullORSimple
		{
			get
			{
				return fullORsimple;
			}
			set
			{
				fullORsimple = value;

				GradleDaemonVisibility = FullORSimple ? Visibility.Hidden : Visibility.Visible;
                RunBuildVisibility = FullORSimple ? Visibility.Hidden : Visibility.Visible;
                LoadBuildVisibility = FullORSimple ? Visibility.Hidden : Visibility.Visible;
                ConfigureBuildVisibility = FullORSimple ? Visibility.Hidden : Visibility.Visible;
                AllProjectsVisibility = FullORSimple ? Visibility.Hidden : Visibility.Visible;

                OnPropertyChanged(nameof(FullORSimple));

			}
		}

        #region SYNC 



        /* SYNC */
        private DateTime syncTime;
		public DateTime SyncTime
		{
			get
			{
				return syncTime;
			}
			set
			{
				syncTime = value;
				OnPropertyChanged(nameof(SyncTime));
			}
		}

		private int syncHours;
		public int SyncHours
		{
			get
			{
				return syncHours;
			}
			set
			{
				syncHours = value;
				UpdateSyncTime();
				OnPropertyChanged(nameof(SyncHours));
			}
		}

		private int syncMinutes;
		public int SyncMinutes
		{
			get
			{
				return syncMinutes;
			}
			set
			{
				syncMinutes = value;
				UpdateSyncTime();
				OnPropertyChanged(nameof(SyncMinutes));
            }
		}

		private int syncSeconds;
		public int SyncSeconds
		{
			get
			{
				return syncSeconds;
			}
			set
			{
				syncSeconds = value;
				UpdateSyncTime();
				OnPropertyChanged(nameof(SyncSeconds));
            }
		}

		private int syncMilliseconds;
		public int SyncMilliseconds
		{
			get
			{
				return syncMilliseconds;
			}
			set
			{
				syncMilliseconds = value;
				UpdateSyncTime();
				OnPropertyChanged(nameof(SyncMilliseconds));
            }
		}



		#endregion




		#region GRADLE DAEMON





		private Visibility gradleDaemonVisibility;
		public Visibility GradleDaemonVisibility
        {
			get
			{
				return gradleDaemonVisibility;
			}
			set
			{
                gradleDaemonVisibility = value;
				OnPropertyChanged(nameof(GradleDaemonVisibility));
			}
		}

		private bool isConsidered;
		public bool IsConsidered
		{
			get
			{
				return isConsidered;
			}
			set
			{
				isConsidered = value;
				OnPropertyChanged(nameof(IsConsidered));
			}
		}



		/* GRADLE DAEMON */
		private DateTime gradleDaemonTime;
		public DateTime GradleDaemonTime
		{
			get
			{
				return gradleDaemonTime;
			}
			set
			{
				gradleDaemonTime = value;
				OnPropertyChanged(nameof(GradleDaemonTime));
			}
		}

		private int gradleDaemonHours;
		public int GradleDaemonHours
		{
			get
			{
				return gradleDaemonHours;
			}
			set
			{
				gradleDaemonHours = value;
				UpdateGradleDaemonTime();
				OnPropertyChanged(nameof(GradleDaemonHours));
			}
		}

		private int gradleDaemonMinutes;
		public int GradleDaemonMinutes
		{
			get
			{
				return gradleDaemonMinutes;
			}
			set
			{
				gradleDaemonMinutes = value;
				UpdateGradleDaemonTime();
				OnPropertyChanged(nameof(GradleDaemonMinutes));
            }
		}

		private int gradleDaemonSeconds;
		public int GradleDaemonSeconds
		{
			get
			{
				return gradleDaemonSeconds;
			}
			set
			{
				gradleDaemonSeconds = value;
				UpdateGradleDaemonTime();
				OnPropertyChanged(nameof(GradleDaemonSeconds));
            }
		}

		private int gradleDaemonMilliseconds;
		public int GradleDaemonMilliseconds
		{
			get
			{
				return gradleDaemonMilliseconds;
			}
			set
			{
				gradleDaemonMilliseconds = value;
				UpdateGradleDaemonTime();
				OnPropertyChanged(nameof(GradleDaemonMilliseconds));
            }
		}




		#endregion



		#region RUN BUILD



		private Visibility runBuildVisibility;
		public Visibility RunBuildVisibility
		{
			get
			{
				return runBuildVisibility;
			}
			set
			{
				runBuildVisibility = value;
				OnPropertyChanged(nameof(RunBuildVisibility));
			}
		}



		/* RUN BUILD */
		private DateTime runBuildTime;
		public DateTime RunBuildTime
		{
			get
			{
				return runBuildTime;
			}
			set
			{
				runBuildTime = value;
				OnPropertyChanged(nameof(RunBuildTime));
			}
		}

		private int runBuildHours;
		public int RunBuildHours
		{
			get
			{
				return runBuildHours;
			}
			set
			{
				runBuildHours = value;
				UpdateRunBuildTime();
				OnPropertyChanged(nameof(RunBuildHours));
			}
		}

		private int runBuildMinutes;
		public int RunBuildMinutes
		{
			get
			{
				return runBuildMinutes;
			}
			set
			{
				runBuildMinutes = value;
				UpdateRunBuildTime();
				OnPropertyChanged(nameof(RunBuildMinutes));
            }
		}

		private int runBuildSeconds;
		public int RunBuildSeconds
		{
			get
			{
				return runBuildSeconds;
			}
			set
			{
				runBuildSeconds = value;
				UpdateRunBuildTime();
				OnPropertyChanged(nameof(RunBuildSeconds));
            }
		}

		private int runBuildMilliseconds;
		public int RunBuildMilliseconds
		{
			get
			{
				return runBuildMilliseconds;
			}
			set
			{
				runBuildMilliseconds = value;
				UpdateRunBuildTime();
				OnPropertyChanged(nameof(RunBuildMilliseconds));
            }
		}



		#endregion




		#region LOAD BUILD



		private Visibility loadBuildVisibility;
		public Visibility LoadBuildVisibility
		{
			get
			{
				return loadBuildVisibility;
			}
			set
			{
				loadBuildVisibility = value;
				OnPropertyChanged(nameof(LoadBuildVisibility));
			}
		}


		/* LOAD BUILD */
		private DateTime loadBuildTime;
		public DateTime LoadBuildTime
		{
			get
			{
				return loadBuildTime;
			}
			set
			{
				loadBuildTime = value;
				OnPropertyChanged(nameof(LoadBuildTime));
            }
		}

		private int loadBuildHours;
		public int LoadBuildHours
		{
			get
			{
				return loadBuildHours;
			}
			set
			{
				loadBuildHours = value;
				UpdateLoadBuildTime();
				OnPropertyChanged(nameof(LoadBuildHours));
            }
		}

		private int loadBuildMinutes;
		public int LoadBuildMinutes
		{
			get
			{
				return loadBuildMinutes;
			}
			set
			{
				loadBuildMinutes = value;
				UpdateLoadBuildTime();
				OnPropertyChanged(nameof(LoadBuildMinutes));
            }
		}

		private int loadBuildSeconds;
		public int LoadBuildSeconds
		{
			get
			{
				return loadBuildSeconds;
			}
			set
			{
				loadBuildSeconds = value;
				UpdateLoadBuildTime();
				OnPropertyChanged(nameof(LoadBuildSeconds));
            }
		}

		private int loadBuildMilliseconds;
		public int LoadBuildMilliseconds
		{
			get
			{
				return loadBuildMilliseconds;
			}
			set
			{
				loadBuildMilliseconds = value;
				UpdateLoadBuildTime();
				OnPropertyChanged(nameof(LoadBuildMilliseconds));
            }
		}




		#endregion




		#region CONFIGURE BUILD




		private Visibility configureBuildVisibility;
		public Visibility ConfigureBuildVisibility
		{
			get
			{
				return configureBuildVisibility;
			}
			set
			{
				configureBuildVisibility = value;
				OnPropertyChanged(nameof(ConfigureBuildVisibility));
			}
		}


		/* CONFIGURE BUILD */
		private DateTime configureBuildTime;
		public DateTime ConfigureBuildTime
		{
			get
			{
				return configureBuildTime;
			}
			set
			{
				configureBuildTime = value;
				OnPropertyChanged(nameof(ConfigureBuildTime));
			}
		}


		private int configureBuildHours;
		public int ConfigureBuildHours
		{
			get
			{
				return configureBuildHours;
			}
			set
			{
				configureBuildHours = value;
				UpdateConfigureBuildTime();
				OnPropertyChanged(nameof(ConfigureBuildHours));
			}
		}

		private int configureBuildMinutes;
		public int ConfigureBuildMinutes
		{
			get
			{
				return configureBuildMinutes;
			}
			set
			{
				configureBuildMinutes = value;
				UpdateConfigureBuildTime();
				OnPropertyChanged(nameof(ConfigureBuildMinutes));
            }
		}

		private int configureBuildSeconds;
		public int ConfigureBuildSeconds
		{
			get
			{
				return configureBuildSeconds;
			}
			set
			{
				configureBuildSeconds = value;
				UpdateConfigureBuildTime();
				OnPropertyChanged(nameof(ConfigureBuildSeconds));
            }
		}

		private int configureBuildMilliseconds;
		public int ConfigureBuildMilliseconds
		{
			get
			{
				return configureBuildMilliseconds;
			}
			set
			{
				configureBuildMilliseconds = value;
				UpdateConfigureBuildTime();
				OnPropertyChanged(nameof(ConfigureBuildMilliseconds));
            }
		}



		#endregion



		#region ALL PROJECTS




		private Visibility allProjectsVisibility;
		public Visibility AllProjectsVisibility
		{
			get
			{
				return allProjectsVisibility;
			}
			set
			{
				allProjectsVisibility = value;
				OnPropertyChanged(nameof(AllProjectsVisibility));
			}
		}

		/* ALL PROJECTS */
		private DateTime allProjectsTime;
		public DateTime AllProjectsTime
		{
			get
			{
				return allProjectsTime;
			}
			set
			{
				allProjectsTime = value;
				OnPropertyChanged(nameof(AllProjectsTime));
			}
		}

		private int allProjectsHours;
		public int AllProjectsHours
		{
			get
			{
				return allProjectsHours;
			}
			set
			{
				allProjectsHours = value;
				UpdateAllProjectsTime();
				OnPropertyChanged(nameof(AllProjectsHours));
			}
		}

		private int allProjectsMinutes;
		public int AllProjectsMinutes
		{
			get
			{
				return allProjectsMinutes;
			}
			set
			{
				allProjectsMinutes = value;
				UpdateAllProjectsTime();
				OnPropertyChanged(nameof(AllProjectsMinutes));
            }
		}

		private int allProjectsSeconds;
		public int AllProjectsSeconds
		{
			get
			{
				return allProjectsSeconds;
			}
			set
			{
				allProjectsSeconds = value;
				UpdateAllProjectsTime();
				OnPropertyChanged(nameof(AllProjectsSeconds));
            }
		}

		private int allProjectsMilliseconds;
		public int AllProjectsMilliseconds
		{
			get
			{
				return allProjectsMilliseconds;
			}
			set
			{
				allProjectsMilliseconds = value;
				UpdateAllProjectsTime();
				OnPropertyChanged(nameof(AllProjectsMilliseconds));
            }
		}


        #endregion






        #region Member Functions




		public void UpdateSyncTime()
		{
			SyncTime = DateTime.Parse(DateTime.Now.ToLongDateString() + " " + SyncHours + ":" + SyncMinutes + ":" + SyncSeconds + "." + SyncMilliseconds);
		}

		public void UpdateGradleDaemonTime()
		{
			GradleDaemonTime = DateTime.Parse(DateTime.Now.ToLongDateString() + " " + GradleDaemonHours + ":" + GradleDaemonMinutes + ":" + GradleDaemonSeconds + "." + GradleDaemonMilliseconds);
        }

		public void UpdateRunBuildTime()
		{
			RunBuildTime = DateTime.Parse(DateTime.Now.ToLongDateString() + " " + RunBuildHours + ":" + RunBuildMinutes + ":" + RunBuildSeconds + "." + RunBuildMilliseconds);
        }

		public void UpdateLoadBuildTime()
		{
			LoadBuildTime = DateTime.Parse(DateTime.Now.ToLongDateString() + " " + LoadBuildHours + ":" + LoadBuildMinutes + ":" + LoadBuildSeconds + "." + LoadBuildMilliseconds);
        }

		public void UpdateConfigureBuildTime()
		{
			ConfigureBuildTime = DateTime.Parse(DateTime.Now.ToLongDateString() + " " + ConfigureBuildHours + ":" + ConfigureBuildMinutes + ":" + ConfigureBuildSeconds + "." + ConfigureBuildMilliseconds);
        }

		public void UpdateAllProjectsTime()
		{
			AllProjectsTime = DateTime.Parse(DateTime.Now.ToLongDateString() + " " + AllProjectsHours + ":" + AllProjectsMinutes + ":" + AllProjectsSeconds + "." + AllProjectsMilliseconds);
        }








        #endregion
    }
}
