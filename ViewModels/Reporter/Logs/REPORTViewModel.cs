using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.ViewModels.Reporter
{
    /// <summary>
    /// The base class for all REPORT items to be shown in a ReportDeskViewModel.
    /// </summary>
    public abstract class REPORTViewModel : ViewModelBase
    {
        protected readonly LOG _LOG;
        public abstract CacheContext Context { get; }
        protected NavigationService _navigationService;

        public REPORTViewModel(LOG log, NavigationService navigationService, DataService dataService)
        {
            _LOG = log;
            _navigationService = navigationService;
        }



        #region Properties




        public int ViewModelID => _LOG.ID;

        public string ProjectName => $"{_LOG.Project.Name} ({_LOG.Application.Name})";

        public string ErrorCount => _LOG.ErrorCount().ToString();

        public string SolutionCount => _LOG.SolutionCount().ToString();

        public string SuggestionCount => _LOG.SuggestionCount().ToString();

        public string CommentCount => _LOG.CommentCount().ToString();


        public string StartEndDate => $"{_LOG.Start.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")} - " +
            $"{_LOG.End.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")}";

        /** Store the first occurence of a note with acceptable input only. **/
        public string NotaryContent => _LOG.Content;





        public ICommand SingleExport { get; set; }

        public ICommand EditLogCommand { get; set; }

        public ICommand ViewLogCommand { get; set; }

        public ICommand DeleteLogCommand { get; set; }







        #endregion






        #region Member Functions



        public static string EF_PostItContent(ObservableCollection<EF_PostItViewModel> efPostItViewModel)
        {
            string pattern = @"\S";
            Regex regex = new Regex(pattern);

            foreach (EF_PostItViewModel postItViewModel in efPostItViewModel)
            {
                if (!string.IsNullOrWhiteSpace(postItViewModel.Display_Error) && regex.IsMatch(postItViewModel.Display_Error))
                    return postItViewModel.Display_Error;
                if (!string.IsNullOrWhiteSpace(postItViewModel.Display_Solution) && regex.IsMatch(postItViewModel.Display_Solution))
                    return postItViewModel.Display_Solution;
                if (!string.IsNullOrWhiteSpace(postItViewModel.Display_Suggestion) && regex.IsMatch(postItViewModel.Display_Suggestion))
                    return postItViewModel.Display_Suggestion;
                if (!string.IsNullOrWhiteSpace(postItViewModel.Display_Comment) && regex.IsMatch(postItViewModel.Display_Comment))
                    return postItViewModel.Display_Comment;
            }

            return "No Notes";
        }


        #endregion
    }
}
