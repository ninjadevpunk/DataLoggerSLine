using Data_Logger_1._3.Models;
using System.Data.SQLite;
using System.Diagnostics;
using static Data_Logger_1._3.Models.NotesLOG;

namespace Data_Logger_1._3.Services
{
    /// <summary>
    /// The class for all database handling activities such as updates and deletions.
    /// </summary>
    public class DATAHANDLER : DATAMASTER
    {
        protected readonly DATAREADER _reader;

        /* CONSTRUCTORS */
        public DATAHANDLER() 
        {
        }

        public DATAHANDLER(DATAREADER reader)
        {
            _reader = reader;
        }

        public DATAHANDLER(string category, SQLiteConnection connection, bool status) : base(connection, category, status) { }










        #region Methods









        #region Updates


        public bool UpdateQtLog(LOG log)
        {
            bool IsUpdated = false;

            using var transaction = _con.BeginTransaction();
            try
            {
                // Update Log
                using var update = _con.CreateCommand();
                update.Transaction = transaction;

                update.CommandText = $@"UPDATE LOG SET {Column.ProjectID} = @projectID,
                                        start = @startDate,
                                        end = @endDate,
                                        {Column.OutputID} = @outputID,
                                        {Column.TypeID} = @typeID
                                            WHERE {Column.LogID} = @logID
                                                AND {Column.AccountID} = @accountID
;";

                update.Parameters.AddWithValue("@projectID", log.ID);
                update.Parameters.AddWithValue("@accountID", log.Author.ID);
                update.Parameters.AddWithValue("@startDate", DATAWRITER.GetDateTimeString(log.Start));
                update.Parameters.AddWithValue("@endDate", DATAWRITER.GetDateTimeString(log.End));
                update.Parameters.AddWithValue("@outputID", log.Output.OutputID);
                update.Parameters.AddWithValue("@typeID", log.Type.TypeID);

                int rowsAffected = update.ExecuteNonQuery();
                IsUpdated = rowsAffected > 0;

                if(!IsUpdated)
                {
                    transaction.Rollback();
                    throw new("Database update failed!");
                }

                // Update PostIts in separate function
                /*if (!UpdatePostIt(log, transaction))
                {
                    transaction.Rollback();
                    return false;
                }*/


                // Update for subclasses



            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found in UpdateLog(logID): {sqlex.Message}");
                IsUpdated = false;
                transaction.Rollback();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found in UpdateLog(logID): {ex.Message}");
                IsUpdated = false;
                transaction.Rollback();
            }


            return IsUpdated;
        }

        /*public bool UpdatePostIt(LOG log, SQLiteTransaction transaction)
        {
            bool isUpdated = true;

            try
            {
                // Step 1: Update existing PostIts
                foreach (var postIt in log.PostItList)
                {
                    if (PostItExists(log.ID, postIt.ID, transaction)) // Function to check if PostIt exists in the database
                    {
                        using var update = _con.CreateCommand();
                        update.Transaction = transaction;
                        update.CommandText = $@"
                    UPDATE PostIt 
                    SET {Column.Content} = @content
                    WHERE {Column.ID} = @postItID;";
                        update.Parameters.AddWithValue("@content", postIt.Content);
                        update.Parameters.AddWithValue("@postItID", postIt.ID);

                        if (update.ExecuteNonQuery() == 0)
                            isUpdated = false; // If an update fails, mark it as false
                    }
                }

                // Step 2: Insert new PostIts that don’t exist in the database
                InsertNewPostIts(log, transaction);

                // Step 3: Delete PostIts from the database that are no longer in log.PostItList
                DeleteRemovedPostIts(log, transaction);
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found in UpdatePostIt(log, transaction): {sqlex.Message}");
                isUpdated = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found in UpdatePostIt(log, transaction): {ex.Message}");
                isUpdated = false;
            }

            return isUpdated;
        }*/

        public bool PostItExists(int logID, int postItID, SQLiteTransaction transaction)
        {
            bool postItExists = false;



            return postItExists;
        }



        public bool UpdateNotesLog(NoteItem noteItem)
        {
            bool notesIsUpdated = false;







            return notesIsUpdated;
        }





        #endregion


        #region Deletions




