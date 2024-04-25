
namespace Data_Logger_1._3.Models
{
    public class GraphicsLOG : LOG
    {
        /* ENUMS */
        public override CATEGORY Category => CATEGORY.GRAPHICS;

        /* MEMBER VARIABLES */
        public string Medium { get; set; } = "Pencil";

        public string Format { get; set; } = "Digital Canvas";

        public string Brush { get; set; } = "";

        public double Height { get; set; } = 0.0;

        public double Width { get; set; } = 0.0;

        public string Unit { get; set; } = "cm";

        public string Size { get; set; } = "A4";

        public double DPI { get; set; } = 0.0;

        public string Depth { get; set; } = "8-bit";

        public bool IsCompleted { get; set; } = false;

        public string Source { get; set; } = @"C:\";



        /* CONSTRUCTORS */




        public GraphicsLOG() { }

        public GraphicsLOG(string medium, string format, string brush, double height, double width, string unit, string size, double dPI, string depth, bool isCompleted, string source)
        {
            Medium = medium;
            Format = format;
            Brush = brush;
            Height = height;
            Width = width;
            Unit = unit;
            Size = size;
            DPI = dPI;
            Depth = depth;
            IsCompleted = isCompleted;
            Source = source;
        }

        public GraphicsLOG(ACCOUNT author, string projectName, string applicationName, DateTime startTime, DateTime endTime, 
            string output, string type, List<PostIt> postItList,
            string medium, string format, string brush, double height, double width, 
            string unit, string size, double dPI, string depth, bool isCompleted, string source) : base(author, projectName, applicationName, 
                                                                                                            startTime, endTime, output, type, postItList)
        {
            Medium = medium;
            Format = format;
            Brush = brush;
            Height = height;
            Width = width;
            Unit = unit;
            Size = size;
            DPI = dPI;
            Depth = depth;
            IsCompleted = isCompleted;
            Source = source;
        }


        /* OVERLOADS */




        public override bool Equals(object? obj)
        {
            return obj is GraphicsLOG lOG &&
                   Category == lOG.Category &&
                   Author == lOG.Author &&
                   ProjectName == lOG.ProjectName &&
                   ApplicationName == lOG.ApplicationName &&
                   StartTime == lOG.StartTime &&
                   EndTime == lOG.EndTime &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   PostItList.Equals(lOG.PostItList) &&
                   Medium == lOG.Medium &&
                   Format == lOG.Format &&
                   Brush == lOG.Brush &&
                   Height == lOG.Height &&
                   Width == lOG.Width &&
                   Unit == lOG.Unit &&
                   Size == lOG.Size &&
                   DPI == lOG.DPI &&
                   Depth == lOG.Depth &&
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
            hash.Add(Category);
            hash.Add(Medium);
            hash.Add(Format);
            hash.Add(Brush);
            hash.Add(Height);
            hash.Add(Width);
            hash.Add(Unit);
            hash.Add(Size);
            hash.Add(DPI);
            hash.Add(Depth);
            hash.Add(IsCompleted);
            hash.Add(Source);
            return hash.ToHashCode();
        }

        public static bool operator ==(GraphicsLOG left, GraphicsLOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GraphicsLOG left, GraphicsLOG right)
        {
            return !left.Equals(right);
        }
    }
}
