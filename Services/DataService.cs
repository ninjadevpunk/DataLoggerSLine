using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
        private readonly DATAWRITER _writer;
        private readonly DATAREADER _reader;
        private readonly DATAHANDLER _handler;
        private readonly Cachemaster _cachemaster;

        private ACCOUNT _account;
        private const string Qt = "Qt Creator";
        private const string Android = "Android Studio Hedgehog 2023.1.1";

        public ProjectClass CurrentProject { get; set; }

        // Retrieve Project Names for the Logger Create Page here
        public ProjectsLIST SQLITE_PROJECTS { get; set; } = new();

        // Retrieve Application Names here
        public ApplicationsLIST SQLITE_APPLICATIONS { get; set; } = new();

        public List<SubjectClass> SQLITE_SUBJECTS { get; set; } = new();

        public DataService(DATAWRITER writer, DATAREADER reader, Cachemaster cachemaster)
        {
            _writer = writer;
            _reader = reader;
            _cachemaster = cachemaster;


        }

        public DataService(DATAWRITER writer, DATAREADER reader, Cachemaster cachemaster, AuthService authService)
        {
            _writer = writer;
            _reader = reader;
            _cachemaster = cachemaster;

            _account = authService.Account;
            SignInUser();

        }

        public DataService(DATAWRITER writer, DATAREADER reader, DATAHANDLER handler, Cachemaster cachemaster, AuthService authService)
        {
            _writer = writer;
            _reader = reader;
            _handler = handler;
            _cachemaster = cachemaster;

            _account = authService.Account;
            SignInUser();

        }

        public void SignInUser()
        {
            if (_account is not null)
            {
                _writer.SetCurrentUser(_account);
                _reader.SetCurrentUser(_account);
            }
        }

        public void SignOutUser()
        {
            if (_account is not null)
                _writer.UnsetCurrentUser(_account);
        }

        public static bool IsValidEmail(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }

        public string UpdateProfilePic(string emailAddress)
        {
            return _reader.UpdateProfilePic(emailAddress);
        }

        public void UpdateWatcher(int CachedItems, int CachedPostIts)
        {
            _writer.Watcher.UpdateOnStart(CachedItems, CachedPostIts);
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

        public void SetAccount(ACCOUNT account)
        {
            _account = account;
        }




        /// <summary>
        /// Retrieves projects from the database.
        /// </summary>
        public void InitialiseProjectsLIST()
        {
            try
            {
                SQLITE_PROJECTS.Clear();

                var collection = _reader.ListProjects();

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
        public void InitialiseProjectsLIST(LOG.CATEGORY category)
        {
            try
            {
                SQLITE_PROJECTS.Clear();

                var collection = _reader.ListProjects(category);

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

        public void InitialiseApplicationsLIST()
        {
            try
            {
                SQLITE_APPLICATIONS.Clear();

                var collection = _reader.ListApplications();

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

        public void InitialiseApplicationsLIST(LOG.CATEGORY category)
        {
            try
            {
                SQLITE_APPLICATIONS.Clear();

                var collection = _reader.ListApplications(category);

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


        public void InitialiseSubjectsLIST(LOG.CATEGORY category)
        {
            try
            {
                SQLITE_SUBJECTS.Clear();

                var collection = _reader.ListSubjects(category);

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

        public void InitialiseSubjectsLIST(ProjectClass project)
        {
            try
            {
                SQLITE_SUBJECTS.Clear();

                var collection = _reader.ListSubjects(project);

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


        #region ID Generation




        public int CreateAccountID()
        {
            return _writer.CreateAccountID();
        }

        public int CreateLogID()
        {
            return _writer.CreateLogID();
        }

        public int CreateProjectID(ACCOUNT account, ApplicationClass application, string projectName)
        {
            return _writer.CreateProjectID(account, application, projectName);
        }

        public ProjectClass RetrieveProject(int projectID)
        {
            return _reader.FindProjectByID(projectID);
        }

        public int CreateAppID(LOG.CATEGORY category, ACCOUNT account, string applicationName)
        {
            return _writer.CreateAppID(category, account, applicationName);
        }

        public int CreatePostItID(List<int>? unUsedIDs)
        {
            return _writer.CreatePostItID(unUsedIDs);
        }

        public List<int>? RetrievePostItIndex()
        {
            return _cachemaster.RetrievePostItIndex();
        }

        public void SavePostItIndex()
        {
            _cachemaster.SavePostItIndex(_writer.Watcher.AvailablePostItIDs);
        }

        public void UpdateAvailablePostItIDs(List<int>? unUsedIDs)
        {
            try
            {
                if (_writer.Watcher.AvailablePostItIDs is not null)
                {
                    _writer.Watcher.AvailablePostItIDs.AddRange(unUsedIDs);
                }
                else
                {
                    _writer.Watcher.AvailablePostItIDs = unUsedIDs;
                }
            }
            catch(ArgumentNullException nullx)
            {
                Debug.WriteLine($"Null exception found near UpdateAvailablePostItIDs(unusedIDs): {nullx.Message}");
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Exception found near UpdateAvailablePostItIDs(unusedIDs): {ex.Message}");

                // TODO


            }
        }

        public int CreateSubjectID(ProjectClass project, string subjectName, List<int>? unUsedIDs)
        {
            return _writer.CreateSubjectID(project, subjectName, unUsedIDs);
        }

        public List<int>? RetrieveSubjectIndex()
        {
            return _cachemaster.RetrieveSubjectIndex();
        }

        public void SaveSubjectIndex()
        {
            _cachemaster.SaveSubjectIndex(_writer.Watcher.AvailableSubjectIDs);
        }

        public void UpdateAvailableSubjectIDs(List<int>? unUsedIDs)
        {
            try
            {
                if (_writer.Watcher.AvailableSubjectIDs is not null)
                {
                    _writer.Watcher.AvailableSubjectIDs.AddRange(unUsedIDs);
                }
                else
                {
                    _writer.Watcher.AvailableSubjectIDs = unUsedIDs;
                }
            }
            catch (ArgumentNullException nullx)
            {
                Debug.WriteLine($"Null exception found near UpdateAvailableSubjectIDs(unusedIDs): {nullx.Message}");
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Exception found near UpdateAvailableSubjectIDs(unusedIDs): {ex.Message}");

                // TODO


            }
        }

        public int CreateOutputID(ACCOUNT account, ApplicationClass applicationName, string outputName)
        {
            return _writer.CreateOutputID(account, applicationName, outputName);
        }

        public int CreateTypeID(ACCOUNT account, ApplicationClass applicationName, string typeName)
        {
            return _writer.CreateTypeID(account, applicationName, typeName);
        }

        public ACCOUNT FindAccountByID(int accountID)
        {
            return _writer.FindAccountByID(accountID);
        }

        public ApplicationClass? FindApplicationByID(int id)
        {
            return _reader.FindAppByID(id);
        }

        #endregion




        #region Log Storage






        public void SaveLog(LOG log, string filePath)
        {
            _cachemaster.SaveLog(log, filePath);
        }

        public bool StoreLog(LOG log)
        {
            bool IsStored = false;
            var initialCount = _reader.LogCount();

            if (_writer.CreateLOG(log))
            {
                IsStored = true;
                CurrentProject = log.Project;
            }

            return IsStored;
        }






        #endregion




        #region Log Retrieval




        public List<LOG> RetrieveLogs()
        {
            _reader.RetrieveLOGS();

            return _reader;
        }

        public List<LOG> RetrieveLogs(LOG.CATEGORY category)
        {
            List<LOG> logs = new();
            _reader.RetrieveLOGS();

            foreach (LOG log in _reader)
            {
                if (log.Category == category)
                    logs.Add(log);
            }

            return logs;
        }

        public List<LOG> RetrieveQtLogs()
        {
            List<LOG> logs = new();
            
            foreach(LOG log in RetrieveLogs(LOG.CATEGORY.CODING))
            {
                if(log.Application.AppID == 1)
                    logs.Add(log);
            }

            return logs;
        }


        /// <summary>
        /// Counts the total number of logs.
        /// </summary>
        /// <returns>The count of logs.</returns>
        public int LogCount()
        {
            return _reader.LogCount();
        }

        public int LogCount(LOG.CATEGORY category)
        {
            return _reader.LogCount(category);
        }

        public int QtLogCount()
        {
            return _reader.QtLogCount();
        }

        public int ASLogCount()
        {
            return _reader.ASLogCount();
        }

        public int FlexiLogCount()
        {
            return _reader.FlexiLogCount();
        }

        public List<SubjectClass> ListSubjects(ProjectClass project)
        {
            return _reader.ListSubjects(project);
        }





        public List<CodingLOG> SearchForQtRecords(string searchBarText, int projectID)
        {
            return _reader.SearchQtLogs(searchBarText, projectID);
        }




        #endregion




        #region Log Management




        #region Updates













        #endregion






        #region Deleteions





        public bool DeleteLog(int logID)
        {
            return _handler.DeleteLog(logID);
        }













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
            return _cachemaster.LoadQtViewModels(logCacheViewModel, dataService, _account);
        }

        /// <summary>
        /// Retrieves Android Studio cache. Android Studio cache are Android Studio logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The Android Studio dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of AndroidLOGViewModels</returns>
        public ObservableCollection<AndroidLOGViewModel> RetrieveASCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadASViewModels(logCacheViewModel, this, _account);
        }

        /// <summary>
        /// Retrieves coding cache. Coding cache are coding logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The coding dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of CodeLOGViewModels</returns>
        public ObservableCollection<CodeLOGViewModel> RetrieveCodeCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadCodeViewModels(logCacheViewModel, this, _account);
        }

        /// <summary>
        /// Retrieves graphics cache. Graphics cache are graphics logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The graphics dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of GraphicsLOGViewModels</returns>
        public ObservableCollection<GraphicsLOGViewModel> RetrieveGraphicsCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadGraphicsViewModels(logCacheViewModel, this, _account);
        }

        /// <summary>
        /// Retrieves film cache. Film cache are film logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The film dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of FilmLOGViewModels</returns>
        public ObservableCollection<FilmLOGViewModel> RetrieveFilmCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadFilmViewModels(logCacheViewModel, this, _account);
        }

        /// <summary>
        /// Retrieves flexible cache. Flexible cache are flexible logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The flexible dashboard/owner that is requesting the cache.</param>
        /// <returns>An observable collection of FlexiLOGViewModels</returns>
        public ObservableCollection<FlexiLOGViewModel> RetrieveFlexibleCache(LogCacheViewModel logCacheViewModel)
        {
            return _cachemaster.LoadFlexiViewModels(logCacheViewModel, this, _account);
        }

        #endregion

    }
}
