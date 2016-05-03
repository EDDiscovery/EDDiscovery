using EDDiscovery2;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace EDDiscovery.DB
{
    public class SQLiteDBClass
    {
        private static Func<string, IDbConnection> _createConnection;
        private static Func<IDbCommand, IDbDataAdapter> _createDataAdapter;

        internal static string ConnectionString;
        string dbfile;

        public static List<SystemClass> globalSystems = new List<SystemClass>();
        public static Dictionary<string, SystemClass> dictSystems = new Dictionary<string, SystemClass>(); 
        
        public static Dictionary<string, double> dictDistances = new Dictionary<string, double>(); 

        public static Dictionary<string, SystemNoteClass> globalSystemNotes = new Dictionary<string, SystemNoteClass>();

        private static Object lockDBInit = new Object();
        private static bool dbUpgraded = false;

        static SQLiteDBClass()
        {
            InitSqliteConstructors();
        }

        public SQLiteDBClass()
        {
            lock (lockDBInit)
            {
                dbfile = GetSQLiteDBFile();

                ConnectionString = "Data Source=" + dbfile + ";Pooling=true;";

                if (!File.Exists(dbfile))
                {
                    CreateDB(dbfile);
                }
                else
                    UpgradeDB();

            }
        }


        private string GetSQLiteDBFile()
        {
            try
            {
                string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EDDiscovery");


                if (!Directory.Exists(datapath))
                    Directory.CreateDirectory(datapath);


                return Path.Combine(datapath, "EDDiscovery.sqlite");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "GetSQLiteDBFile Exception", System.Windows.Forms.MessageBoxButtons.OK);
                return null;
            }
        }



        public bool CreateDB(string file)
        {
            try
            {
                //SQLiteConnection.CreateFile(file);
                InitDB();
                UpgradeDB();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "GetSQLiteDBFile Exception", System.Windows.Forms.MessageBoxButtons.OK);
                return false;
            }

            
        }

        #region DB Initialization
        public bool InitDB()
        {
            string query = "CREATE TABLE Register (ID TEXT PRIMARY KEY  NOT NULL  UNIQUE , \"ValueInt\" INTEGER, \"ValueDouble\" DOUBLE, \"ValueString\" TEXT, \"ValueBlob\" BLOB)";
            ExecuteNonQuery(query);

            UpgradeDB();
            return true;
        }

        public bool UpgradeDB()
        {
            if (dbUpgraded)
                return true;

            int dbver;
            try
            {

                dbver = GetSettingInt("DBVer", 1);

                if (dbver < 2)
                    UpgradeDB2();

                if (dbver < 3)
                    UpgradeDB3();


                if (dbver < 4)
                    UpgradeDB4();

                if (dbver < 5)
                    UpgradeDB5();

                if (dbver < 6)
                    UpgradeDB6();

                if (dbver < 7)
                    UpgradeDB7();

                if (dbver < 8)
                    UpgradeDB8();

                if (dbver < 9)
                    UpgradeDB9();

                if (dbver < 10)
                    UpgradeDB10();

                if (dbver < 11)
                    UpgradeDB11();

                if (dbver < 12)
                    UpgradeDB12();

                dbUpgraded = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UpgradeDB error: " + ex.Message);
                MessageBox.Show(ex.StackTrace);
                return false;
            }

        }


        private bool UpgradeDB2()
        {
            string query = "CREATE TABLE Systems (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , name TEXT NOT NULL COLLATE NOCASE , x FLOAT, y FLOAT, z FLOAT, cr INTEGER, commandercreate TEXT, createdate DATETIME, commanderupdate TEXT, updatedate DATETIME, status INTEGER, population INTEGER )";
            string query2 = "CREATE  INDEX main.SystemsIndex ON Systems (name ASC)";
            string query3 = "CREATE TABLE Distances (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL  UNIQUE , NameA TEXT NOT NULL , NameB TEXT NOT NULL , Dist FLOAT NOT NULL , CommanderCreate TEXT NOT NULL , CreateTime DATETIME NOT NULL , Status INTEGER NOT NULL )";
            string query4 = "CREATE TABLE SystemNote (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Time DATETIME NOT NULL )";
            string query5= "CREATE INDEX DistanceName ON Distances (NameA ASC, NameB ASC)";
            string query6 = "CREATE  TABLE VisitedSystems (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Time DATETIME NOT NULL , SystemID INTEGER, Dist FLOAT)";
            string query7 = "CREATE TABLE Stations (station_id INTEGER PRIMARY KEY  NOT NULL ,system_id INTEGER REFERENCES Systems(id), name TEXT NOT NULL ,blackmarket BOOL DEFAULT (null) ,max_landing_pad_size INTEGER,distance_to_star INTEGER,type TEXT,faction TEXT,shipyard BOOL,outfitting BOOL, commodities_market BOOL)";
            string query8 = "CREATE  INDEX stationIndex ON Stations (system_id ASC)";

            ExecuteNonQuery(query);
            ExecuteNonQuery(query2);
            ExecuteNonQuery(query3);
            ExecuteNonQuery(query4);
            ExecuteNonQuery(query5);
            ExecuteNonQuery(query6);
            ExecuteNonQuery(query7);
            ExecuteNonQuery(query8);

            PutSettingInt("DBVer", 2);

            return true;
        }


        

        private bool UpgradeDB3()
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN Note TEXT";

            ExecuteNonQuery(query1);
            PutSettingInt("DBVer", 3);

            return true;
        }

        private bool UpgradeDB4()
        {
            string query1 = "ALTER TABLE SystemNote ADD COLUMN Note TEXT";
            string dbfile = GetSQLiteDBFile();

            try
            {
                File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", "EDDiscovery3.sqlite"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }


            try
            {
                ExecuteNonQuery(query1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB4 error: " + ex.Message);
            }

            GetAllSystems();


            foreach (SystemClass sys in globalSystems)
            {
                if (sys.Note != null && sys.Note.Length > 0)
                {
                    SystemNoteClass note = new SystemNoteClass();

                    note.Name = sys.name;
                    note.Note = sys.Note;
                    note.Time = DateTime.Now;
                    note.Add();
                }
            }

            
            PutSettingInt("DBVer", 4);

            return true;
        }


        private bool UpgradeDB5()
        {
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN Unit TEXT";
            string query3 = "ALTER TABLE VisitedSystems ADD COLUMN Commander Integer";
            string query4 = "CREATE INDEX VisitedSystemIndex ON VisitedSystems (Name ASC, Time ASC)";
            string dbfile = GetSQLiteDBFile();

            try
            {
                File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", "EDDiscovery4.sqlite"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }


            try
            {
                ExecuteNonQuery(query1);
                //ExecuteNonQuery(query2);
                ExecuteNonQuery(query3);
                ExecuteNonQuery(query4);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB4 error: " + ex.Message);
            }

            PutSettingInt("DBVer", 5);

            return true;
        }


        private bool UpgradeDB6()
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

            string query11 = "DROP TABLE Stations";
            string query12 = "CREATE TABLE Stations (id INTEGER PRIMARY KEY  NOT NULL ,system_id INTEGER, name TEXT NOT NULL ,  " +
                " max_landing_pad_size INTEGER, distance_to_star INTEGER, faction Text, government_id INTEGER, allegiance_id Integer,  state_id INTEGER, type_id Integer, " +
                "has_commodities BOOL DEFAULT (null), has_refuel BOOL DEFAULT (null), has_repair BOOL DEFAULT (null), has_rearm BOOL DEFAULT (null), " +
                "has_outfitting BOOL DEFAULT (null),  has_shipyard BOOL DEFAULT (null), has_blackmarket BOOL DEFAULT (null),   eddb_updated_at Integer  )";



            string query13 = "CREATE TABLE station_commodities (station_id INTEGER PRIMARY KEY NOT NULL, commodity_id INTEGER, type INTEGER)";
            string query14 = "CREATE INDEX station_commodities_index ON station_commodities (station_id ASC, commodity_id ASC, type ASC)";
            string query15 = "CREATE INDEX StationsIndex_ID  ON Stations (id ASC)";
            string query16 = "CREATE INDEX StationsIndex_system_ID  ON Stations (system_id ASC)";
            string query17 = "CREATE INDEX StationsIndex_system_Name  ON Stations (Name ASC)";

            string dbfile = GetSQLiteDBFile();

            try
            {
                File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", "EDDiscovery5.sqlite"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }


            try
            {
                ExecuteNonQuery(query1);
                ExecuteNonQuery(query2);
                //ExecuteNonQuery(query3);
                ExecuteNonQuery(query4);
                ExecuteNonQuery(query5);
                ExecuteNonQuery(query6);
                ExecuteNonQuery(query7);
                ExecuteNonQuery(query8);
                ExecuteNonQuery(query9);
                ExecuteNonQuery(query10);
                ExecuteNonQuery(query11);
                ExecuteNonQuery(query12);
                ExecuteNonQuery(query13);
                ExecuteNonQuery(query14);
                ExecuteNonQuery(query15);
                ExecuteNonQuery(query16);
                ExecuteNonQuery(query17);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB6 error: " + ex.Message);
            }

            PutSettingInt("DBVer", 6);

            return true;
        }


        private bool UpgradeDB7()
        {
            
            string query1 = "DROP TABLE VisitedSystems";
            string query2 = "CREATE TABLE VisitedSystems(id INTEGER PRIMARY KEY  NOT NULL, Name TEXT NOT NULL, Time DATETIME NOT NULL, Unit Text, Commander Integer, Source Integer, edsm_sync BOOL DEFAULT (null))";
            string query3 = "CREATE TABLE TravelLogUnit(id INTEGER PRIMARY KEY  NOT NULL, type INTEGER NOT NULL, name TEXT NOT NULL, size INTEGER, path TEXT)";


            string dbfile = GetSQLiteDBFile();

            try
            {
                File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", "EDDiscovery6.sqlite"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }


            try
            {
                ExecuteNonQuery(query1);
                ExecuteNonQuery(query2);
                ExecuteNonQuery(query3);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB7 error: " + ex.Message);
            }

            PutSettingInt("DBVer", 7);

            return true;
        }


        private bool UpgradeDB8()
        {
            //Default is Color.Red.ToARGB()
            string query1 = "ALTER TABLE VisitedSystems ADD COLUMN Map_colour INTEGER DEFAULT (-65536)";
            string dbfile = GetSQLiteDBFile();
            
            try
            {
                File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", "EDDiscovery7.sqlite"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }


            try
            {
                ExecuteNonQuery(query1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB8 error: " + ex.Message);
            }

            PutSettingInt("DBVer", 8);

            return true;
        }


        private bool UpgradeDB9()
        {
            //Default is Color.Red.ToARGB()
            string query1 = "CREATE TABLE Objects (id INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , SystemName TEXT NOT NULL , ObjectName TEXT NOT NULL , ObjectType INTEGER NOT NULL , ArrivalPoint Float, Gravity FLOAT, Atmosphere Integer, Vulcanism Integer, Terrain INTEGER, Carbon BOOL, Iron BOOL, Nickel BOOL, Phosphorus BOOL, Sulphur BOOL, Arsenic BOOL, Chromium BOOL, Germanium BOOL, Manganese BOOL, Selenium BOOL NOT NULL , Vanadium BOOL, Zinc BOOL, Zirconium BOOL, Cadmium BOOL, Mercury BOOL, Molybdenum BOOL, Niobium BOOL, Tin BOOL, Tungsten BOOL, Antimony BOOL, Polonium BOOL, Ruthenium BOOL, Technetium BOOL, Tellurium BOOL, Yttrium BOOL, Commander  Text, UpdateTime DATETIME, Status INTEGER )";
            string dbfile = GetSQLiteDBFile();

            try
            {
                File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", "EDDiscovery8.sqlite"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }


            try
            {
                ExecuteNonQuery(query1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB9 error: " + ex.Message);
            }

            PutSettingInt("DBVer", 9);

            return true;
        }


        private bool UpgradeDB10()
        {
            string query1 = "CREATE TABLE wanted_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, systemname TEXT UNIQUE NOT NULL)";
            try
            {
                File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", "EDDiscovery9.sqlite"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }

            try
            {
                ExecuteNonQuery(query1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB10 error: " + ex.Message);
            }

            PutSettingInt("DBVer", 10);

            return true;
        }


        private bool UpgradeDB11()
        {
            //Default is Color.Red.ToARGB()
            string query1 = "ALTER TABLE Systems ADD COLUMN FirstDiscovery BOOL";
            string query2 = "ALTER TABLE Objects ADD COLUMN Landed BOOL";
            string query3 = "ALTER TABLE Objects ADD COLUMN terraform Integer";
            string query4 = "ALTER TABLE VisitedSystems ADD COLUMN Status BOOL";
            string dbfile = GetSQLiteDBFile();

            try
            {
                File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", "EDDiscovery10.sqlite"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }


            try
            {
                ExecuteNonQuery(query1);
                ExecuteNonQuery(query2);
                ExecuteNonQuery(query3);
                ExecuteNonQuery(query4);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB11 error: " + ex.Message);
            }

            PutSettingInt("DBVer", 11);

            return true;
        }

        private bool UpgradeDB12()
        {
            string query1 = "CREATE TABLE routes_expeditions (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, name TEXT UNIQUE NOT NULL, start DATETIME, end DATETIME)";
            string query2 = "CREATE TABLE route_systems (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, routeid INTEGER NOT NULL, systemname TEXT NOT NULL)";
            try
            {
                File.Copy(dbfile, dbfile.Replace("EDDiscovery.sqlite", "EDDiscovery11.sqlite"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }

            try
            {
                ExecuteNonQuery(query1);
                ExecuteNonQuery(query2);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB12 error: " + ex.Message);
            }

            PutSettingInt("DBVer", 12);

            return true;
        }
        #endregion

        #region DB Access

        private static void InitSqliteConstructors()
        {
            try
            {
                System.Reflection.Assembly mono_data_sqlite = System.Reflection.Assembly.Load("Mono.Data.Sqlite, Version=4.0.0.0, Culture=neutral");
                Type sqliteconntype = mono_data_sqlite.GetType("Mono.Data.Sqlite.SqliteConnection", true);
                Type sqlitedatype = mono_data_sqlite.GetType("Mono.Data.Sqlite.SqliteDataAdapter", true);
                Type sqlitecmdtype = mono_data_sqlite.GetType("Mono.Data.Sqlite.SqliteCommand", true);
                var connconstructor = sqliteconntype.GetConstructor(new Type[] { typeof(string) });
                var daconstructor = sqlitedatype.GetConstructor(new Type[] { sqlitecmdtype });
                var connfuncparam = System.Linq.Expressions.Expression.Parameter(typeof(string));
                var connfuncexpr = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Convert(
                        System.Linq.Expressions.Expression.New(connconstructor, connfuncparam),
                        typeof(IDbConnection)
                    ),
                    connfuncparam);
                var dafuncparam = System.Linq.Expressions.Expression.Parameter(typeof(IDbCommand));
                var dafuncexpr = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Convert(
                        System.Linq.Expressions.Expression.New(daconstructor,
                            System.Linq.Expressions.Expression.Convert(dafuncparam, sqlitecmdtype)
                        ),
                        typeof(IDbDataAdapter)
                    ),
                    dafuncparam);
                _createConnection = (Func<string, IDbConnection>)connfuncexpr.Compile();
                _createDataAdapter = (Func<IDbCommand, IDbDataAdapter>)dafuncexpr.Compile();
            }
            catch
            {
                _createConnection = (c) => new System.Data.SQLite.SQLiteConnection(c);
                _createDataAdapter = (c) => new System.Data.SQLite.SQLiteDataAdapter((System.Data.SQLite.SQLiteCommand)c);
            }
        }

        public static IDbConnection CreateConnection()
        {
            return _createConnection(ConnectionString);
        }

        public static IDbCommand CreateCommand(IDbConnection conn, string query)
        {
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
            cmd.CommandText = query;
            return cmd;
        }

        public static IDbDataAdapter CreateDataAdapter(IDbCommand cmd)
        {
            return _createDataAdapter(cmd);
        }

        public static int ExecuteNonQuery(string query)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(conn, query))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataSet ExecuteQuery(string query)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(conn, query))
                {
                    DataSet ds = new DataSet();
                    IDbDataAdapter da = CreateDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;
                }
            }
        }

        public static object ExecuteScalar(string query)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = CreateCommand(conn, query))
                {
                    return cmd.ExecuteScalar();
                }
            }
        }

	public static void AddParameter(IDbCommand cmd, string name, object val)
	{
            var par = cmd.CreateParameter();
            par.ParameterName = name;
            par.Value = val;
            cmd.Parameters.Add(par);
	}

	public static void AddParameter(IDbCommand cmd, string name, DbType type)
	{
            var par = cmd.CreateParameter();
            par.ParameterName = name;
            par.DbType = type;
            cmd.Parameters.Add(par);
	}

        public static void SetParameter(IDbCommand cmd, string name, object val)
        {
            ((IDataParameter)cmd.Parameters[name]).Value = val;
        }
        #endregion
        
        public bool GetAllSystems()
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "select * from Systems Order By name";

                        ds = SqlQueryText(cn, cmd);
                        if (ds.Tables.Count == 0)
                        {
                            return false;
                        }
                        //
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return false;
                        }

                        //globalSystems.Clear();
                        //dictSystems.Clear();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SystemClass sys = new SystemClass(dr);

                            if (globalSystemNotes.ContainsKey(sys.SearchName))
                            {
                                sys.Note = globalSystemNotes[sys.SearchName].Note;
                            }
                            
                            dictSystems[sys.SearchName] = sys;
                        }

                        globalSystems = dictSystems.Values.ToList<SystemClass>();

                        return true;

                    }
                }
            }
            catch
            {
                return false;
            }
        }




        public bool GetAllDistances(bool loadAlldata)
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        if (!loadAlldata)
                            cmd.CommandText = "select NameA,NameB, Dist from Distances WHERE status='3' or status = '4'";//         EDDiscovery = 3, EDDiscoverySubmitted = 4
                        else
                            cmd.CommandText = "select NameA,NameB, Dist from Distances";

                        ds = SqlQueryText(cn, cmd);
                        if (ds.Tables.Count == 0)
                        {
                            return false;
                        }
                        //
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return false;
                        }
                        
                        dictDistances.Clear();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string NameA, NameB;
                            double dist;

                            NameA = (string)dr["NameA"];
                            NameB = (string)dr["NameB"];
                            dist = Convert.ToDouble(dr["Dist"]);

                            dictDistances[GetDistanceCacheKey(NameA, NameB)] = dist;
                        }

                        return true;

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                return false;
            }
        }


        public List<DistanceClass> GetDistancesByStatus(int status)
        {
            List<DistanceClass> ldist = new List<DistanceClass>();
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "select * from Distances WHERE status='" + status.ToString() +  "'";

                        ds = SqlQueryText(cn, cmd);
                        if (ds.Tables.Count == 0)
                        {
                            return ldist;
                        }
                        //
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return ldist;
                        }


                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DistanceClass dist = new DistanceClass(dr);
                            ldist.Add(dist);
                        }

                        return ldist;

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);

                return ldist;
            }
        }




        public bool GetAllSystemNotes()
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "select * from SystemNote";

                        ds = SqlQueryText(cn, cmd);
                        if (ds.Tables.Count == 0)
                        {
                            return false;
                        }
                        //
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return false;
                        }

                        globalSystemNotes.Clear();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SystemNoteClass sys = new SystemNoteClass(dr);
                            globalSystemNotes[sys.Name.ToLower()] = sys;
                        }

                        return true;

                    }
                }
            }
            catch 
            {
                return false;
            }
        }

        public List<WantedSystemClass> GetAllWantedSystems()
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "select * from wanted_systems";

                        ds = SqlQueryText(cn, cmd);
                        if (ds.Tables.Count == 0)
                        {
                            return null;
                        }
                        //
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return null;
                        }

                        List<WantedSystemClass> retVal = new List<WantedSystemClass>();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            WantedSystemClass sys = new WantedSystemClass(dr);
                            retVal.Add(sys);
                        }

                        return retVal;

                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public List<SavedRouteClass> GetAllSavedRoutes()
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd1 = cn.CreateCommand())
                    {
                        DataSet ds1 = null;
                        cmd1.Connection = cn;
                        cmd1.CommandType = CommandType.Text;
                        cmd1.CommandTimeout = 30;
                        cmd1.CommandText = "select * from routes_expeditions";

                        ds1 = SqlQueryText(cn, cmd1);
                        if (ds1.Tables.Count == 0)
                        {
                            return null;
                        }
                        //
                        if (ds1.Tables[0].Rows.Count == 0)
                        {
                            return new List<SavedRouteClass>();
                        }

                        using (IDbCommand cmd2 = cn.CreateCommand())
                        {
                            DataSet ds2 = null;
                            cmd2.Connection = cn;
                            cmd2.CommandType = CommandType.Text;
                            cmd2.CommandTimeout = 30;
                            cmd2.CommandText = "select * from route_systems";

                            ds2 = SqlQueryText(cn, cmd2);

                            List<SavedRouteClass> retVal = new List<SavedRouteClass>();

                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                DataRow[] syslist = new DataRow[0];
                                if (ds2.Tables.Count != 0)
                                {
                                    syslist = ds2.Tables[0].Select(String.Format("routeid = {0}", dr["id"]), "id ASC");
                                }
                                SavedRouteClass sys = new SavedRouteClass(dr, syslist);
                                retVal.Add(sys);
                            }

                            return retVal;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

        }

        public int QueryValueInt(string query, int defaultvalue)
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = query;
                        object ob = SqlScalar(cn, cmd);

                        if (ob == null)
                            return defaultvalue;

                        int val = Convert.ToInt32(ob);

                        return val;
                    }
                }
            }
            catch 
            {
                return defaultvalue;
            }
        }

        public Object QueryValue(string query)
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = query;

                        ds = SqlQueryText(cn, cmd);
                        if (ds.Tables.Count == 0)
                        {
                            return null;
                        }
                        //
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return null;
                        }


                        return ds.Tables[0].Rows[0];

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return null;
            }
        }

        static private void LogLine(string text)
        {
            System.Diagnostics.Trace.WriteLine(text);
        }

        public static DataSet SqlQueryText(IDbConnection cn, IDbCommand cmd)
        {

            //LogLine("SqlQueryText: " + cmd.CommandText);

            try
            {
                DataSet ds = new DataSet();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                IDbDataAdapter da = CreateDataAdapter(cmd);
                cn.Open();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SqlQuery Exception: " + ex.Message);
                throw;
            }

        }

        static public object SqlScalar(IDbConnection cn, IDbCommand cmd)
        {
            object ret = null;

            //LogLine("SqlScalar: " + cmd.CommandText);
            try
            {
                cn.Open();
                ret = cmd.ExecuteScalar();
                cn.Close();
                return ret;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SqlNonQuery Exception: " + ex.Message);
                throw;
            }


        }


        static public int SqlNonQueryText(IDbConnection cn, IDbCommand cmd)
        {
            int rows = 0;

            //LogLine("SqlNonQueryText: " + cmd.CommandText);

            try
            {
                if (cn.State == ConnectionState.Open)
                {
                    rows = cmd.ExecuteNonQuery();
                }
                else
                {
                    cn.Open();
                    rows = cmd.ExecuteNonQuery();
                    cn.Close();
                }
                return rows;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SqlNonQueryText Exception: " + ex.Message);
                throw;
            }


        }

