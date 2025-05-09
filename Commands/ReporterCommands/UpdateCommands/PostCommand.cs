using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    public class PostCommand : CommandBase
    {
        public enum ActionType { Create, Edit }

        private readonly NavigationService _navigationService;
        private readonly ReporterUpdaterViewModel _reporterUpdaterViewModel;
        private readonly CreateReporterPostItViewModel _postItViewModel;
        private readonly UpdatePostItViewModel _updatePostItViewModel;
        private ActionType _actionType = ActionType.Create;

        public PostCommand(NavigationService navigationService, ReporterUpdaterViewModel reporterUpdaterViewModel, CreateReporterPostItViewModel createReporterPostItViewModel)
        {
            try
            {

                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _reporterUpdaterViewModel = reporterUpdaterViewModel ?? throw new ArgumentNullException(nameof(reporterUpdaterViewModel));
                _postItViewModel = createReporterPostItViewModel ?? throw new ArgumentNullException(nameof(createReporterPostItViewModel));

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

        public PostCommand(ActionType actionType, NavigationService navigationService, ReporterUpdaterViewModel reporterUpdaterViewModel, UpdatePostItViewModel updatePostItViewModel, CreateReporterPostItViewModel createReporterPostItViewModel)
        {
            try
            {

                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _reporterUpdaterViewModel = reporterUpdaterViewModel ?? throw new ArgumentNullException(nameof(reporterUpdaterViewModel));
                _updatePostItViewModel = updatePostItViewModel ?? throw new ArgumentNullException(nameof(updatePostItViewModel));
                _postItViewModel = createReporterPostItViewModel ?? throw new ArgumentNullException(nameof(createReporterPostItViewModel));
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
                            if (_postItViewModel.Subject == string.Empty)
                            {
                                MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                if ((_postItViewModel.Error.Length == 0 || _postItViewModel.Error.Length == 353) &&
                                (_postItViewModel.Solution.Length == 0 || _postItViewModel.Solution.Length == 353) &&
                                (_postItViewModel.Suggestion.Length == 0 || _postItViewModel.Suggestion.Length == 353) &&
                                (_postItViewModel.Comment.Length == 0 || _postItViewModel.Comment.Length == 353)
                                )
                                {
                                    MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else
                                {
                                    _reporterUpdaterViewModel.PostIts.Add(_postItViewModel);
                                    //_navigationService.GoBack();
                                }
                            }
                            break;
                        }
                    case ActionType.Edit:
                        {
                            if (_updatePostItViewModel.Subject == string.Empty)
                            {
                                MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                if ((_updatePostItViewModel.Error.Length == 0 || _updatePostItViewModel.Error.Length == 353) &&
                                (_updatePostItViewModel.Solution.Length == 0 || _updatePostItViewModel.Solution.Length == 353) &&
                                (_updatePostItViewModel.Suggestion.Length == 0 || _updatePostItViewModel.Suggestion.Length == 353) &&
                                (_updatePostItViewModel.Comment.Length == 0 || _updatePostItViewModel.Comment.Length == 353)
                                )
                                {
                                    MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                else
                                {
                                    var index = _reporterUpdaterViewModel.PostIts.IndexOf(_postItViewModel);
                                    _reporterUpdaterViewModel.PostIts[index] = _updatePostItViewModel;
                                    //_navigationService.GoBack();
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
