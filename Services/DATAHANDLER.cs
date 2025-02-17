using System.Data.SQLite;
using System.Diagnostics;

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
        public DATAHANDLER() 
        {
            CreateConnection();
            CheckTables();
            CheckCategories();
            CheckApplications();
            CheckProject();
            CheckOutputs();
            CheckTypes();
            CheckSubjects();
            CheckMediums();
            CheckFormats();
            CheckUnits();
            CheckFNCategories();
        }

        public DATAHANDLER(string category, SQLiteConnection connection, bool status) : base(connection, category, status) { }










        #region Methods









        #region Updates









        #endregion


        #region Deletions




        public bool DeleteLog(int logID)
        {
            bool isDeleted = false;

            using (var transaction = _con.BeginTransaction())
            {
                try
                {
                    if (!DeletePostIt(logID, transaction))
                    {
                        transaction.Rollback();
                        return false;
                    }

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
                using (var delete = _con.CreateCommand())
                {
                    delete.Transaction = transaction;
                    delete.CommandText = $@"DELETE FROM PostIt WHERE {Column.LogID} = @logID;";
                    delete.Parameters.AddWithValue("@logID", logID);

                    int rowsAffected = delete.ExecuteNonQuery();

                    postItDeleted = rowsAffected > 0;
                }
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
