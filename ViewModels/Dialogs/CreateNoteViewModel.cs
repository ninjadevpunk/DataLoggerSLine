using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class CreateNoteViewModel : ViewModelBase
    {
        private readonly AuthService _authService;
        private readonly DataService _dataService;
        private readonly NavigationService _navigationService;

		private string noteSubject;
		public string NoteSubject
		{
			get
			{
				return noteSubject;
			}
			set
			{
				noteSubject = value;
				OnPropertyChanged(nameof(NoteSubject));
			}
		}

		private string date;
		public string Date
		{
			get
			{
				return date;
			}
			set
			{
				date = value;
				OnPropertyChanged(nameof(Date));
			}
		}

		ICommand SaveNoteCommand { get; set; }

		public CreateNoteViewModel(NavigationService navigationService)
		{
			_navigationService = navigationService;

			NoteSubject = "No Subject";

			SaveNoteCommand = new SaveNoteCommand(_navigationService);
		}
	}
}
