using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class FlexiLOGViewModel : LOGViewModel
    {
        public FlexiNotesLOG _FlexiLOG;
        public override CacheContext LOGViewModelContext => CacheContext.Flexi;



        #region Constructors



        public FlexiLOGViewModel(FlexiNotesLOG flexiLOG, LogCacheViewModel logCacheViewModel, ObservableCollection<CreatePostItViewModel> createPostItViewModels, DataService dataService) :
            base(flexiLOG, logCacheViewModel, createPostItViewModels, dataService)
        {
            _FlexiLOG = flexiLOG;
            EditCommand = new EditCommand(LOGViewModelContext, _vm._navigationService, _vm);

            _cacheMaster.SaveFlexiViewModel(this, LOGViewModelContext);
            dataService.SaveSubjectIndex();
            dataService.SavePostItIndex();
        }

        public FlexiLOGViewModel(FlexiNotesLOG flexiLOG, LogCacheViewModel logCacheViewModel, DataService dataService) :
            base(flexiLOG, logCacheViewModel, dataService)
        {
            _FlexiLOG = flexiLOG;
            EditCommand = new EditCommand(LOGViewModelContext, _vm._navigationService, _vm);
        }


        #endregion





        #region Properties








        #endregion



        #region Member Functions



        protected override void DeleteCacheItem()
        {
            DeleteCacheFile(ViewModelID, LOGViewModelContext);
            DeleteCacheItemCommand.Execute(this);
        }





        #endregion
    }
}
