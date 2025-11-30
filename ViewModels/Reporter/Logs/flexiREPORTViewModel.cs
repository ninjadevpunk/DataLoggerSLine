using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    public class flexiREPORTViewModel : REPORTViewModel
    {
        private readonly FlexiNotesLOG _FlexiLOG;
        public override CacheContext Context => CacheContext.Flexi;

        public flexiREPORTViewModel(FlexiNotesLOG flexiNotesLOG, NavigationService navigationService, DataService dataService, PDFService pdfService) : base(flexiNotesLOG, navigationService, dataService)
        {
            _FlexiLOG = flexiNotesLOG;
            SingleExport = new SingleExportCommand(Context, pdfService);
        }




        #region Methods




        /// <summary>
        /// Returns the Flexible log injected into the ViewModel.
        /// </summary>
        public FlexiNotesLOG GetFlexiLog => _FlexiLOG;





        #endregion
    }
}
