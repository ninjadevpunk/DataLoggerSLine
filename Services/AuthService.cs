using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using System.Windows;
using System.Security.Cryptography;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace Data_Logger_1._3.Services
{
    public class AuthService
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;

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
        }

        public bool SignUp(string email, SecureString password, string displayName)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(email) || password is null || password.Length <= 5)
            {
                return false;
            }

            // Call Firebase AuthClient or other authentication provider to create a new user
            try
            {

                var pass = sha526_hash(SecureStringToString(password), _firebaseAuthClient.User.Uid);

                var userCredential = _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, pass, displayName);
                userCredential.Wait();


            }
            catch (Exception)
            {

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                return false;
            }

            // Handle the result, update UI, or navigate to another view if needed

            return true;
        }

        public bool SignIn(string email, SecureString password)
        {
            try
            {
                var pass = sha526_hash(SecureStringToString(password), _firebaseAuthClient.User.Uid);

                var userCredential = _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, pass);
                userCredential.Wait();

            }
            catch (Exception)
            {
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);

                return false;
            }

            return true;
        
        }

        public bool GoogleSignIn()
        {

            try
            {
                // TODO
                //var userCredential = _firebaseAuthClient.SignInWithRedirectAsync(FirebaseProviderType.Google);
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

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
