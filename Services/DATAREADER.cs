using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using static Data_Logger_1._3.Models.FlexiNotesLOG;
using static Data_Logger_1._3.Models.NotesLOG;

namespace Data_Logger_1._3.Services
{
    public class DATAREADER : DATAMASTER
    {
        /*
         * DOCUMENTATION
         * 
         * The class for all database reading activities.
         * Please use this class for reading the database!
         * Designed for Analyst.
         */


        #region Constructors



        public DATAREADER()
        {

        }

        public DATAREADER(bool status) : base(status) { }

        public DATAREADER(string category, SQLiteConnection connection, bool status) : base(connection, category, status) { }




        #endregion










        #region Methods



        /// <summary>
        /// Checks if an email exists.
        /// </summary>
        /// <param name="email">The email provided by the user signing up.</param>
        /// <returns>Returns whether the email exists or not. Will throw an ExmailConflictException if the email exists.</returns>
        /// <exception cref="EmailConflictException"></exception>
        public bool EmailExists(string email)
        {
            using SQLiteCommand read = _con.CreateCommand();
            read.CommandText = "SELECT email FROM ACCOUNT WHERE email = @email;";
            read.Parameters.AddWithValue("@email", email);

            SQLiteDataReader reader = read.ExecuteReader();
            if (reader.Read())
            {
                throw new EmailConflictException("This email already exists.");
            }

            return false;
        }

        /// <summary>
        /// Checks if a log with a given logID has PostIts in the PostIt table.
        /// </summary>
        /// <param name="logID">The log ID of the log that is being updated.</param>
        /// <param name="postItID">The PostIt ID of the PostIt being checked.</param>
        /// <param name="transaction">The transaction that is currently occurring.</param>
        /// <returns></returns>
        public bool PostItsExists(int logID, int postItID, SQLiteTransaction transaction)
        {
            using var read = _con.CreateCommand();
            read.Transaction = transaction;

            read.CommandText = $@"SELECT {Column.PostItID} FROM PostIt
                                    WHERE {Column.LogID} = @logID 
;";


            return false;
        }



        /// <summary>
        /// Counts the total number of logs.
        /// </summary>
        /// <returns>The count of logs.</returns>
        public int LogCount()
        {
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = $@"SELECT * FROM LOG WHERE {Column.AccountID} = @account
;";
            query.Parameters.AddWithValue("@account", User.ID);

            SQLiteDataReader read = query.ExecuteReader();
            int count = 0;

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return count;
        }

