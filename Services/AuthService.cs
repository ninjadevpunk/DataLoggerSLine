using Data_Logger_1._3.Models;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Data_Logger_1._3.Services
{
    public class AuthService
    {
        private readonly IServiceProvider _serviceProvider;

        public ACCOUNT? Account { get; set; } = new();

        public AuthService()
        {


        }

        public AuthService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public void SetDisplayPic(string source)
        {
            Account.ProfilePic = source;
        }

        public async Task<bool> SignUp(string dp, string email, string password, string displayName, string surname, 
            bool isEmployee, string companyName, string companyAddress, string companyLogo)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

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
                    await master.HandleExceptionAsync(ex, "SignUp(dp,email,password,displayName,surname,isEmployee,companyName,companyAddress,companyLogo)");
                }


                return false;
            }

            try
            {
                var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();

                await writer.UnsetCurrentUser();

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
                    IsOnline = true,
                };

                bool UserIsActive = await writer.AddAccount(account);

                if (UserIsActive)
                {
                    Account = account;
                    return await writer.SetCurrentUser(account);
                }

            }
            catch (Exception ex)
            {

                await master.HandleExceptionAsync(ex, "SignUp(dp,email,password,displayName,surname,isEmployee,companyName,companyAddress,companyLogo)");

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);


            }

            return false;

        }

        public async Task<bool> SignIn(string email, string password)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();

                var temporaryAccount = await writer.FindAccountByEmail(email, password);
                if (temporaryAccount is null)
                    return false;

                temporaryAccount.IsOnline = true;
                var ok = await writer.SetCurrentUser(temporaryAccount);

                if (ok)
                    Account = temporaryAccount;

                return ok;
            }
            catch (Exception ex)
            {

                await master.HandleExceptionAsync(ex, "SignIn(email, password)");

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

            }

            return false;
        }

        public void ForgotPasswordRequest()
        {
            //
        }

        public void SignOut()
        {
            Account = null;
        }
    }
}
