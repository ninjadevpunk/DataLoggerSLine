using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {


        private readonly AuthService _authService;
		private readonly NavigationService _navigationService;

		public SignUpViewModel(AuthService authService, NavigationService navigationService)
        {
            _authService = authService;
			_navigationService = navigationService;

            DisplayPicCommand = new DisplayPicCommand(this, _authService);
			EmailSignUpCommand = new EmailSignUpCommand(this, _authService, _navigationService);
            GoogleSignInCommand = new GoogleSignInCommand(this, _authService);

			SignUpImage = _authService.Account.ProfilePic;
		}

		/* PROPERTIES */

		private string signUpImage;
		public string SignUpImage
		{
			get
			{
				return signUpImage;
			}
			set
			{
				signUpImage = value;
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


		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private string surname;
		public string Surname
		{
			get
			{
				return surname;
			}
			set
			{
				surname = value;
				OnPropertyChanged(nameof(Surname));
			}
		}

		private string password;
		public string Password
		{
			get
			{
				return password;
			}
			set
			{
                password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

		private string email;
		public string Email
		{
			get
			{
				return email;
			}
			set
			{
				email = value;
				OnPropertyChanged(nameof(Email));
			}
		}

		private bool yesBox;
		public bool YesBox
		{
			get
			{
				return yesBox;
			}
			set
			{
				yesBox = value;

				if(YesBox)
					FieldEnabled = true;

				if (NoBox == YesBox)
					NoBox = !YesBox;

				OnPropertyChanged(nameof(YesBox));

			}
		}

		private bool noBox;
		public bool NoBox
		{
			get
			{
				return noBox;
			}
			set
			{
				noBox = value;

				if(NoBox)
					FieldEnabled = false;

				if(YesBox == NoBox)
					YesBox = !NoBox;

				OnPropertyChanged(nameof(NoBox));

			}
		}

		private string companyName;
		public string CompanyName
		{
			get
			{
				return companyName;
			}
			set
			{
				companyName = value;
				OnPropertyChanged(nameof(CompanyName));
			}
		}

		private string companyAddress;
		public string CompanyAddress
		{
			get
			{
				return companyAddress;
			}
			set
			{
				companyAddress = value;
				OnPropertyChanged(nameof(CompanyAddress));
			}
		}

		private bool fieldEnabled;
		public bool FieldEnabled
		{
			get
			{
				return fieldEnabled;
			}
			set
			{
				fieldEnabled = value;
				OnPropertyChanged(nameof(FieldEnabled));
			}
		}

		public ICommand DisplayPicCommand { get; set; }

		public ICommand EmailSignUpCommand { get; set; }

        public ICommand GoogleSignInCommand { get; set; }


    }
}
