using EDDiscovery2;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;

namespace EDDiscovery.DB
{
    public class WantedSystemClass
    {
        public long id;
        public string system;

        public WantedSystemClass()
        { }

        public WantedSystemClass(DataRow dr)
        {
            id = (long)dr["id"];
            system = (string)dr["systemname"];
        }

        public WantedSystemClass(string SystemName)
        {
            system = SystemName;
            Add();
        }

        public bool Add()
        {
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection(true))
            {
                bool ret = Add(cn);
                cn.Close();
                return ret;
            }
        }

        private bool Add(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("Insert into wanted_systems (systemname) values (@systemname)",cn))
            {
                cmd.Parameters.AddWithValue("@systemname", system);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (SQLiteCommand cmd2 = SQLiteDBClass.CreateCommand("Select Max(id) as id from wanted_systems",cn))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }
                return true;
            }
        }

        public bool Delete()
        {
            using (SQLiteConnection cn = SQLiteDBClass.CreateConnection())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("DELETE FROM wanted_systems WHERE id = @id",cn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                return true;
            }
        }

        public static List<WantedSystemClass> GetAllWantedSystems()
        {
            try
            {
                using (SQLiteConnection cn = SQLiteDBClass.CreateConnection())
                {
                    using (SQLiteCommand cmd = SQLiteDBClass.CreateCommand("select * from wanted_systems",cn))
                    {
                        DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);
                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
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
    }
}
