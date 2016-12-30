using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.DB
{
    public class SQLiteConnectionUser : SQLiteConnectionED<SQLiteConnectionUser>
    {
        protected static List<EDDiscovery2.EDCommander> EarlyCommanders;

        public SQLiteConnectionUser() : base(EDDSqlDbSelection.EDDUser)
        {
        }

        public SQLiteConnectionUser(bool utc = true, EDDbAccessMode mode = EDDbAccessMode.Indeterminate) : base(EDDSqlDbSelection.EDDUser, utctimeindicator: utc)
        {
        }

        protected SQLiteConnectionUser(bool initializing, bool utc, EDDbAccessMode mode = EDDbAccessMode.Indeterminate) : base(EDDSqlDbSelection.EDDUser, utctimeindicator: utc, initializing: initializing)
        {
        }

        public static void Initialize()
        {
            InitializeIfNeeded(() =>
            {
                string dbv4file = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDiscovery);
                string dbuserfile = SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDUser);

                if (File.Exists(dbv4file) && !File.Exists(dbuserfile))
                {
                    File.Copy(dbv4file, dbuserfile);
                }

                using (SQLiteConnectionUser conn = new SQLiteConnectionUser(true, true, EDDbAccessMode.Writer))
                {
                    UpgradeUserDB(conn);
                }
            });
        }

        protected static bool UpgradeUserDB(SQLiteConnectionUser conn)
        {
            int dbver;
            try
            {
                ExecuteQuery(conn, "CREATE TABLE IF NOT EXISTS Register (ID TEXT PRIMARY KEY NOT NULL, ValueInt INTEGER, ValueDouble DOUBLE, ValueString TEXT, ValueBlob BLOB)");
                dbver = conn.GetSettingIntCN("DBVer", 1);        // use the constring one, as don't want to go back into ConnectionString code

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

                CreateUserDBTableIndexes(conn);

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

            PerformUpgrade(conn, 2, false, false, new[] { query4 });
        }

        private static void UpgradeUserDB4(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN Note TEXT";
            PerformUpgrade(conn, 4, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB7(SQLiteConnectionED conn)
        {
            string query3 = "CREATE TABLE TravelLogUnit(id INTEGER PRIMARY KEY  NOT NULL, type INTEGER NOT NULL, name TEXT NOT NULL, size INTEGER, path TEXT)";
            PerformUpgrade(conn, 7, true, false, new[] { query3 });
        }

        private static void UpgradeUserDB9(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE Objects (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , SystemName TEXT NOT NULL , ObjectName TEXT NOT NULL , ObjectType INTEGER NOT NULL , ArrivalPoint Float, Gravity FLOAT, Atmosphere Integer, Vulcanism Integer, Terrain INTEGER, Carbon BOOL, Iron BOOL, Nickel BOOL, Phosphorus BOOL, Sulphur BOOL, Arsenic BOOL, Chromium BOOL, Germanium BOOL, Manganese BOOL, Selenium BOOL NOT NULL , Vanadium BOOL, Zinc BOOL, Zirconium BOOL, Cadmium BOOL, Mercury BOOL, Molybdenum BOOL, Niobium BOOL, Tin BOOL, Tungsten BOOL, Antimony BOOL, Polonium BOOL, Ruthenium BOOL, Technetium BOOL, Tellurium BOOL, Yttrium BOOL, Commander  Text, UpdateTime DATETIME, Status INTEGER )";
            PerformUpgrade(conn, 9, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB10(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE wanted_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, systemname TEXT UNIQUE NOT NULL)";
            PerformUpgrade(conn, 10, true, false, new[] { query1 });
        }


        private static void UpgradeUserDB11(SQLiteConnectionED conn)
        {
            string query2 = "ALTER TABLE Objects ADD COLUMN Landed BOOL";
            string query3 = "ALTER TABLE Objects ADD COLUMN terraform Integer";
            PerformUpgrade(conn, 11, true, false, new[] { query2, query3 });
        }

        private static void UpgradeUserDB12(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE routes_expeditions (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT UNIQUE NOT NULL, start DATETIME, end DATETIME)";
            string query2 = "CREATE TABLE route_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, routeid INTEGER NOT NULL, systemname TEXT NOT NULL)";
            PerformUpgrade(conn, 12, true, false, new[] { query1, query2 });
        }


        private static void UpgradeUserDB16(SQLiteConnectionED conn)
        {
            string query = "CREATE TABLE Bookmarks (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , StarName TEXT, x double NOT NULL, y double NOT NULL, z double NOT NULL, Time DATETIME NOT NULL, Heading TEXT, Note TEXT NOT Null )";
            PerformUpgrade(conn, 16, true, false, new[] { query });
        }

        private static void UpgradeUserDB101(SQLiteConnectionED conn)
        {
            string query1 = "DROP TABLE IF EXISTS Systems";
            string query2 = "DROP TABLE IF EXISTS SystemAliases";
            string query3 = "DROP TABLE IF EXISTS Distances";
            string query4 = "VACUUM";

            PerformUpgrade(conn, 101, true, false, new[] { query1, query2, query3, query4 }, () =>
            {
                //                PutSettingString("EDSMLastSystems", "2010 - 01 - 01 00:00:00", conn);        // force EDSM sync..
            });
        }

        private static void UpgradeUserDB102(SQLiteConnectionED conn)
        {
            string query1 = "CREATE TABLE Commanders (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, EdsmApiKey TEXT NOT NULL, NetLogDir TEXT, Deleted INTEGER NOT NULL)";

            PerformUpgrade(conn, 102, true, false, new[] { query1 });
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

            PerformUpgrade(conn, 103, true, false, new[] { query1, query2 });
        }

        private static void UpgradeUserDB104(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN journalid Integer NOT NULL DEFAULT 0";
            PerformUpgrade(conn, 104, true, false, new[] { query1 });
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


            PerformUpgrade(conn, 105, true, false, new[] { query1, query2, query3 });
        }

        private static void UpgradeUserDB106(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN EdsmId INTEGER NOT NULL DEFAULT -1";
            PerformUpgrade(conn, 106, true, false, new[] { query1 });
        }


        private static void UpgradeUserDB107(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN SyncToEdsm INTEGER NOT NULL DEFAULT 1";
            string query2 = "ALTER TABLE Commanders ADD COLUMN SyncFromEdsm INTEGER NOT NULL DEFAULT 0";
            string query3 = "ALTER TABLE Commanders ADD COLUMN SyncToEddn INTEGER NOT NULL DEFAULT 1";
            PerformUpgrade(conn, 107, true, false, new[] { query1, query2, query3});
        }

        private static void UpgradeUserDB108(SQLiteConnectionED conn)
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN JournalDir TEXT";
            PerformUpgrade(conn, 108, true, false, new[] { query1 }, () =>
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

            PerformUpgrade(conn, 109, true, false, new[] { query1 });
        }

        private static void UpgradeUserDB110(SQLiteConnectionUser conn)
        {
            string query1 = "ALTER TABLE Commanders ADD COLUMN EdsmName TEXT";
            string query2 = "ALTER TABLE MaterialsCommodities ADD COLUMN ShortName TEXT NOT NULL COLLATE NOCASE DEFAULT ''";
            PerformUpgrade(conn, 110, true, false, new[] { query1, query2 });
        }

        private static void UpgradeUserDB111(SQLiteConnectionUser conn)
        {
            string query1 = "ALTER TABLE MaterialsCommodities ADD COLUMN Flags INT NOT NULL DEFAULT 0";     // flags
            string query2 = "ALTER TABLE MaterialsCommodities ADD COLUMN Colour INT NOT NULL DEFAULT 15728640";     // ARGB
            string query3 = "ALTER TABLE MaterialsCommodities ADD COLUMN FDName TEXT NOT NULL COLLATE NOCASE DEFAULT ''";
            PerformUpgrade(conn, 111, true, false, new[] { query1, query2, query3 });
        }

        private static void UpgradeUserDB112(SQLiteConnectionUser conn)
        {
            string query1 = "DELETE FROM MaterialsCommodities";     // To fix materialcompatibility wuth wrong tables in 5.0.x
            PerformUpgrade(conn, 112, true, false, new[] { query1 });
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

        public List<EDDiscovery2.EDCommander> GetCommanders()
        {
            List<EDDiscovery2.EDCommander> commanders = new List<EDDiscovery2.EDCommander>();

            if (GetSettingInt("DBVer", 1) >= 102)
            {
                using (DbCommand cmd = CreateCommand("SELECT * FROM Commanders"))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EDDiscovery2.EDCommander edcmdr = new EDDiscovery2.EDCommander(reader);

                            string name = Convert.ToString(reader["Name"]);
                            string edsmapikey = Convert.ToString(reader["EdsmApiKey"]);

                            commanders.Add(edcmdr);
                        }
                    }
                }
            }

            return commanders;
        }

        public static List<EDDiscovery2.EDCommander> GetCommanders(SQLiteConnectionUser conn = null)
        {
            if (File.Exists(GetSQLiteDBFile(EDDSqlDbSelection.EDDUser)))
            {
                bool closeconn = false;

                try
                {
                    if (conn == null)
                    {
                        closeconn = true;
                        conn = new SQLiteConnectionUser(true, true, EDDbAccessMode.Reader);
                    }

                    return conn.GetCommanders();
                }
                finally
                {
                    if (closeconn && conn != null)
                    {
                        conn.Dispose();
                    }
                }
            }
            else
            {
                return new List<EDDiscovery2.EDCommander>();
            }
        }

        public static new List<EDDiscovery2.EDCommander> GetCommandersFromRegister(SQLiteConnectionUser conn = null)
        {
            if (File.Exists(GetSQLiteDBFile(EDDSqlDbSelection.EDDUser)))
            {
                bool closeconn = false;

                try
                {
                    if (conn == null)
                    {
                        closeconn = true;
                        conn = new SQLiteConnectionUser(true, true, EDDbAccessMode.Reader);
                    }
                    return SQLiteConnectionED<SQLiteConnectionUser>.GetCommandersFromRegister(conn);
                }
                finally
                {
                    if (closeconn && conn != null)
                    {
                        conn.Dispose();
                    }
                }
            }
            else if (File.Exists(GetSQLiteDBFile(EDDSqlDbSelection.EDDiscovery)))
            {
                return SQLiteConnectionOld.GetCommandersFromRegister(null);
            }
            else
            {
                return new List<EDDiscovery2.EDCommander>();
            }
        }

        public static void TranferVisitedSystemstoJournalTableIfRequired()
        {
            if (System.IO.File.Exists(SQLiteConnectionED.GetSQLiteDBFile(EDDSqlDbSelection.EDDiscovery)))
            {
                if (SQLiteDBClass.GetSettingBool("ImportVisitedSystems", false) == false)
                {
                    TranferVisitedSystemstoJournalTable();
                    SQLiteDBClass.PutSettingBool("ImportVisitedSystems", true);
                }
            }
        }

        public static void TranferVisitedSystemstoJournalTable()        // DONE purposely without using any VisitedSystem code.. so we can blow it away later.
        {
            List<Object[]> ehl = new List<Object[]>();
            Dictionary<string, Dictionary<string, double>> dists = new Dictionary<string, Dictionary<string, double>>(StringComparer.CurrentCultureIgnoreCase);

            List<EDDiscovery2.DB.TravelLogUnit> tlus = EDDiscovery2.DB.TravelLogUnit.GetAll().Where(t => t.type == 1).ToList();

            using (SQLiteConnectionOld conn = new SQLiteConnectionOld())
            {
                //                                                0      1      2
                using (DbCommand cmd = conn.CreateCommand("SELECT NameA, NameB, Dist FROM Distances WHERE Status >= 1"))    // any distance pairs okay
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object[] vals = new object[3];
                            reader.GetValues(vals);

                            string namea = (string)vals[0];
                            string nameb = (string)vals[1];
                            double dist = (double)vals[2];

                            if (!dists.ContainsKey(namea))
                            {
                                dists[namea] = new Dictionary<string, double>(StringComparer.CurrentCultureIgnoreCase);
                            }

                            dists[namea][nameb] = dist;

                            if (!dists.ContainsKey(nameb))
                            {
                                dists[nameb] = new Dictionary<string, double>(StringComparer.CurrentCultureIgnoreCase);
                            }

                            dists[nameb][namea] = dist;
                        }
                    }
                }

                int olddbver = SQLiteConnectionOld.GetSettingInt("DBVer", 1);

                if (olddbver < 7) // 2.5.2
                {
                    System.Diagnostics.Trace.WriteLine("Database too old - unable to migrate travel log");
                    return;
                }

                string query;

                if (olddbver < 8) // 2.5.6
                {
                    query = "Select Name,Time,Unit,Commander,edsm_sync, -65536 AS Map_colour, NULL AS X, NULL AS Y, NULL AS Z, NULL as id_edsm_assigned From VisitedSystems Order By Time";
                }
                else if (olddbver < 14) // 3.2.1
                {
                    query = "Select Name,Time,Unit,Commander,edsm_sync,Map_colour, NULL AS X, NULL AS Y, NULL AS Z, NULL as id_edsm_assigned From VisitedSystems Order By Time";
                }
                else if (olddbver < 18) // 4.0.2
                {
                    query = "Select Name,Time,Unit,Commander,edsm_sync,Map_colour,X,Y,Z, NULL AS id_edsm_assigned From VisitedSystems Order By Time";
                }
                else
                {
                    //              0    1    2    3         4         5          6 7 8 9
                    query = "Select Name,Time,Unit,Commander,edsm_sync,Map_colour,X,Y,Z,id_edsm_assigned From VisitedSystems Order By Time";
                }

                using (DbCommand cmd = conn.CreateCommand(query))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        string prev = "";

                        while (reader.Read())
                        {
                            Object[] array = new Object[17];
                            reader.GetValues(array);                    // love this call.

                            string tluname = (string)array[2];          // 2 is in terms of its name.. look it up

                            if (tluname.StartsWith("EDSM-")) // Don't migrate the entries that were synced from EDSM
                            {                                // We can sync them from EDSM later.
                                continue;
                            }

                            EDDiscovery2.DB.TravelLogUnit tlu = tlus.Find(x => x.Name.Equals(tluname, StringComparison.InvariantCultureIgnoreCase));

                            array[15] = (tlu != null) ? (long)tlu.id : 0;      // even if we don't find it, tlu may be screwed up, still want to import

                            array[16] = null;
                            if (prev.Length>0 && dists.ContainsKey((string)array[0]))
                            {
                                Dictionary<string, double> _dists = dists[(string)array[0]];
                                if (_dists.ContainsKey(prev))
                                {
                                    array[16] = _dists[prev];
                                }
                            }

                            ehl.Add(array);
                            prev = (string)array[0];
                        }
                    }
                }
            }

            using (SQLiteConnectionUser conn = new SQLiteConnectionUser(utc: true))
            {
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

                            JObject je = new JObject();
                            DateTime eventtime = DateTime.SpecifyKind((DateTime)array[1], DateTimeKind.Local).ToUniversalTime();

                            je["timestamp"] = eventtime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
                            je["event"] = "FSDJump";
                            je["StarSystem"] = ((string)array[0]);

                            if (System.DBNull.Value != array[6] && System.DBNull.Value != array[7] && System.DBNull.Value != array[8])
                            {
                                je["StarPos"] = new JArray() {array[6], array[7], array[8] };
                            }

                            if (array[16] != null)
                            {
                                je["JumpDist"] = (double)array[16];
                            }

                            je["EDDMapColor"] = ((long)array[5]);
                            cmd.AddParameterWithValue("@etime", eventtime);
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
                }
            }

        }

        public static Dictionary<string, RegisterEntry> EarlyGetRegister()
        {
            Dictionary<string, RegisterEntry> reg = new Dictionary<string, RegisterEntry>();

            if (File.Exists(GetSQLiteDBFile(EDDSqlDbSelection.EDDUser)))
            {
                using (SQLiteConnectionUser conn = new SQLiteConnectionUser(true, true, EDDbAccessMode.Reader))
                {
                    conn.GetRegister(reg);
                }
            }
            else
            {
                reg = SQLiteConnectionOld.EarlyGetRegister();
            }

            return reg;
        }

        public static void EarlyReadRegister()
        {
            EarlyRegister = EarlyGetRegister();
        }
    }
}
