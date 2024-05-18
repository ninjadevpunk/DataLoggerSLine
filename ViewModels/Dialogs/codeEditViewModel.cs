using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class codeEditViewModel : codeCreateViewModel
    {
        private readonly QtLOGViewModel _qtLOGViewModel;
        private readonly CodeLOGViewModel _codeLOGViewModel;


        public codeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService, ViewModelBase viewModelBase) : base(navigationService, logCacheViewModel, dataService)
        {

            _codeLOGViewModel = (CodeLOGViewModel)viewModelBase;

            AnnotateCommand = new AnnotateCommand(ActionType.Edit, LogType, _navigationService, this, _logCacheViewModel, _dataService, _codeLOGViewModel);

        }

        public codeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, string app, DataService dataService, ViewModelBase viewModelBase) : base(navigationService, logCacheViewModel, app, dataService)
        {
            if (app == "Qt")
            {
                _qtLOGViewModel = (QtLOGViewModel)viewModelBase;
                AnnotateCommand = new AnnotateCommand(ActionType.Edit, LogType, _navigationService, this, _logCacheViewModel, _dataService, _qtLOGViewModel);
            }
            else
            {
                _codeLOGViewModel = (CodeLOGViewModel)viewModelBase;
                AnnotateCommand = new AnnotateCommand(ActionType.Edit, LogType, _navigationService, this, _logCacheViewModel, _dataService, _codeLOGViewModel);
            }

        }   


    }
}
