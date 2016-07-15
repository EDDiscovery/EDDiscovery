using EDDiscovery2;
using EDDiscovery2.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                bool ret = Add(cn);
                return ret;
            }
        }

        private bool Add(SQLiteConnectionED cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into wanted_systems (systemname) values (@systemname)"))
            {
                cmd.AddParameterWithValue("@systemname", system);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from wanted_systems"))
                {
                    id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                }
                return true;
            }
        }

        public bool Delete()
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnectionED cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM wanted_systems WHERE id = @id"))
            {
                cmd.AddParameterWithValue("@id", id);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);

                return true;
            }
        }

        public static List<WantedSystemClass> GetAllWantedSystems()
        {
            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select * from wanted_systems"))
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
