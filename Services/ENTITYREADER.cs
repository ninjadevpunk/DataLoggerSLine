using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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

        public async Task<List<LOG>> RetrieveLOGS()
        {
            return await _master.Logs
                .Where(l => l.accountID == _master.User.accountID)
                .OrderBy(l => l.Start)
                .ToListAsync();
        }

        public async Task<List<CodingLOG>> RetrieveCodingLogs()
        {
            return await _master.CodingLogs
                .Where(c => c.accountID == _master.User.accountID)
                .OrderBy(c => c.Start)
                .ToListAsync();
        }

        public async Task<List<CodingLOG>> RetrieveQtCodingLogs()
        {
            return await _master.CodingLogs
                .Where(qt => qt.accountID == _master.User.accountID && qt.Application.appID == 1)
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
                    .FirstOrDefaultAsync(a => a.Email == emailAddress);

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

        public async Task<OutputClass> FindOutputByID(int outputID)
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

        public async Task<TypeClass> FindTypeByID(int typeID)
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

                // Retrieve APPLICATION ID
                int appID = await FindAppID(project.Application);

                // If application ID is not 3, perform search and add project if not found
                if (appID != 3)
                {
                    var projectID = await _master.Projects
                        .Where(p => p.Name == project.Name
                        && p.Category == project.Category
                        && p.Application.appID == project.Application.appID
                        && (p.User.accountID == 1 || p.User.accountID == project.User.accountID))
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
                subjectKey = await _master.Subjects
                    .Where(s => s.Subject == subject.Subject
                        && (s.User.accountID == 1 || s.User.accountID == subject.User.accountID)
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
        public async Task<int> LogCount(CATEGORY category)
        {
            try
            {
                return await _master.Logs
                    .Where(l => l.Category == category)
                    .CountAsync();
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
                var codingLogs = _master.Logs
                    .Where(l => l.accountID == _master.User.accountID)
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
                Debug.WriteLine(description);

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
                return await _master.Applications
                    .Where(a => a.accountID == 1 || a.accountID == _master.User.accountID).ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListApplications(category): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
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
                return await _master.Applications
                    .Where(a => a.accountID == 1 || a.accountID == _master.User.accountID)
                    .Where(a => a.Category == category).ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListApplications(category): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
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
                return await _master.Projects
                    .Where(p => p.accountID == 1 || p.accountID == _master.User.accountID).ToListAsync();
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
                return await _master.Projects
                    .Where(p => p.accountID == 1 || p.accountID == _master.User.accountID)
                    .Where(a => a.Category == category).ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListProjects(category): {ex.Message}";

                await _master.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);
            }

            return new();
        }


        public async Task<List<ProjectClass>> ListProjects(ApplicationClass app)
        {
            try
            {
                return await _master.Projects
                    .Where(p => p.accountID == 1 || p.accountID == _master.User.accountID)
                    .Where(a => a.Application == app).ToListAsync();
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
                return await _master.Subjects
                    .Where(s => s.accountID == _master.User.accountID || s.accountID == 1)
                    .Where(s => s.Category == category)
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
                return await _master.Subjects
                    .Where(s => s.accountID == project.accountID || s.accountID == 1)
                    .Where(s => s.Category == project.Category)
                    .Where(s => s.appID == project.appID)
                    .Where(s => s.projectID == project.projectID)
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
                return await _master.Outputs
                    .Where(o => o.accountID == 1 || o.accountID == _master.User.accountID)
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
                    .Where(o => o.accountID == 1 || o.accountID == 1)
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
                    .Where(o => o.accountID == _master.User.accountID || o.accountID == 1)
                    .Where(o => o.Category == category)
                    .Where(o => o.appID != 1 && o.appID != 2)
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
                    .Where(t => t.accountID == 1 || t.accountID == _master.User.accountID)
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
                    .Where(t => t.accountID == 1 || t.accountID == _master.User.accountID)
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
                    .Where(t => t.accountID == _master.User.accountID || t.accountID == 1)
                    .Where(t => t.Category == category)
                    .Where(t => t.appID != 1 && t.appID != 2)
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



        internal List<CodingLOG> SearchQtLogs(string searchBarText, int projectID)
        {
            throw new NotImplementedException();
        }





        #region Exceptions



        public class EmailConflictException : Exception
        {
            public EmailConflictException(string message) : base(message) { }
        }




        #endregion

    }
}
