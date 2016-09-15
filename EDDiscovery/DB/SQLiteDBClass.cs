using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace EDDiscovery.DB
{
    // This class uses a monitor to ensure only one can be
    // active at any one time
    public class SQLiteTxnLockED : IDisposable
    {
        private static object _transactionLock = new object();
        private static System.Threading.Timer _locktimer;
        private bool _locktaken = false;
        private static ConcurrentDictionary<Thread, bool> _waitingthreads = new ConcurrentDictionary<Thread, bool>();
        private Thread _owningThread;
        public DbCommand _executingCommand;
        public bool _commandExecuting = false;
        private bool _isLongRunning = false;
        private string _commandText = null;
        private bool _longRunningLogged = false;

        #region Constructor and Destructor
        public SQLiteTxnLockED()
        {
        }

        ~SQLiteTxnLockED()
        {
            this.Dispose(false);
        }
        #endregion

        #region Opening and Disposal
        private static void DebugLongRunningOperation(object state)
        {
            WeakReference weakref = state as WeakReference;

            if (weakref != null)
            {
                SQLiteTxnLockED txnlock = weakref.Target as SQLiteTxnLockED;

                if (txnlock != null)
                {
                    txnlock._isLongRunning = true;

                    if (txnlock._commandExecuting)
                    {
                        if (txnlock._isLongRunning)
                        {
                            Trace.WriteLine($"The following command is taking a long time to execute:\n{txnlock._commandText}");
                        }
                        if (txnlock._owningThread == Thread.CurrentThread)
                        {
                            StackTrace trace = new StackTrace(1, true);
                            Trace.WriteLine(trace.ToString());
                        }
                    }
                    else
                    {
                        Trace.WriteLine($"The transaction lock has been held for a long time.");

                        if (txnlock._commandText != null)
                        {
                            Trace.WriteLine($"Last command to execute:\n{txnlock._commandText}");
                        }
                    }
                }
            }
        }

        public void BeginCommand(DbCommand cmd)
        {
            this._executingCommand = cmd;
            this._commandText = cmd.CommandText;
            this._commandExecuting = true;

            if (this._isLongRunning && !this._longRunningLogged)
            {
                this._isLongRunning = false;
                DebugLongRunningOperation(new WeakReference(this));
                this._longRunningLogged = true;
            }
        }

        public void EndCommand()
        {
            this._commandExecuting = false;
        }

        public void Open()
        {
            // Only take the lock once
            if (!_locktaken)
            {
                bool retry = false;

                do
                {
                    try
                    {
                        // Add our thread to the set of threads waiting
                        // for the lock
                        lock (_waitingthreads)
                        {
                            _waitingthreads[Thread.CurrentThread] = true;
                        }

                        Monitor.Enter(_transactionLock, ref _locktaken);
                        _owningThread = Thread.CurrentThread;
                        _waitingthreads[Thread.CurrentThread] = false;
                        _locktimer = new System.Threading.Timer(DebugLongRunningOperation, new WeakReference(this), 2000, Timeout.Infinite);
                    }
                    // Retry the lock if we are interrupted by
                    // a leaked lock being finalized
                    catch (ThreadInterruptedException)
                    {
                        if (_waitingthreads[Thread.CurrentThread] == false)
                        {
                            retry = true;
                        }
                    }
                }
                while (retry);

                GC.ReRegisterForFinalize(this);
            }
        }

        public void Close()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // disposing: true if Dispose() was called, false
        // if being finalized by the garbage collector
        protected void Dispose(bool disposing)
        {
            if (_locktaken)
            {
                if (Thread.CurrentThread == _owningThread)
                {
                    Monitor.Exit(_transactionLock);
                }
                else
                {
                    // This thread doesn't own the lock, so we need to create
                    // a new lock object and interrupt all waiting threads
                    Console.WriteLine("ERROR: Transaction Lock Leaked");

                    lock (_waitingthreads)
                    {
                        _transactionLock = new object();

                        foreach (var thread in _waitingthreads.Keys)
                        {
                            if (_waitingthreads[thread])
                            {
                                _waitingthreads[thread] = false;
                                thread.Interrupt();
                            }
                        }
                    }
                }

                _locktaken = false;
            }

            if (_locktimer != null)
            {
                _locktimer.Dispose();
                _locktimer = null;
            }
        }
        #endregion
    }

    // This class wraps a DbTransaction to work around
    // SQLite not using a monitor or mutex when locking
    // the database
    public class SQLiteTransactionED : DbTransaction
    {
        private SQLiteTxnLockED _transactionLock = null;

        public DbTransaction InnerTransaction { get; private set; }

        public SQLiteTransactionED(DbTransaction txn, SQLiteTxnLockED txnlock)
        {
            _transactionLock = txnlock;
            InnerTransaction = txn;
        }

        #region Overridden methods and properties passed to inner transaction
        protected override DbConnection DbConnection { get { return InnerTransaction.Connection; } }
        public override IsolationLevel IsolationLevel { get { return InnerTransaction.IsolationLevel; } }

        public override void Commit() { InnerTransaction.Commit(); }
        public override void Rollback() { InnerTransaction.Rollback(); }
        #endregion

        public void BeginCommand(DbCommand cmd)
        {
            _transactionLock.BeginCommand(cmd);
        }

        public void EndCommand()
        {
            _transactionLock.EndCommand();
        }

        // disposing: true if Dispose() was called, false
        // if being finalized by the garbage collector
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Close the transaction before closing the lock
                if (InnerTransaction != null)
                {
                    InnerTransaction.Dispose();
                    InnerTransaction = null;
                }

                if (_transactionLock != null)
                {
                    _transactionLock.Dispose();
                    _transactionLock = null;
                }
            }

            base.Dispose(disposing);
        }
    }

    // This class wraps a DbDataReader so it can take the
    // above transaction lock, and to work around SQLite
    // not using a monitor or mutex when locking the
    // database
    public class SQLiteDataReaderED : DbDataReader
    {
        // This is the wrapped reader
        protected DbDataReader InnerReader { get; set; }
        protected DbCommand _command;
        protected SQLiteTransactionED _transaction;
        protected SQLiteTxnLockED _txnlock;

        public SQLiteDataReaderED(DbCommand cmd, CommandBehavior behaviour, SQLiteTransactionED txn = null, SQLiteTxnLockED txnlock = null)
        {
            this._command = cmd;
            this.InnerReader = cmd.ExecuteReader(behaviour);
            this._transaction = txn;
            this._txnlock = txnlock;
        }

        protected void BeginCommand()
        {
            if (_transaction != null)
            {
                _transaction.BeginCommand(_command);
            }
            else if (_txnlock != null)
            {
                _txnlock.BeginCommand(_command);
            }
        }

        protected void EndCommand()
        {
            if (_transaction != null)
            {
                _transaction.EndCommand();
            }
            else if (_txnlock != null)
            {
                _txnlock.EndCommand();
            }
        }

        #region Overridden methods and properties passed to inner command
        public override int Depth { get { return InnerReader.Depth; } }
        public override int FieldCount { get { return InnerReader.FieldCount; } }
        public override bool HasRows { get { return InnerReader.HasRows; } }
        public override bool IsClosed { get { return InnerReader.IsClosed; } }
        public override int RecordsAffected { get { return InnerReader.RecordsAffected; } }
        public override int VisibleFieldCount { get { return InnerReader.VisibleFieldCount; } }
        public override object this[int ordinal] { get { return InnerReader[ordinal]; } }
        public override object this[string name] { get { return InnerReader[name]; } }
        public override bool GetBoolean(int ordinal) { return InnerReader.GetBoolean(ordinal); }
        public override byte GetByte(int ordinal) { return InnerReader.GetByte(ordinal); }
        public override char GetChar(int ordinal) { return InnerReader.GetChar(ordinal); }
        public override string GetDataTypeName(int ordinal) { return InnerReader.GetDataTypeName(ordinal); }
        public override DateTime GetDateTime(int ordinal) { return InnerReader.GetDateTime(ordinal); }
        public override decimal GetDecimal(int ordinal) { return InnerReader.GetDecimal(ordinal); }
        public override double GetDouble(int ordinal) { return InnerReader.GetDouble(ordinal); }
        public override Type GetFieldType(int ordinal) { return InnerReader.GetFieldType(ordinal); }
        public override float GetFloat(int ordinal) { return InnerReader.GetFloat(ordinal); }
        public override Guid GetGuid(int ordinal) { return InnerReader.GetGuid(ordinal); }
        public override short GetInt16(int ordinal) { return InnerReader.GetInt16(ordinal); }
        public override int GetInt32(int ordinal) { return InnerReader.GetInt32(ordinal); }
        public override long GetInt64(int ordinal) { return InnerReader.GetInt64(ordinal); }
        public override string GetName(int ordinal) { return InnerReader.GetName(ordinal); }
        public override string GetString(int ordinal) { return InnerReader.GetString(ordinal); }
        public override object GetValue(int ordinal) { return InnerReader.GetValue(ordinal); }
        public override bool IsDBNull(int ordinal) { return InnerReader.IsDBNull(ordinal); }
        public override int GetOrdinal(string name) { return InnerReader.GetOrdinal(name); }
        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) { return InnerReader.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length); }
        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) { return InnerReader.GetChars(ordinal, dataOffset, buffer, bufferOffset, length); }
        public override int GetValues(object[] values) { return InnerReader.GetValues(values); }
        public override DataTable GetSchemaTable() { return InnerReader.GetSchemaTable(); }
        #endregion

        public override System.Collections.IEnumerator GetEnumerator()
        {
            BeginCommand();
            foreach (object val in InnerReader)
            {
                EndCommand();
                yield return val;
                BeginCommand();
            }
            EndCommand();
        }

        public override bool NextResult()
        {
            BeginCommand();
            bool result = InnerReader.NextResult();
            EndCommand();
            return result;
        }

        public override bool Read()
        {
            BeginCommand();
            bool result = InnerReader.Read();
            EndCommand();
            return result;
        }

        public override void Close()
        {
            InnerReader.Close();
        }
    }

    // This class wraps a DbCommand so it can take the
    // above transaction wrapper, and to work around
    // SQLite not using a monitor or mutex when locking
    // the database
    public class SQLiteCommandED : DbCommand
    {
        // This is the wrapped transaction
        protected SQLiteTransactionED _transaction;

        public SQLiteCommandED(DbCommand cmd, DbTransaction txn = null)
        {
            InnerCommand = cmd;
            if (txn != null)
            {
                SetTransaction(txn);
            }
        }

        public DbCommand InnerCommand { get; set; }

        #region Overridden methods and properties passed to inner command
        public override string CommandText { get { return InnerCommand.CommandText; } set { InnerCommand.CommandText = value; } }
        public override int CommandTimeout { get { return InnerCommand.CommandTimeout; } set { InnerCommand.CommandTimeout = value; } }
        public override CommandType CommandType { get { return InnerCommand.CommandType; } set { InnerCommand.CommandType = value; } }
        protected override DbConnection DbConnection { get { return InnerCommand.Connection; } set { InnerCommand.Connection = value; } }
        protected override DbParameterCollection DbParameterCollection { get { return InnerCommand.Parameters; } }
        protected override DbTransaction DbTransaction { get { return _transaction; } set { SetTransaction(value); } }
        public override bool DesignTimeVisible { get { return InnerCommand.DesignTimeVisible; } set { InnerCommand.DesignTimeVisible = value; } }
        public override UpdateRowSource UpdatedRowSource { get { return InnerCommand.UpdatedRowSource; } set { InnerCommand.UpdatedRowSource = value; } }

        protected override DbParameter CreateDbParameter() { return InnerCommand.CreateParameter(); }
        public override void Cancel() { InnerCommand.Cancel(); }
        public override void Prepare() { InnerCommand.Prepare(); }
        #endregion

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            if (this._transaction != null)
            {
                // The transaction should already have the transaction lock
                return new SQLiteDataReaderED(this.InnerCommand, behavior, txn: this._transaction);
            }
            else
            {
                // Take the transaction lock for the duration of this command
                using (var txnlock = new SQLiteTxnLockED())
                {
                    txnlock.Open();
                    return new SQLiteDataReaderED(this.InnerCommand, behavior, txnlock: txnlock);
                }
            }
        }

        public override object ExecuteScalar()
        {
            if (this._transaction != null)
            {
                this._transaction.BeginCommand(this);
                // The transaction should already have the transaction lock
                object result = InnerCommand.ExecuteScalar();
                this._transaction.EndCommand();
                return result;
            }
            else
            {
                // Take the transaction lock for the duration of this command
                using (var txnlock = new SQLiteTxnLockED())
                {
                    txnlock.Open();
                    txnlock.BeginCommand(this);
                    object result = InnerCommand.ExecuteScalar();
                    txnlock.EndCommand();
                    return result;
                }
            }
        }

        public override int ExecuteNonQuery()
        {
            if (this._transaction != null)
            {
                this._transaction.BeginCommand(this);
                // The transaction should already have the transaction lock
                int result = InnerCommand.ExecuteNonQuery();
                this._transaction.EndCommand();
                return result;
            }
            else
            {
                // Take the transaction lock for the duration of this command
                using (var txnlock = new SQLiteTxnLockED())
                {
                    txnlock.Open();
                    txnlock.BeginCommand(this);
                    int result = InnerCommand.ExecuteNonQuery();
                    txnlock.EndCommand();
                    return result;
                }
            }
        }

        // disposing: true if Dispose() was called, false
        // if being finalized by the garbage collector
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (InnerCommand != null)
                {
                    InnerCommand.Dispose();
                    InnerCommand = null;
                }
            }

            base.Dispose(disposing);
        }

        protected void SetTransaction(DbTransaction txn)
        {
            // We only accept wrapped transactions in order to avoid deadlocks
            if (txn == null || txn is SQLiteTransactionED)
            {
                _transaction = (SQLiteTransactionED)txn;
                InnerCommand.Transaction = _transaction.InnerTransaction;
            }
            else
            {
                throw new InvalidOperationException(String.Format("Expected a {0}; got a {1}", typeof(SQLiteTransactionED).FullName, txn.GetType().FullName));
            }
        }
    }

    [Flags]
    public enum EDDSqlDbSelection
    {
        None = 0,
        EDDiscovery = 1,
        EDDUser = 2,
        EDDSystem = 4
    }


    public static partial class SQLiteDBClass
    {
        #region Private properties / fields
        private static Object lockDBInit = new Object();                    // lock to sequence construction
        private static DbProviderFactory DbFactory;
        #endregion

        #region Transitional properties
        public static EDDSqlDbSelection DefaultMainDatabase { get { return  EDDSqlDbSelection.EDDUser; } }
        public static EDDSqlDbSelection UserDatabase { get { return EDDSqlDbSelection.EDDUser; } }
        public static EDDSqlDbSelection SystemDatabase { get { return EDDSqlDbSelection.EDDSystem;  } }
        #endregion


        #region Database Initialization
        private static void InitializeDatabase()
        {
            string dbv4file = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDiscovery);
            string dbuserfile = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDUser);
            string dbsystemsfile = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDSystem);
            DbFactory = GetSqliteProviderFactory();

            try
            {
                if (File.Exists(dbv4file))
                {
                    SplitDataBase();
                }

                using (var conn = new SQLiteConnectionUser())
                {
                    UpgradeUserDB(conn);                                            // upgrade it
                }

                using (var conn = new SQLiteConnectionSystem())
                {
                    UpgradeSystemsDB(conn);                                            // upgrade it
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error creating data base file, Exception", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private static void ExecuteQuery(SQLiteConnectionED conn, string query)
        {
            using (DbCommand command = conn.CreateCommand(query))
                command.ExecuteNonQuery();
        }


        private static bool SplitDataBase()
        {
            string dbfile = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDiscovery);
            string dbuserfile = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDUser);
            string dbsystemsfile = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDSystem);

            try
            {
                if (!File.Exists(dbuserfile))
                {
                    File.Copy(dbfile, dbuserfile);
                }

                if (!File.Exists(dbsystemsfile))
                {
                    File.Copy(dbfile, dbsystemsfile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SplitDatabase error: " + ex.Message);
                MessageBox.Show(ex.StackTrace);
                return false;
            }
            return true;
        }

        private static bool UpgradeUserDB(SQLiteConnectionUser conn)
        {
            int dbver;
            try
            {
                ExecuteQuery(conn, "CREATE TABLE IF NOT EXISTS Register (ID TEXT PRIMARY KEY NOT NULL, ValueInt INTEGER, ValueDouble DOUBLE, ValueString TEXT, ValueBlob BLOB)");
                dbver = GetSettingInt("DBVer", 1, conn);        // use the constring one, as don't want to go back into ConnectionString code

                DropOldUserTables(conn);

                if (dbver < 2)
                    UpgradeUserDB2(conn);

                if (dbver < 4)
                    UpgradeUserDB4(conn);

                if (dbver < 7)
                    UpgradeUserDB7(conn);

                if (dbver < 8)
                    UpgradeUserDB8(conn);

                if (dbver < 9)
                    UpgradeUserDB9(conn);

                if (dbver < 10)
                    UpgradeUserDB10(conn);

                if (dbver < 11)
                    UpgradeUserDB11(conn);

                if (dbver < 12)
                    UpgradeUserDB12(conn);

                if (dbver < 14)
                    UpgradeUserDB14(conn);

                if (dbver < 16)
                    UpgradeUserDB16(conn);

                if (dbver < 17)
                    UpgradeUserDB17(conn);

                if (dbver < 18)
                    UpgradeUserDB18(conn);

                if (dbver < 101)
                    UpgradeUserDB101(conn);
                if (dbver < 102)
                    UpgradeUserDB102(conn);

                CreateUserDBTableIndexes();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UpgradeUserDB error: " + ex.Message);
                MessageBox.Show(ex.StackTrace);
                return false;
            }
        }

        private static bool UpgradeSystemsDB(SQLiteConnectionSystem conn)
        {
            int dbver;
            try
            {
                ExecuteQuery(conn, "CREATE TABLE IF NOT EXISTS Register (ID TEXT PRIMARY KEY NOT NULL, ValueInt INTEGER, ValueDouble DOUBLE, ValueString TEXT, ValueBlob BLOB)");
                dbver = GetSettingInt("DBVer", 1, conn);        // use the constring one, as don't want to go back into ConnectionString code

                DropOldSystemTables(conn);

                if (dbver < 2)
                    UpgradeSystemsDB2(conn);

                if (dbver < 6)
                    UpgradeSystemsDB6(conn);

                if (dbver < 11)
                    UpgradeSystemsDB11(conn);

                if (dbver < 15)
                    UpgradeSystemsDB15(conn);

                if (dbver < 17)
                    UpgradeSystemsDB17(conn);

                if (dbver < 19)
                    UpgradeSystemsDB19(conn);

                if (dbver < 20)
                    UpgradeSystemsDB20(conn);

                if (dbver < 100)
                    UpgradeSystemsDB101(conn);

                if (dbver < 102)
                    UpgradeSystemsDB102(conn);

                CreateSystemDBTableIndexes();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UpgradeSystemsDB error: " + ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

        private static void PerformUpgrade(SQLiteConnectionED conn, int newVersion, bool catchErrors, bool backupDbFile, string[] queries, Action doAfterQueries = null)
        {
            if (backupDbFile)
            {
                string dbfile = conn.DBFile;

                try
                {
                    File.Copy(dbfile, dbfile.Replace(".sqlite", $"{newVersion - 1}.sqlite"));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                    System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                }
            }

            try
            {
                foreach (var query in queries)
                {
                    ExecuteQuery(conn, query);
                }
            }
            catch (Exception ex)
            {
                if (!catchErrors)
                    throw;

                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show($"UpgradeDB{newVersion} error: " + ex.Message);
            }

            doAfterQueries?.Invoke();

            PutSettingInt("DBVer", newVersion, conn);
        }

        private static void UpgradeSystemsDB2(SQLiteConnectionED conn)
        {
            string query = "CREATE TABLE Systems (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , name TEXT NOT NULL COLLATE NOCASE , x FLOAT, y FLOAT, z FLOAT, cr INTEGER, commandercreate TEXT, createdate DATETIME, commanderupdate TEXT, updatedate DATETIME, status INTEGER, population INTEGER )";
            string query3 = "CREATE TABLE Distances (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL  UNIQUE , NameA TEXT NOT NULL , NameB TEXT NOT NULL , Dist FLOAT NOT NULL , CommanderCreate TEXT NOT NULL , CreateTime DATETIME NOT NULL , Status INTEGER NOT NULL )";
            string query5 = "CREATE INDEX DistanceName ON Distances (NameA ASC, NameB ASC)";

            PerformUpgrade(conn, 2, false, false, new[] { query, query3, query5 });
        }

        private static void UpgradeUserDB2(SQLiteConnectionED conn)
        {
            string query4 = "CREATE TABLE SystemNote (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Time DATETIME NOT NULL )";

            PerformUpgrade(conn, 2, false, false, new[] { query4 });
        }

        /*
        private static void UpgradeDB3(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN Note TEXT";
            PerformUpgrade(conn, 3, false, false, new[] { query1 });
        }
         */

        private static void UpgradeUserDB4(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN Note TEXT";
            PerformUpgrade(conn, 4, true, true, new[] { query1 });
        }

        private static void UpgradeSystemsDB6(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN id_eddb Integer";
            string query2 = "ALTER TABLE Systems ADD COLUMN faction TEXT";
            //string query3 = "ALTER TABLE Systems ADD COLUMN population Integer";
            string query4 = "ALTER TABLE Systems ADD COLUMN government_id Integer";
            string query5 = "ALTER TABLE Systems ADD COLUMN allegiance_id Integer";
            string query6 = "ALTER TABLE Systems ADD COLUMN primary_economy_id Integer";
            string query7 = "ALTER TABLE Systems ADD COLUMN security Integer";
            string query8 = "ALTER TABLE Systems ADD COLUMN eddb_updated_at Integer";
            string query9 = "ALTER TABLE Systems ADD COLUMN state Integer";
            string query10 = "ALTER TABLE Systems ADD COLUMN needs_permit Integer";
            string query11 = "DROP TABLE IF EXISTS Stations";
            string query12 = "CREATE TABLE Stations (id INTEGER PRIMARY KEY  NOT NULL ,system_id INTEGER, name TEXT NOT NULL ,  " +
                " max_landing_pad_size INTEGER, distance_to_star INTEGER, faction Text, government_id INTEGER, allegiance_id Integer,  state_id INTEGER, type_id Integer, " +
                "has_commodities BOOL DEFAULT (null), has_refuel BOOL DEFAULT (null), has_repair BOOL DEFAULT (null), has_rearm BOOL DEFAULT (null), " +
                "has_outfitting BOOL DEFAULT (null),  has_shipyard BOOL DEFAULT (null), has_blackmarket BOOL DEFAULT (null),   eddb_updated_at Integer  )";

            string query13 = "CREATE TABLE station_commodities (station_id INTEGER PRIMARY KEY NOT NULL, commodity_id INTEGER, type INTEGER)";
            string query14 = "CREATE INDEX station_commodities_index ON station_commodities (station_id ASC, commodity_id ASC, type ASC)";
            string query15 = "CREATE INDEX StationsIndex_ID  ON Stations (id ASC)";
            string query16 = "CREATE INDEX StationsIndex_system_ID  ON Stations (system_id ASC)";
            string query17 = "CREATE INDEX StationsIndex_system_Name  ON Stations (Name ASC)";

            PerformUpgrade(conn, 6, true, true, new[] {
                query1, query2, query4, query5, query6, query7, query8, query9, query10,
                query11, query12, query13, query14, query15, query16, query17 });
        }

        private static void UpgradeUserDB7(SQLiteConnectionED conn)
        {
            string query1 = "DROP TABLE IF EXISTS VisitedSystems";
            string query2 = "CREATE TABLE VisitedSystems(id INTEGER PRIMARY KEY  NOT NULL, Name TEXT NOT NULL, Time DATETIME NOT NULL, Unit Text, Commander Integer, Source Integer, edsm_sync BOOL DEFAULT (null))";
            string query3 = "CREATE TABLE TravelLogUnit(id INTEGER PRIMARY KEY  NOT NULL, type INTEGER NOT NULL, name TEXT NOT NULL, size INTEGER, path TEXT)";
            PerformUpgrade(conn, 7, true, true, new[] { query1, query2, query3 });
        }

        private static void UpgradeUserDB8(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN Map_colour INTEGER DEFAULT (-65536)";
            PerformUpgrade(conn, 8, true, true, new[] { query1 });
        }

        private static void UpgradeUserDB9(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE Objects (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , SystemName TEXT NOT NULL , ObjectName TEXT NOT NULL , ObjectType INTEGER NOT NULL , ArrivalPoint Float, Gravity FLOAT, Atmosphere Integer, Vulcanism Integer, Terrain INTEGER, Carbon BOOL, Iron BOOL, Nickel BOOL, Phosphorus BOOL, Sulphur BOOL, Arsenic BOOL, Chromium BOOL, Germanium BOOL, Manganese BOOL, Selenium BOOL NOT NULL , Vanadium BOOL, Zinc BOOL, Zirconium BOOL, Cadmium BOOL, Mercury BOOL, Molybdenum BOOL, Niobium BOOL, Tin BOOL, Tungsten BOOL, Antimony BOOL, Polonium BOOL, Ruthenium BOOL, Technetium BOOL, Tellurium BOOL, Yttrium BOOL, Commander  Text, UpdateTime DATETIME, Status INTEGER )";
            PerformUpgrade(conn, 9, true, true, new[] { query1 });
        }

        private static void UpgradeUserDB10(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE wanted_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, systemname TEXT UNIQUE NOT NULL)";
            PerformUpgrade(conn, 10, true, true, new[] { query1 });
        }

        private static void UpgradeSystemsDB11(SQLiteConnectionED conn)
        {
            //Default is Color.Red.ToARGB()
            string query1 = "ALTER TABLE Systems ADD COLUMN FirstDiscovery BOOL";
            PerformUpgrade(conn, 11, true, true, new[] { query1 });
        }

        private static void UpgradeUserDB11(SQLiteConnectionED conn)
        {
            string query2 = "ALTER TABLE Objects ADD COLUMN Landed BOOL";
            string query3 = "ALTER TABLE Objects ADD COLUMN terraform Integer";
            string query4 = "ALTER TABLE VisitedSystems ADD COLUMN Status BOOL";
            PerformUpgrade(conn, 11, true, true, new[] { query2, query3, query4 });
        }

        private static void UpgradeUserDB12(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE routes_expeditions (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT UNIQUE NOT NULL, start DATETIME, end DATETIME)";
            string query2 = "CREATE TABLE route_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, routeid INTEGER NOT NULL, systemname TEXT NOT NULL)";
            PerformUpgrade(conn, 12, true, true, new[] { query1, query2 });
        }


        private static bool UpgradeUserDB14(SQLiteConnectionED conn)
        {
            //Default is Color.Red.ToARGB()
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN X double";
            string query2 = "ALTER TABLE VisitedSystems ADD COLUMN Y double";
            string query3 = "ALTER TABLE VisitedSystems ADD COLUMN Z double";

            PerformUpgrade(conn, 14, true, true, new[] { query1, query2, query3 });
            return true;
        }

        private static void UpgradeSystemsDB15(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN versiondate DATETIME";
            string query2 = "UPDATE Systems SET versiondate = datetime('now')";

            PerformUpgrade(conn, 15, true, true, new[] { query1, query2 });
        }

        private static void UpgradeUserDB16(SQLiteConnectionED conn)
        {
            string query = "CREATE TABLE Bookmarks (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , StarName TEXT, x double NOT NULL, y double NOT NULL, z double NOT NULL, Time DATETIME NOT NULL, Heading TEXT, Note TEXT NOT Null )";
            PerformUpgrade(conn, 16, true, true, new[] { query });
        }

        private static void UpgradeSystemsDB17(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN id_edsm Integer";
            string query4 = "ALTER TABLE Distances ADD COLUMN id_edsm Integer";
            string query5 = "CREATE INDEX Distances_EDSM_ID_Index ON Distances (id_edsm ASC)";

            PerformUpgrade(conn, 17, true, true, new[] { query1,query4,query5 });
        }

        private static void UpgradeUserDB17(SQLiteConnectionED conn)
        {
            string query6 = "Update VisitedSystems set x=null, y=null, z=null where x=0 and y=0 and z=0 and name!=\"Sol\"";
            PerformUpgrade(conn, 17, true, true, new[] { query6 }, () =>
            {
                PutSettingString("EDSMLastSystems", "2010 - 01 - 01 00:00:00", conn);        // force EDSM sync..
                PutSettingString("EDDBSystemsTime", "0", conn);                               // force EDDB
                PutSettingString("EDSCLastDist", "2010-01-01 00:00:00", conn);                // force distances
            });
        }

        private static void UpgradeUserDB18(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN id_edsm_assigned Integer";
            string query2 = "CREATE INDEX VisitedSystems_id_edsm_assigned ON VisitedSystems (id_edsm_assigned)";
            string query3 = "CREATE INDEX VisitedSystems_position ON VisitedSystems (X, Y, Z)";

            PerformUpgrade(conn, 18, true, true, new[] { query1, query2, query3 });
        }

        private static void UpgradeSystemsDB19(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE SystemAliases (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT, id_edsm INTEGER, id_edsm_mergedto INTEGER)";
            string query2 = "CREATE INDEX SystemAliases_name ON SystemAliases (name)";
            string query3 = "CREATE UNIQUE INDEX SystemAliases_id_edsm ON SystemAliases (id_edsm)";
            string query4 = "CREATE INDEX SystemAliases_id_edsm_mergedto ON SystemAliases (id_edsm_mergedto)";

            PerformUpgrade(conn, 19, true, true, new[] { query1, query2, query3, query4 });
        }

        private static void UpgradeSystemsDB20(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN gridid Integer NOT NULL DEFAULT -1";
            string query2 = "ALTER TABLE Systems ADD COLUMN randomid Integer NOT NULL DEFAULT -1";

            PerformUpgrade(conn, 20, true, true, new[] { query1, query2 }, () =>
            {
                PutSettingString("EDSMLastSystems", "2010 - 01 - 01 00:00:00", conn);        // force EDSM sync..
            });
        }

        private static void UpgradeUserDB101(SQLiteConnectionED conn)
        {
            string query1 = "DROP TABLE IF EXISTS Systems";
            string query2 = "DROP TABLE IF EXISTS SystemAliases";
            string query3 = "DROP TABLE IF EXISTS Distances";
            string query4 = "VACUUM";

            PerformUpgrade(conn, 101, true,  false, new[] { query1, query2, query3, query4 }, () =>
            {
//                PutSettingString("EDSMLastSystems", "2010 - 01 - 01 00:00:00", conn);        // force EDSM sync..
            });
        }

        private static void UpgradeUserDB102(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE Commanders (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, EdsmApiKey TEXT NOT NULL, NetLogDir TEXT, Deleted INTEGER NOT NULL)";

            PerformUpgrade(conn, 102, true, false, new[] { query1 });
        }

        private static void UpgradeSystemsDB101(SQLiteConnectionED conn)
        {
            string query1 = "DROP TABLE IF EXISTS Bookmarks";
            string query2 = "DROP TABLE IF EXISTS SystemNote";
            string query3 = "DROP TABLE IF EXISTS TravelLogUnit";
            string query4 = "DROP TABLE IF EXISTS VisitedSystems";
            string query5 = "DROP TABLE IF EXISTS Route_Systems";
            string query6 = "DROP TABLE IF EXISTS Routes_expedition";
            string query7 = "VACUUM";


            PerformUpgrade(conn, 101, true, false, new[] { query1, query2, query3, query4, query5, query6, query7 }, () =>
            {
                //                PutSettingString("EDSMLastSystems", "2010 - 01 - 01 00:00:00", conn);        // force EDSM sync..
            });
        }

        private static void UpgradeSystemsDB102(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE SystemNames (" +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "Name TEXT NOT NULL COLLATE NOCASE, " +
                "EdsmId INTEGER NOT NULL)";
            string query2 = "CREATE TABLE EdsmSystems (" +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "EdsmId INTEGER NOT NULL, " +
                "EddbId INTEGER, " +
                "X INTEGER NOT NULL, " +
                "Y INTEGER NOT NULL, " +
                "Z INTEGER NOT NULL, " +
                "CreateTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                "UpdateTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                "VersionTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                "GridId INTEGER NOT NULL DEFAULT -1, " +
                "RandomId INTEGER NOT NULL DEFAULT -1)";
            string query3 = "CREATE TABLE EddbSystems (" +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "Name TEXT NOT NULL, " +
                "EdsmId INTEGER NOT NULL, " +
                "EddbId INTEGER NOT NULL, " +
                "Population INTEGER , " +
                "Faction TEXT, " +
                "GovernmentId Integer, " +
                "AllegianceId Integer, " +
                "PrimaryEconomyId Integer, " +
                "Security Integer, " +
                "EddbUpdatedAt Integer, " + // Seconds since 1970-01-01 00:00:00 UTC
                "State Integer, " +
                "NeedsPermit Integer)";
            PerformUpgrade(conn, 102, true, false, new[] { query1, query2, query3 });
        }

        private static void DropOldUserTables(SQLiteConnectionUser conn)
        {
            string[] queries = new[]
            {
                "DROP TABLE IF EXISTS Systems",
                "DROP TABLE IF EXISTS SystemAliases",
                "DROP TABLE IF EXISTS Distances",
                "DROP TABLE IF EXISTS Stations",
                "DROP TABLE IF EXISTS station_commodities",
            };

            foreach (string query in queries)
            {
                using (DbCommand cmd = conn.CreateCommand(query))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void DropOldSystemTables(SQLiteConnectionSystem conn)
        {
            string[] queries = new[]
            {
                "DROP TABLE IF EXISTS Bookmarks",
                "DROP TABLE IF EXISTS SystemNote",
                "DROP TABLE IF EXISTS TravelLogUnit",
                "DROP TABLE IF EXISTS VisitedSystems",
                "DROP TABLE IF EXISTS route_Systems",
                "DROP TABLE IF EXISTS routes_expeditions",
                "DROP TABLE IF EXISTS Objects",
                "DROP TABLE IF EXISTS wanted_systems",
            };

            foreach (string query in queries)
            {
                using (DbCommand cmd = conn.CreateCommand(query))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void CreateUserDBTableIndexes()
        {
            string[] queries = new[]
            {
                "CREATE INDEX IF NOT EXISTS VisitedSystemIndex ON VisitedSystems (Name ASC, Time ASC)",
                "CREATE INDEX IF NOT EXISTS VisitedSystems_id_edsm_assigned ON VisitedSystems (id_edsm_assigned)",
                "CREATE INDEX IF NOT EXISTS VisitedSystems_position ON VisitedSystems (X, Y, Z)",
                "CREATE INDEX IF NOT EXISTS TravelLogUnit_Name ON TravelLogUnit (Name)"
            };
            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
            {
                foreach (string query in queries)
                {
                    using (DbCommand cmd = conn.CreateCommand(query))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static void CreateSystemDBTableIndexes()
        {
            string[] queries = new[]
            {
                "CREATE UNIQUE INDEX IF NOT EXISTS SystemAliases_id_edsm ON SystemAliases (id_edsm)",
                "CREATE INDEX IF NOT EXISTS SystemAliases_name ON SystemAliases (name)",
                "CREATE INDEX IF NOT EXISTS SystemAliases_id_edsm_mergedto ON SystemAliases (id_edsm_mergedto)",
                "CREATE INDEX IF NOT EXISTS Distances_EDSM_ID_Index ON Distances (id_edsm ASC)",
                "CREATE INDEX IF NOT EXISTS DistanceName ON Distances (NameA ASC, NameB ASC)",
                "CREATE INDEX IF NOT EXISTS stationIndex ON Stations (system_id ASC)",
                "CREATE INDEX IF NOT EXISTS station_commodities_index ON station_commodities (station_id ASC, commodity_id ASC, type ASC)",
                "CREATE INDEX IF NOT EXISTS StationsIndex_ID  ON Stations (id ASC)",
                "CREATE INDEX IF NOT EXISTS StationsIndex_system_ID  ON Stations (system_id ASC)",
                "CREATE INDEX IF NOT EXISTS StationsIndex_system_Name  ON Stations (Name ASC)",
            };
            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                foreach (string query in queries)
                {
                    using (DbCommand cmd = conn.CreateCommand(query))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void DropSystemsTableIndexes()
        {
            string[] queries = new[]
            {
                "DROP INDEX IF EXISTS SystemsIndex",
                "DROP INDEX IF EXISTS Systems_EDSM_ID_Index",
                "DROP INDEX IF EXISTS Systems_EDDB_ID_Index",
                "DROP INDEX IF EXISTS IDX_Systems_versiondate",
                "DROP INDEX IF EXISTS Systems_position",
                "DROP INDEX IF EXISTS SystemGridId",
                "DROP INDEX IF EXISTS SystemRandomId",
                "DROP INDEX IF EXISTS EdsmSystems_EdsmId",
                "DROP INDEX IF EXISTS EdsmSystems_EddbId",
                "DROP INDEX IF EXISTS EddbSystems_EdsmId",
                "DROP INDEX IF EXISTS EddbSystems_EddbId",
                "DROP INDEX IF EXISTS EdsmSystems_Position",
                "DROP INDEX IF EXISTS EdsmSystems_GridId",
                "DROP INDEX IF EXISTS EdsmSystems_RandomId",
                "DROP INDEX IF EXISTS SystemNames_EdsmId",
                "DROP INDEX IF EXISTS sqlite_autoindex_SystemNames_1"
            };
            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                foreach (string query in queries)
                {
                    using (DbCommand cmd = conn.CreateCommand(query))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void CreateSystemsTableIndexes()
        {
            string[] queries = new[]
            {
                "CREATE INDEX IF NOT EXISTS EdsmSystems_EdsmId ON EdsmSystems (EdsmId ASC)",
                "CREATE INDEX IF NOT EXISTS EdsmSystems_EddbId ON EdsmSystems (EddbId ASC)",
                "CREATE INDEX IF NOT EXISTS EddbSystems_EdsmId ON EddbSystems (EdsmId ASC)",
                "CREATE INDEX IF NOT EXISTS EddbSystems_EddbId ON EddbSystems (EddbId ASC)",
                "CREATE INDEX IF NOT EXISTS EdsmSystems_Position ON EdsmSystems (Z, X, Y)",
                "CREATE INDEX IF NOT EXISTS EdsmSystems_GridId ON EdsmSystems (gridid)",
                "CREATE INDEX IF NOT EXISTS EdsmSystems_RandomId ON EdsmSystems (randomid)",
                "CREATE INDEX IF NOT EXISTS SystemNames_EdsmId ON SystemNames (EdsmId)"
            };
            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                foreach (string query in queries)
                {
                    using (DbCommand cmd = conn.CreateCommand(query))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void CreateTempSystemsTable()
        {
            using (var conn = new SQLiteConnectionSystem())
            {
                ExecuteQuery(conn, "DROP TABLE IF EXISTS EdsmSystems_temp");
                ExecuteQuery(conn, "DROP TABLE IF EXISTS SystemNames_temp");
                ExecuteQuery(conn,
                    "CREATE TABLE SystemNames_temp (" +
                        "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "Name TEXT NOT NULL COLLATE NOCASE, " +
                        "EdsmId INTEGER NOT NULL)");
                ExecuteQuery(conn,
                    "CREATE TABLE EdsmSystems_temp (" +
                        "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "EdsmId INTEGER NOT NULL, " +
                        "EddbId INTEGER, " +
                        "X INTEGER NOT NULL, " +
                        "Y INTEGER NOT NULL, " +
                        "Z INTEGER NOT NULL, " +
                        "CreateTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                        "UpdateTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                        "VersionTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                        "GridId INTEGER NOT NULL DEFAULT -1, " +
                        "RandomId INTEGER NOT NULL DEFAULT -1)");
            }
        }

        public static void ReplaceSystemsTable()
        {
            using (var slock = new SQLiteConnectionED.SchemaLock())
            {
                using (var conn = new SQLiteConnectionSystem())
                {
                    DropSystemsTableIndexes();
                    using (var txn = conn.BeginTransaction())
                    { 
                        ExecuteQuery(conn, "DROP TABLE IF EXISTS Systems");
                        ExecuteQuery(conn, "DROP TABLE IF EXISTS EdsmSystems");
                        ExecuteQuery(conn, "DROP TABLE IF EXISTS SystemNames");
                        ExecuteQuery(conn, "ALTER TABLE EdsmSystems_temp RENAME TO EdsmSystems");
                        ExecuteQuery(conn, "ALTER TABLE SystemNames_temp RENAME TO SystemNames");
                        txn.Commit();
                    }
                    ExecuteQuery(conn, "VACUUM");
                    CreateSystemsTableIndexes();
                }
            }
        }

        private static DbProviderFactory GetSqliteProviderFactory()
        {
            if (WindowsSqliteProviderWorks())
            {
                return GetWindowsSqliteProviderFactory();
            }

            var factory = GetMonoSqliteProviderFactory();

            if (DbFactoryWorks(factory))
            {
                return factory;
            }

            throw new InvalidOperationException("Unable to get a working Sqlite driver");
        }

        private static bool WindowsSqliteProviderWorks()
        {
            try
            {
                // This will throw an exception if the SQLite.Interop.dll can't be loaded.
                System.Diagnostics.Trace.WriteLine($"SQLite version {SQLiteConnection.SQLiteVersion}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool DbFactoryWorks(DbProviderFactory factory)
        {
            if (factory != null)
            {
                try
                {
                    using (var conn = factory.CreateConnection())
                    {
                        conn.ConnectionString = "Data Source=:memory:;Pooling=true;";
                        conn.Open();
                        return true;
                    }
                }
                catch
                {
                }
            }

            return false;
        }

        private static DbProviderFactory GetMonoSqliteProviderFactory()
        {
            try
            {
                // Disable CS0618 warning for LoadWithPartialName
                #pragma warning disable CS0618
                var asm = System.Reflection.Assembly.LoadWithPartialName("Mono.Data.Sqlite");
                #pragma warning restore CS0618
                var factorytype = asm.GetType("Mono.Data.Sqlite.SqliteFactory");
                return (DbProviderFactory)factorytype.GetConstructor(new Type[0]).Invoke(new object[0]);
            }
            catch
            {
                return null;
            }
        }

        private static DbProviderFactory GetWindowsSqliteProviderFactory()
        {
            try
            {
                return new System.Data.SQLite.SQLiteFactory();
            }
            catch
            {
                return null;
            }
        }


        #endregion

        #region Database access
        public static DbConnection CreateCN()
        {
            lock (lockDBInit)                                           // one at a time chaps
            {
                if (DbFactory == null)                                        // first one to ask for a connection sets the db up
                {
                    InitializeDatabase();
                }
            }

            return DbFactory.CreateConnection();
        }

        ///----------------------------
        /// STATIC code helpers for other DB classes

        public static DataSet SQLQueryText(SQLiteConnectionED cn, DbCommand cmd)  
        {
            try
            {
                DataSet ds = new DataSet();
                DbDataAdapter da = cmd.CreateDataAdapter();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SqlQuery Exception: " + ex.Message);
                throw;
            }
        }

        static public int SQLNonQueryText(SQLiteConnectionED cn, DbCommand cmd)   
        {
            int rows = 0;

            try
            {
                rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SqlNonQueryText Exception: " + ex.Message);
                throw;
            }
        }

        static public object SQLScalar(SQLiteConnectionED cn, DbCommand cmd)      
        {
            object ret = null;

            try
            {
                ret = cmd.ExecuteScalar();
                return ret;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SqlNonQuery Exception: " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Extension Methods
        public static void AddParameterWithValue(this DbCommand cmd, string name, object val)
        {
            var par = cmd.CreateParameter();
            par.ParameterName = name;
            par.Value = val;
            cmd.Parameters.Add(par);
        }

        public static void AddParameter(this DbCommand cmd, string name, DbType type)
        {
            var par = cmd.CreateParameter();
            par.ParameterName = name;
            par.DbType = type;
            cmd.Parameters.Add(par);
        }

        public static void SetParameterValue(this DbCommand cmd, string name, object val)
        {
            cmd.Parameters[name].Value = val;
        }

        public static DbDataAdapter CreateDataAdapter(this DbCommand cmd)
        {
            DbDataAdapter da = DbFactory.CreateDataAdapter();
            da.SelectCommand = cmd;
            return da;
        }

        public static DbCommand CreateCommand(this DbConnection conn, string query)
        {
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
            cmd.CommandText = query;
            return cmd;
        }

        public static DbCommand CreateCommand(this DbConnection conn, string query, DbTransaction transaction)
        {
            DbCommand cmd = conn.CreateCommand(query);
            cmd.Transaction = transaction;
            return cmd;
        }
        #endregion

        #region Settings
        ///----------------------------
        /// STATIC functions for discrete values

        static public bool keyExists(string sKey)                   
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return keyExists(sKey, cn);
            }
        }

        static public bool keyExists(string sKey, SQLiteConnectionED cn)
        {
            try
            {
                using (DbCommand cmd = cn.CreateCommand("select ID from Register WHERE ID=@key"))
                {
                    cmd.AddParameterWithValue("@key", sKey);

                    DataSet ds = SQLQueryText(cn, cmd);

                    return (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);        // got a value, true
                }
            }
            catch
            {
            }

            return false;
        }

        static public int GetSettingInt(string key, int defaultvalue)     
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return GetSettingInt(key, defaultvalue, cn);
            }
        }

        static public int GetSettingInt(string key, int defaultvalue, SQLiteConnectionED cn )
        { 
            try
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT ValueInt from Register WHERE ID = @ID"))
                {
                    cmd.AddParameterWithValue("@ID", key);

                    object ob = SQLScalar(cn, cmd);

                    if (ob == null)
                        return defaultvalue;

                    int val = Convert.ToInt32(ob);

                    return val;
                }
            }
            catch 
            {
                return defaultvalue;
            }
        }

        static public bool PutSettingInt(string key, int intvalue)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = PutSettingInt(key, intvalue, cn);
                return ret;
            }
        }

        static public bool PutSettingInt(string key, int intvalue, SQLiteConnectionED cn )
        {
            try
            {
                if (keyExists(key,cn))
                {
                    using (DbCommand cmd = cn.CreateCommand("Update Register set ValueInt = @ValueInt Where ID=@ID"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@ValueInt", intvalue);

                        SQLNonQueryText(cn, cmd);

                        return true;
                    }
                }
                else
                {
                    using (DbCommand cmd = cn.CreateCommand("Insert into Register (ID, ValueInt) values (@ID, @valint)"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@valint", intvalue);

                        SQLNonQueryText(cn, cmd);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        static public double GetSettingDouble(string key, double defaultvalue)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return GetSettingDouble(key, defaultvalue, cn);
            }
        }

        static public double GetSettingDouble(string key, double defaultvalue , SQLiteConnectionED cn )
        {
            try
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT ValueDouble from Register WHERE ID = @ID"))
                {
                    cmd.AddParameterWithValue("@ID", key);

                    object ob = SQLScalar(cn, cmd);

                    if (ob == null)
                        return defaultvalue;

                    double val = Convert.ToDouble(ob);

                    return val;
                }
            }
            catch
            {
                return defaultvalue;
            }
        }

        static public bool PutSettingDouble(string key, double doublevalue)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = PutSettingDouble(key, doublevalue, cn);
                return ret;
            }
        }

        static public bool PutSettingDouble(string key, double doublevalue, SQLiteConnectionED cn)
        {
            try
            {
                if (keyExists(key,cn))
                {
                    using (DbCommand cmd = cn.CreateCommand("Update Register set ValueDouble = @ValueDouble Where ID=@ID"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@ValueDouble", doublevalue);

                        SQLNonQueryText(cn, cmd);

                        return true;
                    }
                }
                else
                {
                    using (DbCommand cmd = cn.CreateCommand("Insert into Register (ID, ValueDouble) values (@ID, @valdbl)"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@valdbl", doublevalue);

                        SQLNonQueryText(cn, cmd);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        static public bool GetSettingBool(string key, bool defaultvalue)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return GetSettingBool(key, defaultvalue, cn);
            }
        }

        static public bool GetSettingBool(string key, bool defaultvalue,SQLiteConnectionED cn)
        {
            try
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT ValueInt from Register WHERE ID = @ID"))
                {
                    cmd.AddParameterWithValue("@ID", key);

                    object ob = SQLScalar(cn, cmd);

                    if (ob == null)
                        return defaultvalue;

                    int val = Convert.ToInt32(ob);

                    if (val == 0)
                        return false;
                    else
                        return true;
                }
            }
            catch
            {
                return defaultvalue;
            }
        }


        static public bool PutSettingBool(string key, bool boolvalue)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = PutSettingBool(key, boolvalue, cn);
                return ret;
            }
        }

        static public bool PutSettingBool(string key, bool boolvalue, SQLiteConnectionED cn)
        {
            try
            {
                int intvalue = 0;

                if (boolvalue == true)
                    intvalue = 1;

                if (keyExists(key,cn))
                {
                    using (DbCommand cmd = cn.CreateCommand("Update Register set ValueInt = @ValueInt Where ID=@ID"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@ValueInt", intvalue);

                        SQLNonQueryText(cn, cmd);

                        return true;
                    }
                }
                else
                {
                    using (DbCommand cmd = cn.CreateCommand("Insert into Register (ID, ValueInt) values (@ID, @valint)"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@valint", intvalue);

                        SQLNonQueryText(cn, cmd);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        static public string GetSettingString(string key, string defaultvalue)
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                return GetSettingString(key, defaultvalue, cn);
            }
        }

        static public string GetSettingString(string key, string defaultvalue, SQLiteConnectionED cn)
        {
            try
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT ValueString from Register WHERE ID = @ID"))
                {
                    cmd.AddParameterWithValue("@ID", key);
                    object ob = SQLScalar(cn, cmd);

                    if (ob == null)
                        return defaultvalue;

                    if (ob == System.DBNull.Value)
                        return defaultvalue;

                    string val = (string)ob;

                    return val;
                }
            }
            catch 
            {
                return defaultvalue;
            }
        }

        static public bool PutSettingString(string key, string strvalue)        // public IF
        {
            using (SQLiteConnectionUser cn = new SQLiteConnectionUser())
            {
                bool ret = PutSettingString(key, strvalue, cn);
                return ret;
            }
        }

        static public bool PutSettingString(string key, string strvalue , SQLiteConnectionED cn )
        {
            try
            {
                if (keyExists(key,cn))
                {
                    using (DbCommand cmd = cn.CreateCommand("Update Register set ValueString = @ValueString Where ID=@ID"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@ValueString", strvalue);

                        SQLNonQueryText(cn, cmd);

                        return true;
                    }
                }
                else
                {
                    using (DbCommand cmd = cn.CreateCommand("Insert into Register (ID, ValueString) values (@ID, @valint)"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@valint", strvalue);

                        SQLNonQueryText(cn, cmd);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
