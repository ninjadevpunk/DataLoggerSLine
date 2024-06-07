
namespace Data_Logger_1._3.Models.App_Models
{
    public class SubjectClass
    {

        public int SubjectID { get; set; } = 1;

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public ACCOUNT User { get; set; }

        public string Subject { get; set; } = "";

        public ProjectClass Project { get; set; }

        public ApplicationClass Application { get; set; }

        public SubjectClass()
        {
        }

        public SubjectClass(int subjectID, LOG.CATEGORY category, ACCOUNT user, string subject, ProjectClass project, ApplicationClass application)
        {
            SubjectID = subjectID;
            Category = category;
            User = user;
            Subject = subject;
            Project = project;
            Application = application;
        }

        public override bool Equals(object? obj)
        {
            return obj is SubjectClass @class &&
                   SubjectID == @class.SubjectID &&
                   Category == @class.Category &&
                   User == @class.User &&
                   Subject == @class.Subject &&
                   Project.Equals(@class.Project) &&
                   Application.Equals(@class.Application);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SubjectID,Category, User, Subject, Project, Application);
        }

        public static bool operator ==(SubjectClass left, SubjectClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SubjectClass left, SubjectClass right)
        {
            return !left.Equals(right);
        }
    }
}