        /// <summary>
        /// Counts the number of logs that are in a particular category.
        /// </summary>
        /// <param name="category">The category being enquired about.</param>
        /// <returns>The count of logs in the given category.</returns>
        public int LogCount(LOG.CATEGORY category)
        {
            SQLiteCommand query = _con.CreateCommand();
            try
            {
                query.CommandText = $@"SELECT * FROM LOG WHERE {Column.CategoryID} = @category
                                        AND {Column.AccountID} = @account
                                        AND {Column.AppID} NOT IN (@id1, @id2)
;";
                query.Parameters.AddWithValue("@category", FindCategoryID(category));
                query.Parameters.AddWithValue("@account", User.ID);
                query.Parameters.AddWithValue("@id1", 1);
                query.Parameters.AddWithValue("@id2", 2);

                SQLiteDataReader read = query.ExecuteReader();
                int count = 0;

                while (read.Read())
                {
                    ++count;
                }

                read.Close();

                return count;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Counts the number of Qt logs.
        /// </summary>
        /// <returns>The count of Qt logs ONLY.</returns>
        public int QtLogCount()
        {
            string queryText = $@"SELECT COUNT(*) FROM LOG WHERE {Column.CategoryID} = @category 
                                    AND {Column.AccountID} = @account
                                    AND {Column.AppID} = @app
;";

            try
            {
                using (var query = _con.CreateCommand())
                {
                    query.CommandText = queryText;
                    query.Parameters.AddWithValue("@category", 1);
                    query.Parameters.AddWithValue("@account", User.ID);
                    query.Parameters.AddWithValue("@app", 1);

                    object result = query.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);


                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception in QtLogCount: {sqlex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in QtLogCount: {ex.Message}");
                return -1;
            }

            return 0;
        }



        /// <summary>
        /// Counts the number of Android Studio logs.
        /// </summary>
        /// <returns>The count of Android Studio logs ONLY.</returns>
        public int ASLogCount()
        {
            string queryText = $@"SELECT COUNT(*) 
                          FROM LOG 
                          WHERE {Column.CategoryID} = @category 
                            AND {Column.AccountID} = @account
                            AND {Column.AppID} = @app;";

            try
            {
                using (var query = _con.CreateCommand())
                {
                    query.CommandText = queryText;
                    query.Parameters.AddWithValue("@category", 1);
                    query.Parameters.AddWithValue("@account", User.ID);
                    query.Parameters.AddWithValue("@app", 2);

                    object result = query.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);

                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception in ASLogCount: {sqlex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near ASLogCount: {ex.Message}");
                return -1;
            }

            return 0;

        }


        /// <summary>
        /// Counts the number of flexible logs only.
        /// </summary>
        /// <returns>The count of flexible logs.</returns>
        public int FlexiLogCount()
        {
            SQLiteCommand query = _con.CreateCommand();
            try
            {
                query.CommandText = $@"SELECT COUNT(*) FROM FlexiNotesLOG WHERE {Column.AccountID} = @account
;";
                query.Parameters.AddWithValue("@account", User.ID);

                object result = query.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                    return Convert.ToInt32(result);


            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException near FlexiLogCount(): {sqlex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near FlexiLogCount(): {ex.Message}");
                return -1;
            }

            return 0;
        }



        #region Querying






        public List<CodingLOG> SearchQtLogs(string searchBarText, int projectID)
        {
            if (string.IsNullOrEmpty(searchBarText))
            {
                return new List<CodingLOG>();
            }

            var logs = new List<CodingLOG>();
            var codingLogs = new Dictionary<int, CodingLOG>();
            var postIts = new Dictionary<int, List<PostIt>>();

            try
            {
                using (var query = _con.CreateCommand())
                {
                    /* Fetch LOG records based on search criteria OR fetch LOGs for PostIts that have the search criteria. */
                    query.CommandText = @"
                        SELECT 
                            *
                        FROM LOG
                        WHERE (
                                  start LIKE @searchText
                                  OR end LIKE @searchText
                                  OR outputID IN (SELECT outputID FROM OUTPUT WHERE output LIKE @searchText AND appID = @appID AND categoryID = @categoryID)
                                  OR typeID IN (SELECT typeID FROM TYPE WHERE type LIKE @searchText AND appID = @appID AND categoryID = @categoryID) 
                               
                                    OR
                               
                                    logID IN (
                                        SELECT logID FROM PostIt WHERE
                                        subjectID IN (SELECT subjectID FROM Subject WHERE subject LIKE @searchText 
                                            AND appID = @appID 
                                            AND categoryID = @categoryID 
                                            AND accountID = @accountID
                                            AND projectID = @projectID
                                                     )
                                        OR error LIKE @searchText
                                        OR solution LIKE @searchText
                                        OR suggestion LIKE @searchText
                                        OR comment LIKE @searchText
                                             )
                              )
                  AND accountID = @accountID
                  AND categoryID = @categoryID
                  AND appID = @appID
;";

                    query.Parameters.AddWithValue("@searchText", $"%{searchBarText}%");
                    query.Parameters.AddWithValue("@accountID", User.ID);
                    query.Parameters.AddWithValue("@categoryID", 1);
                    query.Parameters.AddWithValue("@appID", 1);
                    query.Parameters.AddWithValue("@projectID", projectID);

                    using (var read = query.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            var logID = read.GetInt32(read.GetOrdinal("logID"));
                            codingLogs[logID] = new CodingLOG
                            {
                                ID = logID,
                                Author = User,
                                Application = FindAppByID(read.GetInt32(read.GetOrdinal("appID"))),
                                Project = FindProjectByID(read.GetInt32(read.GetOrdinal("projectID"))),
                                Start = DateTime.Parse(read.GetString(read.GetOrdinal("start"))),
                                End = DateTime.Parse(read.GetString(read.GetOrdinal("end"))),
                                Output = FindOutputByID(read.GetInt32(read.GetOrdinal("outputID"))),
                                Type = FindTypeByID(read.GetInt32(read.GetOrdinal("typeID"))),
                                PostItList = new()
                            };
                        }

                        read.Close();
                    }

                    var IDs = codingLogs.Keys.ToList();

                    /* Retrieve PostIts with logIDs found in codingLogs */
                    query.CommandText = $@"SELECT
                                                *
                                            FROM PostIt
                                            WHERE logID IN(
                                                {string.Join(',', IDs)}
                                                            )

;";

                    using (var read = query.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            var logID = read.GetInt32(read.GetOrdinal("logID"));
                            if (codingLogs.ContainsKey(logID))
                            {
                                var dateFound = read.GetString(read.GetOrdinal("date_found"));
                                var dateSolved = read.GetString(read.GetOrdinal("date_solved"));

                                codingLogs[logID].PostItList.Add(
                                    new PostIt
                                    {
                                        ID = read.GetInt32(read.GetOrdinal("postItID")),
                                        Subject = FindSubjectByID(read.GetInt32(read.GetOrdinal("subjectID"))),
                                        Error = read.GetString(read.GetOrdinal("error")),
                                        ERCaptureTime = string.IsNullOrEmpty(dateFound) ? new() :
                                            DateTime.Parse(dateFound),
                                        Solution = read.GetString(read.GetOrdinal("solution")),
                                        SOCaptureTime = string.IsNullOrEmpty(dateSolved) ? new() :
                                            DateTime.Parse(dateSolved),
                                        Suggestion = read.GetString(read.GetOrdinal("suggestion")),
                                        Comment = read.GetString(read.GetOrdinal("comment"))
                                    }
                                    );
                            }
                        }

                        read.Close();
                    }

                    /* Retrieve CodingLOGs with logIDs found in codingLogs.  */
                    query.CommandText = $@"SELECT
                                                *
                                            FROM CodingLOG
                                            WHERE logID IN(
                                                {string.Join(',', IDs)} )

;";

                    using (var read = query.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            var logID = read.GetInt32(read.GetOrdinal("logID"));
                            if (codingLogs.ContainsKey(logID))
                            {
                                codingLogs[logID].Bugs = read.GetInt32(read.GetOrdinal("bugs"));
                                codingLogs[logID].Success = read.GetBoolean(read.GetOrdinal("opened"));
                            }
                        }

                        read.Close();
                    }

                }
            }
            catch (ArgumentNullException nullex)
            {
                Debug.WriteLine($"ArgumentNullException occurred near SearchQtLogs(query,projectID): {nullex.Message}");
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException occurred near SearchQtLogs(query,projectID): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near SearchQtLogs(query,projectID): {ex.Message}");
            }

            return [.. codingLogs.Values];
        }










        #endregion








        /// <summary>
        /// Finds a category ID in the database for the LOG category enum provided.
        /// </summary>
        /// <param name="category">The category enum that the ID required is associated with.</param>
        /// <returns>An integer value representing an ID.</returns>
        public int FindCategoryID(LOG.CATEGORY category)
        {
            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT {Column.CategoryID} FROM CATEGORY WHERE {Column.category} = @categoryName;";
                    query.Parameters.AddWithValue("@categoryName", category.ToString());

                    object result = query.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Debug.WriteLine($"Exception found near FindCategoryID: {ex.Message}");
            }

            return -1;
        }

        /** Will find an APPLICATION's ID and return both the ID.
        *   It will automatically add an APPLICATION if it doesn't exist.
        *   */
        public int FindAppID(ApplicationClass app)
        {
            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT {Column.AppID} FROM APPLICATION WHERE {Column.app} = @appName
                                                        AND {Column.CategoryID} = @category
                                                        AND {Column.AccountID} IN (@id1, @id2)
;";
                    query.Parameters.AddWithValue("@appName", app.Name);
                    query.Parameters.AddWithValue("@category", FindCategoryID(app.Category));
                    query.Parameters.AddWithValue("@id1", 1);
                    query.Parameters.AddWithValue("@id2", app.User.ID);

                    object result = query.ExecuteScalar();

                    if (result != null)
                        return Convert.ToInt32(result);

                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception found near FindAppID: {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindAppID: {ex.Message}");

                // TODO
            }

            return -1;
        }


        /** Will find a PROJECT's ID and return the ID of the PROJECT.
         *  This function will automatically add a PROJECT if it doesn't exist and return the new project's ID.
         *  */
        public int FindProjectID(ProjectClass project)
        {
            if (string.IsNullOrEmpty(project.Name) || project.Name == "Unknown")
                return 1;


            try
            {

                // Retrieve APPLICATION ID
                int appID = FindAppID(project.Application);

                // If application ID is not 3, perform search and add project if not found
                if (appID != 3)
                {
                    using (SQLiteCommand query = _con.CreateCommand())
                    {
                        query.CommandText = $@"SELECT {Column.ProjectID} FROM PROJECT 
                                      WHERE {Column.AppID} = @app
                                        AND {Column.AccountID} = @account
                                        AND {Column.CategoryID} = @category
                                        AND {Column.project} = @project
;";

                        query.Parameters.AddWithValue("@app", project.Application.AppID);
                        query.Parameters.AddWithValue("@account", project.User.ID);
                        query.Parameters.AddWithValue("@category", FindCategoryID(project.Category));
                        query.Parameters.AddWithValue("@project", project.Name);

                        object result = query.ExecuteScalar();

                        if (result != null)
                            return Convert.ToInt32(result);

                    }
                }

            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception near FindProjectID(projectClass): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near FindProjectID(projectClass): {ex.Message}");
            }

            return 1;
        }



        public ProjectClass FindProjectByID(int id)
        {
            ProjectClass project = new ProjectClass();
            const string unknown = "Unknown";

            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT * FROM PROJECT WHERE {Column.ProjectID} = @id
;";
                    query.Parameters.AddWithValue("@id", id);

                    SQLiteDataReader read = query.ExecuteReader();
                    if (read.Read())
                    {
                        project.ProjectID = read.GetInt32(Column.IDColumn);
                        project.Name = read.GetString(Column.project);

                        project.IsDefault = project.Name.Equals(unknown, StringComparison.OrdinalIgnoreCase);

                        int accountID = read.GetInt32(Column.AccountID);
                        project.User = FindAccountByID(accountID);

                        int appID = read.GetInt32(Column.AppID);
                        project.Application = FindAppByID(appID);
                    }

                    read.Close();
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception near FindProjectByID(id): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near FindProjectByID(id): {ex.Message}");

                // TODO
            }

            return project;
        }


