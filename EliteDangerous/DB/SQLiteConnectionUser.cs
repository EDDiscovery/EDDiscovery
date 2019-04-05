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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using SQLLiteExtensions;

namespace EliteDangerousCore.DB
{
    public class SQLiteConnectionUser : SQLExtConnectionWithLockRegister<SQLiteConnectionUser>
    {
        protected static List<EDCommander> EarlyCommanders;

        public SQLiteConnectionUser() : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath, false, Initialize, AccessMode.ReaderWriter)
        {  
        }

        public SQLiteConnectionUser(bool utc = true, AccessMode mode = AccessMode.ReaderWriter) : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath, utc, Initialize, mode)
        {
        }

        private SQLiteConnectionUser(bool utc, Action init) : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath, utc, init, AccessMode.ReaderWriter)
        {       // used just for init
        }

        public static void Initialize()
        {
            InitializeIfNeeded(() =>
            {
                using (SQLiteConnectionUser conn = new SQLiteConnectionUser(true,null))  // use this special one so we don't get double init.  the flag which stops this has not been set.
                {
                    System.Diagnostics.Debug.WriteLine("Initialise USER DB");
                    UpgradeUserDB(conn);
                }
            });
        }

        protected static bool UpgradeUserDB(SQLiteConnectionUser conn)
        {
            int dbver;
            try
            {
                conn.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS Register (ID TEXT PRIMARY KEY NOT NULL, ValueInt INTEGER, ValueDouble DOUBLE, ValueString TEXT, ValueBlob BLOB)");

                SQLExtRegister reg = new SQLExtRegister(conn);
                dbver = reg.GetSettingInt("DBVer", 1);        // use the constring one, as don't want to go back into ConnectionString code

                DropOldUserTables(conn);

                if (dbver < 2)
                    UpgradeUserDB2(conn);

                if (dbver < 4)
                    UpgradeUserDB4(conn);

                if (dbver < 7)
                    UpgradeUserDB7(conn);

                if (dbver < 9)
                    UpgradeUserDB9(conn);

                if (dbver < 10)
                    UpgradeUserDB10(conn);

                if (dbver < 11)
                    UpgradeUserDB11(conn);

                if (dbver < 12)
                    UpgradeUserDB12(conn);

                if (dbver < 16)
                    UpgradeUserDB16(conn);

                if (dbver < 101)
                    UpgradeUserDB101(conn);

                if (dbver < 102)
                    UpgradeUserDB102(conn);

                if (dbver < 103)
                    UpgradeUserDB103(conn);

                if (dbver < 104)
                    UpgradeUserDB104(conn);

                if (dbver < 105)
                    UpgradeUserDB105(conn);

                if (dbver < 106)
                    UpgradeUserDB106(conn);

                if (dbver < 107)
                    UpgradeUserDB107(conn);

                if (dbver < 108)
                    UpgradeUserDB108(conn);

                if (dbver < 109)
                    UpgradeUserDB109(conn);

                if (dbver < 110)
                    UpgradeUserDB110(conn);

                if (dbver < 111)
                    UpgradeUserDB111(conn);

                if (dbver < 112)
                    UpgradeUserDB112(conn);

                if (dbver < 113)
                    UpgradeUserDB113(conn);

                if (dbver < 114)
                    UpgradeUserDB114(conn);

                if (dbver < 115)
                    UpgradeUserDB115(conn);

                if (dbver < 116)
                    UpgradeUserDB116(conn);

                if (dbver < 117)
                    UpgradeUserDB117(conn);

                if (dbver < 118)
                    UpgradeUserDB118(conn);

                if (dbver < 119)
                    UpgradeUserDB119(conn);

                if (dbver < 120)
                    UpgradeUserDB120(conn);

                CreateUserDBTableIndexes(conn);

                return true;
            }
            catch (Exception ex)
            {
                ExtendedControls.MessageBoxTheme.Show("UpgradeUserDB error: " + ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

        private static void UpgradeUserDB2(SQLExtConnection conn)
        {
            string query4 = "CREATE TABLE SystemNote (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Time DATETIME NOT NULL )";

            conn.PerformUpgrade(2, false, false, new[] { query4 });
        }

        private static void UpgradeUserDB4(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN Note TEXT";
            conn.PerformUpgrade(4, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB7(SQLExtConnection conn)
        {
            string query3 = "CREATE TABLE TravelLogUnit(id INTEGER PRIMARY KEY  NOT NULL, type INTEGER NOT NULL, name TEXT NOT NULL, size INTEGER, path TEXT)";
            conn.PerformUpgrade(7, true, false, new[] { query3 });
        }

        private static void UpgradeUserDB9(SQLExtConnection conn)
        {
            string query1 = "CREATE TABLE Objects (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , SystemName TEXT NOT NULL , ObjectName TEXT NOT NULL , ObjectType INTEGER NOT NULL , ArrivalPoint Float, Gravity FLOAT, Atmosphere Integer, Vulcanism Integer, Terrain INTEGER, Carbon BOOL, Iron BOOL, Nickel BOOL, Phosphorus BOOL, Sulphur BOOL, Arsenic BOOL, Chromium BOOL, Germanium BOOL, Manganese BOOL, Selenium BOOL NOT NULL , Vanadium BOOL, Zinc BOOL, Zirconium BOOL, Cadmium BOOL, Mercury BOOL, Molybdenum BOOL, Niobium BOOL, Tin BOOL, Tungsten BOOL, Antimony BOOL, Polonium BOOL, Ruthenium BOOL, Technetium BOOL, Tellurium BOOL, Yttrium BOOL, Commander  Text, UpdateTime DATETIME, Status INTEGER )";
            conn.PerformUpgrade(9, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB10(SQLExtConnection conn)
        {
            string query1 = "CREATE TABLE wanted_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, systemname TEXT UNIQUE NOT NULL)";
            conn.PerformUpgrade(10, true, false, new[] { query1 });
        }


        private static void UpgradeUserDB11(SQLExtConnection conn)
        {
            string query2 = "ALTER TABLE Objects ADD COLUMN Landed BOOL";
            string query3 = "ALTER TABLE Objects ADD COLUMN terraform Integer";
            conn.PerformUpgrade(11, true, false, new[] { query2, query3 });
        }

        private static void UpgradeUserDB12(SQLExtConnection conn)
        {
            string query1 = "CREATE TABLE routes_expeditions (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT UNIQUE NOT NULL, start DATETIME, end DATETIME)";
            string query2 = "CREATE TABLE route_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, routeid INTEGER NOT NULL, systemname TEXT NOT NULL)";
            conn.PerformUpgrade(12, true, false, new[] { query1, query2 });
        }


        private static void UpgradeUserDB16(SQLExtConnection conn)
        {
            string query = "CREATE TABLE Bookmarks (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , StarName TEXT, x double NOT NULL, y double NOT NULL, z double NOT NULL, Time DATETIME NOT NULL, Heading TEXT, Note TEXT NOT Null )";
            conn.PerformUpgrade(16, true, false, new[] { query });
        }

        private static void UpgradeUserDB101(SQLExtConnection conn)
        {
            string query1 = "DROP TABLE IF EXISTS Systems";
            string query2 = "DROP TABLE IF EXISTS SystemAliases";
            string query3 = "DROP TABLE IF EXISTS Distances";
            string query4 = "VACUUM";

            conn.PerformUpgrade(101, true, false, new[] { query1, query2, query3, query4 });
        }

        private static void UpgradeUserDB102(SQLExtConnection conn)
        {
            string query1 = "CREATE TABLE Commanders (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, EdsmApiKey TEXT NOT NULL, NetLogDir TEXT, Deleted INTEGER NOT NULL)";

            conn.PerformUpgrade(102, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB103(SQLiteConnectionUser conn)
        {
            string query1 = "CREATE TABLE Journals ( " +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "Type INTEGER NOT NULL, " +
                "Name TEXT NOT NULL COLLATE NOCASE, " +
                "Path TEXT COLLATE NOCASE, " +
                "CommanderId INTEGER REFERENCES Commanders(Id), " +
                "Size INTEGER " +
                ") ";


            string query2 = "CREATE TABLE JournalEntries ( " +
                 "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                 "JournalId INTEGER NOT NULL REFERENCES Journals(Id), " +
                 "EventTypeId INTEGER NOT NULL, " +
                 "EventType TEXT, " +
                 "EventTime DATETIME NOT NULL, " +
                 "EventData TEXT, " + //--JSON String of complete line" +
                 "EdsmId INTEGER, " + //--0 if not set yet." +
                 "Synced INTEGER " +
                 ")";

            conn.PerformUpgrade(103, true, false, new[] { query1, query2 });
        }

        private static void UpgradeUserDB104(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN journalid Integer NOT NULL DEFAULT 0";
            conn.PerformUpgrade(104, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB105(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE TravelLogUnit ADD COLUMN CommanderId INTEGER REFERENCES Commanders(Id) ";
            string query2 = "DROP TABLE IF EXISTS JournalEntries";
            string query3 = "CREATE TABLE JournalEntries ( " +
                 "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                 "TravelLogId INTEGER NOT NULL REFERENCES TravelLogUnit(Id), " +
                 "CommanderId INTEGER NOT NULL DEFAULT 0," +
                 "EventTypeId INTEGER NOT NULL, " +
                 "EventType TEXT, " +
                 "EventTime DATETIME NOT NULL, " +
                 "EventData TEXT, " + //--JSON String of complete line" +
                 "EdsmId INTEGER, " + //--0 if not set yet." +
                 "Synced INTEGER " +
                 ")";


            conn.PerformUpgrade(105, true, false, new[] { query1, query2, query3 });
        }

        private static void UpgradeUserDB106(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN EdsmId INTEGER NOT NULL DEFAULT -1";
            conn.PerformUpgrade(106, true, false, new[] { query1 });
        }


        private static void UpgradeUserDB107(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN SyncToEdsm INTEGER NOT NULL DEFAULT 1";
            string query2 = "ALTER TABLE Commanders ADD COLUMN SyncFromEdsm INTEGER NOT NULL DEFAULT 0";
            string query3 = "ALTER TABLE Commanders ADD COLUMN SyncToEddn INTEGER NOT NULL DEFAULT 1";
            conn.PerformUpgrade(107, true, false, new[] { query1, query2, query3});
        }

        private static void UpgradeUserDB108(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN JournalDir TEXT";
            conn.PerformUpgrade(108, true, false, new[] { query1 }, () =>
            {
                try
                {
                    List<int> commandersToMigrate = new List<int>();
                    using (DbCommand cmd = conn.CreateCommand("SELECT Id, NetLogDir, JournalDir FROM Commanders"))
                    {
                        using (DbDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                int nr = Convert.ToInt32(rdr["Id"]);
                                object netlogdir = rdr["NetLogDir"];
                                object journaldir = rdr["JournalDir"];

                                if (netlogdir != DBNull.Value && journaldir == DBNull.Value)
                                {
                                    string logdir = Convert.ToString(netlogdir);

                                    if (logdir != null && System.IO.Directory.Exists(logdir) && System.IO.Directory.EnumerateFiles(logdir, "journal*.log").Any())
                                    {
                                        commandersToMigrate.Add(nr);
                                    }
                                }
                            }
                        }
                    }

                    using (DbCommand cmd2 = conn.CreateCommand("UPDATE Commanders SET JournalDir=NetLogDir WHERE Id=@Nr"))
                    {
                        cmd2.AddParameter("@Nr", System.Data.DbType.Int32);

                        foreach (int nr in commandersToMigrate)
                        {
                            cmd2.Parameters["@Nr"].Value = nr;
                            cmd2.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("UpgradeUser108 exception: " + ex.Message);
                }
            });
        }

        private static void UpgradeUserDB109(SQLiteConnectionUser conn)
        {
            string query1 = "CREATE TABLE MaterialsCommodities ( " +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "Category TEXT NOT NULL, " +
                "Name TEXT NOT NULL COLLATE NOCASE, " +
                "Type TEXT NOT NULL COLLATE NOCASE," +
                "UNIQUE(Category,Name)" +
                ") ";

            conn.PerformUpgrade(109, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB110(SQLiteConnectionUser conn)
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN EdsmName TEXT";
            string query2 = "ALTER TABLE MaterialsCommodities ADD COLUMN ShortName TEXT NOT NULL COLLATE NOCASE DEFAULT ''";
            conn.PerformUpgrade(110, true, false, new[] { query1, query2 });
        }

        private static void UpgradeUserDB111(SQLiteConnectionUser conn)
        {
            string query1 = "ALTER TABLE MaterialsCommodities ADD COLUMN Flags INT NOT NULL DEFAULT 0";     // flags
            string query2 = "ALTER TABLE MaterialsCommodities ADD COLUMN Colour INT NOT NULL DEFAULT 15728640";     // ARGB
            string query3 = "ALTER TABLE MaterialsCommodities ADD COLUMN FDName TEXT NOT NULL COLLATE NOCASE DEFAULT ''";
            conn.PerformUpgrade(111, true, false, new[] { query1, query2, query3 });
        }

        private static void UpgradeUserDB112(SQLiteConnectionUser conn)
        {
            string query1 = "DELETE FROM MaterialsCommodities";     // To fix materialcompatibility wuth wrong tables in 5.0.x
            conn.PerformUpgrade(112, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB113(SQLiteConnectionUser conn)
        {
            string query1 = "DELETE FROM MaterialsCommodities";     // To fix journal name -> in game name mappings for manufactured and encoded commodities
            conn.PerformUpgrade(113, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB114(SQLiteConnectionUser conn)
        {
            string query1 = "DELETE FROM MaterialsCommodities";     // To fix journal name -> in game name mappings for manufactured and encoded commodities  missmatch between  different 8.0 branches... 
            conn.PerformUpgrade(114, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB115(SQLiteConnectionUser conn)
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN SyncToEGO INT NOT NULL DEFAULT 0";
            string query2 = "ALTER TABLE Commanders ADD COLUMN EGOName TEXT";
            string query3 = "ALTER TABLE Commanders ADD COLUMN EGOAPIKey TEXT";
            conn.PerformUpgrade(115, true, false, new[] { query1, query2, query3 });
        }


        private static void UpgradeUserDB116(SQLiteConnectionUser conn)
        {
            string query1 = "ALTER TABLE Bookmarks ADD COLUMN PlanetMarks TEXT DEFAULT NULL";
            conn.PerformUpgrade(116, true, false, new[] { query1 });
        }


        private static void UpgradeUserDB117(SQLiteConnectionUser conn)
        {
            string query1 = "ALTER TABLE routes_expeditions ADD COLUMN Status INT DEFAULT 0";
            conn.PerformUpgrade(117, true, false, new[] { query1 });
        }


        private static void UpgradeUserDB118(SQLiteConnectionUser conn)
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN SyncToInara INT NOT NULL DEFAULT 0";
            string query3 = "ALTER TABLE Commanders ADD COLUMN InaraAPIKey TEXT";
            conn.PerformUpgrade(118, true, false, new[] { query1, query3 });
        }


        private static void UpgradeUserDB119(SQLiteConnectionUser conn)
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN InaraName TEXT";
            conn.PerformUpgrade(119, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB120(SQLiteConnectionUser conn)
        {
            string query1 = "CREATE TABLE CaptainsLog ( " +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "Commander INTEGER NOT NULL, " + 
                "Time DATETIME NOT NULL, " +
                "SystemName TEXT NOT NULL COLLATE NOCASE, " +
                "BodyName TEXT NOT NULL COLLATE NOCASE, " +
                "Note TEXT NOT NULL, " +
                "Tags TEXT DEFAULT NULL, " +
                "Parameters TEXT DEFAULT NULL" +
                ") ";

            conn.PerformUpgrade(120, true, false, new[] { query1 });
        }


        private static void DropOldUserTables(SQLiteConnectionUser conn)
        {
            string[] queries = new[]
            {
                "DROP TABLE IF EXISTS Systems",
                "DROP TABLE IF EXISTS SystemAliases",
                "DROP TABLE IF EXISTS Distances",
                "DROP TABLE IF EXISTS Stations",
                "DROP TABLE IF EXISTS station_commodities",
                "DROP TABLE IF EXISTS Journals",
                "DROP TABLE IF EXISTS VisitedSystems",
                "DROP TABLE IF EXISTS Objects"
            };

            foreach (string query in queries)
            {
                using (DbCommand cmd = conn.CreateCommand(query))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void CreateUserDBTableIndexes(SQLiteConnectionUser conn)
        {
            string[] queries = new[]
            {
                "CREATE INDEX IF NOT EXISTS TravelLogUnit_Name ON TravelLogUnit (Name)",
                "CREATE INDEX IF NOT EXISTS TravelLogUnit_Commander ON TravelLogUnit(CommanderId)",

                "CREATE INDEX IF NOT EXISTS JournalEntry_CommanderId ON JournalEntries (CommanderId)",
                "CREATE INDEX IF NOT EXISTS JournalEntry_TravelLogId ON JournalEntries (TravelLogId)",
                "CREATE INDEX IF NOT EXISTS JournalEntry_EventTypeId ON JournalEntries (EventTypeId)",
                "CREATE INDEX IF NOT EXISTS JournalEntry_EventType ON JournalEntries (EventType)",
                "CREATE INDEX IF NOT EXISTS JournalEntry_EventTime ON JournalEntries (EventTime)",

                "CREATE INDEX IF NOT EXISTS MaterialsCommodities_ClassName ON MaterialsCommodities (Name)",
                "CREATE INDEX IF NOT EXISTS MaterialsCommodities_FDName ON MaterialsCommodities (FDName)",
            };

            foreach (string query in queries)
            {
                using (DbCommand cmd = conn.CreateCommand(query))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    // very old class used everywhere to get register stuff from user DB. its easier for now to keep this so we don't change 1000's of files.

    public static class SQLiteDBClass 
    {
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
    }
}
