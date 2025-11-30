using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    public class codeREPORTViewModel : REPORTViewModel
    {
        private readonly CodingLOG _CodingLOG;
        public override CacheContext Context => CacheContext.Coding;


        public codeREPORTViewModel(CodingLOG codingLOG, NavigationService navigationService, DataService dataService, PDFService pdfService) : base(codingLOG, navigationService, dataService)
        {
            _CodingLOG = codingLOG;
            SingleExport = new SingleExportCommand(Context, pdfService);
        }




        #region Methods




        /// <summary>
        /// Returns the Coding log injected into the ViewModel.
        /// </summary>
        public CodingLOG GetCodingLog => _CodingLOG;





        #endregion

    }
}
