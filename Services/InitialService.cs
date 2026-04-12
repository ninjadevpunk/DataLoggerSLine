using Data_Logger_1._3.ViewModels;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Services
{
    public class InitialService
    {
        #region SERVICES


        private readonly IServiceProvider _serviceProvider;
        private readonly AuthService _authService;
        private readonly IDataService _dataService;


        #endregion



        private readonly MainWindowViewModel _mainWindowViewModel;



        #region Loggers



        private readonly codeCreateViewModel _qtLogger;
        private readonly AScodeCreateViewModel _asLogger;
        private readonly codeCreateViewModel _codingLogger;
        private readonly graphicCreateViewModel _graphicsLogger;
        private readonly filmCreateViewModel _filmLogger;
        private readonly flexiCreateViewModel _flexiLogger;




        #endregion




        #region Dashboard 



        private readonly CodingQtViewModel _codingQtDashboard;

        private readonly CodingAndroidViewModel _codingAndroidDashboard;

        private readonly CodingViewModel _codingDashboard;

        private readonly GraphicsViewModel _graphicsDashboard;

        private readonly FilmViewModel _filmDashboard;

        private readonly FlexiViewModel _flexiDashboard;

        private readonly NOTESViewModel _notesDashboard;



        #endregion






        #region Editors





        #endregion







        #region Viewers





        #endregion






        #region Updaters








        #endregion





        /// <summary>
        /// Retrieve the viewmodels and services from DI
        /// </summary>
        /// <param name="serviceProvider">The DI instance</param>
        public InitialService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _authService = serviceProvider.GetRequiredService<AuthService>();
            _dataService = serviceProvider.GetRequiredService<IDataService>();


            _codingQtDashboard = serviceProvider.GetRequiredService<CodingQtViewModel>();
            _codingAndroidDashboard = serviceProvider.GetRequiredService<CodingAndroidViewModel>();
            _codingDashboard = serviceProvider.GetRequiredService<CodingViewModel>();
            _graphicsDashboard = serviceProvider.GetRequiredService<GraphicsViewModel>();
            _filmDashboard = serviceProvider.GetRequiredService<FilmViewModel>();
            _flexiDashboard = serviceProvider.GetRequiredService<FlexiViewModel>();

            _notesDashboard = serviceProvider.GetRequiredService<NOTESViewModel>();

            var viewModelFactory = serviceProvider.GetRequiredService<ViewModelFactory>();

            _qtLogger = viewModelFactory.CreateQtCodeCreateViewModel();
            _asLogger = serviceProvider.GetRequiredService<AScodeCreateViewModel>();
            _codingLogger = viewModelFactory.CreateCodeCreateViewModel();
            _graphicsLogger = serviceProvider.GetRequiredService<graphicCreateViewModel>();
            _filmLogger = serviceProvider.GetRequiredService<filmCreateViewModel>();
            _flexiLogger = serviceProvider.GetRequiredService<flexiCreateViewModel>();

            _mainWindowViewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();
        }

        /// <summary>
        /// Call to start up required operations.
        /// </summary>
        public async Task Initialise(int id)
        {
            await _dataService.InitialiseApplicationsLISTAsync();
            await _dataService.InitialiseProjectsLISTAsync();

            LoadCachedLogs();
            await LoadNotes(id);
            SetupUserProfile();
        }

        private void LoadCachedLogs()
        {
            try
            {
                var qtList = _dataService.RetrieveQtCache(_codingQtDashboard, _dataService);
                var asList = _dataService.RetrieveASCache(_codingAndroidDashboard);
                var cdeList = _dataService.RetrieveCodeCache(_codingDashboard);
                var graList = _dataService.RetrieveGraphicsCache(_graphicsDashboard);
                var flmList = _dataService.RetrieveFilmCache(_filmDashboard);
                var flxList = _dataService.RetrieveFlexibleCache(_flexiDashboard);

                LoadCacheItems(new ObservableCollection<QtLOGViewModel>(qtList), _codingQtDashboard);
                LoadCacheItems(new ObservableCollection<AndroidLOGViewModel>(asList), _codingAndroidDashboard);
                LoadCacheItems(new ObservableCollection<CodeLOGViewModel>(cdeList), _codingDashboard);
                LoadCacheItems(new ObservableCollection<GraphicsLOGViewModel>(graList), _graphicsDashboard);
                LoadCacheItems(new ObservableCollection<FilmLOGViewModel>(flmList), _filmDashboard);
                LoadCacheItems(new ObservableCollection<FlexiLOGViewModel>(flxList), _flexiDashboard);
            }
            catch (Exception ex)
            {
                //
            }
        }


        private void LoadCacheItems(dynamic cacheList, dynamic dashboard)
        {
            if (cacheList is not null && cacheList.Count > 0)
                dashboard.CacheItems = cacheList;
        }

        private async Task LoadNotes(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();

            foreach (var note in await reader.RetrieveGenericNotes(id))
            {
                _notesDashboard.NoteItems.Add(new(_dataService, _notesDashboard, note));
            }

            var hasNotes = _notesDashboard.NoteItems.Count > 0;
            _notesDashboard.StartUpVisibilitySet(hasNotes);
        }

        private void SetupUserProfile()
        {
            if (_authService.Account != null)
                _mainWindowViewModel.SignUpImage =  string.IsNullOrEmpty(_authService.Account.ProfilePic) ? null : BitmapService.LoadImage(_authService.Account.ProfilePic);
        }

    }
}
