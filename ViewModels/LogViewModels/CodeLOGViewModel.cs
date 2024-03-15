using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using MVVMEssentials.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class CodeLOGViewModel : ViewModelBase
    {
        public CodingLOG _CodeLOG;
        private readonly LogCacheViewModel _vm;
        public bool IsDisposed { get; set; } = false;

        private Timer _timer;

        public ICommand DeleteCacheItemCommand { get; set; }


        #region Constructors



        public CodeLOGViewModel(CodingLOG codingLOG, LogCacheViewModel logCacheViewModel)
        {
            _vm = logCacheViewModel;

            _CodeLOG = codingLOG;

            TimeRemaining = 1200;
            StartCountdown();

            DeleteCacheItemCommand = new DeleteCodingCacheItemCommand(_vm);
        }




        #endregion



        #region Properties




        public string ProjectName => $"{_CodeLOG.ProjectName} ({_CodeLOG.ApplicationName})";

        public string ErrorCount => _CodeLOG.errorCount().ToString();

        public string SolutionCount => _CodeLOG.solutionCount().ToString();

        public string SuggestionCount => _CodeLOG.suggestionCount().ToString();

        public string CommentCount => _CodeLOG.commentCount().ToString();

        /** Save the start and end time here **/
        public string StartEndDate => $"{_CodeLOG.StartTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")} - " +
            $"{_CodeLOG.EndTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")}";

        /** Store the first occurence of a note with acceptable input only. **/
        public string NotaryContent => content();



        private double timeRemaining;

        public double TimeRemaining
        {
            get
            {
                return timeRemaining;
            }
            set
            {
                timeRemaining = value;
                OnPropertyChanged(nameof(TimeRemaining));
            }
        }











        #endregion




        #region Member Functions



        public void StartCountdown()
        {
            _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void TimerCallback(object? state)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (!IsDisposed)
                {
                    if (TimeRemaining == 0)
                    {
                        // Trigger removal logic
                        // TODO
                        DeleteCacheItemCommand.Execute(this);
                        _timer.Dispose();
                        IsDisposed = true;
                    }
                    else
                        TimeRemaining--;
                }
            });
        }

        public string content()
        {
            string pattern = "[A-Za-z]{5}[0-9]{0}";
            Regex xp = new Regex(pattern);

            foreach (PostIt p in _CodeLOG.PostItList)
            {
                if (xp.IsMatch(p.Error))
                    return p.Error;
                else if (xp.IsMatch(p.Solution))
                    return p.Solution;
                else if (xp.IsMatch(p.Suggestion))
                    return p.Suggestion;
                else if (xp.IsMatch(p.Comment))
                    return p.Comment;

            }

            return "No Notes";
        }


















        #endregion
    }
}
