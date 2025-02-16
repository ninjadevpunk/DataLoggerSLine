using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter;
using Microsoft.Win32;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Windows.Documents;
using System.Windows;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Exports a single log to a PDF.
    /// </summary>
    public class SingleExportCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly DataService _dataService;
        private readonly PDFService _pdfService;

        public CacheContext Context { get; set; }


        public SingleExportCommand()
        {
        }

        public SingleExportCommand(CacheContext context, NavigationService navigationService, DataService dataService, PDFService pdfService)
        {
            try
            {
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
                Context = context;
            }
            catch (Exception ex)
            {

            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                if (parameter is REPORTViewModel report)
                {
                    switch (report.Context)
                    {
                        case CacheContext.Qt:
                            _pdfService.ExportQtLogToPDF(report);
                            break;
                        case CacheContext.AndroidStudio:
                            _pdfService.ExportASLogToPDF(report);
                            break;
                        case CacheContext.Coding:
                            _pdfService.ExportCodingLogToPDF(report);
                            break;
                        case CacheContext.Graphics:
                            _pdfService.ExportGraphicsLogToPDF(report);
                            break;
                        case CacheContext.Film:
                            _pdfService.ExportFilmLogToPDF(report);
                            break;
                        case CacheContext.Flexi:
                            _pdfService.ExportFlexiLogToPDF(report);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near SingleExportCommand.Execute(): {ex.Message}");
            }
        }


    }

}
