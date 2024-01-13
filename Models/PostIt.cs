
namespace Data_Logger_1._3.Models
{
    public class PostIt
    {
        /* MEMBER VARIABLES */


        /** What is the subject? 
         *  Usually to describe an error like unaligned widgets in a GUI **/

        private string subject = "No Subject";
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }


        private string error = "";
        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        private string solution = "";
        public string Solution
        {
            get { return solution; }
            set { solution = value; }
        }

        private string suggestion = "";
        public string Suggestion
        {
            get { return suggestion; }
            set { suggestion = value; }
        }

        private string comment = "";
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }


        /** When was the particular note category created.
         *  Ensure the time of the particular category is 
         *  captured and not the time for all the notes! **/
        private DateTime ercapturetime = new DateTime();
        public DateTime ERCaptureTime
        {
            get { return ercapturetime; }
            set { ercapturetime = value; }
        }

        private DateTime socapturetime = new DateTime();
        public DateTime SOCaptureTime
        {
            get { return socapturetime; }
            set { socapturetime = value; }
        }



        /* CONSTRUCTORS */
        public PostIt()
        {
            // EMPTY
        }

        public PostIt(string subject, string error, string solution, string suggestion, string comment)
        {
            this.subject = subject;
            this.error = error;
            this.solution = solution;
            this.suggestion = suggestion;
            this.comment = comment;
        }

        public PostIt(string subject, string error, string solution, string suggestion, string comment, DateTime ercapturetime, DateTime socapturetime)
        {
            this.subject = subject;
            this.error = error;
            this.solution = solution;
            this.suggestion = suggestion;
            this.comment = comment;
            this.ercapturetime = ercapturetime;
            this.socapturetime = socapturetime;
        }


        /* OVERLOADS */



        public override bool Equals(object? obj)
        {
            return obj is PostIt it &&
                   subject == it.subject &&
                   Subject == it.Subject &&
                   error == it.error &&
                   Error == it.Error &&
                   solution == it.solution &&
                   Solution == it.Solution &&
                   suggestion == it.suggestion &&
                   Suggestion == it.Suggestion &&
                   comment == it.comment &&
                   Comment == it.Comment &&
                   ercapturetime == it.ercapturetime &&
                   ERCaptureTime == it.ERCaptureTime &&
                   socapturetime == it.socapturetime &&
                   SOCaptureTime == it.SOCaptureTime;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(subject);
            hash.Add(Subject);
            hash.Add(error);
            hash.Add(Error);
            hash.Add(solution);
            hash.Add(Solution);
            hash.Add(suggestion);
            hash.Add(Suggestion);
            hash.Add(comment);
            hash.Add(Comment);
            hash.Add(ercapturetime);
            hash.Add(ERCaptureTime);
            hash.Add(socapturetime);
            hash.Add(SOCaptureTime);
            return hash.ToHashCode();
        }

        public static bool operator ==(PostIt left, PostIt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PostIt left, PostIt right)
        {
            return !(left == right);
        }
    }
}
