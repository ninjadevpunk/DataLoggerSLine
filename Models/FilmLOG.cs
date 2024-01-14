namespace Data_Logger_1._3.Models
{
    public class FilmLOG : LOG
    {

        /* ENUMS */
        protected override CATEGORY Category => CATEGORY.FILM;



        /* MEMBER VARIABLES */



        public double Height { get; set; } = 0.0;

        public double Width { get; set; } = 0.0;

        public string Unit { get; set; } = "";

        public string Length { get; set; } = "0:00";

        public bool IsCompleted { get; set; } = false;

        public string Source { get; set; } = @"C:\";


        /* CONSTRUCTORS */




        public FilmLOG()
        {
        }

        public FilmLOG(double height, double width, string unit, string length, bool isCompleted, string source)
        {
            Height = height;
            Width = width;
            Unit = unit;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }

        public FilmLOG(ACCOUNT author, string projectName, string applicationName, DateTime startTime, DateTime endTime,
            string output, string type, List<PostIt> postItList,
            double height, double width, string unit, string length, bool isCompleted, string source) : base(author, projectName, applicationName,
                                                                                                            startTime, endTime, output, type, postItList)
        {
            Height = height;
            Width = width;
            Unit = unit;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }



        /* OVERLOADS */




        public override bool Equals(object? obj)
        {
            return obj is FilmLOG lOG &&
                   Category == lOG.Category &&
                   Author == lOG.Author &&
                   ProjectName == lOG.ProjectName &&
                   ApplicationName == lOG.ApplicationName &&
                   StartTime == lOG.StartTime &&
                   EndTime == lOG.EndTime &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   PostItList.Equals(lOG.PostItList) &&
                   Height == lOG.Height &&
                   Width == lOG.Width &&
                   Unit == lOG.Unit &&
                   Length == lOG.Length &&
                   IsCompleted == lOG.IsCompleted &&
                   Source == lOG.Source;
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
            hash.Add(Height);
            hash.Add(Width);
            hash.Add(Unit);
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
