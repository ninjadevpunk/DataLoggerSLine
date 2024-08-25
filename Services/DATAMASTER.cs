using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using static Data_Logger_1._3.Models.FlexiNotesLOG;
using static Data_Logger_1._3.Models.NotesLOG;

namespace Data_Logger_1._3.Services
{
    public abstract class DATAMASTER : List<LOG>
    {
        /*
         * DOCUMENTATION
         * 
         * The base class for all SQLite related activities.
         * Please use its subclasses to do any database work!
         */


        /* MEMBER VARIABLES */


        /** Official Connection to the database */
        protected SQLiteConnection _con;

        /** Store the database connection status here. 
            If the database is open set to true.
            Is false by default.
        */
        private bool databaseStatus = false;


        /** Who's logged in? */
        public ACCOUNT? User { get; set; }


        /** Store the logID of a LOG temporarily so it can be updated in future.
         */
        protected int LOG_ID = 100000000;

        /// <summary>
        /// Stores various integers representing objects that haven't been inserted into the database.
        /// </summary>
        public IDWatcher Watcher { get; set; } = new();

        /// <summary>
        /// Used to access column names.
        /// </summary>
        public ColumnNAME Column { get; } = new();


        protected bool AppAlreadyAdded = false;
        protected bool ProjectAlreadyAdded = false;
        protected bool SubjectAlreadyAdded = false;

        /* Commonly used strings */
        /// <summary>
        /// The database's location.
        /// </summary>
        private const string CONNECTION_STRING = @"Data Source=C:\Data Logger Central\Depository\LOGS_TEST.db";
        protected const string Qt = "Qt Creator";
        protected const string Android = "Android Studio Hedgehog 2023.1.1";


        // ---END MEMBER VARIABLES







        /* CONSTRUCTORS */




        #region CONSTRUCTORS

        public DATAMASTER() { }

        public DATAMASTER(LOG log)
        {

            Add(log);

        }


        public DATAMASTER(bool option)
        {
            if (option)
            {
                _con = new SQLiteConnection(CONNECTION_STRING);

                try
                {
                    _con.Open();
                    databaseStatus = true;
                }
                catch (Exception)
                {
                    // TODO Damage control for if the database doesn't open.
                }
            }
        }

        public DATAMASTER(SQLiteConnection connection, string category, bool databaseStatus)
        {
            _con = connection;
            this.databaseStatus = databaseStatus;


        }

        public DATAMASTER(SQLiteConnection connection, string category, bool databaseStatus, LOG current)
        {
            _con = connection;
            this.databaseStatus = databaseStatus;
        }




        #endregion




        // ---END CONSTRUCTORS---


        /* MEMBER FUNCTIONS */


