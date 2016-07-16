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
    public static class SQLiteDBExtensions
    {
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
            DbDataAdapter da = SQLiteDBClass.DbFactory.CreateDataAdapter();
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
    }

    // This class uses a monitor to ensure only one can be
    // active at any one time
    public class SQLiteTxnLockED : IDisposable
    {
        private static object _transactionLock = new object();
        private bool _locktaken = false;
        private static ConcurrentDictionary<Thread, bool> _waitingthreads = new ConcurrentDictionary<Thread, bool>();
        private Thread _owningThread;

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
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) { return InnerCommand.ExecuteReader(behavior); }
        public override void Cancel() { InnerCommand.Cancel(); }
        public override object ExecuteScalar() { return InnerCommand.ExecuteScalar(); }
        public override void Prepare() { InnerCommand.Prepare(); }
        #endregion

        public override int ExecuteNonQuery()
        {
            if (this._transaction != null)
            {
                // The transaction should already have the transaction lock
                return InnerCommand.ExecuteNonQuery();
            }
            else
            {
                // Take the transaction lock for the duration of this command
                using (var txnlock = new SQLiteTxnLockED())
                {
                    txnlock.Open();
                    return InnerCommand.ExecuteNonQuery();
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

    public class SQLiteConnectionED : IDisposable              // USE this for connections.. 
    {
        //static Object monitor = new Object();                 // monitor disabled for now - it will prevent SQLite DB locked errors but 
                                                                // causes the program to become unresponsive during big DB updates
        private DbConnection _cn;

        public SQLiteConnectionED()
        {
            // System.Threading.Monitor.Enter(monitor);
            //Console.WriteLine("Connection open " + System.Threading.Thread.CurrentThread.Name);
            _cn = SQLiteDBClass.CreateCN();
            _cn.Open();
        }

        public DbCommand CreateCommand(string cmd, DbTransaction tn = null)
        {
            return new SQLiteCommandED(_cn.CreateCommand(cmd), tn);
        }

        public DbTransaction BeginTransaction(IsolationLevel isolevel)
        {
            // Take the transaction lock before beginning the
            // transaction to avoid a deadlock
            var txnlock = new SQLiteTxnLockED();
            txnlock.Open();
            return new SQLiteTransactionED(_cn.BeginTransaction(isolevel), txnlock);
        }

        public DbTransaction BeginTransaction()
        {
            // Take the transaction lock before beginning the
            // transaction to avoid a deadlock
            var txnlock = new SQLiteTxnLockED();
            txnlock.Open();
            return new SQLiteTransactionED(_cn.BeginTransaction(), txnlock);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        
        // disposing: true if Dispose() was called, false
        // if being finalized by the garbage collector
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_cn != null)
                {
                    _cn.Close();
                    _cn.Dispose();
                    _cn = null;
                }
            }
        }

        public SQLiteConnectionED(SQLiteConnection c )            // ONLY use by class below..
        {
            //System.Threading.Monitor.Enter(monitor);
            _cn = c;
            _cn.Open();
        }
    }


    public class SQLiteDBClass
    {
        private static DbProviderFactory _factory;
        public static DbProviderFactory DbFactory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = new SQLiteFactory();
                }
                return _factory;
            }
        }

        public static DbConnection CreateCN()
        {
            lock (lockDBInit)                                           // one at a time chaps
            {
                if (_db == null)                                        // first one to ask for a connection sets the db up
                {
                    _db = new SQLiteDBClass();
                    _db.InitializeDatabase();
                }
            }

            DbConnection cn = DbFactory.CreateConnection();
            cn.ConnectionString = _db.constring;
            return cn;
        }

        private static SQLiteDBClass _db = null;                            // one db class for everyone
        private static Object lockDBInit = new Object();                    // lock to sequence construction
        private string constring;                                           // connection string to use..
        SQLiteConnectionED m_SQLiteConnectionED;                            // only used by class constructor

        private SQLiteDBClass()         // non static class functions in here are only used by the construction
        {                               // so this is private to make sure you don't try and initialise a DB anywhere..
        }

        private void InitializeDatabase()
        {
            string dbfile = GetSQLiteDBFile();
            constring = "Data Source=" + dbfile + ";Pooling=true;";
            try
            {
                bool fileexist = File.Exists(dbfile);

                if (!fileexist)                                         // no file, create it
                    SQLiteConnection.CreateFile(dbfile);

                using (m_SQLiteConnectionED = new SQLiteConnectionED(new SQLiteConnection(constring)))
                {
                    if (!fileexist)                                       // first time, create the register
                        ExecuteQuery("CREATE TABLE Register (ID TEXT PRIMARY KEY  NOT NULL  UNIQUE , \"ValueInt\" INTEGER, \"ValueDouble\" DOUBLE, \"ValueString\" TEXT, \"ValueBlob\" BLOB)");

                    UpgradeDB();                                            // upgrade it
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error creating data base file, Exception", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private void ExecuteQuery(string query)
        {
            using (DbCommand command = m_SQLiteConnectionED.CreateCommand(query))
                command.ExecuteNonQuery();
        }

        private string GetSQLiteDBFile()
        {
            return Path.Combine(Tools.GetAppDataDirectory(), "EDDiscovery.sqlite");
        }

        private bool UpgradeDB()
        {
            int dbver;
            try
            {
                dbver = GetSettingInt("DBVer", 1, m_SQLiteConnectionED);        // use the constring one, as don't want to go back into ConnectionString code
                if (dbver < 2)
                    UpgradeDB2();

                if (dbver < 3)
                    UpgradeDB3();

                if (dbver < 4)
                    UpgradeDB4();

                if (dbver < 5)
                    UpgradeDB5();

                if (dbver < 6)
                    UpgradeDB6();

                if (dbver < 7)
                    UpgradeDB7();

                if (dbver < 8)
                    UpgradeDB8();

                if (dbver < 9)
                    UpgradeDB9();

                if (dbver < 10)
                    UpgradeDB10();

                if (dbver < 11)
                    UpgradeDB11();

                if (dbver < 12)
                    UpgradeDB12();

                // 15 remove due to conflict between 2 branches...

                if (dbver < 14)
                    UpgradeDB14();

                if (dbver < 15)
                    UpgradeDB15();

                if (dbver < 16)
                    UpgradeDB16();

                if (dbver < 17)
                    UpgradeDB17();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UpgradeDB error: " + ex.Message);
                MessageBox.Show(ex.StackTrace);
                return false;
            }
        }

        private void PerformUpgrade(int newVersion, bool catchErrors, bool backupDbFile, string[] queries, Action doAfterQueries = null)
        {
            if (backupDbFile)
            {
                string dbfile = GetSQLiteDBFile();

                try
                {
                    File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", $"EDDiscovery{newVersion - 1}.sqlite"));
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
                    ExecuteQuery(query);
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

            PutSettingInt("DBVer", newVersion, m_SQLiteConnectionED);
        }

        private void UpgradeDB2()
        {
            string query = "CREATE TABLE Systems (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , name TEXT NOT NULL COLLATE NOCASE , x FLOAT, y FLOAT, z FLOAT, cr INTEGER, commandercreate TEXT, createdate DATETIME, commanderupdate TEXT, updatedate DATETIME, status INTEGER, population INTEGER )";
            string query2 = "CREATE  INDEX main.SystemsIndex ON Systems (name ASC)";
            string query3 = "CREATE TABLE Distances (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL  UNIQUE , NameA TEXT NOT NULL , NameB TEXT NOT NULL , Dist FLOAT NOT NULL , CommanderCreate TEXT NOT NULL , CreateTime DATETIME NOT NULL , Status INTEGER NOT NULL )";
            string query4 = "CREATE TABLE SystemNote (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Time DATETIME NOT NULL )";
            string query5 = "CREATE INDEX DistanceName ON Distances (NameA ASC, NameB ASC)";
            string query6 = "CREATE  TABLE VisitedSystems (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Time DATETIME NOT NULL , SystemID INTEGER, Dist FLOAT)";
            string query7 = "CREATE TABLE Stations (station_id INTEGER PRIMARY KEY  NOT NULL ,system_id INTEGER REFERENCES Systems(id), name TEXT NOT NULL ,blackmarket BOOL DEFAULT (null) ,max_landing_pad_size INTEGER,distance_to_star INTEGER,type TEXT,faction TEXT,shipyard BOOL,outfitting BOOL, commodities_market BOOL)";
            string query8 = "CREATE  INDEX stationIndex ON Stations (system_id ASC)";

            PerformUpgrade(2, false, false, new[] { query, query2, query3, query4, query5, query6, query7, query8 });
        }

        private void UpgradeDB3()
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN Note TEXT";
            PerformUpgrade(3, false, false, new[] { query1 });
        }

        private void UpgradeDB4()
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN Note TEXT";
            PerformUpgrade(4, true, true, new[] { query1 }, () =>
            {  });
        }

        private void UpgradeDB5()
        {
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN Unit TEXT";
            string query3 = "ALTER TABLE VisitedSystems ADD COLUMN Commander Integer";
            string query4 = "CREATE INDEX VisitedSystemIndex ON VisitedSystems (Name ASC, Time ASC)";
            PerformUpgrade(5, true, true, new[] {query1, query3, query4});
        }

        private void UpgradeDB6()
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
            string query11 = "DROP TABLE Stations";
            string query12 = "CREATE TABLE Stations (id INTEGER PRIMARY KEY  NOT NULL ,system_id INTEGER, name TEXT NOT NULL ,  " +
                " max_landing_pad_size INTEGER, distance_to_star INTEGER, faction Text, government_id INTEGER, allegiance_id Integer,  state_id INTEGER, type_id Integer, " +
                "has_commodities BOOL DEFAULT (null), has_refuel BOOL DEFAULT (null), has_repair BOOL DEFAULT (null), has_rearm BOOL DEFAULT (null), " +
                "has_outfitting BOOL DEFAULT (null),  has_shipyard BOOL DEFAULT (null), has_blackmarket BOOL DEFAULT (null),   eddb_updated_at Integer  )";

            string query13 = "CREATE TABLE station_commodities (station_id INTEGER PRIMARY KEY NOT NULL, commodity_id INTEGER, type INTEGER)";
            string query14 = "CREATE INDEX station_commodities_index ON station_commodities (station_id ASC, commodity_id ASC, type ASC)";
            string query15 = "CREATE INDEX StationsIndex_ID  ON Stations (id ASC)";
            string query16 = "CREATE INDEX StationsIndex_system_ID  ON Stations (system_id ASC)";
            string query17 = "CREATE INDEX StationsIndex_system_Name  ON Stations (Name ASC)";

            PerformUpgrade(6, true, true, new[] {
                query1, query2, query4, query5, query6, query7, query8, query9, query10,
                query11, query12, query13, query14, query15, query16, query17 });
        }


        private void UpgradeDB7()
        {
            string query1 = "DROP TABLE VisitedSystems";
            string query2 = "CREATE TABLE VisitedSystems(id INTEGER PRIMARY KEY  NOT NULL, Name TEXT NOT NULL, Time DATETIME NOT NULL, Unit Text, Commander Integer, Source Integer, edsm_sync BOOL DEFAULT (null))";
            string query3 = "CREATE TABLE TravelLogUnit(id INTEGER PRIMARY KEY  NOT NULL, type INTEGER NOT NULL, name TEXT NOT NULL, size INTEGER, path TEXT)";
            PerformUpgrade(7, true, true, new[] { query1, query2, query3 });
        }

        private void UpgradeDB8()
        {
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN Map_colour INTEGER DEFAULT (-65536)";
            PerformUpgrade(8, true, true, new[] { query1 });
        }

        private void UpgradeDB9()
        {
            string query1 = "CREATE TABLE Objects (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , SystemName TEXT NOT NULL , ObjectName TEXT NOT NULL , ObjectType INTEGER NOT NULL , ArrivalPoint Float, Gravity FLOAT, Atmosphere Integer, Vulcanism Integer, Terrain INTEGER, Carbon BOOL, Iron BOOL, Nickel BOOL, Phosphorus BOOL, Sulphur BOOL, Arsenic BOOL, Chromium BOOL, Germanium BOOL, Manganese BOOL, Selenium BOOL NOT NULL , Vanadium BOOL, Zinc BOOL, Zirconium BOOL, Cadmium BOOL, Mercury BOOL, Molybdenum BOOL, Niobium BOOL, Tin BOOL, Tungsten BOOL, Antimony BOOL, Polonium BOOL, Ruthenium BOOL, Technetium BOOL, Tellurium BOOL, Yttrium BOOL, Commander  Text, UpdateTime DATETIME, Status INTEGER )";
            PerformUpgrade(9, true, true, new[] { query1 });
        }

        private void UpgradeDB10()
        {
            string query1 = "CREATE TABLE wanted_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, systemname TEXT UNIQUE NOT NULL)";
            PerformUpgrade(10, true, true, new[] { query1 });
        }

        private void UpgradeDB11()
        {
            //Default is Color.Red.ToARGB()
            string query1 = "ALTER TABLE Systems ADD COLUMN FirstDiscovery BOOL";
            string query2 = "ALTER TABLE Objects ADD COLUMN Landed BOOL";
            string query3 = "ALTER TABLE Objects ADD COLUMN terraform Integer";
            string query4 = "ALTER TABLE VisitedSystems ADD COLUMN Status BOOL";
            PerformUpgrade(11, true, true, new[] { query1, query2, query3, query4 });
        }

        private void UpgradeDB12()
        {
            string query1 = "CREATE TABLE routes_expeditions (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT UNIQUE NOT NULL, start DATETIME, end DATETIME)";
            string query2 = "CREATE TABLE route_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, routeid INTEGER NOT NULL, systemname TEXT NOT NULL)";
            PerformUpgrade(12, true, true, new[] { query1, query2 });
        }


        private bool UpgradeDB14()
        {
            //Default is Color.Red.ToARGB()
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN X double";
            string query2 = "ALTER TABLE VisitedSystems ADD COLUMN Y double";
            string query3 = "ALTER TABLE VisitedSystems ADD COLUMN Z double";
            string dbfile = GetSQLiteDBFile();

            PerformUpgrade(14, true, true, new[] { query1, query2, query3 });
            return true;
        }

        private void UpgradeDB15()
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN versiondate DATETIME";
            string query2 = "UPDATE Systems SET versiondate = datetime('now')";
            string query3 = "CREATE INDEX IDX_Systems_versiondate ON Systems (versiondate ASC)";

            PerformUpgrade(15, true, true, new[] { query1, query2, query3 });
        }

        private void UpgradeDB16()
        {
            string query = "CREATE TABLE Bookmarks (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , StarName TEXT, x double NOT NULL, y double NOT NULL, z double NOT NULL, Time DATETIME NOT NULL, Heading TEXT, Note TEXT NOT Null )";
            PerformUpgrade(16, true, true, new[] { query });
        }

        private void UpgradeDB17()
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN id_edsm Integer";
            string query2 = "CREATE INDEX Systems_EDSM_ID_Index ON Systems (id_edsm ASC)";
            string query3 = "CREATE INDEX Systems_EDDB_ID_Index ON Systems (id_eddb ASC)";
            string query4 = "ALTER TABLE Distances ADD COLUMN id_edsm Integer";
            string query5 = "CREATE INDEX Distances_EDSM_ID_Index ON Distances (id_edsm ASC)";
            string query6 = "Update VisitedSystems set x=null, y=null, z=null where x=0 and y=0 and z=0 and name!=\"Sol\"";

            PerformUpgrade(17, true, true, new[] { query1,query2,query3,query4,query5,query6 }, () =>
            {
                PutSettingString("EDSMLastSystems", "2010 - 01 - 01 00:00:00", m_SQLiteConnectionED);        // force EDSM sync..
                PutSettingString("EDDBSystemsTime", "0", m_SQLiteConnectionED);                               // force EDDB
                PutSettingString("EDSCLastDist", "2010-01-01 00:00:00", m_SQLiteConnectionED);                // force distances
            });
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

        ///----------------------------
        /// STATIC functions for discrete values

        static public bool keyExists(string sKey)                   
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
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

    }
}
