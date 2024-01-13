using System.Security;

namespace Data_Logger_1._3.Models
{
    public class ACCOUNT
    {
        /* PROPERTIES */


        public string ProfilePic { get; set; } = "/Assets/login/user.png";

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";

        public SecureString Password { get; set; } = new SecureString();

        public bool IsEmplyee { get; set; } = false;

        public string CompanyName { get; set; } = "";

        public string CompanyAddress { get; set; } = "";

        public string CompanyLogo { get; set; } = "";

        public bool Status { get; set; } = false;


        /* ---CONSTRUCTORS--- */


        public ACCOUNT()
        {
            // EMPTY
        }

        public ACCOUNT(string profilePic, string firstName, string lastName, string email, SecureString password, bool isEmplyee, string companyName, string companyAddress, string companyLogo, bool status)
        {
            ProfilePic = profilePic;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            IsEmplyee = isEmplyee;
            CompanyName = companyName;
            CompanyAddress = companyAddress;
            CompanyLogo = companyLogo;
            Status = status;
        }

        public ACCOUNT(string firstName, string lastName, SecureString password, bool status)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Status = status;
        }



        /* OVERLOADS */


        public override bool Equals(object? obj)
        {
            return obj is ACCOUNT aCCOUNT &&
                   ProfilePic == aCCOUNT.ProfilePic &&
                   FirstName == aCCOUNT.FirstName &&
                   LastName == aCCOUNT.LastName &&
                   Email == aCCOUNT.Email &&
                   Password == aCCOUNT.Password &&
                   IsEmplyee == aCCOUNT.IsEmplyee &&
                   CompanyName == aCCOUNT.CompanyName &&
                   CompanyAddress == aCCOUNT.CompanyAddress &&
                   CompanyLogo == aCCOUNT.CompanyLogo &&
                   Status == aCCOUNT.Status;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(ProfilePic);
            hash.Add(FirstName);
            hash.Add(LastName);
            hash.Add(Email);
            hash.Add(Password);
            hash.Add(IsEmplyee);
            hash.Add(CompanyName);
            hash.Add(CompanyAddress);
            hash.Add(CompanyLogo);
            hash.Add(Status);
            return hash.ToHashCode();
        }

        public static bool operator ==(ACCOUNT left, ACCOUNT right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ACCOUNT left, ACCOUNT right)
        {
            return !left.Equals(right);
        }




    }
}
