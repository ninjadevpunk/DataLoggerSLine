using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
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
        private readonly ACCOUNT _account;

        // Retrieve Project Names for the Logger Create Page here
        public ProjectsLIST FIREBASE_PROJECTS { get; set; } = new();

        // Retrieve Application Names here
        public ApplicationsLIST FIREBASE_APPLICATIONS { get; set; } = new();

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

        public string GetUser()
        {
            return _account.FirstName + " " + _account.LastName;
        }

        public string GetDisplayPic()
        {
            return _account.ProfilePic;
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
                if(pro is not null)
                    FIREBASE_PROJECTS.Add(pro);
            }

            collection = await RetrieveProjects(Branch.Graphics);

            foreach (ProjectClass pro in collection)
            {
                if (pro is not null)
                    FIREBASE_PROJECTS.Add(pro);
            }

            collection = await RetrieveProjects(Branch.Film);

            foreach (ProjectClass pro in collection)
            {
                if (pro is not null)
                    FIREBASE_PROJECTS.Add(pro);
            }

            collection = await RetrieveProjects(Branch.Flexible);

            foreach (ProjectClass pro in collection)
            {
                if (pro is not null)
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

            CheckDefaults(new(1, "Qt Creator", LOG.CATEGORY.CODING));
            CheckDefaults(new(2, "Android Studio Hedgehog 2023.1.1", LOG.CATEGORY.CODING));
            CheckDefaults(new(3, "Unknown", LOG.CATEGORY.CODING));
            CheckDefaults(new(4, "Microsoft Visual Studio Community 2022", LOG.CATEGORY.CODING));
            CheckDefaults(new(5, "Apache NetBeans", LOG.CATEGORY.CODING));
            CheckDefaults(new(6, "Google Chrome", LOG.CATEGORY.CODING));
            CheckDefaults(new(7, "Notepad", LOG.CATEGORY.CODING));
            CheckDefaults(new(8, "Microsoft Edge", LOG.CATEGORY.CODING));
            CheckDefaults(new(9, "WAMPP", LOG.CATEGORY.CODING));
            CheckDefaults(new(10, "Visual Studio Code", LOG.CATEGORY.CODING));

            collection = await RetrieveApplications(Branch.Graphics);

            foreach(ApplicationClass app in collection)
            {
                if (app is not null)
                    FIREBASE_APPLICATIONS.Add(app);
            }

            CheckDefaults(new(11, "Inkscape", LOG.CATEGORY.GRAPHICS));
            CheckDefaults(new(12, "Krita", LOG.CATEGORY.GRAPHICS));
            CheckDefaults(new(13, "Unknown", LOG.CATEGORY.GRAPHICS));
            CheckDefaults(new(14, "Blender", LOG.CATEGORY.GRAPHICS));
            CheckDefaults(new(15, "Figma", LOG.CATEGORY.GRAPHICS));

            collection = await RetrieveApplications(Branch.Film);

            foreach (ApplicationClass app in collection)
            {
                if (app is not null)
                    FIREBASE_APPLICATIONS.Add(app);
            }

            CheckDefaults(new(16, "Blender", LOG.CATEGORY.FILM));
            CheckDefaults(new(17, "DaVinci Resolve 18", LOG.CATEGORY.FILM));
            CheckDefaults(new(18, "Unknown", LOG.CATEGORY.FILM));
            CheckDefaults(new(19, "Adobe Premiere Pro", LOG.CATEGORY.FILM));


            collection = await RetrieveApplications(Branch.Flexible);

            foreach (ApplicationClass app in collection)
            {
                if (app is not null)
                    FIREBASE_APPLICATIONS.Add(app);
            }

            CheckDefaults(new(20, "Unity", LOG.CATEGORY.NOTES));
            CheckDefaults(new(21, "Microsoft Word (Office 365)", LOG.CATEGORY.NOTES));
            CheckDefaults(new(22, "Microsoft Word | Office Home & Student 2021", LOG.CATEGORY.NOTES));
            CheckDefaults(new(23, "Unknown", LOG.CATEGORY.NOTES));
            CheckDefaults(new(24, "Musescore 4.2.1", LOG.CATEGORY.NOTES));
            CheckDefaults(new(25, "Blender", LOG.CATEGORY.NOTES));
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
                    //
                }
            }
        }






        #region Log Storage






        /* CODING */

        public async Task<bool> StoreCodingLog(CodeLOGViewModel codingLog)
        {
            try
            {
                if(codingLog._CodeLOG.Category == LOG.CATEGORY.CODING)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Coding").PostAsync(codingLog);


                    // Store the app
                    ApplicationClass app = new(FIREBASE_APPLICATIONS.Count()+1, _account, codingLog._CodeLOG.ApplicationName, LOG.CATEGORY.CODING);

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, codingLog._CodeLOG.ProjectName, app, LOG.CATEGORY.CODING);

                    FirebaseAppProjectStore(app, project, "Coding", LOG.CATEGORY.CODING);

                    
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

        public async Task<bool> StoreQtCodingLog(QtLOGViewModel QtcodingLog)
        {
            try
            {

                if (QtcodingLog._QtcodingLOG.Category == LOG.CATEGORY.CODING)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Coding").PostAsync(QtcodingLog);


                    // No need to store Qt in ApplicationsList. It's stored by default.

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, QtcodingLog._QtcodingLOG.ProjectName,
                        new(1, "Qt Creator", LOG.CATEGORY.CODING), LOG.CATEGORY.CODING);

                    if (QtcodingLog._QtcodingLOG.ApplicationName == "Qt Creator" && !FIREBASE_PROJECTS.Contains(project))
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

        public async Task<bool> StoreASCodingLog(AndroidLOGViewModel AScodingLog)
        {
            try
            {
                if (AScodingLog._AndroidCodingLOG.Category == LOG.CATEGORY.CODING)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Coding").PostAsync(AScodingLog);

                    // No need to store the Android Studio app in the ApplicationsList. It's stored by default.

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, AScodingLog._AndroidCodingLOG.ProjectName,
                        new(2, "Android Studio Hedgehog 2023.1.1", LOG.CATEGORY.CODING), LOG.CATEGORY.CODING);

                    if (AScodingLog._AndroidCodingLOG.ApplicationName == "Android Studio Hedgehog 2023.1.1" && !FIREBASE_PROJECTS.Contains(project))
                    {
                        await _firebaseDatabase.Child("Projects").Child("Coding").Child("Android").PostAsync(project);

                        InitialiseProjectsLIST();
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

        public async Task<bool> StoreGraphicsLog(GraphicsLOGViewModel graphicsLog)
        {
            try
            {

                if (graphicsLog._GraphicsLOG.Category == LOG.CATEGORY.GRAPHICS)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Graphics").PostAsync(graphicsLog);

                    // Store the app
                    ApplicationClass app = new(FIREBASE_APPLICATIONS.Count() + 1, _account, graphicsLog._GraphicsLOG.ApplicationName, LOG.CATEGORY.GRAPHICS);

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, graphicsLog._GraphicsLOG.ProjectName,
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

        public async Task<bool> StoreFilmLog(FilmLOGViewModel filmLog)
        {
            try
            {

                if (filmLog._FilmLOG.Category == LOG.CATEGORY.FILM)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Film").PostAsync(filmLog);


                    // Store the app
                    ApplicationClass app = new(FIREBASE_APPLICATIONS.Count() + 1, _account, filmLog._FilmLOG.ApplicationName, LOG.CATEGORY.FILM);

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, filmLog._FilmLOG.ProjectName, app, LOG.CATEGORY.FILM);

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

        public async Task<bool> StoreFlexibleLog(FlexiLOGViewModel flexiLog)
        {
            try
            {

                if (flexiLog._FlexiLOG.Category == LOG.CATEGORY.NOTES)
                {
                    // STORE THE LOG
                    await _firebaseDatabase.Child("Log").Child("Flexi").PostAsync(flexiLog);

                    // Store the app
                    ApplicationClass app = new(FIREBASE_APPLICATIONS.Count() + 1, _account, flexiLog._FlexiLOG.ApplicationName, LOG.CATEGORY.NOTES);

                    // Store the project
                    ProjectClass project = new(FIREBASE_PROJECTS.Count() + 1, _account, flexiLog._FlexiLOG.ProjectName, app, LOG.CATEGORY.NOTES);

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

            if(category == LOG.CATEGORY.CODING)
            {
                if (project.Application.Name != "Qt Creator" && project.Application.Name != "Android Studio Hedgehog 2023.1.1" &&
                !FIREBASE_PROJECTS.Contains(project))
                {
                    await _firebaseDatabase.Child("Projects").Child("Coding").Child("Generic").PostAsync(project);

                    await InitialiseProjectsLIST();
                }
            }
            else
            {
                if(!FIREBASE_PROJECTS.Contains(project))
                {
                    await _firebaseDatabase.Child("Projects").Child(branch).PostAsync(project);

                    await InitialiseProjectsLIST();
                }
            }
        }






        #endregion













        #region Retrieval




        public async Task<List<CodeLOGViewModel>> RetrieveCodingLogs()
        {
            List<CodeLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Coding").OrderByKey().OnceAsync<CodeLOGViewModel>();

                foreach (var item in items)
                {
                    if (item.Object._CodeLOG.ApplicationName != "Qt Creator" && item.Object._CodeLOG.ApplicationName != "Android Studio Hedgehog 2023.1.1"
                        && item.Object._CodeLOG.Author == _account)
                        list.Add(item.Object);
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }

        public async Task<List<QtLOGViewModel>> RetrieveQtCodingLogs()
        {
            List<QtLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Coding").OrderByKey().OnceAsync<QtLOGViewModel>();

                foreach (var item in items)
                {
                    if (item.Object._QtcodingLOG.ApplicationName == "Qt Creator" && item.Object._QtcodingLOG.Author == _account)
                        list.Add(item.Object);
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }

        public async Task<List<AndroidLOGViewModel>> RetrieveASCodingLogs()
        {
            List<AndroidLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Coding").OrderByKey().OnceAsync<AndroidLOGViewModel>();

                foreach (var item in items)
                {
                    if (item.Object._AndroidCodingLOG.ApplicationName == "Android Studio Hedgehog 2023.1.1" && item.Object._AndroidCodingLOG.Author == _account)
                        list.Add(item.Object);
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }


        public async Task<List<GraphicsLOGViewModel>> RetrieveGraphicsLogs()
        {
            List<GraphicsLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Graphics").OrderByKey().OnceAsync<GraphicsLOGViewModel>();

                foreach (var item in items)
                {
                    if(item.Object._GraphicsLOG.Category == LOG.CATEGORY.GRAPHICS && item.Object._GraphicsLOG.Author == _account)
                        list.Add(item.Object);
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }

        public async Task<List<FilmLOGViewModel>> RetrieveFilmLogs()
        {
            List<FilmLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Film").OrderByKey().OnceAsync<FilmLOGViewModel>();

                foreach (var item in items)
                {
                    if (item.Object._FilmLOG.Category == LOG.CATEGORY.FILM && item.Object._FilmLOG.Author == _account)
                        list.Add(item.Object);
                }
            }
            catch (Exception)
            {
                //
            }

            return list;
        }

        public async Task<List<FlexiLOGViewModel>> RetrieveFlexibleLogs()
        {
            List<FlexiLOGViewModel> list = new();

            try
            {
                var items = await _firebaseDatabase.Child("Log").Child("Flexi").OrderByKey().OnceAsync<FlexiLOGViewModel>();

                foreach (var item in items)
                {
                    if (item.Object._FlexiLOG.Category == LOG.CATEGORY.NOTES && item.Object._FlexiLOG.Author == _account)
                        list.Add(item.Object);
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
