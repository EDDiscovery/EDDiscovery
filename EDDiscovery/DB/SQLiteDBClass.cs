using EDDiscovery2;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery.DB
{
    public class SQLiteDBClass
    {
        SQLiteConnection m_dbConnection;
        internal static string ConnectionString;
        string dbfile;

        public static List<SystemClass> globalSystems = new List<SystemClass>();
        public static Dictionary<string, SystemClass> dictSystems = new Dictionary<string, SystemClass>(); 
        
        public static List<DistanceClass> globalDistances = new List<DistanceClass>();
        public static Dictionary<string, DistanceClass> dictDistances = new Dictionary<string, DistanceClass>(); 

        public static Dictionary<string, SystemNoteClass> globalSystemNotes = new Dictionary<string, SystemNoteClass>();


        public SQLiteDBClass()
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


        private string GetSQLiteDBFile()
        {
            try
            {
                string datapath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\EDDiscovery";


                if (!Directory.Exists(datapath))
                    Directory.CreateDirectory(datapath);


                return datapath + "\\EDDiscovery.sqlite";
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "GetSQLiteDBFile Exception", System.Windows.Forms.MessageBoxButtons.OK);
                return null;
            }
        }



        public bool Connect2DB()
        {
            

            m_dbConnection = new SQLiteConnection(ConnectionString);
            m_dbConnection.Open();

            return true;
        }

        public bool CreateDB(string file)
        {
            try
            {
                SQLiteConnection.CreateFile(file);
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


        public bool InitDB()
        {
            string query = "CREATE TABLE Register (ID TEXT PRIMARY KEY  NOT NULL  UNIQUE , \"ValueInt\" INTEGER, \"ValueDouble\" DOUBLE, \"ValueString\" TEXT, \"ValueBlob\" BLOB)";
            ExecuteQuery(query);

            UpgradeDB();
            return true;
        }

        public bool UpgradeDB()
        {
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



                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UpgradeDB error: " + ex.Message);
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

            ExecuteQuery(query);
            ExecuteQuery(query2);
            ExecuteQuery(query3);
            ExecuteQuery(query4);
            ExecuteQuery(query5);
            ExecuteQuery(query6);
            ExecuteQuery(query7);
            ExecuteQuery(query8);

            PutSettingInt("DBVer", 2);

            return true;
        }


        

        private bool UpgradeDB3()
        {
            string query1 = "ALTER TABLE Systems ADD COLUMN Note TEXT";

            ExecuteQuery(query1);
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
                ExecuteQuery(query1);
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
                ExecuteQuery(query1);
                //ExecuteQuery(query2);
                ExecuteQuery(query3);
                ExecuteQuery(query4);
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
                ExecuteQuery(query1);
                ExecuteQuery(query2);
                //ExecuteQuery(query3);
                ExecuteQuery(query4);
                ExecuteQuery(query5);
                ExecuteQuery(query6);
                ExecuteQuery(query7);
                ExecuteQuery(query8);
                ExecuteQuery(query9);
                ExecuteQuery(query10);
                ExecuteQuery(query11);
                ExecuteQuery(query12);
                ExecuteQuery(query13);
                ExecuteQuery(query14);
                ExecuteQuery(query15);
                ExecuteQuery(query16);
                ExecuteQuery(query17);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                MessageBox.Show("UpgradeDB4 error: " + ex.Message);
            }

            PutSettingInt("DBVer", 6);

            return true;
        }





        private void ExecuteQuery(string query)
        {
            if (Connect2DB())
            {
                SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
                command.ExecuteNonQuery();


            }
            CloseDB();
        }


        public bool CloseDB()
        {

            m_dbConnection.Close();

            return true;
        }

        public bool GetAllSystems()
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
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

                        globalSystems.Clear();
                        dictSystems.Clear();

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
            catch (Exception ex)
            {
                return false;
            }
        }




        public bool GetAllDistances()
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "select * from Distances";

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

                        globalDistances.Clear();
                        dictDistances.Clear();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DistanceClass sys = new DistanceClass(dr);


                            globalDistances.Add(sys);

                            dictDistances[sys.NameA.ToLower() + ":" + sys.NameB.ToLower()] = sys;
                            dictDistances[sys.NameB.ToLower() + ":" + sys.NameA.ToLower()] = sys;
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



        public bool GetAllSystemNotes()
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
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
            catch (Exception ex)
            {
                return false;
            }
        }



        public int QueryValueInt(string query, int defaultvalue)
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
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
            catch (Exception ex)
            {
                return defaultvalue;
            }
        }

        public Object QueryValue(string query)
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
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

        public DataSet SqlQueryText(SQLiteConnection cn, SQLiteCommand cmd)
        {

            //LogLine("SqlQueryText: " + cmd.CommandText);

            try
            {
                DataSet ds = new DataSet();
                SQLiteDataAdapter da = default(SQLiteDataAdapter);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                da = new SQLiteDataAdapter(cmd);
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


        public int SqlNonQuery(SQLiteConnection cn, SQLiteCommand cmd)
        {
            int rows = 0;

            LogLine("SqlNonQuery: " + cmd.CommandText);

            try
            {
                cn.Open();
                rows = cmd.ExecuteNonQuery();
                cn.Close();
                return rows;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SqlNonQuery Exception: " + ex.Message);
                throw;
            }


        }


        static public object SqlScalar(SQLiteConnection cn, SQLiteCommand cmd)
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


        static public int SqlNonQueryText(SQLiteConnection cn, SQLiteCommand cmd)
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
        private bool keyExists(string sKey)
        {

            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        DataSet ds = null;
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "select ID from Register WHERE ID=@key";
                        cmd.Parameters.AddWithValue("@key", sKey);

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
            catch (Exception ex)
            {
                return false;
            }

        }


        public int GetSettingInt(string key, int defaultvalue)
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "SELECT ValueInt from Register WHERE ID = @ID";
                        cmd.Parameters.AddWithValue("@ID", key);
                        object ob = SqlScalar(cn, cmd);

                        if (ob == null)
                            return defaultvalue;

                        int val = Convert.ToInt32(ob);

                        return val;
                    }
                }
            }
            catch (Exception ex)
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
                    using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Update Register set ValueInt = @ValueInt Where ID=@ID";
                            cmd.Parameters.AddWithValue("@ID", key);
                            cmd.Parameters.AddWithValue("@ValueInt", intvalue);

                            SqlNonQueryText(cn, cmd);

                            return true;
                        }
                    }
                }
                else
                {
                    using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Insert into Register (ID, ValueInt) values (@ID, @valint)";
                            cmd.Parameters.AddWithValue("@ID", key);
                            cmd.Parameters.AddWithValue("@valint", intvalue);

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
                using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "SELECT ValueString from Register WHERE ID = @ID";
                        cmd.Parameters.AddWithValue("@ID", key);
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
            catch (Exception ex)
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
                    using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Update Register set ValueString = @ValueString Where ID=@ID";
                            cmd.Parameters.AddWithValue("@ID", key);
                            cmd.Parameters.AddWithValue("@ValueString", strvalue);

                            SqlNonQueryText(cn, cmd);

                            return true;
                        }
                    }
                }
                else
                {
                    using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 30;
                            cmd.CommandText = "Insert into Register (ID, ValueString) values (@ID, @valint)";
                            cmd.Parameters.AddWithValue("@ID", key);
                            cmd.Parameters.AddWithValue("@valint", strvalue);

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




    }
}
