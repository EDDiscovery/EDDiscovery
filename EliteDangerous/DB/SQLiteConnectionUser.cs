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
    internal class SQLiteConnectionUser : SQLExtConnectionRegister<SQLiteConnectionUser>
    {
        public SQLiteConnectionUser() : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.UserDatabasePath, utctimeindicator:true)
        {
        }

        public bool UpgradeUserDB()
        {
            int dbver;
            try
            {
                ExecuteNonQuery("CREATE TABLE IF NOT EXISTS Register (ID TEXT PRIMARY KEY NOT NULL, ValueInt INTEGER, ValueDouble DOUBLE, ValueString TEXT, ValueBlob BLOB)");

                dbver = GetSettingInt("DBVer", 1);        // use the constring one, as don't want to go back into ConnectionString code

                DropOldUserTables();

                if (dbver < 2)
                    UpgradeUserDB2();

                if (dbver < 4)
                    UpgradeUserDB4();

                if (dbver < 7)
                    UpgradeUserDB7();

                if (dbver < 9)
                    UpgradeUserDB9();

                if (dbver < 10)
                    UpgradeUserDB10();

                if (dbver < 11)
                    UpgradeUserDB11();

                if (dbver < 12)
                    UpgradeUserDB12();

                if (dbver < 16)
                    UpgradeUserDB16();

                if (dbver < 101)
                    UpgradeUserDB101();

                if (dbver < 102)
                    UpgradeUserDB102();

                if (dbver < 103)
                    UpgradeUserDB103();

                if (dbver < 104)
                    UpgradeUserDB104();

                if (dbver < 105)
                    UpgradeUserDB105();

                if (dbver < 106)
                    UpgradeUserDB106();

                if (dbver < 107)
                    UpgradeUserDB107();

                if (dbver < 108)
                    UpgradeUserDB108();

                if (dbver < 109)
                    UpgradeUserDB109();

                if (dbver < 110)
                    UpgradeUserDB110();

                if (dbver < 111)
                    UpgradeUserDB111();

                if (dbver < 112)
                    UpgradeUserDB112();

                if (dbver < 113)
                    UpgradeUserDB113();

                if (dbver < 114)
                    UpgradeUserDB114();

                if (dbver < 115)
                    UpgradeUserDB115();

                if (dbver < 116)
                    UpgradeUserDB116();

                if (dbver < 117)
                    UpgradeUserDB117();

                if (dbver < 118)
                    UpgradeUserDB118();

                if (dbver < 119)
                    UpgradeUserDB119();

                if (dbver < 120)
                    UpgradeUserDB120();

                if (dbver < 121)
                    UpgradeUserDB121();

                if (dbver < 122)
                    UpgradeUserDB122();

                if (dbver < 123)
                    UpgradeUserDB123();

                CreateUserDBTableIndexes();

                return true;
            }
            catch (Exception ex)
            {
                ExtendedControls.MessageBoxTheme.Show("UpgradeUserDB error: " + ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

        private void UpgradeUserDB2()
        {
            string query4 = "CREATE TABLE SystemNote (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Time DATETIME NOT NULL )";

            PerformUpgrade(2, false, false, new[] { query4 });
        }

        private void UpgradeUserDB4()
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN Note TEXT";
            PerformUpgrade(4, true, false, new[] { query1 });
        }

        private void UpgradeUserDB7()
        {
            string query3 = "CREATE TABLE TravelLogUnit(id INTEGER PRIMARY KEY  NOT NULL, type INTEGER NOT NULL, name TEXT NOT NULL, size INTEGER, path TEXT)";
            PerformUpgrade(7, true, false, new[] { query3 });
        }

        private void UpgradeUserDB9()
        {
            string query1 = "CREATE TABLE Objects (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , SystemName TEXT NOT NULL , ObjectName TEXT NOT NULL , ObjectType INTEGER NOT NULL , ArrivalPoint Float, Gravity FLOAT, Atmosphere Integer, Vulcanism Integer, Terrain INTEGER, Carbon BOOL, Iron BOOL, Nickel BOOL, Phosphorus BOOL, Sulphur BOOL, Arsenic BOOL, Chromium BOOL, Germanium BOOL, Manganese BOOL, Selenium BOOL NOT NULL , Vanadium BOOL, Zinc BOOL, Zirconium BOOL, Cadmium BOOL, Mercury BOOL, Molybdenum BOOL, Niobium BOOL, Tin BOOL, Tungsten BOOL, Antimony BOOL, Polonium BOOL, Ruthenium BOOL, Technetium BOOL, Tellurium BOOL, Yttrium BOOL, Commander  Text, UpdateTime DATETIME, Status INTEGER )";
            PerformUpgrade(9, true, false, new[] { query1 });
        }

        private void UpgradeUserDB10()
        {
            string query1 = "CREATE TABLE wanted_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, systemname TEXT UNIQUE NOT NULL)";
            PerformUpgrade(10, true, false, new[] { query1 });
        }


        private void UpgradeUserDB11()
        {
            string query2 = "ALTER TABLE Objects ADD COLUMN Landed BOOL";
            string query3 = "ALTER TABLE Objects ADD COLUMN terraform Integer";
            PerformUpgrade(11, true, false, new[] { query2, query3 });
        }

        private void UpgradeUserDB12()
        {
            string query1 = "CREATE TABLE routes_expeditions (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT UNIQUE NOT NULL, start DATETIME, end DATETIME)";
            string query2 = "CREATE TABLE route_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, routeid INTEGER NOT NULL, systemname TEXT NOT NULL)";
            PerformUpgrade(12, true, false, new[] { query1, query2 });
        }


        private void UpgradeUserDB16()
        {
            string query = "CREATE TABLE Bookmarks (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , StarName TEXT, x double NOT NULL, y double NOT NULL, z double NOT NULL, Time DATETIME NOT NULL, Heading TEXT, Note TEXT NOT Null )";
            PerformUpgrade(16, true, false, new[] { query });
        }

        private void UpgradeUserDB101()
        {
            string query1 = "DROP TABLE IF EXISTS Systems";
            string query2 = "DROP TABLE IF EXISTS SystemAliases";
            string query3 = "DROP TABLE IF EXISTS Distances";
            string query4 = "VACUUM";

            PerformUpgrade(101, true, false, new[] { query1, query2, query3, query4 });
        }

        private void UpgradeUserDB102()
        {
            string query1 = "CREATE TABLE Commanders (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, EdsmApiKey TEXT NOT NULL, NetLogDir TEXT, Deleted INTEGER NOT NULL)";

            PerformUpgrade(102, true, false, new[] { query1 });
        }

        private void UpgradeUserDB103()
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

            PerformUpgrade(103, true, false, new[] { query1, query2 });
        }

        private void UpgradeUserDB104()
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN journalid Integer NOT NULL DEFAULT 0";
            PerformUpgrade(104, true, false, new[] { query1 });
        }

        private void UpgradeUserDB105()
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


            PerformUpgrade(105, true, false, new[] { query1, query2, query3 });
        }

        private void UpgradeUserDB106()
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN EdsmId INTEGER NOT NULL DEFAULT -1";
            PerformUpgrade(106, true, false, new[] { query1 });
        }


        private void UpgradeUserDB107()
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN SyncToEdsm INTEGER NOT NULL DEFAULT 1";
            string query2 = "ALTER TABLE Commanders ADD COLUMN SyncFromEdsm INTEGER NOT NULL DEFAULT 0";
            string query3 = "ALTER TABLE Commanders ADD COLUMN SyncToEddn INTEGER NOT NULL DEFAULT 1";
            PerformUpgrade(107, true, false, new[] { query1, query2, query3 });
        }

        private void UpgradeUserDB108()
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN JournalDir TEXT";
            PerformUpgrade(108, true, false, new[] { query1 }, () =>
            {
                try
                {
                    List<int> commandersToMigrate = new List<int>();
                    using (DbCommand cmd = CreateCommand("SELECT Id, NetLogDir, JournalDir FROM Commanders"))
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

                    using (DbCommand cmd2 = CreateCommand("UPDATE Commanders SET JournalDir=NetLogDir WHERE Id=@Nr"))
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

        private void UpgradeUserDB109()
        {
            string query1 = "CREATE TABLE MaterialsCommodities ( " +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "Category TEXT NOT NULL, " +
                "Name TEXT NOT NULL COLLATE NOCASE, " +
                "Type TEXT NOT NULL COLLATE NOCASE," +
                "UNIQUE(Category,Name)" +
                ") ";

            PerformUpgrade(109, true, false, new[] { query1 });
        }

        private void UpgradeUserDB110()
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN EdsmName TEXT";
            string query2 = "ALTER TABLE MaterialsCommodities ADD COLUMN ShortName TEXT NOT NULL COLLATE NOCASE DEFAULT ''";
            PerformUpgrade(110, true, false, new[] { query1, query2 });
        }

        private void UpgradeUserDB111()
        {
            string query1 = "ALTER TABLE MaterialsCommodities ADD COLUMN Flags INT NOT NULL DEFAULT 0";     // flags
            string query2 = "ALTER TABLE MaterialsCommodities ADD COLUMN Colour INT NOT NULL DEFAULT 15728640";     // ARGB
            string query3 = "ALTER TABLE MaterialsCommodities ADD COLUMN FDName TEXT NOT NULL COLLATE NOCASE DEFAULT ''";
            PerformUpgrade(111, true, false, new[] { query1, query2, query3 });
        }

        private void UpgradeUserDB112()
        {
            string query1 = "DELETE FROM MaterialsCommodities";     // To fix materialcompatibility wuth wrong tables in 5.0.x
            PerformUpgrade(112, true, false, new[] { query1 });
        }

        private void UpgradeUserDB113()
        {
            string query1 = "DELETE FROM MaterialsCommodities";     // To fix journal name -> in game name mappings for manufactured and encoded commodities
            PerformUpgrade(113, true, false, new[] { query1 });
        }

        private void UpgradeUserDB114()
        {
            string query1 = "DELETE FROM MaterialsCommodities";     // To fix journal name -> in game name mappings for manufactured and encoded commodities  missmatch between  different 8.0 branches...
            PerformUpgrade(114, true, false, new[] { query1 });
        }

        private void UpgradeUserDB115()
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN SyncToEGO INT NOT NULL DEFAULT 0";
            string query2 = "ALTER TABLE Commanders ADD COLUMN EGOName TEXT";
            string query3 = "ALTER TABLE Commanders ADD COLUMN EGOAPIKey TEXT";
            PerformUpgrade(115, true, false, new[] { query1, query2, query3 });
        }


        private void UpgradeUserDB116()
        {
            string query1 = "ALTER TABLE Bookmarks ADD COLUMN PlanetMarks TEXT DEFAULT NULL";
            PerformUpgrade(116, true, false, new[] { query1 });
        }


        private void UpgradeUserDB117()
        {
            string query1 = "ALTER TABLE routes_expeditions ADD COLUMN Status INT DEFAULT 0";
            PerformUpgrade(117, true, false, new[] { query1 });
        }


        private void UpgradeUserDB118()
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN SyncToInara INT NOT NULL DEFAULT 0";
            string query3 = "ALTER TABLE Commanders ADD COLUMN InaraAPIKey TEXT";
            PerformUpgrade(118, true, false, new[] { query1, query3 });
        }


        private void UpgradeUserDB119()
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN InaraName TEXT";
            PerformUpgrade(119, true, false, new[] { query1 });
        }

        private void UpgradeUserDB120()
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

            PerformUpgrade(120, true, false, new[] { query1 });
        }

        private void UpgradeUserDB121()
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN HomeSystem TEXT";
            PerformUpgrade(121, true, false, new[] { query1 });
        }

        private void UpgradeUserDB122()
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN MapColour INT";
            string query2 = "ALTER TABLE Commanders ADD COLUMN MapCentreOnSelection INT";
            string query3 = "ALTER TABLE Commanders ADD COLUMN MapZoom REAL";
            PerformUpgrade(122, true, false, new[] { query1, query2, query3 });
        }

        private void UpgradeUserDB123()
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN SyncToIGAU INTEGER NOT NULL DEFAULT 0";
            PerformUpgrade(123, true, false, new[] { query1 });
        }

        private void DropOldUserTables()
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
                using (DbCommand cmd = CreateCommand(query))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void CreateUserDBTableIndexes()
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
                using (DbCommand cmd = CreateCommand(query))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
