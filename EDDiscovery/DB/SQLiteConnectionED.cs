using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;

namespace EDDiscovery.DB
{
    public class SQLiteConnectionOld : SQLiteConnectionED
    {
        public SQLiteConnectionOld() : base(EDDSqlDbSelection.EDDiscovery)
        {
        }
    }

    public class SQLiteConnectionUser : SQLiteConnectionED
    {
        public SQLiteConnectionUser() : base(SQLiteDBClass.UserDatabase)
        {
        }
    }

    public class SQLiteConnectionSystem : SQLiteConnectionED
    {
        public SQLiteConnectionSystem() : base(SQLiteDBClass.SystemDatabase)
        {
        }
    }

    public class SQLiteConnectionCombined : SQLiteConnectionED
    {
        public SQLiteConnectionCombined() : base(EDDSqlDbSelection.None, EDDSqlDbSelection.EDDUser | EDDSqlDbSelection.EDDSystem)
        {
        }
    }

    public abstract class SQLiteConnectionED : IDisposable              // USE this for connections.. 
    {
        private static ReaderWriterLockSlim _schemaLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

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

        //static Object monitor = new Object();                 // monitor disabled for now - it will prevent SQLite DB locked errors but 
        // causes the program to become unresponsive during big DB updates
        private DbConnection _cn;

        public SQLiteConnectionED(EDDSqlDbSelection? maindb = null, EDDSqlDbSelection selector = EDDSqlDbSelection.None)
        {
            bool locktaken = false;
            try
            {
                _schemaLock.EnterReadLock();
                locktaken = true;

                // System.Threading.Monitor.Enter(monitor);
                //Console.WriteLine("Connection open " + System.Threading.Thread.CurrentThread.Name);
                _cn = SQLiteDBClass.CreateCN(maindb ?? SQLiteDBClass.DefaultMainDatabase, selector);
            }
            catch
            {
                if (locktaken)
                {
                    _schemaLock.ExitReadLock();
                }

                throw;
            }
        }

        public string DBFile
        {
            get
            {
                SQLiteConnection sqlite = (SQLiteConnection) _cn;
                return sqlite.FileName;
            }
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

                if (_schemaLock.IsReadLockHeld)
                {
                    _schemaLock.ExitReadLock();
                }
            }
        }
    }
}
