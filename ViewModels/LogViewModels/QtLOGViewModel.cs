
using Data_Logger_1._3.Messages;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class QtLOGViewModel : ViewModelBase
    {
        private readonly CodingLOG _QtcodingLOG;
        private readonly MessagingService _messagingService;

        private Timer _timer;




        #region Constructors



        public QtLOGViewModel(CodingLOG QtcodingLOG)
        {
            _QtcodingLOG = QtcodingLOG;
            TimeRemaining = 1200;
            StartCountdown();
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
