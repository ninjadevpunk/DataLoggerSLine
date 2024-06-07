using System.Runtime.Serialization;

namespace Data_Logger_1._3.Models
{

    /// <summary>
    /// The main authoring class. Represents Data Logger users.
    /// </summary>
    [DataContract]
    public class ACCOUNT
    {
        /* PROPERTIES */

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string ProfilePic { get; set; } = "/Assets/login/user.png";

        [DataMember]
        public string FirstName { get; set; } = "";

        [DataMember]
        public string LastName { get; set; } = "";

        [DataMember]
        public string Email { get; set; } = "";

        [DataMember]
        public string Password { get; set; } = "";

        [DataMember]
        public bool IsEmployee { get; set; } = false;

        [DataMember]
        public string? CompanyName { get; set; } = "";

        [DataMember]
        public string? CompanyAddress { get; set; } = "";

        [DataMember]
        public string? CompanyLogo { get; set; } = "";

        [DataMember]
        public bool Online { get; set; } = false;


        /* ---CONSTRUCTORS--- */


        public ACCOUNT()
        {
            // EMPTY
        }

        public ACCOUNT(int id, string profilePic, string firstName, string lastName, string email, string password, bool isEmplyee, string companyName, string companyAddress, string companyLogo, bool status)
        {
            ID = id;
            ProfilePic = profilePic;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            IsEmployee = isEmplyee;
            CompanyName = companyName;
            CompanyAddress = companyAddress;
            CompanyLogo = companyLogo;
            Online = status;
        }

        public ACCOUNT(int id, string firstName, string lastName, string email, string password, bool isEmplyee, string companyName, string companyAddress)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            IsEmployee = isEmplyee;
            CompanyName = companyName;
            CompanyAddress = companyAddress;
        }

        public ACCOUNT(int id, string firstName, string lastName, string email,  bool isEmplyee, string companyName, string companyAddress, string companyLogo, bool status)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsEmployee = isEmplyee;
            CompanyName = companyName;
            CompanyAddress = companyAddress;
            CompanyLogo = companyLogo;
            Online = status;
        }

        public ACCOUNT(int id, string firstName, string lastName, string email, string password, bool isEmplyee, string companyName, string companyAddress, string companyLogo, bool status)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            IsEmployee = isEmplyee;
            CompanyName = companyName;
            CompanyAddress = companyAddress;
            CompanyLogo = companyLogo;
            Online = status;
        }

        public ACCOUNT(int id, string firstName, string lastName, string password, bool status)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Online = status;
        }



        /* OVERLOADS */


        public override bool Equals(object? obj)
        {
            return obj is ACCOUNT aCCOUNT &&
                   ID == aCCOUNT.ID &&
                   ProfilePic == aCCOUNT.ProfilePic &&
                   FirstName == aCCOUNT.FirstName &&
                   LastName == aCCOUNT.LastName &&
                   Email == aCCOUNT.Email &&
                   Password == aCCOUNT.Password &&
                   IsEmployee == aCCOUNT.IsEmployee &&
                   CompanyName == aCCOUNT.CompanyName &&
                   CompanyAddress == aCCOUNT.CompanyAddress &&
                   CompanyLogo == aCCOUNT.CompanyLogo &&
                   Online == aCCOUNT.Online;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(ID);
            hash.Add(ProfilePic);
            hash.Add(FirstName);
            hash.Add(LastName);
            hash.Add(Email);
            hash.Add(Password);
            hash.Add(IsEmployee);
            hash.Add(CompanyName);
            hash.Add(CompanyAddress);
            hash.Add(CompanyLogo);
            hash.Add(Online);
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
