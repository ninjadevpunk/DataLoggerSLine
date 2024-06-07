
using Data_Logger_1._3.Models.App_Models;
using System.Runtime.Serialization;

namespace Data_Logger_1._3.Models
{

    /// <summary>
    /// The primary LOG sub class. The Coding class has the most Data Logger support 
    /// and is used for coding projects in any coding app - 
    /// especially Qt Creator and Android Studio.
    /// </summary>
    [DataContract]
    public class CodingLOG : LOG
    {
        /* ENUMS */
        public override CATEGORY Category => CATEGORY.CODING;

        /* MEMBER VARIABLES */


        /// <summary>
        /// Store the number of bugs found.
        /// </summary>
        [DataMember]
        public int Bugs { get; set; } = 0;

        /// <summary>
        /// Store the application launch details here like whether the app opened or not.
        /// </summary>
        [DataMember]
        public bool Success { get; set; } = false;


        /* CONSTRUCTORS */

        public CodingLOG()
        {
            // Blank
        }

        public CodingLOG(int bugs, bool success)
        {
            Bugs = bugs;
            Success = success;
        }

        public CodingLOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime, 
            OutputClass output, TypeClass type, List<PostIt> postItList, int bugs, bool success) : base(id, author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            Bugs = bugs;
            Success = success;
        }

        public override bool Equals(object? obj)
        {

            // TODO

            return obj is CodingLOG lOG &&
                   Category == lOG.Category &&
                   ID == lOG.ID &&
                   Author == lOG.Author &&
                   Project == lOG.Project &&
                   Application == lOG.Application &&
                   StartTime == lOG.StartTime &&
                   EndTime == lOG.EndTime &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   PostItList.Equals(lOG.PostItList) &&
                   Category == lOG.Category &&
                   Bugs == lOG.Bugs &&
                   Success == lOG.Success;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Bugs);
            hash.Add(Success);
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
