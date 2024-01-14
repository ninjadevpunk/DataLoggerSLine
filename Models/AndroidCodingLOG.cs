
namespace Data_Logger_1._3.Models
{
    public class AndroidCodingLOG : CodingLOG
    {
        /* DOCUMENTATION 
            
        Please use this class for Android Studio projects. It will be more useful.
         
         */

        /* ENUMS */
        public enum SCOPE { FULL, SIMPLE }


        /* MEMBER VARIABLES */



        /** Store the selection between a simple Android LOG and full Android LOG **/
        public SCOPE Scope { get; set; } = SCOPE.FULL;

        /** Store the sync times as well as times for the build 
         * 
         * Sync/Build
         * Starting Gradle Daemon
         * Run Build
         * Load Build 
         * Configure Build 
         * allProjects 
         * 
         * */

        public DateTime Sync { get; set; } = new DateTime();

        public DateTime StartingGradleDaemon { get; set; }

        public DateTime RunBuild { get; set; }

        public DateTime LoadBuild { get; set; }

        public DateTime ConfigureBuild { get; set; }

        public DateTime AllProjects { get; set; }


        
        /* CONSTRUCTORS */



        public AndroidCodingLOG()
        {
        }

        public AndroidCodingLOG(SCOPE scope, DateTime sync, DateTime startingGradleDaemon, 
            DateTime runBuild, DateTime loadBuild, DateTime configureBuild, DateTime allProjects)
        {
            Scope = scope;
            Sync = sync;
            StartingGradleDaemon = startingGradleDaemon;
            RunBuild = runBuild;
            LoadBuild = loadBuild;
            ConfigureBuild = configureBuild;
            AllProjects = allProjects;
        }

        public AndroidCodingLOG(int bugs, bool success,
            SCOPE scope, DateTime sync, DateTime startingGradleDaemon,
            DateTime runBuild, DateTime loadBuild, DateTime configureBuild, DateTime allProjects) : base(bugs, success)
        {
            Scope = scope;
            Sync = sync;
            StartingGradleDaemon = startingGradleDaemon;
            RunBuild = runBuild;
            LoadBuild = loadBuild;
            ConfigureBuild = configureBuild;
            AllProjects = allProjects;
        }

        public AndroidCodingLOG(ACCOUNT author, string projectName, string applicationName, DateTime startTime, DateTime endTime, 
            string output, string type, List<PostIt> postItList, int bugs, bool success,
            SCOPE scope, DateTime sync, DateTime startingGradleDaemon,
            DateTime runBuild, DateTime loadBuild, DateTime configureBuild, DateTime allProjects) : base(author, projectName, applicationName, startTime, endTime, output, type, postItList, bugs, success)
        {
            Scope = scope;
            Sync = sync;
            StartingGradleDaemon = startingGradleDaemon;
            RunBuild = runBuild;
            LoadBuild = loadBuild;
            ConfigureBuild = configureBuild;
            AllProjects = allProjects;
        }



        /* OVERLAODS */



        public override bool Equals(object? obj)
        {
            return obj is AndroidCodingLOG lOG &&
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
                   Bugs == lOG.Bugs &&
                   Success == lOG.Success &&
                   Scope == lOG.Scope &&
                   Sync == lOG.Sync &&
                   StartingGradleDaemon == lOG.StartingGradleDaemon &&
                   RunBuild == lOG.RunBuild &&
                   LoadBuild == lOG.LoadBuild &&
                   ConfigureBuild == lOG.ConfigureBuild &&
                   AllProjects == lOG.AllProjects;
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
            hash.Add(Bugs);
            hash.Add(Success);
            hash.Add(Scope);
            hash.Add(Sync);
            hash.Add(StartingGradleDaemon);
            hash.Add(RunBuild);
            hash.Add(LoadBuild);
            hash.Add(ConfigureBuild);
            hash.Add(AllProjects);
            return hash.ToHashCode();
        }

        public static bool operator ==(AndroidCodingLOG left, AndroidCodingLOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AndroidCodingLOG left, AndroidCodingLOG right)
        {
            return !left.Equals(right);
        }
    }
}
