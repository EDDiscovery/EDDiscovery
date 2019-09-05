/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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
 
using SQLLiteExtensions;
using System;
using System.Globalization;

namespace EliteDangerousCore.DB
{
    internal class SQLiteConnectionSystem : SQLExtConnectionWithLockRegister<SQLiteConnectionSystem>
    {
        const string tablepostfix = "temp"; // postfix for temp tables

        public SQLiteConnectionSystem() : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath, false, Initialize, AccessMode.ReaderWriter)
        {
        }

        public SQLiteConnectionSystem(AccessMode mode = AccessMode.ReaderWriter) : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath, false, Initialize, mode)
        {
        }

        private SQLiteConnectionSystem(bool utc, Action init) : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath, utc, init, AccessMode.ReaderWriter)
        {
        }

        #region Init

        public static void Initialize()
        {
            InitializeIfNeeded(() =>
            {
                using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem(false, null))       // use this special one so we don't get double init.
                {
                    System.Diagnostics.Debug.WriteLine("Initialise EDSM DB");
                    conn.UpgradeSystemsDB();
                }
            });
        }

        private bool UpgradeSystemsDB()
        {
            try
            {
                ExecuteNonQuery("CREATE TABLE IF NOT EXISTS Register (ID TEXT PRIMARY KEY NOT NULL, ValueInt INTEGER, ValueDouble DOUBLE, ValueString TEXT, ValueBlob BLOB)");

                // BE VERY careful with connections when creating/deleting tables - you end up with SQL Schema errors or it not seeing the table

                SQLExtRegister reg = new SQLExtRegister(this);
                int dbver = reg.GetSettingInt("DBVer", 0);      // use reg, don't use the built in func as they create new connections and confuse the schema

                ExecuteNonQueries(new string[]             // always kill these old tables and make EDDB new table
                    {
                    "DROP TABLE IF EXISTS EddbSystems",
                    "DROP TABLE IF EXISTS Distances",
                    "DROP TABLE IF EXISTS Stations",
                    "DROP TABLE IF EXISTS SystemAliases",
                    "DROP TABLE IF EXISTS station_commodities",
                    "CREATE TABLE IF NOT EXISTS EDDB (edsmid INTEGER PRIMARY KEY NOT NULL, eddbid INTEGER, eddbupdatedat INTEGER, population INTEGER, faction TEXT, government INTEGER, allegiance INTEGER, state INTEGER, security INTEGER, primaryeconomy INTEGER, needspermit INTEGER, power TEXT, powerstate TEXT, properties TEXT)",
                    "CREATE TABLE IF NOT EXISTS Aliases (edsmid INTEGER PRIMARY KEY NOT NULL, edsmid_mergedto INTEGER, name TEXT COLLATE NOCASE)"
                    });

                if (dbver < 102)        // is it older than 102, its unusable
                {
                    ExecuteNonQueries(new string[]         // older than 102, not supporting, remove
                    {
                        "DROP TABLE IF EXISTS EdsmSystems",
                        "DROP TABLE IF EXISTS SystemNames",
                    });

                    reg.DeleteKey("EDSMLastSystems");       // no EDSM system time
                }

                CreateStarTables();                     // ensure we have
                CreateSystemDBTableIndexes();           // ensure they are there 
                DropStarTables(tablepostfix);         // clean out any temp tables half prepared 

                //    conn.Vacuum();  // debug

                if (dbver < 200)
                {
                    reg.PutSettingInt("DBVer", 200);
                    reg.DeleteKey("EDDBSystemsTime");       // force a reload of EDDB
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UpgradeSystemsDB error: " + ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

        #endregion

        #region Time markers

        // time markers - keeping the old code for now, not using better datetime funcs

        static public void ForceEDSMFullUpdate()
        {
            SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", "2010-01-01 00:00:00");
        }

        static public DateTime GetLastEDSMRecordTimeUTC()
        {
            string rwsystime = SQLiteConnectionSystem.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
            DateTime edsmdate;

            if (!DateTime.TryParse(rwsystime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out edsmdate))
                edsmdate = new DateTime(2000, 1, 1);

            return edsmdate;
        }

        static public void SetLastEDSMRecordTimeUTC(DateTime time)
        {
            SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", time.ToString(CultureInfo.InvariantCulture));
            System.Diagnostics.Debug.WriteLine("Last EDSM record " + time.ToString());
        }

        static public DateTime GetLastEDDBDownloadTime()
        {
            return SQLiteConnectionSystem.GetSettingDate("EDDBLastDownloadTime", DateTime.MinValue);    // different ID to 102 on purpose
        }

        static public void SetLastEDDBDownloadTime()
        {
            SQLiteConnectionSystem.PutSettingDate("EDDBLastDownloadTime", DateTime.UtcNow);
        }

        static public void ForceEDDBFullUpdate()
        {
            SQLiteConnectionSystem.PutSettingDate("EDDBLastDownloadTime", DateTime.MinValue);
        }

        #endregion

        #region Helpers

        public void CreateStarTables(string postfix = "")
        {
            ExecuteNonQueries(new string[]
            {
                // purposely not using autoincrement or unique on primary keys - this slows it down.

                "CREATE TABLE IF NOT EXISTS Sectors" + postfix + " (id INTEGER PRIMARY KEY NOT NULL, gridid INTEGER, name TEXT NOT NULL COLLATE NOCASE)",
                "CREATE TABLE IF NOT EXISTS Systems" + postfix + " (edsmid INTEGER PRIMARY KEY NOT NULL , sectorid INTEGER, nameid INTEGER, x INTEGER, y INTEGER, z INTEGER)",
                "CREATE TABLE IF NOT EXISTS Names" + postfix + " (id INTEGER PRIMARY KEY NOT NULL , Name TEXT NOT NULL COLLATE NOCASE )",
            });
        }

        public void DropStarTables(string postfix = "")
        {
            ExecuteNonQueries(new string[]
            {
                "DROP TABLE IF EXISTS Sectors" + postfix,       // dropping the tables kills the indexes
                "DROP TABLE IF EXISTS Systems" + postfix,
                "DROP TABLE IF EXISTS Names" + postfix,
            });
        }

        public void RenameStarTables(string frompostfix, string topostfix)
        {
            ExecuteNonQueries(new string[]
            {
                "ALTER TABLE Sectors" + frompostfix + " RENAME TO Sectors" + topostfix,       
                "ALTER TABLE Systems" + frompostfix + " RENAME TO Systems" + topostfix,       
                "ALTER TABLE Names" + frompostfix + " RENAME TO Names" + topostfix,       
            });
        }

        public void CreateSystemDBTableIndexes() 
        {

            string[] queries = new[]
            {
                "CREATE INDEX IF NOT EXISTS SystemsSectorName ON Systems (sectorid,nameid)",        // worth it for lookups of stars
                "CREATE INDEX IF NOT EXISTS SystemsXZY ON Systems (x,z,y)",        // speeds up searching. 
               
                "CREATE INDEX IF NOT EXISTS NamesName ON Names (Name)",            // improved speed from 9038 (named)/1564 (std) to 516/446ms at minimal cost

                "CREATE INDEX IF NOT EXISTS SectorName ON Sectors (name)",         // name - > entry
                "CREATE INDEX IF NOT EXISTS SectorGridid ON Sectors (gridid)",     // gridid -> entry
            };

            ExecuteNonQueries(queries);
        }

        #endregion

    }
}