        public int FindOutputID(OutputClass output)
        {

            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT outputID FROM OUTPUT WHERE {Column.CategoryID} = @category 
                                                            AND output = @output ORDER BY outputID ASC
;";
                    query.Parameters.AddWithValue("@category", FindCategoryID(output.Category));
                    query.Parameters.AddWithValue("@output", output.Name);

                    object result = query.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception near FindOutputID(outputClass): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near FindOutputID(outputClass): {ex.Message}");

                // TODO
            }

            return 37;
        }


        public OutputClass FindOutputByID(int id)
        {
            OutputClass output = new();

            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = "SELECT * FROM OUTPUT WHERE outputID = @id ORDER BY outputID ASC;";
                    query.Parameters.AddWithValue("@id", id);

                    SQLiteDataReader reader = query.ExecuteReader();
                    if (reader.Read())
                    {
                        output.OutputID = reader.GetInt32(Column.IDColumn);
                        output.Name = reader.GetString(Column.output);
                        output.User = FindAccountByID(reader.GetInt32(Column.AccountID));
                        output.Application = FindAppByID(reader.GetInt32(Column.AppID));
                        output.Category = FindCategoryByID(reader.GetInt32(Column.CategoryID));
                    }

                    reader.Close();
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception: {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error finding output by ID: {ex.Message}");
            }

            return output;
        }


        public int FindTypeID(TypeClass type)
        {
            // Will store the type IDs
            int typeKey = 1, catNumber = 1;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            /** Retrieve TYPEs from the database temporarily.
             *  Look for the type that matches "type" parameter variable.
             *  Then, return the key of the type when it is found.
             */

            query.CommandText = $@"SELECT * FROM TYPE WHERE {Column.CategoryID} = @category 
                                                AND type = @type
;";
            query.Parameters.AddWithValue("@category", FindCategoryID(type.Category));
            query.Parameters.AddWithValue("@type", type.Name);
            read = query.ExecuteReader();


            while (read.Read())
            {
                typeKey = read.GetInt32(Column.IDColumn);
                break;
            }

            read.Close();

            return typeKey;
        }

        public TypeClass FindTypeByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            TypeClass type = new();

            query.CommandText = "SELECT * FROM TYPE WHERE typeID = @id ORDER BY typeID ASC;";
            query.Parameters.AddWithValue("@id", id);

            read = query.ExecuteReader();

            while (read.Read())
            {
                type.TypeID = read.GetInt32(0);
                type.Name = read.GetString(4);
                type.User = FindAccountByID(read.GetInt32(1));
                type.Application = FindAppByID(read.GetInt32(2));
                type.Category = FindCategoryByID(read.GetInt32(3));
            }

            read.Close();

