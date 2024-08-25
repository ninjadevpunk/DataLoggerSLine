using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using MVVMEssentials.ViewModels;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.LoggerCommands
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
        private readonly ApplicationClass QtCreator;
        private const string Android = "Android Studio Hedgehog 2023.1.1";
        private readonly ApplicationClass AndroidStudio;

        // Edit Only
        private readonly ViewModelBase _viewModelBase;

        private ActionType ActionType;


        public CacheContext Type { get; set; }

        public AnnotateCommand(CacheContext type, NavigationService navigationService, LoggerCreateViewModel viewModel, LogCacheViewModel logCacheViewModel, DataService dataService)
        {

            try
            {
                Type = type;
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                ActionType = ActionType.Add;

                if (ActionType == ActionType.Add)
                {
                    QtCreator = _dataService.FindApplicationByID(1);
                    AndroidStudio = _dataService.FindApplicationByID(2);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found in first AnnotateCommand constructor: {e.Message}");
                // TODO
            }


        }

        public AnnotateCommand(ActionType actionType, CacheContext type, NavigationService navigationService, LoggerCreateViewModel viewModel, LogCacheViewModel logCacheViewModel, DataService dataService)
        {


            try
            {
                Type = type;
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                ActionType = actionType;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found in second AnnotateCommand constructor: {e.Message}");

                // TODO
            }


        }

        public AnnotateCommand(ActionType actionType, CacheContext type, NavigationService navigationService, LoggerCreateViewModel viewModel, LogCacheViewModel logCacheViewModel, DataService dataService, ViewModelBase viewModelBase)
        {


            try
            {
                Type = type;
                _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
                _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
                _dashboard = logCacheViewModel ?? throw new ArgumentNullException(nameof(logCacheViewModel));
                _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
                ActionType = actionType;
                _viewModelBase = viewModelBase ?? throw new ArgumentNullException(nameof(viewModelBase));

                if (ActionType == ActionType.Add)
                {
                    QtCreator = _dataService.FindApplicationByID(1);
                    AndroidStudio = _dataService.FindApplicationByID(2);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found in third AnnotateCommand constructor: {e.Message}");
                
                // TODO
            }


        }


        public override void Execute(object parameter)
        {
            try
            {
                var account = _dataService.GetUser();


                List<PostIt> posts = new();
                ApplicationClass application = ActionType == ActionType.Add ? new(_dataService.CreateAppID(_viewModel.Category, account, _viewModel.ApplicationName), account,
                    _viewModel.ApplicationName, _viewModel.Category, false) :
                    new(0, account, _viewModel.ApplicationName, _viewModel.Category, false);

                ProjectClass? project = ActionType == ActionType.Add ? new(_dataService.CreateProjectID(account, application, _viewModel.ProjectName), account,
                    _viewModel.ProjectName, application, _viewModel.Category, false) :
                    new(0, account, _viewModel.ProjectName, application, _viewModel.Category, false);

                OutputClass? output = new(0, account, _viewModel.Output, application, _viewModel.Category);
                TypeClass? type = new(0, account, _viewModel.Type, application, _viewModel.Category);
                SubjectClass subject;
                PostIt postIt;
                bool DateIsWrong = _viewModel.StartDate.Equals(_viewModel.EndDate) || _viewModel.StartDate > _viewModel.EndDate;





                foreach (var item in _viewModel.PostIts)
                {
                    postIt = new();

                    postIt.ID = ActionType == ActionType.Add ? _dataService.CreatePostItID(null) : 0;

                    subject = ActionType == ActionType.Add ? new(_dataService.CreateSubjectID(project, item.Subject, null),
                                    _viewModel.Category,
                                    account,
                                    item.Subject,
                                    project,
                                    application) :

                                    new(0,
                                    _viewModel.Category,
                                    account,
                                    item.Subject,
                                    project,
                                    application);

                    postIt.Subject = subject;

                    postIt.Error = item.Error;
                    postIt.ERCaptureTime = item.DateFound;
                    postIt.Solution = item.Solution;
                    postIt.SOCaptureTime = item.DateSolved;
                    postIt.Suggestion = item.Suggestion;
                    postIt.Comment = item.Comment;

                    posts.Add(postIt);
                }



                if (ActionType == ActionType.Add)
                {

                    switch (Type)
                    {
                        case CacheContext.Qt:
                            {
                                codeCreateViewModel _QtviewModel = (codeCreateViewModel)_viewModel;
                                CodingQtViewModel qtvm = (CodingQtViewModel)_dashboard;
                                project.Application = QtCreator;
                                output.Application = QtCreator;
                                type.Application = QtCreator;

                                foreach (PostIt item in posts)
                                {
                                    item.Subject.Application = QtCreator;
                                    item.Subject.Project.Application = QtCreator;
                                }

                                var list = qtvm.CacheItems;

                                if (DateIsWrong)
                                {

                                    list.Add(new QtLOGViewModel(new CodingLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        QtCreator,
                                        DateTime.Now,
                                        DateTime.Now,
                                        output,
                                        type,
                                        posts,
                                        _QtviewModel.BugsFound,
                                        _QtviewModel.ApplicationOpened
                                        ), qtvm, _QtviewModel.PostIts, _dataService));

                                }
                                else
                                {
                                    list.Add(new QtLOGViewModel(new CodingLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        QtCreator,
                                        _QtviewModel.StartDate,
                                        _QtviewModel.EndDate,
                                        output,
                                        type,
                                        posts,
                                        _QtviewModel.BugsFound,
                                        _QtviewModel.ApplicationOpened
                                        ), qtvm, _QtviewModel.PostIts, _dataService));

                                }

                                qtvm.CacheItems = list;

                                _navigationService.NavigateToLogCachePage(CacheContext.Qt);

                                break;
                            }
                        case CacheContext.AndroidStudio:
                            {
                                AScodeCreateViewModel _ASviewModel = (AScodeCreateViewModel)_viewModel;
                                CodingAndroidViewModel cavm = (CodingAndroidViewModel)_dashboard;
                                project.Application = AndroidStudio;
                                output.Application = AndroidStudio;
                                type.Application = AndroidStudio;

                                foreach (PostIt item in posts)
                                {
                                    item.Subject.Application = AndroidStudio;
                                    item.Subject.Project.Application = AndroidStudio;
                                }

                                AndroidCodingLOG.SCOPE s;
                                s = _ASviewModel.FullORSimple ? AndroidCodingLOG.SCOPE.SIMPLE : AndroidCodingLOG.SCOPE.FULL;

                                var list = cavm.CacheItems;

                                if (DateIsWrong)
                                {
                                    list.Add(new AndroidLOGViewModel(new AndroidCodingLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        AndroidStudio,
                                        DateTime.Now,
                                        DateTime.Now,
                                        output,
                                        type,
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
                                    list.Add(new AndroidLOGViewModel(new AndroidCodingLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        AndroidStudio,
                                        _ASviewModel.StartDate,
                                        _ASviewModel.EndDate,
                                        output,
                                        type,
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

                                cavm.CacheItems = list;

                                _navigationService.NavigateToLogCachePage(CacheContext.AndroidStudio);

                                break;
                            }
                        case CacheContext.Coding:
                            {
                                codeCreateViewModel _GENviewModel = (codeCreateViewModel)_viewModel;
                                CodingViewModel genvm = (CodingViewModel)_dashboard;

                                var list = genvm.CacheItems;

                                if (DateIsWrong)
                                {
                                    list.Add(new CodeLOGViewModel(new CodingLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        application,
                                        DateTime.Now,
                                        DateTime.Now,
                                        output,
                                        type,
                                        posts,
                                        _GENviewModel.BugsFound,
                                        _GENviewModel.ApplicationOpened
                                        ), genvm, _GENviewModel.PostIts, _dataService));

                                }
                                else
                                {
                                    list.Add(new CodeLOGViewModel(new CodingLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        application,
                                        _GENviewModel.StartDate,
                                        _GENviewModel.EndDate,
                                        output,
                                        type,
                                        posts,
                                        _GENviewModel.BugsFound,
                                        _GENviewModel.ApplicationOpened
                                        ), genvm, _GENviewModel.PostIts, _dataService));

                                }

                                genvm.CacheItems = list;

                                _navigationService.NavigateToLogCachePage(CacheContext.Coding);
                                break;
                            }
                        case CacheContext.Graphics:
                            {
                                graphicCreateViewModel _GRAviewModel = (graphicCreateViewModel)_viewModel;
                                GraphicsViewModel gvm = (GraphicsViewModel)_dashboard;

                                var list = gvm.CacheItems;

                                if (DateIsWrong)
                                {
                                    list.Add(new GraphicsLOGViewModel(new GraphicsLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        application,
                                        DateTime.Now,
                                        DateTime.Now,
                                        output,
                                        type,
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
                                    list.Add(new GraphicsLOGViewModel(new GraphicsLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        application,
                                        _GRAviewModel.StartDate,
                                        _GRAviewModel.EndDate,
                                        output,
                                        type,
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

                                gvm.CacheItems = list;

                                _navigationService.NavigateToLogCachePage(CacheContext.Graphics);

                                break;
                            }
                        case CacheContext.Film:
                            {
                                filmCreateViewModel _FILMviewModel = (filmCreateViewModel)_viewModel;
                                FilmViewModel fvm = (FilmViewModel)_dashboard;

                                var list = fvm.CacheItems;

                                if (DateIsWrong)
                                {

                                    list.Add(new FilmLOGViewModel(new FilmLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        application,
                                        DateTime.Now,
                                        DateTime.Now,
                                        output,
                                        type,
                                        posts,
                                        double.Parse(_FILMviewModel.Height),
                                        double.Parse(_FILMviewModel.Width),
                                        _FILMviewModel.Length,
                                        _FILMviewModel.IsCompleted,
                                        _FILMviewModel.Source
                                        ), fvm, _FILMviewModel.PostIts, _dataService));
                                }
                                else
                                {
                                    list.Add(new FilmLOGViewModel(new FilmLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        application,
                                        _FILMviewModel.StartDate,
                                        _FILMviewModel.EndDate,
                                        output,
                                        type,
                                        posts,
                                        double.Parse(_FILMviewModel.Height),
                                        double.Parse(_FILMviewModel.Width),
                                        _FILMviewModel.Length,
                                        _FILMviewModel.IsCompleted,
                                        _FILMviewModel.Source
                                        ), fvm, _FILMviewModel.PostIts, _dataService));

                                }

                                fvm.CacheItems = list;

                                _navigationService.NavigateToLogCachePage(CacheContext.Film);

                                break;
                            }
                        case CacheContext.Flexi:
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

                                var list = flexvm.CacheItems;

                                if (DateIsWrong)
                                {
                                    list.Add(new FlexiLOGViewModel(new FlexiNotesLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        application,
                                        DateTime.Now,
                                        DateTime.Now,
                                        output,
                                        type,
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
                                    list.Add(new FlexiLOGViewModel(new FlexiNotesLOG(
                                        _dataService.CreateLogID(),
                                        account,
                                        project,
                                        application,
                                        _FLEXIviewModel.StartDate,
                                        _FLEXIviewModel.EndDate,
                                        output,
                                        type,
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

                                flexvm.CacheItems = list;

                                _navigationService.NavigateToLogCachePage(CacheContext.Flexi);

                                break;
                            }
                    }

                }
                else
                {

                    int index = -1;


                    switch (Type)
                    {
                        case CacheContext.Qt:
                            {
                                codeEditViewModel _QtviewModel = (codeEditViewModel)_viewModel;
                                CodingQtViewModel qtvm = (CodingQtViewModel)_dashboard;
                                QtLOGViewModel oldLOG = (QtLOGViewModel)_viewModelBase;

                                var oldApp = oldLOG._QtcodingLOG.Application;
                                project.ProjectID = oldLOG._QtcodingLOG.Project.ProjectID;
                                project.Application = oldApp;
                                output.Application = oldApp;
                                type.Application = oldApp;

                                var usedPostItIDs = new List<int>();
                                var usedSubjectIDs = new List<int>();

                                foreach (var item in oldLOG._QtcodingLOG.PostItList)
                                {
                                    usedPostItIDs.Add(item.ID);
                                    if (!usedSubjectIDs.Contains(item.Subject.SubjectID))
                                    {
                                        usedSubjectIDs.Add(item.Subject.SubjectID);
                                    }
                                }

                                foreach (var item in posts)
                                {
                                    item.ID = _dataService.CreatePostItID(usedPostItIDs);

                                    item.Subject.SubjectID = _dataService.CreateSubjectID(project, item.Subject.Subject, usedSubjectIDs);
                                    item.Subject.Application = oldApp;
                                    item.Subject.Project = project;
                                }

                                index = qtvm.CacheItems.IndexOf(oldLOG);

                                if (index != -1)
                                {
                                    var list = qtvm.CacheItems;

                                    QtLOGViewModel newLOG;

                                    if (DateIsWrong)
                                    {
                                        newLOG = new(new CodingLOG(
                                            oldLOG._QtcodingLOG.ID,
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
                                            ), qtvm, _QtviewModel.PostIts, _dataService);
                                    }
                                    else
                                    {
                                        newLOG = new(new CodingLOG(
                                            oldLOG._QtcodingLOG.ID,
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
                                           ), qtvm, _QtviewModel.PostIts, _dataService);
                                    }
                                    oldLOG._timer.Dispose();
                                    newLOG.TimeRemaining = oldLOG.TimeRemaining;
                                    list[index] = newLOG;

                                    qtvm.CacheItems = list;
                                }


                                _navigationService.NavigateToLogCachePage(CacheContext.Qt);

                                break;
                            }
                        case CacheContext.AndroidStudio:
                            {
                                AScodeEditViewModel _ASviewModel = (AScodeEditViewModel)_viewModel;
                                CodingAndroidViewModel cavm = (CodingAndroidViewModel)_dashboard;
                                AndroidLOGViewModel oldLOG = (AndroidLOGViewModel)_viewModelBase;

                                var oldApp = oldLOG._AndroidCodingLOG.Application;
                                project.ProjectID = oldLOG._AndroidCodingLOG.Project.ProjectID;
                                project.Application = oldApp;
                                output.Application = oldApp;
                                type.Application = oldApp;

                                var usedIDs = new List<int>();

                                foreach (var item in oldLOG._AndroidCodingLOG.PostItList)
                                {
                                    usedIDs.Add(item.ID);
                                }

                                foreach (var item in posts)
                                {
                                    item.ID = _dataService.CreatePostItID(usedIDs);

                                    item.Subject.SubjectID = _dataService.CreateSubjectID(project, item.Subject.Subject, usedIDs);
                                    item.Subject.Application = oldApp;
                                    item.Subject.Project = project;
                                }

                                _dataService.SaveSubjectIndex();
                                _dataService.SavePostItIndex();

                                index = cavm.CacheItems.IndexOf(oldLOG);


                                if (index != -1)
                                {
                                    var list = cavm.CacheItems;

                                    AndroidCodingLOG.SCOPE s;
                                    s = _ASviewModel.FullORSimple ? AndroidCodingLOG.SCOPE.SIMPLE : AndroidCodingLOG.SCOPE.FULL;

                                    AndroidLOGViewModel newLOG;

                                    if (DateIsWrong)
                                    {
                                        newLOG = new(new AndroidCodingLOG(
                                            oldLOG._AndroidCodingLOG.ID,
                                            account,
                                            project,
                                            application,
                                            DateTime.Now,
                                            DateTime.Now,
                                            output,
                                            type,
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
                                            oldLOG._AndroidCodingLOG.ID,
                                            account,
                                            project,
                                            application,
                                            _ASviewModel.StartDate,
                                            _ASviewModel.EndDate,
                                            output,
                                            type,
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

                                break;
                            }
                        case CacheContext.Coding:
                            {
                                codeCreateViewModel _GENviewModel = (codeCreateViewModel)_viewModel;
                                CodingViewModel genvm = (CodingViewModel)_dashboard;
                                CodeLOGViewModel oldLOG = (CodeLOGViewModel)_viewModelBase;

                                var oldApp = oldLOG._CodeLOG.Application;
                                application.AppID = oldApp.AppID;
                                project.ProjectID = oldLOG._CodeLOG.Project.ProjectID;
                                project.Application = _GENviewModel.ApplicationName == oldApp.Name ? oldApp : application;

                                var app = project.Application;
                                output.Application = app;
                                type.Application = app;

                                var usedPostItIDs = new List<int>();
                                var usedSubjectIDs = new List<int>();

                                foreach (var item in oldLOG._CodeLOG.PostItList)
                                {
                                    usedPostItIDs.Add(item.ID);
                                    usedSubjectIDs.Add(item.Subject.SubjectID);
                                }

                                foreach (var item in posts)
                                {
                                    item.ID = _dataService.CreatePostItID(usedPostItIDs);

                                    item.Subject.SubjectID = _dataService.CreateSubjectID(project, item.Subject.Subject, usedSubjectIDs);
                                    item.Subject.Application = app;
                                    item.Subject.Project = project;
                                }


                                index = genvm.CacheItems.IndexOf(oldLOG);

                                if (index != -1)
                                {
                                    var list = genvm.CacheItems;

                                    CodeLOGViewModel newLOG;

                                    if (DateIsWrong)
                                    {

                                        newLOG = new(new CodingLOG(
                                            oldLOG._CodeLOG.ID,
                                            account,
                                            project,
                                            application,
                                            DateTime.Now,
                                            DateTime.Now,
                                            output,
                                            type,
                                            posts,
                                            _GENviewModel.BugsFound,
                                            _GENviewModel.ApplicationOpened
                                            ), genvm, _GENviewModel.PostIts, _dataService);

                                    }
                                    else
                                    {
                                        newLOG = new(new CodingLOG(
                                            oldLOG._CodeLOG.ID,
                                            account,
                                            project,
                                            application,
                                            _GENviewModel.StartDate,
                                            _GENviewModel.EndDate,
                                            output,
                                            type,
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

                                _navigationService.NavigateToLogCachePage(CacheContext.Coding);

                                break;
                            }
                        case CacheContext.Graphics:
                            {
                                graphicCreateViewModel _GRAviewModel = (graphicCreateViewModel)_viewModel;
                                GraphicsViewModel gvm = (GraphicsViewModel)_dashboard;
                                GraphicsLOGViewModel oldLOG = (GraphicsLOGViewModel)_viewModelBase;

                                var app = project.Application;
                                output.Application = app;
                                type.Application = app;

                                var usedIDs = new List<int>();

                                foreach (var item in oldLOG._GraphicsLOG.PostItList)
                                {
                                    usedIDs.Add(item.ID);
                                }

                                foreach (var item in posts)
                                {
                                    item.ID = _dataService.CreatePostItID(usedIDs);

                                    item.Subject.SubjectID = _dataService.CreateSubjectID(project, item.Subject.Subject, usedIDs);
                                    item.Subject.Application = app;
                                    item.Subject.Project = project;
                                }

                                _dataService.SaveSubjectIndex();
                                _dataService.SavePostItIndex();

                                index = gvm.CacheItems.IndexOf(oldLOG);

                                if (index != -1)
                                {
                                    var list = gvm.CacheItems;

                                    GraphicsLOGViewModel newLOG;

                                    if (DateIsWrong)
                                    {

                                        newLOG = new(new GraphicsLOG(
                                            oldLOG._GraphicsLOG.ID,
                                            account,
                                            project,
                                            application,
                                            DateTime.Now,
                                            DateTime.Now,
                                            output,
                                            type,
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
                                            oldLOG._GraphicsLOG.ID,
                                            account,
                                            project,
                                            application,
                                            _GRAviewModel.StartDate,
                                            _GRAviewModel.EndDate,
                                            output,
                                            type,
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
                        case CacheContext.Film:
                            {
                                filmCreateViewModel _FILMviewModel = (filmCreateViewModel)_viewModel;
                                FilmViewModel fvm = (FilmViewModel)_dashboard;
                                FilmLOGViewModel oldLOG = (FilmLOGViewModel)_viewModelBase;

                                var app = project.Application;
                                output.Application = app;
                                type.Application = app;

                                var usedIDs = new List<int>();

                                foreach (var item in oldLOG._FilmLOG.PostItList)
                                {
                                    usedIDs.Add(item.ID);
                                }

                                foreach (var item in posts)
                                {
                                    item.ID = _dataService.CreatePostItID(usedIDs);

                                    item.Subject.SubjectID = _dataService.CreateSubjectID(project, item.Subject.Subject, usedIDs);
                                    item.Subject.Application = app;
                                    item.Subject.Project = project;
                                }

                                _dataService.SaveSubjectIndex();
                                _dataService.SavePostItIndex();

                                index = fvm.CacheItems.IndexOf(oldLOG);

                                if (index != -1)
                                {
                                    var list = fvm.CacheItems;

                                    FilmLOGViewModel newLOG;

                                    if (DateIsWrong)
                                    {

                                        newLOG = new(new FilmLOG(
                                            oldLOG._FilmLOG.ID,
                                            account,
                                            project,
                                            application,
                                            DateTime.Now,
                                            DateTime.Now,
                                            output,
                                            type,
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
                                            oldLOG._FilmLOG.ID,
                                            account,
                                            project,
                                            application,
                                            _FILMviewModel.StartDate,
                                            _FILMviewModel.EndDate,
                                            output,
                                            type,
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
                        case CacheContext.Flexi:
                            {
                                flexiCreateViewModel _FLEXIviewModel = (flexiCreateViewModel)_viewModel;
                                FlexiViewModel flexvm = (FlexiViewModel)_dashboard;
                                FlexiLOGViewModel oldLOG = (FlexiLOGViewModel)_viewModelBase;


                                var app = project.Application;
                                output.Application = app;
                                type.Application = app;

                                var usedIDs = new List<int>();

                                foreach (var item in oldLOG._FlexiLOG.PostItList)
                                {
                                    usedIDs.Add(item.ID);
                                }

                                foreach (var item in posts)
                                {
                                    item.ID = _dataService.CreatePostItID(usedIDs);

                                    item.Subject.SubjectID = _dataService.CreateSubjectID(project, item.Subject.Subject, usedIDs);
                                    item.Subject.Application = app;
                                    item.Subject.Project = project;
                                }

                                _dataService.SaveSubjectIndex();
                                _dataService.SavePostItIndex();

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


                                    if (DateIsWrong)
                                    {

                                        newLOG = new(new FlexiNotesLOG(
                                            oldLOG._FlexiLOG.ID,
                                            account,
                                            project,
                                            application,
                                            DateTime.Now,
                                            DateTime.Now,
                                            output,
                                            type,
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
                                            oldLOG._FlexiLOG.ID,
                                            account,
                                            project,
                                            application,
                                            _FLEXIviewModel.StartDate,
                                            _FLEXIviewModel.EndDate,
                                            output,
                                            type,
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


                                _navigationService.NavigateToLogCachePage(CacheContext.Flexi);

                                break;
                            }
                    }
                }
            }
            catch (InvalidCastException castx)
            {
                Debug.WriteLine($"Invalid cast canceled exception found in AnnotateCommand.Execute(): {castx.Message}");
                _navigationService.NavigateToLogCachePage(Type);
            }
            catch (TaskCanceledException taskx)
            {
                Debug.WriteLine($"Task canceled exception found in AnnotateCommand.Execute(): {taskx.Message}");
                _navigationService.NavigateToLogCachePage(Type);
            }
            catch (ArgumentNullException nullx)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Argument null exception found in AnnotateCommand.Execute(): {nullx.Message}");

                _navigationService.NavigateToLogCachePage(Type);
            }
            catch(IndexOutOfRangeException index)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Index out of range exception found in AnnotateCommand.Execute(): {index.Message}");

                _navigationService.NavigateToLogCachePage(Type);
            }
            catch (FormatException formx)
            {
                if(formx.Message.Equals("The input string '' was not in a correct format."))
                {
                    MessageBox.Show($"An error occurred. Please ensure you entered numbers only in fields that require numeric values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }

                Debug.WriteLine($"Format exception found in AnnotateCommand.Execute(): {formx.Message}");

                _navigationService.NavigateToLogCachePage(Type);
            }
            catch (Exception e)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                Debug.WriteLine($"Exception found in AnnotateCommand.Execute(): {e.Message}");

                _navigationService.NavigateToLogCachePage(Type);
            }

        }
    }
}
