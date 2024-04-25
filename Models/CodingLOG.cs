
namespace Data_Logger_1._3.Models
{
    public class CodingLOG : LOG
    {
        /* ENUMS */
        public override CATEGORY Category => CATEGORY.CODING;

        /* MEMBER VARIABLES */


        /** Store the number of bugs **/
        private int bugs = 0;
		public int Bugs
		{
			get { return bugs; }
			set { bugs = value; }
		}

        /** Store the application launch details here
         *  Did it execute?
         *  False by default. **/
        private bool success = false;
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }


        /* CONSTRUCTORS */

        public CodingLOG()
        {
            // Blank
        }

        public CodingLOG(int bugs, bool success)
        {
            this.bugs = bugs;
            this.success = success;
        }

        public CodingLOG(ACCOUNT author, string projectName, string applicationName, DateTime startTime, DateTime endTime, 
            string output, string type, List<PostIt> postItList, int bugs, bool success) : base(author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            this.bugs = bugs;
            this.success = success;
        }

        public override bool Equals(object? obj)
        {

            // TODO

            return obj is CodingLOG lOG &&
                   Category == lOG.Category &&
                   Author == lOG.Author &&
                   ProjectName == lOG.ProjectName &&
                   ApplicationName == lOG.ApplicationName &&
                   StartTime == lOG.StartTime &&
                   EndTime == lOG.EndTime &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   PostItList.Equals(lOG.PostItList) &&
                   Category == lOG.Category &&
                   bugs == lOG.bugs &&
                   Bugs == lOG.Bugs &&
                   success == lOG.success &&
                   Success == lOG.Success;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(bugs);
            hash.Add(success);
            return hash.ToHashCode();
        }

        public static bool operator ==(CodingLOG cLOG1, CodingLOG cLOG2)
        {
            return cLOG1.Equals(cLOG2);
        }

        public static bool operator !=(CodingLOG cLOG1, CodingLOG cLOG2)
        {
            return !cLOG1.Equals(cLOG2);
        }


        /* OVERLOADS */




    }
}
