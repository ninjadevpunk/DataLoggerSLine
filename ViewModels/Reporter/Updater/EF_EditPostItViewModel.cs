using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
namespace Data_Logger_1._3.ViewModels.Reporter.Updater
{
    public class EF_EditPostItViewModel : EF_PostItViewModel
    {
        public EF_EditPostItViewModel(int id, NavigationService navigationService, DataService dataService, ReporterUpdaterViewModel reporterUpdaterViewModel,
            EF_PostItViewModel postItViewModel) :
            base(navigationService, dataService, reporterUpdaterViewModel)
        {
            ID = id;
            Subject = postItViewModel.Subject;
            PlaceholderText = string.IsNullOrEmpty(Subject) ? "Subject" : string.Empty;

            Error = postItViewModel.Error;
            Solution = postItViewModel.Solution;
            Suggestion = postItViewModel.Suggestion;
            Comment = postItViewModel.Comment;

            Display_Error = postItViewModel.Display_Error;
            Display_Solution = postItViewModel.Display_Solution;
            Display_Suggestion = postItViewModel.Display_Suggestion;
            Display_Comment = postItViewModel.Display_Comment;

            PostCommand = new EF_PostCommand(EF_PostCommand.ActionType.Edit, _navigationService, _reporterUpdaterViewModel, this, postItViewModel);
        }

        public EF_EditPostItViewModel(int id, NavigationService navigationService, DataService dataService, ReporterUpdaterViewModel ReporterUpdaterViewModel, ProjectClass project,
            EF_PostItViewModel postItViewModel) :
            base(navigationService, dataService, ReporterUpdaterViewModel, project)
        {
            ID = id;
            Subject = postItViewModel.Subject;
            PlaceholderText = string.IsNullOrEmpty(Subject) ? "Subject" : string.Empty;

            Subjects = postItViewModel.Subjects;

            Error = postItViewModel.Error;
            Solution = postItViewModel.Solution;
            Suggestion = postItViewModel.Suggestion;
            Comment = postItViewModel.Comment;

            Display_Error = postItViewModel.Display_Error;
            Display_Solution = postItViewModel.Display_Solution;
            Display_Suggestion = postItViewModel.Display_Suggestion;
            Display_Comment = postItViewModel.Display_Comment;

            EditCommand = new EF_EditPostItCommand(_navigationService, _reporterUpdaterViewModel);
            PostCommand = new EF_PostCommand(EF_PostCommand.ActionType.Edit, _navigationService, _reporterUpdaterViewModel, this, postItViewModel);
        }
    }
}
