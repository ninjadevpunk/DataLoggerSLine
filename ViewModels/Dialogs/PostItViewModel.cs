using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class PostItViewModel : ViewModelBase
    {

        private readonly NavigationService _navigationService;



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

	}
}
