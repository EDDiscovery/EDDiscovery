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
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using SQLLiteExtensions;

namespace EliteDangerousCore.DB
{
    public class SQLiteConnectionSystem : SQLExtConnectionWithLockRegister<SQLiteConnectionSystem>
    {
        public SQLiteConnectionSystem() : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath, false, Initialize, AccessMode.ReaderWriter)
        {   
        }

        public SQLiteConnectionSystem(AccessMode mode = AccessMode.ReaderWriter) : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath, false, Initialize, mode)
        {
        }

        private SQLiteConnectionSystem(bool utc, Action init) : base(EliteDangerousCore.EliteConfigInstance.InstanceOptions.SystemDatabasePath, utc, init, AccessMode.ReaderWriter)
        {       
        }

        public static void Initialize()
        {
            InitializeIfNeeded(() =>
            {
                using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem(false, null))       // use this special one so we don't get double init.
                {
                    System.Diagnostics.Debug.WriteLine("Initialise System DB");
                    UpgradeSystemsDB(conn);
                }
            });
        }

        protected static bool UpgradeSystemsDB(SQLiteConnectionSystem conn)
        {
            int dbver;
            try
            {
                conn.ExecuteNonQuery( "CREATE TABLE IF NOT EXISTS Register (ID TEXT PRIMARY KEY NOT NULL, ValueInt INTEGER, ValueDouble DOUBLE, ValueString TEXT, ValueBlob BLOB)");

                SQLExtRegister reg = new SQLExtRegister(conn);
                dbver = reg.GetSettingInt("DBVer", 1);        // use the constring one, as don't want to go back into ConnectionString code

                DropOldSystemTables(conn);

                if (dbver < 2)
                    UpgradeSystemsDB2(conn);

                if (dbver < 6)
                    UpgradeSystemsDB6(conn);

                if (dbver < 11)
                    UpgradeSystemsDB11(conn);

                if (dbver < 15)
                    UpgradeSystemsDB15(conn);

                if (dbver < 17)
                    UpgradeSystemsDB17(conn);

                if (dbver < 19)
                    UpgradeSystemsDB19(conn);

                if (dbver < 20)
                    UpgradeSystemsDB20(conn);

                if (dbver < 100)
                    UpgradeSystemsDB101(conn);

                if (dbver < 102)
                    UpgradeSystemsDB102(conn);

                CreateSystemDBTableIndexes(conn);

                return true;
            }
            catch (Exception ex)
            {
                ExtendedControls.MessageBoxTheme.Show("UpgradeSystemsDB error: " + ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

        private static void UpgradeSystemsDB2(SQLExtConnection conn)
        {
            string query = "CREATE TABLE Systems (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , name TEXT NOT NULL COLLATE NOCASE , x FLOAT, y FLOAT, z FLOAT, cr INTEGER, commandercreate TEXT, createdate DATETIME, commanderupdate TEXT, updatedate DATETIME, status INTEGER, population INTEGER )";
            string query3 = "CREATE TABLE Distances (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL  UNIQUE , NameA TEXT NOT NULL , NameB TEXT NOT NULL , Dist FLOAT NOT NULL , CommanderCreate TEXT NOT NULL , CreateTime DATETIME NOT NULL , Status INTEGER NOT NULL )";
            string query5 = "CREATE INDEX DistanceName ON Distances (NameA ASC, NameB ASC)";

            conn.PerformUpgrade( 2, false, false, new[] { query, query3, query5 });
        }

        private static void UpgradeSystemsDB6(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN id_eddb Integer";
            string query2 = "ALTER TABLE Systems ADD COLUMN faction TEXT";
            //string query3 = "ALTER TABLE Systems ADD COLUMN population Integer";
            string query4 = "ALTER TABLE Systems ADD COLUMN government_id Integer";
            string query5 = "ALTER TABLE Systems ADD COLUMN allegiance_id Integer";
            string query6 = "ALTER TABLE Systems ADD COLUMN primary_economy_id Integer";
            string query7 = "ALTER TABLE Systems ADD COLUMN security Integer";
            string query8 = "ALTER TABLE Systems ADD COLUMN eddb_updated_at Integer";
            string query9 = "ALTER TABLE Systems ADD COLUMN state Integer";
            string query10 = "ALTER TABLE Systems ADD COLUMN needs_permit Integer";
            string query11 = "DROP TABLE IF EXISTS Stations";
            string query12 = "CREATE TABLE Stations (id INTEGER PRIMARY KEY  NOT NULL ,system_id INTEGER, name TEXT NOT NULL ,  " +
                " max_landing_pad_size INTEGER, distance_to_star INTEGER, faction Text, government_id INTEGER, allegiance_id Integer,  state_id INTEGER, type_id Integer, " +
                "has_commodities BOOL DEFAULT (null), has_refuel BOOL DEFAULT (null), has_repair BOOL DEFAULT (null), has_rearm BOOL DEFAULT (null), " +
                "has_outfitting BOOL DEFAULT (null),  has_shipyard BOOL DEFAULT (null), has_blackmarket BOOL DEFAULT (null),   eddb_updated_at Integer  )";

            string query13 = "CREATE TABLE station_commodities (station_id INTEGER PRIMARY KEY NOT NULL, commodity_id INTEGER, type INTEGER)";
            string query14 = "CREATE INDEX station_commodities_index ON station_commodities (station_id ASC, commodity_id ASC, type ASC)";
            string query15 = "CREATE INDEX StationsIndex_ID  ON Stations (id ASC)";
            string query16 = "CREATE INDEX StationsIndex_system_ID  ON Stations (system_id ASC)";
            string query17 = "CREATE INDEX StationsIndex_system_Name  ON Stations (Name ASC)";

            conn.PerformUpgrade( 6, true, false, new[] {
                query1, query2, query4, query5, query6, query7, query8, query9, query10,
                query11, query12, query13, query14, query15, query16, query17 });
        }

        private static void UpgradeSystemsDB11(SQLExtConnection conn)
        {
            //Default is Color.Red.ToARGB()
            string query1 = "ALTER TABLE Systems ADD COLUMN FirstDiscovery BOOL";
            conn.PerformUpgrade( 11, true, false, new[] { query1 });
        }

        private static void UpgradeSystemsDB15(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN versiondate DATETIME";
            string query2 = "UPDATE Systems SET versiondate = datetime('now')";

            conn.PerformUpgrade( 15, true, false, new[] { query1, query2 });
        }

        private static void UpgradeSystemsDB17(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN id_edsm Integer";
            string query4 = "ALTER TABLE Distances ADD COLUMN id_edsm Integer";
            string query5 = "CREATE INDEX Distances_EDSM_ID_Index ON Distances (id_edsm ASC)";

            conn.PerformUpgrade( 17, true, false, new[] { query1, query4, query5 });
        }

        private static void UpgradeSystemsDB19(SQLExtConnection conn)
        {
            string query1 = "CREATE TABLE SystemAliases (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT, id_edsm INTEGER, id_edsm_mergedto INTEGER)";
            string query2 = "CREATE INDEX SystemAliases_name ON SystemAliases (name)";
            string query3 = "CREATE UNIQUE INDEX SystemAliases_id_edsm ON SystemAliases (id_edsm)";
            string query4 = "CREATE INDEX SystemAliases_id_edsm_mergedto ON SystemAliases (id_edsm_mergedto)";

            conn.PerformUpgrade( 19, true, false, new[] { query1, query2, query3, query4 });
        }

        private static void UpgradeSystemsDB20(SQLExtConnection conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN gridid Integer NOT NULL DEFAULT -1";
            string query2 = "ALTER TABLE Systems ADD COLUMN randomid Integer NOT NULL DEFAULT -1";

            conn.PerformUpgrade( 20, true, false, new[] { query1, query2 }, () =>
            {
                SQLExtRegister reg = new SQLExtRegister(conn);
                reg.PutSettingString("EDSMLastSystems", "2010 - 01 - 01 00:00:00"); // force EDSM sync..  MUST do this manually, can't use main function as it needs internal one
            }); 
        }

        private static void UpgradeSystemsDB101(SQLExtConnection conn)
        {
            string query1 = "DROP TABLE IF EXISTS Bookmarks";
            string query2 = "DROP TABLE IF EXISTS SystemNote";
            string query3 = "DROP TABLE IF EXISTS TravelLogUnit";
            string query4 = "DROP TABLE IF EXISTS VisitedSystems";
            string query5 = "DROP TABLE IF EXISTS Route_Systems";
            string query6 = "DROP TABLE IF EXISTS Routes_expedition";
            string query7 = "VACUUM";


            conn.PerformUpgrade( 101, true, false, new[] { query1, query2, query3, query4, query5, query6, query7 }, () =>
            {
            });
        }

        private static void UpgradeSystemsDB102(SQLExtConnection conn)
        {
            string query1 = "CREATE TABLE SystemNames (" +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "Name TEXT NOT NULL COLLATE NOCASE, " +
                "EdsmId INTEGER NOT NULL)";
            string query2 = "CREATE TABLE EdsmSystems (" +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "EdsmId INTEGER NOT NULL, " +
                "EddbId INTEGER, " +
                "X INTEGER NOT NULL, " +
                "Y INTEGER NOT NULL, " +
                "Z INTEGER NOT NULL, " +
                "CreateTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                "UpdateTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                "VersionTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                "GridId INTEGER NOT NULL DEFAULT -1, " +
                "RandomId INTEGER NOT NULL DEFAULT -1)";
            string query3 = "CREATE TABLE EddbSystems (" +
                "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "Name TEXT NOT NULL, " +
                "EdsmId INTEGER NOT NULL, " +
                "EddbId INTEGER NOT NULL, " +
                "Population INTEGER , " +
                "Faction TEXT, " +
                "GovernmentId Integer, " +
                "AllegianceId Integer, " +
                "PrimaryEconomyId Integer, " +
                "Security Integer, " +
                "EddbUpdatedAt Integer, " + // Seconds since 1970-01-01 00:00:00 UTC
                "State Integer, " +
                "NeedsPermit Integer)";
            conn.PerformUpgrade( 102, true, false, new[] { query1, query2, query3 });
        }


        public static void DropOldSystemTables(SQLiteConnectionSystem conn)         // UPGRADE
        {
            string[] queries = new[]
            {
                "DROP TABLE IF EXISTS Bookmarks",
                "DROP TABLE IF EXISTS SystemNote",
                "DROP TABLE IF EXISTS TravelLogUnit",
                "DROP TABLE IF EXISTS VisitedSystems",
                "DROP TABLE IF EXISTS route_Systems",
                "DROP TABLE IF EXISTS routes_expeditions",
                "DROP TABLE IF EXISTS Objects",
                "DROP TABLE IF EXISTS wanted_systems",
            };

            foreach (string query in queries)
            {
                using (DbCommand cmd = conn.CreateCommand(query))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void CreateSystemDBTableIndexes(SQLiteConnectionSystem conn)     // UPGRADE
        {
            string[] queries = new[]
            {
                "CREATE UNIQUE INDEX IF NOT EXISTS SystemAliases_id_edsm ON SystemAliases (id_edsm)",
                "CREATE INDEX IF NOT EXISTS SystemAliases_name ON SystemAliases (name)",
                "CREATE INDEX IF NOT EXISTS SystemAliases_id_edsm_mergedto ON SystemAliases (id_edsm_mergedto)",
                "CREATE INDEX IF NOT EXISTS Distances_EDSM_ID_Index ON Distances (id_edsm ASC)",
                "CREATE INDEX IF NOT EXISTS DistanceName ON Distances (NameA ASC, NameB ASC)",
                "CREATE INDEX IF NOT EXISTS stationIndex ON Stations (system_id ASC)",
                "CREATE INDEX IF NOT EXISTS station_commodities_index ON station_commodities (station_id ASC, commodity_id ASC, type ASC)",
                "CREATE INDEX IF NOT EXISTS StationsIndex_ID  ON Stations (id ASC)",
                "CREATE INDEX IF NOT EXISTS StationsIndex_system_ID  ON Stations (system_id ASC)",
                "CREATE INDEX IF NOT EXISTS StationsIndex_system_Name  ON Stations (Name ASC)",
            };

            foreach (string query in queries)
            {
                using (DbCommand cmd = conn.CreateCommand(query))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DropSystemsTableIndexes()        // PERFORM during full table replacement
        {
            string[] queries = new[]
            {
                "DROP INDEX IF EXISTS SystemsIndex",
                "DROP INDEX IF EXISTS Systems_EDSM_ID_Index",
                "DROP INDEX IF EXISTS Systems_EDDB_ID_Index",
                "DROP INDEX IF EXISTS IDX_Systems_versiondate",
                "DROP INDEX IF EXISTS Systems_position",
                "DROP INDEX IF EXISTS SystemGridId",
                "DROP INDEX IF EXISTS SystemRandomId",
                "DROP INDEX IF EXISTS EdsmSystems_EdsmId",
                "DROP INDEX IF EXISTS EdsmSystems_EddbId",
                "DROP INDEX IF EXISTS EddbSystems_EdsmId",
                "DROP INDEX IF EXISTS EddbSystems_EddbId",
                "DROP INDEX IF EXISTS EdsmSystems_Position",
                "DROP INDEX IF EXISTS EdsmSystems_GridId",
                "DROP INDEX IF EXISTS EdsmSystems_RandomId",
                "DROP INDEX IF EXISTS SystemNames_EdsmId",
                "DROP INDEX IF EXISTS SystemNames_IdName",
                "DROP INDEX IF EXISTS SystemNames_NameId",
                "DROP INDEX IF EXISTS sqlite_autoindex_SystemNames_1"
            };
            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                foreach (string query in queries)
                {
                    using (DbCommand cmd = conn.CreateCommand(query))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void CreateSystemsTableIndexes()
        {
            using (var schemalock = new SchemaLock())
            {
                CreateSystemsTableIndexesNoLock();
            }
        }

        private static void CreateSystemsTableIndexesNoLock()           // PERFORM AFTER SYSTEM TABLE REPLACEMENT
        {
            string[] queries = new[]
            {
                "CREATE INDEX IF NOT EXISTS EdsmSystems_EdsmId ON EdsmSystems (EdsmId ASC, EddbId, X, Y, Z)",
                "CREATE INDEX IF NOT EXISTS EdsmSystems_EddbId ON EdsmSystems (EddbId ASC, EdsmId)",
                "CREATE INDEX IF NOT EXISTS EddbSystems_EdsmId ON EddbSystems (EdsmId ASC, EddbId)",
                "CREATE INDEX IF NOT EXISTS EddbSystems_EddbId ON EddbSystems (EddbId ASC, EdsmId)",
                "CREATE INDEX IF NOT EXISTS EdsmSystems_PosId ON EdsmSystems (Z, X, Y, EdsmId)",
                "CREATE INDEX IF NOT EXISTS EdsmSystems_StarGrid ON EdsmSystems (GridId, RandomId, EdsmId, EddbId, X, Y, Z)",
                "CREATE INDEX IF NOT EXISTS SystemNames_IdName ON SystemNames (EdsmId,Name)",
                "CREATE INDEX IF NOT EXISTS SystemNames_NameId ON SystemNames (Name,EdsmId)",
            };
            using (SQLiteConnectionSystem conn = new SQLiteConnectionSystem())
            {
                foreach (string query in queries)
                {
                    using (DbCommand cmd = conn.CreateCommand(query))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void CreateTempSystemsTable()         // PERFORM AT START SYSTEM TABLE REPLACEMENT
        {
            using (var conn = new SQLiteConnectionSystem())
            {
                conn.ExecuteNonQuery( "DROP TABLE IF EXISTS EdsmSystems_temp");
                conn.ExecuteNonQuery( "DROP TABLE IF EXISTS SystemNames_temp");
                conn.ExecuteNonQuery(
                    "CREATE TABLE SystemNames_temp (" +
                        "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "Name TEXT NOT NULL COLLATE NOCASE, " +
                        "EdsmId INTEGER NOT NULL )");

                conn.ExecuteNonQuery(
                    "CREATE TABLE EdsmSystems_temp (" +
                        "Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        "EdsmId INTEGER NOT NULL, " +
                        "EddbId INTEGER, " +
                        "X INTEGER NOT NULL, " +
                        "Y INTEGER NOT NULL, " +
                        "Z INTEGER NOT NULL, " +
                        "CreateTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                        "UpdateTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                        "VersionTimestamp INTEGER NOT NULL, " + // Seconds since 2015-01-01 00:00:00 UTC
                        "GridId INTEGER NOT NULL DEFAULT -1, " +
                        "RandomId INTEGER NOT NULL DEFAULT -1)");
            }
        }

        public static void ReplaceSystemsTable()                // PERFORM AFTER TEMP SYSTEM TABLE UPDATED
        {
            using (var slock = new SchemaLock())
            {
                using (var conn = new SQLiteConnectionSystem())
                {
                    DropSystemsTableIndexes();
                    using (var txn = conn.BeginTransaction())
                    {
                        conn.ExecuteNonQuery( "DROP TABLE IF EXISTS Systems");
                        conn.ExecuteNonQuery( "DROP TABLE IF EXISTS EdsmSystems");
                        conn.ExecuteNonQuery( "DROP TABLE IF EXISTS SystemNames");
                        conn.ExecuteNonQuery( "ALTER TABLE EdsmSystems_temp RENAME TO EdsmSystems");
                        conn.ExecuteNonQuery( "ALTER TABLE SystemNames_temp RENAME TO SystemNames");
                        txn.Commit();
                    }
                    conn.ExecuteNonQuery( "VACUUM");
                    CreateSystemsTableIndexesNoLock();
                }
            }
        }
    }
}
