using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static Data_Logger_1._3.Models.NotesLOG;
using static Data_Logger_1._3.Services.ENTITYREADER;

namespace Data_Logger_1._3.Services
{
    public class ENTITYWRITER
    {
        private readonly ENTITYREADER _reader;
        private readonly ENTITYMASTER _master;

        /// <summary>
        /// Official Entity Framework data writer.
        /// </summary>
        public ENTITYWRITER(ENTITYREADER reader, ENTITYMASTER entityMaster)
        {
            _reader = reader;
            _master = entityMaster;
        }


        /// <summary>
        /// Adds an account that is provided as an argument to the database.
        /// </summary>
        /// <param name="account">The account that will be added to the database.</param>
        /// <returns">A boolean value that indicated if the process was successful or not.</returns>
        public async Task<bool> AddAccount(ACCOUNT account)
        {
            bool accountCreated = false;

            try
            {
                if (account is null)
                    throw new ArgumentNullException($"\"{nameof(account)}\" - Account not initialised. Operation aborted.");


                if (!await _reader.EmailExists(account.Email))
                {
                    account.Online = false;

                    var hiddenPassword = account.Password;
                    account.Password = string.Empty;


                    await _master.Accounts.AddAsync(account);
                    await _master.SaveChangesAsync();

                    account.Password = SaltedSHA256Hash(hiddenPassword, account.accountID.ToString());

                    await _master.SaveChangesAsync();


                    accountCreated = true;
                }
                else
                    throw new EmailConflictException("Email exists!");
                
            }
            catch (EmailConflictException mailex)
            {
                await _master.HandleExceptionAsync("AddAccount(account)", mailex, "Error", "The email you entered has been taken. Please use a different one.", "EmailConflictException");
            }
            catch (ArgumentNullException nullex)
            {
                await _master.HandleExceptionAsync("AddAccount(account)", nullex, "Error", "A problem occurred on our end so please try again later. We " +
                    "apologise for any inconvenience caused. Feedback will automatically be sent to us.", "ArgumentNullException");
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync("AddAccount(account)", ex, "Error", "A problem occurred on our end so please try again later. We " +
                    "apologise for any inconvenience caused. Feedback will automatically be sent to us.");
            }

            return accountCreated;

        }



        /// <summary>
        /// Adds an ApplicationClass provided as an argument to the database.
        /// </summary>
        /// <param name="app">The app that will be added to the database.</param>
        protected async Task AddApplicationAsync(ApplicationClass app)
        {
            try
            {
                await _master.Applications.AddAsync(app);
                await _master.SaveChangesAsync();

            }
            catch (OperationCanceledException opex)
            {
                await _master.HandleExceptionAsync(opex, "AddApplicationAsync(app)", "OperationCanceledException");
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "AddApplicationAsync(app)");
            }
        }


