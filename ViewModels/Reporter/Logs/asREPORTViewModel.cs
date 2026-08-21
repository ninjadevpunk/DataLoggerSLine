using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    public class asREPORTViewModel : REPORTViewModel
    {
        private readonly AndroidCodingLOG _AndroidCodingLOG;
        public override CacheContext Context => CacheContext.AndroidStudio;

        public asREPORTViewModel(AndroidCodingLOG androidCodingLOG, NavigationService navigationService, IDataService dataService, PDFService pdfService) : base(androidCodingLOG, navigationService, dataService)
        {
            _AndroidCodingLOG = androidCodingLOG;
            SingleExport = new SingleExportCommand(Context, pdfService);
        }


        #region Methods




        /// <summary>
        /// Returns the Android Studio log injected into the ViewModel.
        /// </summary>
        public AndroidCodingLOG GetAndroidCodingLog => _AndroidCodingLOG;





        #endregion


    }
}
