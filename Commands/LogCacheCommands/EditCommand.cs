using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.LogCacheCommands
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
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Argument null exception: {nullx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found: {e.Message}");

                // TODO
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                switch (_cacheContext)
                {
                    case CacheContext.Qt:
                        {
                            var log = parameter as QtLOGViewModel;
                            _navigationService.NavigateToLoggerEditor(_dashboard, log, _cacheContext);

                            break;
                        }
                    case CacheContext.AndroidStudio:
                        {
                            var log = parameter as AndroidLOGViewModel;
                            _navigationService.NavigateToLoggerEditor(_dashboard, log, _cacheContext);

                            break;
                        }
                    case CacheContext.Coding:
                        {
                            var log = parameter as CodeLOGViewModel;
                            _navigationService.NavigateToLoggerEditor(_dashboard, log, _cacheContext);

                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            var log = parameter as GraphicsLOGViewModel;
                            _navigationService.NavigateToLoggerEditor(_dashboard, log, _cacheContext);

                            break;
                        }
                    case CacheContext.Film:
                        {
                            var log = parameter as FilmLOGViewModel;
                            _navigationService.NavigateToLoggerEditor(_dashboard, log, _cacheContext);

                            break;
                        }
                    case CacheContext.Flexi:
                        {
                            var log = parameter as FlexiLOGViewModel;
                            _navigationService.NavigateToLoggerEditor(_dashboard, log, _cacheContext);

                            break;
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
