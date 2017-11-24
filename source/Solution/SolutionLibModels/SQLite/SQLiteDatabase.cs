namespace SolutionModelsLib.SQLite
{
    using System;
    using System.Data.SQLite;

    /// <summary>
    /// Wraps around a Sqlite database to provide core functionality,
    /// such as, create, open, close database etc...
    /// </summary>
    public class SQLiteDatabase
    {
        #region fields
        private string _DBFilePath = @".\";
        private string _DBFileName = "database.sqlite";

        private SQLiteConnection _Connection = null;

        private bool _EnforceForeignKeys = true;
        private bool _MutliThreadAccess = false;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class Constructor
        /// </summary>
        public SQLiteDatabase(string dbFileName = null,
                  bool enforceForeignKeys = true,
                  bool mutliThreadAccess = false)
        {
            if (string.IsNullOrEmpty(dbFileName) == false)
                _DBFileName = dbFileName;

            Status = "Not connected - no connection initialized.";

            ExtendendStatus = string.Empty;
            this.Exception = null;

            _EnforceForeignKeys = enforceForeignKeys;
            _MutliThreadAccess = mutliThreadAccess;
        }
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets the name and path of the SQLite file in which the tree
        /// model data is to be stored.
        /// </summary>
        public string DBFileNamePath
        {
            get
            {
                return System.IO.Path.Combine(_DBFilePath, _DBFileName);
            }
        }

        /// <summary>
        /// Gets whether Foreign Keys are:
        /// - Enforced in the SQLite database file
        ///   (wrong values will result in exception on update or insert) or
        ///   
        /// - NOT enforced
        ///   (wrong values are ignored by SQLite).
        /// </summary>
        public bool EnforceForeignKeys { get { return _EnforceForeignKeys; } }

        /// <summary>
        /// Gets whether the SQLite database file can be accessed from within multiple
        /// threads and/or connections or not.
        /// </summary>
        public bool MutliThreadAccess { get { return _MutliThreadAccess; } }

        /// <summary>
        /// Gets the Name of the SQLite file in which the tree
        /// model data is to be stored.
        /// </summary>
        public string DBFileName
        {
            get
            {
                return _DBFileName;
            }
        }

        /// <summary>
        /// Gets the Path of the SQLite file in which the tree
        /// model data is to be stored.
        /// </summary>
        public string DBFilePath
        {
            get
            {
                return _DBFilePath;
            }
        }

        /// <summary>
        /// Sets path and file name of the SQLite database file.
        /// 
        /// This function expects a complete path and file name to be present.
        /// An exception is thrown if the string cannot be split into path and file name.
        /// </summary>
        /// <param name="pathFileName"></param>
        public void SetFileNameAndPath(string pathFileName)
        {
            if (string.IsNullOrEmpty(pathFileName) == true)
                throw new ArgumentNullException("Path and name of database file cannot be null.");

            _DBFilePath = System.IO.Path.GetDirectoryName(pathFileName);
            _DBFileName = System.IO.Path.GetFileName(pathFileName);
        }

        /// <summary>
        /// Gets extended information on exceptions that might have
        /// occurred to reach the current status.
        /// </summary>
        public Exception Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a textual description of the current SQLite database status.
        /// </summary>
        public string Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets/sets additional error/state information (if any).
        /// </summary>
        public string ExtendendStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a connection object that can be uses
        /// to interact with an existing and open SQLite database.
        /// </summary>
        public SQLiteConnection Connection
        {
            get
            {
                return _Connection;
            }
        }

        /// <summary>
        /// Gets whether the database connection is currently established (open), or not (false).
        /// </summary>
        public bool ConnectionState
        {
            get
            {
                try
                {
                    if (_Connection != null)
                    {
                        if (_Connection.State == System.Data.ConnectionState.Open)
                            return true;
                    }

                    return false;
                }
                catch (Exception exp)
                {
                    Status = exp.Message;
                    this.Exception = exp;

                    throw new Exception(exp.Message);
                }
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Opens a connection to a SQLite database if there is none already open.
        /// 
        /// The previously existing database is deleted if <paramref name="overwriteFile"/>
        /// is true.
        /// </summary>
        /// <param name="overwriteFile"></param>
        public void OpenConnection(bool overwriteFile = false)
        {
            try
            {
                if (_Connection != null)
                {
                    if (_Connection.State == System.Data.ConnectionState.Open)
                        return;
                }
                else
                    ConstructConnection(overwriteFile);

                _Connection.Open();
            }
            catch (Exception exp)
            {
                Status = exp.Message;
                this.Exception = exp;

                throw new Exception(exp.Message);
            }
        }

        /// <summary>
        /// Closes any open connections to the SQLite database.
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (ConnectionState == true)
                    _Connection.Close();

                _Connection = null;
            }
            catch (Exception exp)
            {
                Status = exp.Message;
                this.Exception = exp;

                throw new Exception(exp.Message);
            }
        }

        #region Pragma UserVersion
        /// <summary>
        /// Gets the current user version of the currently
        /// opened database (or throws an exception if database was unavailable).
        /// </summary>
        /// <returns></returns>
        public long UserVersion()
        {
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(_Connection))
                {
                    cmd.CommandText = "pragma user_version;";
                    return (long)cmd.ExecuteScalar();
                }
            }
            catch (Exception exp)
            {
                Status = exp.Message;
                this.Exception = exp;

                throw new Exception(exp.Message);
            }
        }

        /// <summary>
        /// Method increases the current user version of the currently
        /// opened database (or throw an exception if database was unavailable).
        /// </summary>
        /// <returns></returns>
        public long UserVersionIncrease()
        {
            var version = UserVersion();

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(_Connection))
                {
                    cmd.CommandText = string.Format("pragma user_version = {0};", version + 1);
                    cmd.ExecuteNonQuery();
                }

                return UserVersion();
            }
            catch (Exception exp)
            {
                Status = exp.Message;
                this.Exception = exp;

                throw new Exception(exp.Message);
            }
        }
        #endregion Pragma UserVersion

        #region Pragma JournalMode
        /// <summary>
        /// Gets the current journal mode of the currently
        /// opened database (or throws an exception if database was unavailable).
        /// </summary>
        /// <returns></returns>
        public string JournalMode()
        {
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(_Connection))
                {
                    cmd.CommandText = "pragma journal_mode;";
                    var result = cmd.ExecuteScalar();

                    return result as string;           // default is "delete"
                }
            }
            catch (Exception exp)
            {
                Status = exp.Message;
                this.Exception = exp;

                throw new Exception(exp.Message);
            }
        }

        /// <summary>
        /// Method sets the journal mode of the currently
        /// opened database (or throws an exception if database was unavailable).
        /// </summary>
        /// <returns></returns>
        public void JournalMode(JournalMode journalMode)
        {
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(_Connection))
                {
                    // String is case-insensitive
                    cmd.CommandText = string.Format("pragma journal_mode = {0};", journalMode.ToString());
                    cmd.ExecuteNonQuery();
                }

                return;
            }
            catch (Exception exp)
            {
                Status = exp.Message;
                this.Exception = exp;

                throw new Exception(exp.Message);
            }
        }
        #endregion Pragma JournalMode

        private void ConstructConnection(bool overWriteFile = false)
        {
            var dbFileNamePath = DBFileNamePath;

            SQLiteConnectionStringBuilder connectString = new SQLiteConnectionStringBuilder();
            connectString.DataSource = dbFileNamePath;
            connectString.ForeignKeys = EnforceForeignKeys;
            connectString.JournalMode = GetJournalMode();

            _Connection = new SQLiteConnection(connectString.ToString());

            if (System.IO.File.Exists(dbFileNamePath) == false)
            {
                // Overwrites a file if it is already there
                SQLiteConnection.CreateFile(dbFileNamePath);

                Status = "Created New Database.";
            }
            else
            {
                if (overWriteFile == false)
                {
                    Status = "Using exsiting Database.";
                }
                else
                {
                    // Overwrites a file if it is already there
                    SQLiteConnection.CreateFile(dbFileNamePath);
                }
            }
        }

        /// <summary>
        /// Determines the journal model of the SQLite database - this is
        /// required to at conneciton/database open time.
        /// </summary>
        /// <returns></returns>
        private SQLiteJournalModeEnum GetJournalMode()
        {
            return (this.MutliThreadAccess ? SQLiteJournalModeEnum.Wal : SQLiteJournalModeEnum.Default);
        }
        #endregion methods
    }
}
