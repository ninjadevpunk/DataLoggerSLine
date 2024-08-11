using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class AndroidLOGViewModel : LOGViewModel
    {
        public readonly AndroidCodingLOG _AndroidCodingLOG;
        public override CacheContext LOGViewModelContext => CacheContext.AndroidStudio;





        #region Constructors



        public AndroidLOGViewModel(AndroidCodingLOG androidCodingLOG, LogCacheViewModel logCacheViewModel, ObservableCollection<CreatePostItViewModel> createPostItViewModels, DataService dataService) :
            base(androidCodingLOG, logCacheViewModel, createPostItViewModels, dataService)

        {
            _AndroidCodingLOG = androidCodingLOG;
            EditCommand = new EditCommand(CacheContext.AndroidStudio, _vm._navigationService, _vm);

            _cacheMaster.SaveASViewModel(this, LOGViewModelContext);
            dataService.SaveSubjectIndex();
            dataService.SavePostItIndex();
        }

        public AndroidLOGViewModel(AndroidCodingLOG androidCodingLOG, LogCacheViewModel logCacheViewModel, DataService dataService) :
            base(androidCodingLOG, logCacheViewModel, dataService)
        {
            _AndroidCodingLOG = androidCodingLOG;
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
