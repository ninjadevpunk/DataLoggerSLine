using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LogCacheCommands
{
    public class DeleteQtCacheItemCommand : CommandBase
    {
        private readonly LogCacheViewModel _viewModel;
        private readonly DataService _dataService;
        private readonly bool _timeUp;


        public DeleteQtCacheItemCommand(LogCacheViewModel logCacheViewModel, DataService dataService, bool timeIsUp)
        {
            try
            {
                _viewModel = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _timeUp = timeIsUp;
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
                CodingQtViewModel QtDashboard = (CodingQtViewModel)_viewModel;
                var item = parameter as QtLOGViewModel;
                bool isLogged = false;

                if (QtDashboard is not null && item is not null)
                {
                    var tempQtDashBoardItems = QtDashboard.CacheItems;

                    // Remove item
                    if (_timeUp)
                    {
                        // Send data to database first
                        isLogged = _dataService.StoreLog(item._QtcodingLOG);

                        if (isLogged)
                        {
                            tempQtDashBoardItems.Remove(item);
                            QtDashboard.CacheItems = tempQtDashBoardItems;
                        }
                    }
                    else
                    {
                        // TODO


                        _dataService.DeleteCacheFile(item._QtcodingLOG.ID, CacheContext.Qt);
                        item._timer.Dispose();
                        tempQtDashBoardItems.Remove(item);
                        QtDashboard.CacheItems = tempQtDashBoardItems;
                    }

                }





            }
            catch (Exception e)
            {
                // TODO
            }

        }

    }
}
