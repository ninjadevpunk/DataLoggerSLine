using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data_Logger_1._3.Services
{
    public class EntityHandler
    {
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// The Entity Framework updater and deleter for EntityMaster.
        /// </summary>
        public EntityHandler(IServiceProvider serviceProvider)
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
        /// <returns></returns>
        public async Task UpdateLogAsync(LOG log)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

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
                {
                    if (log.Application.appID == 0)
                    {
                        // New App
                        existingLog.Application = log.Application;
                    }
                }


                // PROJECT
                if (log.projectID != 0 && log.projectID != existingLog.projectID)
                {
                    existingLog.projectID = log.projectID;
                    existingLog.Project = null;
                }
                else if (log.Project != null && log.Project.projectID == 0)
                {
                    // If the application is existing, use the tracked app; 
                    // if it's new, the reference is already set correctly in the command
                    log.Project.Application = existingLog.Application!;
                    existingLog.Project = log.Project;
                }

                if (log.Output != null)
                    existingLog.Output = master.Outputs
                        .FirstOrDefault(o => o.outputID == log.Output.outputID) ?? log.Output;

                if (log.Type != null)
                    existingLog.Type = master.Types
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
                    // resolve subject

                    SubjectClass? trackedSubject = null;
                    bool subjectIsNew = false;

                    if (postIt.Subject != null)
                    {
                        subjectIsNew = postIt.Subject.subjectID == 0;

                        if (subjectIsNew)
                        {
                            trackedSubject = postIt.Subject;
                        }
                        else
                        {
                            trackedSubject = await master.Subjects
                                .FirstOrDefaultAsync(s => s.subjectID == postIt.Subject.subjectID) ?? postIt.Subject;
                        }
                    }
                    else
                    {
                        // Subject Exists
                        trackedSubject = master.Subjects
                            .FirstOrDefault(s => s.subjectID == postIt.subjectID);
                    }




                    if (trackedSubject != null)
                    {
                        // Subject is NEW
                        if (subjectIsNew)
                        {
                            if (existingLog.Application == null || existingLog.Project == null)
                                throw new InvalidOperationException("Application and Project must be set before creating a new Subject.");

                            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                            trackedSubject.Application = existingLog.Application;
                            trackedSubject.Project = existingLog.Project;

                            await writer.AddSubject(trackedSubject, scope);
                        }



                        // Resolve PostIts

                        // PostIt is NEW
                        if (postIt.postItID == 0)
                        {

                            // create new entity
                            var newPostIt = new PostIt
                            {
                                accountID = existingLog.Author.accountID,
                                logID = existingLog.ID,
                                Error = postIt.Error,
                                Solution = postIt.Solution,
                                Suggestion = postIt.Suggestion,
                                Comment = postIt.Comment,
                                ERCaptureTime = postIt.ERCaptureTime,
                                SOCaptureTime = postIt.SOCaptureTime,
                            };

                            if (!subjectIsNew)
                                newPostIt.subjectID = trackedSubject.subjectID;
                            else
                                newPostIt.Subject = trackedSubject;

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

                            if (subjectIsNew)
                                trackedPostIt.Subject = trackedSubject;
                            else
                                trackedPostIt.subjectID = trackedSubject.subjectID;

                        }
                    }
                }

                await master.SaveChangesAsync();
            }
            catch (ArgumentNullException nullex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(nullex, "UpdateLogAsync(log)", "InvalidOperationException");
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex, "UpdateLogAsync(log)");
            }
        }

        public async Task<bool> UpdateNotesLog(NoteItem noteItem)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            bool isUpdated = false;

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
                master.NoteItems.Update(noteItem);

                await master.SaveChangesAsync();
                isUpdated = true;

            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
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
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

                if (log == null)
                    return false;

                master.Remove(log);

                await master.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex, "DeleteLOG(log)");
            }

            return false;
        }

        public async Task<bool> DeleteLOGByID(int ID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            try
            {
                var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();

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
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                await writer.HandleExceptionAsync(ex, "DeleteLOGByID(ID)");
            }

            return false;
        }

        public async Task<bool> DeleteNote(int ID)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var master = scope.ServiceProvider.GetRequiredService<EntityMaster>();
            var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

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
