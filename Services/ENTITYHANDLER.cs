using Data_Logger_1._3.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Logger_1._3.Services
{
    public class ENTITYHANDLER
    {
        private readonly IServiceProvider _serviceProvider;
        private ENTITYMASTER _master;


        /// <summary>
        /// The Entity Framework updater and deleter for ENTITYMASTER.
        /// </summary>
        public ENTITYHANDLER(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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



        public async Task<bool> DeleteLOG(LOG log)
        {
            if(log == null)
                return false;

            _master.Remove(log);

            await _master.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteLOGByID(int ID)
        {
            var logDeletionCandidate = await _master.Logs
                .FirstOrDefaultAsync(log => log.ID == ID);

            if (logDeletionCandidate == null)
                return false;

            _master.Logs.Remove(logDeletionCandidate);

            await _master.SaveChangesAsync();

            return true;
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
