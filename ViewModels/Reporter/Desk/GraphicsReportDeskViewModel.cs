using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    public class GraphicsReportDeskViewModel : ReportDeskViewModel
    {
        public override CacheContext Context => CacheContext.Graphics;
        public GraphicsReportDeskViewModel(NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
        }

        public GraphicsReportDeskViewModel(NavigationService navigationService, DataService dataService, PDFService pdfService) : base(navigationService, dataService, pdfService)
        {
        }

        public override void UpdateLogs(string project)
        {

        }
    }
}
