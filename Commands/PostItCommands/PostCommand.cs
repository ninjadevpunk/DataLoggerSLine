using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    public class PostCommand : CommandBase
    {
        public enum ActionType { Create, Edit }

        private readonly NavigationService _navigationService;
        private readonly LoggerCreateViewModel _loggerCreateViewModel;
        private readonly CreatePostItViewModel _createPostItViewModel;
        private readonly EditPostItViewModel _editPostItViewModel;
        private ActionType _actionType = ActionType.Create;

        public PostCommand(NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel, CreatePostItViewModel createPostItViewModel)
        {
            try
            {

                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
                _createPostItViewModel = createPostItViewModel ?? throw new ArgumentNullException(nameof(createPostItViewModel));

            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");

            }
        }

        public PostCommand(ActionType actionType, NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel, EditPostItViewModel editPostItViewModel, CreatePostItViewModel createPostItViewModel)
        {
            try
            {

                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
                _createPostItViewModel = createPostItViewModel ?? throw new ArgumentNullException(nameof(createPostItViewModel));
                _editPostItViewModel = editPostItViewModel ?? throw new ArgumentNullException(nameof(editPostItViewModel));
                _actionType = actionType;
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                switch (_actionType)
                {
                    case ActionType.Create:
                        {
                            if (_createPostItViewModel.Subject == string.Empty)
                            {
                                MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                if ((_createPostItViewModel.Error.Length == 0 || _createPostItViewModel.Error.Length == 353) &&
                                (_createPostItViewModel.Solution.Length == 0 || _createPostItViewModel.Solution.Length == 353) &&
                                (_createPostItViewModel.Suggestion.Length == 0 || _createPostItViewModel.Suggestion.Length == 353) &&
                                (_createPostItViewModel.Comment.Length == 0 || _createPostItViewModel.Comment.Length == 353)
                                )
                                {
                                    MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else
                                {
                                    _loggerCreateViewModel.PostIts.Add(_createPostItViewModel);
                                    _navigationService.GoBack();
                                }
                            }
                            break;
                        }
                    case ActionType.Edit:
                        {
                            if (_editPostItViewModel.Subject == string.Empty)
                            {
                                MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                if ((_editPostItViewModel.Error.Length == 0 || _editPostItViewModel.Error.Length == 353) &&
                                (_editPostItViewModel.Solution.Length == 0 || _editPostItViewModel.Solution.Length == 353) &&
                                (_editPostItViewModel.Suggestion.Length == 0 || _editPostItViewModel.Suggestion.Length == 353) &&
                                (_editPostItViewModel.Comment.Length == 0 || _editPostItViewModel.Comment.Length == 353)
                                )
                                {
                                    MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else
                                {
                                    var index = _loggerCreateViewModel.PostIts.IndexOf(_createPostItViewModel);
                                    //var newPostIt = new CreatePostItViewModel(
                                    //    _editPostItViewModel.Subject, _editPostItViewModel.Error, _editPostItViewModel.Solution, _editPostItViewModel.Suggestion,
                                    //    _editPostItViewModel.Comment, _editPostItViewModel.DateFound, _editPostItViewModel.DateSolved, _editPostItViewModel.Subjects
                                    //    ); 
                                    _loggerCreateViewModel.PostIts[index] = _editPostItViewModel;
                                    _navigationService.GoBack();
                                }
                            }
                            break;
                        }
                }


            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.Message}");
            }
        }
    }
}
