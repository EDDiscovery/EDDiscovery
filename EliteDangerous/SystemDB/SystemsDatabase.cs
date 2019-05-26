using SQLLiteExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
        private SystemsDatabase()
        {
        }

        public static SystemsDatabase Instance { get; } = new SystemsDatabase();

        protected void Execute(Action action, int skipframes = 1)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            action();
            if (sw.ElapsedMilliseconds > 100)
            {
                var trace = new System.Diagnostics.StackTrace(skipframes, true);
                System.Diagnostics.Trace.WriteLine($"SystemsDatabase connection held for {sw.ElapsedMilliseconds}ms\n{trace.ToString()}");
            }
        }

        protected T Execute<T>(Func<T> func, int skipframes = 1)
        {
            T ret = default(T);
            Execute(() => ret = func(), skipframes + 1);
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

        #endregion
    }
}
