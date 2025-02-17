using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;

namespace Data_Logger_1._3.ViewModels.Reporter.Result
{
    public class code_SearchResultViewModel : SearchResultViewModel
    {
        public override CacheContext SearchResultContext => CacheContext.Coding;

        public code_SearchResultViewModel(LOG log, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService) : base(log, reportDeskViewModel, navigationService)
        {
        }
    }
}
