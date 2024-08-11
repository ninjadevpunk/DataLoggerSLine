using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Collections.ObjectModel;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class FilmLOGViewModel : LOGViewModel
    {
        public readonly FilmLOG _FilmLOG;
        public override CacheContext LOGViewModelContext => CacheContext.Film;



        #region Constructor


        public FilmLOGViewModel(FilmLOG filmLOG, LogCacheViewModel logCacheViewModel, ObservableCollection<CreatePostItViewModel> createPostItViewModels, DataService dataService) :
            base(filmLOG, logCacheViewModel, createPostItViewModels, dataService)
        {
            _FilmLOG = filmLOG;
            EditCommand = new EditCommand(LOGViewModelContext, _vm._navigationService, _vm);

            _cacheMaster.SaveFilmViewModel(this, LOGViewModelContext);
            dataService.SaveSubjectIndex();
            dataService.SavePostItIndex();
        }

        public FilmLOGViewModel(FilmLOG filmLOG, LogCacheViewModel logCacheViewModel, DataService dataService) :
            base(filmLOG, logCacheViewModel, dataService)
        {
            _FilmLOG = filmLOG;
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
