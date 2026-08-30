using Core.Models;
using FileSys.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;

namespace FileSys.Services
{
    /// <summary>
    /// Class for temporary and permanent file storage. Related to the ENTITYMASTER class. This class creates files to permanently keep contents of LOGS 
    /// for backup purposes.
    /// </summary>
    public class CacheMaster : ICacheService
    {
        public enum CacheContext
        {
            Qt,
            AndroidStudio,
            Coding,
            Graphics,
            Film,
            NOTES,
            Flexi
        }



        public List<string>? Identifiers { get; set; } = new();
        public StreamWriter? Writer { get; set; }

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public readonly string MainFolder;
        public readonly string DepositoryPath;
        public readonly string ProgramDataPath;

        public readonly string ResourceDirectory;
        public readonly string IdentifiersPath;
        public readonly string SubjectIdsPath;
        public readonly string PostitIdsPath;

        public CacheMaster()
        {
#if DEBUG
            string configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.DevMode.json");
#else
            string configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
#endif

            if (!File.Exists(configPath))
                throw new FileNotFoundException("Configuration file not found.", configPath);

            JObject config = JObject.Parse(File.ReadAllText(configPath));

            MainFolder = config["Paths"]?["Root"]?.ToString() ?? throw new InvalidOperationException("Paths:Root is missing from configuration.");
            ProgramDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Data Logger");

            DepositoryPath = Path.Combine(MainFolder, "Depository");

            ResourceDirectory = Path.Combine(DepositoryPath, "res");

            IdentifiersPath = Path.Combine(ResourceDirectory, "_identifiers.index");
            SubjectIdsPath = Path.Combine(ResourceDirectory, "subject.index");
            PostitIdsPath = Path.Combine(ResourceDirectory, "postit.index");

            if (ResourcesCreated())
            {
                IdentifiersChecked();
            }
        }



        public bool CreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in CreateDirectory(): {ex.Message}");
                return false;
            }
        }

        

        public string? ReadAllText(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in ReadAllText(): {ex.Message}");
                return null;
            }
        }

        public bool WriteAllText(string path, string contents)
        {
            try
            {
                File.WriteAllText(path, contents);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in WriteAllText(): {ex.Message}");
                return false;
            }
        }

        public bool DeleteDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    return true;

                Directory.Delete(path, recursive: true);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in DeleteDirectory(): {ex.Message}");
                return false;
            }
        }



        /// <summary>
        /// Creates vital resources for the CacheMaster so the class can function correctly.
        /// </summary>
        /// <returns>Returns whether or not the resources were successfully created if they had not existed.</returns>
        public bool ResourcesCreated()
        {
            /** Make sure the file with Identifiers exists.
             * If it doesn't exist, create it.
             */
            try
            {

                // Base Folder
                if (!Directory.Exists(MainFolder))
                {
                    Directory.CreateDirectory(MainFolder);
                    GrantFolderPermissions(MainFolder);
                }

                // Depository
                if (!Directory.Exists(DepositoryPath))
                {
                    Directory.CreateDirectory(DepositoryPath);

                    // Hide Depository
                    DirectoryInfo depositoryInfo = new DirectoryInfo(DepositoryPath);
                    depositoryInfo.Attributes |= FileAttributes.Hidden;
                }

                // Respurces Folder
                if (!Directory.Exists(ResourceDirectory))
                {
                    Directory.CreateDirectory(ResourceDirectory);

                    // Main Index File
                    using (var fileStream = new FileStream(IdentifiersPath, FileMode.CreateNew)) { }

                    // Subject ID Index
                    using (var fileStream = new FileStream(SubjectIdsPath, FileMode.CreateNew)) { }

                    // PostIt ID Index
                    using (var fileStream = new FileStream(PostitIdsPath, FileMode.CreateNew)) { }
                }
            }
            catch (UnauthorizedAccessException unex)
            {
                Debug.WriteLine($"An UnauthorizedAccessException error occurred in ResourcesCreated(): {unex.Message}");

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An exception occurred in ResourcesCreated(): {ex.Message}");
                Identifiers = null;

                return false;
            }

            return true;

        }

        private void GrantFolderPermissions(string folderPath)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            DirectorySecurity dirSecurity = dirInfo.GetAccessControl();

            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

            FileSystemAccessRule accessRule = new FileSystemAccessRule(
                sid,
                FileSystemRights.Modify | FileSystemRights.ReadAndExecute | FileSystemRights.Synchronize,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow
                );

            dirSecurity.AddAccessRule(accessRule);
            dirInfo.SetAccessControl(dirSecurity);
