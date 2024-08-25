using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;


namespace Data_Logger_1._3.Services
{
    public class Cachemaster
    {
        /** DOCUMENTATION
         * 
         * Please use this class for temporary and permanent file storage.
         * For database support, use DATAMASTER classes.
         * 
         * File Handler Class
         * Use this class to create files to permanently keep contents of unstored LOGS.
         * When a LOG is removed from DATAMASTER, this class will remove the file associated
         * with it in 20 minutes.
         */
        const string IDENTIFIERS_PATH = @"C:\Data Logger Central\Depository\res\_identifiers.index";
        const string SUBJECT_IDS_PATH = @"C:\Data Logger Central\Depository\res\subject.index";
        const string POSTIT_IDS_PATH = @"C:\Data Logger Central\Depository\res\postit.index";
        const string RESOURCE_DIRECTORY = @"C:\Data Logger Central\Depository\res";

        public List<int>? Identifiers { get; set; } = new();
        public StreamWriter Writer { get; set; }

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public Cachemaster()
        {
            if (ResourcesCreated())
            {
                IdentifiersChecked();
            }
        }

        /// <summary>
        /// Creates vital resources for the Cachemaster so the class can function correctly.
        /// </summary>
        /// <returns>Returns whether or not the resources were successfully created if they had not existed.</returns>
        public bool ResourcesCreated()
        {
            /** Make sure the file with Identifiers exists.
             * If it doesn't exist, create it.
             */
            try
            {
                if (!Directory.Exists(RESOURCE_DIRECTORY))
                {

                    DirectoryInfo directory = Directory.CreateDirectory(RESOURCE_DIRECTORY);
                    directory.Create();


                    var fileStream = new FileStream(IDENTIFIERS_PATH, FileMode.CreateNew);
                    fileStream.Close();

                    fileStream = new(SUBJECT_IDS_PATH, FileMode.CreateNew);
                    fileStream.Close();

                    fileStream = new(POSTIT_IDS_PATH, FileMode.CreateNew);
                    fileStream.Close();

                }


            }
            catch (Exception)
            {
                Identifiers = null;
                return false;
            }

            return true;

        }

