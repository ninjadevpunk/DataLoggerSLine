using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using System.Data.SQLite;
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
            CheckMediums();
            CheckFormats();
            CheckFNCategories();
        }

        public DATAWRITER(string category, SQLiteConnection connection, bool status) : base(connection, category, status) { }

        // ---END CONSTRUCTORS---

        // These private functions should be called by this class only


        // Find IDs

        public int FindMediumID(LOG.CATEGORY category, string medium)
        {
            // Will store the medium ID
            int mediumKey = -1;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            /** Retrieve MEDIUMs from the database temporarily.
             *  Look for the MEDIUM that matches "media" parameter variable.
             *  Then, return the key of the output when it is found.
             */
            query.CommandText = "SELECT * FROM MEDIUM WHERE categoryID = @category ORDER BY mediumID ASC;";
            query.Parameters.AddWithValue("@category", FindCategoryID(category));
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (medium == read.GetString(1))
                {
                    mediumKey = read.GetInt32(0);
                    break;
                }

            }

            read.Close();

            return mediumKey;
        }

        public int FindFormatID(LOG.CATEGORY category, string format)
        {
            // Will store the format ID
            int formatKey = -1;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            /** Retrieve FORMATs from the database temporarily.
             *  Look for the FORMAT that matches "format" parameter variable.
             *  Then, return the key of the output when it is found.
             */
            query.CommandText = "SELECT * FROM FORMAT WHERE categoryID = @category ORDER BY formatID ASC;";
            query.Parameters.AddWithValue("@category", FindCategoryID(category));
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (format == read.GetString(1))
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
                    unitKey = read.GetInt32(IDColumn);
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
                FNKey = read.GetInt32(IDColumn);
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
            int catNumber = 1;

            query.CommandText = @"SELECT * FROM OUTPUT WHERE accountID = @account
                                    AND categoryID = @category
                                    AND appID = @app ORDER BY outputID;";
            query.Parameters.AddWithValue("@accounD", User.ID);
            query.Parameters.AddWithValue("@category", app.Category);
            query.Parameters.AddWithValue("@app", app.AppID);
            read = query.ExecuteReader();

            catNumber = FindCategoryID(app.Category);

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
            int catNumber = 1;

            query.CommandText = @"SELECT * FROM TYPE WHERE accountID = @account
                                    AND categoryID = @category
                                    AND appID = @app  ORDER BY typeID;";
            query.Parameters.AddWithValue("@accounD", User.ID);
            query.Parameters.AddWithValue("@category", app.Category);
            query.Parameters.AddWithValue("@app", app.AppID);
            read = query.ExecuteReader();

            catNumber = FindCategoryID(app.Category);

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
            const int id = 1;
            int count = 0;

            // Get the total amount of accounts and add it to id.
            query.CommandText = "SELECT * FROM ACCOUNT;";
            SQLiteDataReader read;

            read = query.ExecuteReader();

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return id + count;
        }

        /* Create ID */
        public int CreateLogID()
        {
            SQLiteCommand query = _con.CreateCommand();
            const int id = 100000001;
            int count = 0;

            // Get the total amount of logs and add it to id.
            query.CommandText = "SELECT * FROM LOG;";
            SQLiteDataReader read;

            read = query.ExecuteReader();

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return id + count;
        }


        /* Create Log Note ID. */
        public int CreatePostItID(int PostItListSize)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            const int LNID = 12000001;
            int id, count = 0;

            // Get the total amount of notaries and store it in LNID.
            query.CommandText = "SELECT * FROM PostIt;";

            read = query.ExecuteReader();

            while (read.Read())
            {

                ++count;

            }

            read.Close();



            id = PostItListSize > 1 ? LNID + count + PostItIDWatcher++ : LNID + count;

            return id;
        }


        public int CreateProjectID()
        {
            SQLiteCommand query = _con.CreateCommand();
            int count = 0, id = 1;

            query.CommandText = "SELECT * FROM PROJECT;";

            SQLiteDataReader read;
            read = query.ExecuteReader();

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return id + count;
        }

        public int CreateProjectID(ACCOUNT account, ApplicationClass app, string project)
        {
            SQLiteCommand query = _con.CreateCommand();
            int count = 0, id = 1;

            query.CommandText = "SELECT * FROM PROJECT;";

            SQLiteDataReader read;
            read = query.ExecuteReader();

            while (read.Read())
            {
                if(read.GetInt32(3) == FindCategoryID(app.Category) &&
                    read.GetInt32(2) == account.ID &&
                    read.GetInt32(1) == app.AppID && 
                    read.GetString(4) == project)
                {
                    id = read.GetInt32(0);
                    break;
                }
                else
                    ++count;
            }


            read.Close();

            return id + count;
        }


        public int CreateAppID()
        {
            SQLiteCommand query = _con.CreateCommand();
            int count = 0, id = 1;

            query.CommandText = "SELECT * FROM APPLICATION;";

            SQLiteDataReader read;
            read = query.ExecuteReader();

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return id + count;
        }

        public int CreateAppID(LOG.CATEGORY category, ACCOUNT account, string applicationName)
        {
            SQLiteCommand query = _con.CreateCommand();
            int count = 0, id = 1;

            var categoryColumn = 3;
            var accountColumn = 1;
            var appColumn = 2;

            query.CommandText = "SELECT * FROM APPLICATION;";

            SQLiteDataReader read;
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetInt32(categoryColumn) == FindCategoryID(category) &&
                    (read.GetInt32(accountColumn) == FindAccountID(account) ||
                        read.GetInt32(accountColumn) == 1) &&
                    read.GetString(appColumn) == applicationName)
                {
                    id = read.GetInt32(IDColumn);
                    break;
                }
                else
                    id = ++count;

            }

            read.Close();

            return id;
        }

        public int CreateSubjectID()
        {
            SQLiteCommand query = _con.CreateCommand();
            int count = 0, id = 1;

            query.CommandText = "SELECT * FROM Subject;";

            SQLiteDataReader read;
            read = query.ExecuteReader();

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return id + count + SubjectIDWatcher++;
        }

        public int CreateSubjectID(ProjectClass project, string subject, int PostItListSize)
        {
            SQLiteCommand query = _con.CreateCommand();
            int count = 0, staticID = 1, dynamicID = 1;
            const string NS = "No Subject";
            bool Found = false;

            var categoryColumn = 4;
            var accountColumn = 3;
            var appColumn = 2;
            var projectColumn = 1;
            var subjectColumn = 5;

            query.CommandText = "SELECT * FROM Subject;";

            if (subject == String.Empty || subject == NS)
            {
                subject = NS;
                return 1;
            }

            SQLiteDataReader read;
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetInt32(categoryColumn) == FindCategoryID(project.Category) &&
                    (read.GetInt32(accountColumn) == project.User.ID  ||
                    read.GetInt32(accountColumn) == 1) &&
                    read.GetInt32(appColumn) == project.Application.AppID &&
                    read.GetInt32(projectColumn) == project.ProjectID &&
                    read.GetString(subjectColumn) == subject)
                {
                    Found = true;
                    staticID = read.GetInt32(IDColumn);
                    count = 0;
                    break;
                }
                else
                    ++count;
            }

            read.Close();

            if(!Found)
            {
                dynamicID += PostItListSize > 1 ? count + SubjectIDWatcher++ : count;
            }

            return Found ? staticID : dynamicID;
        }

        public int CreateOutputID(ACCOUNT account, ApplicationClass app, string output)
        {
            SQLiteCommand query = _con.CreateCommand();
            int count = 0, id = 1;

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
                    id = read.GetInt32(IDColumn);
                    count = 0;
                    break;
                }
                else
                    ++count;
            }

            read.Close();

            return id + count;
        }

        public int CreateTypeID(ACCOUNT account, ApplicationClass app, string type)
        {
            SQLiteCommand query = _con.CreateCommand();
            int count = 0, id = -1;

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
                    id = read.GetInt32(IDColumn);
                    break;
                }
                else
                    id = ++count;
            }

            read.Close();

            return id;
        }

        public int CreateChecklistItemID()
        {
            SQLiteCommand query = _con.CreateCommand();
            int count = 0, id = 1;

            query.CommandText = "SELECT * FROM Checklist;";

            SQLiteDataReader read;
            read = query.ExecuteReader();

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return id + count;
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


                query.CommandText =
                    @"INSERT INTO LOG(logID, categoryID, accountID, projectID, appID, start, end, outputID, typeID)
                                VALUES(@id, @category, @author, @project, @app, @start, @end, @output, @type);
                ";

                query.Parameters.AddWithValue("@id", cachedLOG.ID);
                query.Parameters.AddWithValue("@category", FindCategoryID(cachedLOG.Category));
                query.Parameters.AddWithValue("@author", cachedLOG.Author.ID);

                var tempProjectID = FindProjectID(cachedLOG.Project);
                query.Parameters.AddWithValue("@project", tempProjectID);

                var tempAppID = FindAppID(cachedLOG.Application);
                query.Parameters.AddWithValue("@app", tempAppID);
                

                // TODO Check that times can be read properly by RetrieveLOGS()
                query.Parameters.AddWithValue("@start", cachedLOG.StartTime.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                query.Parameters.AddWithValue("@end", cachedLOG.EndTime.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
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
                                query.CommandText = @"INSERT INTO AndroidCodingLOG(fullORsimple, sync, gradleDaemon, runBuild, loadBuild, configBuild, allProjects)
                                                    VALUES(@fs, @s, @GD, @rb, @lb, @cb, @ap)";
                            else
                                query.CommandText = @"INSERT INTO AndroidCodingLOG(fullORsimple, sync)
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
                            query.Parameters.AddWithValue("@br", fn.BitRate);
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

        // Create one database record
        public void CreateLOG(LOG log)
        {
            SQLiteCommand query = _con.CreateCommand();

            /** Check that the PostIt has valid input and not empty strings */
            string pattern = "[A-Za-z]{5}[0-9]{0}";
            Regex xp = new Regex(pattern);
            const string UKN = "Unknown", NS = "No Subject";


            


            query.CommandText =
                    @"INSERT INTO LOG(logID, categoryID, accountID, projectID, appID, start, end, outputID, typeID)
                                VALUES(@id, @category, @author, @project, @app, @start, @end, @output, @type);
                ";

            query.Parameters.AddWithValue("@id", log.ID);
            query.Parameters.AddWithValue("@category", FindCategoryID(log.Category));
            query.Parameters.AddWithValue("@author", log.Author.ID);

            var tempAppID = FindAppID(log.Application);

            if (log.Application.Name == UKN || log.Application.Name == string.Empty)
                tempAppID = 3;
            query.Parameters.AddWithValue("@app", tempAppID);

            var tempProjectID = FindProjectID(log.Project);

            if (log.Project.Name == UKN || log.Project.Name == string.Empty)
                tempProjectID = 1;
            query.Parameters.AddWithValue("@project", tempProjectID);

            // TODO Check that times can be read properly by RetrieveLOGS()
            query.Parameters.AddWithValue("@start", log.StartTime.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
            query.Parameters.AddWithValue("@end", log.EndTime.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
            query.Parameters.AddWithValue("@output", log.Output.OutputID);
            query.Parameters.AddWithValue("@type", log.Type.TypeID);

            query.ExecuteNonQuery();

            /** LOG NOTE **/
            string er = "", so = "", su = "", co = "";

            foreach (PostIt postIt in log.PostItList)
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
                        @"INSERT INTO PostIt(postItID, logID, subjectID, error, date_found, solution, date_solved, suggestion, comment)
                                        VALUES(@id, @log, @subject, @error, @found, @solution, @solved, @suggest, @comment);
                ";

                query.Parameters.AddWithValue("@id", postIt.ID);
                query.Parameters.AddWithValue("@log", log.ID);

                // Subject

                var tempSubjectID = FindSubjectID(postIt.Subject);
                if (postIt.Subject.Subject == NS || postIt.Subject.Subject == string.Empty)
                    tempSubjectID = 1;

                query.Parameters.AddWithValue("@subject", tempSubjectID);

                // ERROR


                query.Parameters.AddWithValue("@error", er);
                // Date Time
                DateTime datum = new DateTime();
                datum = postIt.ERCaptureTime;
                if (xp.IsMatch(er))
                    query.Parameters.AddWithValue("@found", datum.ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));
                else
                    query.Parameters.AddWithValue("@found", "");

                // SOLUTION


                query.Parameters.AddWithValue("@solution", so);
                datum = postIt.SOCaptureTime;
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
                PostItIDWatcher = 0;

            }


            switch (log.Category)
            {
                case LOG.CATEGORY.CODING:
                    CodingLOG ctemp = (CodingLOG)log;

                    query.CommandText = @"INSERT INTO CodingLOG(logID, bugs, opened)
                                                VALUES(@id, @bugs, @opened);";

                    query.Parameters.AddWithValue("@id", log.ID);
                    query.Parameters.AddWithValue("@bugs", ctemp.Bugs);
                    query.Parameters.AddWithValue("@opened", ctemp.Success);

                    query.ExecuteNonQuery();

                    if (ctemp.Application.Name == Android)
                    {
                        var acl = (AndroidCodingLOG)ctemp;

                        if (acl.Scope == SCOPE.FULL)
                            query.CommandText = @"INSERT INTO AndroidCodingLOG(fullORsimple, sync, gradleDaemon, runBuild, loadBuild, configBuild, allProjects)
                                                    VALUES(@fs, @s, @GD, @rb, @lb, @cb, @ap)";
                        else
                            query.CommandText = @"INSERT INTO AndroidCodingLOG(fullORsimple, sync)
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


                    break;
                case LOG.CATEGORY.GRAPHICS:

                    GraphicsLOG gtemp = (GraphicsLOG)log;

                    query.CommandText = @"INSERT INTO GraphicsLOG(logID, mediumID, formatID, brush, height, width, unitID, 
                                                size, DPI, depth, completed, source)
                                                VALUES(@id, @medium, @format, @brush, @height, @width, @unit, @size, @DPI, 
                                                    @depth, @done, @source);";

                    query.Parameters.AddWithValue("@id", log.ID);
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

                    break;
                case LOG.CATEGORY.FILM:
                    FilmLOG ftemp = (FilmLOG)log;

                    query.CommandText = @"INSERT INTO FilmLOG(logID, height, width, length, completed, source)
                                                VALUES(@id, @height, @width, @length, @done, @source);";

                    query.Parameters.AddWithValue("@id", log.ID);
                    query.Parameters.AddWithValue("@height", ftemp.Height);
                    query.Parameters.AddWithValue("@width", ftemp.Width);
                    query.Parameters.AddWithValue("@length", ftemp.Length);
                    query.Parameters.AddWithValue("@done", ftemp.IsCompleted);
                    query.Parameters.AddWithValue("@source", ftemp.Source);

                    query.ExecuteNonQuery();


                    break;
                case LOG.CATEGORY.NOTES:
                    NotesLOG n = (NotesLOG)log;

                    if (n.notelogtype == NotesLOG.NOTELOGType.GENERIC)
                    {
                        query.CommandText = @"INSERT INTO NotesLOG(logID, noteLogType)
                                                    VALUES(@id, @category);
";

                        query.Parameters.AddWithValue("@id", log.ID);
                        query.Parameters.AddWithValue("@category", FindNoteLogTypeID(n.notelogtype));

                        query.ExecuteNonQuery();

                        NoteItem gn = (NoteItem)n;

                        query.CommandText = @"INSERT INTO NoteItem(logID, IsChecklist, subject, genericNote)
                                                    VALUES(@id, @checklist, @subject, @gn);
";

                        query.Parameters.AddWithValue("@id", log.ID);

                        var IsChecklist = gn.Items is not null && gn.Items.Count > 0;
                        query.Parameters.AddWithValue("@checklist", IsChecklist);
                        query.Parameters.AddWithValue("@subject", gn.Subject);
                        query.Parameters.AddWithValue("@gn", gn.Content);

                        query.ExecuteNonQuery();

                        // Store Check List Items Here
                        if(IsChecklist)
                        {
                            foreach (CheckListItem item in gn.Items)
                            {
                                AddCheckListItem(item.ID, log.ID, item.IsChecked, item.Item);
                            }
                        }
                    }
                    else
                    {
                        query.CommandText = @"INSERT INTO NotesLOG(logID, noteLogType)
                                                    VALUES(@id, @category);
";

                        query.Parameters.AddWithValue("@id", log.ID);
                        query.Parameters.AddWithValue("@category", FindNoteLogTypeID(n.notelogtype));

                        query.ExecuteNonQuery();

                        FlexiNotesLOG fn = (FlexiNotesLOG)log;

                        query.CommandText = @"INSERT INTO FlexiNotesLOG(logID, flexiNoteTypeID, mediumID, formatID, bitRate, length,
                                                        completed, source)
                                                    VALUES(@id, @fnCategory, @medium, @format, @br, @length, @done, @source, @gc);
";

                        query.Parameters.AddWithValue("@id", log.ID);
                        query.Parameters.AddWithValue("@fnCategory", FindFNCategory(fn.flexinotetype));
                        query.Parameters.AddWithValue("@medium", FindMediumID(fn.Category, fn.Medium));
                        query.Parameters.AddWithValue("@format", FindFormatID(fn.Category, fn.Format));
                        query.Parameters.AddWithValue("@br", fn.BitRate);
                        query.Parameters.AddWithValue("@length", fn.Length);
                        query.Parameters.AddWithValue("@done", fn.IsCompleted);
                        query.Parameters.AddWithValue("@source", fn.Source);

                        query.ExecuteNonQuery();

                    }


                    break;
            }


        }
    }
}
