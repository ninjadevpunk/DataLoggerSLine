using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands
{
    /// <summary>
    /// This class will be used to update database logs from the log editor.
    /// </summary>
    public class UpdateCommand : AsyncCommandBase
    {
        private readonly CacheContext? _cacheContext = CacheContext.Coding;
        private readonly ReporterUpdaterViewModel? _editor;
        private readonly ReportDeskViewModel? _desk;
        private readonly LOG? _log;
        private readonly NavigationService? _navigationService;
        private readonly DataService? _dataService;


        public UpdateCommand()
        {

        }

        public UpdateCommand(CacheContext context, ReporterUpdaterViewModel reporterEditorViewModel, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService, DataService dataService, LOG log)
        {
            try
            {
                _cacheContext = context;
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _editor = reporterEditorViewModel ?? throw new ArgumentNullException(nameof(reporterEditorViewModel));
                _desk = reportDeskViewModel ?? throw new ArgumentNullException(nameof(reportDeskViewModel));
                _log = log ?? throw new ArgumentNullException(nameof(log));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred near UpdateCommand(context, reportEditorViewModel, reportDeskViewModel) constructor: {ex.Message}");
            }
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var account = _dataService.GetUser();
                var userID = account.accountID;



                if (string.IsNullOrEmpty(_editor.ApplicationName))
                {
                    _log.Application = await _dataService.FindApplication(userID, "Unknown") ?? new(1, "Unknown");
                }
                else if (_log.Project.Name != _editor.ProjectName)
                {
                    var app = await _dataService.FindApplication(userID, _editor.ApplicationName) ?? new(1, "Unknown");
                    _log.Application = app;
                }

                if (string.IsNullOrEmpty(_editor.ProjectName))
                {
                    _log.Project = await _dataService.FindProject(userID, "Unnamed Project", 3) ?? new("Unnamed Project");
                }
                else if (_log.Project.Name != _editor.ProjectName)
                {
                    var project = await _dataService.FindProject(account.accountID, _editor.ProjectName, _log.Application.appID) ?? new("Unnamed Project");
                    _log.Project = project;
                }

                if (_log.Output.Name != _editor.Output)
                {
                    _log.Output = await _dataService.FindOutput(_editor.Output) ?? new(41, "EXE (*.exe)");
                }

                if (_log.Type.Name != _editor.Type)
                {
                    _log.Type = await _dataService.FindType(_editor.Type) ?? new(39, "Note");
                }

                foreach (var editorPostIt in _editor.PostIts)
                {
                    var subject = await _dataService.FindSubject(editorPostIt.Subject, _log.Category) ??
                                  new(
                                      _log.Category,
                                      account,
                                      editorPostIt.Subject,
                                      _log.Project,
                                      _log.Application);


                    if (editorPostIt.ID == 0)
                    {

                        var newPostIt = new PostIt
                        {
                            Subject = subject,
                            Error = editorPostIt.Error,
                            ERCaptureTime = editorPostIt.DateFound,
                            Solution = editorPostIt.Solution,
                            SOCaptureTime = editorPostIt.DateSolved,
                            Suggestion = editorPostIt.Suggestion,
                            Comment = editorPostIt.Comment,
                            Log = _log,
                            accountID = _log.accountID
                        };

                        _log.PostItList.Add(newPostIt);
                    }
                    else
                    {
                        var existingPostIt = _log.PostItList.First(p => p.postItID == editorPostIt.ID);
                        existingPostIt.Subject = subject;
                        existingPostIt.Error = editorPostIt.Error;
                        existingPostIt.ERCaptureTime = editorPostIt.DateFound;
                        existingPostIt.Solution = editorPostIt.Solution;
                        existingPostIt.SOCaptureTime = editorPostIt.DateSolved;
                        existingPostIt.Suggestion = editorPostIt.Suggestion;
                        existingPostIt.Comment = editorPostIt.Comment;
                    }
                }

                // Remove PostIts deleted in the editor
                var editorIds = _editor.PostIts.Select(p => p.ID).ToList();
                var toRemove = _log.PostItList.Where(p => !editorIds.Contains(p.postItID)).ToList();

                foreach (var removed in toRemove)
                    _log.PostItList.Remove(removed);

                var postIts = new ObservableCollection<EF_PostItViewModel>(_editor.PostIts);

                _log.Content = REPORTViewModel.EF_PostItContent(postIts);

                switch (_cacheContext)
                {
                    case CacheContext.Qt:
                        {





                            break;
                        }
                    default:
                        {
                            var _codingLog = (CodingLOG)_log;
                            var editor = (codeUpdateViewModel)_editor;

                            _codingLog.Bugs = editor.BugsFound;
                            _codingLog.Success = editor.ApplicationOpened;

                            var desk = (CodeReportDeskViewModel)_desk;
                            await desk.UpdateLogsAsync();

                            break;
                        }
                }




                await _dataService.SaveChangesAsync();
                await _navigationService.NavigateToReporter();



            }
            catch (InvalidCastException castx)
            {
                await _dataService.CreateFeedback(castx, "ExecuteAsync(parameter)", "InvalidCastException");
            }
            catch (TaskCanceledException taskx)
            {
                await _dataService.CreateFeedback(taskx, "ExecuteAsync(parameter)", "TaskCanceledException");
            }
            catch (ArgumentNullException nullx)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                await _dataService.CreateFeedback(nullx, "ExecuteAsync(parameter)", "ArgumentNullException");

            }
            catch (IndexOutOfRangeException index)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                await _dataService.CreateFeedback(index, "ExecuteAsync(parameter)", "IndexOutOfRangeException");

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

                await _dataService.CreateFeedback(formx, "ExecuteAsync(parameter)", "FormatException");

            }
            catch (Exception ex)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                await _dataService.CreateFeedback(ex, "ExecuteAsync(parameter)");

            }
        }
    }
}
