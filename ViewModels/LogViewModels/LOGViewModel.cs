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
    public abstract class LOGViewModel : ViewModelBase
    {
        protected readonly LOG _LOG;
        protected readonly LogCacheViewModel _vm;
        protected readonly Cachemaster _cacheMaster;
        public abstract CacheContext LOGViewModelContext { get; }



        public bool IsDisposed { get; set; } = false;

        public Timer _timer;

        public ICommand EditCommand { get; set; }

        public ICommand ViewCommand { get; set; }

        public ICommand DeleteCacheItemCommand { get; set; }

        public ICommand QuickDeleteCacheItemCommand { get; set; }





        #region Constructors



        public LOGViewModel(LOG log, LogCacheViewModel logCacheViewModel, ObservableCollection<CreatePostItViewModel> createPostItViewModels, DataService dataService)
        {
            _vm = logCacheViewModel;

            _LOG = log;
            NotaryContent = PostItContent(createPostItViewModels);
            _LOG.Content = NotaryContent;
            TimeRemaining = 1200;
            StartCountdown();

            // TODO 
            // ViewCommand = ...
            Setup(dataService);

            _cacheMaster = dataService.GetCachemaster();
            _cacheMaster.IdentifiersChecked(_LOG.ID);

        }

        public LOGViewModel(LOG log, LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            _vm = logCacheViewModel;

            _LOG = log;
            NotaryContent = _LOG.Content;
            TimeRemaining = 1200;
            StartCountdown();

            // TODO 
            // ViewCommand = ...
            Setup(dataService);

            _cacheMaster = dataService.GetCachemaster();
        }

        private void Setup(DataService dataService)
        {
            DeleteCacheItemCommand = new DeleteCacheItemCommand(_vm, dataService, true);
            QuickDeleteCacheItemCommand = new DeleteCacheItemCommand(_vm, dataService, false);
        }

        #endregion










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








        private void StartCountdown()
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
                            DeleteCacheItem();
                            _timer.Dispose();
                            IsDisposed = true;
                        }
                        else
                            TimeRemaining--;
                    }
                });
            }
        }

        protected abstract void DeleteCacheItem();

        public void DeleteCacheFile(int id, CacheContext cacheContext)
        {
            _cacheMaster.DeleteViewModel(id, cacheContext);
        }






        public string PostItContent(ObservableCollection<CreatePostItViewModel> createPostItViewModel)
        {
            string pattern = "[A-Za-z]{5}[0-9]{0}";
            Regex xp = new Regex(pattern);

            foreach (CreatePostItViewModel postItViewModel in createPostItViewModel)
            {
                if (xp.IsMatch(postItViewModel.Display_Error))
                    return postItViewModel.Display_Error;
                else if (xp.IsMatch(postItViewModel.Display_Solution))
                    return postItViewModel.Display_Solution;
                else if (xp.IsMatch(postItViewModel.Display_Suggestion))
                    return postItViewModel.Display_Suggestion;
                else if (xp.IsMatch(postItViewModel.Display_Comment))
                    return postItViewModel.Display_Comment;

            }

            return "No Notes";
        }






        #endregion









    }
}
