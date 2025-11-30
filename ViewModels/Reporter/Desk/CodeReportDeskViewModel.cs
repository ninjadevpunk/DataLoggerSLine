using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Logs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    public class CodeReportDeskViewModel : ReportDeskViewModel
    {
        public override CacheContext Context => CacheContext.Coding;
        private bool _isBusy = false;

        public CodeReportDeskViewModel(NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
        }

        public CodeReportDeskViewModel(NavigationService navigationService, DataService dataService, PDFService pdfService) : base(navigationService, dataService, pdfService)
        {





            SearchCommand = new SearchCommand(this, _dataService, _navigationService, Context);
            ExportCommand = new ExportCommand();
            ReturnToDashboard = new DashboardCommand(_navigationService, Context);

        }

        /// <summary>
        /// Populate the application input bar and project input bar. Show logs based on app and project filter on start up of ReportDesk.
        /// </summary>
        public async Task AutoStartAsync()
        {
            _isBusy = true;

            // INITIALISE APPS
            await _dataService.InitialiseApplicationsLISTAsync(LOG.CATEGORY.CODING);

            Applications.Clear();
            Applications.Add("ANY");

            try
            {
                foreach (ApplicationClass app in _dataService.SQLITE_APPLICATIONS)
                {
                    if (!new[] { 1, 2 }.Contains(app.appID))
                    {
                        Applications.Add(app.Name);

                        if (string.IsNullOrEmpty(Application) && Applications.Count > 0)
                            Application = Applications.ElementAt(1);
                        else if (Applications.Count == 1)
                            Application = Applications.ElementAt(0);
                    }
                }
            }
            catch (InvalidOperationException invex)
            {
                Debug.WriteLine($"InvalidOperationException near AutoStartAsync: {invex.Message}");
            }
            catch (ArgumentException index)
            {
                Debug.WriteLine($"ArgumentException near AutoStartAsync: {index.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near AutoStartAsync: {ex.Message}");
            }


            // Initialise PROJECTS
            await _dataService.InitialiseProjectsLISTAsync(LOG.CATEGORY.CODING);

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Projects.Clear();
                Projects.Add("NONE");
                Projects.Add("ANY");

                try
                {
                    foreach (ProjectClass project in _dataService.SQLITE_PROJECTS)
                    {
                        if (!new[] { 1, 2 }.Contains(project.Application.appID) &&
                            project.Application.Name == Application)
                        {
                            Projects.Add(project.Name);

                            try
                            {
                                if (string.IsNullOrEmpty(Project) && Projects.Count > 0)
                                    Project = Projects.ElementAt(2);
                                else if (Projects.Count == 2)
                                {
                                    Project = Projects.ElementAt(0);
                                }
                            }
                            catch (ArgumentException index)
                            {
                                Debug.WriteLine($"ArgumentException near InitialiseProjectsAsync: {index.Message}");
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Exception near InitialiseProjectsAsync: {ex.Message}");
                            }
                        }
                    }
                }
                catch (InvalidOperationException invex)
                {
                    Debug.WriteLine(
                        $"InvalidOperationException near InitialiseProjectsAsync: {invex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error occurred in InitialiseProjectsAsync()");
                }
            });

            // Update LOGS
            Logs.Clear();
            ObservableCollection<REPORTViewModel> list = new ObservableCollection<REPORTViewModel>();
            int projectID = 1;

            await _dataService.InitialiseProjectsLISTAsync(LOG.CATEGORY.CODING);
            foreach (ProjectClass item in _dataService.SQLITE_PROJECTS)
            {
                if (item.Name == Project && item.Application.Name == Application)
                    projectID = item.projectID;
            }

            foreach (LOG log in await _dataService.RetrieveLogs(CacheContext.Qt))
            {
                if (log.Project.projectID == projectID)
                    list.Add(new qtREPORTViewModel((CodingLOG)log, this, _navigationService, _dataService, _pdfService));
            }

            Logs = list;

            _isBusy = false;
        }

        /// <summary>
        /// Initialises the dashboard's application options to filter with.
        /// </summary>
        public override async Task InitialiseAppsAsync()
        {
            if (_isBusy) return;

            await _dataService.InitialiseApplicationsLISTAsync(LOG.CATEGORY.CODING);

            Applications.Clear();
            Applications.Add("ANY");
            string holdApp = "ANY";

            try
            {
                foreach (ApplicationClass app in _dataService.SQLITE_APPLICATIONS)
                {
                    if (!new[] { 1, 2 }.Contains(app.appID))
                    {
                        Applications.Add(app.Name);

                        if (string.IsNullOrEmpty(Application) && Applications.Count > 0)
                            holdApp = Applications.ElementAt(1);
                    }
                }
            }
            catch (InvalidOperationException invex)
            {
                Debug.WriteLine($"InvalidOperationException near AutoStartAsync: {invex.Message}");
            }
            catch (ArgumentException index)
            {
                Debug.WriteLine($"ArgumentException near AutoStartAsync: {index.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near AutoStartAsync: {ex.Message}");
            }

            Application = holdApp;

        }

        /// <summary>
        /// Initialises the project options for filtering. Used in conjunction with a set application. Call this method whenever the application filter is changed.
        /// </summary>
        public override async Task InitialiseProjectsAsync()
        {
            if (_isBusy) return;

            await _dataService.InitialiseProjectsLISTAsync(LOG.CATEGORY.CODING);

            Projects.Clear();
            Projects.Add("NONE");
            Projects.Add("ANY");
            string holdProjectName = "NONE";

            try
            {
                foreach (ProjectClass project in _dataService.SQLITE_PROJECTS)
                {
                    if (!new[] { 1, 2 }.Contains(project.Application.appID) &&
                        project.Application.Name == Application)
                    {
                        Projects.Add(project.Name);

                        try
                        {
                            if (string.IsNullOrEmpty(Project) && Projects.Count > 0)
                                holdProjectName = Projects.ElementAt(2);
                        }
                        catch (ArgumentException index)
                        {
                            Debug.WriteLine($"ArgumentException near InitialiseProjectsAsync: {index.Message}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Exception near InitialiseProjectsAsync: {ex.Message}");
                        }
                    }
                }
            }
            catch (InvalidOperationException invex)
            {
                Debug.WriteLine(
                    $"InvalidOperationException near InitialiseProjectsAsync: {invex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred in InitialiseProjectsAsync()");
            }

            Project = holdProjectName;





        }

        /// <summary>
        /// Updates logs in the Qt Report Desk.
        /// </summary>
        public override async Task UpdateLogsAsync()
        {
            if (_isBusy) return;
            _isBusy = true;

            Logs.Clear();
            ObservableCollection<REPORTViewModel> list = new ObservableCollection<REPORTViewModel>();
            int projectID = 1;

            await _dataService.InitialiseProjectsLISTAsync(LOG.CATEGORY.CODING);
            foreach (ProjectClass item in _dataService.SQLITE_PROJECTS)
            {
                if (item.Name == Project && item.Application.Name == Application)
                    projectID = item.projectID;
            }

            foreach (LOG log in await _dataService.RetrieveLogs(CacheContext.Coding))
            {
                //if (log.Project.projectID == projectID)
                //    list.Add(new codeREPORTViewModel((CodingLOG)log, this, _navigationService, _dataService, _pdfService));
            }

            Logs = list;
            _isBusy = false;
        }

    }
}
