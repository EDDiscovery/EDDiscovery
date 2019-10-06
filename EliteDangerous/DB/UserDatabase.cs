using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Text;
using System.Linq;
using System.Data;
using SQLLiteExtensions;
using System.Threading;
using EliteDangerousCore.DB;

namespace EliteDangerousCore.DB
{
    public class UserDatabaseConnection : IDisposable
    {
        internal SQLiteConnectionUser2 Connection { get; private set; }

        internal UserDatabaseConnection(bool utc, SQLLiteExtensions.SQLExtConnection.AccessMode mode = SQLLiteExtensions.SQLExtConnection.AccessMode.Reader)
        {
            Connection = new SQLiteConnectionUser2(utc, mode);
        }

        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
        }
    }

    public class UserDatabase
    {
        private class Job : IDisposable
        {
            private ManualResetEventSlim WaitHandle;
            private Action Action;

            public Job(Action action)
            {
                this.Action = action;
                this.WaitHandle = new ManualResetEventSlim(false);
            }

            public void Exec()
            {
                Action.Invoke();
                WaitHandle.Set();
            }

            public void Wait(int timeout = 5000)
            {
                WaitHandle.Wait(timeout);
            }

            public void Dispose()
            {
                this.WaitHandle?.Dispose();
            }
        }

        private UserDatabase()
        {
        }

        public static UserDatabase Instance { get; } = new UserDatabase();


        private ConcurrentQueue<Job> JobQueue = new ConcurrentQueue<Job>();
        private Thread SqlThread;
        private ManualResetEvent StopRequestedEvent = new ManualResetEvent(false);
        private bool StopRequested = false;
        private AutoResetEvent JobQueuedEvent = new AutoResetEvent(false);
        private ManualResetEvent StopCompleted = new ManualResetEvent(true);

        public long? SqlThreadId => SqlThread?.ManagedThreadId;

        private void SqlThreadProc()
        {
            while (!StopRequested)
            {
                switch (WaitHandle.WaitAny(new WaitHandle[] { StopRequestedEvent, JobQueuedEvent }))
                {
                    case 1:
                        while (JobQueue.TryDequeue(out Job job))
                        {
                            job.Exec();
                        }
                        break;
                }
            }

            StopCompleted.Set();
        }

        protected void Execute(Action action, int skipframes = 1, int warnthreshold = 500)
        {
            if (StopCompleted.WaitOne(0))
            {
                throw new ObjectDisposedException(nameof(UserDatabase));
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();

            if (Thread.CurrentThread.ManagedThreadId == SqlThreadId)
            {
                System.Diagnostics.Trace.WriteLine($"UserDatabase Re-entrancy\n{new System.Diagnostics.StackTrace(skipframes, true).ToString()}");
                action();
            }
            else if (Thread.CurrentThread.ManagedThreadId == EliteDangerousCore.DB.SystemsDatabase.Instance.SqlThreadId)
            {
                System.Diagnostics.Trace.WriteLine($"Invalid UserDatabase call from SystemDatabase thread\n{new System.Diagnostics.StackTrace(skipframes, true).ToString()}");
                throw new InvalidOperationException("Invalid UserDatabase call from SystemDatabase thread");
            }
            else
            {
                using (var job = new Job(action))
                {
                    JobQueue.Enqueue(job);
                    JobQueuedEvent.Set();
                    job.Wait(Timeout.Infinite);
                }
            }

            if (sw.ElapsedMilliseconds > warnthreshold)
            {
                var trace = new System.Diagnostics.StackTrace(skipframes, true);
                System.Diagnostics.Trace.WriteLine($"UserDatabase connection held for {sw.ElapsedMilliseconds}ms\n{trace.ToString()}");
            }
        }

        protected T Execute<T>(Func<T> func, int skipframes = 1, int warnthreshold = 500)
        {
            T ret = default(T);
            Execute(() => { ret = func(); }, skipframes + 1, warnthreshold);
            return ret;
        }

        private void ExecuteWithDatabaseInternal(Action<UserDatabaseConnection> action, bool utc = true, bool usetxnlock = false, SQLExtConnection.AccessMode mode = SQLExtConnection.AccessMode.Reader)
        {
            SQLExtTransactionLock<SQLiteConnectionUser2> tl = null;

            try
            {
                if (usetxnlock)
                {
                    tl = new SQLExtTransactionLock<SQLiteConnectionUser2>();
                    if (mode == SQLExtConnection.AccessMode.Reader)
                    {
                        tl.OpenReader();
                    }
                    else
                    {
                        tl.OpenWriter();
                    }
                }

                using (var conn = new UserDatabaseConnection(utc, mode: mode))
                {
                    action(conn);
                }
            }
            finally
            {
                // TBD no trasaction commit?
                tl?.Dispose();
            }
        }

        public void ExecuteWithDatabase(Action<UserDatabaseConnection> action, bool utc = true, bool usetxnlock = false, SQLExtConnection.AccessMode mode = SQLExtConnection.AccessMode.Reader, int warnthreshold = 500)
        {
            Execute(() => ExecuteWithDatabaseInternal(action, utc, usetxnlock, mode), warnthreshold: warnthreshold);
        }

        public T ExecuteWithDatabase<T>(Func<UserDatabaseConnection, T> func, bool utc = true, bool usetxnlock = false, SQLExtConnection.AccessMode mode = SQLExtConnection.AccessMode.Reader, int warnthreshold = 500)
        {
            return Execute(() =>
            {
                T ret = default(T);
                ExecuteWithDatabaseInternal(db => ret = func(db), utc, usetxnlock, mode);
                return ret;
            }, warnthreshold: warnthreshold);
        }

        public void Start()
        {
            StopRequested = false;
            StopRequestedEvent.Reset();
            StopCompleted.Reset();

            if (SqlThread == null)
            {
                SqlThread = new Thread(SqlThreadProc);
                SqlThread.Name = "UserDatabaseThread";
                SqlThread.IsBackground = true;
                SqlThread.Start();
            }
        }

        public void Stop()
        {
            StopRequested = true;
            StopRequestedEvent.Set();
            StopCompleted.WaitOne();
            SqlThread = null;
        }

        public void Initialize()
        {
            Execute(() =>
            {
                SQLiteConnectionSystem.Initialize();
            });
        }

        // Register

        public bool KeyExists(string key)
        {
            return ExecuteWithDatabase(db => SQLiteConnectionUser2.keyExists(key, db.Connection));
        }

        public bool DeleteKey(string key)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.DeleteKey(key,db.Connection));
        }

        public int GetSettingInt(string key, int defaultvalue)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.GetSettingInt(key, defaultvalue, db.Connection));
        }

        public bool PutSettingInt(string key, int intvalue)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.PutSettingInt(key, intvalue, db.Connection));
        }

        public double GetSettingDouble(string key, double defaultvalue)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.GetSettingDouble(key, defaultvalue, db.Connection));
        }

        public bool PutSettingDouble(string key, double doublevalue)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.PutSettingDouble(key, doublevalue, db.Connection));
        }

        public bool GetSettingBool(string key, bool defaultvalue)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.GetSettingBool(key, defaultvalue, db.Connection));
        }

        public bool PutSettingBool(string key, bool boolvalue)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.PutSettingBool(key, boolvalue, db.Connection));
        }

        public string GetSettingString(string key, string defaultvalue)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.GetSettingString(key, defaultvalue, db.Connection));
        }

        public bool PutSettingString(string key, string strvalue)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.PutSettingString(key, strvalue, db.Connection));
        }

        public DateTime GetSettingDate(string key, DateTime defaultvalue)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.GetSettingDate(key, defaultvalue, db.Connection));
        }

        public bool PutSettingDate(string key, DateTime value)
        {
            return ExecuteWithDatabase(db =>  SQLiteConnectionUser2.PutSettingDate(key, value, db.Connection));
        }
    }
}
