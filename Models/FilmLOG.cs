using Data_Logger_1._3.Models.App_Models;
using System.Runtime.Serialization;

namespace Data_Logger_1._3.Models
{

    /// <summary>
    /// The LOG class for video projects.
    /// </summary>
    [DataContract]
    public class FilmLOG : LOG
    {

        /* ENUMS */
        public override CATEGORY Category => CATEGORY.FILM;



        /* MEMBER VARIABLES */


        /// <summary>
        /// The height of the footage.
        /// </summary>
        [DataMember]
        public double Height { get; set; } = 0.0;

        /// <summary>
        /// The width of the footage.
        /// </summary>
        [DataMember]
        public double Width { get; set; } = 0.0;

        /// <summary>
        /// The length of the footage. Please use video time format. e.g. 00:00
        /// </summary>
        [DataMember]
        public string Length { get; set; } = "0:00";

        /// <summary>
        /// Whether the video has been completed or filmed.
        /// </summary>
        [DataMember]
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// The location of the video regardless of it being online or not.
        /// </summary>
        [DataMember]
        public string Source { get; set; } = @"C:\";


        /* CONSTRUCTORS */




        public FilmLOG()
        {
        }

        public FilmLOG(double height, double width, string length, bool isCompleted, string source)
        {
            Height = height;
            Width = width;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }

        public FilmLOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList,
            double height, double width, string length, bool isCompleted, string source) : base(id, author, projectName, applicationName,
                                                                                                            startTime, endTime, output, type, postItList)
        {
            Height = height;
            Width = width;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }



        /* OVERLOADS */




        public override bool Equals(object? obj)
        {
            return obj is FilmLOG lOG &&
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
                   Height == lOG.Height &&
                   Width == lOG.Width &&
                   Length == lOG.Length &&
                   IsCompleted == lOG.IsCompleted &&
                   Source == lOG.Source;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Category);
            hash.Add(ID);
            hash.Add(Author);
            hash.Add(Project);
            hash.Add(Application);
            hash.Add(StartTime);
            hash.Add(EndTime);
            hash.Add(Output);
            hash.Add(Type);
            hash.Add(PostItList);
            hash.Add(Height);
            hash.Add(Width);
            hash.Add(Length);
            hash.Add(IsCompleted);
            hash.Add(Source);
            return hash.ToHashCode();
        }

        public static bool operator ==(FilmLOG left, FilmLOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FilmLOG left, FilmLOG right)
        {
            return !left.Equals(right);
        }
    }
}
