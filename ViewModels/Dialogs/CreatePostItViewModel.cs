using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class CreatePostItViewModel : ViewModelBase
    {

        private readonly NavigationService _navigationService;
		private readonly LoggerCreateViewModel _loggerCreateViewModel;


		private readonly ObservableCollection<string> _subjects;
        public bool FoundDateCaptured { get; set; } = false;
        public bool SolvedDateCaptured { get; set; } = false;

        public IEnumerable<string> Subjects => _subjects;

		public int Error_RTFLength { get; set; } = 0;
		public int Solution_RTFLength { get; set; } = 0;
		public int Suggestion_RTFLength { get; set; } = 0;
		public int Comment_RTFLength { get; set; } = 0;
		public int backspaceCONSTANT { get; } = 274;

        public bool Error_LengthIsSet { get; set; } = false;
		public bool Solution_LengthIsSet { get; set; } = false;
		public bool Suggestion_LengthIsSet { get; set; } = false;
		public bool Comment_LengthIsSet { get; set; } = false;


        public int PostItID { get; set; }

		private string subject;
		public string Subject
		{
			get
			{
				return subject;
			}
			set
			{
				subject = value;
				OnPropertyChanged(nameof(Subject));
			}
		}

		private string error;
		public string Error
		{
			get
			{
				return error;
			}
			set
			{
				error = value;

                if (!FoundDateCaptured)
                    DateFound = DateTime.Now;

				ErrorVisible = Error != "" ? Visibility.Visible : Visibility.Collapsed;


				if(!Error_LengthIsSet)
				{
                    Error_RTFLength = Error.Length - 1;
                    Error_LengthIsSet = true;

					if (Error_RTFLength < 0)
					{
                        Error_RTFLength = 0;
                        Error_LengthIsSet = false;
                    }
				}

				if(Error_LengthIsSet && Error.Length == Error_RTFLength || Error_LengthIsSet && Error.Length == backspaceCONSTANT)
				{
					Error = "";
				}

				OnPropertyChanged(nameof(Error));
			}
		}



		private DateTime dateFound;
		public DateTime DateFound
		{
			get
			{
				return dateFound;
			}
			set
			{
				
				if (!FoundDateCaptured)
				{
					dateFound = value;
                    FoundDateCaptured = true;
                    OnPropertyChanged(nameof(DateFound));
                }
                
			}
		}

		private string display_error;
		public string Display_Error
		{
			get
			{
				return display_error;
			}
			set
			{
				display_error = value;
                OnPropertyChanged(nameof(Display_Error));
			}
		}

		private string solution;
		public string Solution
		{
			get
			{
				return solution;
			}
			set
			{
				solution = value;

				if(!SolvedDateCaptured)
					DateSolved = DateTime.Now;


				SolutionVisible = Solution != "" ? Visibility.Visible : Visibility.Collapsed;

                if (!Solution_LengthIsSet)
                {
                    Solution_RTFLength = Solution.Length - 1;
                    Solution_LengthIsSet = true;

                    if (Solution_RTFLength < 0)
                    {
                        Solution_RTFLength = 0;
                        Solution_LengthIsSet = false;
                    }
                }

                if (Solution_LengthIsSet && Solution.Length == Solution_RTFLength || Solution_LengthIsSet && Solution.Length == backspaceCONSTANT)
                {
                    Solution = "";
                }


                OnPropertyChanged(nameof(Solution));
            }
		}

		private DateTime dateSolved;
		public DateTime DateSolved
		{
			get
			{
				return dateSolved;
			}
			set
			{
				if(!SolvedDateCaptured)
				{
                    dateSolved = value;
					SolvedDateCaptured = true;
                    OnPropertyChanged(nameof(DateSolved));
                }
			}
		}

		private string display_solution;
		public string Display_Solution
		{
			get
			{
				return display_solution;
			}
			set
			{
				display_solution = value;
				OnPropertyChanged(nameof(Display_Solution));
			}
		}

		private string suggestion;
		public string Suggestion
		{
			get
			{
				return suggestion;
			}
			set
			{
				suggestion = value;
				SuggestionVisible = Suggestion != "" ? Visibility.Visible : Visibility.Collapsed;

                if (!Suggestion_LengthIsSet)
                {
                    Suggestion_RTFLength = Suggestion.Length - 1;
                    Suggestion_LengthIsSet = true;

                    if (Suggestion_RTFLength < 0)
                    {
                        Suggestion_RTFLength = 0;
                        Suggestion_LengthIsSet = false;
                    }
                }

                if (Suggestion_LengthIsSet && Suggestion.Length == Suggestion_RTFLength || Suggestion_LengthIsSet && Suggestion.Length == backspaceCONSTANT)
                {
                    Suggestion = "";
                }

                OnPropertyChanged(nameof(Suggestion));
            }
		}

		private string display_suggestion;
		public string Display_Suggestion
		{
			get
			{
				return display_suggestion;
			}
			set
			{
				display_suggestion = value;
				OnPropertyChanged(nameof(Display_Suggestion));
			}
		}

		private string comment;
		public string Comment
		{
			get
			{
				return comment;
			}
			set
			{
				comment = value;
				CommentVisible = Comment != "" ? Visibility.Visible : Visibility.Collapsed;


                if (!Comment_LengthIsSet)
                {
                    Comment_RTFLength = Comment.Length - 1;
                    Comment_LengthIsSet = true;

                    if (Comment_RTFLength < 0)
                    {
                        Comment_RTFLength = 0;
                        Comment_LengthIsSet = false;
                    }
                }

                if (Comment_LengthIsSet && Comment.Length == Comment_RTFLength || Comment_LengthIsSet && Comment.Length == backspaceCONSTANT)
                {
                    Comment = "";
                }


                OnPropertyChanged(nameof(Comment));
            }
		}

		private string display_comment;
		public string Display_Comment
		{
			get
			{
				return display_comment;
			}
			set
			{
				display_comment = value;
				OnPropertyChanged(nameof(Display_Comment));
			}
		}

		private Visibility errorVisible;
		public Visibility ErrorVisible
		{
			get
			{
				return errorVisible;
			}
			set
			{
                errorVisible = value;
				OnPropertyChanged(nameof(ErrorVisible));
			}
		}

		private Visibility solutionVisible;
		public Visibility SolutionVisible
		{
			get
			{
				return solutionVisible;
			}
			set
			{
				solutionVisible = value;
				OnPropertyChanged(nameof(SolutionVisible));
			}
		}

		private Visibility suggestionVisible;
		public Visibility SuggestionVisible
		{
			get
			{
				return suggestionVisible;
			}
			set
			{
				suggestionVisible = value;
				OnPropertyChanged(nameof(SuggestionVisible));
			}
		}

		private Visibility commentVisible;
		public Visibility CommentVisible
		{
			get
			{
				return commentVisible;
			}
			set
			{
				commentVisible = value;
				OnPropertyChanged(nameof(CommentVisible));
			}
		}


		// REPLACER 

		private string replace;
		public string Replace
		{
			get
			{
				return replace;
			}
			set
			{
				replace = value;
				OnPropertyChanged(nameof(Replace));
			}
		}

		private bool option1Check;
		public bool Option1Check
        {
			get
			{
				return option1Check;
			}
			set
			{
				option1Check = value;
				OnPropertyChanged(nameof(Option1Check));
			}
		}

		private bool option2Check;
		public bool Option2Check
		{
			get
			{
				return option2Check;
			}
			set
			{
				option2Check = value;
				OnPropertyChanged(nameof(Option2Check));
			}
		}

		private bool option3Check;
		public bool Option3Check
		{
			get
			{
				return option3Check;
			}
			set
			{
				option3Check = value;
				OnPropertyChanged(nameof(Option3Check));
			}
		}

		private bool option4Check;
		public bool Option4Check
		{
			get
			{
				return option4Check;
			}
			set
			{
				option4Check = value;
				OnPropertyChanged(nameof(Option4Check));
			}
		}


		public ICommand OKCommand { get; set; }

		public ICommand PostCommand {  get; set; }

        public ICommand DeletePostItCommand { get; set; }




        #region Toolbar




        public ICommand EraserCommand { get; set; }

		public ICommand HighlighterCommand { get; set; }

		public ICommand FontCommand { get; set; }

		public ICommand ULListCommand { get; set; }

		public ICommand OLListCommand { get; set; }

		public ICommand BoldCommand { get; set; }

		public ICommand ItalicsCommand { get; set; }

		public ICommand UnderlineCommand { get; set; }




        #endregion


        public CreatePostItViewModel(NavigationService navigationService, LoggerCreateViewModel loggerCreateViewModel)
        {
            _navigationService = navigationService;
			_loggerCreateViewModel = loggerCreateViewModel;

			Subject = "";
			Error = "";

			ErrorVisible = Visibility.Collapsed;
			SolutionVisible = Visibility.Collapsed;
			SuggestionVisible = Visibility.Collapsed;
			CommentVisible = Visibility.Collapsed;

			Solution = "";
			Suggestion = "";
			Comment = "";

			Option1Check = true;

			PostCommand = new PostCommand(_navigationService, _loggerCreateViewModel, this);
			DeletePostItCommand = new DeletePostItCommand(_loggerCreateViewModel);
        }

        #region Member Functions



        //public void RTF_Formatter_Method(string Property, int RTFLength, bool lengthIsSet)
        //{

        //	if(!lengthIsSet)
        //	{
        //		RTFLength = Property.Length - 1;
        //		lengthIsSet = true;

        //		if(RTFLength < 0)
        //		{
        //			RTFLength = 0;
        //			lengthIsSet = false;
        //		}
        //	}

        //	if (lengthIsSet && Property.Length == RTFLength)
        //		Property = "";

        //}









        #endregion

    }
}
