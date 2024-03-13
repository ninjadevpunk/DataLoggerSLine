using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Security;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    public class AnnotateCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly LoggerCreateViewModel _viewModel;
        private readonly LogCacheViewModel _dashboard;


        public string _TYPE { get; set; }

        public AnnotateCommand(string TYPE, NavigationService navigationService, LoggerCreateViewModel viewModel, LogCacheViewModel logCacheViewModel)
        {


            try
            {
                _TYPE = TYPE;
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));

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
                                        ), cavm));
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
                                        ), cavm));
                                }
                                
                                
                                cavm.LogCount = cavm.CacheItems.Count.ToString() + " logs cached | x total logs";

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
                                        ), qtvm));
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
                                        ), qtvm));
                                }

                                qtvm.LogCount = qtvm.CacheItems.Count.ToString() + " logs cached | x total logs";

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
                                        ), genvm));
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
                                        ), genvm));
                                }

                                genvm.LogCount = genvm.CacheItems.Count.ToString() + " logs cached | x total logs";

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
                                    ), gvm));
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
                                    ), gvm));

                            }

                            gvm.LogCount = gvm.CacheItems.Count.ToString() + " logs cached | x total logs";

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
                                    , fvm));
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
                                    , fvm));

                            }

                            fvm.LogCount = fvm.CacheItems.Count.ToString() + " logs cached | x total logs";

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
