using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using static Data_Logger_1._3.Models.AndroidCodingLOG;

namespace Data_Logger_1._3.Services
{

    public class DATAWRITER : DATAMASTER
    {
        protected readonly DATAREADER _reader;

        /* CONSTRUCTORS */
        public DATAWRITER() { }

        public DATAWRITER(DATAREADER reader)
        {
            _reader = reader;
        }

        public DATAWRITER(string category, SQLiteConnection connection, bool status) : base(connection, category, status) { }

        // ---END CONSTRUCTORS---


        #region Feedback Management



        public void CreateFeedbackID()
        {
            // TODO
        }

        public void Feedbacker(string description)
        {
            // TODO
        }















        #endregion



        /// <summary>
        /// Adds an account that is provided as an argument to the database.
        /// </summary>
        /// <param name="account">The account that will be added to the database.</param>
        /// <returns">A boolean value that indicated if the process was successful or not.</returns>
        public bool AddAccount(ACCOUNT account)
        {
            bool accountCreated = false;

            try
            {
                if (account is null)
                    throw new ArgumentNullException("Account not initialised. Operation aborted.");


                _reader.EmailExists(account.Email);

                string hashedPassword = SaltedSHA256Hash(account.Password, account.ID.ToString());

                using (SQLiteCommand insert = _con.CreateCommand())
                {
                    insert.CommandText = $@"INSERT INTO ACCOUNT({Column.AccountID}, profilepic, first, last, email, password, IsEmployee, companyName, companyAddress, companyLogo, online)
                                                        VALUES(@account, @profilePic, @firstName, @lastName, @email, @password, 
                                                            @isEmployee, @companyName, @companyAddress, @companyLogo, @online)
;";
                    insert.Parameters.AddWithValue("@account", account.ID);
                    insert.Parameters.AddWithValue("@profilePic", account.ProfilePic);
                    insert.Parameters.AddWithValue("@firstName", account.FirstName);
                    insert.Parameters.AddWithValue("@lastName", account.LastName);
                    insert.Parameters.AddWithValue("@email", account.Email);
                    insert.Parameters.AddWithValue("@password", hashedPassword);
                    insert.Parameters.AddWithValue("@isEmployee", account.IsEmployee);
                    insert.Parameters.AddWithValue("@companyName", account.CompanyName);
                    insert.Parameters.AddWithValue("@companyAddress", account.CompanyAddress);
                    insert.Parameters.AddWithValue("@companyLogo", account.CompanyLogo);
                    insert.Parameters.AddWithValue("@online", false);

                    insert.ExecuteNonQuery();
                }

                accountCreated = true;
            }
            catch (EmailConflictException mailex)
            {
                Debug.WriteLine($"EmailConflictException found near AddAccount(account): {mailex.Message}");

                MessageBox.Show($"The email you entered has been taken. Please use a different one.", "Email Exists", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException nullex)
            {
                Debug.WriteLine($"ArgumentNullException found near AddAccount(account): {nullex.Message}");

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                   "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near AddAccount(account): {ex.Message}");

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                   "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            return accountCreated;

        }

        /// <summary>
        /// Adds an ApplicationClass provided as an argument to the database.
        /// </summary>
        /// <param name="app">The app that will be added to the database.</param>
        protected void AddApplication(ApplicationClass app)
        {
            try
            {
                using SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = $@"INSERT INTO APPLICATION({Column.AppID}, {Column.AccountID}, application, {Column.CategoryID}, IsDefault)
                            VALUES(@app, @account, @appName, @category, @isDefault)
;";
                insert.Parameters.AddWithValue("@app", app.AppID);
                insert.Parameters.AddWithValue("@account", User.ID);
                insert.Parameters.AddWithValue("@appName", app.Name);
                insert.Parameters.AddWithValue("@category", _reader.FindCategoryID(app.Category));
                insert.Parameters.AddWithValue("@isDefault", 0);

                insert.ExecuteNonQuery();

                if (Watcher.AppID > 0)
                    --Watcher.AppID;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception near AddApplication(appClass): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near AddApplication(appClass): {ex.Message}");

                // TODO
            }
        }

        /// <summary>
        /// Adds a ProjectClass provided as an argument to the database.
        /// </summary>
        /// <param name="project">The project that will be added to the database.</param>
        private void AddProject(ProjectClass project)
        {
            try
            {
                var existingProjects = _reader.ListProjects(project.Application);

                foreach (ProjectClass item in existingProjects)
                {
                    if (item.Name.Equals(project.Name) &&
                        item.Application.Equals(project.Application) &&
                        item.Category.Equals(project.Category) &&
                        item.User.Equals(project.User)
                        )
                    {
                        return;
                    }
                }

                if (project.Application.AppID != 3)
                {
                    using SQLiteCommand insert = _con.CreateCommand();
                    insert.CommandText = $@"INSERT INTO PROJECT({Column.ProjectID}, {Column.AppID}, {Column.AccountID}, {Column.CategoryID}, {Column.project})
                                    VALUES(@id, @app, @account, @category, @project)
;";
                    insert.Parameters.AddWithValue("@id", project.ProjectID);
                    insert.Parameters.AddWithValue("@app", project.Application.AppID);
                    insert.Parameters.AddWithValue("@account", project.User.ID);
                    insert.Parameters.AddWithValue("@category", _reader.FindCategoryID(project.Category));
                    insert.Parameters.AddWithValue("@project", project.Name);

                    insert.ExecuteNonQuery();

                    // Reset the ProjectID watcher
                    if (Watcher.ProjectID > 0)
                        --Watcher.ProjectID;
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite error: {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding project: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a subject to the database if it doesn't exist already.
        /// </summary>
        /// <param name="subject">The subject object that will be added.</param>
        private void AddSubject(SubjectClass subject)
        {
            try
            {
                // Ensure subject is not already added for the specified project
                var existingProjects = _reader.ListSubjects(subject.Project);
                bool exists = false;

                foreach (SubjectClass item in existingProjects)
                {
                    if (item.Subject.Equals(subject.Subject) &&
                        item.Application.Equals(subject.Application) &&
                        item.Category.Equals(subject.Category) &&
                        item.User.Equals(subject.User)
                        )
                    {
                        return;
                    }
                }

                using SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = $@"INSERT INTO Subject(subjectID, {Column.ProjectID}, {Column.AppID}, {Column.AccountID}, {Column.CategoryID}, subject)
                                    VALUES(@id, @project, @app, @account, @category, @subject)
;";
                insert.Parameters.AddWithValue("@id", subject.SubjectID);
                insert.Parameters.AddWithValue("@project", subject.Project.ProjectID);
                insert.Parameters.AddWithValue("@app", subject.Application.AppID);
                insert.Parameters.AddWithValue("@account", subject.User.ID);
                insert.Parameters.AddWithValue("@category", _reader.FindCategoryID(subject.Category));
                insert.Parameters.AddWithValue("@subject", subject.Subject);

                insert.ExecuteNonQuery();

                // Reset the SubjectID watcher
                if (Watcher.SubjectID > 0)
                    --Watcher.SubjectID;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException near AddSubject(subjectClass): {sqlex.Message}");

                // Delete the log in the database with the log ID that's currently stored in currentLogID.
                // Rollback Sqlite code
                if (currentLogID > 0)
                {
                    try
                    {
                        using (SQLiteCommand deleteLog = _con.CreateCommand())
                        {
                            deleteLog.CommandText = $"DELETE FROM LOG WHERE {Column.LogID} = @logID";
                            deleteLog.Parameters.AddWithValue("@logID", currentLogID);
                            deleteLog.ExecuteNonQuery();
                        }
                    }
                    catch (Exception deleteEx)
                    {
                        Debug.WriteLine($"Exception near AddSubject inner try-catch: {deleteEx.Message}");
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near AddSubject(subjectClass): {ex.Message}");

                // TODO
            }
        }

        /// <summary>
        /// Adds a CheckListItem to the Checklist table.
        /// </summary>
        /// <param name="id">The item's ID.</param>
        /// <param name="logID">The NotesLOG ID.</param>
        /// <param name="isChecked">Whether the item was checked or not.</param>
        /// <param name="item">The item that needed to be done.</param>
        protected void AddCheckListItem(int id, int logID, bool isChecked, string item)
        {
            using SQLiteCommand insert = _con.CreateCommand();
            insert.CommandText = @"INSERT INTO Checklist(itemsID, logID, item, done)
                               VALUES(@id, @logID, @item, @done)";
            insert.Parameters.AddWithValue("@id", id);
            insert.Parameters.AddWithValue("@logID", logID);
            insert.Parameters.AddWithValue("@item", item);
            insert.Parameters.AddWithValue("@done", isChecked);

            insert.ExecuteNonQuery();

            --Watcher.ChecklistItemID;
        }






        public int CreateAccountID()
        {
            SQLiteCommand query = _con.CreateCommand();
            const int baseID = 1;

            query.CommandText = $"SELECT MAX({Column.AccountID}) FROM ACCOUNT;";

            object result = query.ExecuteScalar();
            int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;

            return ++maxID;
        }

        /* Create ID */
        public int CreateLogID()
        {
            SQLiteCommand query = _con.CreateCommand();

            // Define the starting ID
            const int baseID = 100000001;

            // Get the highest log ID currently in the database
            query.CommandText = $"SELECT MAX({Column.LogID}) FROM LOG;";

            object result = query.ExecuteScalar();
            int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;

            int id = ++maxID + Watcher.LogID++;

            currentLogID = id;

            return id;
        }


        /* Create Log Note ID. */
        public int CreatePostItID(List<int>? unUsedIDs)
        {
            int id;
            Watcher.AvailablePostItIDs = unUsedIDs;
            if (Watcher.AvailablePostItIDs is not null && Watcher.AvailablePostItIDs.Count > 0)
            {
                id = Watcher.AvailablePostItIDs[0];
                Watcher.AvailablePostItIDs.RemoveAt(0);

                return id;
            }

            SQLiteCommand query = _con.CreateCommand();
            const int baseID = 12000001;

            query.CommandText = $"SELECT MAX({Column.PostItID}) FROM PostIt;";

            object result = query.ExecuteScalar();
            int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;


            id = ++maxID;

            return id + Watcher.PostItID++;
        }



        public int CreateProjectID(ACCOUNT account, ApplicationClass app, string project)
        {
            SQLiteCommand query = _con.CreateCommand();
            int baseID = 1;
            bool found = false;

            query.CommandText = $"SELECT MAX({Column.ProjectID}) FROM PROJECT;";

            object result = query.ExecuteScalar();
            int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;

            SQLiteDataReader read;
            query.CommandText = $"SELECT * FROM PROJECT;";
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetInt32(3) == _reader.FindCategoryID(app.Category) &&
                    read.GetInt32(2) == account.ID &&
                    read.GetInt32(1) == app.AppID &&
                    read.GetString(4) == project)
                {
                    baseID = read.GetInt32(Column.IDColumn);
                    found = true;
                    break;
                }
            }


            read.Close();

            return found ? baseID : ++maxID + Watcher.ProjectID++;
        }

        public int CreateAppID()
        {
            SQLiteCommand query = _con.CreateCommand();
            int baseID = 1;

            query.CommandText = $"SELECT MAX({Column.AppID}) FROM APPLICATION;";

            object result = query.ExecuteScalar();
            int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;

            int id = ++maxID;

            return id + Watcher.AppID++;
        }

        public int CreateAppID(LOG.CATEGORY category, ACCOUNT account, string applicationName)
        {
            if (string.IsNullOrEmpty(applicationName))
                return 3;

            SQLiteCommand query = _con.CreateCommand();
            int baseID = 1;
            bool found = false;

            query.CommandText = $"SELECT MAX({Column.AppID}) FROM APPLICATION;";

            object result = query.ExecuteScalar();
            int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;

            var categoryColumn = 3;
            var accountColumn = 1;
            var appColumn = 2;

            SQLiteDataReader read;
            query.CommandText = $"SELECT * FROM APPLICATION;";
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetInt32(categoryColumn) == _reader.FindCategoryID(category) &&
                    (read.GetInt32(accountColumn) == _reader.FindAccountID(account) ||
                        read.GetInt32(accountColumn) == 1) &&
                    read.GetString(appColumn) == applicationName)
                {
                    baseID = read.GetInt32(Column.IDColumn);
                    found = true;
                    break;
                }

            }

            read.Close();

            return found ? baseID : ++maxID + Watcher.AppID++;
        }


        public int CreateSubjectID()
        {
            SQLiteCommand query = _con.CreateCommand();
            int baseID = 1;

            query.CommandText = $"SELECT MAX({Column.SubjectID}) FROM Subject;";

            object result = query.ExecuteScalar();
            int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;

            int id = ++maxID;

            return id + Watcher.SubjectID++;
        }

        public int CreateSubjectID(ProjectClass project, string subject, List<int>? unUsedIDs)
        {
            try
            {
                int baseID = 1, dynamicID = 1;

                Watcher.AvailableSubjectIDs = unUsedIDs;
                if (Watcher.AvailableSubjectIDs is not null && Watcher.AvailableSubjectIDs.Count > 0)
                {
                    dynamicID = Watcher.AvailableSubjectIDs[0];
                    Watcher.AvailableSubjectIDs.RemoveAt(0);

                    return dynamicID;
                }

                SQLiteCommand query = _con.CreateCommand();
                const string NS = "No Subject";
                bool found = false;

                var categoryColumn = 4;
                var accountColumn = 3;
                var appColumn = 2;
                var projectColumn = 1;
                var subjectColumn = 5;

                if (subject == String.Empty || subject == NS)
                {
                    subject = NS;
                    return 1;
                }

                query.CommandText = $"SELECT MAX({Column.SubjectID}) FROM Subject;";

                object result = query.ExecuteScalar();
                int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;

                SQLiteDataReader read;
                query.CommandText = $"SELECT * FROM Subject;";
                read = query.ExecuteReader();

                while (read.Read())
                {
                    if (read.GetInt32(categoryColumn) == _reader.FindCategoryID(project.Category) &&
                        (read.GetInt32(accountColumn) == project.User.ID ||
                        read.GetInt32(accountColumn) == 1) &&
                        read.GetInt32(appColumn) == project.Application.AppID &&
                        read.GetInt32(projectColumn) == project.ProjectID &&
                        read.GetString(subjectColumn) == subject)
                    {
                        found = true;
                        baseID = read.GetInt32(Column.IDColumn);
                        break;
                    }
                }

                read.Close();

                return found ? baseID : ++maxID + Watcher.SubjectID++;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near CreateSubjectID(): {ex.Message}");

                // TODO


            }

            return 1;
        }

        public int CreateOutputID(ACCOUNT account, ApplicationClass app, string output)
        {
            SQLiteCommand query = _con.CreateCommand();
            int id = 1;

            var categoryColumn = 3;
            var accountColumn = 1;
            var appColumn = 2;
            var outputColumn = 4;

            query.CommandText = "SELECT * FROM OUTPUT;";

            SQLiteDataReader read;
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetInt32(categoryColumn) == _reader.FindCategoryID(app.Category) &&
                    (read.GetInt32(accountColumn) == account.ID ||
                    read.GetInt32(accountColumn) == 1) &&
                    read.GetInt32(appColumn) == app.AppID &&
                    read.GetString(outputColumn) == output)
                {
                    id = read.GetInt32(Column.IDColumn);
                    break;
                }
            }

            read.Close();

            return id;
        }

        public int CreateTypeID(ACCOUNT account, ApplicationClass app, string type)
        {
            SQLiteCommand query = _con.CreateCommand();
            int id = 1;

            var categoryColumn = 3;
            var accountColumn = 1;
            var appColumn = 2;
            var typeColumn = 4;

            query.CommandText = "SELECT * FROM TYPE;";

            SQLiteDataReader read;
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetInt32(categoryColumn) == _reader.FindCategoryID(app.Category) &&
                    (read.GetInt32(accountColumn) == account.ID ||
                    read.GetInt32(accountColumn) == 1) &&
                    read.GetInt32(appColumn) == app.AppID &&
                    read.GetString(typeColumn) == type)
                {
                    id = read.GetInt32(Column.IDColumn);
                    break;
                }
            }

