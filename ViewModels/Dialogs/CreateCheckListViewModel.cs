using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class CreateCheckListViewModel : ViewModelBase
    {
        private readonly DataService _dataService;
        private readonly NavigationService _navigationService;

        public CreateCheckListViewModel(NavigationService navigationService, DataService dataService, NOTESViewModel notesViewModel)
        {
            _navigationService = navigationService;
            _dataService = dataService;

            InitialiseCommonFields();

            AddChecklistItem = new AddChecklistItemCommand(this);
            SaveChecklistCommand = new SaveChecklistCommand(_navigationService, _dataService, this, notesViewModel);
        }

        public void InitialiseCommonFields()
        {
            ChecklistItems = new();

            CreationDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm");
            StartandEndDate();
            NoteSubject = "No Subject";
        }

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
                StartandEndDate();
                OnPropertyChanged(nameof(NoteSubject));
            }
        }

        public string CreationDate { get; set; }

        private string modifiedDate;
        public string ModifiedDate
        {
            get
            {
                return modifiedDate;
            }
            set
            {
                modifiedDate = value;
                OnPropertyChanged(nameof(ModifiedDate));
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
                StartandEndDate();
                OnPropertyChanged(nameof(ChecklistItems));
            }
        }

        public ICommand AddChecklistItem { get; set; }

        public ICommand SaveChecklistCommand { get; set; }



        #region Member Functions





        private string UpdateLastModified() => ModifiedDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm");

        private void StartandEndDate() => Date = $"Created on {CreationDate}. Last modified on {UpdateLastModified()}";



        #endregion

    }
}
