
namespace Data_Logger_1._3.Models.App_Models
{
    public class OutputClass
    {
        public int OutputID { get; set; } = 1;

        public ACCOUNT User { get; set; }

        public string Name { get; set; } = "Console Application";

        public ApplicationClass Application { get; set; }

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public OutputClass()
        {            
        }

        public OutputClass(int outputID, ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category)
        {
            OutputID = outputID;
            User = user;
            Name = name;
            Application = application;
            Category = category;
        }

        public override bool Equals(object? obj)
        {
            return obj is OutputClass @class &&
                   OutputID == @class.OutputID &&
                   User.Equals(@class.User) &&
                   Name == @class.Name &&
                   Application.Equals(@class.Application) &&
                   Category == @class.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OutputID, User, Name, Application, Category);
        }

        public static bool operator ==(OutputClass left, OutputClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OutputClass left, OutputClass right)
        {
            return !left.Equals(right);
        }
    }
}
