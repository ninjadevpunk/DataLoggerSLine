using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    public class ASReportDeskViewModel : ReportDeskViewModel
    {
        public override CacheContext Context => CacheContext.AndroidStudio;

        public ASReportDeskViewModel(NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
            ReturnToDashboard = new DashboardCommand(_navigationService, Context);
        }

        public ASReportDeskViewModel(NavigationService navigationService, DataService dataService, PDFService pdfService) : base(navigationService, dataService, pdfService)
        {
            ReturnToDashboard = new DashboardCommand(_navigationService, Context);
        }

        public override void UpdateLogs(string project)
        {
            
        }
    }
}