        public bool SetCurrentUser(ACCOUNT account)
        {
            SQLiteCommand query = _con.CreateCommand();



            try
            {
                query.CommandText = $@"UPDATE ACCOUNT
                          SET online = @online
                          WHERE {Column.AccountID} = @id
;";
                query.Parameters.AddWithValue("@online", account.Online);
                query.Parameters.AddWithValue("@id", account.ID);

                query.ExecuteNonQuery();

                query.CommandText = $@"SELECT * FROM ACCOUNT WHERE {Column.AccountID} = @id
;";
                query.Parameters.AddWithValue("@id", account.ID);

                var emailColumn = 4;
                var onlineColumn = 10;

                SQLiteDataReader read = query.ExecuteReader();

                while (read.Read())
                {
                    User = account;
                    account.Online = read.GetBoolean(onlineColumn);
                }

                read.Close();

                return account.Online;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool UnsetCurrentUser(ACCOUNT account)
        {
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = $@"UPDATE ACCOUNT
                          SET online = @online
                          WHERE {Column.AccountID} = @id;
";
            query.Parameters.AddWithValue("@online", false);
            query.Parameters.AddWithValue("@id", account.ID);

            query.ExecuteNonQuery();

            query.CommandText = $@"SELECT * FROM ACCOUNT WHERE {Column.AccountID} = @id
;";
            query.Parameters.AddWithValue("@id", account.ID);


            var onlineColumn = 10;

            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                account.Online = read.GetBoolean(onlineColumn);
            }

            read.Close();

            return !account.Online;
        }


        public SQLiteConnection GetConnection()
        {
            return _con;
        }

        /** Create the database */
        public SQLiteConnection CreateConnection()
        {
            _con = new SQLiteConnection(CONNECTION_STRING);

            try
            {
                _con.Open();
                databaseStatus = true;
            }
            catch (Exception)
            {
                // TODO Damage control for if the database doesn't open.
            }


            return _con;
        }

        /** Open the database */
        public void open()
        {
            try
            {
                _con.Open();
                databaseStatus = true;
            }
            catch (Exception)
            {
                // TODO Damage control for if the database doesn't open.
            }
        }

        /** Make sure to close the database with this function */
        public void close()
        {
            try
            {
                _con.Close();
                databaseStatus = false;
            }
            catch (Exception)
            {
                databaseStatus = true;
                // TODO Damage control for if the database doesn't open.
            }
        }

        /** Create the tables for the application 
            CALL THESE FUNCTIONS THE MOMENT AN INSTANCE IS CREATED AFTER CreateConnection() or open() is called.
         */

        #region Call IMMEDIATELY. (Vital for the application to function properly)



        public void CheckTables()
        {
            SQLiteCommand query = _con.CreateCommand();

            query.CommandText = $@"CREATE TABLE IF NOT EXISTS CATEGORY(
                                    {Column.CategoryID} INTEGER NOT NULL,
                                    {Column.category} VARCHAR(50) NOT NULL UNIQUE,
                                    PRIMARY KEY({Column.CategoryID} AUTOINCREMENT)
                                    );

                                    CREATE TABLE IF NOT EXISTS APPLICATION(
                                    {Column.AppID} INTEGER NOT NULL,
                                    {Column.AccountID} INTEGER NOT NULL,
                                    application VARCHAR(255) NOT NULL UNIQUE,
                                    {Column.CategoryID} INTEGER NOT NULL,
                                    IsDefault INTEGER NOT NULL DEFAULT 0,
                                    PRIMARY KEY({Column.AppID} AUTOINCREMENT),
                                    FOREIGN KEY({Column.AccountID}) REFERENCES ACCOUNT({Column.AccountID}),
                                    FOREIGN KEY({Column.CategoryID}) REFERENCES CATEGORY({Column.CategoryID}),
                                    UNIQUE(application, {Column.AccountID}, {Column.CategoryID})
                                    );

                                    CREATE TABLE IF NOT EXISTS PROJECT(
                                    {Column.ProjectID} INTEGER NOT NULL,
                                    {Column.AppID} INTEGER NOT NULL,
                                    {Column.AccountID} INTEGER NOT NULL,
                                    {Column.CategoryID} INTEGER NOT NULL,
                                    {Column.project} VARCHAR(255) NOT NULL,
                                    PRIMARY KEY({Column.ProjectID} AUTOINCREMENT),
                                    FOREIGN KEY({Column.AccountID}) REFERENCES ACCOUNT({Column.AccountID}),
                                    FOREIGN KEY({Column.AppID}) REFERENCES APPLICATION({Column.AppID}),
                                    FOREIGN KEY({Column.CategoryID}) REFERENCES CATEGORY({Column.CategoryID}),
                                    UNIQUE ({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {Column.project})
                                    );

                                    CREATE TABLE IF NOT EXISTS OUTPUT(
                                    outputID INTEGER NOT NULL,
                                    {Column.AccountID} INTEGER NOT NULL,
                                    {Column.AppID} INTEGER NOT NULL,
                                    {Column.CategoryID} INTEGER NOT NULL,
                                    output VARCHAR(255) NOT NULL,
                                    PRIMARY KEY(outputID AUTOINCREMENT),
                                    FOREIGN KEY({Column.AccountID}) REFERENCES ACCOUNT({Column.AccountID}),
                                    FOREIGN KEY({Column.AppID}) REFERENCES APPLICATION({Column.AppID}),
                                    FOREIGN KEY({Column.CategoryID}) REFERENCES CATEGORY({Column.CategoryID}),
                                    UNIQUE(output, {Column.AccountID}, {Column.AppID}, {Column.CategoryID})
                                    );

                                    CREATE TABLE IF NOT EXISTS TYPE(
                                    typeID INTEGER NOT NULL,
                                    {Column.AccountID} INTEGER NOT NULL,
                                    {Column.AppID} INTEGER NOT NULL,
                                    {Column.CategoryID} INTEGER NOT NULL,
                                    type VARCHAR(255) NOT NULL,
                                    PRIMARY KEY(typeID AUTOINCREMENT),
                                    FOREIGN KEY({Column.AccountID}) REFERENCES ACCOUNT({Column.AccountID}),
                                    FOREIGN KEY({Column.AppID}) REFERENCES APPLICATION({Column.AppID}),
                                    FOREIGN KEY({Column.CategoryID}) REFERENCES CATEGORY({Column.CategoryID})
                                    UNIQUE(type, {Column.AccountID}, {Column.AppID}, {Column.CategoryID})
                                    );

                                    CREATE TABLE IF NOT EXISTS ACCOUNT(
                                    {Column.AccountID} INTEGER NOT NULL,
                                    profilepic VARCHAR(1000000) NOT NULL DEFAULT """",
                                    first VARCHAR(255) NOT NULL DEFAULT """",
                                    last VARCHAR(255) NOT NULL DEFAULT """",
                                    email VARCHAR(255) NOT NULL UNIQUE,
                                    password VARCHAR(300) NOT NULL,
                                    IsEmployee INTEGER NOT NULL DEFAULT 0,
                                    companyName VARCHAR(255),
                                    companyAddress VARCHAR(255),
                                    companyLogo VARCHAR(255),
                                    online INTEGER NOT NULL DEFAULT 0,
                                    PRIMARY KEY({Column.AccountID} AUTOINCREMENT)
                                    );

                                    CREATE TABLE IF NOT EXISTS PostIt(
                                    postItID INTEGER NOT NULL,
                                    logID INTEGER NOT NULL,
                                    subjectID INTEGER NOT NULL,
                                    error VARCHAR(1000000),
                                    date_found TEXT,
                                    solution VARCHAR(1000000),
                                    date_solved TEXT NOT NULL,
                                    suggestion VARCHAR(1000000),
                                    comment VARCHAR(1000000),
                                    PRIMARY KEY(postItID),
                                    FOREIGN KEY(logID) REFERENCES LOG(logID),
                                    FOREIGN KEY(subjectID) REFERENCES Subject(subjectID)
                                    );

                                    CREATE TABLE IF NOT EXISTS  Subject(
                                    subjectID INTEGER NOT NULL,
                                    {Column.ProjectID} INTEGER NOT NULL,
                                    {Column.AppID} INTEGER NOT NULL,
                                    {Column.AccountID} INTEGER NOT NULL,
                                    {Column.CategoryID} INTEGER NOT NULL,
                                    subject VARCHAR(255),
                                    PRIMARY KEY(subjectID),
                                    FOREIGN KEY({Column.ProjectID}) REFERENCES PROJECT({Column.ProjectID}),
                                    FOREIGN KEY({Column.AppID}) REFERENCES APPLICATION({Column.AppID}),
                                    FOREIGN KEY({Column.AccountID}) REFERENCES ACCOUNT({Column.AccountID}),
                                    FOREIGN KEY({Column.CategoryID}) REFERENCES CATEGORY({Column.CategoryID}),
                                    UNIQUE(subject, {Column.ProjectID}, {Column.AppID}, {Column.AccountID})
                                    );

                                    CREATE TABLE IF NOT EXISTS LOG(
                                    logID INTEGER NOT NULL,
                                    {Column.CategoryID} INTEGER NOT NULL,
                                    {Column.AccountID} INTEGER NOT NULL,
                                    {Column.ProjectID} INTEGER NOT NULL,
                                    {Column.AppID} INTEGER NOT NULL,
                                    start TEXT NOT NULL,
                                    end TEXT NOT NULL,
                                    outputID INTEGER NOT NULL,
                                    typeID INTEGER NOT NULL,
                                    PRIMARY KEY(logID AUTOINCREMENT),
                                    FOREIGN KEY({Column.CategoryID}) REFERENCES CATEGORY({Column.CategoryID}),
                                    FOREIGN KEY({Column.AccountID}) REFERENCES ACCOUNT({Column.AccountID}),
                                    FOREIGN KEY({Column.ProjectID}) REFERENCES PROJECT({Column.ProjectID}),
                                    FOREIGN KEY({Column.AppID}) REFERENCES APPLICATION({Column.AppID}),
                                    FOREIGN KEY(outputID) REFERENCES OUTPUT(outputID),
                                    FOREIGN KEY(typeID) REFERENCES TYPE(typeID)
                                    );

                                    CREATE TABLE IF NOT EXISTS  MEDIUM(
                                    mediumID INTEGER NOT NULL,
                                    {Column.CategoryID} INTEGER NOT NULL,
                                    medium VARCHAR(255) NOT NULL,
                                    PRIMARY KEY(mediumID),
                                    FOREIGN KEY({Column.CategoryID}) REFERENCES CATEGORY({Column.CategoryID})
                                    );
                                    
                                    CREATE TABLE IF NOT EXISTS  FORMAT(
                                    formatID INTEGER NOT NULL,
                                    {Column.CategoryID} INTEGER NOT NULL,
                                    format VARCHAR(255) NOT NULL,
                                    PRIMARY KEY(formatID),
                                    FOREIGN KEY({Column.CategoryID}) REFERENCES CATEGORY({Column.CategoryID})
                                    );
                                    
                                    CREATE TABLE IF NOT EXISTS  MeasuringUnit(
                                    unitID INTEGER NOT NULL,
                                    unit VARCHAR(255) NOT NULL,
                                    PRIMARY KEY(unitID AUTOINCREMENT)
                                    );

                                    CREATE TABLE IF NOT EXISTS  CodingLOG(
                                    logID INTEGER NOT NULL,
                                    bugs INTEGER NOT NULL DEFAULT 0,
                                    opened INTEGER NOT NULL DEFAULT 0,
                                    PRIMARY KEY(logID AUTOINCREMENT),
                                    FOREIGN KEY(logID) REFERENCES LOG(logID)
                                    );
                                    
                                    CREATE TABLE IF NOT EXISTS  AndroidCodingLOG(
                                    logID INTEGER NOT NULL,
                                    fullORsimple INTEGER NOT NULL,
                                    sync TEXT NOT NULL,
                                    gradleDaemon TEXT,
                                    runBuild TEXT,
                                    loadBuild TEXT,
                                    configBuild TEXT,
                                    allProjects TEXT,
                                    PRIMARY KEY(logID),
                                    FOREIGN KEY(logID) REFERENCES LOG(logID)
                                    );

                                    CREATE TABLE IF NOT EXISTS  GraphicsLOG(
                                    logID INTEGER NOT NULL,
                                    mediumID INTEGER NOT NULL,
                                    formatID INTEGER NOT NULL,
                                    brush VARCHAR(255),
                                    height DOUBLE(16) NOT NULL,
                                    width DOUBLE(16) NOT NULL,
                                    unitID INTEGER NOT NULL,
                                    size VARCHAR(255),
                                    DPI DOUBLE(16),
                                    depth VARCHAR(500),
                                    completed INTEGER NOT NULL DEFAULT 0,
                                    source VARCHAR(1000000),
                                    PRIMARY KEY(logID),
                                    FOREIGN KEY(logID) REFERENCES LOG(logID),
                                    FOREIGN KEY(mediumID) REFERENCES MEDIUM(mediumID),
                                    FOREIGN KEY(formatID) REFERENCES FORMAT(formatID),
                                    FOREIGN KEY(unitID) REFERENCES MeasuringUnit(unitID)
                                    );

                                    CREATE TABLE IF NOT EXISTS  FilmLOG(
                                    logID INTEGER NOT NULL,
                                    height DOUBLE(16) NOT NULL,
                                    width DOUBLE(16) NOT NULL,
                                    length TEXT NOT NULL,
                                    completed INTEGER NOT NULL,
                                    source VARCHAR(1000000),
                                    PRIMARY KEY(logID),
                                    FOREIGN KEY(logID) REFERENCES LOG(logID)
                                    );

                                    CREATE TABLE IF NOT EXISTS  NotesLOG(
                                    logID INTEGER NOT NULL,
                                    noteLogTypeID INTEGER NOT NULL,
                                    PRIMARY KEY(logID),
                                    FOREIGN KEY(logID) REFERENCES LOG(logID),
                                    FOREIGN KEY(noteLogTypeID) REFERENCES NoteLogTypes(noteLogTypeID)
                                    );
                                    
                                    CREATE TABLE IF NOT EXISTS NoteLogTypes(
                                    noteLogTypeID INTEGER NOT NULL,
                                    noteLogType VARCHAR(255) NOT NULL,
                                    PRIMARY KEY(noteLogTypeID AUTOINCREMENT) 
                                    );

                                    CREATE TABLE IF NOT EXISTS NoteItem(
                                    logID INTEGER NOT NULL,
                                    IsChecklist INTEGER NOT NULL,
                                    subject VARCHAR(255),
                                    genericNote VARCHAR(100000000),
                                    PRIMARY KEY(logID),
                                    FOREIGN KEY(logID) REFERENCES LOG(logID)
                                    );
                                    
                                    CREATE TABLE IF NOT EXISTS Checklist(
                                    itemsID INTEGER NOT NULL,
                                    logID INTEGER NOT NULL,
                                    item VARCHAR(255) NOT NULL,
                                    done INTEGER NOT NULL,
                                    PRIMARY KEY(itemsID AUTOINCREMENT),
                                    FOREIGN KEY(logID) REFERENCES LOG(logID)
                                    );

                                    CREATE TABLE IF NOT EXISTS  FlexiNotesLOG(
                                    logID INTEGER NOT NULL,
                                    flexiNoteTypeID INTEGER NOT NULL,
                                    mediumID INTEGER NOT NULL,
                                    formatID INTEGER NOT NULL,
                                    bitRate INTEGER,
                                    length TEXT NOT NULL,
                                    completed INTEGER NOT NULL,
                                    source VARCHAR(1000000),
                                    PRIMARY KEY(logID),
                                    FOREIGN KEY(logID) REFERENCES LOG(logID)
                                    FOREIGN KEY(flexiNoteTypeID) REFERENCES FlexiNoteType(flexiNoteTypeID)
                                    );
                                    
                                    
                                    CREATE TABLE IF NOT EXISTS  FlexiNoteType(
                                    flexiNoteTypeID INTEGER NOT NULL,
                                    flexiNoteType VARCHAR(255) NOT NULL,
                                    PRIMARY KEY(flexiNoteTypeID AUTOINCREMENT)
                                    );
                                    
                                    CREATE TABLE IF NOT EXISTS  FEEDBACK(
                                    feedbackID INTEGER NOT NULL,
                                    feedTypeID INTEGER NOT NULL,
                                    {Column.AccountID} INTEGER NOT NULL,
                                    date TEXT NOT NULL,
                                    description VARCHAR(10000000),
                                    CanContact INTEGER NOT NULL,
                                    PRIMARY KEY(feedbackID AUTOINCREMENT),
                                    FOREIGN KEY(feedTypeID) REFERENCES FEEDTYPE(feedTypeID),
                                    FOREIGN KEY({Column.AccountID}) REFERENCES ACCOUNT({Column.AccountID})
                                    );
                                    
                                    CREATE TABLE IF NOT EXISTS  FEEDTYPE(
                                    feedTypeID INTEGER NOT NULL,
                                    FeedBackType VARCHAR(255),
                                    PRIMARY KEY(feedTypeID AUTOINCREMENT)
                                    );


";
            query.ExecuteNonQuery();
        }

        /* Initialise Database Tables */
        public void CheckCategories()
        {
            /** Check the CATEGROY TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, simply add them 
             *  */
            bool ValuesExist = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM CATEGORY";

            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                ValuesExist = true;
                break;
            }
            read.Close();

            if (!ValuesExist)
            {
                SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = $@"
                                    INSERT INTO CATEGORY({Column.category})
                                    VALUES(@category1);
                                    INSERT INTO CATEGORY({Column.category})
                                    VALUES(@category2);
                                    INSERT INTO CATEGORY({Column.category})
                                    VALUES(@category3);
                                    INSERT INTO CATEGORY({Column.category})
                                    VALUES(@category4);
";
                insert.Parameters.AddWithValue("@category1", "CODING");
                insert.Parameters.AddWithValue("@category2", "GRAPHICS");
                insert.Parameters.AddWithValue("@category3", "FILM");
                insert.Parameters.AddWithValue("@category4", "NOTES");

                insert.ExecuteNonQuery();
            }
        }

        public void CheckApplications()
        {
            /** Check the APPLICATION TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, add the defaults
             *  */
            bool ValuesExist = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM APPLICATION
;";

            SQLiteDataReader read = query.ExecuteReader();

            try
            {
                while (read.Read())
                {
                    ValuesExist = true;
                    break;
                }

                read.Close();


                if (!ValuesExist)
                {
                    SQLiteCommand insert = _con.CreateCommand();
                    const string dp = "profilepic", first = "first", last = "last", email = "email", password = "password",
                        isemployee = "IsEmployee", company = "companyName", companyAddress = "companyAddress", logo = "companyLogo", online = "online";

                    insert.CommandText = $@"INSERT INTO ACCOUNT({Column.AccountID}, {dp}, {first}, {last}, {email}, {password}, {isemployee}, {company}, {companyAddress}, {logo}, {online})
                                    VALUES(@account, @dp, @first, @last, @email, @password, @isemployee, @company, @address, @icon, @online);";

                    insert.Parameters.AddWithValue("@account", 1);
                    insert.Parameters.AddWithValue("@dp", "");
                    insert.Parameters.AddWithValue("@first", "admin");
                    insert.Parameters.AddWithValue("@last", "");
                    insert.Parameters.AddWithValue("@email", "support@datalogger.co.za");
                    insert.Parameters.AddWithValue("@password", "pcsx2024");
                    insert.Parameters.AddWithValue("@isemployee", false);
                    insert.Parameters.AddWithValue("@company", "");
                    insert.Parameters.AddWithValue("@address", "");
                    insert.Parameters.AddWithValue("@icon", "");
                    insert.Parameters.AddWithValue("@online", false);


                    insert.ExecuteNonQuery();


                    const string isdefault = "IsDefault";


                    insert.CommandText = $@"
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app1, @categoryID1, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app2, @categoryID2, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app3, @categoryID3, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app4, @categoryID4, 1);

                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app5, @categoryID5, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app6, @categoryID6, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app7, @categoryID7, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app8, @categoryID8, 1);


                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app9, @categoryID9, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app10, @categoryID10, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app11, @categoryID11, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app12, @categoryID12, 1);
                                    
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app13, @categoryID13, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app14, @categoryID14, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app15, @categoryID15, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app16, @categoryID16, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app17, @categoryID17, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app18, @categoryID18, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app19, @categoryID19, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app20, @categoryID20, 1);
                                    INSERT INTO APPLICATION({Column.AccountID}, {Column.app}, {Column.CategoryID}, {isdefault})
                                    VALUES(1, @app21, @categoryID21, 1);
                                    
";
                    insert.Parameters.AddWithValue("@app1", Qt);
                    insert.Parameters.AddWithValue("@categoryID1", 1);

                    insert.Parameters.AddWithValue("@app2", Android);
                    insert.Parameters.AddWithValue("@categoryID2", 1);

                    insert.Parameters.AddWithValue("@app3", "Unknown");
                    insert.Parameters.AddWithValue("@categoryID3", 1);

                    insert.Parameters.AddWithValue("@app4", "Visual Studio Community 2022");
                    insert.Parameters.AddWithValue("@categoryID4", 1);


                    insert.Parameters.AddWithValue("@app5", "Krita");
                    insert.Parameters.AddWithValue("@categoryID5", 2);

                    insert.Parameters.AddWithValue("@app6", "Inkscape");
                    insert.Parameters.AddWithValue("@categoryID6", 2);

                    insert.Parameters.AddWithValue("@app7", "Canva");
                    insert.Parameters.AddWithValue("@categoryID7", 2);

                    insert.Parameters.AddWithValue("@app8", "Adobe Illustrator");
                    insert.Parameters.AddWithValue("@categoryID8", 2);


                    insert.Parameters.AddWithValue("@app9", "Da Vinci Resolve");
                    insert.Parameters.AddWithValue("@categoryID9", 3);

                    insert.Parameters.AddWithValue("@app10", "Blender 3D/2D");
                    insert.Parameters.AddWithValue("@categoryID10", 3);

                    insert.Parameters.AddWithValue("@app11", "Powerpoint");
                    insert.Parameters.AddWithValue("@categoryID11", 3);

                    insert.Parameters.AddWithValue("@app12", "Shotcut");
                    insert.Parameters.AddWithValue("@categoryID12", 3);


                    insert.Parameters.AddWithValue("@app13", "Unity");
                    insert.Parameters.AddWithValue("@categoryID13", 4);

                    insert.Parameters.AddWithValue("@app14", "Steam");
                    insert.Parameters.AddWithValue("@categoryID14", 4);

                    insert.Parameters.AddWithValue("@app15", "Data Logger NOTES");
                    insert.Parameters.AddWithValue("@categoryID15", 4);

                    insert.Parameters.AddWithValue("@app16", "Data Logger Checklist");
                    insert.Parameters.AddWithValue("@categoryID16", 4);

                    insert.Parameters.AddWithValue("@app17", "Microsoft Word");
                    insert.Parameters.AddWithValue("@categoryID17", 4);

                    insert.Parameters.AddWithValue("@app18", "REAPER");
                    insert.Parameters.AddWithValue("@categoryID18", 4);

                    insert.Parameters.AddWithValue("@app19", "Notepad");
                    insert.Parameters.AddWithValue("@categoryID19", 4);

                    insert.Parameters.AddWithValue("@app20", "Microsoft Excel");
                    insert.Parameters.AddWithValue("@categoryID20", 4);

                    insert.Parameters.AddWithValue("@app21", "Microsoft Access");
                    insert.Parameters.AddWithValue("@categoryID21", 4);

                    insert.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }

        public void CheckProject()
        {
            /** Check the PROJECT TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, add the defaults
             *  */
            bool ValuesExist = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM PROJECT";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                ValuesExist = true;
                break;
            }

            read.Close();

            if (!ValuesExist)
            {
                SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = $@"INSERT INTO PROJECT({Column.ProjectID}, {Column.AppID}, {Column.AccountID}, {Column.CategoryID}, {Column.project})
                                    VALUES(@id, @app, @account, @category, @project)
;";
                insert.Parameters.AddWithValue("@id", 1);
                insert.Parameters.AddWithValue("@category", 1);
                insert.Parameters.AddWithValue("@account", 1);
                insert.Parameters.AddWithValue("@app", 3);
                insert.Parameters.AddWithValue("@project", "Unknown");

                insert.ExecuteNonQuery();
            }
        }

        public void CheckOutputs()
        {
            /** Check the OUTPUT TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, simply add them 
             *  */
            bool ValuesExist = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM OUTPUT ORDER BY outputID ASC";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                ValuesExist = true;
                break;
            }

            read.Close();

            if (!ValuesExist)
            {
                SQLiteCommand insert = _con.CreateCommand();
                string output = "output";

                insert.CommandText = $@"
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 1, 1, @o1);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 1, 1, @o2);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 1, 1, @o3);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 2, 1, @o4);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 2, 1, @o5);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 2, 1, @o6);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 1, @o7);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 1, @o8);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 1, @o9);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 1, @o10);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 1, @o11);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 5, 2, @o12);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 5, 2, @o13);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 6, 2, @o14);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 2, @o15);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 6, 2, @o16);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 5, 2, @o17);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 5, 2, @o18);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 5, 2, @o19);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 9, 3, @o20);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 9, 3, @o21);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 9, 3, @o22);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 3, @o23);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 3, @o24);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 17, 4, @o25);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 17, 4, @o26);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 19, 4, @o27);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 19, 4, @o28);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 17, 4, @o29);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 20, 4, @o30);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 21, 4, @o31);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 18, 4, @o32);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 18, 4, @o33);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 18, 4, @o34);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 4, @o35);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 3, 4, @o36);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 15, 4, @o37);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 16, 4, @o38);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 17, 4, @o39);
                                    INSERT INTO OUTPUT({Column.AccountID}, {Column.AppID}, {Column.CategoryID},{output})
                                    VALUES(1, 17, 4, @o40);
                                    
";
                insert.Parameters.AddWithValue("@o1", "Console Application");
                insert.Parameters.AddWithValue("@o2", "Widgets Application");
                insert.Parameters.AddWithValue("@o3", "QtQuick Application");
                insert.Parameters.AddWithValue("@o4", "APK (*.apk)");
                insert.Parameters.AddWithValue("@o5", "USB");
                insert.Parameters.AddWithValue("@o6", "Emulator (*.exe)");
                insert.Parameters.AddWithValue("@o7", "C# Application (*.exe)");
                insert.Parameters.AddWithValue("@o8", "C++ Application (*.exe)");
                insert.Parameters.AddWithValue("@o9", "Java Application (*.exe)");
                insert.Parameters.AddWithValue("@o10", "Database (*.db)");
                insert.Parameters.AddWithValue("@o11", "Database (SQL, Oracle)");

                insert.Parameters.AddWithValue("@o12", "PNG");
                insert.Parameters.AddWithValue("@o13", "JPG");
                insert.Parameters.AddWithValue("@o14", "SVG");
                insert.Parameters.AddWithValue("@o15", "GIF");
                insert.Parameters.AddWithValue("@o16", "PDF");
                insert.Parameters.AddWithValue("@o17", "TIFF");
                insert.Parameters.AddWithValue("@o18", "PSD");
                insert.Parameters.AddWithValue("@o19", "WEBP");

                insert.Parameters.AddWithValue("@o20", "MP4");
                insert.Parameters.AddWithValue("@o21", "AVI");
                insert.Parameters.AddWithValue("@o22", "MKV");
                insert.Parameters.AddWithValue("@o23", "TS");
                insert.Parameters.AddWithValue("@o24", "WEBM");

                insert.Parameters.AddWithValue("@o25", "PDF");
                insert.Parameters.AddWithValue("@o26", "WORD");
                insert.Parameters.AddWithValue("@o27", "TEXT FILE (*.txt)");
                insert.Parameters.AddWithValue("@o28", "HTML (*.html)");
                insert.Parameters.AddWithValue("@o29", "XPS (*.xps)");
                insert.Parameters.AddWithValue("@o30", "EXCEL FILE (*.xlsx)");
                insert.Parameters.AddWithValue("@o31", "ACCESS FILE (*.accdb)");

                insert.Parameters.AddWithValue("@o32", "MP3");
                insert.Parameters.AddWithValue("@o33", "WAV");
                insert.Parameters.AddWithValue("@o34", "AAC");
                insert.Parameters.AddWithValue("@o35", "M4A");
                insert.Parameters.AddWithValue("@o36", "OGG");

                insert.Parameters.AddWithValue("@o37", "Note");
                insert.Parameters.AddWithValue("@o38", "Check List");
                insert.Parameters.AddWithValue("@o39", "PNG");
                insert.Parameters.AddWithValue("@o40", "JPG");

                insert.ExecuteNonQuery();
            }
        }

        public void CheckTypes()
        {
            /** Check the TYPE TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, simply add them 
             *  */
            bool ValuesExist = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM TYPE;";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                ValuesExist = true;
                break;
            }

            read.Close();

            if (!ValuesExist)
            {
                SQLiteCommand insert = _con.CreateCommand();
                string type = "type";

                insert.CommandText = $@"
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 1, 1, @t1);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 1, 1, @t2);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 1, 1, @t3);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 2, 1, @t4);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 2, 1, @t5);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 2, 1, @t6);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 3, 1, @t7);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 3, 1, @t8);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 5, 2, @t9);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 5, 2, @t10);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 5, 2, @t11);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 6, 2, @t12);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 6, 2, @t13);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 5, 2, @t14);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 9, 3, @t15);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 9, 3, @t16);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 9, 3, @t17);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 9, 3, @t18);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 18, 4, @t19);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 17, 4, @t20);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 18, 4, @t21);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 13, 4, @t22);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 13, 4, @t23);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 13, 4, @t24);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 13, 4, @t25);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 17, 4, @t26);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 17, 4, @t27);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 17, 4, @t28);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 17, 4, @t29);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 17, 4, @t30);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 17, 4, @t31);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 17, 4, @t32);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 17, 4, @t33);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 19, 4, @t34);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 11, 4, @t35);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 20, 4, @t36);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 19, 4, @t37);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 21, 4, @t38);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 15, 4, @t39);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 16, 4, @t40);
                                    INSERT INTO TYPE({Column.AccountID}, {Column.AppID}, {Column.CategoryID}, {type})
                                    VALUES(1, 4, 1, @t41);
                                    
";
                insert.Parameters.AddWithValue("@t1", "NONE");
                insert.Parameters.AddWithValue("@t2", "Build");
                insert.Parameters.AddWithValue("@t3", "Runtime");

                insert.Parameters.AddWithValue("@t4", "Sync");
                insert.Parameters.AddWithValue("@t5", "Build");
                insert.Parameters.AddWithValue("@t6", "Runtime");

                insert.Parameters.AddWithValue("@t7", "Compilation");
                insert.Parameters.AddWithValue("@t8", "Runtime");

                insert.Parameters.AddWithValue("@t9", "NONE");
                insert.Parameters.AddWithValue("@t10", "Artwork");
                insert.Parameters.AddWithValue("@t11", "Doodle");
                insert.Parameters.AddWithValue("@t12", "Graphic Design");
                insert.Parameters.AddWithValue("@t13", "Resource");
                insert.Parameters.AddWithValue("@t14", "Portfolio");

                insert.Parameters.AddWithValue("@t15", "NONE");
                insert.Parameters.AddWithValue("@t16", "Film");
                insert.Parameters.AddWithValue("@t17", "Doodle");
                insert.Parameters.AddWithValue("@t18", "Assignment");

                insert.Parameters.AddWithValue("@t19", "Music");
                insert.Parameters.AddWithValue("@t20", "Poetry");
                insert.Parameters.AddWithValue("@t21", "Mantra");

                insert.Parameters.AddWithValue("@t22", "Boardgame");
                insert.Parameters.AddWithValue("@t23", "Video game");
                insert.Parameters.AddWithValue("@t24", "Card game");
                insert.Parameters.AddWithValue("@t25", "Game - Design ONLY");

                insert.Parameters.AddWithValue("@t26", "Document");
                insert.Parameters.AddWithValue("@t27", "Invitation");
                insert.Parameters.AddWithValue("@t28", "Mail");
                insert.Parameters.AddWithValue("@t29", "Report");
                insert.Parameters.AddWithValue("@t30", "Doodle");
                insert.Parameters.AddWithValue("@t31", "Novel");
                insert.Parameters.AddWithValue("@t32", "Questionnaire");
                insert.Parameters.AddWithValue("@t33", "CV");
                insert.Parameters.AddWithValue("@t34", "Web page");
                insert.Parameters.AddWithValue("@t35", "Presentation");
                insert.Parameters.AddWithValue("@t36", "Spreadsheet");
                insert.Parameters.AddWithValue("@t37", "Notes");
                insert.Parameters.AddWithValue("@t38", "Access Database");

                insert.Parameters.AddWithValue("@t39", "Note");
                insert.Parameters.AddWithValue("@t40", "Check List");
                insert.Parameters.AddWithValue("@t41", "Exception");

                

                insert.ExecuteNonQuery();
            }

        }

        public void CheckSubjects()
        {
            /** Check the Subject TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, simply add them 
             *  */
            bool ValuesExist = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM Subject ORDER BY subjectID ASC";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                ValuesExist = true;
                break;
            }
            read.Close();

            if (!ValuesExist)
            {
                SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = $@"INSERT INTO Subject(subjectID, {Column.ProjectID}, {Column.AppID}, {Column.AccountID}, {Column.CategoryID}, subject)
                                    VALUES(@id, @project, @app, @account, @category, @subject)
;";
                insert.Parameters.AddWithValue("@id", 1);
                insert.Parameters.AddWithValue("@project", 1);
                insert.Parameters.AddWithValue("@app", 3);
                insert.Parameters.AddWithValue("@account", 1);
                insert.Parameters.AddWithValue("@category", 1);
                insert.Parameters.AddWithValue("@subject", "No Subject");
                

                insert.ExecuteNonQuery();
            }
        }

        public void CheckMediums()
        {
            /** Check the MEDIUM TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, simply add them 
             *  */
            bool ValuesExist = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM MEDIUM;";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                ValuesExist = true;
                break;
            }

            read.Close();

            if (!ValuesExist)
            {

                query.CommandText = $@"
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(2, @m1);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(2, @m2);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(2, @m3);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(2, @m4);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(2, @m5);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(2, @m6);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(2, @m7);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(2, @m8);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(4, @m9);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(4, @m10);
                                    INSERT INTO MEDIUM({Column.CategoryID}, medium)
                                    VALUES(4, @m11);
                                    
";
                query.Parameters.AddWithValue("@m1", "Pencil");
                query.Parameters.AddWithValue("@m2", "Paint");
                query.Parameters.AddWithValue("@m3", "Pen");
                query.Parameters.AddWithValue("@m4", "Pencil Crayon");
                query.Parameters.AddWithValue("@m5", "Kokie");
                query.Parameters.AddWithValue("@m6", "Crayon");
                query.Parameters.AddWithValue("@m7", "Oil Pastel");
                query.Parameters.AddWithValue("@m8", "Chalk");
                query.Parameters.AddWithValue("@m9", "A Cappella");
                query.Parameters.AddWithValue("@m10", "Song");
                query.Parameters.AddWithValue("@m11", "Orchestral");
                query.Prepare();

                query.ExecuteNonQuery();
            }
        }

        public void CheckFormats()
        {
            /** Check the FORMAT TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, simply add them 
             *  */
            bool ValuesExist = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM FORMAT;";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                ValuesExist = true;
                break;
            }

            read.Close();

            if (!ValuesExist)
            {
                query.CommandText = $@"
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(2, @f1);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(2, @f2);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(2, @f3);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(2, @f4);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(4, @f5);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(4, @f6);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(4, @f7);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(4, @f8);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(4, @f9);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(4, @f10);
                                    INSERT INTO FORMAT({Column.CategoryID}, format)
                                    VALUES(4, @f11);
                                    
";
                query.Parameters.AddWithValue("@f1", "Paper");
                query.Parameters.AddWithValue("@f2", "Digital Canvas");
                query.Parameters.AddWithValue("@f3", "Cardboard");
                query.Parameters.AddWithValue("@f4", "Wall/Street");
                query.Parameters.AddWithValue("@f5", "CD");
                query.Parameters.AddWithValue("@f6", "MIDI");
                query.Parameters.AddWithValue("@f7", "Digital");
                query.Parameters.AddWithValue("@f8", "Tape");
                query.Parameters.AddWithValue("@f9", "LP");
                query.Parameters.AddWithValue("@f10", "EP");
                query.Parameters.AddWithValue("@f11", "Gramophone");
                query.Prepare();

                query.ExecuteNonQuery();
            }
        }

        public void CheckUnits()
        {
            /** Check the MeasuringUnit TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, simply add them 
             *  */
            bool ValuesExist = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM MeasuringUnit ORDER BY unitID ASC";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                ValuesExist = true;
                break;
            }

            read.Close();

            if (!ValuesExist)
            {
                query.CommandText = @"
                                    INSERT INTO MeasuringUnit(unit)
                                    VALUES(@mu1);
                                    INSERT INTO MeasuringUnit(unit)
                                    VALUES(@mu2);
                                    INSERT INTO MeasuringUnit(unit)
                                    VALUES(@mu3);
                                    INSERT INTO MeasuringUnit(unit)
                                    VALUES(@mu4);
                                    INSERT INTO MeasuringUnit(unit)
                                    VALUES(@mu5);
                                    
";
                query.Parameters.AddWithValue("@mu1", "cm");
                query.Parameters.AddWithValue("@mu2", "mm");
                query.Parameters.AddWithValue("@mu3", "px");
                query.Parameters.AddWithValue("@mu4", "in");
                query.Parameters.AddWithValue("@mu5", "m");

                query.ExecuteNonQuery();
            }
        }

        public void CheckNoteLogTypes()
        {
            /** Check the NoteLogType TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, simply add them 
             *  */
            bool HasValues = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM NoteLogTypes ORDER BY noteLogTypeID ASC";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                HasValues = true;
                break;
            }

            read.Close();

            const string type = "noteLogType";

            if (!HasValues)
            {
                SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = $@"
                                    INSERT INTO NoteLogTypes({type})
                                    VALUES(@nt1);
                                    INSERT INTO NoteLogTypes({type})
                                    VALUES(@nt2);
                                    
";
                insert.Parameters.AddWithValue("@nt1", "GENERIC");
                insert.Parameters.AddWithValue("@nt2", "FLEXI");

                insert.ExecuteNonQuery();
            }
        }

        public void CheckFNCategories()
        {
            /** Check the FlexiNoteType TABLE and see if there are values. If there are values do not execute anything.
             *  If there are no values, simply add them 
             *  */
            bool HasValues = false;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = @"SELECT * FROM FlexiNoteType ORDER BY flexiNoteTypeID ASC";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                HasValues = true;
                break;
            }

            read.Close();

            if (!HasValues)
            {
                SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = @"
                                    INSERT INTO FlexiNoteType(flexiNoteType)
                                    VALUES(@fn1);
                                    INSERT INTO FlexiNoteType(flexiNoteType)
                                    VALUES(@fn2);
                                    INSERT INTO FlexiNoteType(flexiNoteType)
                                    VALUES(@fn3);
                                    
";
                insert.Parameters.AddWithValue("@fn1", "Music");
                insert.Parameters.AddWithValue("@fn2", "Gaming");
                insert.Parameters.AddWithValue("@fn3", "Document");

                insert.ExecuteNonQuery();
            }
        }







        #endregion


        /// <summary>
        /// Counts the total number of logs.
        /// </summary>
        /// <returns>The count of logs.</returns>
        public int LogCount()
        {
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = $@"SELECT * FROM LOG WHERE {Column.AccountID} = @account
;";
            query.Parameters.AddWithValue("@account", User.ID);

            SQLiteDataReader read = query.ExecuteReader();
            int count = 0;

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return count;
        }

        /// <summary>
        /// Counts the number of logs that are in a particular category.
        /// </summary>
        /// <param name="category">The category being enquired about.</param>
        /// <returns>The count of logs in the given category.</returns>
        public int LogCount(LOG.CATEGORY category)
        {
            SQLiteCommand query = _con.CreateCommand();
            try
            {
                query.CommandText = $@"SELECT * FROM LOG WHERE {Column.CategoryID} = @category
                                        AND {Column.AccountID} = @account
                                        AND {Column.AppID} NOT IN (@id1, @id2)
;";
                query.Parameters.AddWithValue("@category", FindCategoryID(category));
                query.Parameters.AddWithValue("@account", User.ID);
                query.Parameters.AddWithValue("@id1", 1);
                query.Parameters.AddWithValue("@id2", 2);

                SQLiteDataReader read = query.ExecuteReader();
                int count = 0;

                while (read.Read())
                {
                    ++count;
                }

                read.Close();

                return count;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Counts the number of Qt logs.
        /// </summary>
        /// <returns>The count of Qt logs ONLY.</returns>
        public int QtLogCount()
        {
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = $@"SELECT * FROM LOG WHERE {Column.CategoryID} = @category 
                                            AND {Column.AccountID} = @account
                                            AND {Column.AppID} = @app
;";
            query.Parameters.AddWithValue("@category", 1);
            query.Parameters.AddWithValue("@account", User.ID);
            query.Parameters.AddWithValue("@app", 1);

            SQLiteDataReader read = query.ExecuteReader();
            int count = 0;

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return count;
        }


        /// <summary>
        /// Counts the number of Android Studio logs.
        /// </summary>
        /// <returns>The count of Android Studio logs ONLY.</returns>
        public int ASLogCount()
        {
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = $@"SELECT * FROM LOG WHERE {Column.CategoryID} = @category 
                                            AND {Column.AccountID} = @account
                                            AND {Column.AppID} = @app
;";
            query.Parameters.AddWithValue("@category", 1);
            query.Parameters.AddWithValue("@account", User.ID);
            query.Parameters.AddWithValue("@app", 2);

            SQLiteDataReader read = query.ExecuteReader();
            int count = 0;

            while (read.Read())
            {
                ++count;
            }

            read.Close();

            return count;
        }

        /// <summary>
        /// Adds an account that is provided as an argument to the database.
        /// </summary>
        /// <param name="account">The account that will be added to the database.</param>
        /// <returns">A boolean value that indicated if the process was successful or not.</returns>
        public bool AddAccount(ACCOUNT account)
        {
            bool emailExists = false;
            bool accountCreated = false;

            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = "SELECT email FROM ACCOUNT WHERE email = @email;";
                    query.Parameters.AddWithValue("@email", account.Email);

                    SQLiteDataReader reader = query.ExecuteReader();
                    if (reader.Read())
                        emailExists = true;
                }

                if (emailExists)
                    return false;
                else
                {
                    string hashedPassword = SaltedSHA256Hash(account.Password, account.ID.ToString());

                    using (SQLiteCommand insert = _con.CreateCommand())
                    {
                        insert.CommandText = $@"INSERT INTO ACCOUNT({Column.AccountID}, profilepic, first, last, email, password, IsEmployee, companyName, companyAddress, companyLogo, online)
                                                        VALUES(@account, @profilePic, @firstName, @lastName, @email, @password, 
                                                            @isEmployee, @companyName, @companyAddress, @companyLogo, @online)
;";
                        insert.Parameters.AddWithValue("@account", account.ID);
                        insert.Parameters.AddWithValue("@profilePic", account.ProfilePic);
                        insert.Parameters.AddWithValue("@firstName", account.FirstName);
                        insert.Parameters.AddWithValue("@lastName", account.LastName);
                        insert.Parameters.AddWithValue("@email", account.Email);
                        insert.Parameters.AddWithValue("@password", hashedPassword);
                        insert.Parameters.AddWithValue("@isEmployee", account.IsEmployee);
                        insert.Parameters.AddWithValue("@companyName", account.CompanyName);
                        insert.Parameters.AddWithValue("@companyAddress", account.CompanyAddress);
                        insert.Parameters.AddWithValue("@companyLogo", account.CompanyLogo);
                        insert.Parameters.AddWithValue("@online", false);

                        insert.ExecuteNonQuery();
                    }

                    accountCreated = true;
                }
            }
            catch(SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite exception found: {sqlex.Message}");
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Debug.WriteLine($"Error adding account: {ex.Message}");
            }

            return accountCreated;
        }


        /// <summary>
        /// Adds an ApplicationClass provided as an argument to the database.
        /// </summary>
        /// <param name="app">The app that will be added to the database.</param>
        protected void AddApplication(ApplicationClass app)
        {
            try
            {
                using (SQLiteCommand insert = _con.CreateCommand())
                {
                    insert.CommandText = $@"INSERT INTO APPLICATION({Column.AppID}, {Column.AccountID}, application, {Column.CategoryID}, IsDefault)
                            VALUES(@app, @account, @appName, @category, @isDefault)
;";
                    insert.Parameters.AddWithValue("@app", app.AppID);
                    insert.Parameters.AddWithValue("@account", User.ID);
                    insert.Parameters.AddWithValue("@appName", app.Name);
                    insert.Parameters.AddWithValue("@category", FindCategoryID(app.Category));
                    insert.Parameters.AddWithValue("@isDefault", 0);

                    insert.ExecuteNonQuery();

                    if (Watcher.AppID > 0)
                        --Watcher.AppID;
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception near AddApplication(appClass): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near AddApplication(appClass): {ex.Message}");

                // TODO
            }
        }

        /// <summary>
        /// Adds a ProjectClass provided as an argument to the database.
        /// </summary>
        /// <param name="project">The project that will be added to the database.</param>
        private void AddProject(ProjectClass project)
        {
            try
            {

                // Ensure project is not already added for the specified application
                var existingProjects = ListProjects(project.Application);
                bool exists = false;

                foreach (ProjectClass item in existingProjects)
                {
                    if (item.Name.Equals(project.Name) &&
                        item.Application.Equals(project.Application) &&
                        item.Category.Equals(project.Category) &&
                        item.User.Equals(project.User)
                        )
                    {
                        exists = true;
                        break;
                    }
                }

                if (project.Application.AppID != 3 && !exists)
                {
                    using (SQLiteCommand insert = _con.CreateCommand())
                    {
                        insert.CommandText = $@"INSERT INTO PROJECT({Column.ProjectID}, {Column.AppID}, {Column.AccountID}, {Column.CategoryID}, {Column.project})
                                    VALUES(@id, @app, @account, @category, @project)
;";
                        insert.Parameters.AddWithValue("@id", project.ProjectID);
                        insert.Parameters.AddWithValue("@app", project.Application.AppID);
                        insert.Parameters.AddWithValue("@account", project.User.ID);
                        insert.Parameters.AddWithValue("@category", FindCategoryID(project.Category));
                        insert.Parameters.AddWithValue("@project", project.Name);

                        insert.ExecuteNonQuery();

                        // Reset the ProjectID watcher
                        if (Watcher.ProjectID > 0)
                            --Watcher.ProjectID;
                    }
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite error: {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding project: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a subject to the database if it doesn't exist already.
        /// </summary>
        /// <param name="subject">The subject object that will be added.</param>
        private void AddSubject(SubjectClass subject)
        {
            try
            {
                // Ensure subject is not already added for the specified project
                var existingProjects = ListSubjects(subject.Project);
                bool exists = false;

                foreach (SubjectClass item in existingProjects)
                {
                    if (item.Subject.Equals(subject.Subject) &&
                        item.Application.Equals(subject.Application) &&
                        item.Category.Equals(subject.Category) &&
                        item.User.Equals(subject.User)
                        )
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    using (SQLiteCommand insert = _con.CreateCommand())
                    {
                        insert.CommandText = $@"INSERT INTO Subject(subjectID, {Column.ProjectID}, {Column.AppID}, {Column.AccountID}, {Column.CategoryID}, subject)
                                    VALUES(@id, @project, @app, @account, @category, @subject)
;";
                        insert.Parameters.AddWithValue("@id", subject.SubjectID);
                        insert.Parameters.AddWithValue("@project", subject.Project.ProjectID);
                        insert.Parameters.AddWithValue("@app", subject.Application.AppID);
                        insert.Parameters.AddWithValue("@account", subject.User.ID);
                        insert.Parameters.AddWithValue("@category", FindCategoryID(subject.Category));
                        insert.Parameters.AddWithValue("@subject", subject.Subject);

                        insert.ExecuteNonQuery();

                        // Reset the SubjectID watcher
                        if (Watcher.SubjectID > 0)
                            --Watcher.SubjectID;
                    }
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException near AddSubject(subjectClass): {sqlex.Message}");

                // Delete the log in the database with the log ID that's currently stored in currentLogID.
                // Rollback Sqlite code
                if (currentLogID > 0)
                {
                    try
                    {
                        using (SQLiteCommand deleteLog = _con.CreateCommand())
                        {
                            deleteLog.CommandText = $"DELETE FROM LOG WHERE {Column.LogID} = @logID";
                            deleteLog.Parameters.AddWithValue("@logID", currentLogID);
                            deleteLog.ExecuteNonQuery();
                        }
                    }
                    catch (Exception deleteEx)
                    {
                        Debug.WriteLine($"Exception near AddSubject inner try-catch: {deleteEx.Message}");
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near AddSubject(subjectClass): {ex.Message}");

                // TODO
            }
        }

        /// <summary>
        /// Adds a CheckListItem to the Checklist table.
        /// </summary>
        /// <param name="id">The item's ID.</param>
        /// <param name="logID">The NotesLOG ID.</param>
        /// <param name="isChecked">Whether the item was checked or not.</param>
        /// <param name="item">The item that needed to be done.</param>
        protected void AddCheckListItem(int id, int logID, bool isChecked, string item)
        {
            using (SQLiteCommand insert = _con.CreateCommand())
            {
                insert.CommandText = @"INSERT INTO Checklist(itemsID, logID, item, done)
                               VALUES(@id, @logID, @item, @done)";
                insert.Parameters.AddWithValue("@id", id);
                insert.Parameters.AddWithValue("@logID", logID);
                insert.Parameters.AddWithValue("@item", item);
                insert.Parameters.AddWithValue("@done", isChecked);

                insert.ExecuteNonQuery();
            }
        }









        /// <summary>
        /// Finds an account's ID in the database.
        /// </summary>
        /// <param name="account">The account that is being searched for.</param>
        /// <returns>An account ID.</returns>
        public int FindAccountID(ACCOUNT account)
        {
            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT {Column.AccountID} FROM ACCOUNT WHERE email = @email
;";
                    query.Parameters.AddWithValue("@email", account.Email);

                    object result = query.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Debug.WriteLine($"Exception near FindAccountID(account): {ex.Message}");

                // TODO

                return -1;
            }
        }

        /// <summary>
        /// Finds an account in the database.
        /// </summary>
        /// <param name="id">The account ID for the account that is being searched for.</param>
        /// <returns>An account object with the specified ID.</returns>
        public ACCOUNT FindAccountByID(int id)
        {
            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT * FROM ACCOUNT WHERE {Column.AccountID} = @account;";
                    query.Parameters.AddWithValue("@account", id);

                    using (SQLiteDataReader read = query.ExecuteReader())
                    {
                        if (read.Read())
                        {
                            return new ACCOUNT(
                                read.GetInt32(Column.IDColumn),
                                read.GetString(1),
                                read.GetString(2),
                                read.GetString(3),
                                read.GetString(4),
                                read.GetString(5),
                                read.GetBoolean(6),
                                read.GetString(7),
                                read.GetString(8),
                                read.GetString(9),
                                read.GetBoolean(10)
                            );
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near FindAccountByID(id): {ex.Message}");

                // TODO

            }

            return null;
        }


        /// <summary>
        /// Finds an account in the database.
        /// </summary>
        /// <param name="emailAddress">Account's emails address.</param>
        /// <param name="password">The given password on login.</param>
        /// <returns>An account object.</returns>
        public ACCOUNT? FindAccountByEmail(string emailAddress, string password)
        {
            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = "SELECT * FROM ACCOUNT WHERE email = @email;";
                    query.Parameters.AddWithValue("@email", emailAddress);

                    using (SQLiteDataReader reader = query.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ACCOUNT? account = ParseAccount(reader);
                            if (VerifyPassword(account, password))
                            {
                                return account;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near FindAccountByEmail(email,password): {ex.Message}");

                // TODO
            }

            return null;
        }

        // Add similar error handling for other methods...

        private ACCOUNT? ParseAccount(SQLiteDataReader reader)
        {
            try
            {
                return new ACCOUNT(
                    reader.GetInt32(Column.IDColumn),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetBoolean(6),
                    reader.GetString(7),
                    reader.GetString(8),
                    reader.GetString(9),
                    reader.GetBoolean(10)
                );
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Debug.WriteLine($"Exception near ParseAccount(reader): {ex.Message}");

                // TODO
            }

            return null;
        }

        private bool VerifyPassword(ACCOUNT account, string password)
        {
            try
            {
                string hashedPassword = SaltedSHA256Hash(password, account.ID.ToString());
                return hashedPassword == account.Password;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near VerifyPassword(account,password): {ex.Message}");

                // TODO

            }

            return false;
        }


        /// <summary>
        /// Finds an category in the database associated with an application. Application is to be provided in the ApplicationClass parameter.
        /// </summary>
        /// <param name="app">The application that is associated with the category that is being looked for.</param>
        /// <returns>A category enum from the LOG class.</returns>
        /*public LOG.CATEGORY FindCategory(ApplicationClass app)
        {
            
            int cID = 0;
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = "SELECT * FROM APPLICATION
        ;";
            SQLiteDataReader read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetString(3) == app.Name)
                {
                    cID = read.GetInt32(2);
                    break;
                }
            }

            read.Close();

            query.CommandText = "SELECT * FROM CATEGORY ORDER BY categoryID;";
            read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetInt32(0) == cID)
                {
                    if (read.GetString(1) == app.Category.ToString().ToUpper())
                    {
                        switch (read.GetString(1))
                        {
                            case "CODING":
                                return LOG.CATEGORY.CODING;
                            case "GRAPHICS":
                                return LOG.CATEGORY.GRAPHICS;
                            case "FILM":
                                return LOG.CATEGORY.FILM;
                            case "NOTES":
                                return LOG.CATEGORY.NOTES;
                        }
                    }
                }
            }

            read.Close();

            return LOG.CATEGORY.CODING;
        }*/


        /// <summary>
        /// Finds a category ID in the database for the LOG category enum provided.
        /// </summary>
        /// <param name="category">The category enum that the ID required is associated with.</param>
        /// <returns>An integer value representing an ID.</returns>
        public int FindCategoryID(LOG.CATEGORY category)
        {
            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT {Column.CategoryID} FROM CATEGORY WHERE {Column.category} = @categoryName;";
                    query.Parameters.AddWithValue("@categoryName", category.ToString());

                    object result = query.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Debug.WriteLine($"Exception found near FindCategoryID: {ex.Message}");
            }

            return -1;
        }

        /** Will find an APPLICATION's ID and return both the ID.
        *   It will automatically add an APPLICATION if it doesn't exist.
        *   */
        public int FindAppID(ApplicationClass app)
        {
            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT {Column.AppID} FROM APPLICATION WHERE {Column.app} = @appName
                                                        AND {Column.CategoryID} = @category
                                                        AND {Column.AccountID} IN (@id1, @id2)
;";
                    query.Parameters.AddWithValue("@appName", app.Name);
                    query.Parameters.AddWithValue("@category", FindCategoryID(app.Category));
                    query.Parameters.AddWithValue("@id1", 1);
                    query.Parameters.AddWithValue("@id2", app.User.ID);

                    object result = query.ExecuteScalar();

                    if (result != null)
                        return Convert.ToInt32(result);
                    else
                    {
                        if (!AppAlreadyAdded)
                        {
                            AddApplication(app);
                            AppAlreadyAdded = true;
                            return FindAppID(app);
                        }

                    }
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception found near FindAppID: {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindAppID: {ex.Message}");

                // TODO
            }

            return -1;
        }

        /** Will find an APPLICATION's ID and return both the ID.
        *   It will NOT add an APPLICATION if it doesn't exist.
        *   */
        public int FindAppID(ApplicationClass app, bool appMustBeAdded)
        {
            try
            {
                bool AlreadyAdded = false;
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT {Column.AppID} FROM APPLICATION WHERE {Column.app} = @appName
                                                        AND {Column.CategoryID} = @category
                                                        AND {Column.AccountID} = @account
;";
                    query.Parameters.AddWithValue("@appName", app.Name);
                    query.Parameters.AddWithValue("@category", FindCategoryID(app.Category));
                    query.Parameters.AddWithValue("@account", app.User.ID);

                    object result = query.ExecuteScalar();

                    if (result != null)
                        return Convert.ToInt32(result);
                    else
                    {
                        if (!AlreadyAdded && appMustBeAdded)
                        {
                            AddApplication(app);
                            AlreadyAdded = true;
                            return FindAppID(app);
                        }

                    }
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception found near FindAppID: {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindAppID: {ex.Message}");

                // TODO
            }

            return -1;
        }


        /** Will find a PROJECT's ID and return the ID of the PROJECT.
         *  This function will automatically add a PROJECT if it doesn't exist and return the new project's ID.
         *  */
        public int FindProjectID(ProjectClass project)
        {
            if (string.IsNullOrEmpty(project.Name) || project.Name == "Unknown")
                return 1;


            try
            {

                // Retrieve APPLICATION ID
                int appID = FindAppID(project.Application);

                // If application ID is not 3, perform search and add project if not found
                if (appID != 3)
                {
                    using (SQLiteCommand query = _con.CreateCommand())
                    {
                        query.CommandText = $@"SELECT {Column.ProjectID} FROM PROJECT 
                                      WHERE {Column.AppID} = @app
                                        AND {Column.AccountID} = @account
                                        AND {Column.CategoryID} = @category
                                        AND {Column.project} = @project
;";

                        query.Parameters.AddWithValue("@app", project.Application.AppID);
                        query.Parameters.AddWithValue("@account", project.User.ID);
                        query.Parameters.AddWithValue("@category", FindCategoryID(project.Category));
                        query.Parameters.AddWithValue("@project", project.Name);

                        object result = query.ExecuteScalar();

                        if (result != null)
                            return Convert.ToInt32(result);
                        else
                        {
                            if (!ProjectAlreadyAdded)
                            {
                                // Project not found, add it to the database
                                AddProject(project);
                                ProjectAlreadyAdded = true;

                                // Recursively find the project ID after adding it
                                return FindProjectID(project);
                            }
                        }
                    }
                }

            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception near FindProjectID(projectClass): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near FindProjectID(projectClass): {ex.Message}");
            }

            return 1;
        }



        /** Will find a PROJECT's ID and return the ID of the PROJECT.
         *  This function will NOT add a PROJECT if it doesn't exist.
         *  */
        public int FindProjectID(ProjectClass project, bool projectMustBeAdded)
        {
            if (string.IsNullOrEmpty(project.Name) || project.Name == "Unknown")
                return 1;

            bool AlreadyAdded = false;

            try
            {

                // Retrieve APPLICATION ID
                int appID = FindAppID(project.Application);

                // If application ID is not 3, perform search and add project if not found
                if (appID != 3)
                {
                    using (SQLiteCommand query = _con.CreateCommand())
                    {
                        query.CommandText = $@"SELECT {Column.ProjectID} FROM PROJECT 
                                      WHERE {Column.AppID} = @app
                                        AND {Column.AccountID} = @account
                                        AND {Column.CategoryID} = @category
                                        AND {Column.project} = @project
;";

                        query.Parameters.AddWithValue("@app", project.Application.AppID);
                        query.Parameters.AddWithValue("@account", project.User.ID);
                        query.Parameters.AddWithValue("@category", FindCategoryID(project.Category));
                        query.Parameters.AddWithValue("@project", project.Name);

                        object result = query.ExecuteScalar();

                        if (result != null)
                            return Convert.ToInt32(result);
                        else
                        {
                            if (!AlreadyAdded && projectMustBeAdded)
                            {
                                // Project not found, add it to the database
                                AddProject(project);
                                AlreadyAdded = true;

                                // Recursively find the project ID after adding it
                                return FindProjectID(project);
                            }
                        }
                    }
                }

            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception near FindProjectID(projectClass,projectMustBeAdded): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near FindProjectID(projectClass,projectMustBeAdded): {ex.Message}");

                // TODO
            }

            return 1;
        }

        public ProjectClass FindProjectByID(int id)
        {
            ProjectClass project = new ProjectClass();
            const string unknown = "Unknown";

            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT * FROM PROJECT WHERE {Column.ProjectID} = @id
;";
                    query.Parameters.AddWithValue("@id", id);

                    SQLiteDataReader read = query.ExecuteReader();
                    if (read.Read())
                    {
                        project.ProjectID = read.GetInt32(Column.IDColumn);
                        project.Name = read.GetString(Column.project);

                        project.IsDefault = project.Name.Equals(unknown, StringComparison.OrdinalIgnoreCase);

                        int accountID = read.GetInt32(Column.AccountID);
                        project.User = FindAccountByID(accountID);

                        int appID = read.GetInt32(Column.AppID);
                        project.Application = FindAppByID(appID);
                    }
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception near FindProjectByID(id): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near FindProjectByID(id): {ex.Message}");

                // TODO
            }

            return project;
        }


        public int FindOutputID(OutputClass output)
        {

            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = $@"SELECT outputID FROM OUTPUT WHERE {Column.CategoryID} = @category 
                                                            AND output = @output ORDER BY outputID ASC
;";
                    query.Parameters.AddWithValue("@category", FindCategoryID(output.Category));
                    query.Parameters.AddWithValue("@output", output.Name);

                    object result = query.ExecuteScalar();

                    if (result != null)
                        return Convert.ToInt32(result);
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception near FindOutputID(outputClass): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception near FindOutputID(outputClass): {ex.Message}");

                // TODO
            }

            return 37;
        }


        public OutputClass FindOutputByID(int id)
        {
            OutputClass output = new();

            try
            {
                using (SQLiteCommand query = _con.CreateCommand())
                {
                    query.CommandText = "SELECT * FROM OUTPUT WHERE outputID = @id ORDER BY outputID ASC;";
                    query.Parameters.AddWithValue("@id", id);

                    SQLiteDataReader reader = query.ExecuteReader();
                    if (reader.Read())
                    {
                        output.OutputID = reader.GetInt32(Column.IDColumn);
                        output.Name = reader.GetString(Column.output);
                        output.User = FindAccountByID(reader.GetInt32(Column.AccountID));
                        output.Application = FindAppByID(reader.GetInt32(Column.AppID));
                        output.Category = FindCategoryByID(reader.GetInt32(Column.CategoryID));
                    }
                }
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLite Exception: {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error finding output by ID: {ex.Message}");
            }

            return output;
        }


        public int FindTypeID(TypeClass type)
        {
            // Will store the type IDs
            int typeKey = 1, catNumber = 1;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            /** Retrieve TYPEs from the database temporarily.
             *  Look for the type that matches "type" parameter variable.
             *  Then, return the key of the type when it is found.
             */

            query.CommandText = $@"SELECT * FROM TYPE WHERE {Column.CategoryID} = @category 
                                                AND type = @type
;";
            query.Parameters.AddWithValue("@category", FindCategoryID(type.Category));
            query.Parameters.AddWithValue("@type", type.Name);
            read = query.ExecuteReader();


            while (read.Read())
            {
                typeKey = read.GetInt32(Column.IDColumn);
                break;
            }

            read.Close();

            return typeKey;
        }

        public TypeClass FindTypeByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            TypeClass type = new();

            query.CommandText = "SELECT * FROM TYPE WHERE typeID = @id ORDER BY typeID ASC;";
            query.Parameters.AddWithValue("@id", id);

            read = query.ExecuteReader();

            while (read.Read())
            {
                type.TypeID = read.GetInt32(0);
                type.Name = read.GetString(4);
                type.User = FindAccountByID(read.GetInt32(1));
                type.Application = FindAppByID(read.GetInt32(2));
                type.Category = FindCategoryByID(read.GetInt32(3));
            }

            read.Close();

            return type;
        }

        public int FindSubjectID(SubjectClass subject)
        {
            // Will store the type IDs
            int subjectKey = subject.SubjectID;

            var subjectColumn = 5;

            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            /** Retrieve Subjects from the database temporarily.
             *  Look for the subject that matches the "subject" parameter variable.
             *  Then, return the key of the type when it is found.
             */
            query.CommandText = $@"SELECT * FROM Subject WHERE {Column.CategoryID} = @category
                                    AND {Column.ProjectID} = @project
                                    AND {Column.AppID} = @app
                                    AND {Column.AccountID} = @account
;";
            query.Parameters.AddWithValue("@category", FindCategoryID(subject.Category));

            var projectID = subject.Project.ProjectID;
            query.Parameters.AddWithValue("@project", projectID);

            var appID = subject.Application.AppID;
            query.Parameters.AddWithValue("@app", appID);

            var userID = subject.User.ID;
            query.Parameters.AddWithValue("@account", userID);

            read = query.ExecuteReader();

            while (read.Read())
            {
                if (read.GetString(subjectColumn) == subject.Subject)
                {
                    subjectKey = read.GetInt32(Column.IDColumn);
                    SubjectAlreadyAdded = true;
                    break;
                }
            }

            read.Close();

            if (!SubjectAlreadyAdded)
            {
                AddSubject(subject);
                FindSubjectID(subject);
            }

            return subjectKey;
        }

        public SubjectClass FindSubjectByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            SubjectClass subject = new();
            subject.SubjectID = -1;

            query.CommandText = "SELECT * FROM Subject WHERE subjectID = @id ORDER BY subjectID ASC;";
            query.Parameters.AddWithValue("@id", id);

            read = query.ExecuteReader();

            while (read.Read())
            {
                subject.SubjectID = read.GetInt32(0);
                subject.Project = FindProjectByID(read.GetInt32(1));
                subject.Application = FindAppByID(read.GetInt32(2));
                subject.User = FindAccountByID(read.GetInt32(3));
                subject.Category = FindCategoryByID(read.GetInt32(4));
                subject.Subject = read.GetString(5);
            }

            read.Close();

            return subject;
        }

        public string? FindMediumByID(int id)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;
                string? medium = null;

                var mediumColumn = 2;

                query.CommandText = "SELECT * FROM MEDIUM WHERE mediumID = @id ORDER BY mediumID ASC;";
                query.Parameters.AddWithValue("@id", id);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    medium = read.GetString(mediumColumn);
                }

                read.Close();

                return medium;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindMediumByID(id): {sqlex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindMediumByID(id): {ex.Message}");
                // TODO
                return null;
            }
        }

        public string FindFormatByID(int id)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;
                string? format = null;

                var formatColumn = 2;

                query.CommandText = "SELECT * FROM FORMAT WHERE formatID = @id ORDER BY formatID ASC;";
                query.Parameters.AddWithValue("@id", id);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    format = read.GetString(formatColumn);
                }

                read.Close();

                return format;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindFormatByID(id): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near FindFormatByID(id): {ex.Message}");
                // TODO

            }

                return null;
            }

        public FLEXINOTEType FindFlexiNoteTypeByID(int id)
        {
            try
            {
                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;
                FLEXINOTEType flexiNoteType = new();

                var flexiNoteTypeColumn = 2;

                query.CommandText = "SELECT * FROM FlexiNoteType WHERE flexiNoteTypeID = @id ORDER BY flexiNoteTypeID ASC;";
                query.Parameters.AddWithValue("@id", id);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    var type = read.GetString(flexiNoteTypeColumn);
                    Enum.TryParse(type, out flexiNoteType);
                }

                read.Close();

                return flexiNoteType;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindFlexiNoteTypeByID(id): {sqlex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found found near FindFlexiNoteTypeByID(id): {ex.Message}");
            }

                return new();
        }


        public LOG.CATEGORY FindCategoryByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            LOG.CATEGORY category = new();

            query.CommandText = $@"SELECT * FROM CATEGORY WHERE {Column.CategoryID} = @category;";
            query.Parameters.AddWithValue("@category", id);

            try
            {
                read = query.ExecuteReader();

                while (read.Read())
                {
                    if (read.GetInt32(Column.IDColumn) == id)
                    {
                        string categoryName = read.GetString(1);
                        var IsOkay = Enum.TryParse(categoryName, out category);

                        if (!IsOkay)
                            throw new InvalidCastException();
                        break;
                    }

                }

                read.Close();
            }
            catch (InvalidCastException)
            {
                SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = @"INSERT INTO FEEDBACK(feedTypeID, {Column.AccountID}, date, description, CanContact)
                                        VALUES(@feed, @account, @date_submitted, @desc, @contact)";
                insert.Parameters.AddWithValue("@feed", 1);
                insert.Parameters.AddWithValue("@account", User.ID);
                insert.Parameters.AddWithValue("@date_submitted", DateTime.Now.ToString("d MMMM yyyy HH:mm:ss"));
                insert.Parameters.AddWithValue("@desc", "A bug occurred where the incorrect enum was return or the enum parser was unable to parse the enum at all for whatever reason.");
                insert.Parameters.AddWithValue("@contact", 0);

                insert.ExecuteNonQuery();
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindCategoryByID(id): {sqlex.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near FindCategoryByID(id): {e.Message}");
                // TODO
            }

            return category;
        }

        /// <summary>
        /// Finds a NOTELOGType ID in the database for the enum provided.
        /// </summary>
        /// <param name="type">The type enum that the ID required is associated with.</param>
        /// <returns>An integer value representing a NOTELOGType ID.</returns>
        public int FindNoteLogTypeID(NOTELOGType type)
        {
            /** Retrieve Categories from the database.
             *  Look for the category that matches "category" parameter variable.
             *  Then, return the key of the category when it is found.
             *  Make sure to set the category from outside the DATAMASTER in case an application is new so that getCategory() can help the DM give it a category.
             */
            SQLiteCommand query = _con.CreateCommand();
            query.CommandText = "SELECT * FROM NoteLogTypes;";
            SQLiteDataReader read = query.ExecuteReader();
            int id = -1;
            var noteLogTypeColumn = 1;

            while (read.Read())
            {
                if (read.GetString(noteLogTypeColumn) == type.ToString())
                {
                    id = read.GetInt32(Column.IDColumn);
                    break;
                }
            }

            read.Close();

            return id;
        }

        public NOTELOGType FindNoteLogTypeByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            NOTELOGType type = new();

            var noteLogTypeColumn = 1;

            query.CommandText = "SELECT * FROM NoteLogTypes WHERE noteLogTypeID = @id;";
            query.Parameters.AddWithValue("@id", id);

            try
            {
                read = query.ExecuteReader();

                while (read.Read())
                {
                    string typeName = read.GetString(noteLogTypeColumn);
                    var IsOkay = Enum.TryParse(typeName, out type);

                    break;

                }

                read.Close();
            }
            catch (InvalidCastException)
            {
                SQLiteCommand insert = _con.CreateCommand();
                insert.CommandText = $@"INSERT INTO FEEDBACK(feedTypeID, {Column.AccountID}, date, description, CanContact)
                                        VALUES(@feed, @account, @date_submitted, @desc, @contact)";
                insert.Parameters.AddWithValue("@feed", 1);
                insert.Parameters.AddWithValue("@account", User.ID);
                insert.Parameters.AddWithValue("@date_submitted", DateTime.Now.ToString("d MMMM yyyy HH:mm:ss"));
                insert.Parameters.AddWithValue("@desc", "A bug occurred where the incorrect enum was return or the enum parser was unable to parse the enum at all for whatever reason.");
                insert.Parameters.AddWithValue("@contact", 0);

                insert.ExecuteNonQuery();
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found near FindNoteLogTypeByID(id): {sqlex.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found near FindNoteLogTypeByID(id): {e.Message}");
                
                // TODO


            }

            return type;
        }

        public ApplicationClass? FindAppByID(int id)
        {
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            ApplicationClass? app = null;

            var categoryColumn = 3;
            var accountColumn = 1;
            var appColumn = 2;
            var isDefaultColumn = 4;

            query.CommandText = $@"SELECT * FROM APPLICATION WHERE {Column.AppID} = @app
;";
            query.Parameters.AddWithValue("@app", id);
            read = query.ExecuteReader();

            while (read.Read())
            {
                var account = read.GetBoolean(isDefaultColumn) ? FindAccountByID(1) : FindAccountByID(read.GetInt32(accountColumn));
                app = new ApplicationClass(read.GetInt32(Column.IDColumn), account, read.GetString(appColumn), FindCategoryByID(read.GetInt32(categoryColumn)),
                                read.GetBoolean(isDefaultColumn));
                break;
            }

            read.Close();

            return app;
        }






        /* List Items */



        /// <summary>
        /// List ALL applications by the logged in user.
        /// </summary>
        /// <returns>A list of ApplicationClass objects.</returns>
        public List<ApplicationClass>? ListApplications()
        {
            /** Retrieve the APPLICATIONs from the database, 
             *  add them to a list 
             *  and return the list. */
            try
            {
                List<ApplicationClass>? apps = new();
                SQLiteCommand query = _con.CreateCommand();
                SQLiteDataReader read;

                var categoryColumn = 3;
                var accountColumn = 1;
                var appColumn = 2;
                var isDefaultColumn = 4;

                query.CommandText = $@"SELECT * FROM APPLICATION WHERE {Column.AccountID} IN (@id1, @id2) ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC
;";
                query.Parameters.AddWithValue("@id1", User.ID);
                query.Parameters.AddWithValue("@id2", 1);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    apps.Add(new(read.GetInt32(Column.IDColumn), FindAccountByID(read.GetInt32(accountColumn)), read.GetString(appColumn), FindCategoryByID(read.GetInt32(categoryColumn)),
                                read.GetBoolean(isDefaultColumn)));
                }

                read.Close();

                return apps;
            }
            catch (SQLiteException sqlex)
            {
                Debug.WriteLine($"SQLiteException found: {sqlex.Message}");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception found: {e.Message}");

                // TODO

                return null;
            }
        }

        /// <summary>
        /// List all applications in a particular LOG category.
        /// </summary>
        /// <param name="category">The log category.</param>
        /// <returns>A list of ApplicationClass objects.</returns>
        public List<ApplicationClass> ListApplications(LOG.CATEGORY category)
        {
            /** Retrieve the APPLICATIONs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<ApplicationClass> apps = new();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            var accountColumn = 1;
            var appColumn = 2;
            var isDefaultColumn = 4;

            query.CommandText = $@"SELECT * FROM APPLICATION WHERE {Column.AccountID} IN (@id1, @id2) 
                                            AND {Column.CategoryID} = @category ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC
;";
            query.Parameters.AddWithValue("@id1", User.ID);
            query.Parameters.AddWithValue("@id2", 1);
            query.Parameters.AddWithValue("@category", FindCategoryID(category));

            read = query.ExecuteReader();

            while (read.Read())
            {
                apps.Add(new(read.GetInt32(Column.IDColumn), FindAccountByID(read.GetInt32(accountColumn)), read.GetString(appColumn), category, read.GetBoolean(isDefaultColumn)));
            }

            read.Close();

            return apps;
        }

        /// <summary>
        /// Lists ALL the projects from the database.
        /// </summary>
        /// <returns>A list of ProjectClass objects.</returns>
        public List<ProjectClass>? ListProjects()
        {
            /** Retrieve the PROJECTs from the database, 
             *  add them to a list 
             *  and return the list. */

            List<ProjectClass>? projects = new List<ProjectClass>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            try
            {

                query.CommandText = $@"SELECT * FROM PROJECT WHERE {Column.AccountID} IN (@id1, @id2) ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC, {Column.ProjectID} ASC
;";
                query.Parameters.AddWithValue("@id1", User.ID);
                query.Parameters.AddWithValue("@id2", 1);

                read = query.ExecuteReader();

                while (read.Read())
                {
                    projects.Add(FindProjectByID(read.GetInt32(Column.IDColumn)));
                }
                read.Close();

                return projects;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Lists the projects from the database and filters it by the category enum argument provided. Projects may or may not be unique.
        /// </summary>
        /// <param name="category">The category the projects fall under.</param>
        /// <returns>A list of ProjectClass objects from the specified category.</returns>
        public List<ProjectClass> ListProjects(LOG.CATEGORY category)
        {
            /** Retrieve the PROJECTs from the database, 
             *  add them to a list 
             *  and return the list. */

            List<ProjectClass> projects = new List<ProjectClass>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;


            query.CommandText = $@"SELECT * FROM PROJECT WHERE {Column.AccountID} IN (@id1, @id2)
                                            AND {Column.CategoryID} = @category ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC, {Column.ProjectID} ASC
;";
            query.Parameters.AddWithValue("@id1", User.ID);
            query.Parameters.AddWithValue("@id2", 1);
            query.Parameters.AddWithValue("@category", FindCategoryID(category));

            read = query.ExecuteReader();

            while (read.Read())
            {
                projects.Add(FindProjectByID(read.GetInt32(Column.IDColumn)));
            }

            read.Close();

            return projects;
        }

        /// <summary>
        /// Lists the projects from the database and filters it by the app argument provided. Projects are all unique.
        /// </summary>
        /// <param name="app">The app from which the projects are created in.</param>
        /// <returns>A list of ProjectClass objects created in the app.</returns>
        public List<ProjectClass> ListProjects(ApplicationClass app)
        {


            List<ProjectClass> projects = new List<ProjectClass>();
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;
            bool Exists;

            query.CommandText = $@"SELECT * FROM PROJECT WHERE {Column.AccountID} IN (@id1, @id2) 
                                            AND {Column.AppID} = @app
                                            AND {Column.CategoryID} = @category 
                                            ORDER BY {Column.CategoryID} ASC, {Column.AppID} ASC, {Column.ProjectID} ASC
;";
            query.Parameters.AddWithValue("@id1", User.ID);
            query.Parameters.AddWithValue("@id2", 1);
            query.Parameters.AddWithValue("@app", app.AppID);
            query.Parameters.AddWithValue("@category", FindCategoryID(app.Category));

            read = query.ExecuteReader();

            while (read.Read())
            {
                var project = FindProjectByID(read.GetInt32(Column.IDColumn));
                Exists = false;

                foreach (ProjectClass pro in projects)
                {
                    if (pro.Name == project.Name && pro.Application == project.Application)
                        Exists = true;
                }

                if (!Exists)
                    projects.Add(project);
            }
            read.Close();

            return projects;
        }




        public List<SubjectClass> ListSubjects(LOG.CATEGORY category)
        {
            /** Retrieve the SUBJEECTs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<SubjectClass> subjects = new();

            // Get the logIDs from the PostIt table
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            var categoryColumn = 4;
            var accountColumn = 3;
            var appColumn = 2;
            var projectColumn = 1;
            var subjectColumn = 5;

            // In ACCOUNT
            query.CommandText = $@"SELECT * FROM Subject WHERE {Column.AccountID} IN (@id1, @id2) 
                                            AND {Column.CategoryID} = @category ORDER BY {Column.AccountID} ASC
;";
            query.Parameters.AddWithValue("@id1", User.ID);
            query.Parameters.AddWithValue("@id2", 1);
            query.Parameters.AddWithValue("@category", FindCategoryID(category));

            read = query.ExecuteReader();

            while (read.Read())
            {
                subjects.Add(new SubjectClass(read.GetInt32(Column.IDColumn), FindCategoryByID(read.GetInt32(categoryColumn)), FindAccountByID(read.GetInt32(accountColumn)),
                    read.GetString(subjectColumn), FindProjectByID(read.GetInt32(projectColumn)), FindAppByID(read.GetInt32(appColumn))));
            }

            read.Close();


            return subjects;
        }

        public List<SubjectClass> ListSubjects(ProjectClass project)
        {
            /** Retrieve the SUBJEECTs from the database, 
             *  add them to a list 
             *  and return the list. */
            List<SubjectClass> subjects = new();

            // Get the logIDs from the PostIt table
            SQLiteCommand query = _con.CreateCommand();
            SQLiteDataReader read;

            var categoryColumn = 4;
            var accountColumn = 3;
            var appColumn = 2;
            var projectColumn = 1;
            var subjectColumn = 5;


            query.CommandText = $@"SELECT * FROM Subject WHERE {Column.CategoryID} = @category
                                            AND {Column.AccountID} = @id
                                            AND {Column.AppID} = @app
                                            AND {Column.ProjectID} = @project ORDER BY {Column.AccountID} ASC;";
            query.Parameters.AddWithValue("@category", FindCategoryID(project.Category));
            query.Parameters.AddWithValue("@id", project.User.ID);
            query.Parameters.AddWithValue("@app", project.Application.AppID);
            query.Parameters.AddWithValue("@project", project.ProjectID);
            read = query.ExecuteReader();

            while (read.Read())
            {
                subjects.Add(new SubjectClass(read.GetInt32(Column.IDColumn), FindCategoryByID(read.GetInt32(categoryColumn)),
                    FindAccountByID(read.GetInt32(accountColumn)), read.GetString(subjectColumn), FindProjectByID(read.GetInt32(projectColumn)),
                    FindAppByID(read.GetInt32(appColumn))));
            }

            read.Close();


            return subjects;
        }


        // SecureString Support
        #region Helper Functions


        public static string SaltedSHA256Hash(string value, string accountID)
        {
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;

                // The account account is the salt. 
                // So 2 users with the same password have different hashes. 
                // For example, if someone knows their own hash, they can't see who has the same password.
                string input = value + accountID;
                byte[] result = hash.ComputeHash(enc.GetBytes(input));

                StringBuilder hashedStringBuilder = new StringBuilder();
                foreach (byte b in result)
                {
                    hashedStringBuilder.Append(b.ToString("x2"));
                }

                return hashedStringBuilder.ToString();
            }
        }

        #endregion

        // ---END MEMBER FUNCTIONS






    }



    /// <summary>
    /// A class to keep IDs from duplicating during log creations.
    /// </summary>
    public class IDWatcher
    {
        /// <summary>
        /// Keeps an eye on the number of logs that haven't been added to the database so the correct ID can be generated by the DATAWRITER.
        /// </summary>
        public int LogID { get; set; } = 0;

        /// <summary>
        /// Keeps an eye on the number of apps that haven't been added to the database so the correct ID can be generated by the DATAWRITER.
        /// </summary>
        public int AppID { get; set; } = 0;

        /// <summary>
        /// Keeps an eye on the number of projects that haven't been added to the database so the correct ID can be generated by the DATAWRITER.
        /// </summary>
        public int ProjectID { get; set; } = 0;

        /// <summary>
        /// Keeps an eye on the number of outputs that haven't been added to the database so the correct ID can be generated by the DATAWRITER.
        /// </summary>
        public int OutputID { get; set; } = 0;

        /// <summary>
        /// Keeps an eye on the number of types that haven't been added to the database so the correct ID can be generated by the DATAWRITER.
        /// </summary>
        public int TypeID { get; set; } = 0;

        /// <summary>
        /// Keeps an eye on the number of PostIts that haven't been added to the database so the correct ID can be generated by the DATAWRITER.
        /// </summary>
        public int PostItID { get; set; } = 0;

        /// <summary>
        /// Keeps an eye on the number of subjects that haven't not been added to the databasee so the correct ID can be generated by the DATAWRITER.
        /// </summary>
        public int SubjectID { get; set; } = 0;

        /// <summary>
        /// Keeps an eye on the number of checklist items that haven't not been added to the databasee so the correct ID can be generated by the DATAWRITER.
        /// </summary>
        public int ChecklistItemID { get; set; } = 0;


        /// <summary>
        /// Keeps unused PostIt IDs.
        /// </summary>
        public List<int>? AvailablePostItIDs { get; set; } = null;

        public List<int>? AvailableSubjectIDs { get; set; } = null;

        public IDWatcher() { }

        public IDWatcher(int logID, int appID, int projectID, int outputID, int typeID, int postItID, int subjectID, int checklistItemID)
        {
            LogID = logID;
            AppID = appID;
            ProjectID = projectID;
            OutputID = outputID;
            TypeID = typeID;
            PostItID = postItID;
            SubjectID = subjectID;
            ChecklistItemID = checklistItemID;
        }


        /// <summary>
        /// Retrieves the number of relevant items on start up to prevent ID discrepancies.
        /// </summary>
        /// <param name="CachedItems">The number of cached log items.</param>
        /// <param name="CachedPostIts">The post it count within the cached items.</param>
        public void UpdateOnStart(int CachedItems, int CachedPostIts)
        {
            LogID = CachedItems;
            AppID = CachedItems;
            ProjectID = CachedItems;

            PostItID = CachedPostIts;
            SubjectID = CachedPostIts;
        }

        public override bool Equals(object? obj)
        {
            return obj is IDWatcher watcher &&
                   LogID == watcher.LogID &&
                   AppID == watcher.AppID &&
                   ProjectID == watcher.ProjectID &&
                   OutputID == watcher.OutputID &&
                   TypeID == watcher.TypeID &&
                   PostItID == watcher.PostItID &&
                   SubjectID == watcher.SubjectID &&
                   ChecklistItemID == watcher.ChecklistItemID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LogID, AppID, ProjectID, OutputID, TypeID, PostItID, SubjectID, ChecklistItemID);
        }

        public static bool operator ==(IDWatcher left, IDWatcher right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IDWatcher left, IDWatcher right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// A class for retrieving database column names for DATAMASTER.
    /// </summary>
    public class ColumnNAME
    {

        public int IDColumn { get; } = 0;

        public string AccountID { get; } = "accountID";

        public string CategoryID { get; } = "categoryID";

        public string AppID { get; } = "appID";

        public string ProjectID { get; } = "projectID";

        public string OutputID { get; } = "outputID";

        public string TypeID { get; } = "typeID";

        public string SubjectID { get; } = "subjectID";

        public string ChecklistItemID { get; } = "itemsID";

        public string category { get; } = "category";

        public string app { get; } = "application";

        public string project { get; } = "project";

        public string output { get; } = "output";

        public string type { get; } = "type";

        public string subject { get; } = "subject";

        public string checklistItem { get; } = "item";

        public string done { get; } = "done";

        public string isDefault { get; } = "IsDefault";
    }

}
