using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    public class qtREPORTViewModel : REPORTViewModel
    {
        private readonly CodingLOG _QtcodingLOG;
        public override CacheContext Context => CacheContext.Qt;

        public qtREPORTViewModel(CodingLOG codingLOG, NavigationService navigationService, DataService dataService, PDFService pdfService) : base(codingLOG, navigationService, dataService)
        {
            _QtcodingLOG = codingLOG;
            SingleExport = new SingleExportCommand(Context, navigationService, dataService, pdfService);
        }




        #region Methods



        /// <summary>
        /// Returns the Qt Coding log injected into the ViewModel.
        /// </summary>
        public CodingLOG GetQtCodingLog => _QtcodingLOG;








        #endregion
    }
}
