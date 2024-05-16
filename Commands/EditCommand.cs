using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands
{
    public class EditCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LogCacheViewModel _dashboard;
        private readonly CacheContext _cacheContext;

        public EditCommand(CacheContext cacheContext, NavigationService navigationService, LogCacheViewModel logCacheViewModel)
        {
            try
            {
                _cacheContext = cacheContext;
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                switch(_cacheContext)
                {
                    case CacheContext.Qt:
                        {
                            var log = parameter as QtLOGViewModel;
                            _navigationService.NavigateToLoggerEditor(_dashboard, log, CacheContext.Qt);
                            break;
                        }
                        case CacheContext.AndroidStudio:
                        {

                            break;
                        }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
