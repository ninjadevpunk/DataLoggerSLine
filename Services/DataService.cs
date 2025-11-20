using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using static Data_Logger_1._3.Models.LOG;
using static Data_Logger_1._3.Services.Cachemaster;

namespace Data_Logger_1._3.Services
{

    public enum ProjectsCodingBRANCH
    {
        Qt,
        Android,
        Generic
    }

    public enum Branch
    {
        Coding,
        Graphics,
        Film,
        Flexible
    }

    public class DataService
    {
        private readonly ENTITYWRITER _writer;
        private readonly ENTITYREADER _reader;
        private readonly ENTITYHANDLER _handler;
        private readonly Cachemaster _cachemaster;

        private ACCOUNT _account;
        private const string Qt = "Qt Creator";
        private const string Android = "Android Studio Meerkat 2024.3.1";

        public ProjectClass CurrentProject { get; set; }

        // Retrieve Project Names for the Logger Create Page here
        public ProjectsLIST SQLITE_PROJECTS { get; set; } = new();

        // Retrieve Application Names here
        public ApplicationsLIST SQLITE_APPLICATIONS { get; set; } = new();

        public List<SubjectClass> SQLITE_SUBJECTS { get; set; } = new();


        public DataService(ENTITYWRITER writer, ENTITYREADER reader, ENTITYHANDLER handler, Cachemaster cachemaster, AuthService authService)
        {
            _writer = writer;
            _reader = reader;
            _handler = handler;
            _cachemaster = cachemaster;

            _account = authService.Account;

            SignInUser();

        }

        public async void SignInUser()
        {
            if (_account is not null)
            {
                await _writer.SetCurrentUser(_account);
            }
        }

        public async Task SignOutUser()
        {
            if (_account is not null)
                await _writer.UnsetCurrentUser(_account);
        }

        public static bool IsValidEmail(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }

        public async Task<string> UpdateProfilePic(string emailAddress)
        {
            return await _reader.RetrieveProfilePic(emailAddress);
        }

        public ACCOUNT GetUser()
        {
            return _account;
        }

        public string GetAuthorName()
        {
            return _account.FirstName + " " + _account.LastName;
        }

        public Cachemaster GetCachemaster()
        {
            return _cachemaster;
        }

        public string GetDisplayPic()
        {
            return _account.ProfilePic;
        }




        /// <summary>
        /// Retrieves projects from the database.
        /// </summary>
        public async void InitialiseProjectsLISTAsync()
        {
            try
            {
                SQLITE_PROJECTS.Clear();

                var collection = await _reader.ListProjects();

                foreach (ProjectClass pro in collection)
                {
                    if (pro is not null && pro.Name != "Unknown")
                        SQLITE_PROJECTS.Add(pro);
                }
            }
            catch (Exception)
            {
                // TODO
            }

        }



        /// <summary>
        /// Retrieves projects from the database of a specified category.
        /// </summary>
        /// <param name="category">The type of project.</param>
        public async void InitialiseProjectsLISTAsync(LOG.CATEGORY category)
        {
            try
            {
                SQLITE_PROJECTS.Clear();

                var collection = await _reader.ListProjects(category);

                foreach (ProjectClass pro in collection)
                {
                    if (pro is not null && pro.Name != "Unknown")
                        SQLITE_PROJECTS.Add(pro);
                }
            }
            catch (Exception)
            {
                // TODO
            }

        }


        /// <summary>
        /// Retrieves applications from the database.
        /// </summary>
        public async void InitialiseApplicationsLISTAsync()
        {
            try
            {
                SQLITE_APPLICATIONS.Clear();

                var collection = await _reader.ListApplications();

                foreach (ApplicationClass app in collection)
                {
                    if (app is not null)
                        SQLITE_APPLICATIONS.Add(app);
                }



            }
            catch (Exception)
            {
                // TODO
            }


        }

