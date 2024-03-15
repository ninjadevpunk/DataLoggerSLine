using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using MVVMEssentials.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class QtLOGViewModel : ViewModelBase
    {
        public CodingLOG _QtcodingLOG;
        private readonly LogCacheViewModel _vm;
        public bool IsDisposed { get; set; } = false;

        private Timer _timer;

        public ICommand DeleteCacheItemCommand { get; set; }



        #region Constructors



        public QtLOGViewModel(CodingLOG QtcodingLOG, LogCacheViewModel logCacheViewModel)
        {
            _vm = logCacheViewModel;

            _QtcodingLOG = QtcodingLOG;
            TimeRemaining = 1200;
            StartCountdown();

            DeleteCacheItemCommand = new DeleteQtCacheItemCommand(_vm);
        }





        #endregion



        #region Properties




        public string ProjectName => $"{_QtcodingLOG.ProjectName} ({_QtcodingLOG.ApplicationName})";

        public string ErrorCount => _QtcodingLOG.errorCount().ToString();

        public string SolutionCount => _QtcodingLOG.solutionCount().ToString();

        public string SuggestionCount => _QtcodingLOG.suggestionCount().ToString();

        public string CommentCount => _QtcodingLOG.commentCount().ToString();

        /** Save the start and end time here **/
        public string StartEndDate => $"{_QtcodingLOG.StartTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")} - " +
            $"{_QtcodingLOG.EndTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")}";

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
                if(!IsDisposed)
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

            foreach(PostIt p in _QtcodingLOG.PostItList)
            {
                if (xp.IsMatch(p.Error))
                    return p.Error;
                else if(xp.IsMatch(p.Solution))
                    return p.Solution;
                else if(xp.IsMatch(p.Suggestion))
                    return p.Suggestion;
                else if(xp.IsMatch(p.Comment))
                    return p.Comment;

            }

            return "No Notes";
        }




        #endregion
    }
}
