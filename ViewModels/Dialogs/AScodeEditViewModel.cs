using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class AScodeEditViewModel : AScodeCreateViewModel
    {
        private readonly AndroidLOGViewModel _androidLOGViewModel;

        public AScodeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, DataService dataService, ViewModelBase viewModelBase) : base(navigationService, logCacheViewModel, dataService)
        {
            _androidLOGViewModel = (AndroidLOGViewModel)viewModelBase;

            AnnotateCommand = null;
            EditCommand = new AnnotateCommand(ActionType.Edit, Context, _navigationService, this, _logCacheViewModel, _dataService, _androidLOGViewModel);
        }
    }
}
