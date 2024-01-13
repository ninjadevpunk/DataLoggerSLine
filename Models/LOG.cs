using System.Text.RegularExpressions;

namespace Data_Logger_1._3.Models
{
    public abstract class LOG
    {
        /* ENUMS */


        protected enum CATEGORY {CODING, GRAPHICS, FILM, NOTES}




        /* PROPERTIES */


        protected abstract CATEGORY Category { get; }

        private ACCOUNT author = new ACCOUNT();
        public ACCOUNT Author
        {
            get { return author; }
            set { author = value; }
        }


        private string projectName = "Unknown";
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        private string applicationName = "Unknown";
        public string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }


        private DateTime startTime = DateTime.Now;
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }


        private DateTime endTime = DateTime.Now;
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private string output = "C# Application (*.exe)";
        public string Output
        {
            get { return output; }
            set { output = value; }
        }


        private string type = "NONE";
        public string Type
        {
            get { return type; }
            set { type = value; }
        }


        private List<PostIt> postItList;
        public List<PostIt> PostItList
        {
            get { return postItList; }
            set { postItList = value; }
        }



        /** CONSTRUCTOR */
        public LOG()
        {
            // Blank
        }

        protected LOG(ACCOUNT author, string projectName, string applicationName, DateTime startTime, DateTime endTime, string output, string type, List<PostIt> postItList)
        {
            this.author = author;
            this.projectName = projectName;
            this.applicationName = applicationName;
            this.startTime = startTime;
            this.endTime = endTime;
            this.output = output;
            this.type = type;
            this.postItList = postItList;
        }



        /* COUNT */

        /** Helper function **/
        public int helper(string type)
        {
            int count = 0;
            string pattern = "[A-Za-z]{5}[0-9]{0}";
            Regex xp = new Regex(pattern);

            foreach (PostIt n in postItList)
            {
                switch (type)
                {
                    case "error":
                        {
                            if (xp.IsMatch(n.Error))
                                ++count;
                        }
                        break;
                    case "solution":
                        {
                            if (xp.IsMatch(n.Solution))
                                ++count;
                        }
                        break;
                    case "suggestion":
                        {
                            if (xp.IsMatch(n.Suggestion))
                                ++count;
                        }
                        break;
                    case "comment":
                        {
                            if (xp.IsMatch(n.Comment))
                                ++count;
                        }
                        break;
                }
            }

            return count;
        }


        /** Count how many errors there are
         */
        public int errorCount()
        {
            return helper("error");
        }

        /** Count how many solutions there are
         */
        public int solutionCount()
        {
            return helper("solution");
        }

        /** Count how many suggestions there are
         */
        public int suggestionCount()
        {
            return helper("suggestion");
        }

        /** Count how many comments there are
         */
        public int commentCount()
        {
            return helper("comment");
        }



    }
}
