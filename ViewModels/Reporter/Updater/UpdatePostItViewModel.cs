using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands.PostCommand;

namespace Data_Logger_1._3.ViewModels.Reporter.Updater
{
    public class UpdatePostItViewModel : CreateReporterPostItViewModel
    {
        public UpdatePostItViewModel(NavigationService navigationService, DataService dataService, ReporterUpdaterViewModel reporterUpdaterViewModel, LOG.CATEGORY category,
            CreateReporterPostItViewModel postItViewModel) :
            base(navigationService, dataService, reporterUpdaterViewModel, category)
        {
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

            PostCommand = new PostCommand(ActionType.Edit, _navigationService, _reporterUpdaterViewModel, this, postItViewModel);
        }

        public UpdatePostItViewModel(NavigationService navigationService, DataService dataService, ReporterUpdaterViewModel ReporterUpdaterViewModel, ProjectClass project,
            CreateReporterPostItViewModel postItViewModel) :
            base(navigationService, dataService, ReporterUpdaterViewModel, project)
        {
            Subject = postItViewModel.Subject;
            Subjects = postItViewModel.Subjects;

            Error = postItViewModel.Error;
            Solution = postItViewModel.Solution;
            Suggestion = postItViewModel.Suggestion;
            Comment = postItViewModel.Comment;

            Display_Error = postItViewModel.Display_Error;
            Display_Solution = postItViewModel.Display_Solution;
            Display_Suggestion = postItViewModel.Display_Suggestion;
            Display_Comment = postItViewModel.Display_Comment;

            PostCommand = new PostCommand(ActionType.Edit, _navigationService, _reporterUpdaterViewModel, this, postItViewModel);
        }
    }
}
