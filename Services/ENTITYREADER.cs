using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Data_Logger_1._3.ViewModels.Dialogs;
using static Data_Logger_1._3.Models.FeedbackMessage;
using static Data_Logger_1._3.Models.LOG;

namespace Data_Logger_1._3.Services
{
    public class ENTITYREADER
    {
        private readonly ENTITYMASTER _master;


        /// <summary>
        /// The Entity Framework data reader for ENTITYMASTER.
        /// </summary>
        public ENTITYREADER(ENTITYMASTER entityMaster)
        {
            _master = entityMaster;
        }

        public async Task SaveChangesAsync() => await _master.SaveChangesAsync();


        /// <summary>
        /// Retrieve a profile pic from on an account with the matching email address.
        /// </summary>
        /// <param name="emailAddress">The email of the account in the database.</param>
        /// <returns>Returns a profile pic file path or an empty string if the path is null.</returns>
        public async Task<string> RetrieveProfilePic(string emailAddress)
        {
            return await _master.Accounts
                .Where(a => a.Email == emailAddress)
                .Select(a => a.ProfilePic)
                .FirstOrDefaultAsync() ?? string.Empty;

        }



        /// <summary>
        /// Retrieves a single log from the database.
        /// </summary>
        /// <param name="id">The ID of the log.</param>
        /// <returns>Returns only 1 log with the specified ID.</returns>
        public async Task<LOG?> RetrieveLog(int id)
        {
            var accountID = await GetOnlineAccountIDAsync();

            return await _master.Logs
                .Where(l => l.accountID == accountID && l.ID == id)
                .SingleOrDefaultAsync();
        }


        public async Task<List<LOG>> RetrieveLogs()
        {
            var accountID = await GetOnlineAccountIDAsync();

            return await _master.Logs
                .Where(l => l.accountID == accountID)
                .OrderBy(l => l.Start)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves Coding logs.
        /// </summary>
        /// <returns>Returns a list of CodingLOG objects.</returns>
        public async Task<List<CodingLOG>> RetrieveCodingLogs()
        {
            var accountID = await GetOnlineAccountIDAsync();

            return await _master.CodingLogs
                .Where(c => c.accountID == accountID && !new[] { 1, 2 }.Contains(c.Application.appID))
                .Include(l => l.Project)
                .Include(l => l.Application)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                .ThenInclude(p => p.Subject)
                .Include(l => l.PostItList)
                .ThenInclude(p => p.Author)
                .OrderBy(c => c.Start)
                .ToListAsync();
        }

        public async Task<List<CodingLOG>> RetrieveQtCodingLogs()
        {
            var accountID = await GetOnlineAccountIDAsync();

            return await _master.CodingLogs
                .Where(qt => qt.accountID == accountID && qt.Application.appID == 1)
                .OrderBy(qt => qt.Start)
                .ToListAsync();
        }

        public async Task<List<AndroidCodingLOG>> RetrieveAndroidCodingLogs()
        {
            return await _master.AndroidCodingLogs
                .Where(a => a.accountID == _master.User.accountID && a.Application.appID == 2)
                .OrderBy(a => a.Start)
                .ToListAsync();
        }

        public async Task<List<GraphicsLOG>> RetrieveGraphicsLogs()
        {
            return await _master.GraphicsLogs
                .Where(g => g.accountID == _master.User.accountID)
                .OrderBy(g => g.Start)
                .ToListAsync();
        }

        public async Task<List<FilmLOG>> RetrieveFilmLogs()
        {
            return await _master.FilmLogs
                .Where(f => f.accountID == _master.User.accountID)
                .OrderBy(f => f.Start)
                .ToListAsync();
        }

        public async Task<List<FlexiNotesLOG>> RetrieveFlexiNotesLogs()
        {
            return await _master.FlexiNotesLogs
                .Where(fn => fn.accountID == _master.User.accountID)
                .OrderBy(fn => fn.Start)
                .ToListAsync();
        }


        public async Task<List<NoteItem>> RetrieveGenericNotes(int id)
        {
            return await _master.NoteItems
                .Where(n => n.accountID == id)
                .ToListAsync();
        }

        public async Task<List<CheckList>> RetrieveCheckList()
        {
            return await _master.Checklists
                .Where((cl => cl.accountID == _master.User.accountID))
                .ToListAsync();
        }

        /// <summary>
        /// Find the account ID of the user who's online.
        /// </summary>
        /// <returns>Returns a single account ID</returns>
        public async Task<int?> GetOnlineAccountIDAsync()
        {
            try
            {
                var onlineAccount = await _master.Accounts
                    .SingleAsync(a => a.IsOnline);

                return onlineAccount?.accountID;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "GetOnlineAccountIDAsync");
            }

            return null;
        }


