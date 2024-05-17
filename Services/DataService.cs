using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Database;
using Firebase.Database.Query;

namespace Data_Logger_1._3.Services
{

    public enum ProjectsCodingBRANCH {
        Qt,
        Android,
        Generic
    }

    public enum Branch
    {
        Coding,
        Graphics,
        Film,
        Flexible
    }

    public class DataService
    {
        private readonly string firebaseBaseUrl; // Set your Firebase base URL here
        private readonly FirebaseClient _firebaseDatabase;
        private readonly FirebaseAuthClient _firebaseAuthClient;
        private ACCOUNT _account;
        private const string Qt = "Qt Creator";
        private const string Android = "Android Studio Hedgehog 2023.1.1";

        // Retrieve Project Names for the Logger Create Page here
        public ProjectsLIST FIREBASE_PROJECTS { get; set; } = new();

        // Retrieve Application Names here
        public ApplicationsLIST FIREBASE_APPLICATIONS { get; set; } = new();

        public DataService(string APIKey, string Domain)
        {
            // Initialize FirebaseAuthClient with provided Firebase API key and domain
            _firebaseAuthClient = new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = APIKey,
                AuthDomain = Domain,
                Providers =
                [
                new GoogleProvider().AddScopes("email", "profile"),
                new EmailProvider(),
                new MicrosoftProvider(),
                ],
                UserRepository = new FileUserRepository("Data Logger")
            });

            _firebaseDatabase = new(@"https://dls03-d1959-default-rtdb.europe-west1.firebasedatabase.app/");


