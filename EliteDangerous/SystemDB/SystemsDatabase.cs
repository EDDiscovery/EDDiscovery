using SQLLiteExtensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace EliteDangerousCore.DB
{
    public class SystemsDatabaseConnection : IDisposable
    {
        internal SQLiteConnectionSystem Connection { get; private set; }

        internal SystemsDatabaseConnection(SQLLiteExtensions.SQLExtConnection.AccessMode mode = SQLLiteExtensions.SQLExtConnection.AccessMode.Reader)
        {
            Connection = new SQLiteConnectionSystem(mode);
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

    public class SystemsDatabase
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

            public void Wait()
            {
                WaitHandle.Wait();
            }

            public void Dispose()
            {
                this.WaitHandle?.Dispose();
            }
        }

        private SystemsDatabase()
        {
        }

        public static SystemsDatabase Instance { get; } = new SystemsDatabase();

        private ConcurrentQueue<Job> JobQueue = new ConcurrentQueue<Job>();
        private Thread SqlThread;
        private ManualResetEvent StopRequestedEvent = new ManualResetEvent(false);
        private bool StopRequested = false;
        private AutoResetEvent JobQueuedEvent = new AutoResetEvent(false);
        private ManualResetEvent StopCompleted = new ManualResetEvent(true);

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
                throw new ObjectDisposedException(nameof(SystemsDatabase));
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();

            using (var job = new Job(action))
            {
                JobQueue.Enqueue(job);
                JobQueuedEvent.Set();
                job.Wait();
            }

            if (sw.ElapsedMilliseconds > warnthreshold)
            {
                var trace = new System.Diagnostics.StackTrace(skipframes, true);
                System.Diagnostics.Trace.WriteLine($"SystemsDatabase connection held for {sw.ElapsedMilliseconds}ms\n{trace.ToString()}");
            }
        }

        protected T Execute<T>(Func<T> func, int skipframes = 1, int warnthreshold = 500)
        {
            T ret = default(T);
            Execute(() => { ret = func(); }, skipframes + 1, warnthreshold);
            return ret;
        }

        private void ExecuteWithDatabaseInternal(Action<SystemsDatabaseConnection> action, bool usetxnlock = false, SQLExtConnection.AccessMode mode = SQLExtConnection.AccessMode.Reader)
        {
            SQLExtTransactionLock<SQLiteConnectionSystem> tl = null;

            try
            {
                if (usetxnlock)
                {
                    tl = new SQLExtTransactionLock<SQLiteConnectionSystem>();
                    if (mode == SQLExtConnection.AccessMode.Reader)
                    {
                        tl.OpenReader();
                    }
                    else
                    {
                        tl.OpenWriter();
                    }
                }

                using (var conn = new SystemsDatabaseConnection(mode: mode))
                {
                    action(conn);
                }
            }
            finally
            {
                tl?.Dispose();
            }
        }

        public void ExecuteWithDatabase(Action<SystemsDatabaseConnection> action, bool usetxnlock = false, SQLExtConnection.AccessMode mode = SQLExtConnection.AccessMode.Reader)
        {
            Execute(() => ExecuteWithDatabaseInternal(action, usetxnlock, mode));
        }

        public T ExecuteWithDatabase<T>(Func<SystemsDatabaseConnection, T> func, bool usetxnlock = false, SQLExtConnection.AccessMode mode = SQLExtConnection.AccessMode.Reader)
        {
            return Execute(() =>
            {
                T ret = default(T);
                ExecuteWithDatabaseInternal(db => ret = func(db), usetxnlock, mode);
                return ret;
            });
        }

        public void Start()
        {
            StopRequested = false;
            StopRequestedEvent.Reset();
            StopCompleted.Reset();

            if (SqlThread == null)
            {
                SqlThread = new Thread(SqlThreadProc);
                SqlThread.Name = "SystemsDatabaseThread";
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
            Execute(() => SQLiteConnectionSystem.Initialize());
        }

        public long UpgradeSystemTableFromFile(string filename, bool[] gridids, Func<bool> cancelRequested, Action<string> reportProgress)
        {
            return Execute(() => SQLiteConnectionSystem.UpgradeSystemTableFromFile(filename, gridids, cancelRequested, reportProgress));
        }

        public void UpgradeSystemTableFrom102TypeDB(Func<bool> cancelRequested, Action<string> reportProgress, bool fullsyncrequested)
        {
            Execute(() => SQLiteConnectionSystem.UpgradeSystemTableFrom102TypeDB(cancelRequested, reportProgress, fullsyncrequested));
        }

        public string GetEDSMGridIDs()
        {
            return Execute(() => SQLiteConnectionSystem.GetSettingString("EDSMGridIDs", "Not Set"));
        }

        public bool SetEDSMGridIDs(string value)
        {
            return Execute(() => SQLiteConnectionSystem.PutSettingString("EDSMGridIDs", value));
        }

        public DateTime GetEDSMGalMapLast()
        {
            return Execute(() => SQLiteConnectionSystem.GetSettingDate("EDSMGalMapLast", DateTime.MinValue));
        }

        public bool SetEDSMGalMapLast(DateTime value)
        {
            return Execute(() => SQLiteConnectionSystem.PutSettingDate("EDSMGalMapLast", value));
        }

        #region Time markers

        // time markers - keeping the old code for now, not using better datetime funcs

        public void ForceEDSMFullUpdate()
        {
            Execute(() => SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", "2010-01-01 00:00:00"));
        }

        public DateTime GetLastEDSMRecordTimeUTC()
        {
            return Execute(() =>
            {
                string rwsystime = SQLiteConnectionSystem.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
                DateTime edsmdate;

                if (!DateTime.TryParse(rwsystime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out edsmdate))
                    edsmdate = new DateTime(2000, 1, 1);

                return edsmdate;
            });
        }

        public void SetLastEDSMRecordTimeUTC(DateTime time)
        {
            Execute(() =>
            {
                SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", time.ToString(CultureInfo.InvariantCulture));
                System.Diagnostics.Debug.WriteLine("Last EDSM record " + time.ToString());
            });
        }

        public DateTime GetLastEDDBDownloadTime()
        {
            return Execute(() => SQLiteConnectionSystem.GetSettingDate("EDDBLastDownloadTime", DateTime.MinValue));
        }

        public void SetLastEDDBDownloadTime()
        {
            Execute(() => SQLiteConnectionSystem.PutSettingDate("EDDBLastDownloadTime", DateTime.UtcNow));
        }

        public void ForceEDDBFullUpdate()
        {
            Execute(() => SQLiteConnectionSystem.PutSettingDate("EDDBLastDownloadTime", DateTime.MinValue));
        }

        public int GetEDSMSectorIDNext()
        {
            return Execute(() => SQLiteConnectionSystem.GetSettingInt("EDSMSectorIDNext", 1));
        }

        public void SetEDSMSectorIDNext(int val)
        {
            Execute(() => SQLiteConnectionSystem.PutSettingInt("EDSMSectorIDNext", val));
        }

        #endregion
    }
}
