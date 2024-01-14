
namespace Data_Logger_1._3.Models
{
    public class NoteItem : NotesLOG
    {
        /* DOCUMENTATION
         * 
         * Please use this class for generic notes!
         * 
         */

        

        /* MEMBER VARIABLES */
        /** This is a generic note.
         */
        protected override NOTELOGType notelogtype => NOTELOGType.GENERIC;

        public string Subject { get; set; } = "No Subject";

        /** The note should be stored here.
         *  The checklist items must be stored in this variable and the items variable!
         *  */
        public string Content { get; set; } = "";

        /** Check list items must be stored here!
         *  Use the bool key to store whether an item has been checked or not.
         *  */
        public CheckList Items { get; set; } = new CheckList();



        /* CONSTRUCTORS */



        public NoteItem()
        {
        }

        // For Generic Notes
        public NoteItem(string subject, string genericNote)
        {
            Subject = subject;
            Content = genericNote;
        }

        // For Checklists
        public NoteItem(string subject, string genericNote, CheckList items)
        {
            Subject = subject;
            Content = genericNote;
            Items = items;
        }

        public NoteItem(ACCOUNT author, string projectName, string applicationName, DateTime startTime, DateTime endTime, 
            string output, string type, List<PostIt> postItList,
            string subject, string genericNote) : base(author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            Subject = subject;
            Content = genericNote;
        }
        
        public NoteItem(ACCOUNT author, string projectName, string applicationName, DateTime startTime, DateTime endTime, 
            string output, string type, List<PostIt> postItList,
            string subject, string genericNote, CheckList items) : base(author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            Subject = subject;
            Content = genericNote;
            Items = items;
        }


        /* OVERLOADS */



        public override bool Equals(object? obj)
        {
            return obj is NoteItem item &&
                   base.Equals(obj) &&
                   Category == item.Category &&
                   Author == item.Author &&
                   ProjectName == item.ProjectName &&
                   ApplicationName == item.ApplicationName &&
                   StartTime == item.StartTime &&
                   EndTime == item.EndTime &&
                   Output == item.Output &&
                   Type == item.Type &&
                   PostItList.Equals(item.PostItList) &&
                   notelogtype == item.notelogtype &&
                   Subject == item.Subject &&
                   Content == item.Content &&
                   Items == item.Items;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Category);
            hash.Add(Author);
            hash.Add(ProjectName);
            hash.Add(ApplicationName);
            hash.Add(StartTime);
            hash.Add(EndTime);
            hash.Add(Output);
            hash.Add(Type);
            hash.Add(PostItList);
            hash.Add(notelogtype);
            hash.Add(Subject);
            hash.Add(Content);
            hash.Add(Items);
            return hash.ToHashCode();
        }

        public static bool operator ==(NoteItem left, NoteItem right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NoteItem left, NoteItem right)
        {
            return !left.Equals(right);
        }




    }
}