            setup();
        }

        public DataService(string APIKey, string Domain, AuthService authService)
        {
            // Initialize FirebaseAuthClient with provided Firebase API key and domain
            _firebaseAuthClient = new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = APIKey,
                AuthDomain = Domain,
                Providers =
                [
                new GoogleProvider().AddScopes("email", "profile"),
                new EmailProvider(),
                new MicrosoftProvider(),
                ],
                UserRepository = new FileUserRepository("Data Logger")
            });

            _firebaseDatabase = new(@"https://dls03-d1959-default-rtdb.europe-west1.firebasedatabase.app/");

            _account = authService.Account;

            setup();
        }

        public async void setup()
        {
            await InitialiseApplicationsLIST();
            await InitialiseProjectsLIST();
        }



        public async Task<bool> AddUser(ACCOUNT account)
        {

            try
            {
                await _firebaseDatabase.Child("Accounts").PostAsync(account);
            }
            catch (Exception)
            {

                // TODO
            }

            return false;
        }

        public ACCOUNT GetUser()
        {
            return _account;
        }

        public string GetAuthorName()
        {
            return _account.FirstName + " " + _account.LastName;
        }

        public string GetDisplayPic()
        {
            return _account.ProfilePic;
        }

        public void SetAccount(AuthService authService)
        {
            _account = authService.Account;
        }

        /* Use this to retrieve projects from Firebase. Use the total count of projects to create project IDs.
         * Use the Category to filter out projects for a specific category in the project object.
         * */

        public async Task InitialiseProjectsLIST()
        {
            FIREBASE_PROJECTS.Clear();

            var collection = await RetrieveCodingPROJECTS(ProjectsCodingBRANCH.Qt);

            foreach(ProjectClass pro in collection)
            {
                if(pro is not null && pro.Name != "Unknown")
                    FIREBASE_PROJECTS.Add(pro);
            }

            collection = await RetrieveProjects(Branch.Graphics);

            foreach (ProjectClass pro in collection)
            {
                if (pro is not null && pro.Name != "Unknown")
                    FIREBASE_PROJECTS.Add(pro);
            }

            collection = await RetrieveProjects(Branch.Film);

            foreach (ProjectClass pro in collection)
            {
                if (pro is not null && pro.Name != "Unknown")
                    FIREBASE_PROJECTS.Add(pro);
            }

            collection = await RetrieveProjects(Branch.Flexible);

            foreach (ProjectClass pro in collection)
            {
                if (pro is not null && pro.Name != "Unknown")
                    FIREBASE_PROJECTS.Add(pro);
            }

        }

        public async Task InitialiseApplicationsLIST()
        {
            FIREBASE_APPLICATIONS.Clear();

            var collection = await RetrieveApplications(Branch.Coding);

            foreach(ApplicationClass app in collection)
            {
                if (app is not null)
                    FIREBASE_APPLICATIONS.Add(app);
            }

            CheckDefaults(new(1, _account, Qt, LOG.CATEGORY.CODING, true));
            CheckDefaults(new(2, _account, Android, LOG.CATEGORY.CODING, true));
            CheckDefaults(new(3, _account, "Unknown", LOG.CATEGORY.CODING, true));
            CheckDefaults(new(4, _account, "Microsoft Visual Studio Community 2022", LOG.CATEGORY.CODING, true));
            CheckDefaults(new(5, _account, "Apache NetBeans", LOG.CATEGORY.CODING, true));
            CheckDefaults(new(6, _account, "Google Chrome", LOG.CATEGORY.CODING, true));
            CheckDefaults(new(7, _account, "Notepad", LOG.CATEGORY.CODING, true));
            CheckDefaults(new(8, _account, "Microsoft Edge", LOG.CATEGORY.CODING, true));
            CheckDefaults(new(9, _account, "WAMPP", LOG.CATEGORY.CODING, true));
            CheckDefaults(new(10, _account, "Visual Studio Code", LOG.CATEGORY.CODING, true));

            collection = await RetrieveApplications(Branch.Graphics);

            foreach(ApplicationClass app in collection)
            {
                if (app is not null)
                    FIREBASE_APPLICATIONS.Add(app);
            }

            CheckDefaults(new(11, _account, "Inkscape", LOG.CATEGORY.GRAPHICS, true));
            CheckDefaults(new(12, _account, "Krita", LOG.CATEGORY.GRAPHICS, true));
            CheckDefaults(new(13, _account, "Unknown", LOG.CATEGORY.GRAPHICS, true));
            CheckDefaults(new(14, _account, "Blender", LOG.CATEGORY.GRAPHICS, true));
            CheckDefaults(new(15, _account, "Figma", LOG.CATEGORY.GRAPHICS, true));

            collection = await RetrieveApplications(Branch.Film);

            foreach (ApplicationClass app in collection)
            {
                if (app is not null)
                    FIREBASE_APPLICATIONS.Add(app);
            }

            CheckDefaults(new(16, _account, "Blender", LOG.CATEGORY.FILM, true));
            CheckDefaults(new(17, _account, "DaVinci Resolve 18", LOG.CATEGORY.FILM, true));
            CheckDefaults(new(18, _account, "Unknown", LOG.CATEGORY.FILM, true));
            CheckDefaults(new(19, _account, "Adobe Premiere Pro", LOG.CATEGORY.FILM, true));


            collection = await RetrieveApplications(Branch.Flexible);

            foreach (ApplicationClass app in collection)
            {
                if (app is not null)
                    FIREBASE_APPLICATIONS.Add(app);
            }

            CheckDefaults(new(20, _account, "Unity", LOG.CATEGORY.NOTES, true));
            CheckDefaults(new(21, _account, "Microsoft Word (Office 365)", LOG.CATEGORY.NOTES, true));
            CheckDefaults(new(22, _account, "Microsoft Word | Office Home & Student 2021", LOG.CATEGORY.NOTES, true));
            CheckDefaults(new(23, _account, "Unknown", LOG.CATEGORY.NOTES, true));
            CheckDefaults(new(24, _account, "Musescore 4.2.1", LOG.CATEGORY.NOTES, true));
            CheckDefaults(new(25, _account, "Blender", LOG.CATEGORY.NOTES, true));
        }

        // Ensure default apps like Qt Creator exist even before the user creates their first log
        private async void CheckDefaults(ApplicationClass application)
        {
            string branch;

            if (application.Category == LOG.CATEGORY.CODING)
                branch = "Coding";
            else if (application.Category == LOG.CATEGORY.GRAPHICS)
                branch = "Graphics";
            else if (application.Category == LOG.CATEGORY.FILM)
                branch = "Film";
            else
                branch = "Flexi";

            if (!FIREBASE_APPLICATIONS.Contains(application))
            {
                try
                {
                    await _firebaseDatabase.Child("Applications").Child(branch).PostAsync(application);

                    await InitialiseApplicationsLIST();
                }
                catch (Exception)
                {
                    // TODO
                }
            }
        }






        #region Log Storage






        /* CODING */

        public async Task<bool> StoreCodingLog(CodingLOG codingLog)
        {
            try
            {
                if(codingLog.Category == LOG.CATEGORY.CODING)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Coding").PostAsync(codingLog);


                    // Store the app
                    ApplicationClass app = new(FIREBASE_APPLICATIONS.Count()+1, _account, codingLog.ApplicationName, LOG.CATEGORY.CODING, false);

                    // Store the project
                    string name;

                    if (codingLog.ProjectName != "")
                    {
                        name = codingLog.ProjectName;

                        ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, name, app, LOG.CATEGORY.CODING);

                        FirebaseAppProjectStore(app, project, "Coding", LOG.CATEGORY.CODING);
                    }

                    

                    
                }
                

                return true;
            }
            catch (Exception)
            {
                // TODO
            }

            return false;
        }

        /* Qt */

        public async Task<bool> StoreQtCodingLog(CodingLOG QtcodingLog)
        {
            try
            {

                if (QtcodingLog.Category == LOG.CATEGORY.CODING)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Coding").Child("Qt").PostAsync(QtcodingLog);


                    // No need to store Qt in ApplicationsList. It's stored by default.

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, QtcodingLog.ProjectName,
                        new(1, Qt, LOG.CATEGORY.CODING), LOG.CATEGORY.CODING, true);

                    Dictionary<string, string> pairs = new();

                    foreach (ProjectClass pro in FIREBASE_PROJECTS)
                    {
                        pairs.Add(pro.Application.Name, pro.Name);
                    }

                    if (QtcodingLog.ApplicationName == Qt && 
                        !(pairs.ContainsKey(project.Application.Name) && pairs[project.Application.Name] == project.Name))
                    {
                        await _firebaseDatabase.Child("Projects").Child("Coding").Child("Qt").PostAsync(project);

                        await InitialiseProjectsLIST();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                // TODO
            }

            return false;
        }

        /* Android Studio */

        public async Task<bool> StoreASCodingLog(AndroidCodingLOG AScodingLog)
        {
            try
            {
                if (AScodingLog.Category == LOG.CATEGORY.CODING)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Coding").Child("AS").PostAsync(AScodingLog);

                    // No need to store the Android Studio app in the ApplicationsList. It's stored by default.

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, AScodingLog.ProjectName,
                        new(2, Android, LOG.CATEGORY.CODING), LOG.CATEGORY.CODING, true);

                    if (AScodingLog.ApplicationName == Android && !FIREBASE_PROJECTS.Contains(project))
                    {
                        await _firebaseDatabase.Child("Projects").Child("Coding").Child("Android").PostAsync(project);

                        await InitialiseProjectsLIST();
                    }
                    
                }

                return true;
            }
            catch (Exception)
            {
                // TODO
            }

            return false;
        }

        /* GRAPHICS */

        public async Task<bool> StoreGraphicsLog(GraphicsLOG graphicsLog)
        {
            try
            {

                if (graphicsLog.Category == LOG.CATEGORY.GRAPHICS)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Graphics").PostAsync(graphicsLog);

                    // Store the app
                    ApplicationClass app = new(FIREBASE_APPLICATIONS.Count() + 1, _account, graphicsLog.ApplicationName, LOG.CATEGORY.GRAPHICS);

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, graphicsLog.ProjectName,
                        app, LOG.CATEGORY.GRAPHICS);


                    FirebaseAppProjectStore(app, project, "Graphics", LOG.CATEGORY.GRAPHICS);
                    
                }


                return true;
            }
            catch (Exception)
            {
                // TODO
            }

            return false;
        }

        /* FILM */

        public async Task<bool> StoreFilmLog(FilmLOG filmLog)
        {
            try
            {

                if (filmLog.Category == LOG.CATEGORY.FILM)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Film").PostAsync(filmLog);


                    // Store the app
                    ApplicationClass app = new(FIREBASE_APPLICATIONS.Count() + 1, _account, filmLog.ApplicationName, LOG.CATEGORY.FILM);

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, filmLog.ProjectName, app, LOG.CATEGORY.FILM);

                    FirebaseAppProjectStore(app, project, "Film", LOG.CATEGORY.FILM);


                }
                    

                return true;
            }
            catch (Exception)
            {
                // TODO
            }

            return false;
        }

        /* FLEXIBLE */

        public async Task<bool> StoreFlexibleLog(FlexiNotesLOG flexiLog)
        {
            try
            {

                if (flexiLog.Category == LOG.CATEGORY.NOTES)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Flexi").PostAsync(flexiLog);

                    // Store the app
                    ApplicationClass app = new(FIREBASE_APPLICATIONS.Count() + 1, _account, flexiLog.ApplicationName, LOG.CATEGORY.NOTES);

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, flexiLog.ProjectName, app, LOG.CATEGORY.NOTES);

                    FirebaseAppProjectStore(app, project, "Flexi", LOG.CATEGORY.NOTES);
                }
                    

                return true;
            }
            catch (Exception)
            {
                // TODO
            }

            return false;
        }






        // //////////////////////////////////////////////////////////
        // Helper function
        public async void FirebaseAppProjectStore(ApplicationClass application, ProjectClass project, string branch, LOG.CATEGORY category)
        {
            // Store the app

            if (!FIREBASE_APPLICATIONS.Contains(application))
            {
                await _firebaseDatabase.Child("Applications").Child(branch).PostAsync(application);

                await InitialiseApplicationsLIST();
            }


            // Store the project
            Dictionary<string, string> pairs = new();
            
            foreach(ProjectClass pro in FIREBASE_PROJECTS)
            {
                pairs.Add(pro.Application.Name, pro.Name);
            }

            if (category == LOG.CATEGORY.CODING)
            {


                if (project.Application.Name != Qt && project.Application.Name != Android &&
                        !(pairs.ContainsKey(project.Application.Name) && pairs[project.Application.Name] == project.Name))
                {
                    await _firebaseDatabase.Child("Projects").Child("Coding").Child("Generic").PostAsync(project);

                    await InitialiseProjectsLIST();
                }
            }
            else
            {
                if(!(pairs.ContainsKey(project.Application.Name) && pairs[project.Application.Name] == project.Name))
                {
                    await _firebaseDatabase.Child("Projects").Child(branch).PostAsync(project);

                    await InitialiseProjectsLIST();
                }
            }
        }






        #endregion













        #region Retrieval




        public async Task<List<CodeLOGViewModel>> RetrieveCodingLogs(LogCacheViewModel logCacheViewModel)
        {
            List<CodeLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Coding").OrderByKey().OnceAsync<CodingLOG>();

                foreach (var item in items)
                {
                    if (item.Object.ApplicationName != Qt && item.Object.ApplicationName != Android
                        && item.Object.Author == _account)
                    {
                        
                        list.Add(new(item.Object, logCacheViewModel, this));
                    }
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }

        public async Task<List<QtLOGViewModel>> RetrieveQtCodingLogs(LogCacheViewModel logCacheViewModel)
        {
            List<QtLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Coding").Child("Qt").OrderByKey().OnceAsync<CodingLOG>();

                foreach (var item in items)
                {
                    if (item.Object.ApplicationName == Qt && item.Object.Author == _account)
                        list.Add(new(item.Object, logCacheViewModel, this));
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }

        public async Task<List<AndroidLOGViewModel>> RetrieveASCodingLogs(LogCacheViewModel logCacheViewModel)
        {
            List<AndroidLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Coding").Child("AS").OrderByKey().OnceAsync<AndroidCodingLOG>();

                foreach (var item in items)
                {
                    if (item.Object.ApplicationName == Android && item.Object.Author == _account)
                        list.Add(new(item.Object, logCacheViewModel, this));
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }


        public async Task<List<GraphicsLOGViewModel>> RetrieveGraphicsLogs(LogCacheViewModel logCacheViewModel)
        {
            List<GraphicsLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Graphics").OrderByKey().OnceAsync<GraphicsLOG>();

                foreach (var item in items)
                {
                    if(item.Object.Category == LOG.CATEGORY.GRAPHICS && item.Object.Author == _account)
                        list.Add(new(item.Object, logCacheViewModel, this));
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }

        public async Task<List<FilmLOGViewModel>> RetrieveFilmLogs(LogCacheViewModel logCacheViewModel)
        {
            List<FilmLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Film").OrderByKey().OnceAsync<FilmLOG>();

                foreach (var item in items)
                {
                    if (item.Object.Category == LOG.CATEGORY.FILM && item.Object.Author == _account)
                        list.Add(new(item.Object, logCacheViewModel, this));
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }

        public async Task<List<FlexiLOGViewModel>> RetrieveFlexibleLogs(LogCacheViewModel logCacheViewModel)
        {
            List<FlexiLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Flexi").OrderByKey().OnceAsync<FlexiNotesLOG>();

                foreach (var item in items)
                {
                    if (item.Object.Category == LOG.CATEGORY.NOTES && item.Object.Author == _account)
                        list.Add(new(item.Object, logCacheViewModel, this));
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }






        /* PROJECTS ONLY */


        // Enter whether it's a Qt, Android Studio or Generic Log in the "enumBranch" parameter
        public async Task<List<ProjectClass>> RetrieveCodingPROJECTS(ProjectsCodingBRANCH enumBranch)
        {
            List<ProjectClass> list = new();

            string branch;

            if (enumBranch == ProjectsCodingBRANCH.Qt)
                branch = "Qt";
            else if (enumBranch == ProjectsCodingBRANCH.Android)
            {
                branch = "Android";
            }
            else
                branch = "Generic";

            try
            {
                var items = await _firebaseDatabase.Child("Projects").Child("Coding").Child(branch).OnceAsync<ProjectClass>();

                foreach (var item in items)
                {
                    if(item.Object.User == _account)
                        list.Add(item.Object);
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }

        public async Task<List<ProjectClass>> RetrieveProjects(Branch enumBranch)
        {
            List<ProjectClass> list = new();
            string branch;

            if (enumBranch == Branch.Graphics)
                branch = "Graphics";
            else if (enumBranch == Branch.Film)
                branch = "Film";
            else
                branch = "Flexi";

            try
            {
                var items = await _firebaseDatabase.Child("Projects").Child(branch).OnceAsync<ProjectClass>();

                foreach (var item in items)
                {
                    if(item.Object.User == _account)
                        list.Add(item.Object);
                }
            }
            catch (Exception)
            {
                //
            }

            return list;

        }




        /* APPLICATIONS ONLY */

        public async Task<List<ApplicationClass>> RetrieveApplications(Branch enumBranch)
        {
            List<ApplicationClass> list = new();

            string branch;

            if (enumBranch == Branch.Coding)
                branch = "Coding";
            else if (enumBranch == Branch.Graphics)
                branch = "Graphics";
            else if (enumBranch == Branch.Film)
                branch = "Film";
            else
                branch = "Flexi";

            try
            {
                var items = await _firebaseDatabase.Child("Applications").Child(branch).OnceAsync<ApplicationClass>();

                foreach (var item in items)
                {
                    if(item.Object.User == _account)
                        list.Add(item.Object);
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }







        #endregion

    }
}
