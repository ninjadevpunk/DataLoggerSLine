using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.LoggerCommands
{
    public class SaveCommand : CommandBase
    {
        private readonly LoggerCreateViewModel _loggerCreateViewModel;
        private readonly DataService _dataService;
        private readonly ApplicationClass QtCreator;
        private readonly ApplicationClass AndroidStudio;


        public SaveCommand(LoggerCreateViewModel loggerCreateViewModel, DataService dataService)
        {
            try
            {
                _loggerCreateViewModel = loggerCreateViewModel ?? throw new ArgumentNullException(nameof(loggerCreateViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

                QtCreator = _dataService.FindApplicationByID(1);
                AndroidStudio = _dataService.FindApplicationByID(2);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Argument null exception found: {ex.Message}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                ProjectClass project = new(_loggerCreateViewModel.ProjectName);
                OutputClass output = new(_loggerCreateViewModel.Output);
                TypeClass type = new(_loggerCreateViewModel.Type);
                List<PostIt> postItsList = new();
                PostIt postIt;
                SubjectClass subject;

                foreach (var item in _loggerCreateViewModel.PostIts)
                {
                    postIt = new();

                    postIt.ID = 0;

                    subject = new(item.Subject);

                    postIt.Subject = subject;

                    postIt.Error = item.Error;
                    postIt.ERCaptureTime = item.DateFound;
                    postIt.Solution = item.Solution;
                    postIt.SOCaptureTime = item.DateSolved;
                    postIt.Suggestion = item.Suggestion;
                    postIt.Comment = item.Comment;

                    postItsList.Add(postIt);
                }

                switch (_loggerCreateViewModel.Context)
                {
                    case CacheContext.Qt:
                        {
                            dialog.DefaultExt = ".qtlog"; // Default file extension
                            dialog.Filter = "Qt LOG (.qtlog)|*qtlog|JSON (.json)|*.json"; // Filter files by extension

                            // Show save file dialog box
                            bool? result = dialog.ShowDialog();

                            // Process save file dialog box results
                            if (result == true)
                            {
                                var codingCreator = (codeCreateViewModel)_loggerCreateViewModel;
                                _dataService.SaveLog(new CodingLOG(
                                        0,
                                        _dataService.GetUser(),
                                        project,
                                        QtCreator,
                                        codingCreator.StartDate,
                                        codingCreator.EndDate,
                                        output,
                                        type,
                                        postItsList,
                                        codingCreator.BugsFound,
                                        codingCreator.ApplicationOpened
                                        ),
                                        dialog.FileName);
                            }
                            break;
                        }
                    case CacheContext.AndroidStudio:
                        {
                            break;
                        }
                    case CacheContext.Coding:
                        {
                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            break;
                        }
                    case CacheContext.Film:
                        {
                            break;
                        }
                    case CacheContext.NOTES:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
