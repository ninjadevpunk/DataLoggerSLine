using Core.Models;
using static FileSys.Services.CacheMaster;

namespace FileSys.Interfaces
{
    public interface ICacheService
    {
        bool CreateDirectory(string path);
        public bool Exists(string path) => File.Exists(path);
        string? ReadAllText(string path);
        bool WriteAllText(string path, string contents);
        bool DeleteDirectory(string path);


        /// <summary>
        /// Creates vital resources for the CacheMaster so the class can function correctly.
        /// </summary>
        /// <returns>Returns whether or not the resources were successfully created if they had not existed.</returns>
        bool ResourcesCreated();


        /// <summary>
        /// Only checks if the Identifiers file exists and creates it if it doesn't. Will then insert found identifiers into the Identifiers list.
        /// </summary>
        /// <returns>Returns true if the file was successfully created in the event that the file doesn't already exist or true because all identifiers found were successfully added.
        /// </returns>
        bool IdentifiersChecked();

        /// <summary>
        /// Checks if the Identifiers file exists and adds a new log's ID into the list. P.S. Will call IdentifiersChecked() automatically.
        /// </summary>
        /// <param name="log">The log that will be cached.</param>
        /// <returns>The log's ID if the identifier was successfully inserted.</returns>
        string IdentifiersChecked(LOG log);

        void IdentifiersChecked(string id);





        /// <summary>
        /// Saves a log on the logger.
        /// </summary>
        /// <param name="log">The log that is top be saved.</param>
        /// <param name="filePath">The location to store the log.</param>
        /// <param name="cacheContext">The type of log being saved.</param>
        void SaveLog(LOG log, string filePath);



        string LogExtension(CacheContext context);


        void DeleteViewModel(string logID, CacheContext cacheContext);
    }
}
