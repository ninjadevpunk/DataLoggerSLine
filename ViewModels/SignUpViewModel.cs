
using Data_Logger_1._3.Commands;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Security;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {


        private readonly AuthService _authService;

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

		private SecureString password;
		public SecureString Password
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
				OnPropertyChanged(nameof(YesBox));

				if(YesBox)
					FieldEnabled = true;

				if (NoBox == YesBox)
					NoBox = !yesBox;
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
				OnPropertyChanged(nameof(NoBox));

				if(NoBox)
					FieldEnabled = false;

				if(YesBox == NoBox)
					YesBox = !noBox;
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

		ICommand DisplayCommand { get; }

		ICommand EmailSignUpCommand { get; }

        ICommand GoogleSignUpCommand { get; }


		public SignUpViewModel(AuthService authService)
        {
            _authService = authService;

			// TODO
			// DisplayCommand = new DisplayPicCommand(this, _authService);
			//EmailSignUpCommand = new EmailSignUpCommand(this, _authService);
			// GoogleSignUpCommand = new GoogleSignUpCommand(this, _authService);
        }


    }
}