        public async Task<ACCOUNT> FindAccountByID(int id)
        {
            try
            {
                return await _master.Accounts
                    .FirstOrDefaultAsync(a => a.accountID == id);
            }
            catch (Exception ex)
            {
                var description = $"Exception near FindAccountByID(id): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return null;
            }
        }

        public async Task<ACCOUNT?> FindAccountByEmail(string emailAddress, string password)
        {
            try
            {
                var account = await _master.Accounts
                    .SingleAsync(a => a.Email == emailAddress);

                if (account is not null && VerifyPassword(account, password))
                {
                    return account;
                }

                return null;
            }
            catch (Exception ex)
            {
                var description = $"Exception near FindAccountByEmail(email,password): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return null;
            }
        }

        public async Task<ApplicationClass?> FindApplication(string name)
        {
            try
            {
                var application = await _master.Applications
                    .FirstOrDefaultAsync(app => app.Name == name);

                return application;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindApplication(name)");
            }

            return null;
        }

        public async Task<ApplicationClass?> FindApplication(int? ID, string name)
        {
            try
            {
                if (ID == null)
                    throw new ArgumentNullException("ID is needed to find application's from Data Logger or user!");

                var application = await _master.Applications
                    .Where(app => app.accountID == ID || app.accountID == 1)
                    .FirstOrDefaultAsync(app => app.Name == name);

                return application;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindApplication(name)");
            }

            return null;
        }


        public async Task<ApplicationClass?> FindApplicationByID(int appID)
        {
            try
            {
                var application = await _master.Applications
                    .FirstOrDefaultAsync(app => app.appID == appID);

                return application;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindApplicationByID(appID)");
            }

            return null;
        }

        public async Task<ProjectClass?> FindProject(int? userID, string projectName, int appID)
        {
            try
            {
                if (userID == null)
                    throw new ArgumentNullException("userID is required to find a user's projects!");

                var project = await _master.Projects
                    .Where(p => (p.accountID == userID || p.accountID == 1)
                                && p.Name == projectName
                                && p.appID == appID)
                    .FirstOrDefaultAsync();

                return project;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindProject(projectName, appID)");
            }

            return null;
        }


        public async Task<ProjectClass> FindProjectByID(int projectID)
        {
            try
            {
                var project = await _master.Projects
                    .FirstOrDefaultAsync((pro => pro.projectID == projectID));

                return project;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindProjectByID(projectID)");
            }

            return null;
        }

        public async Task<OutputClass?> FindOutput(string name)
        {
            try
            {
                return await _master.Outputs
                    .Where(o => o.accountID == 1 && o.Name == name)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindOutput(name)");
            }

            return null;
        }

        public async Task<OutputClass?> FindOutputByID(int outputID)
        {
            try
            {
                var output = await _master.Outputs
                    .FirstOrDefaultAsync(output => output.outputID == outputID);

                return output;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindOutputByID(outputID)");
            }

            return null;
        }

        public async Task<TypeClass?> FindType(string name)
        {
            try
            {
                return await _master.Types
                    .Where(t => t.accountID == 1 && t.Name == name)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindType(name)");
            }

            return null;
        }

