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

        public UserDatabaseConnection()
        {
            Connection = new SQLiteConnectionUser2();
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
