using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dashboard
{
    public enum NoteItemType
    {
        Default,
        Checklist
    }

    public class NOTESViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;




        public NOTESViewModel(NavigationService navigationService, DataService dataService)
        {
            _navigationService = navigationService;

            NoNotesMessageVisibility = Visibility.Visible;

            NewNoteCommand = new NewNoteCommand(_navigationService);
            NoteItems = new();
        }

        public void InitialiseNotesList(DataService dataService)
        {

        }


        private Visibility noNotesMessageVisibility;
        public Visibility NoNotesMessageVisibility  
        {
            get
            {
                return noNotesMessageVisibility;
            }
            set
            {
                noNotesMessageVisibility = value;
                OnPropertyChanged(nameof(NoNotesMessageVisibility));
            }
        }



        private ObservableCollection<NoteLOGViewModel> noteItems;
        public ObservableCollection<NoteLOGViewModel> NoteItems
        {
            get
            {
                return noteItems;
            }
            set
            {
                noteItems = value;

                UpdateNotesMessageVisibility();

                OnPropertyChanged(nameof(NoteItems));
            }
        }


        private NoteItemType itemType;
        public NoteItemType ItemType
        {
            get
            {
                return itemType;
            }
            set
            {
                itemType = value;
                OnPropertyChanged(nameof(ItemType));
            }
        }

        public ICommand NewNoteCommand { get; set; }




        private void UpdateNotesMessageVisibility()
        {
            NoNotesMessageVisibility = NoteItems.Count == 0 ? Visibility.Visible : Visibility.Hidden;
        }


    }
}
