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


        private readonly AuthService _authService;
        private readonly DataService _dataService;
        private readonly Cachemaster _cacheMaster;
        private readonly ENTITYREADER _reader;
        private readonly ENTITYWRITER _writer;
        private readonly ENTITYHANDLER _handler;


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
            _authService = serviceProvider.GetRequiredService<AuthService>();
            _dataService = serviceProvider.GetRequiredService<DataService>();
            _cacheMaster = serviceProvider.GetRequiredService<Cachemaster>();
            _reader = serviceProvider.GetRequiredService<ENTITYREADER>();
            _writer = serviceProvider.GetRequiredService<ENTITYWRITER>();
            _handler = serviceProvider.GetRequiredService<ENTITYHANDLER>();

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
        public void Initialise()
        {
            if (_dataService == null)
            {
                MessageBox.Show("An error occurred on our end. We apologize for any inconvenience.", "Error");
                Debug.WriteLine("DataService is null!");
                return;
            }

            _dataService.InitialiseApplicationsLISTAsync();
            _dataService.InitialiseProjectsLISTAsync();

            LoadCachedLogs();
            SetupUserProfile();

        }

        private void LoadCachedLogs()
        {
            var qtList = _dataService.RetrieveQtCache(_codingQtDashboard, _dataService);
            var asList = _dataService.RetrieveASCache(_codingAndroidDashboard);
            var cdeList = _dataService.RetrieveCodeCache(_codingDashboard);
            var graList = _dataService.RetrieveGraphicsCache(_graphicsDashboard);
            var flmList = _dataService.RetrieveFilmCache(_filmDashboard);
            var flxList = _dataService.RetrieveFlexibleCache(_flexiDashboard);

            LoadCacheItems(new ObservableCollection<LOGViewModel>(qtList),  _codingQtDashboard);
            LoadCacheItems(new ObservableCollection<LOGViewModel>(asList), _codingAndroidDashboard);
            LoadCacheItems(new ObservableCollection<LOGViewModel>(cdeList), _codingDashboard);
            LoadCacheItems(new ObservableCollection<LOGViewModel>(graList), _graphicsDashboard);
            LoadCacheItems(new ObservableCollection<LOGViewModel>(flmList), _filmDashboard);
            LoadCacheItems(new ObservableCollection<LOGViewModel>(flxList), _flexiDashboard);

        }

        private void LoadCacheItems(ObservableCollection<LOGViewModel> cacheList, dynamic dashboard)
        {
            if (cacheList is not null && cacheList.Count > 0)
                dashboard.CacheItems = cacheList;
        }

        private void SetupUserProfile()
        {
            string displayPic = "/Assets/login/user.png";

            _mainWindowViewModel.SignUpImage = _authService.Account.ProfilePic;

            if (_mainWindowViewModel.SignUpImage == displayPic)
                _mainWindowViewModel.SignUpImage = string.Empty;
        }

    }
}