        /// <summary>
        /// Only checks if the Identifiers file exists and creates it if it doesn't. Will then insert found identifiers into the Identifiers list.
        /// </summary>
        /// <returns>Returns true if the file was successfully created in the event that the file doesn't already exist or true because all identifiers found were successfully added.
        /// </returns>
        public bool IdentifiersChecked()
        {
            if (Identifiers is not null)
                Identifiers.Clear();
            else
                return false;

            try
            {
                if (!File.Exists(IDENTIFIERS_PATH))
                {
                    var f = new FileStream(IDENTIFIERS_PATH, FileMode.CreateNew);
                    f.Close();

                }
                else
                {

                    foreach (string s in File.ReadLines(IDENTIFIERS_PATH))
                    {
                        Identifiers.Add(int.Parse(s));
                    }

                }


            }
            catch (Exception)
            {
                Identifiers = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the Identifiers file exists and adds a new log's ID into the list. P.S. Will call IdentifiersChecked() automatically.
        /// </summary>
        /// <param name="log">The log that will be cached.</param>
        /// <returns>The log's ID if the identifier was successfully inserted.</returns>
        public int IdentifiersChecked(LOG log)
        {
            if (IdentifiersChecked() && Identifiers is not null)
            {
                try
                {
                    int id = log.ID;

                    /** If the id doesn't exist, add it to the Identifiers List and write the new id to the
                     * Identifiers.index file.
                     */

                    if (!Identifiers.Contains(id))
                    {
                        Identifiers.Add(id);

                        Writer = new StreamWriter(IDENTIFIERS_PATH);
                        foreach (int t in Identifiers.Order())
                        {
                            Writer.WriteLine(t);
                        }

                    }

                    return id;
                }
                catch (Exception)
                {
                    Identifiers = null;
                }
            }

            return -1;

        }

        public void IdentifiersChecked(int id)
        {
            if (IdentifiersChecked() && Identifiers is not null)
            {
                try
                {

                    /** If the id doesn't exist, add it to the Identifiers List and write the new id to the
                     * Identifiers.index file.
                     */

                    if (!Identifiers.Contains(id))
                    {
                        Identifiers.Add(id);

                        Writer = new StreamWriter(IDENTIFIERS_PATH);
                        foreach (int t in Identifiers.Order())
                        {
                            Writer.WriteLine(t);
                        }

                        Writer.Close();

                    }

                }
                catch (Exception)
                {
                    Identifiers = null;
                }
            }


        }

        /// <summary>
        /// Retrieves the subjectIDs that were stored by SaveSubjectIndex. To be used on application startup by the DataService for the IDWatcher's 
        /// AvailableSubjectIDs.
        /// </summary>
        /// <returns>An integer list of stored subjectIDs.</returns>
        public List<int>? RetrieveSubjectIndex()
        {
            try
            {
                if (!File.Exists(SUBJECT_IDS_PATH))
                {
                    var f = new FileStream(SUBJECT_IDS_PATH, FileMode.CreateNew);
                    f.Close();

                }
                else
                {
                    List<int>? oldSubjectIDs = new();

                    foreach (string s in File.ReadLines(SUBJECT_IDS_PATH))
                    {
                        oldSubjectIDs.Add(int.Parse(s));
                    }

                    return oldSubjectIDs;

                }


            }
            catch (Exception)
            {
                // TODO
            }

            return null;

        }

        /// <summary>
        /// Saves unused subjectIDs. To be used by IDWatcher's AvailableSubjectIDs when the app closes and after a log has been stored.
        /// </summary>
        /// <param name="idsToCache">A list of subjectIDs that aren't used which can be given to new subjects. This method will store them.</param>
        public void SaveSubjectIndex(List<int>? idsToCache)
        {
            try
            {
                if (idsToCache is not null)
                {
                    File.Delete(SUBJECT_IDS_PATH);
                    var fileStream = new FileStream(SUBJECT_IDS_PATH, FileMode.CreateNew);
                    fileStream.Close();

                    Writer = new StreamWriter(SUBJECT_IDS_PATH);

                    foreach (int id in idsToCache.Order())
                    {
                        Writer.WriteLine(id);
                    }

                    Writer.Close();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Retrieves the PostItIDs that were stored by SavePostItIndex.  To be used on application startup by the DataService for the IDWatcher's 
        /// AvailablePostItIDs.
        /// </summary>
        /// <returns></returns>
        public List<int>? RetrievePostItIndex()
        {
            try
            {
                if (!File.Exists(POSTIT_IDS_PATH))
                {
                    var f = new FileStream(POSTIT_IDS_PATH, FileMode.CreateNew);
                    f.Close();

                }
                else
                {
                    List<int>? oldSubjectIDs = new();

                    foreach (string s in File.ReadLines(POSTIT_IDS_PATH))
                    {
                        oldSubjectIDs.Add(int.Parse(s));
                    }

                    return oldSubjectIDs;
                }


            }
            catch (Exception)
            {
                // TODO
            }

            return null;
        }

        /// <summary>
        /// Saves unused PostItIDs. To be used by IDWatcher's AvailablePostItIDs when the app closes and after a log has been stored.
        /// </summary>
        /// <param name="idsToCache">A list of PostItIDs that aren't used which can be given to new post its. This method will store them.</param>
        public void SavePostItIndex(List<int>? idsToCache)
        {
            try
            {
                if (idsToCache is not null)
                {
                    File.Delete(POSTIT_IDS_PATH);
                    var fileStream = new FileStream(POSTIT_IDS_PATH, FileMode.CreateNew);
                    fileStream.Close();

                    Writer = new StreamWriter(POSTIT_IDS_PATH);

                    foreach (int id in idsToCache.Order())
                    {
                        Writer.WriteLine(id);
                    }

                    Writer.Close();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }










        /// <summary>
        /// Saves a log on the logger.
        /// </summary>
        /// <param name="log">The log that is top be saved.</param>
        /// <param name="filePath">The location to store the log.</param>
        /// <param name="cacheContext">The type of log being saved.</param>
        public void SaveLog(LOG log, string filePath)
        {

            using var fileStream = new FileStream(filePath, FileMode.Create);


            switch (log.Category)
            {
                case LOG.CATEGORY.CODING:
                    {
                        var codingLog = (CodingLOG)log;
                        string json = JsonConvert.SerializeObject(codingLog, settings);

                        File.WriteAllText(filePath, json);
                        break;
                    }
                case LOG.CATEGORY.GRAPHICS:
                    {
                        var graphicsLog = (GraphicsLOG)log;
                        string json = JsonConvert.SerializeObject(graphicsLog, settings);

                        File.WriteAllText(filePath, json);
                        break;
                    }
                case LOG.CATEGORY.FILM:
                    {
                        var filmLog = (FilmLOG)log;
                        string json = JsonConvert.SerializeObject(filmLog, settings);

                        File.WriteAllText(filePath, json);
                        break;
                    }
                case LOG.CATEGORY.NOTES:
                    {
                        var flexiLog = (FlexiNotesLOG)log;
                        string json = JsonConvert.SerializeObject(flexiLog, settings);

                        File.WriteAllText(filePath, json);
                        break;
                    }
            }
        }

        // TODO
        public LoggerCreateViewModel LoadLog(LOG log, string filePath, CacheContext cacheContext)
        {

            using var fileStream = new FileStream(filePath, FileMode.Create);


            switch (log.Category)
            {
                case LOG.CATEGORY.CODING:
                    {

                        break;
                    }
                case LOG.CATEGORY.GRAPHICS:
                    {

                        break;
                    }
                case LOG.CATEGORY.FILM:
                    {

                        break;
                    }
                case LOG.CATEGORY.NOTES:
                    {

                        break;
                    }
            }

            return null;
        }



        public string LogExtension(CacheContext context)
        {
            switch (context)
            {
                case CacheContext.Qt:
                    return "qtcache";
                case CacheContext.AndroidStudio:
                    return "ascache";
                case CacheContext.Coding:
                    return "cdecache";
                case CacheContext.Graphics:
                    return "gpscache";
                case CacheContext.Film:
                    return "flmcache";
                case CacheContext.Flexi:
                    return "flxcache";
                default:
                    return string.Empty;
            }
        }

        private void SaveViewModel(int logID, string json, CacheContext cacheContext)
        {
            try
            {
                string filePath = $@"{RESOURCE_DIRECTORY}\{logID}.{LogExtension(cacheContext)}";

                File.WriteAllText(filePath, json);
            }
            catch (IOException ioX)
            {
                Debug.WriteLine($"IOException found near SaveViewModel(logID,json...: {ioX.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near SaveViewModel(logID,json...: {e.Message}");

                // TODO: Handle exception

            }
        }


        public void DeleteViewModel(int logID, CacheContext cacheContext)
        {
            if (Identifiers is null || !Identifiers.Contains(logID))
                return;

            try
            {
                var filePath = $@"{RESOURCE_DIRECTORY}\{logID}.{LogExtension(cacheContext)}";

                if (File.Exists(filePath))
                    File.Delete(filePath);

                // Remove logID from IDENTIFIERS
                Identifiers.Remove(logID);

                if (File.Exists(IDENTIFIERS_PATH))
                    File.Delete(IDENTIFIERS_PATH);

                // Create new IDENTIFIERS file with updated list
                using var fileStream = new FileStream(IDENTIFIERS_PATH, FileMode.Create);
                fileStream.Close();

                // Write IDENTIFIERS to new file
                using var writer = new StreamWriter(IDENTIFIERS_PATH);
                foreach (int id in Identifiers.Order())
                {
                    writer.WriteLine(id);
                }
                writer.Close();
            }
            catch (IOException ioX)
            {
                Debug.WriteLine($"IOException found near DeleteViewModel(logID, cacheContext): {ioX.Message}");
            }
            catch (Exception ex)
            {
                // Handle exception
                Debug.WriteLine($"Exception found near DeleteViewModel(logID, cacheContext): {ex.Message}");
            }
        }




        #region Qt




        /// <summary>
        /// Saves a temporarily logged Qt log after its been annotated.
        /// </summary>
        /// <param name="qtViewModel">The qt interface that is being logged. 
        /// The method will extract the relevant info.</param>
        /// <param name="cacheContext">The type of log being cached.</param>
        public void SaveQtViewModel(QtLOGViewModel qtViewModel, CacheContext cacheContext)
        {
            int logID = qtViewModel.ViewModelID;

            try
            {
                string json = JsonConvert.SerializeObject(qtViewModel._QtcodingLOG, settings);

                SaveViewModel(logID, json, cacheContext);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        /// <summary>
        /// Retrieve's Qt cache. Qt cache are Qt logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The Qt dashboard/owner that is requesting the cache.</param>
        /// <param name="dataService">The service that will interface with the UI.</param>
        /// <param name="account">The author of the logs.</param>
        /// <returns>A list of QtLOGViewModels.</returns>
        public ObservableCollection<QtLOGViewModel>? LoadQtViewModels(LogCacheViewModel logCacheViewModel, DataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "qtcache";
                    string root = $@"{RESOURCE_DIRECTORY}\";
                    ObservableCollection<QtLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (int id in Identifiers)
                    {
                        var filePath = $@"{root}{id}.{extenstion}";

                        if (File.Exists(filePath) && list is not null)
                        {
                            var json = File.ReadAllText(filePath);

                            var log = JsonConvert.DeserializeObject<CodingLOG>(json, settings);
                            var viewModel = new QtLOGViewModel(log, logCacheViewModel, dataService);

                            if (viewModel is not null && viewModel._QtcodingLOG.Author.Equals(account))
                                list.Add(viewModel);
                        }
                    }

                    return list;


                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception found near LoadQtViewModels(...): {ex.Message}");
                    // TODO

                }
            }

            return null;
        }





        #endregion








        #region Android Studio



        /// <summary>
        /// Saves a temporarily logged Androi Studio log after its been annotated.
        /// </summary>
        /// <param name="asViewModel">The Androi Studio interface that is being logged. 
        /// The method will extract the relevant info.</param>
        /// <param name="cacheContext">The type of log being cached.</param>
        public void SaveASViewModel(AndroidLOGViewModel asViewModel, CacheContext cacheContext)
        {
            int logID = asViewModel.ViewModelID;

            try
            {
                string json = JsonConvert.SerializeObject(asViewModel._AndroidCodingLOG, settings);

                SaveViewModel(logID, json, cacheContext);
            }
            catch (Exception)
            {
                // TODO
            }
        }

        /// <summary>
        /// Retrieve's Android Studio cache. Android Studio cache are Android Studio logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The Android Studio dashboard/owner that is requesting the cache.</param>
        /// <param name="dataService">The service that will interface with the UI.</param>
        /// <returns></returns>
        public ObservableCollection<AndroidLOGViewModel>? LoadASViewModels(LogCacheViewModel logCacheViewModel, DataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "ascache";
                    string root = $@"{RESOURCE_DIRECTORY}\";
                    ObservableCollection<AndroidLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (int id in Identifiers)
                    {
                        var filePath = $@"{root}{id}.{extenstion}";

                        if (File.Exists(filePath) && list is not null)
                        {
                            var json = File.ReadAllText(filePath);

                            var log = JsonConvert.DeserializeObject<AndroidCodingLOG>(json, settings);
                            var viewModel = new AndroidLOGViewModel(log, logCacheViewModel, dataService);

                            if (viewModel is not null && viewModel._AndroidCodingLOG.Author.Equals(account))
                                list.Add(viewModel);
                        }
                    }

                    return list;


                }
                catch (Exception)
                {
                    // TODO
                }
            }

            return null;
        }



        #endregion




        #region Coding





        /// <summary>
        /// Saves a temporarily logged coding log after its been annotated.
        /// </summary>
        /// <param name="codeLOGViewModel">The coding interface that is being logged. 
        /// The method will extract the relevant info.</param>
        /// <param name="cacheContext">The type of log being cached.</param>
        public void SaveCodeViewModel(CodeLOGViewModel codeLOGViewModel, CacheContext cacheContext)
        {
            int logID = codeLOGViewModel._CodeLOG.ID;

            try
            {
                string json = JsonConvert.SerializeObject(codeLOGViewModel._CodeLOG, settings);

                SaveViewModel(logID, json, cacheContext);
            }
            catch (Exception)
            {
                // TODO
            }
        }


        /// <summary>
        /// Retrieve's Qt cache. Qt cache are Qt logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The coding dashboard/owner that is requesting the cache.</param>
        /// <param name="dataService">The service that will interface with the UI.</param>
        /// <param name="account">The author of the logs.</param>
        /// <returns>A list of CodeLOGViewModels.</returns>
        public ObservableCollection<CodeLOGViewModel>? LoadCodeViewModels(LogCacheViewModel logCacheViewModel, DataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "cdecache";
                    string root = $@"{RESOURCE_DIRECTORY}\";
                    ObservableCollection<CodeLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (int id in Identifiers)
                    {
                        var filePath = $@"{root}{id}.{extenstion}";

                        if (File.Exists(filePath) && list is not null)
                        {
                            var json = File.ReadAllText(filePath);

                            var log = JsonConvert.DeserializeObject<CodingLOG>(json, settings);
                            var viewModel = new CodeLOGViewModel(log, logCacheViewModel, dataService);

                            if (viewModel is not null && viewModel._CodeLOG.Author.Equals(account))
                                list.Add(viewModel);
                        }
                    }

                    return list;


                }
                catch (Exception)
                {
                    // TODO
                }
            }

            return null;
        }





        #endregion






        #region Graphics



        /// <summary>
        /// Saves a temporarily logged graphics log after its been annotated.
        /// </summary>
        /// <param name="graphicsLOGViewModel">The graphics interface that is being logged. 
        /// The method will extract the relevant info.</param>
        /// <param name="cacheContext">The type of log being cached.</param>
        public void SaveGraphicsViewModel(GraphicsLOGViewModel graphicsLOGViewModel, CacheContext cacheContext)
        {
            int logID = graphicsLOGViewModel._GraphicsLOG.ID;

            try
            {
                string json = JsonConvert.SerializeObject(graphicsLOGViewModel._GraphicsLOG, settings);

                SaveViewModel(logID, json, cacheContext);
            }
            catch (Exception)
            {
                // TODO
            }
        }


        /// <summary>
        /// Retrieve's graphics cache. Graphics cache are graphics logs that haven't been stored.
        /// </summary>
        /// <param name="logCacheViewModel">The graphics dashboard/owner that is requesting the cache.</param>
        /// <param name="dataService">The service that will interface with the UI.</param>
        /// <param name="account"></param>
        /// <returns>A list of GraphicsLOGViewModels.</returns>
        public ObservableCollection<GraphicsLOGViewModel>? LoadGraphicsViewModels(LogCacheViewModel logCacheViewModel, DataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "gpscache";
                    string root = $@"{RESOURCE_DIRECTORY}\";
                    ObservableCollection<GraphicsLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (int id in Identifiers)
                    {
                        var filePath = $@"{root}{id}.{extenstion}";

                        if (File.Exists(filePath) && list is not null)
                        {
                            var json = File.ReadAllText(filePath);

                            var log = JsonConvert.DeserializeObject<GraphicsLOG>(json, settings);
                            var viewModel = new GraphicsLOGViewModel(log, logCacheViewModel, dataService);

                            if (viewModel is not null && viewModel._GraphicsLOG.Author.Equals(account))
                                list.Add(viewModel);
                        }
                    }

                    return list;


                }
                catch (Exception)
                {
                    // TODO
                }
            }

            return null;
        }






        #endregion







        #region Film




        public void SaveFilmViewModel(FilmLOGViewModel filmLOGViewModel, CacheContext cacheContext)
        {
            int logID = filmLOGViewModel._FilmLOG.ID;

            try
            {
                string json = JsonConvert.SerializeObject(filmLOGViewModel._FilmLOG, settings);

                SaveViewModel(logID, json, cacheContext);
            }
            catch (Exception)
            {
                // TODO
            }
        }



        public ObservableCollection<FilmLOGViewModel>? LoadFilmViewModels(LogCacheViewModel logCacheViewModel, DataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "flmcache";
                    string root = $@"{RESOURCE_DIRECTORY}\";
                    ObservableCollection<FilmLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (int id in Identifiers)
                    {
                        var filePath = $@"{root}{id}.{extenstion}";

                        if (File.Exists(filePath) && list is not null)
                        {
                            var json = File.ReadAllText(filePath);

                            var log = JsonConvert.DeserializeObject<FilmLOG>(json, settings);
                            var viewModel = new FilmLOGViewModel(log, logCacheViewModel, dataService);

                            if (viewModel is not null && viewModel._FilmLOG.Author.Equals(account))
                                list.Add(viewModel);
                        }
                    }

                    return list;


                }
                catch (Exception)
                {
                    // TODO
                }
            }

