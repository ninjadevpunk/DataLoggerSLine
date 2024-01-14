
namespace Data_Logger_1._3.Models
{
    public abstract class NotesLOG : LOG
    {
        /* DOCUMENTATION
         * 
         * Please use this class for flexi notes or generic notes!
         */


        /* ENUMS */

        // Log Category
        protected override CATEGORY Category => CATEGORY.NOTES;

        // Generic Note or Flexi Note
        protected enum NOTELOGType { GENERIC, FLEXI }


        /* PROPERTIES */



        protected abstract NOTELOGType notelogtype { get; }



        /* CONSTRUCTORS */



        public NotesLOG()
        {
            
        }
        

        public NotesLOG(ACCOUNT author, string projectName, string applicationName, DateTime startTime, DateTime endTime, 
            string output, string type, List<PostIt> postItList) : base(author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
        }

        public override bool Equals(object? obj)
        {
            return obj is NotesLOG lOG &&
                   Category == lOG.Category &&
                   Author == lOG.Author &&
                   ProjectName == lOG.ProjectName &&
                   ApplicationName == lOG.ApplicationName &&
                   StartTime == lOG.StartTime &&
                   EndTime == lOG.EndTime &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   PostItList.Equals(lOG.PostItList);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Category);
            hash.Add(Author);
            hash.Add(ProjectName);
            hash.Add(ApplicationName);
            hash.Add(StartTime);
            hash.Add(EndTime);
            hash.Add(Output);
            hash.Add(Type);
            hash.Add(PostItList);
            return hash.ToHashCode();
        }

        public static bool operator ==(NotesLOG left, NotesLOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NotesLOG left, NotesLOG right)
        {
            return !left.Equals(right);
        }
    }
}
