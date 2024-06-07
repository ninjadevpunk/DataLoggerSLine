using Data_Logger_1._3.Commands;
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
    public class QtLOGViewModel : ViewModelBase
    {
        public CodingLOG _QtcodingLOG;
        private readonly LogCacheViewModel _vm;
        private readonly ObservableCollection<CreatePostItViewModel> _createPostItViewModels;
        private readonly Cachemaster _cacheMaster;

        public bool IsDisposed { get; set; } = false;

        public Timer _timer;

        public ICommand EditCommand { get; set; }

        public ICommand ViewCommand { get; set; }

        public ICommand DeleteCacheItemCommand { get; set; }

        public ICommand QuickDeleteCacheItemCommand { get; set; }



        #region Constructors



        public QtLOGViewModel()
        {
            // Blank
        }

        public QtLOGViewModel(CodingLOG QtcodingLOG, LogCacheViewModel logCacheViewModel, ObservableCollection<CreatePostItViewModel> createPostItViewModels, DataService dataService)
        {
            _vm = logCacheViewModel;
            _createPostItViewModels = createPostItViewModels;

            _QtcodingLOG = QtcodingLOG;
            NotaryContent = content();
            _QtcodingLOG.Content = content();
            TimeRemaining = 1200;
            StartCountdown();

            EditCommand = new EditCommand(CacheContext.Qt, _vm._navigationService, _vm);

            // TODO 
            // ViewCommand = ...

            DeleteCacheItemCommand = new DeleteQtCacheItemCommand(_vm, dataService, true);
            QuickDeleteCacheItemCommand = new DeleteQtCacheItemCommand(_vm, dataService, false);

            _cacheMaster =  dataService.GetCachemaster();
            _cacheMaster.IdentifiersChecked(_QtcodingLOG.ID);
            _cacheMaster.SaveQtViewModel(this);
        }

        public QtLOGViewModel(CodingLOG QtcodingLOG, LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            _vm = logCacheViewModel;

            _QtcodingLOG = QtcodingLOG;
            NotaryContent = _QtcodingLOG.Content;
            TimeRemaining = 1200;
            StartCountdown();

            EditCommand = new EditCommand(CacheContext.Qt, _vm._navigationService, _vm);

            // TODO 
            // ViewCommand = ...

            DeleteCacheItemCommand = new DeleteQtCacheItemCommand(_vm, dataService, true);
            QuickDeleteCacheItemCommand = new DeleteQtCacheItemCommand(_vm, dataService, false);

            _cacheMaster = dataService.GetCachemaster();
        }



        #endregion



        #region Properties




        public string ProjectName => $"{_QtcodingLOG.Project.Name} ({_QtcodingLOG.Application.Name})";

        public string ErrorCount => _QtcodingLOG.errorCount().ToString();

        public string SolutionCount => _QtcodingLOG.solutionCount().ToString();

        public string SuggestionCount => _QtcodingLOG.suggestionCount().ToString();

        public string CommentCount => _QtcodingLOG.commentCount().ToString();

        /** Save the start and end time here **/
        public string StartEndDate => $"{_QtcodingLOG.StartTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")} - " +
            $"{_QtcodingLOG.EndTime.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")}";

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
            try
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (!IsDisposed)
                        {
                            if (TimeRemaining == 0)
                            {
                                DeleteQtCacheFile(_QtcodingLOG.ID);
                                DeleteCacheItemCommand.Execute(this);
                                _timer.Dispose();
                                IsDisposed = true;
                            }
                            else
                            {
                                TimeRemaining--;
                            }
                        }
                    });
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void DeleteQtCacheFile(int id)
        {
            _cacheMaster.DeleteQtViewModel(id);
        }

        public string content()
        {
            string pattern = "[A-Za-z]{5}[0-9]{0}";
            Regex xp = new Regex(pattern);

            foreach(CreatePostItViewModel p in _createPostItViewModels)
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
