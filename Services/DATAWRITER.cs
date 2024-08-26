using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static Data_Logger_1._3.Models.AndroidCodingLOG;
using static Data_Logger_1._3.Models.FlexiNotesLOG;

namespace Data_Logger_1._3.Services
{
    public class DATAWRITER : DATAMASTER
    {

        /* CONSTRUCTORS */
        public DATAWRITER()
        {
            CreateConnection();
            CheckTables();
            CheckCategories();
            CheckApplications();
            CheckProject();
            CheckOutputs();
            CheckTypes();
            CheckNoteLogTypes();
            CheckMediums();
            CheckFormats();
            CheckFNCategories();
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


        // These private functions should be called by this class only


        // Find IDs

        public int FindMediumID(LOG.CATEGORY category, string medium)
        {
            // Will store the medium ID
            int mediumKey = -1;

            int mediumColumn = 2;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            /** Retrieve MEDIUMs from the database temporarily.
             *  Look for the MEDIUM that matches "media" parameter variable.
             *  Then, return the key of the output when it is found.
             */
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

            return mediumKey;
        }

        public int FindFormatID(LOG.CATEGORY category, string format)
        {
            int formatKey = -1;

            int formatColumn = 2;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            /** Retrieve FORMATs from the database temporarily.
             *  Look for the FORMAT that matches "format" parameter variable.
             *  Then, return the key of the output when it is found.
             */
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

            return formatKey;
        }

        public int FindUnitID(string unit)
        {
            // Will store the unit ID
            int unitKey = -1;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            var unitColumn = 1;

            /** Retrieve UNITs from the database temporarily.
             *  Look for the UNIT that matches "unit" parameter variable.
             *  Then, return the key of the output when it is found.
             */
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

            return FNKey;
        }


        // List items
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


        public int CreateProjectID()
        {
            SQLiteCommand query = _con.CreateCommand();
            const int baseID = 1;

            query.CommandText = $"SELECT MAX({Column.ProjectID}) FROM PROJECT;";

            object result = query.ExecuteScalar();
            int maxID = result != null && result != DBNull.Value ? Convert.ToInt32(result) : baseID - 1;

            int id = ++maxID;

            return id + Watcher.ProjectID++;
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
                if (read.GetInt32(3) == FindCategoryID(app.Category) &&
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
                if (read.GetInt32(categoryColumn) == FindCategoryID(category) &&
                    (read.GetInt32(accountColumn) == FindAccountID(account) ||
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
                    if (read.GetInt32(categoryColumn) == FindCategoryID(project.Category) &&
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
                if (read.GetInt32(categoryColumn) == FindCategoryID(app.Category) &&
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
                if (read.GetInt32(categoryColumn) == FindCategoryID(app.Category) &&
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

        /** Create database records
         * Call Synchronise when this is called.
         */
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
                query.Parameters.AddWithValue("@category", FindCategoryID(cachedLOG.Category));
                query.Parameters.AddWithValue("@author", cachedLOG.Author.ID);

                var tempProjectID = FindProjectID(cachedLOG.Project);
                query.Parameters.AddWithValue("@project", tempProjectID);

                var tempAppID = FindAppID(cachedLOG.Application);
                query.Parameters.AddWithValue("@app", tempAppID);


                // TODO Check that times can be read properly by RetrieveLOGS()
                query.Parameters.AddWithValue("@start", cachedLOG.Start.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                query.Parameters.AddWithValue("@end", cachedLOG.End.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                query.Parameters.AddWithValue("@output", FindOutputID(cachedLOG.Output));
                query.Parameters.AddWithValue("@type", FindTypeID(cachedLOG.Type));

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

                    var tempSubjectID = FindSubjectID(postIt.Subject);

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
                        query.Parameters.AddWithValue("@medium", FindMediumID(gtemp.Category, gtemp.Medium));
                        query.Parameters.AddWithValue("@format", FindFormatID(gtemp.Category, gtemp.Format));
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
                            query.Parameters.AddWithValue("@fnCategory", FindFNCategory(fn.flexinotetype));
                            query.Parameters.AddWithValue("@medium", FindMediumID(fn.Category, fn.Medium));
                            query.Parameters.AddWithValue("@format", FindFormatID(fn.Category, fn.Format));
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
        public void CreateLOG(LOG log)
        {
            try
            {
                using (SQLiteCommand insert = _con.CreateCommand())
                {
                    InsertLogDetails(log, insert);
                    InsertPostItDetails(log, insert);
                    InsertLogCategorySpecificDetails(log, insert);
                }
            }
            catch(InvalidCastException castx)
            {
                // Handle cast exceptions
                Debug.WriteLine($"Invalid cast exception near CreateLOG(log): {castx.Message}");
            }
            catch (SQLiteException sqlex)
            {
                // Handle SQLite exceptions
                Debug.WriteLine($"SQLite Exception near CreateLOG(log): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Debug.WriteLine($"Exception near CreateLOG(log): {ex.Message}");

                // TODO


            }
        }

        private void InsertLogDetails(LOG log, SQLiteCommand insert)
        {
            insert.CommandText = $@"INSERT INTO LOG(logID, {Column.CategoryID}, {Column.AccountID}, {Column.ProjectID}, {Column.AppID}, start, end, outputID, typeID)
                           VALUES(@id, @category, @author, @project, @app, @start, @end, @output, @type);";

            insert.Parameters.AddWithValue("@id", log.ID);
            insert.Parameters.AddWithValue("@category", FindCategoryID(log.Category));
            insert.Parameters.AddWithValue("@author", log.Author.ID);
            insert.Parameters.AddWithValue("@app", FindAppID(log.Application));
            AppAlreadyAdded = false;
            insert.Parameters.AddWithValue("@project", FindProjectID(log.Project));
            ProjectAlreadyAdded = false;
            insert.Parameters.AddWithValue("@start", log.Start.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
            insert.Parameters.AddWithValue("@end", log.End.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
            insert.Parameters.AddWithValue("@output", FindOutputID(log.Output));
            insert.Parameters.AddWithValue("@type", FindTypeID(log.Type));

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
            insert.Parameters.AddWithValue("@subject", FindSubjectID(postIt.Subject));
            insert.Parameters.AddWithValue("@error", error);
            insert.Parameters.AddWithValue("@found", GetDateTimeString(postIt.ERCaptureTime, error));
            insert.Parameters.AddWithValue("@solution", solution);
            insert.Parameters.AddWithValue("@solved", GetDateTimeString(postIt.SOCaptureTime, solution));
            insert.Parameters.AddWithValue("@suggest", suggestion);
            insert.Parameters.AddWithValue("@comment", comment);

            insert.ExecuteNonQuery();
            SubjectAlreadyAdded = false;

            if (Watcher.PostItID > 0)
                --Watcher.PostItID;
        }

        private string GetDateTimeString(DateTime dateTime, string inputValue)
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

        private void InsertCodingLogDetails(CodingLOG log, SQLiteCommand insert)
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

        private void InsertAndroidCodingLogDetails(AndroidCodingLOG log, SQLiteCommand insert)
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
            insert.Parameters.AddWithValue("@medium", FindMediumID(log.Category, log.Medium));
            insert.Parameters.AddWithValue("@format", FindFormatID(log.Category, log.Format));
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

        private void InsertFilmLogDetails(FilmLOG log, SQLiteCommand insert)
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
            insert.Parameters.AddWithValue("@category", FindNoteLogTypeID(log.notelogtype));

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
            insert.Parameters.AddWithValue("@fnCategory", FindFNCategory(note.flexinotetype));
            insert.Parameters.AddWithValue("@medium", FindMediumID(note.Category, note.Medium));
            insert.Parameters.AddWithValue("@format", FindFormatID(note.Category, note.Format));
            insert.Parameters.AddWithValue("@br", note.Bitrate);
            insert.Parameters.AddWithValue("@length", note.Length);
            insert.Parameters.AddWithValue("@done", note.IsCompleted);
            insert.Parameters.AddWithValue("@source", note.Source);

            insert.ExecuteNonQuery();
        }


    }
}
