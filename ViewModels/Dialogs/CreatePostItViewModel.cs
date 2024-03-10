using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class CreatePostItViewModel : ViewModelBase
    {

        private readonly NavigationService _navigationService;
		private readonly LoggerCreateViewModel _loggerCreateViewModel;


		private readonly ObservableCollection<string> _subjects;

		public IEnumerable<string> Subjects => _subjects;


        public int PostItID { get; set; }

		private string subject;
		public string Subject
		{
			get
			{
				return subject;
			}
			set
			{
				subject = value;
				OnPropertyChanged(nameof(Subject));
			}
		}

		private string error;
		public string Error
		{
			get
			{
				return error;
			}
			set
			{
				error = value;
				OnPropertyChanged(nameof(Error));
			}
		}

		private DateTime dateFound;
		public DateTime DateFound
		{
			get
			{
				return dateFound;
			}
			set
			{
				dateFound = value;
				OnPropertyChanged(nameof(DateFound));
			}
		}

		private string solution;
		public string Solution
		{
			get
			{
				return solution;
			}
			set
			{
				solution = value;
				OnPropertyChanged(nameof(Solution));
			}
		}

		private DateTime dateSolved;
		public DateTime DateSolved
		{
			get
			{
				return dateSolved;
			}
			set
			{
				dateSolved = value;
				OnPropertyChanged(nameof(DateSolved));
			}
		}

		private string suggestion;
		public string Suggestion
		{
			get
			{
				return suggestion;
			}
			set
			{
				suggestion = value;
				OnPropertyChanged(nameof(Suggestion));
			}
		}

		private string comment;
		public string Comment
		{
			get
			{
				return comment;
			}
			set
			{
				comment = value;
				OnPropertyChanged(nameof(Comment));
			}
		}


		// REPLACER 

		private string replace;
		public string Replace
		{
			get
			{
				return replace;
			}
			set
			{
				replace = value;
				OnPropertyChanged(nameof(Replace));
			}
		}

		private bool option1Check;
		public bool Option1Check
        {
			get
			{
				return option1Check;
			}
			set
			{
				option1Check = value;
				OnPropertyChanged(nameof(Option1Check));
			}
		}

		private bool option2Check;
		public bool Option2Check
		{
			get
			{
				return option2Check;
			}
			set
			{
				option2Check = value;
				OnPropertyChanged(nameof(Option2Check));
			}
		}

		private bool option3Check;
		public bool Option3Check
		{
			get
			{
				return option3Check;
			}
			set
			{
				option3Check = value;
				OnPropertyChanged(nameof(Option3Check));
			}
		}

		private bool option4Check;
		public bool Option4Check
		{
			get
			{
				return option4Check;
			}
			set
			{
				option4Check = value;
				OnPropertyChanged(nameof(Option4Check));
			}
		}


		public ICommand OKCommand { get; set; }
		public ICommand PostCommand {  get; set; }




        #region Toolbar




        public ICommand EraserCommand { get; set; }

		public ICommand HighlighterCommand { get; set; }

		public ICommand FontCommand { get; set; }

		public ICommand ULListCommand { get; set; }

		public ICommand OLListCommand { get; set; }

		public ICommand BoldCommand { get; set; }

		public ICommand ItalicsCommand { get; set; }

		public ICommand UnderlineCommand { get; set; }




        #endregion


        public CreatePostItViewModel(NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel)
        {
            _navigationService = navigationService;
			_loggerCreateViewModel = loggerCreateViewModel;
        }

    }
}
