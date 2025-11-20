using Data_Logger_1._3.Models;

namespace Data_Logger_1._3.Services
{
    public class ENTITYHANDLER
    {
        /// <summary>
        /// The Entity Framework updater and deleter for ENTITYMASTER.
        /// </summary>
        public ENTITYHANDLER()
        {
            
        }


        #region Updates



        public async Task<bool> UpdateQtLog(LOG log)
        {
            bool isUpdated = false;


            return isUpdated;
        }

        public async Task<bool> UpdateNotesLog(NoteItem noteItem)
        {
            bool isUpdated = false;


            return isUpdated;
        }






        #endregion












        #region Deletions



        public async Task<bool> DeleteLog(LOG log)
        {
            bool isDeleted = false;

            return isDeleted;
        }










        #endregion




    }
}
