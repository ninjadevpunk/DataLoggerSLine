using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class SaveChecklistCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly CreateCheckListViewModel _createCheckListViewModel;


        public SaveChecklistCommand(NavigationService navigationService)
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

        public SaveChecklistCommand(NavigationService navigationService, CreateCheckListViewModel createCheckListViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _createCheckListViewModel = createCheckListViewModel ?? throw new ArgumentNullException(nameof(createCheckListViewModel));

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
