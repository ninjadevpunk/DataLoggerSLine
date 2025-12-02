using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;
using Data_Logger_1._3.Models;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public enum ViewType
    {
        Cache,
        Log
    }

    public class ViewCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly CacheContext _cacheContext = CacheContext.Coding;
        private ViewType viewType = ViewType.Cache;

        public ViewCommand(NavigationService navigationService, LogCacheViewModel logCacheViewModel, CacheContext cacheContext)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _cacheContext = cacheContext;
                viewType = ViewType.Cache;
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

        public ViewCommand(NavigationService navigationService, CacheContext cacheContext, ViewType viewType)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _cacheContext = cacheContext;
                this.viewType = viewType;
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

        public ViewCommand(LOG log, NavigationService navigationService, CacheContext cacheContext, ViewType viewType)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _cacheContext = cacheContext;
                this.viewType = viewType;
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
                switch (viewType)
                {
                    case ViewType.Cache:
                        {
                            switch (_cacheContext)
                            {
                                case CacheContext.Qt:
                                    {
                                        var log = parameter as QtLOGViewModel;
                                        _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.AndroidStudio:
                                    {
                                        var log = parameter as AndroidLOGViewModel;
                                        _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.Coding:
                                    {
                                        var log = parameter as CodeLOGViewModel;
                                        _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.Graphics:
                                    {
                                        var log = parameter as GraphicsLOGViewModel;
                                        _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.Film:
                                    {
                                        var log = parameter as FilmLOGViewModel;
                                        _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                                case CacheContext.Flexi:
                                    {
                                        var log = parameter as FlexiLOGViewModel;
                                        _navigationService.NavigateToViewer(log);

                                        break;
                                    }
                            }
                            break;
                        }
                    case ViewType.Log:
                        {

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
