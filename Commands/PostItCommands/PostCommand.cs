using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    public class PostCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LoggerCreateViewModel _loggerCreateViewModel;
        private readonly CreatePostItViewModel _item;

        public PostCommand(NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel, CreatePostItViewModel createPostItViewModel)
        {
            try
            {

                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
                _item = createPostItViewModel ?? throw new ArgumentNullException(nameof(createPostItViewModel));


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
                if(_item.Subject == string.Empty)
                {
                    MessageBox.Show("Please add a subject.", "No Subject", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if ((_item.Error.Length == 0 || _item.Error.Length == 353) &&
                    (_item.Solution.Length == 0 || _item.Solution.Length == 353) &&
                    (_item.Suggestion.Length == 0 || _item.Suggestion.Length == 353) &&
                    (_item.Comment.Length == 0 || _item.Comment.Length == 353)
                    )
                    {
                        MessageBox.Show("You have not entered any relevant data. All logs require that you submit a valid PostIt.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        _loggerCreateViewModel.PostIts.Add(_item);
                        _navigationService.GoBack();
                    }
                }

                



            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