            return type;
        }

        public int FindSubjectID(SubjectClass subject)
        {
            // Will store the type IDs
            int subjectKey = -1;

            var subjectColumn = 5;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            /** Retrieve Subjects from the database temporarily.
             *  Look for the subject that matches the "subject" parameter variable.
             *  Then, return the key of the type when it is found.
             */
            query.CommandText = $@"SELECT * FROM Subject WHERE {Column.CategoryID} = @category
                                    AND {Column.ProjectID} = @project
                                    AND {Column.AppID} = @app
                                    AND {Column.AccountID} = @account
;";
            query.Parameters.AddWithValue("@category", FindCategoryID(subject.Category));

            var projectID = subject.Project.ProjectID;
            query.Parameters.AddWithValue("@project", projectID);

            var appID = subject.Application.AppID;
            query.Parameters.AddWithValue("@app", appID);

            var userID = subject.User.ID;
            query.Parameters.AddWithValue("@account", userID);

            read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetString(subjectColumn) == subject.Subject)
                {
                    subjectKey = read.GetInt32(Column.IDColumn);
                    SubjectAlreadyAdded = true;
                    break;
                }
            }

            read.Close();

            return subjectKey;
        }

        public SubjectClass FindSubjectByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            SubjectClass subject = new();
            subject.SubjectID = -1;

            query.CommandText = "SELECT * FROM Subject WHERE subjectID = @id ORDER BY subjectID ASC;";
            query.Parameters.AddWithValue("@id", id);

            read = query.ExecuteReader();

            while (read.Read())
            {
                subject.SubjectID = read.GetInt32(0);
                subject.Project = FindProjectByID(read.GetInt32(1));
                subject.Application = FindAppByID(read.GetInt32(2));
                subject.User = FindAccountByID(read.GetInt32(3));
                subject.Category = FindCategoryByID(read.GetInt32(4));
                subject.Subject = read.GetString(5);
            }

            read.Close();

            return subject;
        }

        public string? FindMediumByID(int id)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;
                string? medium = null;

                var mediumColumn = 2;

                query.CommandText = "SELECT * FROM MEDIUM WHERE mediumID = @id ORDER BY mediumID ASC;";
                query.Parameters.AddWithValue("@id", id);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    medium = read.GetString(mediumColumn);
                }

                read.Close();

                return medium;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindMediumByID(id): {sqlex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindMediumByID(id): {ex.Message}");
                // TODO
                return null;
            }
        }

        public string FindFormatByID(int id)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;
                string? format = null;

                var formatColumn = 2;

                query.CommandText = "SELECT * FROM FORMAT WHERE formatID = @id ORDER BY formatID ASC;";
                query.Parameters.AddWithValue("@id", id);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    format = read.GetString(formatColumn);
                }

                read.Close();

                return format;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindFormatByID(id): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindFormatByID(id): {ex.Message}");
                // TODO

            }

            return null;
        }

        public FLEXINOTEType FindFlexiNoteTypeByID(int id)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;
                FLEXINOTEType flexiNoteType = new();

                var flexiNoteTypeColumn = 2;

                query.CommandText = "SELECT * FROM FlexiNoteType WHERE flexiNoteTypeID = @id ORDER BY flexiNoteTypeID ASC;";
                query.Parameters.AddWithValue("@id", id);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    var type = read.GetString(flexiNoteTypeColumn);
                    Enum.TryParse(type, out flexiNoteType);
                }

                read.Close();

                return flexiNoteType;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindFlexiNoteTypeByID(id): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found found near FindFlexiNoteTypeByID(id): {ex.Message}");
            }

            return new();
        }

        /// <summary>
        /// Looks for a category's categoryID in the Category table.
        /// </summary>
        /// <param name="id">The categoryID in the category table.</param>
        /// <returns>The category's LOG.CATEGORY enum.</returns>
        public LOG.CATEGORY FindCategoryByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            LOG.CATEGORY category = new();

            query.CommandText = $@"SELECT * FROM CATEGORY WHERE {Column.CategoryID} = @category;";
            query.Parameters.AddWithValue("@category", id);

            try
            {
                read = query.ExecuteReader();

                while (read.Read())
                {
                    if (read.GetInt32(Column.IDColumn) == id)
                    {
                        string categoryName = read.GetString(1);
                        var IsOkay = Enum.TryParse(categoryName, out category);

                        if (!IsOkay)
                            throw new InvalidCastException();
                        break;
                    }

                }

                read.Close();
            }
            catch (InvalidCastException)
            {
                SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = @"INSERT INTO FEEDBACK(feedTypeID, {Column.AccountID}, date, description, CanContact)
                                        VALUES(@feed, @account, @date_submitted, @desc, @contact)";
                insert.Parameters.AddWithValue("@feed", 1);
                insert.Parameters.AddWithValue("@account", User.ID);
                insert.Parameters.AddWithValue("@date_submitted", DateTime.Now.ToString("d MMMM yyyy HH:mm:ss"));
                insert.Parameters.AddWithValue("@desc", "A bug occurred where the incorrect enum was returned or the enum parser was unable to parse the enum at all for whatever reason.");
                insert.Parameters.AddWithValue("@contact", 0);

                insert.ExecuteNonQuery();
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindCategoryByID(id): {sqlex.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near FindCategoryByID(id): {e.Message}");
                // TODO
            }

            return category;
        }

        /// <summary>
        /// Finds a NOTELOGType ID in the database for the enum provided.
        /// </summary>
        /// <param name="type">The type enum that the ID required is associated with.</param>
        /// <returns>An integer value representing a NOTELOGType ID.</returns>
        public int FindNoteLogTypeID(NOTELOGType type)
        {
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = "SELECT * FROM NoteLogTypes;";
            SQLiteDataReader read = query.ExecuteReader();
            int id = -1;
            var noteLogTypeColumn = 1;

            while (read.Read())
            {
                if (read.GetString(noteLogTypeColumn) == type.ToString())
                {
                    id = read.GetInt32(Column.IDColumn);
                    break;
                }
            }

            read.Close();

            return id;
        }


        public NOTELOGType FindNoteLogTypeByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            NOTELOGType type = new();

            var noteLogTypeColumn = 1;

            query.CommandText = "SELECT * FROM NoteLogTypes WHERE noteLogTypeID = @id;";
            query.Parameters.AddWithValue("@id", id);

            try
            {
                read = query.ExecuteReader();

                while (read.Read())
                {
                    string typeName = read.GetString(noteLogTypeColumn);
                    var IsOkay = Enum.TryParse(typeName, out type);

                    break;

                }

                read.Close();
            }
            catch (InvalidCastException)
            {
                SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = $@"INSERT INTO FEEDBACK(feedTypeID, {Column.AccountID}, date, description, CanContact)
                                        VALUES(@feed, @account, @date_submitted, @desc, @contact)";
                insert.Parameters.AddWithValue("@feed", 1);
                insert.Parameters.AddWithValue("@account", User.ID);
                insert.Parameters.AddWithValue("@date_submitted", DateTime.Now.ToString("d MMMM yyyy HH:mm:ss"));
                insert.Parameters.AddWithValue("@desc", "A bug occurred where the incorrect enum was return or the enum parser was unable to parse the enum at all for whatever reason.");
                insert.Parameters.AddWithValue("@contact", 0);

                insert.ExecuteNonQuery();
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindNoteLogTypeByID(id): {sqlex.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near FindNoteLogTypeByID(id): {e.Message}");

                // TODO


            }

            return type;
        }


        public ApplicationClass? FindAppByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            ApplicationClass? app = null;

            var categoryColumn = 3;
            var accountColumn = 1;
            var appColumn = 2;
            var isDefaultColumn = 4;

            query.CommandText = $@"SELECT * FROM APPLICATION WHERE {Column.AppID} = @app
