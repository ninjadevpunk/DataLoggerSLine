using Data_Logger_1._3.Services;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    public class FilmReportDeskViewModel : ReportDeskViewModel
    {
        public override CacheContext Context => CacheContext.Film;

        public FilmReportDeskViewModel(NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
        }

        public FilmReportDeskViewModel(NavigationService navigationService, DataService dataService, PDFService pdfService) : base(navigationService, dataService, pdfService)
        {
        }

        public override void UpdateLogs(string project)
        {
            
        }
    }
}
