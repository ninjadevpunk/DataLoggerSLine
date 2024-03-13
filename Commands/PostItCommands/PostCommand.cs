using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

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
            _loggerCreateViewModel.PostIts.Add(_item);
            _navigationService.GoBack();
        }
    }
}
