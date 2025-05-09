using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using MVVMEssentials.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Searches for the query inside the database.
    /// </summary>
    public class SearchCommand : CommandBase
    {
        private readonly ReportDeskViewModel _reportDesk;
        private readonly DataService _dataService;
        private readonly NavigationService _navigationService;

        public CacheContext Context { get; set; } = CacheContext.Coding;

        public SearchCommand(ReportDeskViewModel reportDesk, DataService dataService, NavigationService navigationService, CacheContext cacheContext)
        {
            _reportDesk = reportDesk ?? throw new ArgumentNullException(nameof(reportDesk));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            Context = cacheContext;
        }

        public SearchCommand()
        {
        }

        public override void Execute(object parameter)
        {
            try
            {
                ProjectClass projectFilter = null;

                _dataService.InitialiseProjectsLISTAsync();
                foreach(ProjectClass project in _dataService.SQLITE_PROJECTS)
                {
                    if(_reportDesk.Project == project.Name)
                    {
                        projectFilter = project;
                        break;
                    }
                }

                switch(Context)
                {
                    case CacheContext.Qt:
                        {
                            var list = projectFilter is null ? 
                                throw new ArgumentNullException("Project not found.") : 
                                _dataService.SearchForQtRecords(_reportDesk.Query, projectFilter.projectID);

                            var items = new ObservableCollection<SearchResultViewModel>();

                            foreach (var record in list)
                            {
                                items.Add(new qt_SearchResultViewModel(record, _reportDesk, _navigationService));
                            }

                            _reportDesk.SearchBarItems = items;

                            break;
                        }
                    case CacheContext.AndroidStudio:
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
                    case CacheContext.Flexi:
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
            
            }
            catch (ArgumentNullException nullex)
            {
                Debug.WriteLine($"ArgumentNullException occurred near SearchCommand.Execute(): {nullex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near SearchCommand.Execute(): {ex.Message}");
            }
        }
    }

}