            return null;
        }






        #endregion






        #region Flexi Notes




        public void SaveFlexiViewModel(FlexiLOGViewModel flexiLOGViewModel, CacheContext cacheContext)
        {
            int logID = flexiLOGViewModel.ViewModelID;

            try
            {
                string json = JsonConvert.SerializeObject(flexiLOGViewModel._FlexiLOG, settings);

                SaveViewModel(logID, json, cacheContext);
            }
            catch (Exception)
            {
                // TODO
            }
        }



        public ObservableCollection<FlexiLOGViewModel>? LoadFlexiViewModels(LogCacheViewModel logCacheViewModel, DataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "flxcache";
                    string root = $@"{RESOURCE_DIRECTORY}\";
                    ObservableCollection<FlexiLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (int id in Identifiers)
                    {
                        var filePath = $@"{root}{id}.{extenstion}";

                        if (File.Exists(filePath) && list is not null)
                        {
                            var json = File.ReadAllText(filePath);

                            var log = JsonConvert.DeserializeObject<FlexiNotesLOG>(json, settings);
                            var viewModel = new FlexiLOGViewModel(log, logCacheViewModel, dataService);

                            if (viewModel is not null && log.Author.Equals(account))
                                list.Add(viewModel);
                        }
                    }

                    return list;


                }
                catch (Exception)
                {
                    // TODO
                }
            }

            return null;
        }






        #endregion


    }
}