;";
            query.Parameters.AddWithValue("@app", id);
            read = query.ExecuteReader();

            while (read.Read())
            {
                var account = read.GetBoolean(isDefaultColumn) ? FindAccountByID(1) : FindAccountByID(read.GetInt32(accountColumn));
                app = new ApplicationClass(read.GetInt32(Column.IDColumn), account, read.GetString(appColumn), FindCategoryByID(read.GetInt32(categoryColumn)),
                                read.GetBoolean(isDefaultColumn));
                break;
            }

            read.Close();

            return app;
        }


        /// <summary>
        /// Retrieve the ID of a medium with a given category and medium.
        /// </summary>
        /// <param name="category">The class the medium belongs to.</param>
        /// <param name="medium">The name of the medium.</param>
        /// <returns>A medium ID as an integer. Returns -1 if not found.</returns>
        public int FindMediumID(LOG.CATEGORY category, string medium)
        {
            int mediumKey = -1;

            try
            {
                int mediumColumn = 2;

                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;

                query.CommandText = $@"SELECT * FROM MEDIUM WHERE {Column.CategoryID} = @category
;";
                query.Parameters.AddWithValue("@category", FindCategoryID(category));
                read = query.ExecuteReader();

                while (read.Read())
                {
                    if (medium == read.GetString(mediumColumn))
                    {
                        mediumKey = read.GetInt32(Column.IDColumn);
                        break;
                    }

                }

                read.Close();
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindMediumID(category,medium): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindMediumID(category,medium): {ex.Message}");
            }

            return mediumKey;
        }



        /// <summary>
        /// Retrieves the ID of a format with a given category and format.
        /// </summary>
        /// <param name="category">The class the format belongs to</param>
        /// <param name="format">The name of the format.</param>
        /// <returns>A format ID as an integer. Returns -1 if not found.</returns>
        public int FindFormatID(LOG.CATEGORY category, string format)
        {
            int formatKey = -1;

            try
            {
                int formatColumn = 2;

                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;

                query.CommandText = $"SELECT * FROM FORMAT WHERE {Column.CategoryID} = @category ORDER BY formatID ASC;";
                query.Parameters.AddWithValue("@category", FindCategoryID(category));
                read = query.ExecuteReader();

                while (read.Read())
                {
                    if (format == read.GetString(formatColumn))
                    {
                        formatKey = read.GetInt32(0);
                        break;
                    }

                }

                read.Close();
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindFormatID(category,format): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindFormatID(category,format): {ex.Message}");
            }

            return formatKey;
        }


        /// <summary>
        /// Retrieves the ID of a unit with a given unit.
        /// </summary>
        /// <param name="unit">The name of the unit.</param>
        /// <returns>A unit ID as an integer. Returns -1 if not found.</returns>
        public int FindUnitID(string unit)
        {
            int unitKey = -1;

            try
            {
                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;

                var unitColumn = 1;

                query.CommandText = "SELECT * FROM MeasuringUnit ORDER BY unitID ASC;";
                read = query.ExecuteReader();

                while (read.Read())
                {
                    if (unit == read.GetString(unitColumn))
                    {
                        unitKey = read.GetInt32(Column.IDColumn);
                        break;
                    }

                }

                read.Close();
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindUnitID(unit): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindUnitID(unit): {ex.Message}");
            }

            return unitKey;
        }


        public int FindFNCategory(FLEXINOTEType type)
        {
            // Will store the flexi note ID
            int FNKey = -1;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            /** Retrieve FLEXI_NOTE CATEGORY from the database temporarily.
             *  Look for the FLEXI NOTE CATEGORY that matches "FNCategory" parameter variable.
             *  Then, return the key of the output when it is found.
             */
            query.CommandText = "SELECT * FROM FlexiNoteType WHERE flexiNoteType = @category ORDER BY flexiNoteTypeID ASC;";

            query.Parameters.AddWithValue("@category", type.ToString());


            read = query.ExecuteReader();

            while (read.Read())
            {
                FNKey = read.GetInt32(Column.IDColumn);
                break;

            }

            read.Close();

            return FNKey;
        }




        /// <summary>
        /// List ALL applications by the logged in user.
        /// </summary>
        /// <returns>A list of ApplicationClass objects.</returns>
        public List<ApplicationClass>? ListApplications()
        {
            /** Retrieve the APPLICATIONs from the database, 
             *  add them to a list 
             *  and return the list. */
            try
            {
                List<ApplicationClass>? apps = new();
                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;

                var categoryColumn = 3;
                var accountColumn = 1;
                var appColumn = 2;
                var isDefaultColumn = 4;

                query.CommandText = $@"SELECT * FROM APPLICATION WHERE {Column.AccountID} IN (@id1, @id2) ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC
;";
                query.Parameters.AddWithValue("@id1", User.ID);
                query.Parameters.AddWithValue("@id2", 1);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    apps.Add(new(read.GetInt32(Column.IDColumn), FindAccountByID(read.GetInt32(accountColumn)), read.GetString(appColumn), FindCategoryByID(read.GetInt32(categoryColumn)),
                                read.GetBoolean(isDefaultColumn)));
                }

                read.Close();

                return apps;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found: {sqlex.Message}");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found: {e.Message}");

                // TODO

                return null;
            }
        }

        /// <summary>
        /// List all applications in a particular LOG category.
        /// </summary>
        /// <param name="category">The log category.</param>
        /// <returns>A list of ApplicationClass objects.</returns>
        public List<ApplicationClass> ListApplications(LOG.CATEGORY category)
        {
            /** Retrieve the APPLICATIONs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<ApplicationClass> apps = new();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            var accountColumn = 1;
            var appColumn = 2;
            var isDefaultColumn = 4;

            query.CommandText = $@"SELECT * FROM APPLICATION WHERE {Column.AccountID} IN (@id1, @id2) 
                                            AND {Column.CategoryID} = @category ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC
;";
            query.Parameters.AddWithValue("@id1", User.ID);
            query.Parameters.AddWithValue("@id2", 1);
            query.Parameters.AddWithValue("@category", FindCategoryID(category));

            read = query.ExecuteReader();

            while (read.Read())
            {
                apps.Add(new(read.GetInt32(Column.IDColumn), FindAccountByID(read.GetInt32(accountColumn)), read.GetString(appColumn), category, read.GetBoolean(isDefaultColumn)));
            }

            read.Close();

            return apps;
        }

        /// <summary>
        /// Lists ALL the projects from the database.
        /// </summary>
        /// <returns>A list of ProjectClass objects.</returns>
        public List<ProjectClass>? ListProjects()
        {
            /** Retrieve the PROJECTs from the database, 
             *  add them to a list 
             *  and return the list. */

            List<ProjectClass>? projects = new List<ProjectClass>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            try
            {

                query.CommandText = $@"SELECT * FROM PROJECT WHERE {Column.AccountID} IN (@id1, @id2) ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC, {Column.ProjectID} ASC
;";
                query.Parameters.AddWithValue("@id1", User.ID);
                query.Parameters.AddWithValue("@id2", 1);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    projects.Add(FindProjectByID(read.GetInt32(Column.IDColumn)));
                }
                read.Close();

                return projects;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Lists the projects from the database and filters it by the category enum argument provided. Projects may or may not be unique.
        /// </summary>
        /// <param name="category">The category the projects fall under.</param>
        /// <returns>A list of ProjectClass objects from the specified category.</returns>
        public List<ProjectClass> ListProjects(LOG.CATEGORY category)
        {
            /** Retrieve the PROJECTs from the database, 
             *  add them to a list 
             *  and return the list. */

            List<ProjectClass> projects = new List<ProjectClass>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;


            query.CommandText = $@"SELECT * FROM PROJECT WHERE {Column.AccountID} IN (@id1, @id2)
                                            AND {Column.CategoryID} = @category ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC, {Column.ProjectID} ASC
;";
            query.Parameters.AddWithValue("@id1", User.ID);
            query.Parameters.AddWithValue("@id2", 1);
            query.Parameters.AddWithValue("@category", FindCategoryID(category));

            read = query.ExecuteReader();

            while (read.Read())
            {
                projects.Add(FindProjectByID(read.GetInt32(Column.IDColumn)));
            }

            read.Close();

            return projects;
        }

        /// <summary>
        /// Lists the projects from the database and filters it by the app argument provided. Projects are all unique.
        /// </summary>
        /// <param name="app">The app from which the projects are created in.</param>
        /// <returns>A list of ProjectClass objects created in the app.</returns>
        public List<ProjectClass> ListProjects(ApplicationClass app)
        {


            List<ProjectClass> projects = new List<ProjectClass>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            bool Exists;

            query.CommandText = $@"SELECT * FROM PROJECT WHERE {Column.AccountID} IN (@id1, @id2) 
                                            AND {Column.AppID} = @app
                                            AND {Column.CategoryID} = @category 
                                            ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC, {Column.ProjectID} ASC
;";
            query.Parameters.AddWithValue("@id1", User.ID);
            query.Parameters.AddWithValue("@id2", 1);
            query.Parameters.AddWithValue("@app", app.AppID);
            query.Parameters.AddWithValue("@category", FindCategoryID(app.Category));

            read = query.ExecuteReader();

            while (read.Read())
            {
                var project = FindProjectByID(read.GetInt32(Column.IDColumn));
                Exists = false;

                foreach (ProjectClass pro in projects)
                {
                    if (pro.Name == project.Name && pro.Application == project.Application)
                        Exists = true;
                }

                if (!Exists)
                    projects.Add(project);
            }
            read.Close();

            return projects;
        }




        public List<SubjectClass> ListSubjects(LOG.CATEGORY category)
        {
            /** Retrieve the SUBJEECTs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<SubjectClass> subjects = new();

            // Get the logIDs from the PostIt table
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            var categoryColumn = 4;
            var accountColumn = 3;
            var appColumn = 2;
            var projectColumn = 1;
            var subjectColumn = 5;

            // In ACCOUNT
            query.CommandText = $@"SELECT * FROM Subject WHERE {Column.AccountID} IN (@id1, @id2) 
                                            AND {Column.CategoryID} = @category ORDER BY {Column.AccountID} ASC
;";
            query.Parameters.AddWithValue("@id1", User.ID);
            query.Parameters.AddWithValue("@id2", 1);
            query.Parameters.AddWithValue("@category", FindCategoryID(category));

            read = query.ExecuteReader();

            while (read.Read())
            {
                subjects.Add(new SubjectClass(read.GetInt32(Column.IDColumn), FindCategoryByID(read.GetInt32(categoryColumn)), FindAccountByID(read.GetInt32(accountColumn)),
                    read.GetString(subjectColumn), FindProjectByID(read.GetInt32(projectColumn)), FindAppByID(read.GetInt32(appColumn))));
            }

            read.Close();


            return subjects;
        }

        public List<SubjectClass> ListSubjects(ProjectClass project)
        {
            /** Retrieve the SUBJEECTs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<SubjectClass> subjects = new();

            // Get the logIDs from the PostIt table
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            var categoryColumn = 4;
            var accountColumn = 3;
            var appColumn = 2;
            var projectColumn = 1;
            var subjectColumn = 5;


            query.CommandText = $@"SELECT * FROM Subject WHERE {Column.CategoryID} = @category
                                            AND {Column.AccountID} = @id
                                            AND {Column.AppID} = @app
                                            AND {Column.ProjectID} = @project ORDER BY {Column.AccountID} ASC;";
            query.Parameters.AddWithValue("@category", FindCategoryID(project.Category));
            query.Parameters.AddWithValue("@id", project.User.ID);
            query.Parameters.AddWithValue("@app", project.Application.AppID);
            query.Parameters.AddWithValue("@project", project.ProjectID);
            read = query.ExecuteReader();

            while (read.Read())
            {
                subjects.Add(new SubjectClass(read.GetInt32(Column.IDColumn), FindCategoryByID(read.GetInt32(categoryColumn)),
                    FindAccountByID(read.GetInt32(accountColumn)), read.GetString(subjectColumn), FindProjectByID(read.GetInt32(projectColumn)),
                    FindAppByID(read.GetInt32(appColumn))));
            }

            read.Close();


            return subjects;
        }

        public List<string> ListOutputs()
        {
            /** Retrieve the OUTPUTs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<string> outputs = new List<string>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            query.CommandText = @"SELECT * FROM OUTPUT ORDER BY outputID;";
            read = query.ExecuteReader();

            while (read.Read())
            {
                outputs.Add(read.GetString(1));
            }
            read.Close();

            return outputs;
        }

        public List<string> ListOutputs(ApplicationClass app)
        {
            /** Retrieve the OUTPUTs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<string> outputs = new List<string>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            query.CommandText = $@"SELECT * FROM OUTPUT WHERE {Column.AccountID} = @account
                                    AND {Column.CategoryID} = @category
                                    AND {Column.AppID} = @app ORDER BY outputID
;";
            query.Parameters.AddWithValue("@accounD", User.ID);
            query.Parameters.AddWithValue("@category", FindCategoryID(app.Category));
            query.Parameters.AddWithValue("@app", app.AppID);
            read = query.ExecuteReader();


            while (read.Read())
            {
                outputs.Add(read.GetString(4));
            }

            read.Close();

            return outputs;
        }


        public List<string> ListTypes(ApplicationClass app)
        {
            /** Retrieve the TYPEs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<string> types = new List<string>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            query.CommandText = $@"SELECT * FROM TYPE WHERE {Column.AccountID} = @account
                                    AND {Column.CategoryID} = @category
                                    AND {Column.AppID} = @app  ORDER BY typeID
;";
            query.Parameters.AddWithValue("@accounD", User.ID);
            query.Parameters.AddWithValue("@category", FindCategoryID(app.Category));
            query.Parameters.AddWithValue("@app", app.AppID);
            read = query.ExecuteReader();


            while (read.Read())
            {
                types.Add(read.GetString(4));
            }

            read.Close();

            return types;
        }

        public List<string> ListMediums()
        {
            /** Retrieve the MEDIUMs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<string> mediums = new List<string>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            query.CommandText = @"SELECT * FROM MEDIUM ORDER BY mediumID;";
            read = query.ExecuteReader();

            while (read.Read())
            {
                mediums.Add(read.GetString(1));
            }
            read.Close();

            return mediums;
        }

        public List<string> ListMediums(string category)
        {
            /** Retrieve the MEDIUMs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<string> mediums = new List<string>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            query.CommandText = @"SELECT * FROM MEDIUM ORDER BY mediumID;";
            read = query.ExecuteReader();

            while (read.Read())
            {
                switch (category)
                {
                    case "GRAPHICS":
                        if (read.GetInt32(0) <= 8)
                        {
                            mediums.Add(read.GetString(1));
                        }
                        break;
                    case "NOTES":
                        if (read.GetInt32(0) > 8)
                        {
                            mediums.Add(read.GetString(1));
                        }
                        break;
                }

            }
            read.Close();

            return mediums;
        }

        public List<string> ListFormats()
        {
            /** Retrieve the FORMATs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<string> formats = new List<string>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            query.CommandText = @"SELECT * FROM FORMAT ORDER BY formatID;";
            read = query.ExecuteReader();

            while (read.Read())
            {
                formats.Add(read.GetString(1));
            }
            read.Close();

            return formats;
        }

        public List<string> ListFormats(string category)
        {
            /** Retrieve the FORMATs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<string> formats = new List<string>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            query.CommandText = @"SELECT * FROM FORMAT ORDER BY formatID;";
            read = query.ExecuteReader();

            while (read.Read())
            {
                switch (category)
                {
                    case "GRAPHICS":
                        if (read.GetInt32(0) <= 4)
                        {
                            formats.Add(read.GetString(1));
                        }
                        break;
                    case "NOTES":
                        if (read.GetInt32(0) > 4)
                        {
                            formats.Add(read.GetString(1));
                        }
                        break;
                }

            }
            read.Close();

            return formats;
        }

        public List<string>? ListUnits()
        {
            /** Retrieve the UNITs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<string> units = null;
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            var unitColumn = 1;

            query.CommandText = @"SELECT * FROM MeasuringUnit ORDER BY unitID;";
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (units is null)
                    units = new();
                units.Add(read.GetString(unitColumn));

            }

            read.Close();

            return units;
        }







        /** Populates this DATAREADER with LOGS **/
        public void RetrieveLOGS()
        {
            try
            {
                this.Clear();

                List<LOG>? logs = new();
                bool Initialised = false;
                var note_IDs_LIST = new List<int>();
                var flexiLog_IDs_LIST = new List<int>();

                var categoryColumn = 1;
                var accountColumn = 2;
                var projectColumn = 3;
                var appColumn = 4;
                var startColumn = 5;
                var endColumn = 6;
                var outputColumn = 7;
                var typeColumn = 8;

                var noteLogTypeIDColumn = 1;

                SQLiteCommand query = _con.CreateCommand();


                query.CommandText = @"SELECT * FROM NotesLOG ORDER BY logID;";


                using (var read = query.ExecuteReader())
                {
                    logs = new();
                    Initialised = true;

                    while (read.Read())
                    {


                        if (logs is not null)
                        {
                            var type = FindNoteLogTypeByID(read.GetInt32(noteLogTypeIDColumn));



                            switch (type)
                            {
                                case NotesLOG.NOTELOGType.GENERIC:
                                    {
                                        NoteItem note = new();
                                        note.ID = read.GetInt32(Column.IDColumn);
                                        note_IDs_LIST.Add(note.ID);
                                        logs.Add(note);

                                        break;
                                    }
                                case NotesLOG.NOTELOGType.FLEXI:
                                    {
                                        FlexiNotesLOG flexiLog = new();
                                        flexiLog.ID = read.GetInt32(Column.IDColumn);
                                        flexiLog_IDs_LIST.Add(flexiLog.ID);
                                        logs.Add(flexiLog);

                                        break;
                                    }
                            }
                        }

                    }



                    read.Close();
                }


                query.CommandText = @"SELECT * FROM LOG ORDER BY logID;";


                using (var read = query.ExecuteReader())
                {
                    while (read.Read() && logs is not null)
                    {

                        switch (FindCategoryByID(read.GetInt32(categoryColumn)))
                        {
                            case LOG.CATEGORY.CODING:
                                {
                                    CodingLOG codingLog = new();
                                    codingLog.ID = read.GetInt32(Column.IDColumn);
                                    codingLog.Author = FindAccountByID(read.GetInt32(accountColumn));
                                    codingLog.Project = FindProjectByID(read.GetInt32(projectColumn));
                                    codingLog.Application = FindAppByID(read.GetInt32(appColumn));
                                    codingLog.Start = DateTime.Parse(read.GetString(startColumn));
                                    codingLog.End = DateTime.Parse(read.GetString(endColumn));
                                    codingLog.Output = FindOutputByID(read.GetInt32(outputColumn));
                                    codingLog.Type = FindTypeByID(read.GetInt32(typeColumn));
                                    codingLog.PostItList = new();

                                    logs.Add(codingLog);

                                    break;
                                }
                            case LOG.CATEGORY.GRAPHICS:
                                {
                                    GraphicsLOG graphicsLog = new();
                                    graphicsLog.ID = read.GetInt32(Column.IDColumn);
                                    graphicsLog.Author = FindAccountByID(read.GetInt32(accountColumn));
                                    graphicsLog.Project = FindProjectByID(read.GetInt32(projectColumn));
                                    graphicsLog.Application = FindAppByID(read.GetInt32(appColumn));
                                    graphicsLog.Start = DateTime.Parse(read.GetString(startColumn));
                                    graphicsLog.End = DateTime.Parse(read.GetString(endColumn));
                                    graphicsLog.Output = FindOutputByID(read.GetInt32(outputColumn));
                                    graphicsLog.Type = FindTypeByID(read.GetInt32(typeColumn));
                                    graphicsLog.PostItList = new();


                                    break;
                                }
                            case LOG.CATEGORY.FILM:
                                {
                                    FilmLOG filmLog = new();
                                    filmLog.ID = read.GetInt32(Column.IDColumn);
                                    filmLog.Author = FindAccountByID(read.GetInt32(accountColumn));
                                    filmLog.Project = FindProjectByID(read.GetInt32(projectColumn));
                                    filmLog.Application = FindAppByID(read.GetInt32(appColumn));
                                    filmLog.Start = DateTime.Parse(read.GetString(startColumn));
                                    filmLog.End = DateTime.Parse(read.GetString(endColumn));
                                    filmLog.Output = FindOutputByID(read.GetInt32(outputColumn));
                                    filmLog.Type = FindTypeByID(read.GetInt32(typeColumn));
                                    filmLog.PostItList = new();

                                    break;
                                }
                            case LOG.CATEGORY.NOTES:
                                {
                                    foreach (LOG log in logs)
                                    {
                                        if (read.GetInt32(Column.IDColumn) == log.ID)
                                        {
                                            log.Author = FindAccountByID(read.GetInt32(accountColumn));
                                            log.Project = FindProjectByID(read.GetInt32(projectColumn));
                                            log.Application = FindAppByID(read.GetInt32(appColumn));
                                            log.Start = DateTime.Parse(read.GetString(startColumn));
                                            log.End = DateTime.Parse(read.GetString(endColumn));
                                            log.Output = FindOutputByID(read.GetInt32(outputColumn));
                                            log.Type = FindTypeByID(read.GetInt32(typeColumn));
                                            log.PostItList = new();

                                            break;
                                        }
                                    }
                                    break;

                                }
                        }
                    }

                    read.Close();
                }





                query.CommandText = @"SELECT * FROM POSTIT ORDER BY logID ASC, postItID ASC;";

                using (var read = query.ExecuteReader())
                {
                    var logIDColumn = 1;
                    var subjectColumn = 2;
                    var errorColumn = 3;
                    var solutionColumn = 5;
                    var suggestionColumn = 7;
                    var commentColumn = 8;
                    var foundColumn = 4;
                    var solvedColumn = 6;

                    while (read.Read() && logs is not null)
                    {

                        foreach (LOG log in logs)
                        {
                            if (read.GetInt32(logIDColumn) == log.ID)
                            {
                                DateTime dateFound;
                                DateTime dateSolved;

                                string foundDateString = read.GetString(foundColumn);
                                string solvedDateString = read.GetString(solvedColumn);

                                bool isFoundDateValid = DateTime.TryParse(foundDateString, out dateFound);
                                bool isSolvedDateValid = DateTime.TryParse(solvedDateString, out dateSolved);


                                log.PostItList.Add(new(read.GetInt32(Column.IDColumn), FindSubjectByID(read.GetInt32(subjectColumn)), read.GetString(errorColumn),
                                    read.GetString(solutionColumn), read.GetString(suggestionColumn), read.GetString(commentColumn), isFoundDateValid ? dateFound : new(),
                                    isSolvedDateValid ? dateSolved : new()));

                                break;
                            }
                        }

                    }

                    read.Close();
                }


                foreach (LOG log in logs)
                {
                    switch (log.Category)
                    {
                        case LOG.CATEGORY.CODING:
                            {
                                RetrieveCodingLOG((CodingLOG)log);

                                break;
                            }
                        case LOG.CATEGORY.GRAPHICS:
                            {
                                RetrieveGraphicsLOG((GraphicsLOG)log);

                                break;
                            }
                        case LOG.CATEGORY.FILM:
                            {
                                RetrieveFilmLOG((FilmLOG)log);

                                break;
                            }
                        case LOG.CATEGORY.NOTES:
                            {
                                if (note_IDs_LIST.Contains(log.ID))
                                    RetrieveNoteItem((NoteItem)log);
                                else if (flexiLog_IDs_LIST.Contains(log.ID))
                                    RetrieveFlexiNotesLOG((FlexiNotesLOG)log);

                                break;
                            }
                    }
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found in RetrieveLOGS(): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found in RetrieveLOGS(): {ex.Message}");
            }

        }

        public void RetrieveCodingLOG(CodingLOG codingLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM CodingLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", codingLog.ID);

                var bugsColumn = 1;
                var openedColumn = 2;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    codingLog.Bugs = read.GetInt32(bugsColumn);
                    codingLog.Success = read.GetBoolean(openedColumn);
                }

                read.Close();

                if (codingLog.Application.Name == Android)
                    RetrieveAndroidCodingLOG((AndroidCodingLOG)codingLog);
                else
                    this.Add(codingLog);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void RetrieveAndroidCodingLOG(AndroidCodingLOG androidCodingLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM AndroidCodingLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", androidCodingLog.ID);

                var fullORsimple = 1;
                var syncColumn = 2;
                var gradleColumn = 3;
                var runBuildColumn = 4;
                var loadBuildColumn = 5;
                var configBuildColumn = 6;
                var allProjectsColumn = 7;


                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    androidCodingLog.Scope = read.GetBoolean(fullORsimple) ? AndroidCodingLOG.SCOPE.SIMPLE : AndroidCodingLOG.SCOPE.FULL;
                    androidCodingLog.Sync = DateTime.Parse(read.GetString(syncColumn));
                    androidCodingLog.StartingGradleDaemon = DateTime.Parse(read.GetString(gradleColumn));
                    androidCodingLog.RunBuild = DateTime.Parse(read.GetString(runBuildColumn));
                    androidCodingLog.LoadBuild = DateTime.Parse(read.GetString(loadBuildColumn));
                    androidCodingLog.ConfigureBuild = DateTime.Parse(read.GetString(configBuildColumn));
                    androidCodingLog.AllProjects = DateTime.Parse(read.GetString(allProjectsColumn));
                }

                read.Close();


                this.Add(androidCodingLog);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near RetrieveAndroidCodingLOG(androidCodingLOG): {ex.Message}");
                // TODO
            }
        }

        public void RetrieveGraphicsLOG(GraphicsLOG graphicsLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM GraphicsLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", graphicsLog.ID);

                var mediumColumn = 1;
                var formatColumn = 2;
                var brushColumn = 3;
                var heightColumn = 4;
                var widthColumn = 5;
                var unitColumn = 6;
                var sizeColumn = 7;
                var DPIColumn = 8;
                var depthColumn = 9;
                var IsCompletedColumn = 10;
                var sourceColumn = 11;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    graphicsLog.Medium = read.GetString(mediumColumn);
                    graphicsLog.Format = read.GetString(formatColumn);
                    graphicsLog.Brush = read.GetString(brushColumn);
                    graphicsLog.Height = read.GetDouble(heightColumn);
                    graphicsLog.Width = read.GetDouble(widthColumn);
                    graphicsLog.Unit = read.GetString(unitColumn);
                    graphicsLog.Size = read.GetString(sizeColumn);
                    graphicsLog.DPI = read.GetDouble(DPIColumn);
                    graphicsLog.Depth = read.GetString(depthColumn);
                    graphicsLog.IsCompleted = read.GetBoolean(IsCompletedColumn);
                    graphicsLog.Source = read.GetString(sourceColumn);

                }

                read.Close();

                this.Add(graphicsLog);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void RetrieveFilmLOG(FilmLOG filmLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM FilmLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", filmLog.ID);

                var heightColumn = 1;
                var widthColumn = 2;
                var lengthColumn = 3;
                var IsCompletedColumn = 4;
                var sourceColumn = 5;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    filmLog.Height = read.GetDouble(heightColumn);
                    filmLog.Width = read.GetDouble(widthColumn);
                    filmLog.Length = read.GetString(lengthColumn);
                    filmLog.IsCompleted = read.GetBoolean(IsCompletedColumn);
                    filmLog.Source = read.GetString(sourceColumn);
                }

                read.Close();

                this.Add(filmLog);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void RetrieveNoteItem(NoteItem note)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM NoteItem WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", note.ID);
                bool IsChecklist = false;

                var IsChecklistColumn = 1;
                var subjectColumn = 2;
                var noteColumn = 3;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    IsChecklist = read.GetBoolean(IsChecklistColumn);
                    note.Items = IsChecklist ? new() : null;
                    note.Subject = read.GetString(subjectColumn);
                    note.Content = read.GetString(noteColumn);
                }

                read.Close();

                var itemColumn = 2;
                var doneColumn = 3;


                if (IsChecklist)
                {
                    query.CommandText = @"SELECT * FROM Checklist WHERE logID = @id ORDER BY logID;";
                    query.Parameters.AddWithValue("@id", note.ID);

                    read = query.ExecuteReader();

                    while (read.Read() && note.Items is not null)
                    {
                        note.Items.Add(new CheckListItem(read.GetInt32(Column.IDColumn), read.GetBoolean(doneColumn), read.GetString(itemColumn)));
                    }
                }

                this.Add(note);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void RetrieveFlexiNotesLOG(FlexiNotesLOG flexiLog)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                query.CommandText = @"SELECT * FROM FlexiNotesLOG WHERE logID = @id ORDER BY logID;";
                query.Parameters.AddWithValue("@id", flexiLog.ID);

                var flexiNoteTypeIDColumn = 1;
                var mediumColumn = 2;
                var formatColumn = 3;
                var bitRateColumn = 4;
                var lengthColumn = 5;
                var IsCompletedColumn = 6;
                var sourceColumn = 7;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    flexiLog.flexinotetype = FindFlexiNoteTypeByID(flexiNoteTypeIDColumn);
                    flexiLog.Medium = FindMediumByID(read.GetInt32(mediumColumn));
                    flexiLog.Format = FindFormatByID(read.GetInt32(formatColumn));
                    flexiLog.Bitrate = read.GetInt32(bitRateColumn);
                    flexiLog.Length = read.GetString(lengthColumn);
                    flexiLog.IsCompleted = read.GetBoolean(IsCompletedColumn);
                    flexiLog.Source = read.GetString(sourceColumn);
                }

                read.Close();

                this.Add(flexiLog);
            }
            catch (Exception)
            {
                // TODO
            }
        }




        public string UpdateProfilePic(string emailAddress)
        {
            string filePath = "";

            var profilePicColumn = 1;

            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM ACCOUNT WHERE email = @emailAddress;";
            query.Parameters.AddWithValue("@emailAddress", emailAddress);

            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                filePath = read.GetString(profilePicColumn);
                break;
            }


            return filePath;
        }



        #endregion
    }

    public class EmailConflictException : Exception
    {
        public EmailConflictException(string message) : base(message) { }
    }
}
