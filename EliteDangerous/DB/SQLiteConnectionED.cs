/*
 * Copyright © 2015 - 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;
using System.IO;


namespace EliteDangerousCore.DB
{
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

        public static bool IsInitialized { get { return _initialized; } }

        private static ReaderWriterLockSlim _schemaLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private static bool _initialized = false;
        private static int _initsem = 0;
        private static ManualResetEvent _initbarrier = new ManualResetEvent(false);
        private SQLiteTxnLockED<TConn> _transactionLock;
        protected static Dictionary<string, RegisterEntry> EarlyRegister;

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

        protected static void InitializeIfNeeded(Action initializer)
        {
            if (!_initialized)
            {
                int cur = Interlocked.Increment(ref _initsem);

                if (cur == 1)
                {
                    using (var slock = new SchemaLock())
                    {
                        _initbarrier.Set();
                        initializer();
                        _initialized = true;
                    }
                }

                if (!_initialized)
                {
                    _initbarrier.WaitOne();
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
                    System.Diagnostics.Trace.WriteLine($"Database {typeof(TConn).Name} initialized before Initialize()");
                    System.Diagnostics.Trace.WriteLine(new System.Diagnostics.StackTrace(2, true).ToString());

                    if (typeof(TConn) == typeof(SQLiteConnectionUser))
                    {
                        SQLiteConnectionUser.Initialize();
                    }
                    else if (typeof(TConn) == typeof(SQLiteConnectionSystem))
                    {
                        SQLiteConnectionSystem.Initialize();
                    }
                }

                _schemaLock.EnterReadLock();
                locktaken = true;

                // System.Threading.Monitor.Enter(monitor);
                //Console.WriteLine("Connection open " + System.Threading.Thread.CurrentThread.Name);
                DBFile = GetSQLiteDBFile(maindb ?? EDDSqlDbSelection.EDDUser);
                _cn = DbFactory.CreateConnection();

                // Use the database selected by maindb as the 'main' database
                _cn.ConnectionString = "Data Source=" + DBFile.Replace("\\", "\\\\") + ";Pooling=true;";

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

        public override DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            DbDataAdapter da = DbFactory.CreateDataAdapter();
            da.SelectCommand = cmd;
            return da;
        }

        public override DbCommand CreateCommand(string query, DbTransaction tn = null)
        {
            AssertThreadOwner();
            DbCommand cmd = _cn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
            cmd.CommandText = query;
            return new SQLiteCommandED<TConn>(cmd, this, _transactionLock, tn);
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

        protected static T RegisterGet<T>(string key, T defval, Func<RegisterEntry, T> early, Func<TConn, T> normal, TConn conn)
        {
            if (conn != null)
            {
                return normal(conn);
            }

            if (!_initialized && EarlyRegister != null)
            {
                return EarlyRegister.ContainsKey(key) ? early(EarlyRegister[key]) : defval;
            }
            else
            {
                if (!_initialized && !_schemaLock.IsWriteLockHeld)
                {
                    throw new InvalidOperationException("Read from register before EarlyReadRegister()");
                }

                using (TConn cn = new TConn())
                {
                    return normal(cn);
                }
            }
        }

        protected static bool RegisterPut(Func<TConn, bool> action, TConn conn)
        {
            if (conn != null)
            {
                return action(conn);
            }

            if (!_initialized && !_schemaLock.IsWriteLockHeld)
            {
                System.Diagnostics.Trace.WriteLine("Write to register before Initialize()");
            }

            using (TConn cn = new TConn())
            {
                return action(cn);
            }
        }

        protected static bool RegisterDelete(string key , Func<TConn, bool> action, TConn conn)
        {
            if (conn != null)
            {
                return action(conn);
            }

            if (!_initialized && EarlyRegister != null)
            {
                EarlyRegister.Remove(key);
                return true;
            }
            else
            {
                if (!_initialized && !_schemaLock.IsWriteLockHeld)
                {
                    System.Diagnostics.Trace.WriteLine("Delete register before Initialize()");
                }

                using (TConn cn = new TConn())
                {
                    return action(cn);
                }
            }
        }

        static public bool keyExists(string sKey, TConn conn = null)
        {
            return RegisterGet(sKey, false, r => true, cn => cn.keyExistsCN(sKey), conn);
        }

        static public bool DeleteKey(string key, TConn conn = null)
        {
            return RegisterDelete(key, cn => cn.DeleteKeyCN(key), conn);
        }

        static public int GetSettingInt(string key, int defaultvalue, TConn conn = null)
        {
            return (int)RegisterGet(key, defaultvalue, r => r.ValueInt, cn => cn.GetSettingIntCN(key, defaultvalue), conn);
        }

        static public bool PutSettingInt(string key, int intvalue, TConn conn = null)
        {
            return RegisterPut(cn => cn.PutSettingIntCN(key, intvalue), conn);
        }

        static public double GetSettingDouble(string key, double defaultvalue, TConn conn = null)
        {
            return RegisterGet(key, defaultvalue, r => r.ValueDouble, cn => cn.GetSettingDoubleCN(key, defaultvalue), conn);
        }

        static public bool PutSettingDouble(string key, double doublevalue, TConn conn = null)
        {
            return RegisterPut(cn => cn.PutSettingDoubleCN(key, doublevalue), conn);
        }

        static public bool GetSettingBool(string key, bool defaultvalue, TConn conn = null)
        {
            return RegisterGet(key, defaultvalue, r => r.ValueInt != 0, cn => cn.GetSettingBoolCN(key, defaultvalue), conn);
        }

        static public bool PutSettingBool(string key, bool boolvalue, TConn conn = null)
        {
            return RegisterPut(cn => cn.PutSettingBoolCN(key, boolvalue), conn);
        }

        static public string GetSettingString(string key, string defaultvalue, TConn conn = null)
        {
            return RegisterGet(key, defaultvalue, r => r.ValueString, cn => cn.GetSettingStringCN(key, defaultvalue), conn);
        }

        static public bool PutSettingString(string key, string strvalue, TConn conn = null)        // public IF
        {
            return RegisterPut(cn => cn.PutSettingStringCN(key, strvalue), conn);
        }

        protected void GetRegister(Dictionary<string, RegisterEntry> regs)
        {
            using (DbCommand cmd = CreateCommand("SELECT Id, ValueInt, ValueDouble, ValueBlob, ValueString FROM register"))
            {
                using (DbDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        string id = (string)rdr["Id"];
                        object valint = rdr["ValueInt"];
                        object valdbl = rdr["ValueDouble"];
                        object valblob = rdr["ValueBlob"];
                        object valstr = rdr["ValueString"];
                        regs[id] = new RegisterEntry(
                            valstr as string,
                            valblob as byte[],
                            (valint as long?) ?? 0L,
                            (valdbl as double?) ?? Double.NaN
                        );
                    }
                }
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
        protected static DbProviderFactory DbFactory = GetSqliteProviderFactory();

        protected SQLiteConnectionED(bool initializing)
        {
            lock (_openConnections)
            {
                _openConnections.Add(this);
            }
            _owningThread = Thread.CurrentThread;
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
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return false;
            }

            try
            {
                // This will throw an exception if the SQLite.Interop.dll can't be loaded.
                string sqliteversion = SQLiteConnection.SQLiteVersion;

                if (!String.IsNullOrEmpty(sqliteversion))
                {
                    System.Diagnostics.Trace.WriteLine($"SQLite Version {sqliteversion}");
                    return true;
                }
            }
            catch
            {
            }

            return false;
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
#pragma warning disable 618
                var asm = System.Reflection.Assembly.LoadWithPartialName("Mono.Data.Sqlite");
#pragma warning restore 618
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
                return EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath;
            }
            else if (selector.HasFlag(EDDSqlDbSelection.EDDSystem))
            {
                // Get the EDDSystem database path
                return EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath;
            }
            else
            {
                // Get the old EDDiscovery database path - no longer supported
                System.Diagnostics.Debug.Assert(false);
                return EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath; // just to make the code compile
            }
        }

        protected void AttachDatabase(EDDSqlDbSelection dbflag, string name)
        {
            // Check if the connection is already connected to the selected database
            using (DbCommand cmd = CreateCommand("PRAGMA database_list"))
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
            using (DbCommand cmd = CreateCommand("ATTACH DATABASE @dbfile AS @dbname"))
            {
                cmd.AddParameterWithValue("@dbfile", GetSQLiteDBFile(dbflag));
                cmd.AddParameterWithValue("@dbname", name);
                cmd.ExecuteNonQuery();
            }
        }

        protected static void PerformUpgrade(SQLiteConnectionED conn, int newVersion, bool catchErrors, bool backupDbFile, string[] queries, Action doAfterQueries = null)
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
                ExtendedControls.MessageBoxTheme.Show($"UpgradeDB{newVersion} error: " + ex.Message);
            }

            doAfterQueries?.Invoke();

            conn.PutSettingIntCN("DBVer", newVersion);
        }

        protected static void ExecuteQuery(SQLiteConnectionED conn, string query)
        {
            conn.ExecuteQuery(query);
        }

        public void ExecuteQuery(string query)
        {
            using (DbCommand command = CreateCommand(query))
                command.ExecuteNonQuery();
        }

        public abstract DbCommand CreateCommand(string cmd, DbTransaction tn = null);
        public abstract DbTransaction BeginTransaction(IsolationLevel isolevel);
        public abstract DbTransaction BeginTransaction();
        public abstract void Dispose();
        public abstract DbDataAdapter CreateDataAdapter(DbCommand cmd);

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

        public bool DeleteKeyCN(string sKey)        // SQL wildcards
        {
            using (DbCommand cmd = CreateCommand("Delete from Register WHERE ID like @key"))
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
