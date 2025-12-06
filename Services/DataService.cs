using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using static Data_Logger_1._3.Models.LOG;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Services
{

    public class DataService
    {
        private readonly ENTITYREADER _reader;
        private readonly ENTITYHANDLER _handler;
        private readonly Cachemaster _cachemaster;
        private readonly IServiceProvider _serviceProvider;

        private readonly AuthService _authService;

        private const string Qt = "Qt Creator";
        private const string Android = "Android Studio Meerkat 2024.3.1";

        // Retrieve Project Names for the Logger Create Page here
        public ProjectsLIST SQLITE_PROJECTS { get; set; } = new();

        // Retrieve Application Names here
        public ApplicationsLIST SQLITE_APPLICATIONS { get; set; } = new();

        public List<SubjectClass> SQLITE_SUBJECTS { get; set; } = new();


        public DataService(ENTITYWRITER writer, ENTITYREADER reader, ENTITYHANDLER handler, Cachemaster cachemaster,
            AuthService authService, IServiceProvider serviceProvider)
        {
            _reader = reader;
            _handler = handler;
            _cachemaster = cachemaster;

            _authService = authService;
            _serviceProvider = serviceProvider;
        }

        public async Task SignInUser()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var account = _authService?.Account;
                if (account == null)
                    return;

                var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();
                await writer.SetCurrentUser(account);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "SignInUser()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "SignInUser()");
            }
        }


        public async Task SignOutUser()
        {
            var account = _authService?.Account;
            if (account is null)
                return;

            await using var scope = _serviceProvider.CreateAsyncScope();
            var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();
            await writer.UnsetCurrentUser();
        }


        public static bool IsValidEmail(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }

        public async Task<string> UpdateProfilePic(string emailAddress)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();

            return await reader.RetrieveProfilePic(emailAddress);
        }


        public ACCOUNT GetUser()
        {
            return _authService.Account;
        }

        public string GetAuthorName()
        {
            return _authService.Account.FirstName + " " + _authService.Account.LastName;
        }

        public Cachemaster GetCachemaster()
        {
            return _cachemaster;
        }

        public string GetDisplayPic()
        {
            return _authService.Account.ProfilePic;
        }




        /// <summary>
        /// Retrieves projects from the database.
        /// </summary>
        public async Task InitialiseProjectsLISTAsync()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                SQLITE_PROJECTS.Clear();

                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                var collection = await reader.ListProjects();

                foreach (var pro in collection)
                {
                    if (pro != null && pro.Name != "Unnamed Project")
                        SQLITE_PROJECTS.Add(pro);
                }
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "SignInUser()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "InitialiseProjectsLISTAsync()");
            }
        }




        /// <summary>
        /// Retrieves projects from the database of a specified category.
        /// </summary>
        /// <param name="category">The type of project.</param>
        public async Task InitialiseProjectsLISTAsync(LOG.CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                SQLITE_PROJECTS.Clear();

                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                var collection = await reader.ListProjects(category);

                foreach (var pro in collection)
                {
                    if (pro != null && pro.Name != "Unnamed Project")
                        SQLITE_PROJECTS.Add(pro);
                }
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "InitialiseProjectsLISTAsync(category)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "InitialiseProjectsLISTAsync(category)");
            }
        }



        /// <summary>
        /// Retrieves applications from the database.
        /// </summary>
        public async Task InitialiseApplicationsLISTAsync()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                SQLITE_APPLICATIONS.Clear();

                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                var collection = await reader.ListApplications();

                foreach (var app in collection)
                {
                    if (app != null)
                        SQLITE_APPLICATIONS.Add(app);
                }
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "InitialiseApplicationsLISTAsync()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "InitialiseApplicationsLISTAsync()");
            }
        }


        /// <summary>
        /// Retrieves applications from the database of a specified category.
        /// </summary>
        /// <param name="category">The type of application.</param>
        public async Task InitialiseApplicationsLISTAsync(LOG.CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                SQLITE_APPLICATIONS.Clear();

                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                var collection = await reader.ListApplications(category);

                foreach (var app in collection)
                {
                    if (app != null)
                        SQLITE_APPLICATIONS.Add(app);
                }
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "InitialiseApplicationsLISTAsync(category)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "InitialiseApplicationsLISTAsync(category)");
            }
        }



        /// <summary>
        /// Retrieves subjects from the database.
        /// </summary>
        /// <param name="category">The type of subjects being retrieved.</param>
        public async Task InitialiseSubjectsLIST(LOG.CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                SQLITE_SUBJECTS.Clear();

                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                var collection = await reader.ListSubjects(category);

                foreach (var subject in collection)
                {
                    if (subject != null && subject.Subject != "No Subject")
                        SQLITE_SUBJECTS.Add(subject);
                }
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "InitialiseSubjectsLIST(category)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "InitialiseSubjectsLIST(category)");
            }
        }


        /// <summary>
        /// Retrieves subjects from the database. Also adds the subjects in DataService's subject list property for easy retrieval in cases where the most recent database update is not needed.
        /// </summary>
        /// <param name="project">Subjects from the specified ProjectClass will be retrieved ONLY.</param>
        public async Task InitialiseSubjectsLIST(ProjectClass project)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                SQLITE_SUBJECTS.Clear();

                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                var collection = await reader.ListSubjects(project);

                foreach (var subject in collection)
                {
                    if (subject != null)
                        SQLITE_SUBJECTS.Add(subject);
                }
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "InitialiseSubjectsLIST(project)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "InitialiseSubjectsLIST(project)");
            }
        }



        public async Task SaveChangesAsync()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                await reader.SaveChangesAsync();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "SaveChangesAsync()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "SaveChangesAsync()");
            }
        }



        /// <summary>
        /// Retrieves subjects from the database. Used every time the subjects are needed as a list immediately.
        /// </summary>
        /// <param name="project">Subjects from the specified ProjectClass will be retrieved ONLY.</param>
        /// <returns>Returns the subjects as a List.</returns>
        public async Task<List<SubjectClass>?> ListSubjects(ProjectClass project)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.ListSubjects(project);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "ListSubjects(project)", "InvalidOperationException");
                return new List<SubjectClass>();
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "ListSubjects(project)");
                return new List<SubjectClass>();
            }
        }

        /// <summary>
        /// Retrieves all Qt outputs from the database.
        /// </summary>
        /// <returns>Returns the Qt outputs as a List.</returns>
        public async Task<List<OutputClass>?> ListQtOutputs()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.ListQtOutputs();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "ListQtOutputs()", "InvalidOperationException");
                return new List<OutputClass>();
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "ListQtOutputs()");
                return new List<OutputClass>();
            }
        }

        /// <summary>
        /// Retrieves all Android Studio outputs from the database.
        /// </summary>
        /// <returns>Returns the AS outputs as a List.</returns>
        public async Task<List<OutputClass>?> ListASOutputs()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.ListASOutputs();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "ListASOutputs()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "ListASOutputs()");
            }

            return null;
        }

        /// <summary>
        /// Retrieves outputs from the database of a specified category.
        /// </summary>
        /// <param name="category">The type of outputs.</param>
        /// <returns>Returns the outputs as a List.</returns>
        public async Task<List<OutputClass>?> ListOutputs(CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.ListOutputs(category);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "ListOutputs(category)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "ListOutputs(category)");
            }

            return null;
        }



        /// <summary>
        /// Retrieves all Qt types from the database.
        /// </summary>
        /// <returns>Returns the Qt types as a List, or null if an error occurs.</returns>
        public async Task<List<TypeClass>?> ListQtTypes()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.ListQtTypes();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "ListQtTypes()", "InvalidOperationException");
                return null;
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "ListQtTypes()");
                return null;
            }
        }

        /// <summary>
        /// Retrieves all Android Studio types from the database.
        /// </summary>
        /// <returns>Returns the AS types as a List, or null if an error occurs.</returns>
        public async Task<List<TypeClass>?> ListASTypes()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.ListASTypes();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "ListASTypes()", "InvalidOperationException");
                return null;
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "ListASTypes()");
                return null;
            }
        }

        /// <summary>
        /// Retrieves types from the database of a specified category.
        /// </summary>
        /// <param name="category">The category of types to retrieve.</param>
        /// <returns>Returns the types as a List, or null if an error occurs.</returns>
        public async Task<List<TypeClass>?> ListTypes(CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.ListTypes(category);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "ListTypes(category)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "ListTypes(category)");
            }

            return null;
        }











        /// <summary>
        /// Finds an application by ID or name.
        /// </summary>
        /// <param name="ID">The application ID.</param>
        /// <param name="name">The application name.</param>
        /// <returns>Returns the ApplicationClass if found; otherwise null.</returns>
        public async Task<ApplicationClass?> FindApplication(int ID, string name)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindApplication(ID, name);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindApplication(ID, name)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindApplication(ID, name)");
            }

            return null;
        }

        /// <summary>
        /// Finds an application by its ID.
        /// </summary>
        /// <param name="appID">The application ID.</param>
        /// <returns>Returns the ApplicationClass if found; otherwise null.</returns>
        public async Task<ApplicationClass?> FindApplicationByID(int appID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindApplicationByID(appID);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindApplicationByID(appID)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindApplicationByID(appID)");
            }

            return null;
        }


        /// <summary>
        /// Finds a project by user ID, project name, and application ID.
        /// </summary>
        /// <param name="userID">The user ID (nullable).</param>
        /// <param name="projectName">The project name.</param>
        /// <param name="appID">The application ID.</param>
        /// <returns>Returns the ProjectClass if found; otherwise null.</returns>
        public async Task<ProjectClass?> FindProject(int? userID, string projectName, int appID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindProject(userID, projectName, appID);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindProject(userID, projectName, appID)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindProject(userID, projectName, appID)");
            }

            return null;
        }

        /// <summary>
        /// Finds a project by its ID.
        /// </summary>
        /// <param name="projectID">The project ID.</param>
        /// <returns>Returns the ProjectClass if found; otherwise null.</returns>
        public async Task<ProjectClass?> FindProjectByID(int projectID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindProjectByID(projectID);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindProjectByID(projectID)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindProjectByID(projectID)");
            }

            return null;
        }


        /// <summary>
        /// Finds an output by its name.
        /// </summary>
        /// <param name="name">The output name.</param>
        /// <returns>Returns the OutputClass if found; otherwise null.</returns>
        public async Task<OutputClass?> FindOutput(string name)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindOutput(name);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindOutput(name)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindOutput(name)");
            }

            return null;
        }

        /// <summary>
        /// Finds an output by its ID.
        /// </summary>
        /// <param name="outputID">The output ID.</param>
        /// <returns>Returns the OutputClass if found; otherwise null.</returns>
        public async Task<OutputClass?> FindOutputByID(int outputID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindOutputByID(outputID);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindOutputByID(outputID)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindOutputByID(outputID)");
            }

            return null;
        }



        /// <summary>
        /// Finds a type by its name.
        /// </summary>
        /// <param name="name">The type name.</param>
        /// <returns>Returns the TypeClass if found; otherwise null.</returns>
        public async Task<TypeClass?> FindType(string name)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindType(name);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindType(name)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindType(name)");
            }

            return null;
        }

        /// <summary>
        /// Finds a type by its ID.
        /// </summary>
        /// <param name="typeID">The type ID.</param>
        /// <returns>Returns the TypeClass if found; otherwise null.</returns>
        public async Task<TypeClass?> FindTypeByID(int typeID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindTypeByID(typeID);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindTypeByID(typeID)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindTypeByID(typeID)");
            }

            return null;
        }


        /// <summary>
        /// Finds a subject by its name and category.
        /// </summary>
        /// <param name="subject">The subject name.</param>
        /// <param name="category">The category of the subject.</param>
        /// <returns>Returns the SubjectClass if found; otherwise null.</returns>
        public async Task<SubjectClass?> FindSubject(string subject, LOG.CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindSubject(subject, category);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindSubject(subject, category)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindSubject(subject, category)");
            }

            return null;
        }

        /// <summary>
        /// Finds the ID of a subject.
        /// </summary>
        /// <param name="subject">The SubjectClass object.</param>
        /// <returns>Returns the subject ID if found; otherwise 0.</returns>
        public async Task<int> FindSubjectID(SubjectClass subject)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FindSubjectID(subject);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FindSubjectID(subject)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FindSubjectID(subject)");
            }

            return 0;
        }













        #region Log Storage






        public void SaveLOG(LOG log, string filePath)
        {
            _cachemaster.SaveLog(log, filePath);
        }

        public async Task<bool> InsertLOG(LOG log)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();

                if (await writer.CreateLOG(log))
                {
                    return true;
                }
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "InsertLOG(log)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "InsertLOG(log)");
            }

            return false;
        }






        #endregion




        #region Log Retrieval




        /// <summary>
        /// Retrieves all logs from the database.
        /// </summary>
        /// <returns>Returns a List of LOG objects, or null if an error occurs.</returns>
        public async Task<List<LOG>?> RetrieveLogs()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.RetrieveLogs();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "RetrieveLogs()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "RetrieveLogs()");
            }

            return null;
        }

        /// <summary>
        /// Retrieves logs from the database for a specific cache context.
        /// </summary>
        /// <param name="context">The cache context to filter logs.</param>
        /// <returns>Returns an IEnumerable of LOG objects, or null if an error occurs.</returns>
        public async Task<IEnumerable<LOG>?> RetrieveLogs(CacheContext context)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();

                switch (context)
                {
                    case CacheContext.Qt:
                        return await reader.RetrieveQtCodingLogs();
                    case CacheContext.AndroidStudio:
                        return await reader.RetrieveAndroidCodingLogs();
                    case CacheContext.Coding:
                        return await reader.RetrieveCodingLogs();
                    case CacheContext.Graphics:
                        return await reader.RetrieveGraphicsLogs();
                    case CacheContext.Film:
                        return await reader.RetrieveFilmLogs();
                    case CacheContext.Flexi:
                        return await reader.RetrieveFlexiNotesLogs();
                }
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "RetrieveLogs(context)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "RetrieveLogs(context)");
            }

            return null;
        }


        /// <summary>
        /// Counts the total number of logs.
        /// </summary>
        /// <returns>The total count of logs, or null if an error occurs.</returns>
        public async Task<int> LogCount()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.LogCount();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "LogCount()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "LogCount()");
            }

            return 0;
        }

        /// <summary>
        /// Counts the number of logs in a specific category.
        /// </summary>
        /// <param name="category">The log category to count.</param>
        /// <returns>The count of logs in the specified category, or null if an error occurs.</returns>
        public async Task<int?> LogCount(LOG.CATEGORY category)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.LogCount(category);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "LogCount(category)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "LogCount(category)");
            }

            return 0;
        }

        /// <summary>
        /// Counts the total number of Qt logs.
        /// </summary>
        /// <returns>The count of Qt logs, or 0 if an error occurs.</returns>
        public async Task<int> QtLogCount()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.QtLogCount();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "QtLogCount()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "QtLogCount()");
            }

            return 0;
        }

        /// <summary>
        /// Counts the total number of Android Studio logs.
        /// </summary>
        /// <returns>The count of AS logs, or 0 if an error occurs.</returns>
        public async Task<int> ASLogCount()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.ASLogCount();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "ASLogCount()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "ASLogCount()");
            }

            return 0;
        }

        /// <summary>
        /// Counts the total number of Flexi Notes logs.
        /// </summary>
        /// <returns>The count of Flexi Notes logs, or 0 if an error occurs.</returns>
        public async Task<int> FlexiLogCountAsync()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();
                return await reader.FlexiNotesLogCount();
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "FlexiLogCountAsync()", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "FlexiLogCountAsync()");
            }

            return 0;
        }










        // Qt
        public async Task<List<CodingLOG>?> SearchQtLogs(string searchBarText)
        {
            return await _reader.SearchQtLogs(searchBarText);
        }
        public async Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID)
        {
            return await _reader.SearchQtLogs(searchBarText, projectID);
        }

        public async Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID, int appID)
        {
            return await _reader.SearchQtLogs(searchBarText, projectID, appID);
        }


        // CODING
        public async Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText)
        {
            return await _reader.SearchCodingLogs(searchBarText);
        }
        public async Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int projectID)
        {
            return await _reader.SearchCodingLogs(searchBarText, projectID);
        }

        public async Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int projectID, int appID)
        {
            return await _reader.SearchCodingLogs(searchBarText, projectID, appID);
        }




        #endregion




        #region Log Management




        #region Updates



        //public async Task<bool> UpdateQtLog(LOG lOG) => await _handler.UpdateQtLog(lOG);

        //public async Task<bool> UpdateNotesLog(NoteItem noteItem) => await _handler.UpdateNotesLog(noteItem);









        #endregion






        #region Deleteions






        public async Task<bool> DeleteLOG(LOG log)
        {
            
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            try
            {
                var handler = scope.ServiceProvider.GetRequiredService<ENTITYHANDLER>();
                return await handler.DeleteLOG(log);
            }
            catch (InvalidOperationException invex)
            {
                await master.HandleExceptionAsync(invex, "DeleteLOG(log)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                await master.HandleExceptionAsync(ex, "DeleteLOG(log)");
            }

            return false;
        }
        public async Task<bool> DeleteLOGByID(int ID) => await _handler.DeleteLOGByID(ID);

        public async Task<bool> DeleteNote(int ID) => await _handler.DeleteNote(ID);













        #endregion






        #endregion


        #region Cache Management




        public void DeleteCacheFile(string id, CacheContext cacheContext)
        {
            _cachemaster.DeleteViewModel(id, cacheContext);
        }






        #endregion




        #region Cache Retrieval




        /// <summary>
        /// Retrieves Qt cache. Qt cache are Qt logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The Qt dashboard/owner that is requesting the cache.</param>
        /// <param name="dataService">The service that will interface with the UI.</param>
        /// <returns>An observable collection of QtLOGViewModels</returns>
        public ObservableCollection<QtLOGViewModel> RetrieveQtCache(LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            return _cachemaster.LoadQtViewModels(logCacheViewModel, dataService, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves Android Studio cache. Android Studio cache are Android Studio logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The Android Studio dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of AndroidLOGViewModels</returns>
        public ObservableCollection<AndroidLOGViewModel> RetrieveASCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadASViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves coding cache. Coding cache are coding logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The coding dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of CodeLOGViewModels</returns>
        public ObservableCollection<CodeLOGViewModel> RetrieveCodeCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadCodeViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves graphics cache. Graphics cache are graphics logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The graphics dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of GraphicsLOGViewModels</returns>
        public ObservableCollection<GraphicsLOGViewModel> RetrieveGraphicsCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadGraphicsViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves film cache. Film cache are film logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The film dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of FilmLOGViewModels</returns>
        public ObservableCollection<FilmLOGViewModel> RetrieveFilmCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadFilmViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        /// <summary>
        /// Retrieves flexible cache. Flexible cache are flexible logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The flexible dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of FlexiLOGViewModels</returns>
        public ObservableCollection<FlexiLOGViewModel> RetrieveFlexibleCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadFlexiViewModels(logCacheViewModel, this, _authService.Account) ?? new();
        }

        #endregion







        #region Feedback Management



        public async Task CreateFeedback(Exception exception, string methodName, string exceptionType = "Exception")
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            await master.HandleExceptionAsync(exception, methodName, exceptionType);
        }



        #endregion

    }
}
