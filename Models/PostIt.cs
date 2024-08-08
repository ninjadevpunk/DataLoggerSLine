
using Data_Logger_1._3.Models.App_Models;


namespace Data_Logger_1._3.Models
{


    public class PostIt
    {
        /* MEMBER VARIABLES */




        /// <summary>
        /// The PostIt's identifier.
        /// </summary>

        public int ID { get; set; } = -1;


        /// <summary>
        /// The PostIt's subject. Gives context to the PostIt's content.
        /// </summary>

        public SubjectClass Subject { get; set; }

        /// <summary>
        /// The error section. For explaining bug occurences or problems that 
        /// need to be addressed in a project.
        /// </summary>

        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// The solution section. Strictly in relationship with the Error 
        /// property. Explains how to solve a previously mentioned error.
        /// </summary>

        public string Solution { get; set; } = string.Empty;

        /// <summary>
        /// The suggestion section. For giving suggestions on errors or 
        /// solutions or even general project advice or possible ideas.
        /// </summary>

        public string Suggestion { get; set; } = string.Empty;

        /// <summary>
        /// The comment section, somewhat akin to suggestions, provides a 
        /// user the option to write down thoughts on errors, solutions 
        /// and suggestions. Comments can also be carefree and off-topic 
        /// to allow more freedom for the user.
        /// </summary>

        public string Comment { get; set; } = string.Empty;


        /// <summary>
        /// When was the particular note category created. This only captures 
        /// the error's "found" date and time. The other PostIt field edit times will 
        /// not be recorded here.
        /// </summary>

        public DateTime ERCaptureTime { get; set; }


        /// <summary>
        /// When the solution was added. Like the error date time capture, 
        /// this field only records when a solution was found.
        /// </summary>

        public DateTime SOCaptureTime { get; set; }



        /* CONSTRUCTORS */
        public PostIt()
        {
            // EMPTY
        }

        public PostIt(int id, SubjectClass subject, string error, string solution, string suggestion, string comment)
        {
            ID = id;
            Subject = subject;
            Error = error;
            Solution = solution;
            Suggestion = suggestion;
            Comment = comment;
        }

        public PostIt(int id, SubjectClass subject, string error, string solution, string suggestion, string comment, DateTime ercapturetime, DateTime socapturetime)
        {
            ID = id;
            Subject = subject;
            Error = error;
            Solution = solution;
            Suggestion = suggestion;
            Comment = comment;
            ERCaptureTime = ercapturetime;
            SOCaptureTime = socapturetime;
        }


        /* OVERLOADS */



        public override bool Equals(object? obj)
        {
            return obj is PostIt it &&
                   ID == it.ID &&
                   Subject == it.Subject &&
                   Error == it.Error &&
                   Solution == it.Solution &&
                   Suggestion == it.Suggestion &&
                   Comment == it.Comment &&
                   ERCaptureTime == it.ERCaptureTime &&
                   SOCaptureTime == it.SOCaptureTime;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(ID);
            hash.Add(Subject);
            hash.Add(Error);
            hash.Add(Solution);
            hash.Add(Suggestion);
            hash.Add(Comment);
            hash.Add(ERCaptureTime);
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
