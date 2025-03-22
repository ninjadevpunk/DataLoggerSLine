using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    /// <summary>
    /// LOG ViewModel class for a log cached in the log cache. Specialised for Qt logs.
    /// </summary>
    public class QtLOGViewModel : LOGViewModel
    {
        public readonly CodingLOG _QtcodingLOG;
        public override CacheContext LOGViewModelContext => CacheContext.Qt;



        #region Constructors




        public QtLOGViewModel(CodingLOG QtcodingLOG, LogCacheViewModel logCacheViewModel, ObservableCollection<CreatePostItViewModel> createPostItViewModels, DataService dataService) :
            base(QtcodingLOG, logCacheViewModel, createPostItViewModels, dataService)
        {
            _QtcodingLOG = QtcodingLOG;
            EditCommand = new EditCommand(LOGViewModelContext, _vm._navigationService, _vm);
            ViewCommand = new ViewCommand(_vm._navigationService, _vm, LOGViewModelContext);

            _cacheMaster.SaveQtViewModel(this, LOGViewModelContext);
            dataService.SaveSubjectIndex();
            dataService.SavePostItIndex();

        }

        public QtLOGViewModel(CodingLOG QtcodingLOG, LogCacheViewModel logCacheViewModel, DataService dataService) :
            base(QtcodingLOG, logCacheViewModel, dataService)
        {
            _QtcodingLOG = QtcodingLOG;
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