        /// <summary>
        /// Deletes a log from the LOG table. It also deletes related records in other tables.
        /// </summary>
        /// <param name="logID"></param>
        /// <returns>Returns if the log and records with the log's log ID has been successfully deleted in full.</returns>
        public bool DeleteLog(LOG log)
        {
            bool isDeleted = false;
            int logID = log.ID;

            using (var transaction = _con.BeginTransaction())
            {
                try
                {

                    // LOG 

                    using (var delete = _con.CreateCommand())
                    {
                        delete.Transaction = transaction;
                        delete.CommandText = $@"DELETE FROM LOG WHERE {Column.LogID} = @logID
                                                AND {Column.AccountID} = @accountID;
";
                        delete.Parameters.AddWithValue("@logID", logID);
                        delete.Parameters.AddWithValue("@accountID", User.ID);

                        int rowsAffected = delete.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0;
                    }


                    // PostIt

                    var noteLogType = _reader.FindNoteLogTypeByLogID(logID, _con, transaction);

                    if (noteLogType is not null && noteLogType is NOTELOGType.FLEXI)
                    {
                        if (!DeleteLog(logID, "PostIt", transaction))
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    // Do for subclasses
                    using var read = _con.CreateCommand();
                    read.Transaction = transaction;

                    read.CommandText = $@"SELECT {Column.CategoryID} FROM LOG WHERE logID = @logID
;";
                    read.Parameters.AddWithValue("@logID", logID);

                    object result = read.ExecuteScalar();




                    switch (log.Category)
                    {
                        case LOG.CATEGORY.CODING:
                            {
                                if (!DeleteLog(logID, "CodingLOG", transaction))
                                {
                                    transaction.Rollback();
                                    return false;
                                }

                                DeleteLog(logID, "AndroidCodingLOG", transaction);

                                break;
                            }
                        case LOG.CATEGORY.GRAPHICS:
                            {
                                isDeleted = DeleteLog(logID, "GraphicsLOG", transaction);

                                break;
                            }
                        case LOG.CATEGORY.FILM:
                            {
                                isDeleted = DeleteLog(logID, "FilmLOG", transaction);

                                break;
                            }
                        case LOG.CATEGORY.NOTES:
                            {
                                isDeleted = DeleteLog(logID, "NotesLOG", transaction);

                                if (noteLogType is null)
                                {
                                    transaction.Rollback();
                                    return false;
                                }

                                if (noteLogType == NOTELOGType.GENERIC)
                                {
                                    isDeleted = DeleteLog(logID, "NoteItem", transaction);

                                    var note = (NotesLOG)log;
                                    var noteItem = (NoteItem)note;

                                    var isChecklist = noteItem.Items is not null;
                                    if(isChecklist)
                                        isDeleted = DeleteLog(logID, "Checklist", transaction);

                                }
                                else
                                {
                                    isDeleted = DeleteLog(logID, "FlexiNotesLOG", transaction);
                                }
                                break;
                            }
                    }



                    if (isDeleted)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
                catch (SQLiteException sqlex)
                {
                    Debug.WriteLine($"SQLiteException found in DeleteLog(logID): {sqlex.Message}");
                    isDeleted = false;
                    transaction.Rollback();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception found in DeleteLog(logID): {ex.Message}");
                    isDeleted = false;
                    transaction.Rollback();
                }
            }

            return isDeleted;
        }

        /// <summary>
        /// Helps DeleteLog(logID) to delete related records. Shouldn't be used outside the DATAHANDLER.
        /// </summary>
        /// <param name="logID"></param>
        /// <param name="transaction"></param>
        /// <returns>Returns whether the record(s) has been successfully deleted.</returns>
        public bool DeleteLog(int logID, string tableName, SQLiteTransaction transaction)
        {
            bool logDeleted = false;

            try
            {
                using var delete = _con.CreateCommand();
                delete.Transaction = transaction;
                delete.CommandText = $@"DELETE FROM {tableName} WHERE {Column.LogID} = @logID;";
                delete.Parameters.AddWithValue("@logID", logID);

                int rowsAffected = delete.ExecuteNonQuery();

                logDeleted = rowsAffected > 0;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found in DeleteLog(logID,transaction): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found in DeleteLog(logID,transaction): {ex.Message}");
            }

            return logDeleted;
        }




        #endregion











        #endregion
    }
}
