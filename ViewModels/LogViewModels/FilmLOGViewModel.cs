using Data_Logger_1._3.Messages;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class FilmLOGViewModel : ViewModelBase
    {
        private readonly FilmLOG _filmLOG;
        private readonly MessagingService _messagingService;

        private Timer _timer;

        #region Constructor


        public FilmLOGViewModel(FilmLOG filmLOG)
        {
            _filmLOG = filmLOG;
            TimeRemaining = 1200;
            StartCountdown();
        }









        #endregion





        #region Properties



        public string ProjectName => $"{_filmLOG.ProjectName} ({_filmLOG.ApplicationName})";

        public string ErrorCount => _filmLOG.errorCount().ToString();

        public string SolutionCount => _filmLOG.solutionCount().ToString();

        public string SuggestionCount => _filmLOG.suggestionCount().ToString();

        public string CommentCount => _filmLOG.commentCount().ToString();

        /** Save the start and end time here **/
        public string StartEndDate => $"{_filmLOG.StartTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")} - " +
            $"{_filmLOG.EndTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")}";

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

        private void TimerCallback(object state)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TimeRemaining--;

                if (TimeRemaining <= 0)
                {
                    // Trigger removal logic
                    RemoveItem();
                }
            });
        }


        private void RemoveItem()
        {
            _messagingService.Send(new RemoveItemMessage(this));
        }

        public string content()
        {
            return "No Notes";
        }








        #endregion
    }
}
