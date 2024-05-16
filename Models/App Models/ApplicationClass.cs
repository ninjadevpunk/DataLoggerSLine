
namespace Data_Logger_1._3.Models.App_Models
{
    public class ApplicationClass
    {
        public int AppID { get; set; }

        public ACCOUNT User { get; set; } = new();

        public string Name { get; set; } = "Unknown";

        public LOG.CATEGORY Category { get; set; }

        public bool IsDefault { get; set; } = false;


        public ApplicationClass()
        {
        }

        public ApplicationClass(string applicationName)
        {
            AppID = -1;
            Name = applicationName;
        }

        public ApplicationClass(int appID, string name)
        {
            AppID = appID;
            Name = name;
        }

        public ApplicationClass(int appID, string name, LOG.CATEGORY category)
        {
            AppID = appID;
            Name = name;
            Category = category;
        }

        public ApplicationClass(int appID, ACCOUNT user, string name, LOG.CATEGORY category)
        {
            AppID = appID;
            User = user;
            Name = name;
            Category = category;
        }

        public ApplicationClass(int appID, ACCOUNT user, string name, LOG.CATEGORY category, bool isDefault)
        {
            AppID = appID;
            User = user;
            Name = name;
            Category = category;
            IsDefault = isDefault;
        }

        public override bool Equals(object? obj)
        {
            return obj is ApplicationClass @class &&
                   AppID == @class.AppID &&
                   Name == @class.Name &&
                   Category == @class.Category &&
                   IsDefault == @class.IsDefault;
        }

        public static bool operator ==(ApplicationClass left, ApplicationClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ApplicationClass left, ApplicationClass right)
        {
            return !left.Equals(right);
        }
    }
}
