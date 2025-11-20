using Data_Logger_1._3.Models;
using System.Windows;

namespace Data_Logger_1._3.Services
{
    public class AuthService
    {
        private readonly ENTITYWRITER _writer;

        public ACCOUNT Account { get; set; } = new();

        public AuthService()
        {


        }

        public AuthService(ENTITYWRITER writer)
        {
            _writer = writer;
            Account.accountID = 1;
            Account.Email = "support@datalogger.co.za";
            Account.Online = true;
        }


        public void SetDisplayPic(string source)
        {
            Account.ProfilePic = source;
        }

        public async Task<bool> SignUp(string dp, string email, string password, string displayName, string surname, bool isEmployee, string companyName, string companyAddress, string companyLogo)
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
                catch (Exception ex)
                {
                    await _writer.HandleExceptionAsync(ex, "SignUp()");
                }


                return false;
            }

            try
            {
                await _writer.UnsetCurrentUser(Account);

                var account = new ACCOUNT
                {
                    ProfilePic = dp,
                    FirstName = displayName,
                    LastName = surname,
                    Email = email,
                    Password = password,
                    IsEmployee = isEmployee,
                    CompanyName = companyName,
                    CompanyAddress = companyAddress,
                    CompanyLogo = companyLogo,
                    Online = true,
                };

                bool UserIsActive = await _writer.AddAccount(account);

                if (UserIsActive)
                {
                    Account = account;
                    return await _writer.SetCurrentUser(account);
                }


            }
            catch (Exception ex)
            {

                await _writer.HandleExceptionAsync(ex, "SignUp()");


                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);


            }

            return false;

        }

        public async Task<bool> SignIn(string email, string password)
        {
            try
            {
                var temporaryAccount = await _writer.FindAccountByEmail(email, password);

                if (temporaryAccount is not null)
                {
                    await _writer.UnsetCurrentUser(Account);
                    await _writer.UnsetCurrentUser(temporaryAccount);

                    Account = temporaryAccount;
                    Account.Online = true;

                    return await _writer.SetCurrentUser(Account);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

            }

            return false;

        }

        public async Task<bool> SignOut()
        {

            try
            {
                // TODO
                Account.Online = false;

                // Modify account status to show user is offline
                var UserIsActive = await _writer.UnsetCurrentUser(Account);

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
    }
}
