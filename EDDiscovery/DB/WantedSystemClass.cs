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
        public int id;
        public string system;

        public WantedSystemClass()
        { }

        public WantedSystemClass(DataRow dr)
        {
            id = (int)(long)dr["id"];
            system = (string)dr["systemname"];
        }

        public WantedSystemClass(string SystemName)
        {
            system = SystemName;
            Add();
        }

        public bool Add()
        {
            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                return Add(cn);
            }
        }

        private bool Add(DbConnection cn)
        {
            using (DbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Insert into wanted_systems (systemname) values (@systemname)";
                cmd.AddParameterWithValue("@systemname", system);
                
                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "Select Max(id) as id from wanted_systems";

                    id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                }
                return true;
            }
        }

        public bool Delete()
        {
            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                return Delete(cn);
            }
        }

        private bool Delete(DbConnection cn)
        {
            using (DbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "DELETE FROM wanted_systems WHERE id = @id";
                cmd.AddParameterWithValue("@id", id);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                return true;
            }
        }
    }
}
