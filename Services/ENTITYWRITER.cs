using Data_Logger_1._3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using static Data_Logger_1._3.Models.FeedbackMessage;
using static Data_Logger_1._3.Models.NotesLOG;
using static Data_Logger_1._3.Services.ENTITYREADER;

namespace Data_Logger_1._3.Services
{
    public class ENTITYWRITER
    {
        private readonly IServiceProvider _serviceProvider;
        private ENTITYREADER _reader;
        private ENTITYMASTER _master;

        /// <summary>
        /// Official Entity Framework data writer.
        /// </summary>
        public ENTITYWRITER(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }













        private async Task<AsyncServiceScope> ActivateMasterAsync()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            _master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

            return scope;
        }

        private async Task ActivateMasterAsync(AsyncServiceScope scope)
        {
            _master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();
        }

        private async Task<AsyncServiceScope> ActivateReaderAsync()
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            _reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();

            return scope;
        }

        private async Task<AsyncServiceScope> ActivateReaderAsync(AsyncServiceScope scope)
        {
            _reader = scope.ServiceProvider.GetRequiredService<ENTITYREADER>();

            return scope;
        }

        /// <summary>
        /// Sets the currently online user to online.
        /// </summary>
        /// <param name="account">The account that is online.</param>
        /// <returns>Returns whether the account is set successfully.</returns>
        public async Task<bool> SetCurrentUser(ACCOUNT account)
        {
            var scope = await ActivateReaderAsync();
            await ActivateMasterAsync(scope);

            try
            {
                var all = await _master.Accounts.ToListAsync();

                foreach (var a in all)
                    a.IsOnline = (a.accountID == account.accountID);

                await _master.SaveChangesAsync();

                var user = await _master.Accounts.FirstOrDefaultAsync(a => a.accountID == account.accountID);

                return user?.IsOnline ?? false;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, "SetCurrentUser(account)");
            }

            return false;
        }

        /// <summary>
        /// Sets the currently online user to offline.
        /// </summary>
        /// <returns>Returns whether the account is unset successfully.</returns>
        public async Task<bool> UnsetCurrentUser()
        {
            await ActivateMasterAsync();

            try
            {
                var onlineUsers = await _master.Accounts.Where(a => a.IsOnline).ToListAsync();

                if (!onlineUsers.Any())
                    return true;

                foreach (var u in onlineUsers)
                {
                    u.IsOnline = false;
                }

                await _master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, "UnsetCurrentUser(account)");
            }

            return false;
        }


        /// <summary>
        /// Adds an account that is provided as an argument to the database.
        /// </summary>
        /// <param name="account">The account that will be added to the database.</param>
        /// <returns">A boolean value that indicated if the process was successful or not.</returns>
        public async Task<bool> AddAccount(ACCOUNT account)
        {
            var scope = await ActivateMasterAsync();
            await ActivateReaderAsync(scope);

            bool accountCreated = false;

            try
            {
                if (account is null)
                    throw new ArgumentNullException($"\"{nameof(account)}\" - Account not initialised. Operation aborted.");


                if (!await _reader.EmailExists(scope, account.Email))
                {
                    account.IsOnline = false;

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
                await HandleExceptionAsync("AddAccount(account)", mailex, "Error", "The email you entered has been taken. Please use a different one.", "EmailConflictException");
            }
            catch (ArgumentNullException nullex)
            {
                await HandleExceptionAsync("AddAccount(account)", nullex, "Error", "A problem occurred on our end so please try again later. We " +
                    "apologise for any inconvenience caused. Feedback will automatically be sent to us.", "ArgumentNullException");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync("AddAccount(account)", ex, "Error", "A problem occurred on our end so please try again later. We " +
                    "apologise for any inconvenience caused. Feedback will automatically be sent to us.");
            }

            return accountCreated;

        }










        #region Store Log



        /// <summary>
        /// Creates a log in the database.
        /// </summary>
        /// <param name="log">The log being stored.</param>
        public async Task<bool> CreateLOG(LOG log)
        {
            var scope = await ActivateMasterAsync();

            await using var transaction = await _master.Database.BeginTransactionAsync();

            try
            {
                await PrepareLogDetails(log, scope);
                await PreparePostItDetails(log, scope);

                await InsertSubLogDetails(log);

                await _master.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (InvalidCastException castx)
            {
                await HandleExceptionAsync(castx, "CreateLOG(log)", "InvalidCastException");

            }
            catch (OperationCanceledException opex)
            {
                await HandleExceptionAsync(opex, "CreateLOG(log)", "OperationCanceledException");

            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, "CreateLOG(log)");
            }
            finally
            {
                var dbConnection = transaction.GetDbTransaction().Connection;
                if (dbConnection != null && !dbConnection.State.HasFlag(ConnectionState.Closed))
                {
                    await transaction.RollbackAsync();
                }
            }

            return false;
        }


        private async Task PrepareLogDetails(LOG log, AsyncServiceScope scope)
        {
            await ActivateReaderAsync(scope);

            var onlineUser = await _reader.FindAccountByID((int)await _reader.GetOnlineAccountIDAsync());
            var userID = onlineUser.accountID;

            log.Author = onlineUser;
            var app = log.Application;
            app.User = onlineUser;

            var existingApp = await _reader.FindApplication(userID, app.Name);

            var project = log.Project;
            project.User = onlineUser;

            log.Output = await _reader.FindOutput(log.Output.Name);
            log.Type = await _reader.FindType(log.Type.Name);

            if (existingApp != null)
            {
                app = existingApp;

                var existingProject = await _reader.FindProject(userID, project.Name, app.appID);

                if (existingProject != null)
                {
                    project = existingProject;
                }

            }
            else if (project.Name.Contains("Unnamed Project", StringComparison.OrdinalIgnoreCase))
            {
                var existingProject = await _reader.FindProject(userID, project.Name, app.appID);

                if (existingProject != null)
                {
                    project = existingProject;
                }
            }

            log.Application = app;
            project.Application = app;
            log.Project = project;
        }

        private string ValidateString(string input)
        {
            string pattern = @"[\s\S]+";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input) ? input : string.Empty;
        }

        private async Task PreparePostItDetails(LOG log, AsyncServiceScope scope)
        {
            await ActivateReaderAsync(scope);

            var onlineUser = await _reader.FindAccountByID((int)await _reader.GetOnlineAccountIDAsync());

            foreach (PostIt postIt in log.PostItList)
            {

                if (postIt.Subject != null)
                {
                    postIt.Subject.User = onlineUser;
                    postIt.Subject.Application = log.Application;
                    postIt.Subject.Project = log.Project;

                    var existingSubject = await _reader.FindSubject(postIt.Subject.Subject, log.Category);

                    if (existingSubject != null)
                    {
                        postIt.Subject = existingSubject;
                    }

                    postIt.Author = onlineUser;
                }
            }
        }

        private async Task InsertSubLogDetails(LOG log)
        {
            switch (log.Category)
            {
                case LOG.CATEGORY.CODING when log is CodingLOG codingLog:

                    if (codingLog is AndroidCodingLOG androidCodingLog)
                    {
                        await _master.AndroidCodingLogs.AddAsync(androidCodingLog);
                    }
                    else
                    {
                        await _master.CodingLogs.AddAsync(codingLog);
                    }

                    break;
                case LOG.CATEGORY.GRAPHICS when log is GraphicsLOG graphicsLog:
                    await _master.GraphicsLogs.AddAsync(graphicsLog);
                    break;
                case LOG.CATEGORY.FILM when log is FilmLOG filmLog:
                    await _master.FilmLogs.AddAsync(filmLog);
                    break;
                case LOG.CATEGORY.NOTES when log is NotesLOG notesLog:

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


                                if (genericNotesLog.Checklist is not null && genericNotesLog.Checklist.Items.Count > 0)
                                {
                                    _master.Checklists.Add(genericNotesLog.Checklist);

                                    foreach (var item in genericNotesLog.Checklist.Items)
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




        #endregion





        #region Feedback Creation



        public async Task<int> CreateFeedback(int accountID, string description, bool canContact, bool isAutoFeed = true, FeedbackType category = FeedbackType.Bug)
        {
            try
            {
                var feedback = new FeedbackMessage
                {
                    accountID = accountID,
                    Description = description,
                    CanContact = canContact,
                    Category = category,
                    DateReported = DateTime.Now,
                    IsAutoFeed = isAutoFeed
                };

                _master.AllFeedback.Add(feedback);
                await _master.SaveChangesAsync();

                return feedback.feedbackID;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in CreateFeedback: {ex.Message}. Inner exception: {ex.InnerException.Message ?? ""}");
                return -1;
            }
        }

        public async Task HandleExceptionAsync(string methodName, Exception ex, string messageBoxCaption, string messageBoxMessage = "A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.", string exceptionType = "Exception")
        {
            // Build the exception description
            var description = $"{exceptionType ?? "Exception"} occurred near {methodName}: {ex.Message}" +
                  (ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : "");


            // Log to CreateFeedback
            await CreateFeedback(1, description, false, true, FeedbackType.Exception);

            // Show a message box only if showMessageBox is true
            if (!string.IsNullOrEmpty(messageBoxCaption))
            {
                MessageBox.Show(messageBoxMessage,
                                messageBoxCaption, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        public async Task HandleExceptionAsync(Exception ex, string methodName, string exceptionType = null)
        {
            // Build the exception description
            var description = $"{exceptionType ?? "Exception"} occurred near {methodName}: {ex.Message}" +
                  (ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : "");

            // Log to CreateFeedback
            await CreateFeedback(1, description, false, true, FeedbackType.Exception);
        }






        #endregion


    }
}