        /// <summary>
        /// Retrieves applications from the database of a specified category.
        /// </summary>
        /// <param name="category">The type of application.</param>
        public async void InitialiseApplicationsLIST(LOG.CATEGORY category)
        {
            try
            {
                SQLITE_APPLICATIONS.Clear();

                var collection = await _reader.ListApplications(category);

                foreach (ApplicationClass app in collection)
                {
                    if (app is not null)
                        SQLITE_APPLICATIONS.Add(app);
                }



            }
            catch (Exception)
            {
                // TODO
            }


        }


        /// <summary>
        /// Retrieves subjects from the database.
        /// </summary>
        /// <param name="category">The type of subjects being retrieved.</param>
        public async void InitialiseSubjectsLIST(LOG.CATEGORY category)
        {
            try
            {
                SQLITE_SUBJECTS.Clear();

                var collection = await _reader.ListSubjects(category);

                foreach (SubjectClass subject in collection)
                {
                    if (subject is not null && subject.Subject != "No Subject")
                        SQLITE_SUBJECTS.Add(subject);
                }
            }
            catch (Exception)
            {
                // TODO
            }

        }

        /// <summary>
        /// Retrieves subjects from the database. Also adds the subjects in DataService's subject list property for easy retrieval in cases where the most recent database update is not needed.
        /// </summary>
        /// <param name="project">Subjects from the specified ProjectClass will be retrieved ONLY.</param>
        public async void InitialiseSubjectsLIST(ProjectClass project)
        {
            try
            {
                SQLITE_SUBJECTS.Clear();

                var collection = await _reader.ListSubjects(project);

                foreach (SubjectClass subject in collection)
                {
                    if (subject is not null && subject.Subject != "No Subject")
                        SQLITE_SUBJECTS.Add(subject);
                }
            }
            catch (Exception)
            {
                // TODO
            }

        }


        /// <summary>
        /// Retrieves subjects from the database. Used every time the subjects are needed as a list immediately.
        /// </summary>
        /// <param name="project">Subjects from the specified ProjectClass will be retrieved ONLY.</param>
        /// <returns>Returns the subjects as a List.</returns>
        public async Task<List<SubjectClass>> ListSubjects(ProjectClass project)
        {
            return await _reader.ListSubjects(project);
        }

        public async Task<List<OutputClass>> ListQtOutputs() => await _reader.ListQtOutputs();

        public async Task<List<OutputClass>> ListASOutputs() => await _reader.ListASOutputs();

        public async Task<List<OutputClass>> ListOutputs(CATEGORY category) => await _reader.ListOutputs(category);


        public async Task<List<TypeClass>> ListQtTypes() => await _reader.ListQtTypes();

        public async Task<List<TypeClass>> ListASTypes() => await _reader.ListASTypes();

        public async Task<List<TypeClass>> ListTypes(CATEGORY category) => await _reader.ListTypes(category);


        public async Task<ApplicationClass?> FindApplicationByID(int appID) => await _reader.FindApplicationByID(appID);

        public async Task<OutputClass> FindOutputByID(int outputID) => await _reader.FindOutputByID(outputID);

        public async Task<TypeClass> FindTypeByID(int typeID) => await _reader.FindTypeByID(typeID);




        #region Log Storage






        public void SaveLog(LOG log, string filePath)
        {
            _cachemaster.SaveLog(log, filePath);
        }

        public async Task<bool> StoreLog(LOG log)
        {
            bool IsStored = false;

            if (await _writer.CreateLOG(log))
            {
                IsStored = true;
                CurrentProject = log.Project;
            }

            return IsStored;
        }






        #endregion




        #region Log Retrieval




        public async Task<List<LOG>> RetrieveLogs()
        {

            return await _reader.RetrieveLOGS();
        }

        public async Task<List<LOG>> RetrieveLogs(LOG.CATEGORY category)
        {
            List<LOG> logs = new();

            foreach (LOG log in await _reader.RetrieveLOGS())
            {
                if (log.Category == category && log.Author == _account)
                    logs.Add(log);
            }

            return logs;
        }

        /// <summary>
        /// Retrieve notes from database. Useful for start up loading. Call again when notes page is selected.
        /// </summary>
        /// <returns></returns>
        public List<NotesLOG> RetrieveNotes()
        {
            return new();
        }

