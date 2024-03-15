using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using System.Windows;
using System.Security.Cryptography;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;
using Firebase.Database;
using Firebase.Database.Query;
using Data_Logger_1._3.Models;
using System.Diagnostics;
using System.Net;

namespace Data_Logger_1._3.Services
{
    public class AuthService
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;
        private readonly FirebaseClient _firebaseDatabase;
        private readonly string _domain;

        private const string clientID = "983908719391-gmq0iv9fsm8s8j1qlmqh873bq4529kcn.apps.googleusercontent.com";
        private const string redirectUri = "https://dls03-d1959.firebaseapp.com/__/auth/handler";
        private const string scopes = "email profile";
        private const string responseType = "code";

        public ACCOUNT Account { get; set; }

        public AuthService(string firebaseApiKey, string firebaseDomain)
        {
            // Initialize FirebaseAuthClient with provided Firebase API key and domain
            _firebaseAuthClient = new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = firebaseApiKey,
                AuthDomain = firebaseDomain,
                Providers = new FirebaseAuthProvider[]
                {
                new GoogleProvider().AddScopes("email", "profile"),
                new EmailProvider(),
                new MicrosoftProvider(),
                },
                UserRepository = new FileUserRepository("Data Logger"),
            });

            _firebaseDatabase = new FirebaseClient("https://dls03-d1959-default-rtdb.europe-west1.firebasedatabase.app/");
            _domain = firebaseDomain;
        }

        public bool SignUp(string email, string password, string displayName, string surname, bool IsEmployee, string companyName, string companyAddress)
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
                    //
                }


                return false;
            }

            // Call Firebase AuthClient or other authentication provider to create a new user
            try
            {


                var userCredential = _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);

                Account = new ACCOUNT(displayName, surname, email, IsEmployee, companyName, companyAddress, "", true);

                userCredential.Wait();

                _firebaseDatabase.Child("Accounts").Child(_firebaseAuthClient.User.Uid).PostAsync(Account).Wait();


            }
            catch (Exception e)
            {
                Console.WriteLine("The error is: " + e.Message);


                try
                {
                    _firebaseAuthClient.User.DeleteAsync().Wait();
                    _firebaseDatabase.Child("Feedback").Child(DateTime.Now.ToLongDateString()).PostAsync($"An error occured for user {_firebaseAuthClient.User.Uid}. " +
                        $"Error message: {e.Message}").Wait();
                }
                catch (Exception)
                {
                    //
                }

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                return false;

            }

            // Handle the result, update UI, or navigate to another view if needed
            return true;
        }

        public bool SignIn(string email, string password)
        {
            try
            {

                var userCredential = _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
                userCredential.Wait();

                // Retrieve Account data from Firebase here e.g. _firebaseDatabase.Child()...

            }
            catch (Exception)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                return false;
            }

            return true;
        
        }

        public async Task<string> GoogleSignIn()
        {

            try
            {
                // TODO

                // sign in via provider specific AuthCredential
                const string authUrl = $"https://accounts.google.com/o/oauth2/auth?client_id={clientID}&redirect_uri={redirectUri}&scope={scopes}&response_type={responseType}";

                // Open the user's default web browser to the authorization URL
                Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });
            }
            catch (Exception)
            {
                //return null;
                //
            }

            return await HandleGoogleCallback();
        }

        public async Task<string> HandleGoogleCallback()
        {
            // Set up a local HTTP server to listen for the redirect
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(redirectUri + "/");
                listener.Start();

                // Wait for the incoming request
                var context = await listener.GetContextAsync();

                // Extract the authorization code from the query parameters
                var code = context.Request.QueryString["code"];

                // Return the authorization code
                return code;
            }
        }

        public void ForgotPasswordRequest()
        {
            //
        }


        // HELPERS

        public static string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }


        }

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
