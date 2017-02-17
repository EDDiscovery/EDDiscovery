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
        #region Database access
        ///----------------------------
        /// STATIC code helpers for other DB classes

        public static DataSet SQLQueryText(SQLiteConnectionED cn, DbCommand cmd)  
        {
            try
            {
                DataSet ds = new DataSet();
                DbDataAdapter da = cn.CreateDataAdapter(cmd);
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
