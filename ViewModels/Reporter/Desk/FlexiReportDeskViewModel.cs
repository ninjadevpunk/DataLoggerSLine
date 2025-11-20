using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    public class FlexiReportDeskViewModel : ReportDeskViewModel
    {
        public override CacheContext Context => CacheContext.Flexi;

        public FlexiReportDeskViewModel(NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
        }

        public FlexiReportDeskViewModel(NavigationService navigationService, DataService dataService, PDFService pdfService) : base(navigationService, dataService, pdfService)
        {
        }

        public override void UpdateLogs(string project)
        {
            
        }
    }
}
