using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    public class qtREPORTViewModel : REPORTViewModel
    {
        private readonly CodingLOG _QtcodingLOG;
        public override CacheContext Context => CacheContext.Qt;

        public qtREPORTViewModel(CodingLOG codingLOG, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService, IDataService dataService, PDFService pdfService) : base(codingLOG, navigationService, dataService)
        {
            _QtcodingLOG = codingLOG;
            SingleExport = new SingleExportCommand(Context, pdfService);
            EditLogCommand = new EditLogCommand(codingLOG, reportDeskViewModel, navigationService);
        }




        #region Methods



        /// <summary>
        /// Returns the Qt Coding log injected into the ViewModel.
        /// </summary>
        public CodingLOG GetQtCodingLog => _QtcodingLOG;








        #endregion
    }
}
