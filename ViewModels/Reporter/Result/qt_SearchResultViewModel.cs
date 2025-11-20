using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Diagnostics;
using System.Windows;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter
{
    public class qt_SearchResultViewModel : SearchResultViewModel
    {
        private readonly CodingLOG _QtCodingLOG;
        public override CacheContext SearchResultContext => CacheContext.Qt;

        public qt_SearchResultViewModel(CodingLOG log, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService) : base(log, reportDeskViewModel, navigationService)
        {
            _QtCodingLOG = log;
            var style = TryParsePath("path_Qt_icon");
            IconStyle = style == null ? new Style() : style;

            View = new ViewCommand(reportDeskViewModel._navigationService, SearchResultContext, ViewType.Log);
            Edit = new EditLogCommand();
            Delete = new DeleteLogCommand();
        }







        #region Methods




        /// <summary>
        /// Returns the Qt Coding log injected into the ViewModel.
        /// </summary>
        public CodingLOG GetCodingLog => _QtCodingLOG;


        private static Style? TryParsePath(string value)
        {
            try
            {
                return (Style)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException rex)
            {
                Debug.WriteLine($"ResourceReferenceKeyNotFoundException found near qt_SearchResultViewModel.TryParseBrush(): {rex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near qt_SearchResultViewModel.TryParseBrush(): {ex.Message}");
                return null;
            }
        }









        #endregion

    }
}
