using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.ViewModels;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    public class codeEditViewModel : codeCreateViewModel
    {
        private readonly QtLOGViewModel _qtLOGViewModel;
        private readonly CodeLOGViewModel _codeLOGViewModel;

        public codeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService, ViewModelBase viewModelBase)
            : base(navigationService, logCacheViewModel, dataService)
        {

            _codeLOGViewModel = (CodeLOGViewModel)viewModelBase;

            AnnotateCommand = null;
            EditCommand = new AnnotateCommand(ActionType.Edit, Context, _navigationService, this, _logCacheViewModel, _dataService, _codeLOGViewModel);

        }

        public codeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, string app, DataService dataService, ViewModelBase viewModelBase)
            : base(navigationService, logCacheViewModel, app, dataService)
        {
            if (app == "Qt")
            {
                _qtLOGViewModel = (QtLOGViewModel)viewModelBase;
                EditCommand = new AnnotateCommand(ActionType.Edit, CacheContext.Qt, _navigationService, this, _logCacheViewModel, _dataService, _qtLOGViewModel);
            }
            else
            {
                AnnotateCommand = null;
                EditCommand = new AnnotateCommand(ActionType.Edit, Context, _navigationService, this, _logCacheViewModel, _dataService, _codeLOGViewModel);
            }

        }


    }
}
