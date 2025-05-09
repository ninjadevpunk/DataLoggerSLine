using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;
using Data_Logger_1._3.ViewModels.Reporter.Logs;
using Data_Logger_1._3.ViewModels.Reporter;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    /// <summary>
    /// This class will ne used to update database logs from the log editor.
    /// </summary>
    public class UpdateCommand : CommandBase
    {
        private readonly CacheContext _cacheContext;
        private readonly ReporterUpdaterViewModel _editor;
        private readonly ReportDeskViewModel _desk;
        private readonly REPORTViewModel _report;
        private readonly NavigationService _navigationService;
        private readonly DataService _dataService;
        private readonly PDFService _pdfService;


        public UpdateCommand()
        {

        }

        public UpdateCommand(CacheContext context, ReporterUpdaterViewModel reporterEditorViewModel, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService, DataService dataService, REPORTViewModel reportViewModel, PDFService pdfService)
        {
            try
            {
                _cacheContext = context;
                _editor = reporterEditorViewModel ?? throw new ArgumentNullException(nameof(reporterEditorViewModel));
                _desk = reportDeskViewModel ?? throw new ArgumentNullException(nameof(reportDeskViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _report = reportViewModel ?? throw new ArgumentNullException(nameof(reportViewModel));
                _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred near UpdateCommand(context, reportEditorViewModel, reportDeskViewModel) constructor: {ex.Message}");
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                var account = _dataService.GetUser();


                List<PostIt> posts = new();
                ApplicationClass? application = null;

                ProjectClass? project = new(1, account, _editor.ProjectName, application, _editor.Category, false);


                OutputClass? output = new(0, account, _editor.Output, application, _editor.Category);
                TypeClass? type = new(0, account, _editor.Type, application, _editor.Category);
                SubjectClass subject;

                PostIt postIt;

                bool DateIsWrong = _editor.StartDate.Equals(_editor.EndDate) || _editor.StartDate > _editor.EndDate;

                foreach (var item in _editor.PostIts)
                {
                    postIt = new();

                    postIt.postItID = 0;

                    subject = new(0, _editor.Category, account, item.Subject, project, application);

                    postIt.Subject = subject;

                    postIt.Error = item.Error;
                    postIt.ERCaptureTime = item.DateFound;
                    postIt.Solution = item.Solution;
                    postIt.SOCaptureTime = item.DateSolved;
                    postIt.Suggestion = item.Suggestion;
                    postIt.Comment = item.Comment;

                    posts.Add(postIt);
                }



                int index = -1;


                switch (_cacheContext)
                {
                    case CacheContext.Qt:
                        {
                            codeUpdateViewModel _QtviewModel = (codeUpdateViewModel)_editor;
                            QtReportDeskViewModel qtDesk = (QtReportDeskViewModel)_desk;
                            qtREPORTViewModel oldLOG = (qtREPORTViewModel)_report;

                            var oldApp = oldLOG.GetQtCodingLog.Application;
                            project.projectID = oldLOG.GetQtCodingLog.Project.projectID;
                            project.Application = oldApp;
                            application = oldApp;
                            application.appID = 1;
                            application.IsDefault = true;
                            output.Application = oldApp;
                            type.Application = oldApp;

                            foreach (var item in posts)
                            {

                                item.Subject.Application = oldApp;
                                item.Subject.Project = project;
                            }

                            index = qtDesk.Logs.IndexOf(oldLOG);

                            if (index != -1)
                            {
                                var list = qtDesk.Logs;

                                qtREPORTViewModel newLOG;

                                if (DateIsWrong)
                                {
                                    newLOG = new(new CodingLOG(
                                        oldLOG.GetQtCodingLog.ID,
                                        account,
                                        project,
                                        application,
                                        DateTime.Now,
                                        DateTime.Now,
                                        output,
                                        type,
                                        posts,
                                        _QtviewModel.BugsFound,
                                        _QtviewModel.ApplicationOpened
                                        ), qtDesk, _navigationService, _dataService, _pdfService);
                                }
                                else
                                {
                                    newLOG = new(new CodingLOG(
                                        oldLOG.GetQtCodingLog.ID,
                                        account,
                                        project,
                                        application,
                                        _QtviewModel.StartDate,
                                        _QtviewModel.EndDate,
                                        output,
                                        type,
                                        posts,
                                        _QtviewModel.BugsFound,
                                        _QtviewModel.ApplicationOpened
                                       ), qtDesk, _navigationService, _dataService, _pdfService);
                                }


                                list[index] = newLOG;

                                qtDesk.Logs = list;

                                // Update database with the new log
                                _dataService.UpdateQtLog(newLOG.GetQtCodingLog);
                            }





                            break;
                        }
                }


            }
            catch (InvalidCastException castx)
            {
                Debug.WriteLine($"Invalid cast canceled exception found in UpdateCommand.Execute(): {castx.Message}");
            }
            catch (TaskCanceledException taskx)
            {
                Debug.WriteLine($"Task canceled exception found in UpdateCommand.Execute(): {taskx.Message}");
            }
            catch (ArgumentNullException nullx)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Argument null exception found in UpdateCommand.Execute(): {nullx.Message}");

            }
            catch (IndexOutOfRangeException index)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Index out of range exception found in UpdateCommand.Execute(): {index.Message}");

            }
            catch (FormatException formx)
            {
                if (formx.Message.Equals("The input string '' was not in a correct format."))
                {
                    MessageBox.Show($"An error occurred. Please ensure you entered numbers only in fields that require numeric values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }

                Debug.WriteLine($"Format exception found in UpdateCommand.Execute(): {formx.Message}");

            }
            catch (Exception ex)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Exception found in UpdateCommand.Execute(): {ex.Message}");

            }
        }
    }
}
