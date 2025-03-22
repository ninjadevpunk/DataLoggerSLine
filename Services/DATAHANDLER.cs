using Data_Logger_1._3.Models;
using System.Data.SQLite;
using System.Diagnostics;

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
                if (!UpdatePostIt(log, transaction))
                {
                    transaction.Rollback();
                    return false;
                }


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

        public bool UpdatePostIt(LOG log, SQLiteTransaction transaction)
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
        }

        public bool PostItExists(int logID, int postItID, SQLiteTransaction transaction)
        {
            bool postItExists = false;



            return postItExists;
        }





        #endregion


        #region Deletions




        public bool DeleteLog(int logID)
        {
            bool isDeleted = false;

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

                    if (!DeletePostIt(logID, transaction))
                    {
                        transaction.Rollback();
                        return false;
                    }

                    // Do for subclasses




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


        public bool DeletePostIt(int logID, SQLiteTransaction transaction)
        {
            bool postItDeleted = false;

            try
            {
                using var delete = _con.CreateCommand();
                delete.Transaction = transaction;
                delete.CommandText = $@"DELETE FROM PostIt WHERE {Column.LogID} = @logID;";
                delete.Parameters.AddWithValue("@logID", logID);

                int rowsAffected = delete.ExecuteNonQuery();

                postItDeleted = rowsAffected > 0;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found in DeletePostIt(logID,transaction): {sqlex.Message}");
                postItDeleted = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found in DeletePostIt(logID,transaction): {ex.Message}");
                postItDeleted = false;
            }

            return postItDeleted;
        }




        #endregion











        #endregion
    }
}