        public async Task<TypeClass?> FindTypeByID(int typeID)
        {
            try
            {
                var type = await _master.Types
                    .FirstOrDefaultAsync(type => type.typeID == typeID);

                return type;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindTypeByID(typeID)");
            }

            return null;
        }





        private bool VerifyPassword(ACCOUNT account, string password)
        {
            try
            {
                string hashedPassword = ENTITYWRITER.SaltedSHA256Hash(password, account.accountID.ToString());
                return hashedPassword == account.Password;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near VerifyPassword(account,password): {ex.Message}");

                // TODO

            }

            return false;
        }

        /// <summary>
        /// Checks if an email exists in the database.
        /// </summary>
        /// <param name="email">The email provided by the user signing up.</param>
        /// <returns>Returns whether the email exists or not. Will throw an ExmailConflictException if the email exists.</returns>
        /// <exception cref="EmailConflictException"></exception>
        public async Task<bool> EmailExists(string email)
        {
            try
            {
                var account = await _master.Accounts
                .FirstOrDefaultAsync(a => a.Email == email);

                if (account is not null)
                {
                    throw new EmailConflictException("This email already exists.");
                }

                return false;
            }
            catch (EmailConflictException mailex)
            {
                return true;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "EmailExists(email)");

                return true;
            }
        }

        /// <summary>
        /// Checks if a log with a given logID has PostIts in the PostIt table.
        /// </summary>
        /// <param name="logID">The log ID of the log that is being updated.</param>
        /// <returns>Returns whether the log with the specified logID has PostIts.</returns>
        public async Task<bool> PostItsExists(int logID)
        {
            return await _master.PostIts.AnyAsync(p =>
                    p.logID == logID);
        }



        /// <summary>
        /// Finds an account's ID.
        /// </summary>
        /// <param name="account"></param>
        /// <returns>Returns an account ID.</returns>
        public async Task<int> FindAccountIDAsync(ACCOUNT account)
        {
            try
            {
                var result = await _master.Accounts
                    .Where(a => a.Email == account.Email)
                    .Select(a => a.accountID)
                    .FirstOrDefaultAsync();

                return result != 0 ? result : -1;
            }
            catch (Exception ex)
            {
                var description = $"Exception near FindAccountID(account): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return -1;
            }
        }



        /// <summary>
        /// Will find an ApplicationClass's ID.
        /// </summary>
        /// <param name="app">The application being searched.</param>
        /// <returns>An app ID if found; -1 if not.</returns>
        public async Task<int> FindAppID(ApplicationClass app)
        {
            try
            {
                var appID = await _master.Applications
                    .Where(a => a.Name == app.Name
                                && a.Category == app.Category
                                && (a.User.accountID == 1 || a.User.accountID == app.User.accountID))
                    .Select(a => a.appID)
                    .FirstOrDefaultAsync();

                return appID != 0 ? appID : -1;
            }
            catch (ArgumentNullException nullex)
            {
                await _master.HandleExceptionAsync(nullex, "FindAppID(app)", "ArgumentNullException");
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindAppID(app)");
            }

            return -1;
        }


        /// <summary>
        /// Finds a project's ID.
        /// </summary>
        /// <param name="project">The project being searched.</param>
        /// <returns>Returns an ID for the given project. Returns 1 (Unknown) if not found.</returns>
        public async Task<int> FindProjectID(ProjectClass project)
        {
            if (string.IsNullOrEmpty(project.Name) || project.Name == "Unknown")
                return 1;


            try
            {
                var onlineUserID = await GetOnlineAccountIDAsync();

                // Retrieve APPLICATION ID
                int appID = await FindAppID(project.Application);

                // If application ID is not 3, perform search and add project if not found
                if (appID != 3)
                {
                    var projectID = await _master.Projects
                        .Where(p => p.Name == project.Name
                        && p.Category == project.Category
                        && p.Application.appID == project.Application.appID
                        && (p.User.accountID == 1 || p.User.accountID == onlineUserID))
                        .Select(p => p.projectID)
                        .FirstOrDefaultAsync();

                    if (projectID != 0)
                        return projectID;
                }

            }
            catch (OperationCanceledException opex)
            {
                await _master.HandleExceptionAsync(opex, "FindProjectID(project)", "OperationCanceledException");
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindProjectID(project)");
            }

            return 1;
        }



