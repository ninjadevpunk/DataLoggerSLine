using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Logs;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Data_Logger_1._3.ViewModels.Reporter.Desk
{
    /// <summary>
    /// Dashboard for Qt Reporter.
    /// </summary>
    public class QtReportDeskViewModel : ReportDeskViewModel
    {
        public override CacheContext Context => CacheContext.Qt;

        /// <summary>
        /// For database CRUD operations only.
        /// </summary>
        /// <param name="navigationService">The service for navigating in adn out of the Qt Report desk.</param>
        /// <param name="dataService">The service required to perform database operations.</param>
        public QtReportDeskViewModel(NavigationService navigationService, DataService dataService) : base(navigationService, dataService)
        {
            _dataService.InitialiseProjectsLIST(LOG.CATEGORY.CODING);

            foreach(ProjectClass project in _dataService.SQLITE_PROJECTS)
            {
                if(project.Application.AppID == 1)
                {
                    Projects.Add(project.Name);

                    try
                    {
                        if (string.IsNullOrEmpty(Project))
                        {
                            Project = Projects.ElementAt(0);
                        }
                    }
                    catch(InvalidOperationException invex)
                    {
                        Debug.WriteLine($"InvalidOperationException near QtReportDeskViewModel constructor: {invex.Message}");
                    }
                    catch (ArgumentException index)
                    {
                        Debug.WriteLine($"ArgumentException near QtReportDeskViewModel constructor: {index.Message}");
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine($"Exception near QtReportDeskViewModel constructor: {ex.Message}");
                    }
                }
            }

            Application = "Qt Creator";
            ApplicationEnabled = false;
            ApplicationToolTip = "You CANNOT edit this field at this stage for only Qt logs are shown on this page.";
            Applications.Add("Qt Creator");


            Search = new SearchCommand(this, _dataService, _navigationService, Context);
            Export = new ExportCommand();
            ReturnToDashboard = new DashboardCommand(_navigationService, Context);

            UpdateLogs();

            AwaitCall = false;
        }

        /// <summary>
        /// For PDF generation and database CRUD operations.
        /// </summary>
        /// <param name="navigationService">The service for navigating in adn out of the Qt Report desk.</param>
        /// <param name="dataService">The service required to perform database operations.</param>
        /// <param name="pdfService">The service for generating PDFs.</param>
        public QtReportDeskViewModel(NavigationService navigationService, DataService dataService, PDFService pdfService) : base(navigationService, dataService, pdfService)
        {
            _dataService.InitialiseProjectsLIST(LOG.CATEGORY.CODING);

            foreach (ProjectClass project in _dataService.SQLITE_PROJECTS)
            {
                if (project.Application.AppID == 1)
                {
                    Projects.Add(project.Name);

                    try
                    {
                        if (string.IsNullOrEmpty(Project))
                        {
                            Project = Projects.ElementAt(0);
                        }
                    }
                    catch (InvalidOperationException invex)
                    {
                        Debug.WriteLine($"InvalidOperationException near QtReportDeskViewModel constructor: {invex.Message}");
                    }
                    catch (ArgumentException index)
                    {
                        Debug.WriteLine($"ArgumentException near QtReportDeskViewModel constructor: {index.Message}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Exception near QtReportDeskViewModel constructor: {ex.Message}");
                    }
                }
            }

            Application = "Qt Creator";
            ApplicationEnabled = false;
            ApplicationToolTip = "You CANNOT edit this field at this stage for only Qt logs are shown on this page.";
            Applications.Add("Qt Creator");


            Search = new SearchCommand(this, _dataService, _navigationService, Context);
            Export = new ExportCommand();
            ReturnToDashboard = new DashboardCommand(_navigationService, Context);

            UpdateLogs();

            AwaitCall = false;
        }

        /// <summary>
        /// Updates logs in the Qt Report Desk.
        /// </summary>
        /// <param name="project">The Qt project you want logs from.</param>

        public override void UpdateLogs(string project)
        {
            Logs.Clear();
            ObservableCollection<REPORTViewModel> list = new ObservableCollection<REPORTViewModel>();
            int projectID = 1;

            _dataService.InitialiseProjectsLIST(LOG.CATEGORY.CODING);
            foreach(ProjectClass item in _dataService.SQLITE_PROJECTS)
            {
                if(item.Name == project)
                    projectID = item.ProjectID;
            }

            foreach (LOG log in _dataService.RetrieveLogs(LOG.CATEGORY.CODING))
            {
                if(log.Project.ProjectID == projectID)
                    list.Add(new qtREPORTViewModel((CodingLOG)log, _navigationService, _dataService, _pdfService));
            }

            Logs = list;
        }

        public void UpdateLogs()
        {
            ObservableCollection<REPORTViewModel> list = new ObservableCollection<REPORTViewModel>();

            foreach (LOG log in _dataService.RetrieveLogs(LOG.CATEGORY.CODING))
            {
                if(Project == log.Project.Name)
                    list.Add(new qtREPORTViewModel((CodingLOG)log, _navigationService, _dataService, _pdfService));
            }

            Logs = list;
        }
    }
}
