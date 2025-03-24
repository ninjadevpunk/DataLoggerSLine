using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class AddChecklistItemCommand : CommandBase
    {
        private readonly CreateCheckListViewModel _checklist;


        public AddChecklistItemCommand(CreateCheckListViewModel viewModel)
        {
            _checklist = viewModel;
        }

        public override void Execute(object parameter)
        {
            var list = _checklist.ChecklistItems;
            list.Add(new CreateChecklistItemViewModel(_checklist, new CheckListItem(false, "")));

            _checklist.ChecklistItems = list;
        }
    }
}
