using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public enum NoteItemType
    {
        Default,
        Checklist
    }

    public class NOTESViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;




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

        public ICommand DeleteNoteItemCommand { get; set; }




        public NOTESViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            NewNoteCommand = new NewNoteCommand(_navigationService);
            DeleteNoteItemCommand = new DeleteNoteItemCommand(this);
        }

    }
}
