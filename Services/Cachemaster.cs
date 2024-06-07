using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;

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
         * Use this class to create files for permanently keep contents of unstored LOGS.
         * When a LOG is removed from DATAMASTER, this class will remove the file associated
         * with it in 20 minutes.
         */
        const string IDENTIFIERS_PATH = @"C:\Data Logger Central\Depository\res\_identifiers.index";
        const string RESOURCE_DIRECTORY = @"C:\Data Logger Central\Depository\res";

        public List<int>? Identifiers { get; set; } = new();
        public StreamWriter Writer { get; set; }

        public Cachemaster()
        {
            if (ResourcesCreated())
                IdentifiersChecked();
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
            if (!Directory.Exists(RESOURCE_DIRECTORY))
            {
                try
                {
                    DirectoryInfo directory = Directory.CreateDirectory(RESOURCE_DIRECTORY);
                    directory.Create();


                    var f = new FileStream(IDENTIFIERS_PATH, FileMode.CreateNew);
                    f.Close();

                }
                catch (Exception)
                {
                    Identifiers = null;
                    return false;
                }
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

        private static readonly DataContractSerializerSettings Settings = new DataContractSerializerSettings
        {
            PreserveObjectReferences = true
        };


        public void SaveLog(LOG log)
        {
            string filePath = $@"{RESOURCE_DIRECTORY}\{log.ID}.dls1cache";

            IdentifiersChecked(log);

            var fileStream = new FileStream(filePath, FileMode.Create);

            switch (log.Category)
            {
                case LOG.CATEGORY.CODING:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(CodingLOG), Settings);

                        var codingLog = (CodingLOG)log;
                        serializer.WriteObject(fileStream, codingLog);
                        break;
                    }
                case LOG.CATEGORY.GRAPHICS:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(GraphicsLOG), Settings);

                        var graphicsLog = (GraphicsLOG)log;
                        serializer.WriteObject(fileStream, graphicsLog);
                        break;
                    }
                case LOG.CATEGORY.FILM:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(FilmLOG), Settings);

                        var filmLog = (FilmLOG)log;
                        serializer.WriteObject(fileStream, filmLog);
                        break;
                    }
                case LOG.CATEGORY.NOTES:
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(FlexiNotesLOG), Settings);

                        var flexiNotesLog = (FlexiNotesLOG)log;
                        serializer.WriteObject(fileStream, flexiNotesLog);
                        break;
                    }
            }
        }

        public void SaveQtViewModel(QtLOGViewModel qtViewModel)
        {
            int logID = qtViewModel._QtcodingLOG.ID;
            string filePath = $@"{RESOURCE_DIRECTORY}\{logID}.qtcache";


            try
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(qtViewModel._QtcodingLOG, settings);
                File.WriteAllText(filePath, json);

            }
            catch (Exception)
            {
                // TODO
            }
        }

        public ObservableCollection<QtLOGViewModel>? LoadQtViewModels(LogCacheViewModel logCacheViewModel, DataService dataService)
        {
            if (Identifiers is not null)
            {
                try
                {
                    const string extenstion = ".qtcache";
                    string root = $@"{RESOURCE_DIRECTORY}\";
                    ObservableCollection<QtLOGViewModel>? list = null;

                    if (Identifiers.Count > 0)
                        list = new();

                    foreach (int id in Identifiers)
                    {
                        var filePath = $@"{root}{id}{extenstion}";

                        if (File.Exists(filePath) && list is not null)
                        {
                            var json = File.ReadAllText(filePath);
                            var settings = new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            };

                            var log = JsonConvert.DeserializeObject<CodingLOG>(json, settings);
                            var viewModel = new QtLOGViewModel(log, logCacheViewModel, dataService);

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


        public void DeleteQtViewModel(int logID)
        {
            if (Identifiers is not null && Identifiers.Contains(logID))
            {
                try
                {
                    var filePath = $@"{RESOURCE_DIRECTORY}\{logID}.qtcache";

                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    // Remove logID from IDENTIFIERS
                    Identifiers.Remove(logID);
                    if (File.Exists(IDENTIFIERS_PATH))
                        File.Delete(IDENTIFIERS_PATH);

                    // Create new IDENTIFIERS file with updated list
                    var f = new FileStream(IDENTIFIERS_PATH, FileMode.Create);
                    f.Close();

                    // Write IDENTIFIERS to new file
                    Writer = new StreamWriter(IDENTIFIERS_PATH);

                    foreach (int id in Identifiers.Order())
                    {
                        Writer.WriteLine(id);
                    }

                    Writer.Close();
                }
                catch (Exception)
                {
                    // TODO
                }

            }
        }


        public void SaveASViewModel(AndroidLOGViewModel asViewModel)
        {
            string filePath = $@"{RESOURCE_DIRECTORY}\{asViewModel._AndroidCodingLOG.ID}.AScache";

            var fileStream = new FileStream(filePath, FileMode.Create);
            DataContractSerializer serializer = new DataContractSerializer(typeof(AndroidLOGViewModel), Settings);

            serializer.WriteObject(fileStream, asViewModel);
            fileStream.Close();
        }

        public void SaveCodingViewModel(CodeLOGViewModel codingViewModel)
        {
            string filePath = $@"{RESOURCE_DIRECTORY}\{codingViewModel._CodeLOG.ID}.cdecache";

            var fileStream = new FileStream(filePath, FileMode.Create);
            DataContractSerializer serializer = new DataContractSerializer(typeof(CodeLOGViewModel), Settings);

            serializer.WriteObject(fileStream, codingViewModel);
            fileStream.Close();
        }

        public void SaveGraphicsViewModel(GraphicsLOGViewModel graphicsViewModel)
        {
            string filePath = $@"{RESOURCE_DIRECTORY}\{graphicsViewModel._GraphicsLOG.ID}.gpscache";

            var fileStream = new FileStream(filePath, FileMode.Create);
            DataContractSerializer serializer = new DataContractSerializer(typeof(GraphicsLOGViewModel), Settings);

            serializer.WriteObject(fileStream, graphicsViewModel);
            fileStream.Close();
        }


        public void SaveFilmViewModel(FilmLOGViewModel filmViewModel)
        {
            string filePath = $@"{RESOURCE_DIRECTORY}\{filmViewModel._FilmLOG.ID}.flmcache";

            var fileStream = new FileStream(filePath, FileMode.Create);
            DataContractSerializer serializer = new DataContractSerializer(typeof(FilmLOGViewModel), Settings);

            serializer.WriteObject(fileStream, filmViewModel);
            fileStream.Close();
        }

        public void SaveFlexiViewModel(FlexiLOGViewModel flexiViewModel)
        {
            string filePath = $@"{RESOURCE_DIRECTORY}\{flexiViewModel._FlexiLOG.ID}.flxcache";

            var fileStream = new FileStream(filePath, FileMode.Create);
            DataContractSerializer serializer = new DataContractSerializer(typeof(FlexiLOGViewModel), Settings);

            serializer.WriteObject(fileStream, flexiViewModel);
            fileStream.Close();
        }



        public void DeleteLog(int logID)
        {

            if (Identifiers.Contains(logID))
            {
                var filePath = $@"{RESOURCE_DIRECTORY}\{logID}.dls1cache";

                File.Delete(filePath);

                // Remove logID from IDENTIFIERS
                Identifiers.Remove(logID);
                File.Delete(IDENTIFIERS_PATH);

                // Create new IDENTIFIERS file with updated list
                var f = new FileStream(IDENTIFIERS_PATH, FileMode.Create);
                f.Close();

                // Write IDENTIFIERS to new file
                Writer = new StreamWriter(IDENTIFIERS_PATH);
                foreach (int id in Identifiers.Order())
                {
                    Writer.WriteLine(id);
                }
                Writer.Close();

            }
        }
    }
}