// Check if a key exists
        public bool keyExists(string sKey)
        {

            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "select ID from Register WHERE ID=@key";
                        SQLiteDBClass.AddParameter(cmd, "@key", sKey);

                        ds = SqlQueryText(cn, cmd);
                        if (ds.Tables.Count == 0)
                        {
                            return false;
                        }
                        //
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return false;
                        }
                        return true;

                    }
                }
            }
            catch
            {
                return false;
            }

        }


        public int GetSettingInt(string key, int defaultvalue)
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "SELECT ValueInt from Register WHERE ID = @ID";
                        SQLiteDBClass.AddParameter(cmd, "@ID", key);
                        object ob = SqlScalar(cn, cmd);

                        if (ob == null)
                            return defaultvalue;

                        int val = Convert.ToInt32(ob);

                        return val;
                    }
                }
            }
            catch 
            {
                return defaultvalue;
            }
        }


        public bool PutSettingInt(string key, int intvalue)
        {
            try
            {
                if (keyExists(key))
                {
                    using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                    {
                        using (IDbCommand cmd = cn.CreateCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Update Register set ValueInt = @ValueInt Where ID=@ID";
                            SQLiteDBClass.AddParameter(cmd, "@ID", key);
                            SQLiteDBClass.AddParameter(cmd, "@ValueInt", intvalue);

                            SqlNonQueryText(cn, cmd);

                            return true;
                        }
                    }
                }
                else
                {
                    using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                    {
                        using (IDbCommand cmd = cn.CreateCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Insert into Register (ID, ValueInt) values (@ID, @valint)";
                            SQLiteDBClass.AddParameter(cmd, "@ID", key);
                            SQLiteDBClass.AddParameter(cmd, "@valint", intvalue);

                            SqlNonQueryText(cn, cmd);
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

		public double GetSettingDouble(string key, double defaultvalue)
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "SELECT ValueDouble from Register WHERE ID = @ID";
                        SQLiteDBClass.AddParameter(cmd, "@ID", key);
                        object ob = SqlScalar(cn, cmd);

                        if (ob == null)
                            return defaultvalue;

                        double val = Convert.ToDouble(ob);

                        return val;
                    }
                }
            }
            catch
            {
                return defaultvalue;
            }
        }


        public bool PutSettingDouble(string key, double doublevalue)
        {
            try
            {
                if (keyExists(key))
                {
                    using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                    {
                        using (IDbCommand cmd = cn.CreateCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Update Register set ValueDouble = @ValueDouble Where ID=@ID";
                            SQLiteDBClass.AddParameter(cmd, "@ID", key);
                            SQLiteDBClass.AddParameter(cmd, "@ValueDouble", doublevalue);

                            SqlNonQueryText(cn, cmd);

                            return true;
                        }
                    }
                }
                else
                {
                    using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                    {
                        using (IDbCommand cmd = cn.CreateCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Insert into Register (ID, ValueDouble) values (@ID, @valdbl)";
                            SQLiteDBClass.AddParameter(cmd, "@ID", key);
                            SQLiteDBClass.AddParameter(cmd, "@valdbl", doublevalue);

                            SqlNonQueryText(cn, cmd);
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool GetSettingBool(string key, bool defaultvalue)
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "SELECT ValueInt from Register WHERE ID = @ID";
                        SQLiteDBClass.AddParameter(cmd, "@ID", key);
                        object ob = SqlScalar(cn, cmd);

                        if (ob == null)
                            return defaultvalue;

                        int val = Convert.ToInt32(ob);

                        if (val == 0)
                            return false;
                        else
                            return true;
                       
                    }
                }
            }
            catch
            {
                return defaultvalue;
            }
        }


        public bool PutSettingBool(string key, bool boolvalue)
        {
            try
            {
                int intvalue = 0;

                if (boolvalue == true)
                    intvalue = 1;

                if (keyExists(key))
                {
                    using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                    {
                        using (IDbCommand cmd = cn.CreateCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Update Register set ValueInt = @ValueInt Where ID=@ID";
                            SQLiteDBClass.AddParameter(cmd, "@ID", key);

                            
                            SQLiteDBClass.AddParameter(cmd, "@ValueInt", intvalue);

                            SqlNonQueryText(cn, cmd);

                            return true;
                        }
                    }
                }
                else
                {
                    using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                    {
                        using (IDbCommand cmd = cn.CreateCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Insert into Register (ID, ValueInt) values (@ID, @valint)";
                            SQLiteDBClass.AddParameter(cmd, "@ID", key);
                            SQLiteDBClass.AddParameter(cmd, "@valint", intvalue);

                            SqlNonQueryText(cn, cmd);
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public string GetSettingString(string key, string defaultvalue)
        {
            try
            {
                using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (IDbCommand cmd = cn.CreateCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "SELECT ValueString from Register WHERE ID = @ID";
                        SQLiteDBClass.AddParameter(cmd, "@ID", key);
                        object ob = SqlScalar(cn, cmd);

                        if (ob == null)
                            return defaultvalue;

                        if (ob == System.DBNull.Value)
                            return defaultvalue;

                        string val = (string)ob;

                        return val;
                    }
                }
            }
            catch 
            {
                return defaultvalue;
            }
        }


        public bool PutSettingString(string key, string strvalue)
        {
            try
            {
                if (keyExists(key))
                {
                    using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                    {
                        using (IDbCommand cmd = cn.CreateCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Update Register set ValueString = @ValueString Where ID=@ID";
                            SQLiteDBClass.AddParameter(cmd, "@ID", key);
                            SQLiteDBClass.AddParameter(cmd, "@ValueString", strvalue);

                            SqlNonQueryText(cn, cmd);

                            return true;
                        }
                    }
                }
                else
                {
                    using (IDbConnection cn = SQLiteDBClass.CreateConnection())
                    {
                        using (IDbCommand cmd = cn.CreateCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Insert into Register (ID, ValueString) values (@ID, @valint)";
                            SQLiteDBClass.AddParameter(cmd, "@ID", key);
                            SQLiteDBClass.AddParameter(cmd, "@valint", strvalue);

                            SqlNonQueryText(cn, cmd);
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public int GetNetLogFileSize(string filename)
        {
            string key = "NetLogFileSize_" + Path.GetFileName(filename);

            return GetSettingInt(key, -1);
        }

        public void SaveNetLogFileSize(string filename, int size)
        {
            string key = "NetLogFileSize_" + Path.GetFileName(filename);


            PutSettingInt(key, size);
        }


        public static void AddDistanceToCache(DistanceClass distance)
        {
            dictDistances[GetDistanceCacheKey(distance.NameA, distance.NameB)] = distance.Dist;
        }

        public static string GetDistanceCacheKey(string systemA, string systemB)
        {
            var systemALower = systemA.ToLower();
            var systemBLower = systemB.ToLower();
            var cmp = string.Compare(systemALower, systemBLower, false, CultureInfo.InvariantCulture);
            return cmp < 0 ? systemALower + ":" + systemBLower : systemBLower + ":" + systemALower;
        }
    }
}
