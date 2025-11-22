using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteCacheItemCommand : CommandBase
    {
        private readonly LogCacheViewModel _viewModel;
        private readonly DataService _dataService;
        private readonly bool _timeUp;

        public DeleteCacheItemCommand(LogCacheViewModel logCacheViewModel, DataService dataService, bool timeIsUp)
        {
            try
            {
                _viewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _timeUp = timeIsUp;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Argument null exception found: {ex.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found: {e.Message}");
                // TODO
            }
        }

        public override async void Execute(object parameter)
        {
            try
            {
                var logViewModel = parameter as LOGViewModel;

                switch (logViewModel.LOGViewModelContext)
                {
                    case CacheContext.Qt:
                        HandleQtCacheItem((QtLOGViewModel)logViewModel);
                        break;
                    case CacheContext.AndroidStudio:
                        HandleAndroidCacheItem((AndroidLOGViewModel)logViewModel);
                        break;
                    case CacheContext.Coding:
                        HandleCodingCacheItem((CodeLOGViewModel)logViewModel);
                        break;
                    case CacheContext.Graphics:
                        HandleGraphicsCacheItem((GraphicsLOGViewModel)logViewModel);
                        break;
                    case CacheContext.Film:
                        HandleFilmCacheItem((FilmLOGViewModel)logViewModel);
                        break;
                    case CacheContext.Flexi:
                        HandleFlexiCacheItem((FlexiLOGViewModel)logViewModel);
                        break;
                }
            }
            catch (InvalidCastException castx)
            {
                _dataService.CreateFeedback(castx, "Execute() DeleteCacheItemCommand", "InvalidCastException");
            }
            catch (Exception e)
            {
                _dataService.CreateFeedback(e, "Execute() DeleteCacheItemCommand");
            }
        }

        private async void HandleQtCacheItem(QtLOGViewModel item)
        {
            var tempQtDashBoardItems = ((CodingQtViewModel)_viewModel).CacheItems;
            bool isLogged = false;

            if (_timeUp)
            {
                // Send data to database first
                isLogged = await _dataService.StoreLog(item._QtcodingLOG);

                if (isLogged)
                {
                    tempQtDashBoardItems.Remove(item);
                    ((CodingQtViewModel)_viewModel).CacheItems = tempQtDashBoardItems;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                item._timer.Dispose();
                tempQtDashBoardItems.Remove(item);
                ((CodingQtViewModel)_viewModel).CacheItems = tempQtDashBoardItems;
            }
        }

        private async void HandleAndroidCacheItem(AndroidLOGViewModel item)
        {
            var tempASDashboardItems = ((CodingAndroidViewModel)_viewModel).CacheItems;
            bool isLogged = false;

            if (_timeUp)
            {
                // Send data to database first
                isLogged = await _dataService.StoreLog(item._AndroidCodingLOG);

                if (isLogged)
                {
                    tempASDashboardItems.Remove(item);
                    ((CodingAndroidViewModel)_viewModel).CacheItems = tempASDashboardItems;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                item._timer.Dispose();
                tempASDashboardItems.Remove(item);
                ((CodingAndroidViewModel)_viewModel).CacheItems = tempASDashboardItems;
            }
        }

        private async void HandleCodingCacheItem(CodeLOGViewModel item)
        {
            var tempCodingDashboardItems = ((CodingViewModel)_viewModel).CacheItems;
            bool isLogged = false;

            if (_timeUp)
            {
                // Send data to database first
                isLogged = await _dataService.StoreLog(item._CodeLOG);

                if (isLogged)
                {
                    tempCodingDashboardItems.Remove(item);
                    ((CodingViewModel)_viewModel).CacheItems = tempCodingDashboardItems;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                item._timer.Dispose();
                tempCodingDashboardItems.Remove(item);
                ((CodingViewModel)_viewModel).CacheItems = tempCodingDashboardItems;
            }
        }

        private async void HandleGraphicsCacheItem(GraphicsLOGViewModel item)
        {
            var tempGraphicsDashboard = ((GraphicsViewModel)_viewModel).CacheItems;
            bool isLogged = false;

            if (_timeUp)
            {
                // Send data to database first
                isLogged = await _dataService.StoreLog(item._GraphicsLOG);

                if (isLogged)
                {
                    tempGraphicsDashboard.Remove(item);
                    ((GraphicsViewModel)_viewModel).CacheItems = tempGraphicsDashboard;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                item._timer.Dispose();
                tempGraphicsDashboard.Remove(item);
                ((GraphicsViewModel)_viewModel).CacheItems = tempGraphicsDashboard;
            }
        }

        private async void HandleFilmCacheItem(FilmLOGViewModel item)
        {
            var tempFilmDashboardItems = ((FilmViewModel)_viewModel).CacheItems;
            bool isLogged = false;

            if (_timeUp)
            {
                // Send data to database first
                isLogged = await _dataService.StoreLog(item._FilmLOG);

                if (isLogged)
                {
                    tempFilmDashboardItems.Remove(item);
                    ((FilmViewModel)_viewModel).CacheItems = tempFilmDashboardItems;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                item._timer.Dispose();
                tempFilmDashboardItems.Remove(item);
                ((FilmViewModel)_viewModel).CacheItems = tempFilmDashboardItems;
            }
        }

        private async void HandleFlexiCacheItem(FlexiLOGViewModel item)
        {
            var tempFlexiDashboardItems = ((FlexiViewModel)_viewModel).CacheItems;
            bool isLogged = false;

            if (_timeUp)
            {
                // Send data to database first
                isLogged = await _dataService.StoreLog(item._FlexiLOG);

                if (isLogged)
                {
                    tempFlexiDashboardItems.Remove(item);
                    ((FlexiViewModel)_viewModel).CacheItems = tempFlexiDashboardItems;
                }
            }
            else
            {
                _dataService.DeleteCacheFile(item.StartAsID, item.LOGViewModelContext);
                item._timer.Dispose();
                tempFlexiDashboardItems.Remove(item);
                ((FlexiViewModel)_viewModel).CacheItems = tempFlexiDashboardItems;
            }
        }









    }
}
