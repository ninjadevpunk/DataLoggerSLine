using Data_Logger_1._3.Models.App_Models;

using System.Text.RegularExpressions;

namespace Data_Logger_1._3.Models
{

    /// <summary>
    /// The parent class. Defines all common log properties.
    /// </summary>

    public abstract class LOG
    {
        /* ENUMS */


        /// <summary>
        /// Log categories. All LOGs can only be Coding, Graphics, 
        /// Film or Notes types.
        /// </summary>
        public enum CATEGORY { CODING, GRAPHICS, FILM, NOTES }





        /* PROPERTIES */






        public abstract CATEGORY Category { get; }

        /// <summary>
        /// The log's identifier. Important for database and caching operations.
        /// </summary>

        public int ID { get; set; } = -1;

        /// <summary>
        /// The creator of a log.
        /// </summary>

        public ACCOUNT Author { get; set; }

        /// <summary>
        /// The project that is associated with the log.
        /// </summary>

        public ProjectClass Project { get; set; }

        /// <summary>
        /// The application in which the project was created in.
        /// </summary>

        public ApplicationClass Application { get; set; }

        /// <summary>
        /// The start date and time of the project.
        /// </summary>

        public DateTime Start { get; set; } = DateTime.Now;

        /// <summary>
        /// The end date and time of the project.
        /// </summary>

        public DateTime End { get; set; } = DateTime.Now;

        /// <summary>
        /// The deliverable for the project.
        /// </summary>

        public OutputClass Output { get; set; }

        /// <summary>
        /// What type of build is created in this project.
        /// </summary>

        public TypeClass Type { get; set; }

        /// <summary>
        /// The most important property of the log. All the thoughts, 
        /// suggestions and details of the project are described in PostIts.
        /// </summary>

        public List<PostIt> PostItList { get; set; } = new();


        public string Content { get; set; } = string.Empty;


        /** CONSTRUCTOR */
        public LOG()
        {
            // Blank
        }

        protected LOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime, OutputClass output, TypeClass type, List<PostIt> postItList)
        {
            ID = id;
            Author = author;
            Project = projectName;
            Application = applicationName;
            Start = startTime;
            End = endTime;
            Output = output;
            Type = type;
            PostItList = postItList;
        }

        protected LOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime, OutputClass output, TypeClass type, List<PostIt> postItList, string content)
        {
            ID = id;
            Author = author;
            Project = projectName;
            Application = applicationName;
            Start = startTime;
            End = endTime;
            Output = output;
            Type = type;
            PostItList = postItList;
        }



        /* COUNT */

        /** Helper function **/
        public int helper(string type)
        {
            int count = 0;
            string pattern = "[A-Za-z]{5}[0-9]{0}";
            Regex xp = new Regex(pattern);

            foreach (PostIt n in PostItList)
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
        public int ErrorCount()
        {
            return helper("error");
        }

        /** Count how many solutions there are
         */
        public int SolutionCount()
        {
            return helper("solution");
        }

        /** Count how many suggestions there are
         */
        public int SuggestionCount()
        {
            return helper("suggestion");
        }

        /** Count how many comments there are
         */
        public int CommentCount()
        {
            return helper("comment");
        }

        public override bool Equals(object? obj)
        {
            return obj is LOG lOG &&
                   Category == lOG.Category &&
                   ID == lOG.ID &&
                   Author.Equals(lOG.Author) &&
                   Project == lOG.Project &&
                   Application == lOG.Application &&
                   Start == lOG.Start &&
                   End == lOG.End &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   EqualityComparer<List<PostIt>>.Default.Equals(PostItList, lOG.PostItList) &&
                   Content == lOG.Content;
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
            hash.Add(Content);
            return hash.ToHashCode();
        }

        public static bool operator ==(LOG left, LOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LOG left, LOG right)
        {
            return !left.Equals(right);
        }
    }
}
