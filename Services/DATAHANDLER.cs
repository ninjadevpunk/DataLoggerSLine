using Data_Logger_1._3.Models;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Data_Logger_1._3.Services
{
    public class DATAHANDLER : DATAMASTER
    {
        /*
        * DOCUMENTATION
        * 
        * The class for all database handling activities such as updates.
        * Please use this class for reading the database!
        * Designed for Editor.
        */


        /* CONSTRUCTORS */
        public DATAHANDLER() { }

        public DATAHANDLER(string category, SQLiteConnection connection, bool status) : base(connection, category, status) { }

        /* MEMBER FUNCTION */


        
//        public void updateLOG(LOG log)
//        {
//            /** Retrieve a LOG's index first before updating.
//             * To update a LOG, you must retrieve the LOGs from the database and match the index with the position of the LOG.
//             * In other words, index 5 will point to the position of the logNoteID no.6.
//             */
//            SQLiteCommand query = _con.CreateCommand();
//            SQLiteDataReader read;

//            /** Check that the LOG_NOTE has valid input and not empty strings */
//            string pattern = "[A-Za-z]{5}[0-9]{0}";
//            Regex xp = new Regex(pattern);



//            if (LOG_ID != 100000000)
//            {
//                query.CommandText = $@"UPDATE LOG
//                                   SET {Column.AccountID} = @account,
//                                       {Column.ProjectID} = @project,
//                                       start = @start,
//                                       end = @end,
//                                       outputID = @output,
//                                       typeID = @type
//                                       WHERE logID = @id
//;";

//                query.Parameters.AddWithValue("@id", LOG_ID);
//                query.Parameters.AddWithValue("@account", log.Author.ID);
//                query.Parameters.AddWithValue("@project", log.Project.ProjectID);
//                query.Parameters.AddWithValue("@start", log.Start);
//                query.Parameters.AddWithValue("@end", log.End);
//                query.Parameters.AddWithValue("@output", log.Output.OutputID);
//                query.Parameters.AddWithValue("@type", log.Type.TypeID);

//                query.ExecuteNonQuery();


//                List<int> LNIDs = new List<int>(); // List of Log Note IDs for all the log notes that need updating.

//                query.CommandText = "SELECT * FROM LOG_NOTE ORDER BY logNoteID;";
//                read = query.ExecuteReader();


//                while (read.Read())
//                {
//                    if (read.GetInt32(1) == LOG_ID)
//                    {
//                        LNIDs.Add(read.GetInt32(0));
//                    }
//                }

//                read.Close();

//                string er, so, su, co;

//                query.CommandText = "DELETE FROM LOG_NOTE WHERE logID = @id;";

//                query.Parameters.AddWithValue("@id", LOG_ID);

//                query.ExecuteNonQuery();

//                //for (int i = 0; i < LNIDs.Count; i++)
//                //{
//                //    query.CommandText = "INSERT INTO LOG_NOTE(logID, subjectID, error, date_found, solution, date_solved, suggestion, comment)" +
//                //        "                   VALUES(@id, @subject, @error, @date1, @solution, @date2, @suggestion, @comment);";

//                //    query.Parameters.AddWithValue("@id", LOG_ID);


//                //    query.Parameters.AddWithValue("@subject", findSubjectID(log.getNotarised().ElementAt(i).getSubject()));

//                //    er = xp.IsMatch(log.getNotarised().ElementAt(i).getError()) ? log.getNotarised().ElementAt(i).getError() : "";
//                //    query.Parameters.AddWithValue("@error", er);

//                //    if (xp.IsMatch(log.getNotarised().ElementAt(i).getError()))
//                //        query.Parameters.AddWithValue("@date1", log.getNotarised().ElementAt(i).getERCaptureTime().ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));

//                //    so = xp.IsMatch(log.getNotarised().ElementAt(i).getSolution()) ? log.getNotarised().ElementAt(i).getSolution() : "";
//                //    query.Parameters.AddWithValue("@solution", so);

//                //    if (xp.IsMatch(log.getNotarised().ElementAt(i).getSolution()))
//                //        query.Parameters.AddWithValue("@date2", log.getNotarised().ElementAt(i).getSOCaptureTime().ToString("dddd dd MMMM yyyy HH:mm:ss.fff"));

//                //    su = xp.IsMatch(log.getNotarised().ElementAt(i).getSuggestion()) ? log.getNotarised().ElementAt(i).getSuggestion() : "";
//                //    query.Parameters.AddWithValue("@suggestion", su);

//                //    co = xp.IsMatch(log.getNotarised().ElementAt(i).getComment()) ? log.getNotarised().ElementAt(i).getComment() : "";
//                //    query.Parameters.AddWithValue("@comment", co);

//                //    query.ExecuteNonQuery();
//                //}

//                //var temp = log;

//                //switch (temp.getCATEGORY())
//                //{
//                //    case "CODING":
//                //        {
//                //            var clog = (CODE_LOG)temp;

//                //            query.CommandText = "UPDATE CODE_LOG SET bugs = @bugs, opened = @opened WHERE logID = @id;";

//                //            query.Parameters.AddWithValue("@id", LOG_ID);
//                //            query.Parameters.AddWithValue("@bugs", clog.getBugs());
//                //            query.Parameters.AddWithValue("@opened", clog.getSuccess());

//                //            query.ExecuteNonQuery();

//                //            if (clog.getApplicationName() == "Android Studio Electric Eel")
//                //            {
//                //                var andLOG = (ANDROID_CODE_LOG)clog;
//                //                query.CommandText = "UPDATE ANDROID_CODE_LOG SET sync = @sync, gradleDaemon = @GD, runBuild = @runBuild, loadBuild = @loadBuild, " +
//                //                    "                   configBuild = @configBuild, allProjects = @AP WHERE logID = @id;";

//                //                query.Parameters.AddWithValue("@id", LOG_ID);
//                //                query.Parameters.AddWithValue("@sync", andLOG.getSync().ToString("HH:mm:ss.fff"));
//                //                query.Parameters.AddWithValue("@GD", andLOG.getGradleDaemon().ToString("HH:mm:ss.fff"));
//                //                query.Parameters.AddWithValue("@runBuild", andLOG.getBuildRun().ToString("HH:mm:ss.fff"));
//                //                query.Parameters.AddWithValue("@loadBuild", andLOG.getBuildLoad().ToString("HH:mm:ss.fff"));
//                //                query.Parameters.AddWithValue("@configBuild", andLOG.getBuildConfig().ToString("HH:mm:ss.fff"));
//                //                query.Parameters.AddWithValue("@AP", andLOG.getAllProjects().ToString("HH:mm:ss.fff"));

//                //                query.ExecuteNonQuery();
//                //            }
//                //        }
//                //        break;
//                //    case "GRAPHICS":
//                //        {
//                //            var glog = (GRAPHICS_LOG)temp;

//                //            query.CommandText = "UPDATE GRAPHICS_LOG SET mediumID = @medium, formatID = @format, brush = @brush, height = @height, width = @width, unitID = @unit," +
//                //                "                   size = @size, DPI = @dpi, depth = @depth, completed = @done, source = @source WHERE logID = @id;";

//                //            query.Parameters.AddWithValue("@id", LOG_ID);
//                //            query.Parameters.AddWithValue("@medium", findMediumID(glog.getMedium()));
//                //            query.Parameters.AddWithValue("@format", findFormatID(glog.getFormat()));
//                //            query.Parameters.AddWithValue("@brush", glog.getBrush());
//                //            query.Parameters.AddWithValue("@height", glog.getHeight());
//                //            query.Parameters.AddWithValue("@width", glog.getWidth());
//                //            query.Parameters.AddWithValue("@unit", findUnitID(glog.getUnit()));
//                //            query.Parameters.AddWithValue("@size", glog.getSize());
//                //            query.Parameters.AddWithValue("@dpi", glog.getDPI());
//                //            query.Parameters.AddWithValue("@depth", glog.getDepth());
//                //            query.Parameters.AddWithValue("@done", glog.getCompleted());
//                //            query.Parameters.AddWithValue("@source", glog.getSource());

//                //            query.ExecuteNonQuery();
//                //        }
//                //        break;
//                //    case "FILM":
//                //        {
//                //            var flog = (FILM_LOG)temp;

//                //            query.CommandText = "UPDATE FILM_LOG SET height = @height, width = @width, unitID = @unit, length = @length, completed = @done, source = @source" +
//                //                "                   WHERE logID = @id;";

//                //            query.Parameters.AddWithValue("@id", LOG_ID);
//                //            query.Parameters.AddWithValue("@height", flog.getHeight());
//                //            query.Parameters.AddWithValue("@width", flog.getWidth());
//                //            query.Parameters.AddWithValue("@unit", findUnitID(flog.getUnit()));
//                //            query.Parameters.AddWithValue("@length", flog.getLength());
//                //            query.Parameters.AddWithValue("@done", flog.isCompleted());
//                //            query.Parameters.AddWithValue("@source", flog.getSource());

//                //            query.ExecuteNonQuery();
//                //        }
//                //        break;
//                //    case "NOTES":
//                //        {
//                //            var n = (NOTE_LOG)log;

//                //            if (n.getNoteLogType())
//                //            {
//                //                var gen = (GENERIC_NOTE)n;

//                //                query.CommandText = "UPDATE GENERIC_NOTE_LOG SET category = @category, subject = @subject, genericNote = @GN WHERE logID = @id;";

//                //                query.Parameters.AddWithValue("@id", LOG_ID);
//                //                query.Parameters.AddWithValue("@category", gen.getCategory());
//                //                query.Parameters.AddWithValue("@subject", gen.getSubject());
//                //                query.Parameters.AddWithValue("@GN", gen.getNote());

//                //                query.ExecuteNonQuery();

//                //                if (!gen.getCategory())
//                //                {

//                //                    List<int> GNLIDs = new List<int>(); // Keep the gnlCheckIDs so we can update each value.
//                //                    query.CommandText = "SELECT * FROM GNL_CHECKLIST ORDER BY logID;";
//                //                    read = query.ExecuteReader();

//                //                    while (read.Read())
//                //                    {
//                //                        if (read.GetInt32(1) == LOG_ID)
//                //                        {
//                //                            GNLIDs.Add(read.GetInt32(0));
//                //                        }
//                //                    }

//                //                    read.Close();


//                //                    query.CommandText = "DELETE FROM GNL_CHECKLIST WHERE logID = @id;";
//                //                    query.Parameters.AddWithValue("@id", LOG_ID);

//                //                    query.ExecuteNonQuery();


//                //                    for (int i = 0; i < gen.getItems().Count; i++)
//                //                    {
//                //                        query.CommandText = "INSERT INTO GNL_CHECKLIST(logID, item, done)" +
//                //                            "                   VALUES(@id, @item, @done);";

//                //                        query.Parameters.AddWithValue("@id", LOG_ID);
//                //                        query.Parameters.AddWithValue("@item", gen.getItems().getItem(i));
//                //                        query.Parameters.AddWithValue("@done", gen.getItems().isChecked(i));

//                //                        query.ExecuteNonQuery();
//                //                    }
//                //                }
//                //            }
//                //            else
//                //            {
//                //                var flex = (FLEXI_NOTE)n;

//                //                query.CommandText = "UPDATE FLEXI_NOTE_LOG SET flexiNoteID = @FNID, mediumID = @medium, formatID = @format, bitRate = @bitrate, length = @length," +
//                //                    "                   completed = @done, source = @source, gameCategory = @GC WHERE logID = @id;";

//                //                query.Parameters.AddWithValue("@id", LOG_ID);
//                //                query.Parameters.AddWithValue("@FNID", findFNCategory(flex.getFNCategory()));
//                //                query.Parameters.AddWithValue("@medium", findMediumID(flex.getMedium()));
//                //                query.Parameters.AddWithValue("@format", findFormatID(flex.getFormat()));
//                //                query.Parameters.AddWithValue("@bitrate", flex.getBitRate());
//                //                query.Parameters.AddWithValue("@length", flex.getLength());
//                //                query.Parameters.AddWithValue("@done", flex.isCompleted());
//                //                query.Parameters.AddWithValue("@source", flex.getSource());
//                //                query.Parameters.AddWithValue("@GC", flex.getGameCategory());


//                //                query.ExecuteNonQuery();
//                //            }
//                //        }
//                //        break;
//                //}
//            }


//        }

        public static List<LOG> Sort(List<LOG> list, bool asc_desc)
        {
            /** Return the sorted list. true means ascending.
             */
            return list;
        }
    }
}
