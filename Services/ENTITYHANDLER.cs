using Data_Logger_1._3.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Logger_1._3.Services
{
    public class ENTITYHANDLER
    {
        private readonly ENTITYMASTER _master;


        /// <summary>
        /// The Entity Framework updater and deleter for ENTITYMASTER.
        /// </summary>
        public ENTITYHANDLER(ENTITYMASTER entityMaster)
        {
            _master = entityMaster;
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

        public async Task<bool> DeleteNote(int ID)
        {
            try
            {
                var noteToDelete = await _master.NoteItems
                    .FirstOrDefaultAsync(n => n.ID == ID);

                if (noteToDelete == null)
                    return false;

                _master.NoteItems.Remove(noteToDelete);

                await _master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                //
                return false;
            }
        }











        #endregion




    }
}
