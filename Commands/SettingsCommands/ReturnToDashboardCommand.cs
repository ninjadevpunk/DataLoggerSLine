using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.SettingsCommands
{
    public class ReturnToDashboardCommand : AsyncCommandBase
    {

        private readonly NavigationService _navigationService;
        private readonly IDataService _dataService;


        public ReturnToDashboardCommand(NavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }



        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                _navigationService.NavigateToLogCachePage();
            }
            catch (Exception e)
            {
                await _dataService.HandleExceptionAsync(e, "Exception occurred in ReturnToDashboardCommand.ExecuteAsync");
            }
        }



    }
}
