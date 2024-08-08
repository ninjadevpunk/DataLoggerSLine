using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostItCommands
{
    public class EditPostItCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LoggerCreateViewModel _loggerCreateViewModel;

        public EditPostItCommand()
        {

        }

        public EditPostItCommand(NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
            }
            catch (ArgumentNullException ex)
            {
                // TODO
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
                _navigationService.NavigateToPostItEditor(_loggerCreateViewModel, parameter as CreatePostItViewModel);
            }
            catch (ArgumentNullException ex)
            {
                //
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
