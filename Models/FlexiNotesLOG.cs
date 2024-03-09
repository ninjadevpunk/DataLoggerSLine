namespace Data_Logger_1._3.Models
{
    public class FlexiNotesLOG : NotesLOG
    {
        /* DOCUMENTATION
         * 
         * Please use this class for flexi notes!
         * 
         */

        /* ENUMS */

        public enum FLEXINOTEType { Document, Music, Gaming }
        public enum GAMINGContext { Create, Play}


        /* MEMBER VARIABLES */
        /** This is a flexi note.
         */
        protected override NOTELOGType notelogtype => NOTELOGType.FLEXI;

        public FLEXINOTEType flexinotetype { get; set; } = FLEXINOTEType.Document;

        public GAMINGContext gamingcontext { get; set; } = GAMINGContext.Create;

        public string Medium { get; set; }

        public string Format { get; set; }

        public int BitRate { get; set; } = 0;

        public string Length { get; set; } = "0:00";

        public bool IsCompleted { get; set; } = false;

        public string Source { get; set; } = @"C:\";



        /* CONSTRUCTORS */




        public FlexiNotesLOG()
        {
        }

        public FlexiNotesLOG(FLEXINOTEType flexinotetype, GAMINGContext gamingcontext, string medium, string format, int bitRate, 
            string length, bool isCompleted, string source)
        {
            this.flexinotetype = flexinotetype;
            this.gamingcontext = gamingcontext;
            Medium = medium;
            Format = format;
            BitRate = bitRate;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }

        public FlexiNotesLOG(ACCOUNT author, string projectName, string applicationName, DateTime startTime, DateTime endTime, 
            string output, string type, List<PostIt> postItList,
            FLEXINOTEType flexinotetype, GAMINGContext gamingcontext, string medium, string format, int bitRate,
            string length, bool isCompleted, string source) : base(author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            this.flexinotetype = flexinotetype;
            this.gamingcontext = gamingcontext;
            Medium = medium;
            Format = format;
            BitRate = bitRate;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }



        /* OVERLOADS */



        public override bool Equals(object? obj)
        {
            return obj is FlexiNotesLOG lOG &&
                   base.Equals(obj) &&
                   Category == lOG.Category &&
                   Author == lOG.Author &&
                   ProjectName == lOG.ProjectName &&
                   ApplicationName == lOG.ApplicationName &&
                   StartTime == lOG.StartTime &&
                   EndTime == lOG.EndTime &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   PostItList.Equals(lOG.PostItList) &&
                   notelogtype == lOG.notelogtype &&
                   flexinotetype == lOG.flexinotetype &&
                   gamingcontext == lOG.gamingcontext &&
                   Medium == lOG.Medium &&
                   Format == lOG.Format &&
                   BitRate == lOG.BitRate &&
                   Length == lOG.Length &&
                   IsCompleted == lOG.IsCompleted &&
                   Source == lOG.Source;
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
            hash.Add(flexinotetype);
            hash.Add(gamingcontext);
            hash.Add(Medium);
            hash.Add(Format);
            hash.Add(BitRate);
            hash.Add(Length);
            hash.Add(IsCompleted);
            hash.Add(Source);
            return hash.ToHashCode();
        }

        public static bool operator ==(FlexiNotesLOG left, FlexiNotesLOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FlexiNotesLOG left, FlexiNotesLOG right)
        {
            return !left.Equals(right);
        }
    }
}
