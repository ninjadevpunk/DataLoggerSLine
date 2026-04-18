using Data_Logger_1._3.Models;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Data_Logger_1._3.Services
{
    public class AuthService
    {
        private readonly IServiceProvider _serviceProvider;

        public ACCOUNT? Account { get; set; }

        public AuthService()
        {


        }

        public AuthService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> SignUp(string dp, string email, string password, string displayName, string surname,
            bool isEmployee, string companyName, string companyAddress, string companyLogo)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            // Validate input
            // TODO
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
                    var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                    await writer.HandleExceptionAsync(ex, "SignUp(dp,email,password,displayName,surname,isEmployee,companyName,companyAddress,companyLogo)");
                }


                return false;
            }

            try
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
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
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex, "SignUp(dp,email,password,displayName,surname,isEmployee,companyName,companyAddress,companyLogo)");

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);


            }

            return false;

        }

        public async Task<bool> SignIn(string email, string password)
        {
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {


                var (userFound, temporaryAccount) = await dataService.SignInUser(email, password);
                if (!userFound)
                    return false;


                Account = temporaryAccount;

                return userFound;
            }
            catch (Exception ex)
            {

                await dataService.HandleExceptionAsync(ex, "SignIn(email, password)");

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
