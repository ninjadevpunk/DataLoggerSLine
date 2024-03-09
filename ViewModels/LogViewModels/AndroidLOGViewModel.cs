
using Data_Logger_1._3.Messages;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class AndroidLOGViewModel : ViewModelBase
    {
        private readonly AndroidCodingLOG _androidCodingLOG;
        private readonly MessagingService _messagingService;

        private Timer _timer;

        #region Constructors



        public AndroidLOGViewModel(AndroidCodingLOG androidCodingLOG)

        {
            _androidCodingLOG = androidCodingLOG; 
            TimeRemaining = 1200;
            StartCountdown();
        }



        #endregion





        #region Properties




        public string ProjectName => $"{_androidCodingLOG.ProjectName} ({_androidCodingLOG.ApplicationName})";

        public string ErrorCount => _androidCodingLOG.errorCount().ToString();

        public string SolutionCount => _androidCodingLOG.solutionCount().ToString();

        public string SuggestionCount => _androidCodingLOG.suggestionCount().ToString();

        public string CommentCount => _androidCodingLOG.commentCount().ToString();



        /** Save the start and end time here **/
        public string StartEndDate => $"{_androidCodingLOG.StartTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")} - " +
            $"{_androidCodingLOG.EndTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")}";

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
