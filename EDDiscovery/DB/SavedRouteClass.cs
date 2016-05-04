using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace EDDiscovery.DB
{
    public class SavedRouteClass
    {
        public SavedRouteClass()
        {
            this.Id = -1;
            this.Systems = new List<string>();
        }

        public SavedRouteClass(string name, params string[] systems)
        {
            this.Id = -1;
            this.Name = name;
            this.Systems = systems.ToList();
        }

        public SavedRouteClass(DataRow dr, DataRow[] syslist)
        {
            this.Id = (int)(long)dr["id"];
            this.Name = (string)dr["name"];
            if (dr["start"] != DBNull.Value)
                this.StartDate = (DateTime?)dr["start"];
            if (dr["end"] != DBNull.Value)
                this.EndDate = (DateTime?)dr["end"];
            this.Systems = syslist.Select(s => (string)s["systemname"]).ToList();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string> Systems { get; private set; }

        public bool Equals(SavedRouteClass other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.Name == other.Name || (String.IsNullOrEmpty(this.Name) && String.IsNullOrEmpty(other.Name))) &&
                   this.StartDate == other.StartDate &&
                   this.EndDate == other.EndDate &&
                   this.Systems.Count == other.Systems.Count &&
                   this.Systems.Zip(other.Systems, (t, o) => t == o).All(v => v);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SavedRouteClass)
            {
                return this.Equals((SavedRouteClass)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
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
                cmd.CommandText = "Insert into routes_expeditions (name, start, end) values (@name, @start, @end)";
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@start", StartDate);
                cmd.AddParameterWithValue("@end", EndDate);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "Select Max(id) as id from routes_expeditions";

                    Id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                }

                using (DbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)";
                    cmd2.AddParameter("@routeid", DbType.String);
                    cmd2.AddParameter("@name", DbType.String);

                    foreach (var sysname in Systems)
                    {
                        cmd2.Parameters["@routeid"].Value = Id;
                        cmd2.Parameters["@name"].Value = sysname;
                        SQLiteDBClass.SqlNonQueryText(cn, cmd2);
                    }
                }

                return true;
            }
        }

        public bool Update()
        {
            var db = new SQLiteDBClass();
            using (DbConnection cn = db.CreateConnection())
            {
                return Update(cn);
            }
        }

        private bool Update(DbConnection cn)
        {
            using (DbCommand cmd = cn.CreateCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "UPDATE routes_expeditions SET name=@name, start=@start, end=@end WHERE id=@id";
                cmd.AddParameterWithValue("@id", Id);
                cmd.AddParameterWithValue("@name", Name);
                cmd.AddParameterWithValue("@start", StartDate);
                cmd.AddParameterWithValue("@end", EndDate);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);

                using (DbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "DELETE FROM route_systems WHERE routeid=@routeid";
                    cmd2.AddParameterWithValue("@routeid", Id);
                    SQLiteDBClass.SqlNonQueryText(cn, cmd2);
                }

                using (DbCommand cmd2 = cn.CreateCommand())
                {
                    cmd2.Connection = cn;
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandTimeout = 30;
                    cmd2.CommandText = "INSERT INTO route_systems (routeid, systemname) VALUES (@routeid, @name)";
                    cmd2.AddParameter("@routeid", DbType.String);
                    cmd2.AddParameter("@name", DbType.String);

                    foreach (var sysname in Systems)
                    {
                        cmd2.Parameters["@routeid"].Value = Id;
                        cmd2.Parameters["@name"].Value = sysname;
                        SQLiteDBClass.SqlNonQueryText(cn, cmd2);
                    }
                }

                return true;
            }
        }
    }
}
