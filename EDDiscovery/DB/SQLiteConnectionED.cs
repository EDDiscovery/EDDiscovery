using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace EDDiscovery.DB
{
    public class SQLiteConnectionOld : SQLiteConnectionED<SQLiteConnectionOld>
    {
        public SQLiteConnectionOld() : base(EDDSqlDbSelection.EDDiscovery)
        {
        }

        protected override void InitializeDatabase()
        {
        }
    }

    public class SQLiteConnectionUser : SQLiteConnectionED<SQLiteConnectionUser>
    {
        public SQLiteConnectionUser() : base(SQLiteDBClass.UserDatabase)
        {
        }

        public SQLiteConnectionUser(bool utc = true, EDDbAccessMode mode = EDDbAccessMode.Indeterminate) : base(SQLiteDBClass.UserDatabase, utctimeindicator: utc)
        {
        }

        protected SQLiteConnectionUser(bool initializing, bool utc, EDDbAccessMode mode = EDDbAccessMode.Indeterminate) : base(SQLiteDBClass.UserDatabase, utctimeindicator: utc, initializing: initializing)
        {
        }

        protected override void InitializeDatabase()
        {
            string dbv4file = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDiscovery);
            string dbuserfile = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDUser);

            if (File.Exists(dbv4file) && !File.Exists(dbuserfile))
            {
                File.Copy(dbv4file, dbuserfile);
            }

            using (SQLiteConnectionUser conn = new SQLiteConnectionUser(true, true, EDDbAccessMode.Writer))
            {
                SQLiteDBUserClass.UpgradeUserDB(conn);
            }
        }
    }

    public class SQLiteConnectionSystem : SQLiteConnectionED<SQLiteConnectionSystem>
    {
        public SQLiteConnectionSystem() : base(SQLiteDBClass.SystemDatabase)
        {
        }

        public SQLiteConnectionSystem(EDDbAccessMode mode = EDDbAccessMode.Indeterminate) : base(SQLiteDBClass.SystemDatabase)
        {
        }

        public SQLiteConnectionSystem(bool initializing, EDDbAccessMode mode = EDDbAccessMode.Indeterminate) : base(SQLiteDBClass.SystemDatabase, initializing: initializing)
        {
        }

        protected override void InitializeDatabase()
        {
            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem(true, EDDbAccessMode.Writer))
            {
                SQLiteDBSystemClass.UpgradeSystemsDB(conn);
            }
        }
    }

    /*
    public class SQLiteConnectionCombined : SQLiteConnectionED<SQLiteConnectionCombined>
    {
        public SQLiteConnectionCombined() : base(EDDSqlDbSelection.None, EDDSqlDbSelection.EDDUser | EDDSqlDbSelection.EDDSystem)
        {
        }
    }
     */

    public enum EDDbAccessMode
    {
        Reader,
        Writer,
        Indeterminate
    }

    public abstract class SQLiteConnectionED<TConn> : SQLiteConnectionED
        where TConn : SQLiteConnectionED, new()
    {
        public static bool IsReadWaiting
        {
            get
            {
                return SQLiteTxnLockED<TConn>.IsReadWaiting;
            }
        }
        private static ReaderWriterLockSlim _schemaLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private static bool _initialized = false;
        private static int _initsem = 0;
        private static ManualResetEvent _initbarrier = new ManualResetEvent(false);
        private SQLiteTxnLockED<TConn> _transactionLock;

        public class SchemaLock : IDisposable
        {
            public SchemaLock()
            {
                if (_schemaLock.RecursiveReadCount != 0)
                {
                    throw new InvalidOperationException("Cannot take a schema lock while holding an open database connection");
                }

                _schemaLock.EnterWriteLock();
            }

            public void Dispose()
            {
                if (_schemaLock.IsWriteLockHeld)
                {
                    _schemaLock.ExitWriteLock();
                }
            }
        }

        public SQLiteConnectionED(EDDSqlDbSelection? maindb = null, bool utctimeindicator = false, bool initializing = false, bool shortlived = true)
            : base(initializing)
        {
            bool locktaken = false;
            try
            {
                if (!initializing && !_initialized)
                {
                    int cur = Interlocked.Increment(ref _initsem);

                    if (cur == 0)
                    {
                        using (var slock = new SchemaLock())
                        {
                            _initbarrier.Set();
                            InitializeDatabase();
                            _initialized = true;
                        }
                    }
                }

                if (!_initialized)
                {
                    _initbarrier.WaitOne();
                }

                _schemaLock.EnterReadLock();
                locktaken = true;

                // System.Threading.Monitor.Enter(monitor);
                //Console.WriteLine("Connection open " + System.Threading.Thread.CurrentThread.Name);
                DBFile = GetSQLiteDBFile(maindb ?? SQLiteDBClass.DefaultMainDatabase);
                _cn = SQLiteDBClass.CreateCN();

                // Use the database selected by maindb as the 'main' database
                _cn.ConnectionString = "Data Source=" + DBFile + ";Pooling=true;";

                if (utctimeindicator)   // indicate treat dates as UTC.
                    _cn.ConnectionString += "DateTimeKind=Utc;";

                _transactionLock = new SQLiteTxnLockED<TConn>();
                _cn.Open();
            }
            catch
            {
                if (_transactionLock != null)
                {
                    _transactionLock.Dispose();
                }

                if (locktaken)
                {
                    _schemaLock.ExitReadLock();
                }
                throw;
            }
        }

        public override DbCommand CreateCommand(string cmd, DbTransaction tn = null)
        {
            AssertThreadOwner();
            return new SQLiteCommandED<TConn>(_cn.CreateCommand(cmd), this, _transactionLock, tn);
        }

        public override DbTransaction BeginTransaction(IsolationLevel isolevel)
        {
            // Take the transaction lock before beginning the
            // transaction to avoid a deadlock
            AssertThreadOwner();
            _transactionLock.OpenWriter();
            return new SQLiteTransactionED<TConn>(_cn.BeginTransaction(isolevel), _transactionLock);
        }

        public override DbTransaction BeginTransaction()
        {
            // Take the transaction lock before beginning the
            // transaction to avoid a deadlock
            AssertThreadOwner();
            _transactionLock.OpenWriter();
            return new SQLiteTransactionED<TConn>(_cn.BeginTransaction(), _transactionLock);
        }

        public override void Dispose()
        {
            Dispose(true);
        }

        // disposing: true if Dispose() was called, false
        // if being finalized by the garbage collector
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_cn != null)
                {
                    _cn.Close();
                    _cn.Dispose();
                    _cn = null;
                }

                if (_schemaLock.IsReadLockHeld)
                {
                    _schemaLock.ExitReadLock();
                }

                if (_transactionLock != null)
                {
                    _transactionLock.Dispose();
                    _transactionLock = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Settings
        ///----------------------------
        /// STATIC functions for discrete values

        static public bool keyExists(string sKey)
        {
            using (TConn cn = new TConn())
            {
                return cn.keyExistsCN(sKey);
            }
        }

        static public int GetSettingInt(string key, int defaultvalue)
        {
            using (TConn cn = new TConn())
            {
                return cn.GetSettingIntCN(key, defaultvalue);
            }
        }

        static public bool PutSettingInt(string key, int intvalue)
        {
            using (TConn cn = new TConn())
            {
                bool ret = cn.PutSettingIntCN(key, intvalue);
                return ret;
            }
        }

        static public double GetSettingDouble(string key, double defaultvalue)
        {
            using (TConn cn = new TConn())
            {
                return cn.GetSettingDoubleCN(key, defaultvalue);
            }
        }

        static public bool PutSettingDouble(string key, double doublevalue)
        {
            using (TConn cn = new TConn())
            {
                bool ret = cn.PutSettingDoubleCN(key, doublevalue);
                return ret;
            }
        }

        static public bool GetSettingBool(string key, bool defaultvalue)
        {
            using (TConn cn = new TConn())
            {
                return cn.GetSettingBoolCN(key, defaultvalue);
            }
        }


        static public bool PutSettingBool(string key, bool boolvalue)
        {
            using (TConn cn = new TConn())
            {
                bool ret = cn.PutSettingBoolCN(key, boolvalue);
                return ret;
            }
        }

        static public string GetSettingString(string key, string defaultvalue)
        {
            using (TConn cn = new TConn())
            {
                return cn.GetSettingStringCN(key, defaultvalue);
            }
        }

        static public bool PutSettingString(string key, string strvalue)        // public IF
        {
            using (TConn cn = new TConn())
            {
                bool ret = cn.PutSettingStringCN(key, strvalue);
                return ret;
            }
        }
        #endregion
    }

    public abstract class SQLiteConnectionED : IDisposable              // USE this for connections.. 
    {
        //static Object monitor = new Object();                 // monitor disabled for now - it will prevent SQLite DB locked errors but 
        // causes the program to become unresponsive during big DB updates
        protected DbConnection _cn;
        protected Thread _owningThread;
        protected static List<SQLiteConnectionED> _openConnections = new List<SQLiteConnectionED>();

        protected SQLiteConnectionED(bool initializing)
        {
            lock (_openConnections)
            {
                _openConnections.Add(this);
            }
            _owningThread = Thread.CurrentThread;
        }

        protected void AssertThreadOwner()
        {
            if (Thread.CurrentThread != _owningThread)
            {
                throw new InvalidOperationException($"DB connection was passed between threads.  Owning thread: {_owningThread.Name} ({_owningThread.ManagedThreadId}); this thread: {Thread.CurrentThread.Name} ({Thread.CurrentThread.ManagedThreadId})");
            }
        }

        public string DBFile { get; protected set; }

        public static string GetSQLiteDBFile(EDDSqlDbSelection selector)
        {
            if (selector == EDDSqlDbSelection.None)
            {
                // Use an in-memory database if no database is selected
                return ":memory:";
            }
            else if (selector.HasFlag(EDDSqlDbSelection.EDDUser))
            {
                // Get the EDDUser database path
                return System.IO.Path.Combine(Tools.GetAppDataDirectory(), "EDDUser.sqlite");
            }
            else if (selector.HasFlag(EDDSqlDbSelection.EDDSystem))
            {
                // Get the EDDSystem database path
                return System.IO.Path.Combine(Tools.GetAppDataDirectory(), "EDDSystem.sqlite");
            }
            else
            {
                // Get the old EDDiscovery database path
                return System.IO.Path.Combine(Tools.GetAppDataDirectory(), "EDDiscovery.sqlite");
            }
        }

        protected void AttachDatabase(EDDSqlDbSelection dbflag, string name)
        {
            // Check if the connection is already connected to the selected database
            using (DbCommand cmd = _cn.CreateCommand("PRAGMA database_list"))
            {
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dbname = reader["name"] as string;
                        if (dbname == name)
                        {
                            return;
                        }
                    }
                }
            }

            // Attach to the selected database under the given schema name
            using (DbCommand cmd = _cn.CreateCommand("ATTACH DATABASE @dbfile AS @dbname"))
            {
                cmd.AddParameterWithValue("@dbfile", GetSQLiteDBFile(dbflag));
                cmd.AddParameterWithValue("@dbname", name);
                cmd.ExecuteNonQuery();
            }
        }

        public abstract DbCommand CreateCommand(string cmd, DbTransaction tn = null);
        public abstract DbTransaction BeginTransaction(IsolationLevel isolevel);
        public abstract DbTransaction BeginTransaction();
        public abstract void Dispose();
        protected abstract void InitializeDatabase();

        protected virtual void Dispose(bool disposing)
        {
            lock (_openConnections)
            {
                SQLiteConnectionED._openConnections.Remove(this);
            }
        }


        #region Settings
        ///----------------------------
        /// STATIC functions for discrete values

        public bool keyExistsCN(string sKey)
        {
            using (DbCommand cmd = CreateCommand("select ID from Register WHERE ID=@key"))
            {
                cmd.AddParameterWithValue("@key", sKey);
                return cmd.ExecuteScalar() != null;
            }
        }

        public int GetSettingIntCN(string key, int defaultvalue)
        {
            try
            {
                using (DbCommand cmd = CreateCommand("SELECT ValueInt from Register WHERE ID = @ID"))
                {
                    cmd.AddParameterWithValue("@ID", key);

                    object ob = cmd.ExecuteScalar();

                    if (ob == null || ob == DBNull.Value)
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

        public bool PutSettingIntCN(string key, int intvalue)
        {
            try
            {
                if (keyExistsCN(key))
                {
                    using (DbCommand cmd = CreateCommand("Update Register set ValueInt = @ValueInt Where ID=@ID"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@ValueInt", intvalue);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                else
                {
                    using (DbCommand cmd = CreateCommand("Insert into Register (ID, ValueInt) values (@ID, @valint)"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@valint", intvalue);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public double GetSettingDoubleCN(string key, double defaultvalue)
        {
            try
            {
                using (DbCommand cmd = CreateCommand("SELECT ValueDouble from Register WHERE ID = @ID"))
                {
                    cmd.AddParameterWithValue("@ID", key);

                    object ob = cmd.ExecuteScalar();

                    if (ob == null || ob == DBNull.Value)
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

        public bool PutSettingDoubleCN(string key, double doublevalue)
        {
            try
            {
                if (keyExistsCN(key))
                {
                    using (DbCommand cmd = CreateCommand("Update Register set ValueDouble = @ValueDouble Where ID=@ID"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@ValueDouble", doublevalue);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                else
                {
                    using (DbCommand cmd = CreateCommand("Insert into Register (ID, ValueDouble) values (@ID, @valdbl)"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@valdbl", doublevalue);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool GetSettingBoolCN(string key, bool defaultvalue)
        {
            return GetSettingIntCN(key, defaultvalue ? 1 : 0) != 0;
        }

        public bool PutSettingBoolCN(string key, bool boolvalue)
        {
            return PutSettingIntCN(key, boolvalue ? 1 : 0);
        }

        public string GetSettingStringCN(string key, string defaultvalue)
        {
            try
            {
                using (DbCommand cmd = CreateCommand("SELECT ValueString from Register WHERE ID = @ID"))
                {
                    cmd.AddParameterWithValue("@ID", key);
                    object ob = cmd.ExecuteScalar();

                    if (ob == null || ob == DBNull.Value)
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

        public bool PutSettingStringCN(string key, string strvalue)
        {
            try
            {
                if (keyExistsCN(key))
                {
                    using (DbCommand cmd = CreateCommand("Update Register set ValueString = @ValueString Where ID=@ID"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@ValueString", strvalue);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                else
                {
                    using (DbCommand cmd = CreateCommand("Insert into Register (ID, ValueString) values (@ID, @valint)"))
                    {
                        cmd.AddParameterWithValue("@ID", key);
                        cmd.AddParameterWithValue("@valint", strvalue);
                        cmd.ExecuteNonQuery();
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
