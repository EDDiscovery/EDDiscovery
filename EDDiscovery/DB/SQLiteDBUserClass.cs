using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery.DB
{
    static public class SQLiteDBUserClass   //: SQLiteDBClass
    {
        public static bool UpgradeUserDB(SQLiteConnectionUser conn)
        {
            int dbver;
            try
            {
                SQLiteDBClass.ExecuteQuery(conn, "CREATE TABLE IF NOT EXISTS Register (ID TEXT PRIMARY KEY NOT NULL, ValueInt INTEGER, ValueDouble DOUBLE, ValueString TEXT, ValueBlob BLOB)");
                dbver = SQLiteDBClass.GetSettingInt("DBVer", 1, conn);        // use the constring one, as don't want to go back into ConnectionString code

                DropOldUserTables(conn);

                if (dbver < 2)
                    UpgradeUserDB2(conn);

                if (dbver < 4)
                    UpgradeUserDB4(conn);

                if (dbver < 7)
                    UpgradeUserDB7(conn);

                if (dbver < 8)
                    UpgradeUserDB8(conn);

                if (dbver < 9)
                    UpgradeUserDB9(conn);

                if (dbver < 10)
                    UpgradeUserDB10(conn);

                if (dbver < 11)
                    UpgradeUserDB11(conn);

                if (dbver < 12)
                    UpgradeUserDB12(conn);

                if (dbver < 14)
                    UpgradeUserDB14(conn);

                if (dbver < 16)
                    UpgradeUserDB16(conn);

                if (dbver < 17)
                    UpgradeUserDB17(conn);

                if (dbver < 18)
                    UpgradeUserDB18(conn);

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


                CreateUserDBTableIndexes();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UpgradeUserDB error: " + ex.Message);
                MessageBox.Show(ex.StackTrace);
                return false;
            }
        }

        private static void UpgradeUserDB2(SQLiteConnectionED conn)
        {
            string query4 = "CREATE TABLE SystemNote (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Time DATETIME NOT NULL )";

            SQLiteDBClass.PerformUpgrade(conn, 2, false, false, new[] { query4 });
        }

        /*
        private static void UpgradeDB3(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN Note TEXT";
            SQLiteDBClass.PerformUpgrade(conn, 3, false, false, new[] { query1 });
        }
         */

        private static void UpgradeUserDB4(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN Note TEXT";
            SQLiteDBClass.PerformUpgrade(conn, 4, true, true, new[] { query1 });
        }

        private static void UpgradeUserDB7(SQLiteConnectionED conn)
        {
            string query1 = "DROP TABLE IF EXISTS VisitedSystems";
            string query2 = "CREATE TABLE VisitedSystems(id INTEGER PRIMARY KEY  NOT NULL, Name TEXT NOT NULL, Time DATETIME NOT NULL, Unit Text, Commander Integer, Source Integer, edsm_sync BOOL DEFAULT (null))";
            string query3 = "CREATE TABLE TravelLogUnit(id INTEGER PRIMARY KEY  NOT NULL, type INTEGER NOT NULL, name TEXT NOT NULL, size INTEGER, path TEXT)";
            SQLiteDBClass.PerformUpgrade(conn, 7, true, true, new[] { query1, query2, query3 });
        }

        private static void UpgradeUserDB8(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN Map_colour INTEGER DEFAULT (-65536)";
            SQLiteDBClass.PerformUpgrade(conn, 8, true, true, new[] { query1 });
        }

        private static void UpgradeUserDB9(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE Objects (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , SystemName TEXT NOT NULL , ObjectName TEXT NOT NULL , ObjectType INTEGER NOT NULL , ArrivalPoint Float, Gravity FLOAT, Atmosphere Integer, Vulcanism Integer, Terrain INTEGER, Carbon BOOL, Iron BOOL, Nickel BOOL, Phosphorus BOOL, Sulphur BOOL, Arsenic BOOL, Chromium BOOL, Germanium BOOL, Manganese BOOL, Selenium BOOL NOT NULL , Vanadium BOOL, Zinc BOOL, Zirconium BOOL, Cadmium BOOL, Mercury BOOL, Molybdenum BOOL, Niobium BOOL, Tin BOOL, Tungsten BOOL, Antimony BOOL, Polonium BOOL, Ruthenium BOOL, Technetium BOOL, Tellurium BOOL, Yttrium BOOL, Commander  Text, UpdateTime DATETIME, Status INTEGER )";
            SQLiteDBClass.PerformUpgrade(conn, 9, true, true, new[] { query1 });
        }

        private static void UpgradeUserDB10(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE wanted_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, systemname TEXT UNIQUE NOT NULL)";
            SQLiteDBClass.PerformUpgrade(conn, 10, true, true, new[] { query1 });
        }


        private static void UpgradeUserDB11(SQLiteConnectionED conn)
        {
            string query2 = "ALTER TABLE Objects ADD COLUMN Landed BOOL";
            string query3 = "ALTER TABLE Objects ADD COLUMN terraform Integer";
            string query4 = "ALTER TABLE VisitedSystems ADD COLUMN Status BOOL";
            SQLiteDBClass.PerformUpgrade(conn, 11, true, true, new[] { query2, query3, query4 });
        }

        private static void UpgradeUserDB12(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE routes_expeditions (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT UNIQUE NOT NULL, start DATETIME, end DATETIME)";
            string query2 = "CREATE TABLE route_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, routeid INTEGER NOT NULL, systemname TEXT NOT NULL)";
            SQLiteDBClass.PerformUpgrade(conn, 12, true, true, new[] { query1, query2 });
        }


        private static bool UpgradeUserDB14(SQLiteConnectionED conn)
        {
            //Default is Color.Red.ToARGB()
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN X double";
            string query2 = "ALTER TABLE VisitedSystems ADD COLUMN Y double";
            string query3 = "ALTER TABLE VisitedSystems ADD COLUMN Z double";

            SQLiteDBClass.PerformUpgrade(conn, 14, true, true, new[] { query1, query2, query3 });
            return true;
        }

        private static void UpgradeUserDB16(SQLiteConnectionED conn)
        {
            string query = "CREATE TABLE Bookmarks (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , StarName TEXT, x double NOT NULL, y double NOT NULL, z double NOT NULL, Time DATETIME NOT NULL, Heading TEXT, Note TEXT NOT Null )";
            SQLiteDBClass.PerformUpgrade(conn, 16, true, true, new[] { query });
        }


        private static void UpgradeUserDB17(SQLiteConnectionED conn)
        {
            string query6 = "Update VisitedSystems set x=null, y=null, z=null where x=0 and y=0 and z=0 and name!=\"Sol\"";
            SQLiteDBClass.PerformUpgrade(conn, 17, true, true, new[] { query6 }, () =>
            {
                SQLiteDBClass.PutSettingString("EDSMLastSystems", "2010 - 01 - 01 00:00:00", conn);        // force EDSM sync..
                SQLiteDBClass.PutSettingString("EDDBSystemsTime", "0", conn);                               // force EDDB
                SQLiteDBClass.PutSettingString("EDSCLastDist", "2010-01-01 00:00:00", conn);                // force distances
            });
        }

        private static void UpgradeUserDB18(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN id_edsm_assigned Integer";
            string query2 = "CREATE INDEX VisitedSystems_id_edsm_assigned ON VisitedSystems (id_edsm_assigned)";
            string query3 = "CREATE INDEX VisitedSystems_position ON VisitedSystems (X, Y, Z)";

            SQLiteDBClass.PerformUpgrade(conn, 18, true, true, new[] { query1, query2, query3 });
        }


        private static void UpgradeUserDB101(SQLiteConnectionED conn)
        {
            string query1 = "DROP TABLE IF EXISTS Systems";
            string query2 = "DROP TABLE IF EXISTS SystemAliases";
            string query3 = "DROP TABLE IF EXISTS Distances";
            string query4 = "VACUUM";

            SQLiteDBClass.PerformUpgrade(conn, 101, true, false, new[] { query1, query2, query3, query4 }, () =>
            {
                //                PutSettingString("EDSMLastSystems", "2010 - 01 - 01 00:00:00", conn);        // force EDSM sync..
            });
        }

        private static void UpgradeUserDB102(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE Commanders (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, EdsmApiKey TEXT NOT NULL, NetLogDir TEXT, Deleted INTEGER NOT NULL)";

            SQLiteDBClass.PerformUpgrade(conn, 102, true, false, new[] { query1 });
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

            SQLiteDBClass.PerformUpgrade(conn, 103, true, false, new[] { query1, query2 });
        }

        private static void UpgradeUserDB104(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN journalid Integer NOT NULL DEFAULT 0";
            SQLiteDBClass.PerformUpgrade(conn, 104, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB105(SQLiteConnectionED conn)
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


            SQLiteDBClass.PerformUpgrade(conn, 105, true, false, new[] { query1, query2, query3 });
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
            };

            foreach (string query in queries)
            {
                using (DbCommand cmd = conn.CreateCommand(query))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void CreateUserDBTableIndexes()
        {
            string[] queries = new[]
            {
                "CREATE INDEX IF NOT EXISTS VisitedSystemIndex ON VisitedSystems (Name ASC, Time ASC)",
                "CREATE INDEX IF NOT EXISTS VisitedSystems_id_edsm_assigned ON VisitedSystems (id_edsm_assigned)",
                "CREATE INDEX IF NOT EXISTS VisitedSystems_position ON VisitedSystems (X, Y, Z)",
                "CREATE INDEX IF NOT EXISTS TravelLogUnit_Name ON TravelLogUnit (Name)",
                "CREATE INDEX IF NOT EXISTS TravelLogUnit_Commander ON TravelLogUnit(CommanderId)",
                "CREATE INDEX IF NOT EXISTS JournalEntry_TravelLogId ON JournalEntries (TravelLogId)",
                "CREATE INDEX IF NOT EXISTS JournalEntry_EventTypeId ON JournalEntries (EventTypeId)",
                "CREATE INDEX IF NOT EXISTS JournalEntry_EventType ON JournalEntries (EventType)",
                "CREATE INDEX IF NOT EXISTS JournalEntry_EventTime ON JournalEntries (EventTime)",
            };
            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
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

        public static void TranferVisitedSystemstoJournalTableIfRequired()
        {
            if (SQLiteDBClass.GetSettingBool("ImportVisitedSystems", false) == false )
            {
                TranferVisitedSystemstoJournalTable();
                SQLiteDBClass.PutSettingBool("ImportVisitedSystems", true);
            }
        }

        public static void TranferVisitedSystemstoJournalTable()        // DONE purposely without using any VisitedSystem code.. so we can blow it away later.
        {
            List<Object[]> ehl = new List<Object[]>();

            List<EDDiscovery2.DB.TravelLogUnit> tlus = EDDiscovery2.DB.TravelLogUnit.GetAll();

            using (SQLiteConnectionUser conn = new SQLiteConnectionUser())
            {
                                                               // 0    1    2    3         4         5          6 7 8 9
                using (DbCommand cmd = conn.CreateCommand("Select Name,Time,Unit,Commander,edsm_sync,Map_colour,X,Y,Z,id_edsm_assigned From VisitedSystems Order By Time"))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Object[] array = new Object[16];
                            reader.GetValues(array);                    // love this call.

                            string tluname = (string)array[2];          // 2 is in terms of its name.. look it up
                            EDDiscovery2.DB.TravelLogUnit tlu = tlus.Find(x => x.Name.Equals(tluname, StringComparison.InvariantCultureIgnoreCase));

                            if (tlu != null)                            // found it, assign to slot 15 the id.
                                array[15] = (long)tlu.id;
                            else
                            {
                                array[15] = (long)0;
                                Console.WriteLine("Entry with tluname {0} not found in TLU list", tluname);
                            }

                            ehl.Add(array);
                        }
                    }
                }

                using (DbTransaction txn = conn.BeginTransaction())
                {
                    foreach (Object[] array in ehl)
                    {
                        using (DbCommand cmd = conn.CreateCommand(
                            "Insert into JournalEntries (TravelLogId,CommanderId,EventTypeId,EventType,EventTime,EventData,EdsmId,Synced) " +
                            "values (@tli,@cid,@eti,@et,@etime,@edata,@edsmid,@synced)", txn))
                        {
                            cmd.AddParameterWithValue("@tli", (long)array[15]);
                            cmd.AddParameterWithValue("@cid", (long)array[3]);
                            cmd.AddParameterWithValue("@eti", EDDiscovery.EliteDangerous.JournalTypeEnum.FSDJump);
                            cmd.AddParameterWithValue("@et", "FSDJump");
                            cmd.AddParameterWithValue("@etime", (DateTime)array[1]);

                            JObject je = new JObject();

                            je["timestamp"] = ((DateTime)array[1]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
                            je["event"] = "FSDJump";
                            je["StarSystem"] = ((string)array[0]);

                            if (System.DBNull.Value != array[6] && System.DBNull.Value != array[7] && System.DBNull.Value != array[8])
                            {
                                je["StarPos"] = new JArray() {array[6], array[7], array[8] };
                            }

                            je["EDDMapColor"] = ((long)array[5]);
                            cmd.AddParameterWithValue("@edata", je.ToString());    // order number - look at the dbcommand above

                            long edsmid = 0;
                            if (System.DBNull.Value != array[9])
                                edsmid = (long)array[9];

                            cmd.AddParameterWithValue("@edsmid", edsmid);    // order number - look at the dbcommand above
                            cmd.AddParameterWithValue("@synced", ((bool)array[4] == true) ? 1 : 0);

                            SQLiteDBClass.SQLNonQueryText(conn, cmd);
                        }
                    }

                    txn.Commit();
                    Console.WriteLine("Converted VS->JT");
                }
            }

        }
    }
}
