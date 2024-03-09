using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels;
using MVVMEssentials.Commands;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Data_Logger_1._3.Commands
{
    public class GoogleSignUpCommand : AsyncCommandBase
    {

        private readonly SignUpViewModel _signUpViewModel;
        private readonly AuthService _authService;

        public GoogleSignUpCommand(SignUpViewModel signUpViewModel)
        {
            _signUpViewModel = signUpViewModel;
        }

        public GoogleSignUpCommand(SignUpViewModel signUpViewModel, AuthService authService)
        {
            _signUpViewModel = signUpViewModel;
            _authService = authService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }


        /* PASSWORD SUPPORT */

        public static String sha526_hash(String value, string userID)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                //the user id is the salt. 
                //So 2 users with same password have different hashes. 
                //For example if someone knows his own hash he can't see who has same password
                string input = value + userID;
                Byte[] result = hash.ComputeHash(enc.GetBytes(input));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2")); //You could also use other encodingslike BASE64 
            }


            return Sb.ToString();
        }
    }
}
