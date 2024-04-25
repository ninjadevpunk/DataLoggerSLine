
namespace Data_Logger_1._3.Models.App_Models
{
    public class ProjectClass
    {

        public int ProjectID { get; set; }

        public ACCOUNT User { get; set; }

        public string Name { get; set; } = "Unknown";

        public ApplicationClass Application{ get; set; } = new();

        public LOG.CATEGORY Category { get; set; }

        public bool IsDefault { get; set; } = false;




        public ProjectClass()
        {
        }

        public ProjectClass(int projectID, string name, ApplicationClass application)
        {
            ProjectID = projectID;
            Name = name;
            Application= application;
            Category = LOG.CATEGORY.CODING;
        }



        public ProjectClass(int projectID, ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category)
        {
            ProjectID = projectID;
            User = user;
            Name = name;
            Application = application;
            Category = category;
        }

        public ProjectClass(int projectID, ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category, bool isDefault)
        {
            ProjectID = projectID;
            User = user;
            Name = name;
            Application = application;
            Category = category;
            IsDefault = isDefault;
        }

        public override bool Equals(object? obj)
        {
            return obj is ProjectClass @class &&
                   ProjectID == @class.ProjectID &&
                   User == @class.User &&
                   IsDefault == @class.IsDefault &&
                   Name == @class.Name &&
                   Application == @class.Application &&
                   Category == @class.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProjectID, User, Name, Application, Category);
        }

        public static bool operator ==(ProjectClass left, ProjectClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ProjectClass left, ProjectClass right)
        {
            return !left.Equals(right);
        }
    }
}
