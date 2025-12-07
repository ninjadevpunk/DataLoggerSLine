using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Data_Logger_1._3.ViewModels.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using static Data_Logger_1._3.Models.FeedbackMessage;
using static Data_Logger_1._3.Models.LOG;

namespace Data_Logger_1._3.Services
{
    public class ENTITYREADER
    {
        private ENTITYMASTER _master;
        private ENTITYWRITER _writer;
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// The Entity Framework data reader for ENTITYMASTER.
        /// </summary>
        public ENTITYREADER(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private async Task<AsyncServiceScope> ActivateMasterAsync()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            _master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            return scope;
        }

        private async Task<AsyncServiceScope> ActivateMasterAsync(AsyncServiceScope scope)
        {
            _master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            return scope;
        }

        private async Task<AsyncServiceScope> ActivateWriterAsync()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            _writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();

            return scope;
        }

        private async Task<AsyncServiceScope> ActivateWriterAsync(AsyncServiceScope scope)
        {
            _writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();

            return scope;
        }

        private async Task ActivateMasterAndWriterAsync()
        {
            var scope = await ActivateMasterAsync();
            await ActivateWriterAsync(scope);
        }

        public async Task<AsyncServiceScope> SaveChangesAsync()
        {
            var scope = await ActivateMasterAsync();

            await _master.SaveChangesAsync();

            return scope;
        }

        public async Task SaveChangesAsync(AsyncServiceScope scope)
        {
            _master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            await _master.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieve a profile pic from on an account with the matching email address.
        /// </summary>
        /// <param name="emailAddress">The email of the account in the database.</param>
        /// <returns>Returns a profile pic file path or an empty string if the path is null.</returns>
        public async Task<string> RetrieveProfilePic(string emailAddress)
        {
            await ActivateMasterAsync();

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
            await ActivateMasterAsync();

            var accountID = await GetOnlineAccountIDAsync();

            return await _master.Logs
                .Where(l => l.accountID == accountID && l.ID == id)
                .SingleOrDefaultAsync();
        }


        public async Task<List<LOG>> RetrieveLogs()
        {
            await ActivateMasterAsync();

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
            await ActivateMasterAsync();

            var accountID = await GetOnlineAccountIDAsync();

            return await _master.CodingLogs
                .Where(c => c.accountID == accountID && !new[] { 1, 2 }.Contains(c.Application.appID))
                .Include(l => l.Author)
                .Include(l => l.Project)
                .Include(l => l.Application)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                .ThenInclude(p => p.Subject)
                .OrderBy(c => c.Start)
                .ToListAsync();
        }

        public async Task<List<CodingLOG>> RetrieveQtCodingLogs()
        {
            await ActivateMasterAsync();

            var accountID = await GetOnlineAccountIDAsync();

            return await _master.CodingLogs
                .Where(qt => qt.accountID == accountID && qt.Application.appID == 1)
                .OrderBy(qt => qt.Start)
                .ToListAsync();
        }

        public async Task<List<AndroidCodingLOG>> RetrieveAndroidCodingLogs()
        {
            await ActivateMasterAsync();

            var accountID = await GetOnlineAccountIDAsync();

            return await _master.AndroidCodingLogs
                .Where(a => a.accountID == accountID && a.Application.appID == 2)
                .OrderBy(a => a.Start)
                .ToListAsync();
        }

        public async Task<List<GraphicsLOG>> RetrieveGraphicsLogs()
        {
            await ActivateMasterAsync();

            var accountID = await GetOnlineAccountIDAsync();

            return await _master.GraphicsLogs
                .Where(g => g.accountID == accountID)
                .OrderBy(g => g.Start)
                .ToListAsync();
        }

        public async Task<List<FilmLOG>> RetrieveFilmLogs()
        {
            await ActivateMasterAsync();

            var accountID = await GetOnlineAccountIDAsync();

            return await _master.FilmLogs
                .Where(f => f.accountID == accountID)
                .OrderBy(f => f.Start)
                .ToListAsync();
        }

        public async Task<List<FlexiNotesLOG>> RetrieveFlexiNotesLogs()
        {
            await ActivateMasterAsync();

            var accountID = await GetOnlineAccountIDAsync();

            return await _master.FlexiNotesLogs
                .Where(fn => fn.accountID == accountID)
                .OrderBy(fn => fn.Start)
                .ToListAsync();
        }


        public async Task<List<NoteItem>> RetrieveGenericNotes(int id)
        {
            await ActivateMasterAsync();

            return await _master.NoteItems
                .Where(n => n.accountID == id)
                .ToListAsync();
        }

        public async Task<List<CheckList>> RetrieveCheckList()
        {
            await ActivateMasterAsync();

            var accountID = await GetOnlineAccountIDAsync();

            return await _master.Checklists
                .Where((cl => cl.accountID == accountID))
                .ToListAsync();
        }

        /// <summary>
        /// Find the account ID of the user who's online.
        /// </summary>
        /// <returns>Returns a single account ID</returns>
        public async Task<int?> GetOnlineAccountIDAsync()
        {
            var scope = await ActivateMasterAsync();
            await ActivateWriterAsync(scope);

            try
            {
                var onlineAccount = await _master.Accounts
                    .SingleAsync(a => a.IsOnline);

                return onlineAccount?.accountID;
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "GetOnlineAccountIDAsync");
            }

            return null;
        }


