using Data_Logger_1._3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data_Logger_1._3.Services
{
    public class ENTITYHANDLER
    {
        private readonly IServiceProvider _serviceProvider;


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

        /// <summary>
        /// Updates a log in the database.
        /// </summary>
        /// <param name="log">The log needing updates.</param>
        /// <returns>Returns a scope. (use of the scope is optional)</returns>
        public async Task UpdateLogAsync(LOG log)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

                var existingLog = await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstOrDefaultAsync(l => l.ID == log.ID);

                if (existingLog == null)
                    throw new ArgumentNullException("Cannot update a log that doesn't exist.");

                master.Entry(existingLog).CurrentValues.SetValues(log);

                if (log.Application != null)
                    existingLog.Application = master.Applications.Local
                        .FirstOrDefault(a => a.appID == log.Application.appID) ?? log.Application;

                if (log.Project != null)
                    existingLog.Project = master.Projects.Local
                        .FirstOrDefault(p => p.projectID == log.Project.projectID) ?? log.Project;

                if (log.Output != null)
                    existingLog.Output = master.Outputs.Local
                        .FirstOrDefault(o => o.outputID == log.Output.outputID) ?? log.Output;

                if (log.Type != null)
                    existingLog.Type = master.Types.Local
                        .FirstOrDefault(t => t.typeID == log.Type.typeID) ?? log.Type;

                var incomingIds = log.PostItList
                    .Where(p => p.postItID != 0)
                    .Select(p => p.postItID)
                    .ToHashSet();

                // REMOVE
                foreach (var postIt in existingLog.PostItList.ToList())
                {
                    if (!incomingIds.Contains(postIt.postItID))
                    {
                        master.PostIts.Remove(postIt);
                    }
                }

                // ADD + UPDATE
                foreach (var postIt in log.PostItList)
                {
                    if (postIt.postItID == 0)
                    {
                        // resolve subject
                        var trackedSubject = master.Subjects.Local
                            .FirstOrDefault(s => s.subjectID == postIt.subjectID) ?? postIt.Subject;

                        // create new entity
                        var newPostIt = new PostIt
                        {
                            Error = postIt.Error,
                            Solution = postIt.Solution,
                            Suggestion = postIt.Suggestion,
                            Comment = postIt.Comment,
                            ERCaptureTime = postIt.ERCaptureTime,
                            SOCaptureTime = postIt.SOCaptureTime,

                            Subject = trackedSubject,
                            subjectID = trackedSubject.subjectID
                        };

                        existingLog.PostItList.Add(newPostIt);
                    }
                    else
                    {
                        var trackedPostIt = existingLog.PostItList
                            .First(p => p.postItID == postIt.postItID);

                        // explicit update
                        trackedPostIt.Error = postIt.Error;
                        trackedPostIt.Solution = postIt.Solution;
                        trackedPostIt.Suggestion = postIt.Suggestion;
                        trackedPostIt.Comment = postIt.Comment;
                        trackedPostIt.ERCaptureTime = postIt.ERCaptureTime;
                        trackedPostIt.SOCaptureTime = postIt.SOCaptureTime;

                        if (postIt.Subject != null)
                        {
                            var trackedSubject = master.Subjects.Local
                                .FirstOrDefault(s => s.subjectID == postIt.Subject.subjectID) ?? postIt.Subject;

                            trackedPostIt.Subject = trackedSubject;
                            trackedPostIt.subjectID = trackedSubject.subjectID;
                        }
                    }
                }

                await master.SaveChangesAsync();
            }
            catch (ArgumentNullException nullex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();
                await writer.HandleExceptionAsync(nullex, "UpdateLogAsync(log)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();
                await writer.HandleExceptionAsync(ex, "UpdateLogAsync(log)");
            }
        }

        public async Task<bool> UpdateNotesLog(NoteItem noteItem)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            bool isUpdated = false;

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();
                master.NoteItems.Update(noteItem);

                await master.SaveChangesAsync();
                isUpdated = true;

            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();
                await writer.HandleExceptionAsync(ex, "UpdateLogAsync(log)");
            }

            return isUpdated;
        }






        #endregion












        #region Deletions



        public async Task<bool> DeleteLOG(LOG log)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

                if (log == null)
                    return false;

                master.Remove(log);

                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();
                await writer.HandleExceptionAsync(ex, "DeleteLOG(log)");
            }

            return false;
        }

        public async Task<bool> DeleteLOGByID(int ID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();

                var logDeletionCandidate = await master.Logs
                .FirstOrDefaultAsync(log => log.ID == ID);

                if (logDeletionCandidate == null)
                    return false;

                master.Logs.Remove(logDeletionCandidate);

                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();
                await writer.HandleExceptionAsync(ex, "DeleteLOGByID(ID)");
            }

            return false;
        }

        public async Task<bool> DeleteNote(int ID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<ENTITYMASTER>();
            var writer = scope.ServiceProvider.GetRequiredService<ENTITYWRITER>();

            try
            {
                var noteToDelete = await master.NoteItems
                    .FirstOrDefaultAsync(n => n.ID == ID);

                if (noteToDelete == null)
                    return false;

                master.NoteItems.Remove(noteToDelete);

                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                await writer.HandleExceptionAsync(ex, "DeleteNote(ID)");
            }

            return false;
        }











        #endregion




    }
}