        public async Task<List<LOG>> RetrieveQtLogs()
        {
            List<LOG> logs = new();
            
            foreach(LOG log in await RetrieveLogs(LOG.CATEGORY.CODING))
            {
                if(log.Application.appID == 1)
                    logs.Add(log);
            }

            return logs;
        }


        /// <summary>
        /// Counts the total number of logs.
        /// </summary>
        /// <returns>The count of logs.</returns>
        public async Task<int> LogCount()
        {
            return await _reader.LogCount();
        }

        public async Task<int> LogCount(LOG.CATEGORY category)
        {
            return await _reader.LogCount(category);
        }

        public async Task<int> QtLogCount()
        {
            return await _reader.QtLogCount();
        }

        public async Task<int> ASLogCount()
        {
            return await _reader.ASLogCount();
        }

        public async Task<int> FlexiLogCountAsync()
        {
            return await _reader.FlexiNotesLogCount();
        }





        





        public List<CodingLOG> SearchForQtRecords(string searchBarText, int projectID)
        {
            return _reader.SearchQtLogs(searchBarText, projectID);
        }




        #endregion




        #region Log Management




        #region Updates



        public async Task<bool> UpdateQtLog(LOG lOG) => await _handler.UpdateQtLog(lOG);

        public async Task<bool> UpdateNotesLog(NoteItem noteItem) => await _handler.UpdateNotesLog(noteItem);









        #endregion






        #region Deleteions






        public async Task<bool> DeleteLog(LOG log) => await _handler.DeleteLog(log);













        #endregion






        #endregion


        #region Cache Management




        public void DeleteCacheFile(int id, CacheContext cacheContext)
        {
            _cachemaster.DeleteViewModel(id, cacheContext);
        }






        #endregion
        



        #region Cache Retrieval




        /// <summary>
        /// Retrieves Qt cache. Qt cache are Qt logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The Qt dashboard/owner that is requesting the cache.</param>
        /// <param name="dataService">The service that will interface with the UI.</param>
        /// <returns>An observable collection of QtLOGViewModels</returns>
        public ObservableCollection<QtLOGViewModel> RetrieveQtCache(LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            return _cachemaster.LoadQtViewModels(logCacheViewModel, dataService, _account) ?? new();
        }

        /// <summary>
        /// Retrieves Android Studio cache. Android Studio cache are Android Studio logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The Android Studio dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of AndroidLOGViewModels</returns>
        public ObservableCollection<AndroidLOGViewModel> RetrieveASCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadASViewModels(logCacheViewModel, this, _account) ?? new();
        }

        /// <summary>
        /// Retrieves coding cache. Coding cache are coding logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The coding dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of CodeLOGViewModels</returns>
        public ObservableCollection<CodeLOGViewModel> RetrieveCodeCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadCodeViewModels(logCacheViewModel, this, _account) ?? new();
        }

        /// <summary>
        /// Retrieves graphics cache. Graphics cache are graphics logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The graphics dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of GraphicsLOGViewModels</returns>
        public ObservableCollection<GraphicsLOGViewModel> RetrieveGraphicsCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadGraphicsViewModels(logCacheViewModel, this, _account) ?? new();
        }

        /// <summary>
        /// Retrieves film cache. Film cache are film logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The film dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of FilmLOGViewModels</returns>
        public ObservableCollection<FilmLOGViewModel> RetrieveFilmCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadFilmViewModels(logCacheViewModel, this, _account) ?? new();
        }

        /// <summary>
        /// Retrieves flexible cache. Flexible cache are flexible logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The flexible dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of FlexiLOGViewModels</returns>
        public ObservableCollection<FlexiLOGViewModel> RetrieveFlexibleCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadFlexiViewModels(logCacheViewModel, this, _account) ?? new();
        }

        #endregion







        #region Feedback Management



        public async void CreateFeedback(Exception exception, string methodName, string exceptionType = "Exception")
        {
            await _writer.HandleExceptionAsync(exception, methodName, exceptionType);
        }



        #endregion

    }
}
