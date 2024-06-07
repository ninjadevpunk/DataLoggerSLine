using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class GraphicsLOGViewModel : ViewModelBase
    {
        public GraphicsLOG _GraphicsLOG;
        private readonly LogCacheViewModel _vm;
        private readonly ObservableCollection<CreatePostItViewModel> _createPostItViewModels;

        public bool IsDisposed { get; set; } = false;

        public Timer _timer;

        public ICommand EditCommand { get; set; }

        public ICommand ViewCommand { get; set; }

        public ICommand DeleteCacheItemCommand { get; set; }
        public ICommand QuickDeleteCacheItemCommand { get; set; }


        #region Constructor


        public GraphicsLOGViewModel(GraphicsLOG graphicsLOG, LogCacheViewModel logCacheViewModel, ObservableCollection<CreatePostItViewModel> createPostItViewModels, DataService dataService)
        {
            _vm = logCacheViewModel;
            _createPostItViewModels = createPostItViewModels;

            _GraphicsLOG = graphicsLOG;
            NotaryContent = content();
            _GraphicsLOG.Content = content();
            TimeRemaining = 1200;
            StartCountdown();

            DeleteCacheItemCommand = new DeleteGraphicsCacheItemCommand(_vm, dataService, true);
        }

        public GraphicsLOGViewModel(GraphicsLOG graphicsLOG, LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            _vm = logCacheViewModel;

            _GraphicsLOG = graphicsLOG;
            NotaryContent = _GraphicsLOG.Content;
            TimeRemaining = 1200;
            StartCountdown();

            DeleteCacheItemCommand = new DeleteGraphicsCacheItemCommand(_vm, dataService, true);
        }




        #endregion




        #region Properties


        public string ProjectName => $"{_GraphicsLOG.Project.Name} ({_GraphicsLOG.Application.Name})";

        public string ErrorCount => _GraphicsLOG.errorCount().ToString();

        public string SolutionCount => _GraphicsLOG.solutionCount().ToString();

        public string SuggestionCount => _GraphicsLOG.suggestionCount().ToString();

        public string CommentCount => _GraphicsLOG.commentCount().ToString();

        /** Save the start and end time here **/
        public string StartEndDate => $"{_GraphicsLOG.StartTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")} - " +
            $"{_GraphicsLOG.EndTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")}";

        /** Store the first occurence of a note with acceptable input only. **/
        public string NotaryContent { get; set; }



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
            if (Application.Current != null)
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
        }

        public string content()
        {
            string pattern = "[A-Za-z]{5}[0-9]{0}";
            Regex xp = new Regex(pattern);

            foreach (CreatePostItViewModel p in _createPostItViewModels)
            {
                if (xp.IsMatch(p.Display_Error))
                    return p.Display_Error;
                else if (xp.IsMatch(p.Display_Solution))
                    return p.Display_Solution;
                else if (xp.IsMatch(p.Display_Suggestion))
                    return p.Display_Suggestion;
                else if (xp.IsMatch(p.Display_Comment))
                    return p.Display_Comment;

            }

            return "No Notes";
        }











        #endregion
    }
}
