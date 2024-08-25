using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class CodeLOGViewModel : LOGViewModel
    {
        public readonly CodingLOG _CodeLOG;
        public override CacheContext LOGViewModelContext => CacheContext.Coding;



        #region Constructors



        public CodeLOGViewModel(CodingLOG codingLOG, LogCacheViewModel logCacheViewModel, ObservableCollection<CreatePostItViewModel> createPostItViewModels, DataService dataService) :
            base(codingLOG, logCacheViewModel, createPostItViewModels, dataService)
        {
            _CodeLOG = codingLOG;
            EditCommand = new EditCommand(LOGViewModelContext, _vm._navigationService, _vm);
            ViewCommand = new ViewCommand(_vm._navigationService, _vm, LOGViewModelContext);

            _cacheMaster.SaveCodeViewModel(this, LOGViewModelContext);
            dataService.SaveSubjectIndex();
            dataService.SavePostItIndex();
        }

        public CodeLOGViewModel(CodingLOG codingLOG, LogCacheViewModel logCacheViewModel, DataService dataService) :
            base(codingLOG, logCacheViewModel, dataService)
        {
            _CodeLOG = codingLOG;
            EditCommand = new EditCommand(LOGViewModelContext, _vm._navigationService, _vm);
            ViewCommand = new ViewCommand(_vm._navigationService, _vm, LOGViewModelContext);
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