        public async Task<ACCOUNT> FindAccountByID(int id)
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                return await _master.Accounts
                    .FirstOrDefaultAsync(a => a.accountID == id);
            }
            catch (Exception ex)
            {
                var description = $"Exception near FindAccountByID(id): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return null;
            }
        }

        public async Task<ACCOUNT?> FindAccountByEmail(string emailAddress, string password)
        {
            await ActivateMasterAndWriterAsync();

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

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
                Debug.WriteLine(description);

                return null;
            }
        }

        public async Task<ApplicationClass?> FindApplication(string name)
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                var application = await _master.Applications
                    .FirstOrDefaultAsync(app => app.Name == name);

                return application;
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "FindApplication(name)");
            }

            return null;
        }

        public async Task<ApplicationClass?> FindApplication(int? ID, string name)
        {
            await ActivateMasterAndWriterAsync();

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
                await _writer.HandleExceptionAsync(ex, "FindApplication(name)");
            }

            return null;
        }


        public async Task<ApplicationClass?> FindApplicationByID(int appID)
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                var application = await _master.Applications
                    .FirstOrDefaultAsync(app => app.appID == appID);

                return application;
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "FindApplicationByID(appID)");
            }

            return null;
        }

        public async Task<ProjectClass?> FindProject(int? userID, string projectName, int appID)
        {
            await ActivateMasterAndWriterAsync();

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
                await _writer.HandleExceptionAsync(ex, "FindProject(projectName, appID)");
            }

            return null;
        }


        public async Task<ProjectClass?> FindProjectByID(int projectID)
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                var project = await _master.Projects
                    .FirstOrDefaultAsync((pro => pro.projectID == projectID));

                return project;
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "FindProjectByID(projectID)");
            }

            return null;
        }

        public async Task<OutputClass?> FindOutput(string name)
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                return await _master.Outputs
                    .Where(o => o.accountID == 1 && o.Name == name)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "FindOutput(name)");
            }

            return null;
        }

        public async Task<OutputClass?> FindOutputByID(int outputID)
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                var output = await _master.Outputs
                    .FirstOrDefaultAsync(output => output.outputID == outputID);

                return output;
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "FindOutputByID(outputID)");
            }

            return null;
        }

        public async Task<TypeClass?> FindType(string name)
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                return await _master.Types
                    .Where(t => t.accountID == 1 && t.Name == name)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "FindType(name)");
            }

            return null;
        }

        public async Task<TypeClass?> FindTypeByID(int typeID)
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                var type = await _master.Types
                    .FirstOrDefaultAsync(type => type.typeID == typeID);

                return type;
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "FindTypeByID(typeID)");
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
            var scope = await ActivateMasterAsync();
            await ActivateWriterAsync(scope);

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
                await _writer.HandleExceptionAsync(ex, "EmailExists(email)");

                return true;
            }
        }

        /// <summary>
        /// Checks if an email exists in the database.
        /// </summary>
        /// <param name="scope">The scope for the DbContext.</param>
        /// <param name="email">The email provided by the user signing up.</param>
        /// <returns>Returns whether the email exists or not. Will throw an ExmailConflictException if the email exists.</returns>
        /// <exception cref="EmailConflictException"></exception>
        public async Task<bool> EmailExists(AsyncServiceScope scope, string email)
        {
            await ActivateMasterAsync(scope);
            await ActivateWriterAsync(scope);

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
                await _writer.HandleExceptionAsync(ex, "EmailExists(email)");

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
            await ActivateMasterAsync();

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
            await ActivateMasterAndWriterAsync();

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

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

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
                await _writer.HandleExceptionAsync(nullex, "FindAppID(app)", "ArgumentNullException");
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "FindAppID(app)");
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
            await ActivateMasterAndWriterAsync();

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
                await _writer.HandleExceptionAsync(opex, "FindProjectID(project)", "OperationCanceledException");
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "FindProjectID(project)");
            }

            return 1;
        }



        public async Task<SubjectClass?> FindSubject(string name, LOG.CATEGORY category)
        {
            await ActivateMasterAndWriterAsync();

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
                await _writer.HandleExceptionAsync(ex, "FindType(name)");
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
            await ActivateMasterAndWriterAsync();

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
                await _writer.HandleExceptionAsync(ex, "FindSubjectID(subject)");
            }

            return subjectKey;
        }


        /// <summary>
        /// Counts all logs in the database regardless of category.
        /// </summary>
        /// <returns>Returns the log count.</returns>
        public async Task<int> LogCount()
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                return await _master.Logs.CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception in LogCount: {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

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

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var onlineUserID = await GetOnlineAccountIDAsync();

                var qtLogs = _master.Logs
                    .Where(l => l.accountID == onlineUserID)
                    .Where(l => l.appID == 1)
                    .Where(l => l.Category == LOG.CATEGORY.CODING);

                return await qtLogs.CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception in QtLogCount: {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var onlineUserID = await GetOnlineAccountIDAsync();

                var androidLogs = _master.Logs
                    .Where(l => l.accountID == onlineUserID)
                    .Where(l => l.appID == 2)
                    .Where(l => l.Category == LOG.CATEGORY.CODING);

                return await androidLogs.CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ASLogCount: {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

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

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                var graphicsLogs = _master.Logs.Where(l => l.accountID == accountID)
                    .Where(l => l.Category == LOG.CATEGORY.GRAPHICS);

                return await graphicsLogs.CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near GraphicsLogCount: {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Logs
                    .Where(l => l.accountID == accountID)
                    .Where(l => l.Category == LOG.CATEGORY.FILM).CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near FilmLogCount: {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Logs
                    .Where(l => l.accountID == accountID)
                    .Where(l => l.Category == LOG.CATEGORY.NOTES)
                    .Where(l => l.appID == 15 || l.appID == 16).CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near NoteItemCount: {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Logs
                    .Where(l => l.accountID == accountID)
                    .Where(l => l.Category == LOG.CATEGORY.NOTES)
                    .Where(l => l.appID != 15 && l.appID != 16).CountAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near FlexiNotesLogCount: {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);

                return -1;
            }
        }






        /// <summary>
        /// Retrieves applications from the database that belong to the currently online user.
        /// </summary>
        /// <returns>A List of ApplicationClass objects.</returns>
        public async Task<List<ApplicationClass>> ListApplications()
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Applications
                    .Where(a => new[] { 1, accountID }.Contains(a.accountID))
                    .Where(a => a.Name != "Unknown")
                    .Include(a => a.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListApplications(category): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Applications
                    .Where(a => new[] { 1, accountID }.Contains(a.accountID))
                    .Where(a => !new[] { 1, 2 }.Contains(a.appID))
                    .Where(a => a.Category == category)
                    .Where(a => a.Name != "Unknown")
                    .Include(a => a.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListApplications(category): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
            }

            return new();
        }



        /// <summary>
        /// Retrieves projects from the database that belong to the currently online user.
        /// </summary>
        /// <returns>A List of ProjectClass objects.</returns>
        public async Task<List<ProjectClass>> ListProjects()
        {
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Projects
                    .Where(p => new[] { 1, accountID }.Contains(p.accountID))
                    .Where(p => p.Name != "Unnamed Project")
                    .Include(p => p.User)
                    .Include(p => p.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListProjects(category): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Projects
                    .Where(p => new[] { 1, accountID }.Contains(p.accountID))
                    .Where(p => p.Category == category)
                    .Where(p => p.Name != "Unnamed Project")
                    .Include(p => p.User)
                    .Include(p => p.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListProjects(category): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Projects
                    .Where(p => new[] { 1, accountID }.Contains(p.accountID))
                    .Where(p => p.Application == app)
                    .Where(p => p.Name != "Unnamed Project")
                    .Include(p => p.User)
                    .Include(p => p.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _writer.HandleExceptionAsync(ex, "ListProjects(app)");
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Subjects
                    .Where(s => new[] { 1, accountID }.Contains(s.accountID))
                    .Where(s => s.Category == category)
                    .Where(s => s.Subject != "No Subject")
                    .Include(s => s.User)
                    .Include(s => s.Application)
                    .Include(s => s.Project)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListSubjects(category): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Subjects
                    .Where(s => new[] { 1, accountID }.Contains(s.accountID))
                    .Where(s => s.Category == project.Category)
                    .Where(s => s.appID == project.appID)
                    .Where(s => s.projectID == project.projectID)
                    .Where(s => s.Subject != "No Subject")
                    .Include(s => s.User)
                    .Include(s => s.Application)
                    .Include(s => s.Project)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListSubjects(project): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                var accountID = await GetOnlineAccountIDAsync();

                return await _master.Outputs
                    .Where(o => o.appID == 1)
                    .Include(o => o.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListQtOutputs(): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                return await _master.Outputs
                    .Where(o => o.appID == 2)
                    .Include(o => o.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListASOutputs(): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                return await _master.Outputs
                    .Where(o => o.Category == category)
                    .Where(o => !new[] { 1, 2 }.Contains(o.appID))
                    .Include(o => o.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListOutputs(application): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                return await _master.Types
                    .Where(t => t.appID == 1)
                    .Include(t => t.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListQtTypes(): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                return await _master.Types
                    .Where(t => t.appID == 2)
                    .Include(t => t.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListASTypes(): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

            try
            {
                return await _master.Types
                    .Where(t => t.Category == category)
                    .Where(t => !new[] { 1, 2 }.Contains(t.appID))
                    .Include(t => t.Application)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                var description = $"Exception occurred near ListOutputs(application): {ex.Message}";

                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

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
                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

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
                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
            await ActivateMasterAndWriterAsync();

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
                await _writer.CreateFeedback(1, description, false, true, FeedbackType.Exception);
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
