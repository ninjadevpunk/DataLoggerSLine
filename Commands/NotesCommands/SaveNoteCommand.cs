using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class SaveNoteCommand : CommandBase
    {
        protected readonly NavigationService _navigationService;
        protected readonly DataService _dataService;
        protected readonly CreateNoteViewModel _createNoteViewModel;
        protected readonly NOTESViewModel _notesViewModel;


        public SaveNoteCommand(NavigationService navigationService, DataService dataService, CreateNoteViewModel createNoteViewModel, NOTESViewModel notesViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _createNoteViewModel = createNoteViewModel ?? throw new ArgumentNullException(nameof(createNoteViewModel));
                _notesViewModel = notesViewModel ?? throw new ArgumentNullException(nameof(notesViewModel));

            }
            catch (Exception)
            {
                //
            }
        }

        public SaveNoteCommand(NavigationService navigationService, DataService dataService, CreateNoteViewModel createNoteViewModel, NOTESViewModel notesViewModel, NoteLOGViewModel noteLOGViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _createNoteViewModel = createNoteViewModel ?? throw new ArgumentNullException(nameof(createNoteViewModel));
                _notesViewModel = notesViewModel ?? throw new ArgumentNullException(nameof(notesViewModel));

            }
            catch (Exception)
            {
                //
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                NoteItem noteItem = new();

                noteItem.ID = _dataService.CreateLogID();

                ACCOUNT account = _dataService.GetUser();
                ApplicationClass DataLoggerNotesApp = _dataService.FindApplicationByID(15);

                noteItem.Author = account;
                noteItem.Project = new(_dataService.CreateProjectID(), account, "", DataLoggerNotesApp,
                    LOG.CATEGORY.NOTES, false);

                noteItem.Application = DataLoggerNotesApp;
                noteItem.Start = DateTime.Parse(_createNoteViewModel.CreationDate);
                noteItem.End = DateTime.Parse(_createNoteViewModel.ModifiedDate);
                noteItem.Output = _dataService.FindOutputByID(37);
                noteItem.Type = _dataService.FindTypeByID(39);

                noteItem.Subject = _createNoteViewModel.NoteSubject;
                noteItem.Content = _createNoteViewModel.NoteContent;




                // Send to database
                _dataService.StoreLog(noteItem);

                var list = _notesViewModel.NoteItems;
                list.Add(new NoteLOGViewModel(_dataService, _notesViewModel, noteItem));

                _notesViewModel.NoteItems = list;
                _navigationService.NavigateToNOTESDashboard();
                _navigationService.Main.GenericNotesChecked = false;
            }
            catch (Exception ex)
            {

                //
            }
        }
    }
}
