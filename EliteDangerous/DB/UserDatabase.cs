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
        internal SQLiteConnectionUser Connection { get; private set; }

        public UserDatabaseConnection()
        {
            Connection = new SQLiteConnectionUser();
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

    public class UserDatabase : SQLProcessingThread<UserDatabaseConnection>
    {
        private UserDatabase()
        {
        }

        public static UserDatabase Instance { get; } = new UserDatabase();

        public void Initialize()
        {
            ExecuteWithDatabase(cn => { cn.Connection.UpgradeUserDB(); });
        }

        protected override UserDatabaseConnection CreateConnection()
        {
            return new UserDatabaseConnection();
        }

        // Register

        public bool KeyExists(string key)
        {
            return ExecuteWithDatabase(db => db.Connection.keyExists(key));
        }

        public bool DeleteKey(string key)
        {
            return ExecuteWithDatabase(db =>  db.Connection.DeleteKey(key));
        }

        public int GetSettingInt(string key, int defaultvalue)
        {
            return ExecuteWithDatabase(db =>  db.Connection.GetSettingInt(key, defaultvalue));
        }

        public bool PutSettingInt(string key, int intvalue)
        {
            return ExecuteWithDatabase(db =>  db.Connection.PutSettingInt(key, intvalue));
        }

        public double GetSettingDouble(string key, double defaultvalue)
        {
            return ExecuteWithDatabase(db =>  db.Connection.GetSettingDouble(key, defaultvalue));
        }

        public bool PutSettingDouble(string key, double doublevalue)
        {
            return ExecuteWithDatabase(db =>  db.Connection.PutSettingDouble(key, doublevalue));
        }

        public bool GetSettingBool(string key, bool defaultvalue)
        {
            return ExecuteWithDatabase(db =>  db.Connection.GetSettingBool(key, defaultvalue));
        }

        public bool PutSettingBool(string key, bool boolvalue)
        {
            return ExecuteWithDatabase(db =>  db.Connection.PutSettingBool(key, boolvalue));
        }

        public string GetSettingString(string key, string defaultvalue)
        {
            return ExecuteWithDatabase(db =>  db.Connection.GetSettingString(key, defaultvalue));
        }

        public bool PutSettingString(string key, string strvalue)
        {
            return ExecuteWithDatabase(db =>  db.Connection.PutSettingString(key, strvalue));
        }

        public DateTime GetSettingDate(string key, DateTime defaultvalue)
        {
            return ExecuteWithDatabase(db =>  db.Connection.GetSettingDate(key, defaultvalue));
        }

        public bool PutSettingDate(string key, DateTime value)
        {
            return ExecuteWithDatabase(db =>  db.Connection.PutSettingDate(key, value));
        }
    }
}
