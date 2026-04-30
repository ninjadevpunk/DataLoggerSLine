using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    public class codeREPORTViewModel : REPORTViewModel
    {
        private readonly CodingLOG _CodingLOG;
        public override CacheContext Context => CacheContext.Coding;


        public codeREPORTViewModel(CodingLOG codingLOG, CodeReportDeskViewModel codeReportDeskViewModel, NavigationService navigationService, IDataService dataService, PDFService pdfService) : 
            base(codingLOG, navigationService, dataService)
        {
            _CodingLOG = codingLOG;
            SingleExport = new SingleExportCommand(codingLOG, Context, pdfService);
            EditLogCommand = new EditLogCommand(codingLOG, codeReportDeskViewModel, navigationService);
            ViewLogCommand = new ViewCommand(navigationService, Context, ViewType.Log, codingLOG);
            DeleteLogCommand = new DeleteLogCommand(dataService, codeReportDeskViewModel, Context, codingLOG);
        }




        #region Methods




        /// <summary>
        /// Returns the Coding log injected into the ViewModel.
        /// </summary>
        public CodingLOG GetCodingLog => _CodingLOG;





        #endregion

    }
}
