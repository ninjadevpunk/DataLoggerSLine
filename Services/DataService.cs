using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.LogViewModels;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Data_Logger_1._3.Services
{

    public enum ProjectsCodingBRANCH {
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
        private readonly Cachemaster _cachemaster;

        private ACCOUNT _account;
        private const string Qt = "Qt Creator";
        private const string Android = "Android Studio Hedgehog 2023.1.1";
        private bool IsStartup = true;

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
            if(_account is not null)
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

        /* Use this to retrieve projects from database.
         * Use the Category to filter out projects for a specific category in the project object.
         * */

        public void InitialiseProjectsLIST()
        {
            try
            {
                SQLITE_PROJECTS.Clear();

                var collection = _writer.ListProjects();

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

        public void InitialiseProjectsLIST(LOG.CATEGORY category)
        {
            try
            {
                SQLITE_PROJECTS.Clear();

                var collection = _writer.ListProjects(category);

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

                var collection = _writer.ListApplications();

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

                var collection = _writer.ListSubjects(category);

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

                var collection = _writer.ListSubjects(project);

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
            return _writer.FindProjectByID(projectID);
        }

        public int CreateAppID(LOG.CATEGORY category, ACCOUNT account, string applicationName)
        {
            return _writer.CreateAppID(category, account, applicationName);
        }

        public int CreatePostItID(int PostItLiseSize)
        {
            return _writer.CreatePostItID(PostItLiseSize);
        }

        public int CreateSubjectID(ProjectClass project, string subjectName, int PostItListSize)
        {
            return _writer.CreateSubjectID(project, subjectName, PostItListSize);
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
            return _writer.FindAppByID(id);
        }

        #endregion




        #region Log Storage



        public bool StoreLog(LOG log)
        {
            bool IsStored = false;
            var initialCount = _writer.LogCount();

            _writer.CreateLOG(log);

            if(_writer.LogCount() > initialCount)
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

            foreach(LOG log in _reader)
            {
                if(log.Category == category)
                    logs.Add(log);
            }

            return logs;
        }


        public int LogCount(LOG.CATEGORY category)
        {
            return _writer.LogCount(category);
        }

        public int QtLogCount()
        {
            return _writer.QtLogCount();
        }

        public int ASLogCount()
        {
            return _writer.ASLogCount();
        }





        #endregion


        #region Cache Management




        public void DeleteQtCacheFile(int id)
        {
            _cachemaster.DeleteQtViewModel(id);
        }









        #endregion


        #region Cache Retrieval





        public ObservableCollection<QtLOGViewModel> RetrieveQtCache(LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            return _cachemaster.LoadQtViewModels(logCacheViewModel, dataService);
        }




        #endregion

    }
}
