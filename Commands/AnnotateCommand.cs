using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    public class AnnotateCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly DataService _dataService;
        private readonly LoggerCreateViewModel _viewModel;
        private readonly LogCacheViewModel _dashboard;


        public string _TYPE { get; set; }

        public AnnotateCommand(string TYPE, NavigationService navigationService, LoggerCreateViewModel viewModel, LogCacheViewModel logCacheViewModel, DataService dataService)
        {


            try
            {
                _TYPE = TYPE;
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

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
                List<PostIt> posts = new();

                foreach (var item in _viewModel.PostIts)
                {
                    PostIt p = new();

                    p.Subject = item.Subject;

                    p.Error = item.Error;
                    p.ERCaptureTime = item.DateFound;
                    p.Solution = item.Solution;
                    p.SOCaptureTime = item.DateSolved;
                    p.Suggestion = item.Suggestion;
                    p.Comment = item.Comment;

                    posts.Add(p);
                }


                // DUMMY ACCOUNT
                var account = new ACCOUNT("Anda", "Mbambo", "", true);





                switch (_TYPE)
                {
                    case "CODING LOG":
                        {
                            if (_viewModel.ApplicationName == "Android Studio Hedgehog 2023.1.1")
                            {
                                AScodeCreateViewModel _ASviewModel = (AScodeCreateViewModel)_viewModel;
                                CodingAndroidViewModel cavm = (CodingAndroidViewModel)_dashboard;

                                AndroidCodingLOG.SCOPE s;
                                s = _ASviewModel.FullORSimple ? AndroidCodingLOG.SCOPE.SIMPLE : AndroidCodingLOG.SCOPE.FULL;


                                if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                                {
                                    cavm.CacheItems.Add(new AndroidLOGViewModel(new AndroidCodingLOG(
                                        account,
                                        _ASviewModel.ProjectName,
                                        _ASviewModel.ApplicationName,
                                        _ASviewModel.StartDate,
                                        DateTime.Now,
                                        _ASviewModel.Output,
                                        _ASviewModel.Type,
                                        posts,
                                        _ASviewModel.BugsFound,
                                        _ASviewModel.ApplicationOpened,
                                        s,
                                        _ASviewModel.SyncTime,
                                        _ASviewModel.GradleDaemonTime,
                                        _ASviewModel.RunBuildTime,
                                        _ASviewModel.LoadBuildTime,
                                        _ASviewModel.ConfigureBuildTime,
                                        _ASviewModel.AllProjectsTime
                                        ), cavm, _ASviewModel.PostIts, _dataService));
                                }
                                else
                                {
                                    cavm.CacheItems.Add(new AndroidLOGViewModel(new AndroidCodingLOG(
                                        account,
                                        _ASviewModel.ProjectName,
                                        _ASviewModel.ApplicationName,
                                        _ASviewModel.StartDate,
                                        _ASviewModel.EndDate,
                                        _ASviewModel.Output,
                                        _ASviewModel.Type,
                                        posts,
                                        _ASviewModel.BugsFound,
                                        _ASviewModel.ApplicationOpened,
                                        s,
                                        _ASviewModel.SyncTime,
                                        _ASviewModel.GradleDaemonTime,
                                        _ASviewModel.RunBuildTime,
                                        _ASviewModel.LoadBuildTime,
                                        _ASviewModel.ConfigureBuildTime,
                                        _ASviewModel.AllProjectsTime
                                        ), cavm, _ASviewModel.PostIts, _dataService));
                                }
                                
                                
                                cavm.LogCount = cavm.CacheItems.Count.ToString() + " android studio logs cached | x total logs";

                                _navigationService.NavigateToLogCachePage(CacheContext.AndroidStudio);

                            }
                            else if (_viewModel.ApplicationName == "Qt Creator")
                            {
                                codeCreateViewModel _QtviewModel = (codeCreateViewModel)_viewModel;
                                CodingQtViewModel qtvm = (CodingQtViewModel)_dashboard;

                                if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                                {
                                    qtvm.CacheItems.Add(new QtLOGViewModel(new CodingLOG(
                                        account,
                                        _QtviewModel.ProjectName,
                                        "Qt Creator",
                                        _QtviewModel.StartDate,
                                        DateTime.Now,
                                        _QtviewModel.Output,
                                        _QtviewModel.Type,
                                        posts,
                                        _QtviewModel.BugsFound,
                                        _QtviewModel.ApplicationOpened
                                        ), qtvm, _QtviewModel.PostIts, _dataService));
                                }
                                else
                                {
                                    qtvm.CacheItems.Add(new QtLOGViewModel(new CodingLOG(
                                        account,
                                        _QtviewModel.ProjectName,
                                        _QtviewModel.ApplicationName,
                                        _QtviewModel.StartDate,
                                        _QtviewModel.EndDate,
                                        _QtviewModel.Output,
                                        _QtviewModel.Type,
                                        posts,
                                        _QtviewModel.BugsFound,
                                        _QtviewModel.ApplicationOpened
                                        ), qtvm, _QtviewModel.PostIts, _dataService));
                                }

                                qtvm.LogCount = qtvm.CacheItems.Count.ToString() + " qt logs cached | x total logs";

                                _navigationService.NavigateToLogCachePage(CacheContext.Qt);

                            }
                            else
                            {
                                codeCreateViewModel _GENviewModel = (codeCreateViewModel)_viewModel;
                                CodingGenericViewModel genvm = (CodingGenericViewModel)_dashboard;

                                if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                                {
                                    genvm.CacheItems.Add(new CodeLOGViewModel(new CodingLOG(
                                        account,
                                        _GENviewModel.ProjectName,
                                        _GENviewModel.ApplicationName,
                                        _GENviewModel.StartDate,
                                        DateTime.Now,
                                        _GENviewModel.Output,
                                        _GENviewModel.Type,
                                        posts,
                                        _GENviewModel.BugsFound,
                                        _GENviewModel.ApplicationOpened
                                        ), genvm, _GENviewModel.PostIts, _dataService));
                                }
                                else
                                {
                                    genvm.CacheItems.Add(new CodeLOGViewModel(new CodingLOG(
                                        account,
                                        _GENviewModel.ProjectName,
                                        _GENviewModel.ApplicationName,
                                        _GENviewModel.StartDate,
                                        _GENviewModel.EndDate,
                                        _GENviewModel.Output,
                                        _GENviewModel.Type,
                                        posts,
                                        _GENviewModel.BugsFound,
                                        _GENviewModel.ApplicationOpened
                                        ), genvm, _GENviewModel.PostIts, _dataService));
                                }

                                genvm.LogCount = genvm.CacheItems.Count.ToString() + " coding logs cached | x total logs";

                                _navigationService.NavigateToLogCachePage(CacheContext.Generic);

                            }
                            break;
                        }
                    case "GRAPHICS LOG":
                        {
                            graphicCreateViewModel _GRAviewModel = (graphicCreateViewModel)_viewModel;
                            GraphicsViewModel gvm = (GraphicsViewModel)_dashboard;

                            if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                            {
                                
                                gvm.CacheItems.Add(new GraphicsLOGViewModel(new GraphicsLOG(
                                    account,
                                    _GRAviewModel.ProjectName,
                                    _GRAviewModel.ApplicationName,
                                    _GRAviewModel.StartDate,
                                    DateTime.Now,
                                    _GRAviewModel.Output,
                                    _GRAviewModel.Type,
                                    posts,
                                    _GRAviewModel.Medium,
                                    _GRAviewModel.Format,
                                    _GRAviewModel.Brush,
                                    double.Parse(_GRAviewModel.Height),
                                    double.Parse(_GRAviewModel.Width),
                                    _GRAviewModel.MeasuringUnit,
                                    _GRAviewModel.Size,
                                    double.Parse(_GRAviewModel.DPI),
                                    _GRAviewModel.ColourDepth,
                                    _GRAviewModel.IsCompleted,
                                    _GRAviewModel.Source
                                    ), gvm, _GRAviewModel.PostIts, _dataService));
                            }
                            else
                            {

                                gvm.CacheItems.Add(new GraphicsLOGViewModel(new GraphicsLOG(
                                    account,
                                    _GRAviewModel.ProjectName,
                                    _GRAviewModel.ApplicationName,
                                    _GRAviewModel.StartDate,
                                    _GRAviewModel.EndDate,
                                    _GRAviewModel.Output,
                                    _GRAviewModel.Type,
                                    posts,
                                    _GRAviewModel.Medium,
                                    _GRAviewModel.Format,
                                    _GRAviewModel.Brush,
                                    double.Parse(_GRAviewModel.Height),
                                    double.Parse(_GRAviewModel.Width),
                                    _GRAviewModel.MeasuringUnit,
                                    _GRAviewModel.Size,
                                    double.Parse(_GRAviewModel.DPI),
                                    _GRAviewModel.ColourDepth,
                                    _GRAviewModel.IsCompleted,
                                    _GRAviewModel.Source
                                    ), gvm, _GRAviewModel.PostIts, _dataService));

                            }

                            gvm.LogCount = gvm.CacheItems.Count.ToString() + " graphics logs cached | x total logs";

                            _navigationService.NavigateToLogCachePage(CacheContext.Graphics);

                            break;
                        }
                    case "FILM LOG":
                        {
                            filmCreateViewModel _FILMviewModel = (filmCreateViewModel)_viewModel;
                            FilmViewModel fvm = (FilmViewModel)_dashboard;

                            if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                            {
                                fvm.CacheItems.Add(new FilmLOGViewModel(new FilmLOG(
                                    account,
                                    _FILMviewModel.ProjectName,
                                    _FILMviewModel.ApplicationName,
                                    _FILMviewModel.StartDate,
                                    DateTime.Now,
                                    _FILMviewModel.Output,
                                    _FILMviewModel.Type,
                                    posts,
                                    double.Parse(_FILMviewModel.Height),
                                    double.Parse(_FILMviewModel.Width),
                                    _FILMviewModel.Length,
                                    _FILMviewModel.IsCompleted,
                                    _FILMviewModel.Source
                                    )
                                    , fvm, _FILMviewModel.PostIts, _dataService));
                            }
                            else
                            {

                                fvm.CacheItems.Add(new FilmLOGViewModel(new FilmLOG(
                                    account,
                                    _FILMviewModel.ProjectName,
                                    _FILMviewModel.ApplicationName,
                                    _FILMviewModel.StartDate,
                                    _FILMviewModel.EndDate,
                                    _FILMviewModel.Output,
                                    _FILMviewModel.Type,
                                    posts,
                                    double.Parse(_FILMviewModel.Height),
                                    double.Parse(_FILMviewModel.Width),
                                    _FILMviewModel.Length,
                                    _FILMviewModel.IsCompleted,
                                    _FILMviewModel.Source
                                    )
                                    , fvm, _FILMviewModel.PostIts, _dataService));

                            }

                            fvm.LogCount = fvm.CacheItems.Count.ToString() + " film logs cached | x total logs";

                            _navigationService.NavigateToLogCachePage(CacheContext.Film);

                            break;
                        }
                    case "FLEXI LOG":
                        {
                            flexiCreateViewModel _FLEXIviewModel = (flexiCreateViewModel)_viewModel;
                            FlexiViewModel flexvm = (FlexiViewModel)_dashboard;

                            FlexiNotesLOG.FLEXINOTEType temp;
                            if (_FLEXIviewModel.FlexibleLogCategory == "Document")
                                temp = FlexiNotesLOG.FLEXINOTEType.Document;
                            else if (_FLEXIviewModel.FlexibleLogCategory == "Music")
                                temp = FlexiNotesLOG.FLEXINOTEType.Music;
                            else
                                temp = FlexiNotesLOG.FLEXINOTEType.Gaming;


                            if(_viewModel.StartDate.Equals(_viewModel.EndDate))
                            {
                                flexvm.CacheItems.Add(new FlexiLOGViewModel(new FlexiNotesLOG(
                                    account,
                                    _FLEXIviewModel.ProjectName,
                                    _FLEXIviewModel.ApplicationName,
                                    _FLEXIviewModel.StartDate,
                                    DateTime.Now,
                                    _FLEXIviewModel.Output,
                                    _FLEXIviewModel.Type,
                                    posts,
                                    temp,
                                    FlexiNotesLOG.GAMINGContext.Create,
                                    _FLEXIviewModel.Medium,
                                    _FLEXIviewModel.Format,
                                    int.Parse(_FLEXIviewModel.BitRate),
                                    _FLEXIviewModel.Duration,
                                    _FLEXIviewModel.IsCompleted,
                                    _FLEXIviewModel.Source
                                    ),
                                    flexvm,
                                    _FLEXIviewModel.PostIts, _dataService));
                            }
                            else
                            {
                                flexvm.CacheItems.Add(new FlexiLOGViewModel(new FlexiNotesLOG(
                                    account,
                                    _FLEXIviewModel.ProjectName,
                                    _FLEXIviewModel.ApplicationName,
                                    _FLEXIviewModel.StartDate,
                                    _FLEXIviewModel.EndDate,
                                    _FLEXIviewModel.Output,
                                    _FLEXIviewModel.Type,
                                    posts,
                                    temp,
                                    FlexiNotesLOG.GAMINGContext.Create,
                                    _FLEXIviewModel.Medium,
                                    _FLEXIviewModel.Format,
                                    int.Parse(_FLEXIviewModel.BitRate),
                                    _FLEXIviewModel.Duration,
                                    _FLEXIviewModel.IsCompleted,
                                    _FLEXIviewModel.Source
                                    ),
                                    flexvm,
                                    _FLEXIviewModel.PostIts, _dataService));
                            }

                            flexvm.LogCount = flexvm.CacheItems.Count.ToString() + " flexible logs cached | x total logs";

                            _navigationService.NavigateToLogCachePage(CacheContext.Film);

                            break;
                        }
                }


            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occurred: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
