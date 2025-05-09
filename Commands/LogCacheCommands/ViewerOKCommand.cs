using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class ViewerOKCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly CacheContext _cacheContext;

        public ViewerOKCommand(NavigationService navigationService, CacheContext cacheContext)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _cacheContext = cacheContext;
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public override void Execute(object parameter)
        {
            _navigationService.NavigateToLogCachePage(_cacheContext);
        }
    }
}
