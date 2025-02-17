using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    public class CodeReportDeskViewModel : ReportDeskViewModel
    {
        public override CacheContext Context => CacheContext.Coding;

        public CodeReportDeskViewModel(NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
        }

        public CodeReportDeskViewModel(NavigationService navigationService, DataService dataService, PDFService pdfService) : base(navigationService, dataService, pdfService)
        {
        }

        public override void UpdateLogs(string project)
        {
            
        }
    }
}
