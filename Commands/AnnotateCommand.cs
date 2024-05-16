using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using MVVMEssentials.ViewModels;
using System;
using System.Windows;

namespace Data_Logger_1._3.Commands
{
    public enum ActionType
    {
        Add,
        Edit
    }

    public class AnnotateCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly DataService _dataService;
        private readonly LoggerCreateViewModel _viewModel;
        private readonly LogCacheViewModel _dashboard;

        private const string Qt = "Qt Creator";
        private const string Android = "Android Studio Hedgehog 2023.1.1";

        // Edit Only
        private readonly ViewModelBase _viewModelBase;

        private ActionType _actionType;


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
                _actionType = ActionType.Add;
            }
            catch (Exception)
            {
                // TODO
            }


        }

        public AnnotateCommand(ActionType actionType, string TYPE, NavigationService navigationService, LoggerCreateViewModel viewModel, LogCacheViewModel logCacheViewModel, DataService dataService)
        {


            try
            {
                _TYPE = TYPE;
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _actionType = actionType;
            }
            catch (Exception)
            {
                // TODO
            }


        }

        public AnnotateCommand(ActionType actionType, string TYPE, NavigationService navigationService, LoggerCreateViewModel viewModel, LogCacheViewModel logCacheViewModel, DataService dataService, ViewModelBase viewModelBase)
        {


            try
            {
                _TYPE = TYPE;
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                _actionType = actionType;
                _viewModelBase = viewModelBase ?? throw new ArgumentNullException(nameof(viewModelBase));
            }
            catch (Exception)
            {
                // TODO
            }


        }


        public override void Execute(object parameter)
        {
            try
            {
                var account = _dataService.GetUser();


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


                if (_actionType == ActionType.Add)
                {

                    switch (_TYPE)
                    {
                        case "CODING LOG":
                            {
                                if (_viewModel.ApplicationName == Android)
                                {
                                    AScodeCreateViewModel _ASviewModel = (AScodeCreateViewModel)_viewModel;
                                    CodingAndroidViewModel cavm = (CodingAndroidViewModel)_dashboard;

                                    AndroidCodingLOG.SCOPE s;
                                    s = _ASviewModel.FullORSimple ? AndroidCodingLOG.SCOPE.SIMPLE : AndroidCodingLOG.SCOPE.FULL;


                                    if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                                    {
                                        var list = cavm.CacheItems;

                                        list.Add(new AndroidLOGViewModel(new AndroidCodingLOG(
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

                                        cavm.CacheItems = list;
                                    }
                                    else
                                    {
                                        var list = cavm.CacheItems;

                                        list.Add(new AndroidLOGViewModel(new AndroidCodingLOG(
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

                                        cavm.CacheItems = list;
                                    }


                                    _navigationService.NavigateToLogCachePage(CacheContext.AndroidStudio);

                                }
                                else if (_viewModel.ApplicationName == Qt)
                                {
                                    codeCreateViewModel _QtviewModel = (codeCreateViewModel)_viewModel;
                                    CodingQtViewModel qtvm = (CodingQtViewModel)_dashboard;

                                    if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                                    {
                                        var list = qtvm.CacheItems;

                                        list.Add(new QtLOGViewModel(new CodingLOG(
                                            account,
                                            _QtviewModel.ProjectName,
                                            Qt,
                                            _QtviewModel.StartDate,
                                            DateTime.Now,
                                            _QtviewModel.Output,
                                            _QtviewModel.Type,
                                            posts,
                                            _QtviewModel.BugsFound,
                                            _QtviewModel.ApplicationOpened
                                            ), qtvm, _QtviewModel.PostIts, _dataService));

                                        qtvm.CacheItems = list;
                                    }
                                    else
                                    {
                                        var list = qtvm.CacheItems;

                                        list.Add(new QtLOGViewModel(new CodingLOG(
                                            account,
                                            _QtviewModel.ProjectName,
                                            Qt,
                                            _QtviewModel.StartDate,
                                            _QtviewModel.EndDate,
                                            _QtviewModel.Output,
                                            _QtviewModel.Type,
                                            posts,
                                            _QtviewModel.BugsFound,
                                            _QtviewModel.ApplicationOpened
                                            ), qtvm, _QtviewModel.PostIts, _dataService));

                                        qtvm.CacheItems = list;
                                    }

                                    _navigationService.NavigateToLogCachePage(CacheContext.Qt);

                                }
                                else
                                {
                                    codeCreateViewModel _GENviewModel = (codeCreateViewModel)_viewModel;
                                    CodingGenericViewModel genvm = (CodingGenericViewModel)_dashboard;

                                    if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                                    {
                                        var list = genvm.CacheItems;

                                        list.Add(new CodeLOGViewModel(new CodingLOG(
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

                                        genvm.CacheItems = list;
                                    }
                                    else
                                    {
                                        var list = genvm.CacheItems;

                                        list.Add(new CodeLOGViewModel(new CodingLOG(
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

                                        genvm.CacheItems = list;
                                    }

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

                                    var list = gvm.CacheItems;

                                    list.Add(new GraphicsLOGViewModel(new GraphicsLOG(
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

                                    gvm.CacheItems = list;
                                }
                                else
                                {

                                    var list = gvm.CacheItems;

                                    list.Add(new GraphicsLOGViewModel(new GraphicsLOG(
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

                                    gvm.CacheItems = list;

                                }

                                _navigationService.NavigateToLogCachePage(CacheContext.Graphics);

                                break;
                            }
                        case "FILM LOG":
                            {
                                filmCreateViewModel _FILMviewModel = (filmCreateViewModel)_viewModel;
                                FilmViewModel fvm = (FilmViewModel)_dashboard;

                                if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                                {
                                    var list = fvm.CacheItems;

                                    list.Add(new FilmLOGViewModel(new FilmLOG(
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

                                    fvm.CacheItems = list;
                                }
                                else
                                {

                                    var list = fvm.CacheItems;

                                    list.Add(new FilmLOGViewModel(new FilmLOG(
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

                                    fvm.CacheItems = list;

                                }

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

                                // TODO
                                // FlexiNotesLOG.GAMINGContext context;


                                if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                                {
                                    var list = flexvm.CacheItems;

                                    list.Add(new FlexiLOGViewModel(new FlexiNotesLOG(
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

                                    flexvm.CacheItems = list;
                                }
                                else
                                {
                                    var list = flexvm.CacheItems;

                                    list.Add(new FlexiLOGViewModel(new FlexiNotesLOG(
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

                                    flexvm.CacheItems = list;
                                }


                                _navigationService.NavigateToLogCachePage(CacheContext.Film);

                                break;
                            }
                    }


                }
                else
                {

                    int index = -1;


                    switch (_TYPE)
                    {
                        case "CODING LOG":
                            {
                                if (_viewModel.ApplicationName == Android)
                                {
                                    AScodeEditViewModel _ASviewModel = (AScodeEditViewModel)_viewModel;
                                    CodingAndroidViewModel cavm = (CodingAndroidViewModel)_dashboard;
                                    AndroidLOGViewModel oldLOG = (AndroidLOGViewModel)_viewModelBase;

                                    index = cavm.CacheItems.IndexOf(oldLOG);


                                    if (index != -1)
                                    {
                                        var list = cavm.CacheItems;

                                        AndroidCodingLOG.SCOPE s;
                                        s = _ASviewModel.FullORSimple ? AndroidCodingLOG.SCOPE.SIMPLE : AndroidCodingLOG.SCOPE.FULL;

                                        AndroidLOGViewModel newLOG;

                                        if (_viewModel.StartDate.Equals(_viewModel.EndDate) || _viewModel.StartDate > _viewModel.EndDate)
                                        {
                                            newLOG = new(new AndroidCodingLOG(
                                                account,
                                                _ASviewModel.ProjectName,
                                                _ASviewModel.ApplicationName,
                                                DateTime.Now,
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
                                                ), cavm, _ASviewModel.PostIts, _dataService);
                                        }
                                        else
                                        {
                                            newLOG = new(new AndroidCodingLOG(
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
                                                ), cavm, _ASviewModel.PostIts, _dataService);
                                        }

                                        oldLOG._timer.Dispose();
                                        newLOG.TimeRemaining = oldLOG.TimeRemaining;
                                        list[index] = newLOG;

                                        cavm.CacheItems = list;
                                    }                                    


                                    _navigationService.NavigateToLogCachePage(CacheContext.AndroidStudio);

                                }
                                else if (_viewModel.ApplicationName == Qt)
                                {
                                    codeCreateViewModel _QtviewModel = (codeCreateViewModel)_viewModel;
                                    CodingQtViewModel qtvm = (CodingQtViewModel)_dashboard;
                                    QtLOGViewModel oldLOG = (QtLOGViewModel)_viewModelBase;

                                    index = qtvm.CacheItems.IndexOf(oldLOG);

                                    
                                    if(index != -1)
                                    {
                                        var list = qtvm.CacheItems;

                                        QtLOGViewModel newLOG;

                                        if (_viewModel.StartDate.Equals(_viewModel.EndDate) || _viewModel.StartDate > _viewModel.EndDate)
                                        {
                                            newLOG = new(new CodingLOG(
                                                account,
                                                _QtviewModel.ProjectName,
                                                Qt,
                                                DateTime.Now,
                                                DateTime.Now,
                                                _QtviewModel.Output,
                                                _QtviewModel.Type,
                                                posts,
                                                _QtviewModel.BugsFound,
                                                _QtviewModel.ApplicationOpened
                                                ), qtvm, _QtviewModel.PostIts, _dataService);
                                        }
                                        else
                                        {
                                            newLOG = new(new CodingLOG(
                                               account,
                                               _QtviewModel.ProjectName,
                                               Qt,
                                               _QtviewModel.StartDate,
                                               _QtviewModel.EndDate,
                                               _QtviewModel.Output,
                                               _QtviewModel.Type,
                                               posts,
                                               _QtviewModel.BugsFound,
                                               _QtviewModel.ApplicationOpened
                                               ), qtvm, _QtviewModel.PostIts, _dataService);
                                        }
                                        oldLOG._timer.Dispose();
                                        newLOG.TimeRemaining = oldLOG.TimeRemaining;
                                        list[index] = newLOG;

                                        qtvm.CacheItems = list;
                                    }
                                    

                                    _navigationService.NavigateToLogCachePage(CacheContext.Qt);

                                }
                                else
                                {
                                    codeCreateViewModel _GENviewModel = (codeCreateViewModel)_viewModel;
                                    CodingGenericViewModel genvm = (CodingGenericViewModel)_dashboard;
                                    CodeLOGViewModel oldLOG = (CodeLOGViewModel)_viewModelBase;

                                    index = genvm.CacheItems.IndexOf(oldLOG);

                                    if(index != -1)
                                    {
                                        var list = genvm.CacheItems;

                                        CodeLOGViewModel newLOG;

                                        if (_viewModel.StartDate.Equals(_viewModel.EndDate) || _viewModel.StartDate > _viewModel.EndDate)
                                        {

                                            newLOG = new(new CodingLOG(
                                                account,
                                                _GENviewModel.ProjectName,
                                                _GENviewModel.ApplicationName,
                                                DateTime.Now,
                                                DateTime.Now,
                                                _GENviewModel.Output,
                                                _GENviewModel.Type,
                                                posts,
                                                _GENviewModel.BugsFound,
                                                _GENviewModel.ApplicationOpened
                                                ), genvm, _GENviewModel.PostIts, _dataService);

                                        }
                                        else
                                        {
                                            newLOG = new(new CodingLOG(
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
                                                ), genvm, _GENviewModel.PostIts, _dataService);
                                        }
                                        oldLOG._timer.Dispose();
                                        newLOG.TimeRemaining = oldLOG.TimeRemaining;
                                        list[index] = newLOG;

                                        genvm.CacheItems = list;
                                    }

                                    _navigationService.NavigateToLogCachePage(CacheContext.Generic);

                                }
                                break;
                            }
                        case "GRAPHICS LOG":
                            {
                                graphicCreateViewModel _GRAviewModel = (graphicCreateViewModel)_viewModel;
                                GraphicsViewModel gvm = (GraphicsViewModel)_dashboard;
                                GraphicsLOGViewModel oldLOG = (GraphicsLOGViewModel)_viewModelBase;

                                index = gvm.CacheItems.IndexOf(oldLOG);

                                if (index != -1)
                                {
                                    var list = gvm.CacheItems;

                                    GraphicsLOGViewModel newLOG;

                                    if (_viewModel.StartDate.Equals(_viewModel.EndDate) || _viewModel.StartDate > _viewModel.EndDate)
                                    {

                                        newLOG = new(new GraphicsLOG(
                                            account,
                                            _GRAviewModel.ProjectName,
                                            _GRAviewModel.ApplicationName,
                                            DateTime.Now,
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
                                            ), gvm, _GRAviewModel.PostIts, _dataService);

                                    }
                                    else
                                    {
                                        newLOG = new(new GraphicsLOG(
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
                                            ), gvm, _GRAviewModel.PostIts, _dataService);
                                    }
                                    oldLOG._timer.Dispose();
                                    newLOG.TimeRemaining = oldLOG.TimeRemaining;
                                    list[index] = newLOG;

                                    gvm.CacheItems = list;
                                }

                                _navigationService.NavigateToLogCachePage(CacheContext.Graphics);

                                break;
                            }
                        case "FILM LOG":
                            {
                                filmCreateViewModel _FILMviewModel = (filmCreateViewModel)_viewModel;
                                FilmViewModel fvm = (FilmViewModel)_dashboard;
                                FilmLOGViewModel oldLOG = (FilmLOGViewModel)_viewModelBase;

                                index = fvm.CacheItems.IndexOf(oldLOG);

                                if (index != -1)
                                {
                                    var list = fvm.CacheItems;

                                    FilmLOGViewModel newLOG;

                                    if (_viewModel.StartDate.Equals(_viewModel.EndDate) || _viewModel.StartDate > _viewModel.EndDate)
                                    {

                                        newLOG = new(new FilmLOG(
                                            account,
                                            _FILMviewModel.ProjectName,
                                            _FILMviewModel.ApplicationName,
                                            DateTime.Now,
                                            DateTime.Now,
                                            _FILMviewModel.Output,
                                            _FILMviewModel.Type,
                                            posts,
                                            double.Parse(_FILMviewModel.Height),
                                            double.Parse(_FILMviewModel.Width),
                                            _FILMviewModel.Length,
                                            _FILMviewModel.IsCompleted,
                                            _FILMviewModel.Source
                                            ), fvm, _FILMviewModel.PostIts, _dataService);

                                    }
                                    else
                                    {
                                        newLOG = new(new FilmLOG(
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
                                            ), fvm, _FILMviewModel.PostIts, _dataService);
                                    }
                                    oldLOG._timer.Dispose();
                                    newLOG.TimeRemaining = oldLOG.TimeRemaining;
                                    list[index] = newLOG;

                                    fvm.CacheItems = list;
                                }

                                _navigationService.NavigateToLogCachePage(CacheContext.Film);

                                break;
                            }
                        case "FLEXI LOG":
                            {
                                flexiCreateViewModel _FLEXIviewModel = (flexiCreateViewModel)_viewModel;
                                FlexiViewModel flexvm = (FlexiViewModel)_dashboard;
                                FlexiLOGViewModel oldLOG = (FlexiLOGViewModel)_viewModelBase;

                                index = flexvm.CacheItems.IndexOf(oldLOG);

                                if (index != -1)
                                {
                                    var list = flexvm.CacheItems;

                                    FlexiLOGViewModel newLOG;

                                    FlexiNotesLOG.FLEXINOTEType temp;
                                    if (_FLEXIviewModel.FlexibleLogCategory == "Document")
                                        temp = FlexiNotesLOG.FLEXINOTEType.Document;
                                    else if (_FLEXIviewModel.FlexibleLogCategory == "Music")
                                        temp = FlexiNotesLOG.FLEXINOTEType.Music;
                                    else
                                        temp = FlexiNotesLOG.FLEXINOTEType.Gaming;

                                    // TODO
                                    // FlexiNotesLOG.GAMINGContext context;


                                    if (_viewModel.StartDate.Equals(_viewModel.EndDate))
                                    {

                                        newLOG = new(new FlexiNotesLOG(
                                            account,
                                            _FLEXIviewModel.ProjectName,
                                            _FLEXIviewModel.ApplicationName,
                                            DateTime.Now,
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
                                            _FLEXIviewModel.PostIts, _dataService);
                                    }
                                    else
                                    {

                                        newLOG = new FlexiLOGViewModel(new FlexiNotesLOG(
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
                                            _FLEXIviewModel.PostIts, _dataService);
                                    }
                                    oldLOG._timer.Dispose();
                                    newLOG.TimeRemaining = oldLOG.TimeRemaining;
                                    list[index] = newLOG;

                                    flexvm.CacheItems = list;
                                }


                                _navigationService.NavigateToLogCachePage(CacheContext.Film);

                                break;
                            }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occurred: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                _navigationService.NavigateToLogCachePage(CacheContext.Generic);
            }

        }
    }
}
