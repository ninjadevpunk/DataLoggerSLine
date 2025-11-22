using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.Cachemaster;

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

        public override async Task UpdateLogs(string project)
        {
            //
        }
    }
}
