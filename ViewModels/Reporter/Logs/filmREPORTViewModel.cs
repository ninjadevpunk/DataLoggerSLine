using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    public class filmREPORTViewModel : REPORTViewModel
    {
        private readonly FilmLOG _FilmLOG;
        public override CacheContext Context => CacheContext.Film;

        public filmREPORTViewModel(FilmLOG filmLOG, NavigationService navigationService, IDataService dataService, PDFService pdfService) : base(filmLOG, navigationService, dataService)
        {
            _FilmLOG = filmLOG;
            SingleExport = new SingleExportCommand(Context, pdfService);
        }




        #region Methods




        /// <summary>
        /// Returns the Film log injected into the ViewModel.
        /// </summary>
        public FilmLOG GetFilmLog => _FilmLOG;





        #endregion

    }
}
