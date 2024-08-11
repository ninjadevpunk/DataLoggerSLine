using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Data_Logger_1._3.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly AuthService _authService;

        public MainWindowViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            ShowDefault = Visibility.Visible;

            CodingChecked = true;
            CodingQtChecked = true;
            NotesChecked = false;

            GoBackCommand = new GoBackCommand(_navigationService, this);
            GoForwardCommand = new GoForwardCommand(_navigationService, this);

            IconBackFill = DisabledColor;
            IconForwardFill = DisabledColor;
        }

        public MainWindowViewModel(NavigationService navigationService, AuthService authService)
        {
            _navigationService = navigationService;
            _authService = authService;

            ShowDefault = Visibility.Visible;

            CodingChecked = true;
            CodingQtChecked = true;
            NotesChecked = false;

            GoBackCommand = new GoBackCommand(_navigationService, this);
            GoForwardCommand = new GoForwardCommand(_navigationService, this);

            IconBackFill = DisabledColor;
            IconForwardFill = DisabledColor;
        }





        private static readonly Brush? EnabledColor = TryParseBrush("iconCOLOURAccent01");
        private static readonly Brush? DisabledColor = TryParseBrush("AccentColour");


        private Brush? iconBackFill;
        public Brush? IconBackFill
        {
            get
            {
                return iconBackFill;
            }
            set
            {
                iconBackFill = value;
                OnPropertyChanged(nameof(IconBackFill));
            }
        }

        private Brush? iconForwardFill;
        public Brush? IconForwardFill
        {
            get
            {
                return iconForwardFill;
            }
            set
            {
                iconForwardFill = value;
                OnPropertyChanged(nameof(IconForwardFill));
            }
        }




        private bool backEnabled;
        public bool BackEnabled
        {
            get
            {
                return backEnabled;
            }
            set
            {
                backEnabled = value;
                IconBackFill = BackEnabled ? EnabledColor : DisabledColor;
                OnPropertyChanged(nameof(BackEnabled));
            }
        }

        private bool forwardEnabled;
        public bool ForwardEnabled
        {
            get
            {
                return forwardEnabled;
            }
            set
            {
                forwardEnabled = value;
                IconForwardFill = ForwardEnabled ? EnabledColor : DisabledColor;
                OnPropertyChanged(nameof(ForwardEnabled));
            }
        }


        private bool codingChecked;
        public bool CodingChecked
        {
            get
            {
                return codingChecked;
            }
            set
            {
                if (codingChecked != value)
                {
                    codingChecked = value;
                    OnPropertyChanged(nameof(CodingChecked));

                    if (codingChecked)
                    {
                        // Navigate to the Coding page
                        _navigationService.NavigateToLogCachePage();
                        _navigationService.ChangeData(CacheContext.Qt);
                        CodingQtChecked = true;
                        UncheckButtons();
                    }
                }
            }
        }


        private bool codingQtChecked;
        public bool CodingQtChecked
        {
            get
            {
                return codingQtChecked;
            }
            set
            {
                if (codingQtChecked != value)
                {
                    codingQtChecked = value;
                    OnPropertyChanged(nameof(CodingQtChecked));

                    if (codingQtChecked)
                    {
                        _navigationService.NavigateToLogCachePage(CacheContext.Qt);
                    }
                }

            }
        }


        private bool codingAndroidChecked;
        public bool CodingAndroidChecked
        {
            get
            {
                return codingAndroidChecked;
            }
            set
            {
                if (codingAndroidChecked != value)
                {
                    codingAndroidChecked = value;
                    OnPropertyChanged(nameof(CodingAndroidChecked));

                    if (codingAndroidChecked)
                    {
                        _navigationService.NavigateToLogCachePage(CacheContext.AndroidStudio);

                    }
                }
            }
        }


        private bool codingGenericChecked;
        public bool CodingGenericChecked
        {
            get
            {
                return codingGenericChecked;
            }
            set
            {
                if (codingGenericChecked != value)
                {
                    codingGenericChecked = value;
                    OnPropertyChanged(nameof(CodingGenericChecked));

                    if (codingGenericChecked)
                    {
                        _navigationService.NavigateToLogCachePage(CacheContext.Coding);

                    }
                }
            }
        }


        private bool graphicsChecked;
        public bool GraphicsChecked
        {
            get
            {
                return graphicsChecked;
            }
            set
            {
                if (graphicsChecked != value)
                {
                    graphicsChecked = value;
                    OnPropertyChanged(nameof(GraphicsChecked));

                    if (graphicsChecked)
                    {
                        _navigationService.NavigateToLogCachePage(CacheContext.Graphics);
                        UncheckButtons();

                    }
                }
            }
        }


        private bool filmChecked;
        public bool FilmChecked
        {
            get
            {
                return filmChecked;
            }
            set
            {
                if (filmChecked != value)
                {
                    filmChecked = value;
                    OnPropertyChanged(nameof(FilmChecked));

                    if (filmChecked)
                    {
                        _navigationService.NavigateToLogCachePage(CacheContext.Film);
                        UncheckButtons();

                    }
                }
            }
        }


        private bool notesChecked;
        public bool NotesChecked
        {
            get
            {
                return notesChecked;
            }
            set
            {
                if (notesChecked != value)
                {
                    notesChecked = value;
                    OnPropertyChanged(nameof(NotesChecked));

                    if (notesChecked)
                    {
                        _navigationService.NavigateToNOTESDashboard();
                        UncheckButtons();
                    }
                }
            }
        }


        private bool genericNotesChecked;
        public bool GenericNotesChecked
        {
            get
            {
                return genericNotesChecked;
            }
            set
            {
                if (genericNotesChecked != value)
                {
                    genericNotesChecked = value;
                    OnPropertyChanged(nameof(GenericNotesChecked));

                    if (genericNotesChecked)
                    {
                        _navigationService.NavigateToCreateNotesPage();
                    }
                }
            }
        }


        private bool checklistNotesChecked;
        public bool ChecklistNotesChecked
        {
            get
            {
                return checklistNotesChecked;
            }
            set
            {
                if (checklistNotesChecked != value)
                {
                    checklistNotesChecked = value;
                    OnPropertyChanged(nameof(ChecklistNotesChecked));

                    if (checklistNotesChecked)
                    {
                        _navigationService.NavigateToCreateCheckListPage();
                    }
                }
            }
        }


        private bool flexiChecked;
        public bool FlexiChecked
        {
            get
            {
                return flexiChecked;
            }
            set
            {
                if (flexiChecked != value)
                {
                    flexiChecked = value;
                    OnPropertyChanged(nameof(FlexiChecked));

                    if (flexiChecked)
                    {
                        _navigationService.NavigateToLogCachePage(CacheContext.Flexi);
                    }
                }
            }
        }


        private string signupImage;
        public string SignUpImage
        {
            get
            {
                return signupImage;
            }
            set
            {
                signupImage = value;

                if (SignUpImage != "")
                    ShowDefault = Visibility.Collapsed;
                else
                    ShowDefault = Visibility.Visible;

                OnPropertyChanged(nameof(SignUpImage));
            }
        }


        private Visibility showDefault;
        public Visibility ShowDefault
        {
            get
            {
                return showDefault;
            }
            set
            {
                showDefault = value;
                OnPropertyChanged(nameof(ShowDefault));
            }
        }



        public void UncheckButtons()
        {
            GenericNotesChecked = false;
            ChecklistNotesChecked = false;
            FlexiChecked = false;
        }

        public ICommand FeedbackCommand { get; set; }

        public ICommand HelpCommand { get; set; }

        public ICommand SettingsCommand { get; set; }

        public ICommand GoBackCommand { get; set; }
        public ICommand GoForwardCommand { get; set; }


        private static Brush? TryParseBrush(string value)
        {
            try
            {
                return (Brush)Application.Current.FindResource(value);
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return Brushes.Transparent;
            }
            catch (Exception ex)
            {
                return Brushes.Black;
            }
        }
    }
}