        /// <summary>
        /// Adds a ProjectClass.
        /// </summary>
        /// <param name="project">The new project to be added</param>
        private async Task AddProjectAsync(ProjectClass project)
        {
            try
            {

                var exists = await _master.Projects.AnyAsync(item =>
                    item.Name == project.Name &&
                    item.Application.appID == project.Application.appID &&
                    item.Category == project.Category &&
                    item.User.accountID == project.User.accountID
                );

                if(exists)
                    return;

                if (project.Application.appID != 3)
                {
                    await _master.Projects.AddAsync(project);
                    await _master.SaveChangesAsync();
                }
            }
            catch (OperationCanceledException opex)
            {
                await _master.HandleExceptionAsync(opex, "AddProjectAsync(project)", "OperationCanceledException");
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "AddProjectAsync(project)");
            }
        }


        /// <summary>
        /// Adds a subject to the database if it doesn't exist already.
        /// </summary>
        /// <param name="subject">The subject object that will be added.</param>
        public async Task AddSubjectAsync(SubjectClass subject)
        {
            try
            {
                var exists = await _master.Subjects.AnyAsync(s =>
                    s.Subject == subject.Subject &&
                    s.Application.appID == subject.Application.appID &&
                    s.Category == subject.Category &&
                    s.Project.projectID == subject.Project.projectID &&
                    s.User.accountID == subject.User.accountID
                );

                if (exists)
                    return;

                await _master.Subjects.AddAsync(subject);
                await _master.SaveChangesAsync();
            }
            catch (DbUpdateException dbex)
            {
                await _master.HandleExceptionAsync(dbex, "AddSubjectAsync(subject)", "DbUpdateException");
            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "AddSubjectAsync(subject)");
            }
        }







        #region Store Log



        /// <summary>
        /// Creates a log in the database.
        /// </summary>
        /// <param name="log">The log being stored.</param>
        public async Task<bool> CreateLOG(LOG log)
        {
            using var transaction = await _master.Database.BeginTransactionAsync();

            try
            {

                await InsertLogDetails(log);
                await InsertPostItDetails(log);
                await InsertSubLogDetails(log);

                await _master.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (InvalidCastException castx)
            {
                await _master.HandleExceptionAsync(castx, "CreateLOG(log)", "InvalidCastException");

            }
            catch (OperationCanceledException opex)
            {
                await _master.HandleExceptionAsync(opex, "CreateLOG(log)", "OperationCanceledException");

            }
            catch (Exception ex)
            {
                await _master.HandleExceptionAsync(ex, "CreateLOG(log)");
            }
            finally
            {
                if (transaction.GetDbTransaction().Connection != null)
                {
                    await transaction.RollbackAsync();
                }
            }

            return false;
        }


        private async Task InsertLogDetails(LOG log)
        {
            var app = log.Application;
            var temporaryID = await _reader.FindAppID(app);
            if (temporaryID == -1)
            {
                await AddApplicationAsync(app);

                temporaryID = await _reader.FindAppID(app);
            }


            var project = log.Project;
            temporaryID = await _reader.FindProjectID(project);
            if (temporaryID == 1)
            {
                await AddProjectAsync(project);
            }

            await _master.Logs.AddAsync(log);
        }

        private string ValidateString(string input)
        {
            string pattern = @"[\s\S]+";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input) ? input : string.Empty;
        }

        private async Task InsertPostItDetails(LOG log)
        {
            foreach (PostIt postIt in log.PostItList)
            {
                string error = ValidateString(postIt.Error);
                string solution = ValidateString(postIt.Solution);
                string suggestion = ValidateString(postIt.Suggestion);
                string comment = ValidateString(postIt.Comment);

                var subject = postIt.Subject;
                var tempSubjectID = await _reader.FindSubjectID(subject);

                if (tempSubjectID == 1)
                {
                    await AddSubjectAsync(subject);
                }

                await _master.PostIts.AddAsync(postIt);
            }
        }

        private async Task InsertSubLogDetails(LOG log)
        {
            switch (log.Category)
            {
                case LOG.CATEGORY.CODING when log is CodingLOG codingLog:
                    await _master.CodingLogs.AddAsync(codingLog);
                    break;
                case LOG.CATEGORY.GRAPHICS when log is GraphicsLOG graphicsLog:
                    await _master.GraphicsLogs.AddAsync(graphicsLog);
                    break;
                case LOG.CATEGORY.FILM when log is FilmLOG filmLog:
                    await _master.FilmLogs.AddAsync(filmLog);
                    break;
                case LOG.CATEGORY.NOTES when log is NotesLOG notesLog:

                    await _master.NotesLogs.AddAsync(notesLog);

                    switch (notesLog.notelogtype)
                    {
                        case NOTELOGType.FLEXI when notesLog is FlexiNotesLOG flexiNotesLog:
                            {
                                await _master.FlexiNotesLogs.AddAsync(flexiNotesLog);

                                break;
                            }
                        default:
                            {
                                var genericNotesLog = (NoteItem)notesLog;
                                

                                if(genericNotesLog.Checklist is not null && genericNotesLog.Checklist.Items.Count > 0)
                                {
                                    _master.Checklists.Add(genericNotesLog.Checklist);

                                    foreach(var item in  genericNotesLog.Checklist.Items)
                                        await _master.ChecklistItems.AddAsync(item);
                                }
                                else
                                {
                                    await _master.NoteItems.AddAsync(genericNotesLog);
                                }
                                break;
                            }
                    }
                    break;
            }
        }




        #endregion




        #region Security



        public static string SaltedSHA256Hash(string value, string accountID)
        {
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;

                // The account ID is the salt. 
                // So 2 users with the same password have different hashes. 
                // For example, if someone knows their own hash, they can't see who has the same password.
                string input = value + accountID;
                byte[] result = hash.ComputeHash(enc.GetBytes(input));

                StringBuilder hashedStringBuilder = new StringBuilder();
                foreach (byte b in result)
                {
                    hashedStringBuilder.Append(b.ToString("x2"));
                }

                return hashedStringBuilder.ToString();
            }
        }

        public async Task<ACCOUNT> FindAccountByEmail(string email, string password)
        {
            return await _reader.FindAccountByEmail(email, password);
        }

        public async Task<bool> UnsetCurrentUser(ACCOUNT account)
        {
            return await _master.UnsetCurrentUser(account);
        }

        public async Task<bool> SetCurrentUser(ACCOUNT account)
        {
            return await _master.SetCurrentUser(account);
        }

        public async Task HandleExceptionAsync(Exception exception, string methodName, string exceptionType = null)
        {
            await _master.HandleExceptionAsync(exception, methodName, exceptionType);
        }



        #endregion


    }
}
