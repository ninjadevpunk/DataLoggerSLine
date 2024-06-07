using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Services;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class CreateCheckListViewModel : ViewModelBase
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

        private ObservableCollection<CreateChecklistItemViewModel> checklistItems;
        public ObservableCollection<CreateChecklistItemViewModel> ChecklistItems
        {
            get
            {
                return checklistItems;
            }
            set
            {
                checklistItems = value;
                OnPropertyChanged(nameof(ChecklistItems));
            }
        }

        ICommand AddChecklistItem { get; set; }

        ICommand SaveChecklistCommand { get; set; }

        public CreateCheckListViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            NoteSubject = "No Subject";

            AddChecklistItem = new AddChecklistItemCommand(this);
            SaveChecklistCommand = new SaveChecklistCommand(_navigationService);
        }
    }
}