        public async Task<SubjectClass?> FindSubject(string name, LOG.CATEGORY category)
        {
            try
            {
                var onlineUserID = await GetOnlineAccountIDAsync();

                return await _master.Subjects
                    .Where(s => new[]{1, onlineUserID}.Contains(s.accountID))
                    .Where(s => s.Subject == name)
                    .Where(s => s.Category == category)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindType(name)");
            }

            return null;
        }

        /// <summary>
        /// Looks for a subject ID for the given SubjectClass.
        /// </summary>
        /// <param name="subject">The SubjectClass being searched.</param>
        /// <returns>Returns an ID for the SubjectClass if it exists. 1 returned for nothing found.</returns>
        public async Task<int> FindSubjectID(SubjectClass subject)
        {
            int subjectKey = 1;

            try
            {
                var onlineUserID = await GetOnlineAccountIDAsync();

                subjectKey = await _master.Subjects
                    .Where(s => s.Subject == subject.Subject
                        && (s.accountID == 1 || s.accountID == onlineUserID)
                        && s.Category == subject.Category
                            && s.Application.appID == subject.Application.appID
                                && s.Project.projectID == subject.Project.projectID)
                    .Select(s => s.subjectID)
                    .FirstOrDefaultAsync();

                if (subjectKey != 0)
                    return subjectKey;
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "FindSubjectID(subject)");
            }

