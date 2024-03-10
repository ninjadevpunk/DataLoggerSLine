using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.PostIt
{
    public class AddPostItCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LoggerCreateViewModel _loggerCreateViewModel;

        public AddPostItCommand(NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel)
        {


            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
            }
            catch (Exception)
            {
                //
            }


        }

        public override void Execute(object parameter)
        {
            _navigationService.NavigateToPostItCreator(_loggerCreateViewModel);
        }
    }
}
