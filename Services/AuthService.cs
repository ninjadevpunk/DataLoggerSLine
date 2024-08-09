using Data_Logger_1._3.Models;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Data_Logger_1._3.Services
{
    public class AuthService
    {
        private readonly DATAWRITER _writer;
        private readonly DATAREADER _reader;

        public ACCOUNT Account { get; set; } = new();

        public AuthService()
        {


        }

        public AuthService(DATAWRITER writer, DATAREADER reader)
        {
            _writer = writer;
            _reader = reader;
            Account.ID = 1;
            Account.Email = "support@datalogger.co.za";
            Account.Online = true;
        }


        public void SetDisplayPic(string source)
        {
            Account.ProfilePic = source;
        }

        public bool SignUp(string email, string password, string displayName, string surname, bool IsEmployee, string companyName, string companyAddress, string companyLogo)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(email) || password is null || password.Length <= 5)
            {
                try
                {
                    if (password.Length <= 5)
                    {
                        throw new InvalidOperationException();
                    }
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Your password is too short! Please enter a password that is at least 6 characters long.",
                        "Password Too Short", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
                catch (Exception e)
                {
                    // TODO
                }


                return false;
            }

            try
            {
                _writer.UnsetCurrentUser(Account);

                Account.ID = _writer.CreateAccountID();
                Account.FirstName = displayName;
                Account.LastName = surname;
                Account.Email = email;
                Account.Password = password;
                Account.IsEmployee = IsEmployee;
                Account.CompanyName = companyName;
                Account.CompanyAddress = companyAddress;
                Account.CompanyLogo = companyLogo;
                Account.Online = true;

                bool UserIsActive = _writer.AddAccount(Account);
                Account.Password = SaltedSHA256Hash(password, Account.ID.ToString());

                if (UserIsActive)
                    return _writer.SetCurrentUser(Account);


            }
            catch (Exception e)
            {

                // TODO
                // Send Feedback here and use DLS for the account.


                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);


            }

            return false;

        }

        public bool SignIn(string email, string password)
        {
            try
            {
                var temporaryAccount = _writer.FindAccountByEmail(email, password);

                if (temporaryAccount is not null)
                {
                    _writer.UnsetCurrentUser(Account);

                    Account.ID = temporaryAccount.ID;
                    Account.ProfilePic = temporaryAccount.ProfilePic;
                    Account.FirstName = temporaryAccount.FirstName;
                    Account.LastName = temporaryAccount.LastName;
                    Account.Email = temporaryAccount.Email;
                    Account.Password = temporaryAccount.Password;
                    Account.IsEmployee = temporaryAccount.IsEmployee;
                    Account.CompanyName = temporaryAccount.CompanyName;
                    Account.CompanyAddress = temporaryAccount.CompanyAddress;
                    Account.CompanyLogo = temporaryAccount.CompanyLogo;
                    Account.Online = true;

                    _writer.SetCurrentUser(Account);

                    return true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

            }

            return false;

        }

        public bool SignOut()
        {

            try
            {
                // TODO
                Account.Online = false;

                // Modify account status to show user is offline
                var UserIsActive = _writer.UnsetCurrentUser(Account);

                if (!UserIsActive)
                    return true;
            }
            catch (Exception)
            {
                // TODO
            }

            return false;
        }

        public void ForgotPasswordRequest()
        {
            //
        }

        public static string SaltedSHA256Hash(string value, string accountID)
        {
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;

                // The account account is the salt. 
                // So 2 users with the same password have different hashes. 
                // For example, if someone knows their own hash, they can't see who has the same password.
                string input = value + accountID;
                byte[] result = hash.ComputeHash(enc.GetBytes(input));

                StringBuilder hashedStringBuilder = new StringBuilder();
                foreach (byte b in result)
                {
                    hashedStringBuilder.Append(b.ToString("x2"));
                }

                return hashedStringBuilder.ToString();
            }
        }
    }
}
