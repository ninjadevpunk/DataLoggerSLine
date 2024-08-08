
using Data_Logger_1._3.Models.App_Models;


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
        public override CATEGORY Category => CATEGORY.NOTES;

        // Generic Note or Flexi Note
        public enum NOTELOGType { GENERIC, FLEXI }


        /* PROPERTIES */




        public abstract NOTELOGType notelogtype { get; }



        /* CONSTRUCTORS */



        public NotesLOG()
        {

        }


        public NotesLOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList) : base(id, author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
        }

        public override bool Equals(object? obj)
        {
            return obj is NotesLOG lOG &&
                   Category == lOG.Category &&
                   ID == lOG.ID &&
                   Author == lOG.Author &&
                   Project == lOG.Project &&
                   Application == lOG.Application &&
                   Start == lOG.Start &&
                   End == lOG.End &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   PostItList.Equals(lOG.PostItList);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Category);
            hash.Add(ID);
            hash.Add(Author);
            hash.Add(Project);
            hash.Add(Application);
            hash.Add(Start);
            hash.Add(End);
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