            return subjectKey;
        }


        /// <summary>
        /// Counts all logs in the database regardless of category.
        /// </summary>
        /// <returns>Returns the log count.</returns>
        public async Task<int> LogCount()
        {
            try
            {
                return await _master.Logs.CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception in LogCount: {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return -1;
            }
        }

        /// <summary>
        /// Counts all logs in the database regardless of category.
        /// </summary>
        /// <returns>Returns the log count.</returns>
        public async Task<int?> LogCount(CATEGORY category)
        {
            try
            {
                var onlineUserID = await GetOnlineAccountIDAsync();

                return await _master.Logs
                    .Where(l => l.accountID == onlineUserID)
                    .Where(l => l.Category == category)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception in LogCount: {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

            }

            return null;
        }


        /// <summary>
        /// Counts the number of Qt logs.
        /// </summary>
        /// <returns>The count of Qt logs ONLY.</returns>
        public async Task<int> QtLogCount()
        {

            try
            {
                var qtLogs = _master.Logs
                    .Where(l => l.accountID == _master.User.accountID)
                    .Where(l => l.appID == 1)
                    .Where(l => l.Category == LOG.CATEGORY.CODING);

                return await qtLogs.CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception in QtLogCount: {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return -1;
            }
        }

        /// <summary>
        /// Counts the number of Android Studio logs.
        /// </summary>
        /// <returns>The count of Android Studio logs ONLY.</returns>
        public async Task<int> ASLogCount()
        {

            try
            {
                var androidLogs = _master.Logs
                    .Where(l => l.accountID == _master.User.accountID)
                    .Where(l => l.appID == 2)
                    .Where(l => l.Category == LOG.CATEGORY.CODING);

                return await androidLogs.CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ASLogCount: {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
                return -1;
            }


        }

        /// <summary>
        /// Counts the number of coding logs.
        /// </summary>
        /// <returns>Returns the count of coding logs.</returns>
        public async Task<int> CodingLogCount()
        {
            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                var codingLogs = _master.Logs
                    .Where(l => l.accountID == accountID)
                    .Where(l => l.Category == LOG.CATEGORY.CODING)
                    .Where(l => l.appID > 2);

                return await codingLogs.CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near CodingLogCount: {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return -1;
            }
        }


        /// <summary>
        /// Counts the number of gfx logs.
        /// </summary>
        /// <returns>Returns the count of gfx logs.</returns>
        public async Task<int> GraphicsLogCount()
        {
            try
            {
                var graphicsLogs = _master.Logs.Where(l => l.accountID == _master.User.accountID)
                    .Where(l => l.Category == LOG.CATEGORY.GRAPHICS);

                return await graphicsLogs.CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near GraphicsLogCount: {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return -1;
            }
        }


        /// <summary>
        /// Counts the number of film logs.
        /// </summary>
        /// <returns>Returns the count of film logs.</returns>
        public async Task<int> FilmLogCount()
        {
            try
            {
                return await _master.Logs
                    .Where(l => l.accountID == _master.User.accountID)
                    .Where(l => l.Category == LOG.CATEGORY.FILM).CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near FilmLogCount: {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return -1;
            }
        }


        /// <summary>
        /// Counts the number of notes logs.
        /// </summary>
        /// <returns>Returns the count of notes logs.</returns>
        public async Task<int> NoteItemCount()
        {
            try
            {
                return await _master.Logs
                    .Where(l => l.accountID == _master.User.accountID)
                    .Where(l => l.Category == LOG.CATEGORY.NOTES)
                    .Where(l => l.appID == 15 || l.appID == 16).CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near NoteItemCount: {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return -1;
            }
        }


        /// <summary>
        /// Counts the number of flexible notes logs.
        /// </summary>
        /// <returns>Returns the count of flexible notes logs.</returns>
        public async Task<int> FlexiNotesLogCount()
        {
            try
            {
                return await _master.Logs
                    .Where(l => l.accountID == _master.User.accountID)
                    .Where(l => l.Category == LOG.CATEGORY.NOTES)
                    .Where(l => l.appID != 15 && l.appID != 16).CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near FlexiNotesLogCount: {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);

                return -1;
            }
        }






        /// <summary>
        /// Retrieves applications from the database that belong to the currently online user.
        /// </summary>
        /// <returns>A List of ApplicationClass objects.</returns>
        public async Task<List<ApplicationClass>> ListApplications()
        {
            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Applications
                    .Where(a => new[] { 1, accountID }.Contains(a.accountID))
                    .Where(a => a.Name != "Unknown")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListApplications(category): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
            }

            return new();

        }


        /// <summary>
        /// Retrieves applications from the database that belong to the currently online user. Only retrieves by the given category.
        /// </summary>
        /// <param name="category">The category in which the application falls under in terms of logs.</param>
        /// <returns>A List of ApplicationClass objects.</returns>
        public async Task<List<ApplicationClass>> ListApplications(CATEGORY category)
        {
            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Applications
                    .Where(a => new[] { 1, accountID }.Contains(a.accountID))
                    .Where(a => !new[] { 1, 2 }.Contains(a.appID))
                    .Where(a => a.Category == category)
                    .Where(a => a.Name != "Unknown")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListApplications(category): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
            }

            return new();
        }



        /// <summary>
        /// Retrieves projects from the database that belong to the currently online user.
        /// </summary>
        /// <returns>A List of ProjectClass objects.</returns>
        public async Task<List<ProjectClass>> ListProjects()
        {
            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Projects
                    .Where(p => new[] { 1, accountID }.Contains(p.accountID))
                    .Where(a => a.Name != "Unnamed Project")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListProjects(category): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }


        /// <summary>
        /// Retrieves projects from the database that belong to the currently online user. Only retrieves by the given category.
        /// </summary>
        /// <param name="category">The category in which the project falls under in terms of logs.</param>
        /// <returns>A List of ProjectClass objects.</returns>
        public async Task<List<ProjectClass>> ListProjects(CATEGORY category)
        {
            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Projects
                    .Where(p => new[] { 1, accountID }.Contains(p.accountID))
                    .Where(p => p.Category == category)
                    .Where(p => p.Name != "Unnamed Project")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListProjects(category): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }



        /// <summary>
        /// Retrieves projects from the database that belong to the currently online user for a specific application.
        /// Excludes projects with the name "Unnamed Project".
        /// </summary>
        /// <param name="app">The application to filter projects by.</param>
        /// <returns>A List of ProjectClass objects.</returns>
        public async Task<List<ProjectClass>> ListProjects(ApplicationClass app)
        {
            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Projects
                    .Where(p => new[] { 1, accountID }.Contains(p.accountID))
                    .Where(p => p.Application == app)
                    .Where(p => p.Name != "Unnamed Project")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "ListProjects(app)");
            }

            return new();
        }






        /// <summary>
        /// Retrieves subjects in the database based on its category.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task<List<SubjectClass>> ListSubjects(CATEGORY category)
        {
            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Subjects
                    .Where(s => new[] { 1, accountID }.Contains(s.accountID))
                    .Where(s => s.Category == category)
                    .Where(s => s.Subject != "No Subject")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListSubjects(category): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }

        /// <summary>
        /// Retrieves subjects in the database.
        /// </summary>
        /// <param name="project">The project in which the subjects preside.</param>
        /// <returns>A List with SubjectClass objects.</returns>
        public async Task<List<SubjectClass>> ListSubjects(ProjectClass project)
        {
            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Subjects
                    .Where(s => new[] { 1, accountID }.Contains(s.accountID))
                    .Where(s => s.Category == project.Category)
                    .Where(s => s.appID == project.appID)
                    .Where(s => s.projectID == project.projectID)
                    .Where(s => s.Subject != "No Subject")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListSubjects(project): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }


        /// <summary>
        /// Retrieves Qt outputs from the database.
        /// </summary>
        /// <returns>A List of Qt OutputClass objects.</returns>
        public async Task<List<OutputClass>> ListQtOutputs()
        {
            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Outputs
                    .Where(o => o.appID == 1)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListQtOutputs(): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }

        /// <summary>
        /// Retrieves Qt outputs from the database.
        /// </summary>
        /// <returns>A List of Qt OutputClass objects.</returns>
        public async Task<List<OutputClass>> ListASOutputs()
        {
            try
            {
                return await _master.Outputs
                    .Where(o => o.appID == 2)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListASOutputs(): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }

        /// <summary>
        /// Retrieves outputs from the database.
        /// </summary>
        /// <param name="application">The outputs are only found in a particular application.</param>
        /// <returns>A List of OutputClass objects.</returns>
        public async Task<List<OutputClass>> ListOutputs(CATEGORY category)
        {
            try
            {
                return await _master.Outputs
                    .Where(o => o.Category == category)
                    .Where(o => !new[] { 1, 2 }.Contains(o.appID))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListOutputs(application): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }







        /// <summary>
        /// Retrieves Qt outputs from the database.
        /// </summary>
        /// <returns>A List of Qt OutputClass objects.</returns>
        public async Task<List<TypeClass>> ListQtTypes()
        {
            try
            {
                return await _master.Types
                    .Where(t => t.appID == 1)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListQtTypes(): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }

        /// <summary>
        /// Retrieves Qt outputs from the database.
        /// </summary>
        /// <returns>A List of Qt OutputClass objects.</returns>
        public async Task<List<TypeClass>> ListASTypes()
        {
            try
            {
                return await _master.Types
                    .Where(t => t.appID == 2)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListASTypes(): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }

        /// <summary>
        /// Retrieves outputs from the database.
        /// </summary>
        /// <param name="application">The outputs are only found in a particular application.</param>
        /// <returns>A List of OutputClass objects.</returns>
        public async Task<List<TypeClass>> ListTypes(CATEGORY category)
        {
            try
            {
                return await _master.Types
                    .Where(t => t.Category == category)
                    .Where(t => !new[] { 1, 2 }.Contains(t.appID))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListOutputs(application): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }



        public async Task<List<CodingLOG>?> SearchQtLogs(string searchBarText)
        {
            return null;
        }
        public async Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID)
        {
            return null;
        }

        public async Task<List<CodingLOG>?> SearchQtLogs(string searchBarText, int projectID, int appID)
        {
            return null;
        }

        /// <summary>
        /// Searches for logs that match the text provided. All fields are checked to see if they contain the text. Use this method if the PROJECT and APP is not specified.
        /// </summary>
        /// <param name="searchBarText">The query for the database.</param>
        /// <returns>Returns a list of logs that match the search bar text.</returns>
        public async Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText)
        {
            try
            {
                searchBarText = searchBarText.Trim();
                var accountID = await GetOnlineAccountIDAsync();

                var logs = await _master.CodingLogs
                    .Where(l => l.accountID == accountID)
                    .Include(l => l.Project)
                    .Include(l => l.Application)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                    .ThenInclude(postIt => postIt.Subject)
                    .OrderBy(l => l.Start)
                    .ToListAsync();

                var result = logs.Where(l =>
                    l.Project.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                    || l.Application.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                    || l.Start.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                    || l.End.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                    || l.Output.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                    || l.Type.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                    || (l.PostItList != null && l.PostItList.Any(p =>
                        (!string.IsNullOrEmpty(p.Error) && PostItViewModel.ConvertRtfToPlainText(p.Error).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                        || (!string.IsNullOrEmpty(p.Solution) && PostItViewModel.ConvertRtfToPlainText(p.Solution).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                        || (!string.IsNullOrEmpty(p.Suggestion) && PostItViewModel.ConvertRtfToPlainText(p.Suggestion).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                        || (!string.IsNullOrEmpty(p.Comment) && PostItViewModel.ConvertRtfToPlainText(p.Comment).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                        || (p.Subject != null && !string.IsNullOrEmpty(p.Subject.Subject) && p.Subject.Subject.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                    ))
                    || (l.Success && searchBarText.Contains("True", StringComparison.OrdinalIgnoreCase))
                    || (searchBarText.Equals("apps opened", StringComparison.OrdinalIgnoreCase) && l.Success)
                    || (searchBarText.Equals("apps that launched", StringComparison.OrdinalIgnoreCase) && l.Success)
                    || (searchBarText.Equals("launch successful", StringComparison.OrdinalIgnoreCase) && l.Success)
                    || (searchBarText.Equals("launch succesful", StringComparison.OrdinalIgnoreCase) && l.Success)
                    || (searchBarText.Equals("apps that didnt crash", StringComparison.OrdinalIgnoreCase) && l.Success)
                    || (searchBarText.Equals("apps that didn't crash", StringComparison.OrdinalIgnoreCase) && l.Success)
                ).ToList();

                return result;
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near SearchCodingLogs(searchBarText): {ex.Message}";
                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
            }

            return null;
        }



        /// <summary>
        /// Searches for logs that match the text provided. All fields are checked to see if they contain the text. Use this method if the PROJECT is not specified.
        /// </summary>
        /// <param name="searchBarText">The query for the database.</param>
        /// <param name="appID">The application that logs use.</param>
        /// <returns>A list of logs that match the search bar text that were created with the provided app filter.</returns>
        public async Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int appID)
        {
            try
            {
                searchBarText = searchBarText.Trim();
                var accountID = await GetOnlineAccountIDAsync();

                // Fetch only logs for current user first
                var logs = await _master.CodingLogs
                    .Where(l => l.accountID == accountID)
                    .Include(l => l.Project)
                    .Include(l => l.Application)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                    .ThenInclude(postIt => postIt.Subject)
                    .OrderBy(l => l.Start)
                    .ToListAsync();

                // In-memory filtering for search + IDs
                var result = logs.Where(l =>
                    l.Application.appID == appID
                    && (
                           l.Project.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.Application.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.Start.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.End.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.Output.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.Type.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || (l.PostItList != null && l.PostItList.Any(p =>
                            (!string.IsNullOrEmpty(p.Error) && PostItViewModel.ConvertRtfToPlainText(p.Error).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                            || (!string.IsNullOrEmpty(p.Solution) && PostItViewModel.ConvertRtfToPlainText(p.Solution).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                            || (!string.IsNullOrEmpty(p.Suggestion) && PostItViewModel.ConvertRtfToPlainText(p.Suggestion).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                            || (!string.IsNullOrEmpty(p.Comment) && PostItViewModel.ConvertRtfToPlainText(p.Comment).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                            || (p.Subject != null && !string.IsNullOrEmpty(p.Subject.Subject) && p.Subject.Subject.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                        ))
                        || (l.Success && searchBarText.Contains("True", StringComparison.OrdinalIgnoreCase))
                        || (searchBarText.Equals("apps opened", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("apps that launched", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("launch successful", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("launch succesful", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("apps that didnt crash", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("apps that didn't crash", StringComparison.OrdinalIgnoreCase) && l.Success)
                       )
                ).ToList();

                return result;
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near SearchCodingLogs(searchBarText,projectID,appID): {ex.Message}";
                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
            }

            return null;
        }



        /// <summary>
        /// Searches for logs that match the text provided. All fields are checked to see if they contain the text.
        /// </summary>
        /// <param name="searchBarText">The query for the database.</param>
        /// <param name="projectID">The project that the log was created for.</param>
        /// <param name="appID">The application that logs use.</param>
        /// <returns>A list of logs that match the search bar text that were created with the provided app and project filter.</returns>
        public async Task<List<CodingLOG>?> SearchCodingLogs(string searchBarText, int projectID, int appID)
        {
            try
            {
                searchBarText = searchBarText.Trim();
                var accountID = await GetOnlineAccountIDAsync();

                // Fetch only logs for current user first
                var logs = await _master.CodingLogs
                    .Where(l => l.accountID == accountID)
                    .Include(l => l.Project)
                    .Include(l => l.Application)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                    .ThenInclude(postIt => postIt.Subject)
                    .OrderBy(l => l.Start)
                    .ToListAsync();

                // In-memory filtering for search + IDs
                var result = logs.Where(l =>
                       l.Project.projectID == projectID
                    && l.Application.appID == appID
                    && (
                           l.Project.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.Application.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.Start.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.End.ToLongDateString().Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.Output.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || l.Type.Name.Contains(searchBarText, StringComparison.OrdinalIgnoreCase)
                        || (l.PostItList != null && l.PostItList.Any(p =>
                            (!string.IsNullOrEmpty(p.Error) && PostItViewModel.ConvertRtfToPlainText(p.Error).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                            || (!string.IsNullOrEmpty(p.Solution) && PostItViewModel.ConvertRtfToPlainText(p.Solution).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                            || (!string.IsNullOrEmpty(p.Suggestion) && PostItViewModel.ConvertRtfToPlainText(p.Suggestion).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                            || (!string.IsNullOrEmpty(p.Comment) && PostItViewModel.ConvertRtfToPlainText(p.Comment).Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                            || (p.Subject != null && !string.IsNullOrEmpty(p.Subject.Subject) && p.Subject.Subject.Contains(searchBarText, StringComparison.OrdinalIgnoreCase))
                        ))
                        || (l.Success && searchBarText.Contains("True", StringComparison.OrdinalIgnoreCase))
                        || (searchBarText.Equals("apps opened", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("apps that launched", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("launch successful", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("launch succesful", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("apps that didnt crash", StringComparison.OrdinalIgnoreCase) && l.Success)
                        || (searchBarText.Equals("apps that didn't crash", StringComparison.OrdinalIgnoreCase) && l.Success)
                       )
                ).ToList();

                return result;
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near SearchCodingLogs(searchBarText,projectID,appID): {ex.Message}";
                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
            }

            return null;
        }






        #region Exceptions



        public class EmailConflictException : Exception
        {
            public EmailConflictException(string message) : base(message) { }
        }




        #endregion

    }
}
