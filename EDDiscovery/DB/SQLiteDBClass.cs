﻿using EDDiscovery2.DB;
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
    public class SQLiteTxnLockED<TConn> : IDisposable
        where TConn : SQLiteConnectionED
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
                SQLiteTxnLockED<TConn> txnlock = weakref.Target as SQLiteTxnLockED<TConn>;

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
                    if (_locktimer != null)
                    {
                        _locktimer.Dispose();
                        _locktimer = null;
                    }

                    Monitor.Exit(_transactionLock);
                }
                else
                {
                    // This thread doesn't own the lock, so we need to create
                    // a new lock object and interrupt all waiting threads
                    Console.WriteLine("ERROR: Transaction Lock Leaked");

                    lock (_waitingthreads)
                    {
                        if (_locktimer != null)
                        {
                            _locktimer.Dispose();
                            _locktimer = null;
                        }

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
    public class SQLiteTransactionED<TConn> : DbTransaction
        where TConn : SQLiteConnectionED
    {
        private SQLiteTxnLockED<TConn> _transactionLock = null;

        public DbTransaction InnerTransaction { get; private set; }

        public SQLiteTransactionED(DbTransaction txn, SQLiteTxnLockED<TConn> txnlock)
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
    public class SQLiteDataReaderED<TConn> : DbDataReader
        where TConn : SQLiteConnectionED
    {
        // This is the wrapped reader
        protected DbDataReader InnerReader { get; set; }
        protected DbCommand _command;
        protected SQLiteTransactionED<TConn> _transaction;
        protected SQLiteTxnLockED<TConn> _txnlock;

        public SQLiteDataReaderED(DbCommand cmd, CommandBehavior behaviour, SQLiteTransactionED<TConn> txn = null, SQLiteTxnLockED<TConn> txnlock = null)
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
    public class SQLiteCommandED<TConn> : DbCommand
        where TConn : SQLiteConnectionED
    {
        // This is the wrapped transaction
        protected SQLiteTransactionED<TConn> _transaction;
        protected SQLiteConnectionED _connection;

        public SQLiteCommandED(DbCommand cmd, SQLiteConnectionED conn, DbTransaction txn = null)
        {
            _connection = conn;
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
        protected override DbConnection DbConnection { get { return InnerCommand.Connection; } set { throw new InvalidOperationException("Cannot change connection of command"); } }
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
                return new SQLiteDataReaderED<TConn>(this.InnerCommand, behavior, txn: this._transaction);
            }
            else
            {
                // Take the transaction lock for the duration of this command
                using (var txnlock = new SQLiteTxnLockED<TConn>())
                {
                    txnlock.Open();
                    return new SQLiteDataReaderED<TConn>(this.InnerCommand, behavior, txnlock: txnlock);
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
                using (var txnlock = new SQLiteTxnLockED<TConn>())
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
                using (var txnlock = new SQLiteTxnLockED<TConn>())
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
            if (txn == null || txn is SQLiteTransactionED<TConn>)
            {
                _transaction = (SQLiteTransactionED<TConn>)txn;
                InnerCommand.Transaction = _transaction.InnerTransaction;
            }
            else
            {
                throw new InvalidOperationException(String.Format("Expected a {0}; got a {1}", typeof(SQLiteTransactionED<TConn>).FullName, txn.GetType().FullName));
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

                SQLiteConnectionUser.Initialize();
                SQLiteConnectionSystem.Initialize();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error creating data base file, Exception", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        public static void ExecuteQuery(SQLiteConnectionED conn, string query)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("SplitDatabase error: " + ex.Message);
                MessageBox.Show(ex.StackTrace);
                return false;
            }
            return true;
        }

        
     
        public static void PerformUpgrade(SQLiteConnectionED conn, int newVersion, bool catchErrors, bool backupDbFile, string[] queries, Action doAfterQueries = null)
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

            conn.PutSettingIntCN("DBVer", newVersion);
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
            return SQLiteConnectionUser.keyExists(sKey);
        }

        static public int GetSettingInt(string key, int defaultvalue)
        {
            return SQLiteConnectionUser.GetSettingInt(key, defaultvalue);
        }

        static public bool PutSettingInt(string key, int intvalue)
        {
            return SQLiteConnectionUser.PutSettingInt(key, intvalue);
        }

        static public double GetSettingDouble(string key, double defaultvalue)
        {
            return SQLiteConnectionUser.GetSettingDouble(key, defaultvalue);
        }

        static public bool PutSettingDouble(string key, double doublevalue)
        {
            return SQLiteConnectionUser.PutSettingDouble(key, doublevalue);
        }

        static public bool GetSettingBool(string key, bool defaultvalue)
        {
            return SQLiteConnectionUser.GetSettingBool(key, defaultvalue);
        }

        static public bool PutSettingBool(string key, bool boolvalue)
        {
            return SQLiteConnectionUser.PutSettingBool(key, boolvalue);
        }

        static public string GetSettingString(string key, string defaultvalue)
        {
            return SQLiteConnectionUser.GetSettingString(key, defaultvalue);
        }

        static public bool PutSettingString(string key, string strvalue)
        {
            return SQLiteConnectionUser.PutSettingString(key, strvalue);
        }
        #endregion
    }
}