#pragma warning restore CA1416 // Validate platform compatibility
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
                if (!File.Exists(IdentifiersPath))
                {
                    var f = new FileStream(IdentifiersPath, FileMode.CreateNew);
                    f.Close();

                }
                else
                {

                    foreach (string s in File.ReadLines(IdentifiersPath))
                    {
                        Identifiers.Add(s);
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
        public string IdentifiersChecked(LOG log)
        {
            if (IdentifiersChecked() && Identifiers is not null)
            {
                try
                {
                    var id = log.Start.ToString("yyyyMMdd_HHmmss");

                    /** If the id doesn't exist, add it to the Identifiers List and write the new id to the
                     * Identifiers.index file.
                     */

                    if (!Identifiers.Contains(id))
                    {
                        Identifiers.Add(id);

                        Writer = new StreamWriter(IdentifiersPath);
                        foreach (string t in Identifiers.Order())
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

            return "";

        }

        public void IdentifiersChecked(string id)
        {
            if (IdentifiersChecked() && Identifiers is not null)
            {
                try
                {

                    // If the id doesn't exist, add it to the Identifiers List and write the new id to the
                    // Identifiers .index file.

                    if (!Identifiers.Contains(id))
                    {
                        Identifiers.Add(id);

                        Writer = new StreamWriter(IdentifiersPath);
                        foreach (string t in Identifiers.Order())
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



        public void DeleteViewModel(string logID, CacheContext cacheContext)
        {
            if (Identifiers is null || !Identifiers.Contains(logID))
                return;

            try
            {
                var filePath = $@"{ResourceDirectory}\{logID}.{LogExtension(cacheContext)}";

                if (File.Exists(filePath))
                    File.Delete(filePath);

                // Remove logID from IDENTIFIERS
                Identifiers.Remove(logID);

                if (File.Exists(IdentifiersPath))
                    File.Delete(IdentifiersPath);

                // Create new IDENTIFIERS file with updated list
                using var fileStream = new FileStream(IdentifiersPath, FileMode.Create);
                fileStream.Close();

                // Write IDENTIFIERS to new file
                using var writer = new StreamWriter(IdentifiersPath);
                foreach (string id in Identifiers.Order())
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

        

        /*
         #region Coding





        /// <summary>
        /// Saves a temporarily logged coding log after its been annotated.
        /// </summary>
        /// <param name="codeLOGViewModel">The coding interface that is being logged. 
        /// The method will extract the relevant info.</param>
        /// <param name="cacheContext">The type of log being cached.</param>
        public void SaveCodeViewModel(CodeLOGViewModel codeLOGViewModel, CacheContext cacheContext)
        {
            var logID = codeLOGViewModel.StartAsID;

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
        public ObservableCollection<CodeLOGViewModel>? LoadCodeViewModels(LogCacheViewModel logCacheViewModel, IDataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "cdecache";
                    string root = $@"{ResourceDirectory}\";
                    ObservableCollection<CodeLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (string id in Identifiers)
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
            var logID = graphicsLOGViewModel.StartAsID;

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
        public ObservableCollection<GraphicsLOGViewModel>? LoadGraphicsViewModels(LogCacheViewModel logCacheViewModel, IDataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "gpscache";
                    string root = $@"{ResourceDirectory}\";
                    ObservableCollection<GraphicsLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (string id in Identifiers)
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
            var logID = filmLOGViewModel.StartAsID;

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



        public ObservableCollection<FilmLOGViewModel>? LoadFilmViewModels(LogCacheViewModel logCacheViewModel, IDataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "flmcache";
                    string root = $@"{ResourceDirectory}\";
                    ObservableCollection<FilmLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (string id in Identifiers)
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
            var logID = flexiLOGViewModel.StartAsID;

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



        public ObservableCollection<FlexiLOGViewModel>? LoadFlexiViewModels(LogCacheViewModel logCacheViewModel, IDataService dataService, ACCOUNT account)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = "flxcache";
                    string root = $@"{ResourceDirectory}\";
                    ObservableCollection<FlexiLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (string id in Identifiers)
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
         */
    }
}
