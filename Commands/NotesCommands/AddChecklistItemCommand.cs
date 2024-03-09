using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class AddChecklistItemCommand : CommandBase
    {
        private readonly CreateCheckListViewModel _viewModel;


        public AddChecklistItemCommand(CreateCheckListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.ChecklistItems.Add(new CreateChecklistItemViewModel(new CheckListItem(false, "")));
        }
    }
}
