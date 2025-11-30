using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{

    public class graphicsREPORTViewModel : REPORTViewModel
    {
        private readonly GraphicsLOG _GraphicsLOG;
        public override CacheContext Context => CacheContext.Graphics;

        public graphicsREPORTViewModel(GraphicsLOG graphicsLOG, NavigationService navigationService, DataService dataService, PDFService pdfService) : base(graphicsLOG, navigationService, dataService)
        {
            _GraphicsLOG = graphicsLOG;

            SingleExport = new SingleExportCommand(Context, pdfService);
        }




        #region Methods




        /// <summary>
        /// Returns the Graphics log injected into the ViewModel.
        /// </summary>
        public GraphicsLOG GetGraphicsLog => _GraphicsLOG;





        #endregion





    }
}
