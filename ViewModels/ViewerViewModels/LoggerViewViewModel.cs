using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Windows.Input;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    public abstract class LoggerViewViewModel
    {
        protected readonly NavigationService _navigationService;
        private readonly List<CreatePostItViewModel> _postIts;

        public abstract LOG.CATEGORY Category { get; }

        public abstract CacheContext Context { get; }

        protected LoggerViewViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _postIts = new List<CreatePostItViewModel>();
        }

        public void AddPostIt(CreatePostItViewModel postIt)
        {
            _postIts.Add(postIt);
        }


        public Visibility DisplayPicVisibility { get; set; }

        public string Author { get; set; }


        public string SignUpImage { get; set; }

        public string ProjectName { get; set; }

        public string ApplicationName { get; set; }

        public string Date { get; set; }



        private string output;
        public string Output { get; set; }

        public string Type { get; set; }

        public IEnumerable<CreatePostItViewModel> PostIts => _postIts;

        public ICommand OKCommand { get; set; }
    }
}