            read.Close();

            return id;
        }



        public int CreateChecklistItemID()
        {
            SQLiteCommand query = _con.CreateCommand();
            int baseID = 1;

            query.CommandText = $"SELECT MAX({Column.ChecklistItemID}) FROM Checklist;";

            object result = query.ExecuteScalar();
            int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;

            int id = ++maxID;

            return id + Watcher.ChecklistItemID++;
        }

        /// <summary>
        /// Creates a batch of logs in the database from the DATAMASTER repo.
        /// </summary>
        public void CreateLOGS()
        {

            SQLiteCommand query = _con.CreateCommand();

            /** Check that the PostIt has valid input and not empty strings */
            string pattern = "[A-Za-z]{5}[0-9]{0}";
            Regex xp = new Regex(pattern);




            foreach (LOG cachedLOG in this)
            {


                query.CommandText = $@"INSERT INTO LOG(logID, {Column.CategoryID}, {Column.AccountID}, {Column.ProjectID}, {Column.AppID}, start, end, outputID, typeID)
                                VALUES(@id, @category, @author, @project, @app, @start, @end, @output, @type)
;";

                query.Parameters.AddWithValue("@id", cachedLOG.ID);
                query.Parameters.AddWithValue("@category", _reader.FindCategoryID(cachedLOG.Category));
                query.Parameters.AddWithValue("@author", cachedLOG.Author.ID);

                var tempProjectID = _reader.FindProjectID(cachedLOG.Project);
                query.Parameters.AddWithValue("@project", tempProjectID);

                var app = cachedLOG.Application;
                var tempAppID = _reader.FindAppID(app);
                if (!AppAlreadyAdded && tempAppID == -1)
                {
                    AddApplication(app);
                    AppAlreadyAdded = true;

                    tempAppID = _reader.FindAppID(app);
                }

                query.Parameters.AddWithValue("@app", tempAppID);


                // TODO Check that times can be read properly by RetrieveLOGS()
                query.Parameters.AddWithValue("@start", cachedLOG.Start.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                query.Parameters.AddWithValue("@end", cachedLOG.End.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                query.Parameters.AddWithValue("@output", _reader.FindOutputID(cachedLOG.Output));
                query.Parameters.AddWithValue("@type", _reader.FindTypeID(cachedLOG.Type));

                query.ExecuteNonQuery();

                /** LOG NOTE **/
                string er = "", so = "", su = "", co = "";

                foreach (PostIt postIt in cachedLOG.PostItList)
                {
                    er = xp.IsMatch(postIt.Error) ?
                        postIt.Error : "";

                    so = xp.IsMatch(postIt.Solution) ?
                        postIt.Solution : "";

                    su = xp.IsMatch(postIt.Suggestion) ?
                        postIt.Suggestion : "";

                    co = xp.IsMatch(postIt.Comment) ?
                        postIt.Comment : "";

                    query.CommandText =
                            @"INSERT INTO PostIt(logNoteID, logID, subjectID, error, date_found, solution, date_solved, suggestion, comment)
                                        VALUES(@id, @log, @subject, @error, @found, @solution, @solved, @suggest, @comment);
                ";

                    query.Parameters.AddWithValue("@id", postIt.ID);
                    query.Parameters.AddWithValue("@log", cachedLOG.ID);

                    var subject = postIt.Subject;
                    var tempSubjectID = _reader.FindSubjectID(subject);

                    if (!SubjectAlreadyAdded && tempSubjectID == -1)
                    {
                        AddSubject(subject);
                        SubjectAlreadyAdded = true;

                        tempSubjectID = _reader.FindSubjectID(subject);
                    }

                    // Subject
                    if (tempSubjectID == -1)
                        query.Parameters.AddWithValue("@subject", 1);
                    else
                        query.Parameters.AddWithValue("@subject", tempSubjectID);

                    // ERROR
                    query.Parameters.AddWithValue("@error", er);
                    // Date Time
                    DateTime datum;
                    datum = postIt.ERCaptureTime;
                    // TODO Make sure times can be read properly by RetrieveLOGS()
                    if (xp.IsMatch(er))
                        query.Parameters.AddWithValue("@found", datum.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                    else
                        query.Parameters.AddWithValue("@found", "");

                    // SOLUTION
                    query.Parameters.AddWithValue("@solution", so);
                    datum = postIt.SOCaptureTime;
                    // TODO Make sure times can be read properly by RetrieveLOGS()
                    if (xp.IsMatch(so))
                        query.Parameters.AddWithValue("@solved", datum.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                    else
                        query.Parameters.AddWithValue("@solved", "");

                    // SUGGESTION
                    if (xp.IsMatch(su))
                        query.Parameters.AddWithValue("@suggest", su);
                    else
                        query.Parameters.AddWithValue("@suggest", "");

                    // COMMENT
                    if (xp.IsMatch(co))
                        query.Parameters.AddWithValue("@comment", co);
                    else
                        query.Parameters.AddWithValue("@comment", "");

                    query.ExecuteNonQuery();

                }


                switch (cachedLOG.Category)
                {
                    case LOG.CATEGORY.CODING:
                        CodingLOG ctemp = (CodingLOG)cachedLOG;

                        query.CommandText = @"INSERT INTO CodingLOG(logID, bugs, opened)
                                                VALUES(@id, @bugs, @opened);";

                        query.Parameters.AddWithValue("@id", cachedLOG.ID);
                        query.Parameters.AddWithValue("@bugs", ctemp.Bugs);
                        query.Parameters.AddWithValue("@opened", ctemp.Success);

                        query.ExecuteNonQuery();

                        if (ctemp.Application.Name == Android)
                        {
                            var acl = (AndroidCodingLOG)ctemp;

                            if (acl.Scope == SCOPE.FULL)
                                query.CommandText = @"INSERT INTO AndroidCodingLOG(isSimple, sync, gradleDaemon, runBuild, loadBuild, configBuild, allProjects)
                                                    VALUES(@fs, @s, @GD, @rb, @lb, @cb, @ap)";
                            else
                                query.CommandText = @"INSERT INTO AndroidCodingLOG(isSimple, sync)
                                                    VALUES(@fs, @s)";

                            var FullORSimple = acl.Scope == SCOPE.FULL ? 0 : 1;
                            query.Parameters.AddWithValue("@fs", FullORSimple);
                            query.Parameters.AddWithValue("@s", acl.Sync.ToString("HH:mm:ss.fff"));

                            if (acl.Scope == SCOPE.FULL)
                            {
                                query.Parameters.AddWithValue("@GD", acl.StartingGradleDaemon.ToString("HH:mm:ss.fff"));
                                query.Parameters.AddWithValue("@rb", acl.RunBuild.ToString("HH:mm:ss.fff"));
                                query.Parameters.AddWithValue("@lb", acl.LoadBuild.ToString("HH:mm:ss.fff"));
                                query.Parameters.AddWithValue("@cb", acl.ConfigureBuild.ToString("HH:mm:ss.fff"));
                                query.Parameters.AddWithValue("@ap", acl.AllProjects.ToString("HH:mm:ss.fff"));
                            }


                            query.ExecuteNonQuery();
                        }

                        Remove(ctemp);

                        break;
                    case LOG.CATEGORY.GRAPHICS:

                        GraphicsLOG gtemp = (GraphicsLOG)cachedLOG;

                        query.CommandText = @"INSERT INTO GraphicsLOG(logID, mediumID, formatID, brush, height, width, unitID, 
                                                size, DPI, depth, completed, source)
                                                VALUES(@id, @medium, @format, @brush, @height, @width, @unit, @size, @DPI, 
                                                    @depth, @done, @source);";

                        query.Parameters.AddWithValue("@id", cachedLOG.ID);
                        query.Parameters.AddWithValue("@medium", _reader.FindMediumID(gtemp.Category, gtemp.Medium));
                        query.Parameters.AddWithValue("@format", _reader.FindFormatID(gtemp.Category, gtemp.Format));
                        query.Parameters.AddWithValue("@brush", gtemp.Brush);
                        query.Parameters.AddWithValue("@height", gtemp.Height);
                        query.Parameters.AddWithValue("@width", gtemp.Width);
                        query.Parameters.AddWithValue("@unit", gtemp.Unit);
                        query.Parameters.AddWithValue("@size", gtemp.Size);
                        query.Parameters.AddWithValue("@DPI", gtemp.DPI);
                        query.Parameters.AddWithValue("@depth", gtemp.Depth);
                        query.Parameters.AddWithValue("@done", gtemp.IsCompleted);
                        query.Parameters.AddWithValue("@source", gtemp.Source);


                        query.ExecuteNonQuery();

                        Remove(gtemp);

                        break;
                    case LOG.CATEGORY.FILM:
                        FilmLOG ftemp = (FilmLOG)cachedLOG;

                        query.CommandText = @"INSERT INTO FilmLOG(logID, height, width, length, completed, source)
                                                VALUES(@id, @height, @width, @length, @done, @source);";

                        query.Parameters.AddWithValue("@id", cachedLOG.ID);
                        query.Parameters.AddWithValue("@height", ftemp.Height);
                        query.Parameters.AddWithValue("@width", ftemp.Width);
                        query.Parameters.AddWithValue("@length", ftemp.Length);
                        query.Parameters.AddWithValue("@done", ftemp.IsCompleted);
                        query.Parameters.AddWithValue("@source", ftemp.Source);

                        query.ExecuteNonQuery();

                        Remove(ftemp);

                        break;
                    case LOG.CATEGORY.NOTES:
                        NotesLOG n = (NotesLOG)cachedLOG;

                        if (n.notelogtype == NotesLOG.NOTELOGType.GENERIC)
                        {
                            query.CommandText = @"INSERT INTO NotesLOG(logID, noteLogType)
                                                    VALUES(@id, @category);
";

                            query.Parameters.AddWithValue("@id", cachedLOG.ID);
                            query.Parameters.AddWithValue("@category", 0);

                            query.ExecuteNonQuery();

                            NoteItem gn = (NoteItem)n;

                            query.CommandText = @"INSERT INTO NoteItem(logID, IsChecklist, subject, genericNote)
                                                    VALUES(@id, @checklist, @subject, @gn);
";

                            query.Parameters.AddWithValue("@id", cachedLOG.ID);

                            var IsChecklist = gn.Items.Count > 0;
                            query.Parameters.AddWithValue("@checklist", IsChecklist);
                            query.Parameters.AddWithValue("@subject", gn.Subject);
                            query.Parameters.AddWithValue("@gn", gn.Content);

                            query.ExecuteNonQuery();

                            // Store Check List Items Here
                            foreach (CheckListItem item in gn.Items)
                            {
                                AddCheckListItem(item.ID, cachedLOG.ID, item.IsChecked, item.Item);
                            }

                            Remove(gn);
                        }
                        else
                        {
                            query.CommandText = @"INSERT INTO NotesLOG(logID, noteLogType)
                                                    VALUES(@id, @category);
";

                            query.Parameters.AddWithValue("@id", cachedLOG.ID);
                            query.Parameters.AddWithValue("@category", 1);

                            query.ExecuteNonQuery();

                            FlexiNotesLOG fn = (FlexiNotesLOG)cachedLOG;

                            query.CommandText = @"INSERT INTO FlexiNotesLOG(logID, flexiNoteTypeID, mediumID, formatID, bitRate, length,
                                                        completed, source)
                                                    VALUES(@id, @fnCategory, @medium, @format, @br, @length, @done, @source, @gc);
";

                            query.Parameters.AddWithValue("@id", cachedLOG.ID);
                            query.Parameters.AddWithValue("@fnCategory", _reader.FindFNCategory(fn.flexinotetype));
                            query.Parameters.AddWithValue("@medium", _reader.FindMediumID(fn.Category, fn.Medium));
                            query.Parameters.AddWithValue("@format", _reader.FindFormatID(fn.Category, fn.Format));
                            query.Parameters.AddWithValue("@br", fn.Bitrate);
                            query.Parameters.AddWithValue("@length", fn.Length);
                            query.Parameters.AddWithValue("@done", fn.IsCompleted);
                            query.Parameters.AddWithValue("@source", fn.Source);

                            query.ExecuteNonQuery();

                            Remove(fn);

                        }


                        break;
                }

            }


        }












        /// <summary>
        /// Creates a log in the database.
        /// </summary>
        /// <param name="log">The log being stored.</param>
        public bool CreateLOG(LOG log)
        {
            try
            {
                using SQLiteCommand insert = _con.CreateCommand();

                InsertLogDetails(log, insert);
                InsertPostItDetails(log, insert);
                InsertLogCategorySpecificDetails(log, insert);

                return true;
            }
            catch (InvalidCastException castx)
            {
                // Handle cast exceptions
                Debug.WriteLine($"Invalid cast exception near CreateLOG(log): {castx.Message}");

                //TODO
                /*
                 
                try{

                // Send to Feedback table within this class.


                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"Exception found near CreateLog(log): {ex.Message}");
                }


                 */

                return false;
            }
            catch (SQLiteException sqlex)
            {
                // Handle SQLite exceptions
                Debug.WriteLine($"SQLiteException near CreateLOG(log): {sqlex.Message}");

                return false;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Debug.WriteLine($"Exception near CreateLOG(log): {ex.Message}");

                // TODO


                return false;
            }
        }

        private void InsertLogDetails(LOG log, SQLiteCommand insert)
        {
            AppAlreadyAdded = false;
            ProjectAlreadyAdded = false;
            insert.CommandText = $@"INSERT INTO LOG(logID, {Column.CategoryID}, {Column.AccountID}, {Column.ProjectID}, {Column.AppID}, start, end, outputID, typeID)
                           VALUES(@id, @category, @author, @project, @app, @start, @end, @output, @type);";

            insert.Parameters.AddWithValue("@id", log.ID);
            insert.Parameters.AddWithValue("@category", _reader.FindCategoryID(log.Category));
            insert.Parameters.AddWithValue("@author", log.Author.ID);

            var app = log.Application;
            var temporaryID = _reader.FindAppID(app);
            if (!AppAlreadyAdded && temporaryID == -1)
            {
                AddApplication(app);
                AppAlreadyAdded = true;

                temporaryID = _reader.FindAppID(app);
            }

            insert.Parameters.AddWithValue("@app", temporaryID);
            AppAlreadyAdded = false;

            var project = log.Project;
            temporaryID = _reader.FindProjectID(project);
            if (!ProjectAlreadyAdded && temporaryID == 1)
            {
                AddProject(project);
                ProjectAlreadyAdded = true;

                temporaryID = _reader.FindProjectID(project);
            }

            insert.Parameters.AddWithValue("@project", temporaryID);
            ProjectAlreadyAdded = false;

            insert.Parameters.AddWithValue("@start", log.Start.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
            insert.Parameters.AddWithValue("@end", log.End.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
            insert.Parameters.AddWithValue("@output", _reader.FindOutputID(log.Output));
            insert.Parameters.AddWithValue("@type", _reader.FindTypeID(log.Type));

            insert.ExecuteNonQuery();

            if (Watcher.LogID > 0)
                --Watcher.LogID;
        }


        private void InsertPostItDetails(LOG log, SQLiteCommand insert)
        {
            foreach (PostIt postIt in log.PostItList)
            {
                string error = ValidateString(postIt.Error);
                string solution = ValidateString(postIt.Solution);
                string suggestion = ValidateString(postIt.Suggestion);
                string comment = ValidateString(postIt.Comment);

                InsertPostItIntoDatabase(insert, postIt, log.ID, error, solution, suggestion, comment);
            }
        }

        private string ValidateString(string input)
        {
            string pattern = @"[\s\S]+";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input) ? input : string.Empty;
        }

        private void InsertPostItIntoDatabase(SQLiteCommand insert, PostIt postIt, int logID, string error, string solution, string suggestion, string comment)
        {
            insert.CommandText = @"INSERT INTO PostIt(postItID, logID, subjectID, error, date_found, solution, date_solved, suggestion, comment)
                           VALUES(@id, @log, @subject, @error, @found, @solution, @solved, @suggest, @comment);";

            insert.Parameters.AddWithValue("@id", postIt.ID);
            insert.Parameters.AddWithValue("@log", logID);

            var subject = postIt.Subject;
            var tempSubjectID = _reader.FindSubjectID(subject);

            if (!SubjectAlreadyAdded && tempSubjectID == -1)
            {
                AddSubject(subject);
                SubjectAlreadyAdded = true;

                tempSubjectID = _reader.FindSubjectID(subject);
            }
            insert.Parameters.AddWithValue("@subject", tempSubjectID);
            SubjectAlreadyAdded = false;

            insert.Parameters.AddWithValue("@error", error);
            insert.Parameters.AddWithValue("@found", GetDateTimeString(postIt.ERCaptureTime, error));
            insert.Parameters.AddWithValue("@solution", solution);
            insert.Parameters.AddWithValue("@solved", GetDateTimeString(postIt.SOCaptureTime, solution));
            insert.Parameters.AddWithValue("@suggest", suggestion);
            insert.Parameters.AddWithValue("@comment", comment);

            insert.ExecuteNonQuery();

            if (Watcher.PostItID > 0)
                --Watcher.PostItID;
        }

        /// <summary>
        /// Creates a Data Logger format date and time as a string for the database.
        /// </summary>
        /// <param name="dateTime">The DateTime provided that needs conversion.</param>
        /// <returns>Returns a DateTime as a string in the format "1 January 2025 00:00:00.000".</returns>
        public static string GetDateTimeString(DateTime dateTime) => dateTime.ToString("dddd dd MMMM yyyy HH:mm:ss.fff");

        public static string GetDateTimeString(DateTime dateTime, string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue) || dateTime == default)
                return string.Empty;

            return dateTime.ToString("dddd dd MMMM yyyy HH:mm:ss.fff");
        }

        private void InsertLogCategorySpecificDetails(LOG log, SQLiteCommand insert)
        {
            switch (log.Category)
            {
                case LOG.CATEGORY.CODING:
                    InsertCodingLogDetails((CodingLOG)log, insert);
                    break;
                case LOG.CATEGORY.GRAPHICS:
                    InsertGraphicsLogDetails((GraphicsLOG)log, insert);
                    break;
                case LOG.CATEGORY.FILM:
                    InsertFilmLogDetails((FilmLOG)log, insert);
                    break;
                case LOG.CATEGORY.NOTES:
                    InsertNotesLogDetails((NotesLOG)log, insert);
                    break;
            }
        }

        private static void InsertCodingLogDetails(CodingLOG log, SQLiteCommand insert)
        {
            insert.CommandText = @"INSERT INTO CodingLOG(logID, bugs, opened)
                           VALUES(@id, @bugs, @opened);";

            insert.Parameters.AddWithValue("@id", log.ID);
            insert.Parameters.AddWithValue("@bugs", log.Bugs);
            insert.Parameters.AddWithValue("@opened", log.Success);

            insert.ExecuteNonQuery();

            if (log.Application.Name == Android)
            {
                InsertAndroidCodingLogDetails((AndroidCodingLOG)log, insert);
            }
        }

        private static void InsertAndroidCodingLogDetails(AndroidCodingLOG log, SQLiteCommand insert)
        {
            if (log.Scope == SCOPE.FULL)
            {
                insert.CommandText = @"INSERT INTO AndroidCodingLOG(isSimple, sync, gradleDaemon, runBuild, loadBuild, configBuild, allProjects)
                               VALUES(@fs, @s, @GD, @rb, @lb, @cb, @ap);";

                insert.Parameters.AddWithValue("@fs", log.Scope == SCOPE.FULL ? 0 : 1);
                insert.Parameters.AddWithValue("@s", log.Sync.ToString("HH:mm:ss.fff"));
                insert.Parameters.AddWithValue("@GD", log.StartingGradleDaemon.ToString("HH:mm:ss.fff"));
                insert.Parameters.AddWithValue("@rb", log.RunBuild.ToString("HH:mm:ss.fff"));
                insert.Parameters.AddWithValue("@lb", log.LoadBuild.ToString("HH:mm:ss.fff"));
                insert.Parameters.AddWithValue("@cb", log.ConfigureBuild.ToString("HH:mm:ss.fff"));
                insert.Parameters.AddWithValue("@ap", log.AllProjects.ToString("HH:mm:ss.fff"));
            }
            else
            {
                insert.CommandText = @"INSERT INTO AndroidCodingLOG(isSimple, sync)
                               VALUES(@fs, @s);";

                insert.Parameters.AddWithValue("@fs", log.Scope == SCOPE.FULL ? 0 : 1);
                insert.Parameters.AddWithValue("@s", log.Sync.ToString("HH:mm:ss.fff"));
            }

            insert.ExecuteNonQuery();
        }

        private void InsertGraphicsLogDetails(GraphicsLOG log, SQLiteCommand insert)
        {
            insert.CommandText = @"INSERT INTO GraphicsLOG(logID, mediumID, formatID, brush, height, width, unit, size, DPI, depth, completed, source)
                           VALUES(@id, @medium, @format, @brush, @height, @width, @unit, @size, @DPI, @depth, @done, @source);";

            insert.Parameters.AddWithValue("@id", log.ID);
            insert.Parameters.AddWithValue("@medium", _reader.FindMediumID(log.Category, log.Medium));
            insert.Parameters.AddWithValue("@format", _reader.FindFormatID(log.Category, log.Format));
            insert.Parameters.AddWithValue("@brush", log.Brush);
            insert.Parameters.AddWithValue("@height", log.Height);
            insert.Parameters.AddWithValue("@width", log.Width);
            insert.Parameters.AddWithValue("@unit", log.Unit);
            insert.Parameters.AddWithValue("@size", log.Size);
            insert.Parameters.AddWithValue("@DPI", log.DPI);
            insert.Parameters.AddWithValue("@depth", log.Depth);
            insert.Parameters.AddWithValue("@done", log.IsCompleted);
            insert.Parameters.AddWithValue("@source", log.Source);

            insert.ExecuteNonQuery();
        }

        private static void InsertFilmLogDetails(FilmLOG log, SQLiteCommand insert)
        {
            insert.CommandText = @"INSERT INTO FilmLOG(logID, height, width, length, completed, source)
                           VALUES(@id, @height, @width, @length, @done, @source);";

            insert.Parameters.AddWithValue("@id", log.ID);
            insert.Parameters.AddWithValue("@height", log.Height);
            insert.Parameters.AddWithValue("@width", log.Width);
            insert.Parameters.AddWithValue("@length", log.Length);
            insert.Parameters.AddWithValue("@done", log.IsCompleted);
            insert.Parameters.AddWithValue("@source", log.Source);

            insert.ExecuteNonQuery();
        }

        private void InsertNotesLogDetails(NotesLOG log, SQLiteCommand insert)
        {
            insert.CommandText = @"INSERT INTO NotesLOG(logID, noteLogTypeID)
                           VALUES(@id, @category);";

            insert.Parameters.AddWithValue("@id", log.ID);
            var noteLogType = log.notelogtype == NotesLOG.NOTELOGType.GENERIC ? 1 : 2;
            insert.Parameters.AddWithValue("@category", noteLogType);

            insert.ExecuteNonQuery();

            switch (log.notelogtype)
            {
                case NotesLOG.NOTELOGType.GENERIC:
                    InsertGenericNoteDetails((NoteItem)log, insert);
                    break;
                case NotesLOG.NOTELOGType.FLEXI:
                    InsertFlexiNoteDetails((FlexiNotesLOG)log, insert);
                    break;
            }
        }

        private void InsertGenericNoteDetails(NoteItem note, SQLiteCommand insert)
        {
            insert.CommandText = @"INSERT INTO NoteItem(logID, IsChecklist, subject, genericNote)
                           VALUES(@id, @checklist, @subject, @gn);";

            insert.Parameters.AddWithValue("@id", note.ID);
            insert.Parameters.AddWithValue("@checklist", note.Items != null && note.Items.Count > 0);
            insert.Parameters.AddWithValue("@subject", note.Subject);
            insert.Parameters.AddWithValue("@gn", note.Content);

            insert.ExecuteNonQuery();

            if (note.Items is not null && note.Items.Count > 0)
            {
                foreach (CheckListItem item in note.Items)
                {
                    AddCheckListItem(item.ID, note.ID, item.IsChecked, item.Item);
                }
            }
        }

        private void InsertFlexiNoteDetails(FlexiNotesLOG note, SQLiteCommand insert)
        {
            insert.CommandText = @"INSERT INTO FlexiNotesLOG(logID, flexiNoteTypeID, mediumID, formatID, bitRate, length, completed, source)
                           VALUES(@id, @fnCategory, @medium, @format, @br, @length, @done, @source);";

            insert.Parameters.AddWithValue("@id", note.ID);
            insert.Parameters.AddWithValue("@fnCategory", _reader.FindFNCategory(note.flexinotetype));
            insert.Parameters.AddWithValue("@medium", _reader.FindMediumID(note.Category, note.Medium));
            insert.Parameters.AddWithValue("@format", _reader.FindFormatID(note.Category, note.Format));
            insert.Parameters.AddWithValue("@br", note.Bitrate);
            insert.Parameters.AddWithValue("@length", note.Length);
            insert.Parameters.AddWithValue("@done", note.IsCompleted);
            insert.Parameters.AddWithValue("@source", note.Source);

            insert.ExecuteNonQuery();
        }


    }
}
