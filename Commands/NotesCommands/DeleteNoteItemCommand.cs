using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class DeleteNoteItemCommand : CommandBase
    {
        private readonly NOTESViewModel _notesViewModel;

        public DeleteNoteItemCommand(NOTESViewModel notesViewModel)
        {
            try
            {
                _notesViewModel = notesViewModel ?? throw new ArgumentNullException(nameof(notesViewModel));
            }
            catch (Exception)
            {

                //
            }
        }

        public override void Execute(object parameter)
        {
            if (parameter is NoteLOGViewModel noteToDelete)
            {
                // Remove the note from the ObservableCollection
                _notesViewModel.NoteItems.Remove(noteToDelete);

            }
        }
    }
}
