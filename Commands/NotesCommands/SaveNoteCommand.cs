using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class SaveNoteCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly CreateNoteViewModel _createNoteViewModel;

        public SaveNoteCommand(NavigationService navigationService)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            }
            catch (Exception)
            {
                //
            }
        }

        public SaveNoteCommand(NavigationService navigationService, CreateNoteViewModel createNoteViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _createNoteViewModel = createNoteViewModel ?? throw new ArgumentNullException(nameof(createNoteViewModel));

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
                _navigationService.NavigateToNOTESDashboard();
                _navigationService.Main.ChecklistNotesChecked = false;
            }
            catch (Exception)
            {

                //
            }
        }
    }
}
